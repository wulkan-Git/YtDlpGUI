using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace YtDlpGUI
{
    public partial class MainForm : Form
    {
        private const string YtDlpNightlyUrl = "https://github.com/yt-dlp/yt-dlp-nightly-builds/releases/latest/download/yt-dlp.exe";
        private const string DenoLatestUrl = "https://github.com/denoland/deno/releases/latest/download/deno-x86_64-pc-windows-msvc.zip";
        private const string FfmpegLatestUrl = "https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip";

        // Dark-blue palette inspired by my.ws-soft.ru: deep navy surfaces + bright electric-blue accents.
        private readonly Color _brandBlue = Color.FromArgb(0, 145, 255);
        private readonly Color _brandBlueBright = Color.FromArgb(42, 173, 255);
        private readonly Color _brandBlueDark = Color.FromArgb(8, 83, 166);
        private readonly Color _pageBackground = Color.FromArgb(4, 18, 34);
        private readonly Color _panelBackground = Color.FromArgb(10, 34, 61);
        private readonly Color _inputBackground = Color.FromArgb(13, 44, 76);
        private readonly Color _logBackground = Color.FromArgb(4, 17, 31);
        private readonly Color _textColor = Color.FromArgb(229, 242, 255);
        private readonly Color _mutedColor = Color.FromArgb(137, 178, 218);
        private readonly Color _borderColor = Color.FromArgb(29, 72, 111);

        private Process _activeProcess;
        private bool _cancelRequested;
        private bool _suppressLanguageEvent;
        private bool _installingComponents;
        private bool _lastRunHad403;
        private bool _formatScanLoaded;
        private string _scannedUrl = string.Empty;
        private string _scannedVideoTitle = string.Empty;
        private readonly List<VideoFormatOption> _detectedFormats = new List<VideoFormatOption>();

        private string CurrentLanguageCode
        {
            get
            {
                switch (cmbLanguage.SelectedIndex)
                {
                    case 1: return "be";
                    case 2: return "ar-QA";
                    case 3: return "en";
                    case 4: return "de";
                    case 5: return "zh-CN";
                    default: return "ru";
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            chkAudioOnly.CheckedChanged += chkAudioOnly_CheckedChanged;
            txtUrl.TextChanged += txtUrl_TextChanged;
            cmbDetectedFormat.SelectedIndexChanged += cmbDetectedFormat_SelectedIndexChanged;
            FormClosing += MainForm_FormClosing;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PopulateLanguageCombo();
            LoadSettings();
            ApplyLanguage();
            ApplyBrandTheme();
            ResetDetectedFormats();
            UpdateModeControls();
            CheckDependenciesStatus(true);
        }

        private void PopulateLanguageCombo()
        {
            _suppressLanguageEvent = true;
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add("Русский");
            cmbLanguage.Items.Add("Беларуская");
            cmbLanguage.Items.Add("العربية (قطر)");
            cmbLanguage.Items.Add("English");
            cmbLanguage.Items.Add("Deutsch");
            cmbLanguage.Items.Add("简体中文");
            if (cmbLanguage.SelectedIndex < 0)
                cmbLanguage.SelectedIndex = 0;
            _suppressLanguageEvent = false;
        }

        private void PopulateQualityCombo(int selectedIndex)
        {
            cmbQuality.Items.Clear();
            cmbQuality.Items.Add(T("quality_best"));
            cmbQuality.Items.Add("2160p (4K)");
            cmbQuality.Items.Add("1440p (2K)");
            cmbQuality.Items.Add("1080p (Full HD)");
            cmbQuality.Items.Add("720p (HD)");
            cmbQuality.Items.Add("480p");
            cmbQuality.Items.Add("360p");

            if (selectedIndex < 0 || selectedIndex >= cmbQuality.Items.Count)
                selectedIndex = 0;
            cmbQuality.SelectedIndex = selectedIndex;
        }

        private void PopulateAudioFormatCombo(string selected)
        {
            cmbAudioFormat.Items.Clear();
            cmbAudioFormat.Items.AddRange(new object[] { "mp3", "m4a", "opus", "wav" });

            int index = cmbAudioFormat.Items.IndexOf(selected);
            cmbAudioFormat.SelectedIndex = index >= 0 ? index : 0;
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressLanguageEvent || cmbLanguage.SelectedIndex < 0)
                return;

            int qualityIndex = cmbQuality.SelectedIndex;
            ApplyLanguage();
            PopulateQualityCombo(qualityIndex);
            SaveSettings();
        }

        private void chkAudioOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateModeControls();
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (_formatScanLoaded && !string.Equals(txtUrl.Text.Trim(), _scannedUrl, StringComparison.Ordinal))
                ResetDetectedFormats();
        }

        private void cmbDetectedFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModeControls();
        }

        private void UpdateModeControls()
        {
            bool idle = !_installingComponents && _activeProcess == null;
            bool audioOnly = chkAudioOnly.Checked;
            bool exactFormatSelected = GetSelectedDetectedFormat() != null;

            cmbAudioFormat.Enabled = audioOnly && idle;
            cmbQuality.Enabled = !audioOnly && idle && !exactFormatSelected;
            cmbDetectedFormat.Enabled = !audioOnly && idle && _formatScanLoaded && cmbDetectedFormat.Items.Count > 0;
            btnShowFormats.Enabled = !audioOnly && idle;

            labelAudioFormat.ForeColor = audioOnly ? _textColor : _mutedColor;
            labelQuality.ForeColor = audioOnly || exactFormatSelected ? _mutedColor : _textColor;
            labelDetectedFormat.ForeColor = audioOnly ? _mutedColor : _textColor;
        }

        private void ApplyLanguage()
        {
            int qualityIndex = cmbQuality.SelectedIndex;

            Text = T("title");
            labelBrandSubtitle.Text = T("brand_subtitle");
            labelUrl.Text = T("url");
            labelLanguage.Text = T("language");
            labelQuality.Text = T("quality");
            chkAudioOnly.Text = T("audio_only");
            labelAudioFormat.Text = T("audio_format");
            labelOutputPath.Text = T("output_folder");
            labelDetectedFormat.Text = T("detected_format");
            btnBrowse.Text = T("browse");
            btnDownload.Text = T("download");
            btnShowFormats.Text = T("scan_formats");
            btnBatchDownload.Text = T("batch");
            btnCancel.Text = T("cancel");
            btnUpdateYtDlp.Text = T("update_components");
            labelProgress.Text = T("progress");
            labelLog.Text = T("log");
            lnkAuthor.Text = T("author");

            bool rtl = CurrentLanguageCode == "ar-QA";
            RightToLeft = rtl ? RightToLeft.Yes : RightToLeft.No;
            RightToLeftLayout = rtl;

            // URLs, paths and yt-dlp logs are easier to read left-to-right even in Arabic UI.
            txtUrl.RightToLeft = RightToLeft.No;
            txtOutputPath.RightToLeft = RightToLeft.No;
            txtOutput.RightToLeft = RightToLeft.No;
            cmbDetectedFormat.RightToLeft = RightToLeft.No;

            PopulateQualityCombo(qualityIndex);
            RefreshDetectedFormatTexts();
            UpdateModeControls();
        }

        private void ApplyBrandTheme()
        {
            BackColor = _pageBackground;
            ForeColor = _textColor;
            DoubleBuffered = true;

            panelHeader.StartColor = Color.FromArgb(3, 22, 47);
            panelHeader.EndColor = Color.FromArgb(9, 86, 181);
            panelSettings.BackColor = _panelBackground;
            panelSettings.BorderColor = _borderColor;
            panelFormats.BackColor = _panelBackground;
            panelFormats.BorderColor = _borderColor;
            panelProgress.BackColor = _panelBackground;
            panelProgress.BorderColor = _borderColor;
            panelLog.BackColor = Color.FromArgb(7, 24, 43);
            panelLog.BorderColor = _borderColor;

            labelBrandTitle.ForeColor = Color.White;
            labelBrandSubtitle.ForeColor = Color.FromArgb(190, 220, 250);
            labelBrandMark.BackColor = _brandBlue;
            labelBrandMark.ForeColor = Color.White;
            labelHeaderVersion.BackColor = Color.FromArgb(17, 67, 117);
            labelHeaderVersion.ForeColor = Color.FromArgb(218, 238, 255);

            Label[] labels =
            {
                labelUrl, labelLanguage, labelQuality, labelAudioFormat,
                labelOutputPath, labelDetectedFormat, labelProgress, labelProgressValue,
                labelLog
            };
            foreach (Label label in labels)
                label.ForeColor = _textColor;

            labelFormatHint.ForeColor = _mutedColor;
            chkAudioOnly.ForeColor = _textColor;
            chkAudioOnly.BackColor = Color.Transparent;

            TextBox[] textBoxes = { txtUrl, txtOutputPath };
            foreach (TextBox box in textBoxes)
            {
                box.BackColor = _inputBackground;
                box.ForeColor = _textColor;
                box.BorderStyle = BorderStyle.FixedSingle;
            }

            ComboBox[] combos = { cmbLanguage, cmbQuality, cmbAudioFormat, cmbDetectedFormat };
            foreach (ComboBox combo in combos)
            {
                combo.BackColor = _inputBackground;
                combo.ForeColor = _textColor;
                combo.FlatStyle = FlatStyle.Flat;
            }

            StylePrimaryButton(btnDownload);
            StyleSecondaryButton(btnBrowse);
            StyleAccentButton(btnShowFormats);
            StyleSecondaryButton(btnBatchDownload);
            StyleSecondaryButton(btnUpdateYtDlp);
            StyleDangerButton(btnCancel);

            txtOutput.BackColor = _logBackground;
            txtOutput.ForeColor = Color.FromArgb(199, 222, 243);
            txtOutput.BorderStyle = BorderStyle.None;

            progressBar.TrackColor = Color.FromArgb(12, 45, 77);
            progressBar.FillColor = _brandBlue;
            progressBar.GlowColor = _brandBlueBright;

            lnkYtDlp.LinkColor = _brandBlueBright;
            lnkYtDlp.ActiveLinkColor = Color.White;
            lnkYtDlp.VisitedLinkColor = _brandBlueBright;
            lnkAuthor.LinkColor = _brandBlueBright;
            lnkAuthor.ActiveLinkColor = Color.White;
            lnkAuthor.VisitedLinkColor = _brandBlueBright;

            labelStatus.ForeColor = _brandBlueBright;
            UpdateModeControls();
        }

        private void StylePrimaryButton(Button button)
        {
            button.BackColor = _brandBlue;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = _brandBlueBright;
            button.FlatAppearance.MouseDownBackColor = _brandBlueDark;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        private void StyleAccentButton(Button button)
        {
            button.BackColor = Color.FromArgb(12, 73, 127);
            button.ForeColor = Color.FromArgb(222, 243, 255);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(25, 126, 203);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(16, 96, 164);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(11, 61, 108);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        private void StyleSecondaryButton(Button button)
        {
            button.BackColor = Color.FromArgb(12, 48, 82);
            button.ForeColor = Color.FromArgb(205, 229, 250);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = _borderColor;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(17, 67, 111);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(9, 39, 68);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        private void StyleDangerButton(Button button)
        {
            button.BackColor = Color.FromArgb(43, 59, 79);
            button.ForeColor = Color.FromArgb(225, 233, 241);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(70, 88, 110);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(61, 78, 99);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(33, 45, 61);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        private void ResetDetectedFormats()
        {
            _formatScanLoaded = false;
            _scannedUrl = string.Empty;
            _scannedVideoTitle = string.Empty;
            _detectedFormats.Clear();
            RefreshDetectedFormatTexts();
            UpdateModeControls();
        }

        private void RefreshDetectedFormatTexts()
        {
            int selectedIndex = cmbDetectedFormat.SelectedIndex;
            cmbDetectedFormat.BeginUpdate();
            try
            {
                cmbDetectedFormat.Items.Clear();
                if (!_formatScanLoaded)
                {
                    cmbDetectedFormat.Items.Add(T("format_not_scanned"));
                    cmbDetectedFormat.SelectedIndex = 0;
                    labelFormatHint.Text = T("format_scan_hint");
                    return;
                }

                VideoFormatOption automatic = new VideoFormatOption
                {
                    IsAutomatic = true,
                    DisplayText = T("format_auto")
                };
                cmbDetectedFormat.Items.Add(automatic);

                foreach (VideoFormatOption option in _detectedFormats)
                {
                    option.DisplayText = BuildFormatDisplay(option);
                    cmbDetectedFormat.Items.Add(option);
                }

                if (selectedIndex < 0 || selectedIndex >= cmbDetectedFormat.Items.Count)
                    selectedIndex = 0;
                cmbDetectedFormat.SelectedIndex = selectedIndex;
                labelFormatHint.Text = string.Format(T("formats_found"), _detectedFormats.Count, _scannedVideoTitle);
            }
            finally
            {
                cmbDetectedFormat.EndUpdate();
            }
        }

        private string BuildFormatDisplay(VideoFormatOption option)
        {
            StringBuilder text = new StringBuilder();
            text.Append(option.Id).Append("  •  ")
                .Append(option.Extension.ToUpperInvariant()).Append("  •  ")
                .Append(option.Resolution);

            if (option.Fps > 0)
                text.Append("  •  ").Append(option.Fps.ToString("0.#", CultureInfo.InvariantCulture)).Append(" FPS");
            if (!string.IsNullOrWhiteSpace(option.CodecLabel))
                text.Append("  •  ").Append(option.CodecLabel);
            if (!string.IsNullOrWhiteSpace(option.FileSizeText))
                text.Append("  •  ").Append(option.FileSizeText);

            text.Append("  •  ").Append(option.HasAudio ? T("video_audio") : T("video_only_auto_audio"));
            return text.ToString();
        }

        private string T(string key)
        {
            string lang = CurrentLanguageCode;

            if (lang == "be")
            {
                switch (key)
                {
                    case "title": return "YtDlpGUI 2.2.0 — загрузнік відэа";
                    case "url": return "URL відэа або плэйліста";
                    case "language": return "Мова";
                    case "quality": return "Якасць відэа";
                    case "quality_best": return "Найлепшая";
                    case "brand_subtitle": return "Загрузка відэа і аўдыя праз yt-dlp";
                    case "detected_format": return "Дакладны фармат пасля сканавання";
                    case "scan_formats": return "Сканаваць фарматы";
                    case "format_not_scanned": return "Фарматы яшчэ не прасканаваныя";
                    case "format_scan_hint": return "Устаўце URL і націсніце «Сканаваць фарматы»";
                    case "format_auto": return "Аўтаматычна — выкарыстоўваць выбар якасці вышэй";
                    case "formats_found": return "Знойдзена фарматаў: {0}  •  {1}";
                    case "status_scanning": return "Сканаванне фарматаў...";
                    case "scan_failed": return "Не атрымалася атрымаць фарматы";
                    case "no_video_formats": return "Відэафарматы не знойдзены.";
                    case "video_audio": return "відэа + аўдыя";
                    case "video_only_auto_audio": return "відэа + лепшае аўдыя аўтаматычна";
                    case "audio_only": return "Толькі аўдыя";
                    case "audio_format": return "Фармат аўдыя";
                    case "output_folder": return "Папка захавання";
                    case "browse": return "Агляд...";
                    case "download": return "Спампаваць";
                    case "show_formats": return "Паказаць фарматы";
                    case "batch": return "Пакетная загрузка";
                    case "cancel": return "Адмена";
                    case "update_components": return "Абнавіць кампаненты";
                    case "progress": return "Прагрэс";
                    case "log": return "Журнал";
                    case "author": return "Аўтар";
                    case "error": return "Памылка";
                    case "enter_url": return "Увядзіце URL відэа або плэйліста.";
                    case "text_files": return "Тэкставыя файлы";
                    case "ytdlp_found": return "yt-dlp.exe знойдзены.";
                    case "ytdlp_missing": return "yt-dlp.exe не знойдзены.";
                    case "deno_found": return "Deno знойдзены — падтрымка YouTube JS уключана.";
                    case "deno_missing": return "Deno не знойдзены — YouTube можа вяртаць HTTP 403.";
                    case "ffmpeg_found": return "FFmpeg і FFprobe знойдзены.";
                    case "ffmpeg_missing": return "FFmpeg/FFprobe не знойдзены — лепшая якасць і аб'яднанне патокаў недаступныя.";
                    case "starting": return "Запуск";
                    case "finished": return "Працэс завершаны з кодам";
                    case "cancelled": return "Загрузка адменена.";
                    case "updating": return "Падрыхтоўка кампанентаў...";
                    case "updated": return "Кампаненты гатовыя да працы.";
                    case "update_failed": return "Не атрымалася ўсталяваць/абнавіць кампаненты";
                    case "open_failed": return "Не атрымалася адкрыць спасылку";
                    case "status_ready": return "Гатова";
                    case "status_downloading": return "Загрузка...";
                    case "status_components": return "Кампаненты...";
                    case "components_missing_title": return "Патрэбныя кампаненты";
                    case "components_missing_question": return "Для надзейнай працы з YouTube патрэбныя актуальны yt-dlp, Deno і FFmpeg. Усталяваць адсутныя кампаненты зараз?";
                    case "download_ytdlp": return "Загрузка yt-dlp nightly";
                    case "download_deno": return "Загрузка Deno";
                    case "download_ffmpeg": return "Загрузка FFmpeg";
                    case "extracting": return "Распакоўка";
                    case "youtube_403_hint": return "YouTube вярнуў HTTP 403. Націсніце «Абнавіць кампаненты». Калі памылка застанецца, для некаторых відэа могуць спатрэбіцца cookies з браўзера.";
                }
            }
            else if (lang == "ar-QA")
            {
                switch (key)
                {
                    case "title": return "YtDlpGUI 2.2.0 — تنزيل الفيديو";
                    case "url": return "رابط الفيديو أو قائمة التشغيل";
                    case "language": return "اللغة";
                    case "quality": return "جودة الفيديو";
                    case "quality_best": return "أفضل جودة";
                    case "brand_subtitle": return "تنزيل الفيديو والصوت عبر yt-dlp";
                    case "detected_format": return "الصيغة الدقيقة بعد الفحص";
                    case "scan_formats": return "فحص الصيغ";
                    case "format_not_scanned": return "لم يتم فحص الصيغ بعد";
                    case "format_scan_hint": return "أدخل الرابط ثم اضغط «فحص الصيغ»";
                    case "format_auto": return "تلقائي — استخدم اختيار الجودة أعلاه";
                    case "formats_found": return "تم العثور على {0} صيغة  •  {1}";
                    case "status_scanning": return "جارٍ فحص الصيغ...";
                    case "scan_failed": return "تعذر الحصول على الصيغ";
                    case "no_video_formats": return "لم يتم العثور على صيغ فيديو.";
                    case "video_audio": return "فيديو + صوت";
                    case "video_only_auto_audio": return "فيديو + أفضل صوت تلقائياً";
                    case "audio_only": return "صوت فقط";
                    case "audio_format": return "صيغة الصوت";
                    case "output_folder": return "مجلد الحفظ";
                    case "browse": return "استعراض...";
                    case "download": return "تنزيل";
                    case "show_formats": return "عرض الصيغ";
                    case "batch": return "تنزيل دفعي";
                    case "cancel": return "إلغاء";
                    case "update_components": return "تحديث المكونات";
                    case "progress": return "التقدم";
                    case "log": return "السجل";
                    case "author": return "المؤلف";
                    case "error": return "خطأ";
                    case "enter_url": return "أدخل رابط الفيديو أو قائمة التشغيل.";
                    case "text_files": return "ملفات نصية";
                    case "ytdlp_found": return "تم العثور على yt-dlp.exe.";
                    case "ytdlp_missing": return "لم يتم العثور على yt-dlp.exe.";
                    case "deno_found": return "تم العثور على Deno — دعم JavaScript ليوتيوب مفعّل.";
                    case "deno_missing": return "لم يتم العثور على Deno — قد يعيد YouTube الخطأ HTTP 403.";
                    case "ffmpeg_found": return "تم العثور على FFmpeg وFFprobe.";
                    case "ffmpeg_missing": return "لم يتم العثور على FFmpeg/FFprobe — قد لا تتوفر أفضل جودة ودمج المسارات.";
                    case "starting": return "بدء التشغيل";
                    case "finished": return "انتهت العملية برمز";
                    case "cancelled": return "تم إلغاء التنزيل.";
                    case "updating": return "جارٍ تجهيز المكونات...";
                    case "updated": return "المكونات جاهزة للعمل.";
                    case "update_failed": return "تعذر تثبيت/تحديث المكونات";
                    case "open_failed": return "تعذر فتح الرابط";
                    case "status_ready": return "جاهز";
                    case "status_downloading": return "جارٍ التنزيل...";
                    case "status_components": return "المكونات...";
                    case "components_missing_title": return "مكونات مطلوبة";
                    case "components_missing_question": return "يتطلب YouTube إصداراً حديثاً من yt-dlp وDeno وFFmpeg للعمل بشكل موثوق. هل تريد تثبيت المكونات المفقودة الآن؟";
                    case "download_ytdlp": return "تنزيل yt-dlp nightly";
                    case "download_deno": return "تنزيل Deno";
                    case "download_ffmpeg": return "تنزيل FFmpeg";
                    case "extracting": return "فك الضغط";
                    case "youtube_403_hint": return "أعاد YouTube الخطأ HTTP 403. استخدم «تحديث المكونات». إذا استمرت المشكلة فقد تتطلب بعض المقاطع cookies من المتصفح.";
                }
            }
            else if (lang == "en")
            {
                switch (key)
                {
                    case "title": return "YtDlpGUI 2.2.0 — Video Downloader";
                    case "url": return "Video or playlist URL";
                    case "language": return "Language";
                    case "quality": return "Video quality";
                    case "quality_best": return "Best available";
                    case "brand_subtitle": return "Video and audio downloads powered by yt-dlp";
                    case "detected_format": return "Exact format after scan";
                    case "scan_formats": return "Scan formats";
                    case "format_not_scanned": return "Formats have not been scanned yet";
                    case "format_scan_hint": return "Paste a URL and click “Scan formats”";
                    case "format_auto": return "Automatic — use the quality selector above";
                    case "formats_found": return "Found {0} formats  •  {1}";
                    case "status_scanning": return "Scanning formats...";
                    case "scan_failed": return "Could not retrieve formats";
                    case "no_video_formats": return "No video formats were found.";
                    case "video_audio": return "video + audio";
                    case "video_only_auto_audio": return "video + best audio automatically";
                    case "audio_only": return "Audio only";
                    case "audio_format": return "Audio format";
                    case "output_folder": return "Output folder";
                    case "browse": return "Browse...";
                    case "download": return "Download";
                    case "show_formats": return "Show formats";
                    case "batch": return "Batch download";
                    case "cancel": return "Cancel";
                    case "update_components": return "Update components";
                    case "progress": return "Progress";
                    case "log": return "Log";
                    case "author": return "Author";
                    case "error": return "Error";
                    case "enter_url": return "Enter a video or playlist URL.";
                    case "text_files": return "Text files";
                    case "ytdlp_found": return "yt-dlp.exe found.";
                    case "ytdlp_missing": return "yt-dlp.exe was not found.";
                    case "deno_found": return "Deno found — YouTube JavaScript support is enabled.";
                    case "deno_missing": return "Deno was not found — YouTube may return HTTP 403.";
                    case "ffmpeg_found": return "FFmpeg and FFprobe found.";
                    case "ffmpeg_missing": return "FFmpeg/FFprobe not found — best quality and stream merging may be unavailable.";
                    case "starting": return "Starting";
                    case "finished": return "Process finished with code";
                    case "cancelled": return "Download cancelled.";
                    case "updating": return "Preparing components...";
                    case "updated": return "Components are ready.";
                    case "update_failed": return "Could not install/update components";
                    case "open_failed": return "Could not open link";
                    case "status_ready": return "Ready";
                    case "status_downloading": return "Downloading...";
                    case "status_components": return "Components...";
                    case "components_missing_title": return "Required components";
                    case "components_missing_question": return "Reliable YouTube support requires a current yt-dlp build, Deno and FFmpeg. Install the missing components now?";
                    case "download_ytdlp": return "Downloading yt-dlp nightly";
                    case "download_deno": return "Downloading Deno";
                    case "download_ffmpeg": return "Downloading FFmpeg";
                    case "extracting": return "Extracting";
                    case "youtube_403_hint": return "YouTube returned HTTP 403. Use “Update components”. If it still fails, some videos may require browser cookies.";
                }
            }
            else if (lang == "de")
            {
                switch (key)
                {
                    case "title": return "YtDlpGUI 2.2.0 — Video-Downloader";
                    case "url": return "Video- oder Playlist-URL";
                    case "language": return "Sprache";
                    case "quality": return "Videoqualität";
                    case "quality_best": return "Beste verfügbar";
                    case "brand_subtitle": return "Video- und Audio-Downloads mit yt-dlp";
                    case "detected_format": return "Exaktes Format nach dem Scan";
                    case "scan_formats": return "Formate scannen";
                    case "format_not_scanned": return "Formate wurden noch nicht gescannt";
                    case "format_scan_hint": return "URL einfügen und „Formate scannen“ klicken";
                    case "format_auto": return "Automatisch — Qualitätsauswahl oben verwenden";
                    case "formats_found": return "{0} Formate gefunden  •  {1}";
                    case "status_scanning": return "Formate werden gescannt...";
                    case "scan_failed": return "Formate konnten nicht abgerufen werden";
                    case "no_video_formats": return "Keine Videoformate gefunden.";
                    case "video_audio": return "Video + Audio";
                    case "video_only_auto_audio": return "Video + bestes Audio automatisch";
                    case "audio_only": return "Nur Audio";
                    case "audio_format": return "Audioformat";
                    case "output_folder": return "Zielordner";
                    case "browse": return "Durchsuchen...";
                    case "download": return "Herunterladen";
                    case "show_formats": return "Formate anzeigen";
                    case "batch": return "Stapel-Download";
                    case "cancel": return "Abbrechen";
                    case "update_components": return "Komponenten aktualisieren";
                    case "progress": return "Fortschritt";
                    case "log": return "Protokoll";
                    case "author": return "Autor";
                    case "error": return "Fehler";
                    case "enter_url": return "Geben Sie eine Video- oder Playlist-URL ein.";
                    case "text_files": return "Textdateien";
                    case "ytdlp_found": return "yt-dlp.exe gefunden.";
                    case "ytdlp_missing": return "yt-dlp.exe wurde nicht gefunden.";
                    case "deno_found": return "Deno gefunden — YouTube-JavaScript-Unterstützung ist aktiv.";
                    case "deno_missing": return "Deno wurde nicht gefunden — YouTube kann HTTP 403 zurückgeben.";
                    case "ffmpeg_found": return "FFmpeg und FFprobe gefunden.";
                    case "ffmpeg_missing": return "FFmpeg/FFprobe fehlen — beste Qualität und Stream-Zusammenführung sind möglicherweise nicht verfügbar.";
                    case "starting": return "Start";
                    case "finished": return "Prozess beendet mit Code";
                    case "cancelled": return "Download abgebrochen.";
                    case "updating": return "Komponenten werden vorbereitet...";
                    case "updated": return "Komponenten sind bereit.";
                    case "update_failed": return "Komponenten konnten nicht installiert/aktualisiert werden";
                    case "open_failed": return "Link konnte nicht geöffnet werden";
                    case "status_ready": return "Bereit";
                    case "status_downloading": return "Download läuft...";
                    case "status_components": return "Komponenten...";
                    case "components_missing_title": return "Erforderliche Komponenten";
                    case "components_missing_question": return "Für zuverlässige YouTube-Unterstützung werden ein aktuelles yt-dlp, Deno und FFmpeg benötigt. Fehlende Komponenten jetzt installieren?";
                    case "download_ytdlp": return "yt-dlp nightly wird geladen";
                    case "download_deno": return "Deno wird geladen";
                    case "download_ffmpeg": return "FFmpeg wird geladen";
                    case "extracting": return "Entpacken";
                    case "youtube_403_hint": return "YouTube hat HTTP 403 zurückgegeben. Verwenden Sie „Komponenten aktualisieren“. Falls der Fehler bleibt, können für einige Videos Browser-Cookies nötig sein.";
                }
            }
            else if (lang == "zh-CN")
            {
                switch (key)
                {
                    case "title": return "YtDlpGUI 2.2.0 — 视频下载器";
                    case "url": return "视频或播放列表 URL";
                    case "language": return "语言";
                    case "quality": return "视频质量";
                    case "quality_best": return "最佳可用质量";
                    case "brand_subtitle": return "基于 yt-dlp 的视频和音频下载器";
                    case "detected_format": return "扫描后选择精确格式";
                    case "scan_formats": return "扫描格式";
                    case "format_not_scanned": return "尚未扫描格式";
                    case "format_scan_hint": return "粘贴 URL，然后点击“扫描格式”";
                    case "format_auto": return "自动 — 使用上方质量选择";
                    case "formats_found": return "找到 {0} 个格式  •  {1}";
                    case "status_scanning": return "正在扫描格式...";
                    case "scan_failed": return "无法获取格式";
                    case "no_video_formats": return "未找到视频格式。";
                    case "video_audio": return "视频 + 音频";
                    case "video_only_auto_audio": return "视频 + 自动最佳音频";
                    case "audio_only": return "仅音频";
                    case "audio_format": return "音频格式";
                    case "output_folder": return "保存文件夹";
                    case "browse": return "浏览...";
                    case "download": return "下载";
                    case "show_formats": return "显示格式";
                    case "batch": return "批量下载";
                    case "cancel": return "取消";
                    case "update_components": return "更新组件";
                    case "progress": return "进度";
                    case "log": return "日志";
                    case "author": return "作者";
                    case "error": return "错误";
                    case "enter_url": return "请输入视频或播放列表 URL。";
                    case "text_files": return "文本文件";
                    case "ytdlp_found": return "已找到 yt-dlp.exe。";
                    case "ytdlp_missing": return "未找到 yt-dlp.exe。";
                    case "deno_found": return "已找到 Deno — YouTube JavaScript 支持已启用。";
                    case "deno_missing": return "未找到 Deno — YouTube 可能返回 HTTP 403。";
                    case "ffmpeg_found": return "已找到 FFmpeg 和 FFprobe。";
                    case "ffmpeg_missing": return "未找到 FFmpeg/FFprobe — 可能无法获得最佳质量或合并音视频流。";
                    case "starting": return "启动";
                    case "finished": return "进程结束，退出代码";
                    case "cancelled": return "下载已取消。";
                    case "updating": return "正在准备组件...";
                    case "updated": return "组件已准备就绪。";
                    case "update_failed": return "无法安装/更新组件";
                    case "open_failed": return "无法打开链接";
                    case "status_ready": return "就绪";
                    case "status_downloading": return "正在下载...";
                    case "status_components": return "组件...";
                    case "components_missing_title": return "需要组件";
                    case "components_missing_question": return "为了可靠支持 YouTube，需要最新的 yt-dlp、Deno 和 FFmpeg。现在安装缺少的组件吗？";
                    case "download_ytdlp": return "正在下载 yt-dlp nightly";
                    case "download_deno": return "正在下载 Deno";
                    case "download_ffmpeg": return "正在下载 FFmpeg";
                    case "extracting": return "正在解压";
                    case "youtube_403_hint": return "YouTube 返回 HTTP 403。请使用“更新组件”。如果仍然失败，某些视频可能需要浏览器 cookies。";
                }
            }

            switch (key)
            {
                case "title": return "YtDlpGUI 2.2.0 — загрузчик видео";
                case "url": return "URL видео или плейлиста";
                case "language": return "Язык";
                case "quality": return "Качество видео";
                case "quality_best": return "Лучшее доступное";
                case "brand_subtitle": return "Скачивание видео и аудио через yt-dlp";
                case "detected_format": return "Точный формат после сканирования";
                case "scan_formats": return "Сканировать форматы";
                case "format_not_scanned": return "Форматы ещё не просканированы";
                case "format_scan_hint": return "Вставьте URL и нажмите «Сканировать форматы»";
                case "format_auto": return "Автоматически — использовать выбор качества выше";
                case "formats_found": return "Найдено форматов: {0}  •  {1}";
                case "status_scanning": return "Сканирование форматов...";
                case "scan_failed": return "Не удалось получить список форматов";
                case "no_video_formats": return "Видео-форматы не найдены.";
                case "video_audio": return "видео + аудио";
                case "video_only_auto_audio": return "видео + лучшее аудио автоматически";
                case "audio_only": return "Только аудио";
                case "audio_format": return "Формат аудио";
                case "output_folder": return "Папка сохранения";
                case "browse": return "Обзор...";
                case "download": return "Скачать";
                case "show_formats": return "Показать форматы";
                case "batch": return "Пакетное скачивание";
                case "cancel": return "Отмена";
                case "update_components": return "Обновить компоненты";
                case "progress": return "Прогресс";
                case "log": return "Журнал";
                case "author": return "Автор";
                case "error": return "Ошибка";
                case "enter_url": return "Введите URL видео или плейлиста.";
                case "text_files": return "Текстовые файлы";
                case "ytdlp_found": return "yt-dlp.exe найден.";
                case "ytdlp_missing": return "yt-dlp.exe не найден.";
                case "deno_found": return "Deno найден — поддержка JavaScript для YouTube включена.";
                case "deno_missing": return "Deno не найден — YouTube может возвращать HTTP 403.";
                case "ffmpeg_found": return "FFmpeg и FFprobe найдены.";
                case "ffmpeg_missing": return "FFmpeg/FFprobe не найдены — лучшее качество и объединение потоков могут быть недоступны.";
                case "starting": return "Запуск";
                case "finished": return "Процесс завершён с кодом";
                case "cancelled": return "Загрузка отменена.";
                case "updating": return "Подготовка компонентов...";
                case "updated": return "Компоненты готовы к работе.";
                case "update_failed": return "Не удалось установить/обновить компоненты";
                case "open_failed": return "Не удалось открыть ссылку";
                case "status_ready": return "Готово";
                case "status_downloading": return "Загрузка...";
                case "status_components": return "Компоненты...";
                case "components_missing_title": return "Нужны компоненты";
                case "components_missing_question": return "Для надёжной работы с YouTube нужны актуальный yt-dlp, Deno и FFmpeg. Установить недостающие компоненты сейчас?";
                case "download_ytdlp": return "Загрузка yt-dlp nightly";
                case "download_deno": return "Загрузка Deno";
                case "download_ffmpeg": return "Загрузка FFmpeg";
                case "extracting": return "Распаковка";
                case "youtube_403_hint": return "YouTube вернул HTTP 403. Нажмите «Обновить компоненты». Если ошибка останется, для некоторых видео могут понадобиться cookies из браузера.";
                default: return key;
            }
        }

        private void CheckDependenciesStatus(bool verbose)
        {
            bool ytDlpExists = File.Exists(GetYtDlpPath());
            bool denoExists = !string.IsNullOrEmpty(FindExecutable("deno.exe"));
            bool ffmpegExists = HasFfmpeg();

            // Keep actions available even when dependencies are missing: clicking them
            // will offer automatic component installation.
            bool idle = !_installingComponents && _activeProcess == null;
            btnDownload.Enabled = idle;
            btnBatchDownload.Enabled = idle;
            btnShowFormats.Enabled = idle && !chkAudioOnly.Checked;
            UpdateModeControls();

            if (!verbose)
                return;

            AppendOutput(ytDlpExists ? T("ytdlp_found") : T("ytdlp_missing"),
                ytDlpExists ? LogType.Success : LogType.Error);
            AppendOutput(denoExists ? T("deno_found") : T("deno_missing"),
                denoExists ? LogType.Success : LogType.Warning);
            AppendOutput(ffmpegExists ? T("ffmpeg_found") : T("ffmpeg_missing"),
                ffmpegExists ? LogType.Success : LogType.Warning);
        }

        private string GetYtDlpPath()
        {
            return Path.Combine(Application.StartupPath, "yt-dlp.exe");
        }

        private string GetLocalDenoPath()
        {
            return Path.Combine(Application.StartupPath, "deno.exe");
        }

        private string GetLocalFfmpegPath()
        {
            return Path.Combine(Application.StartupPath, "ffmpeg.exe");
        }

        private string GetLocalFfprobePath()
        {
            return Path.Combine(Application.StartupPath, "ffprobe.exe");
        }

        private string FindExecutable(string fileName)
        {
            string localPath = Path.Combine(Application.StartupPath, fileName);
            if (File.Exists(localPath))
                return localPath;

            string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string rawPath in pathVariable.Split(Path.PathSeparator))
            {
                string candidatePath = rawPath.Trim().Trim('"');
                if (string.IsNullOrWhiteSpace(candidatePath))
                    continue;

                try
                {
                    string candidate = Path.Combine(candidatePath, fileName);
                    if (File.Exists(candidate))
                        return candidate;
                }
                catch
                {
                    // Ignore malformed PATH entries.
                }
            }

            return null;
        }

        private bool HasFfmpeg()
        {
            return !string.IsNullOrEmpty(FindExecutable("ffmpeg.exe")) &&
                   !string.IsNullOrEmpty(FindExecutable("ffprobe.exe"));
        }

        private string GetFfmpegDirectory()
        {
            string ffmpegPath = FindExecutable("ffmpeg.exe");
            if (string.IsNullOrEmpty(ffmpegPath))
                return null;
            return Path.GetDirectoryName(ffmpegPath);
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show(T("enter_url"), T("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!await EnsureComponentsForDownloadAsync())
                return;

            await RunYtDlpAsync(BuildArguments(txtUrl.Text.Trim()));
        }

        private async Task<bool> EnsureComponentsForDownloadAsync()
        {
            bool ytDlpExists = File.Exists(GetYtDlpPath());
            bool denoExists = !string.IsNullOrEmpty(FindExecutable("deno.exe"));
            bool ffmpegExists = HasFfmpeg();

            if (ytDlpExists && denoExists && ffmpegExists)
                return true;

            DialogResult result = MessageBox.Show(
                T("components_missing_question"),
                T("components_missing_title"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (result != DialogResult.Yes)
                return false;

            await InstallComponentsAsync(false);
            return File.Exists(GetYtDlpPath()) &&
                   !string.IsNullOrEmpty(FindExecutable("deno.exe")) &&
                   HasFfmpeg();
        }

        private string BuildArguments(string url)
        {
            StringBuilder arguments = BuildCommonArguments();
            arguments.Append("-- ");
            arguments.Append(QuoteArg(url));
            return arguments.ToString();
        }

        private StringBuilder BuildCommonArguments(bool allowExactFormat = true)
        {
            StringBuilder arguments = new StringBuilder();
            arguments.Append("--ignore-config --newline --windows-filenames ");
            arguments.Append("--retries 10 --fragment-retries 10 --extractor-retries 3 ");
            AppendRuntimeSupportArguments(arguments);

            if (!string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                string template = Path.Combine(txtOutputPath.Text.Trim(), "%(title)s.%(ext)s");
                arguments.Append("-o ").Append(QuoteArg(template)).Append(" ");
            }

            if (chkAudioOnly.Checked)
            {
                string audioFormat = cmbAudioFormat.SelectedItem == null ? "mp3" : cmbAudioFormat.SelectedItem.ToString();
                arguments.Append("-x --audio-format ").Append(QuoteArg(audioFormat)).Append(" --audio-quality 0 ");
            }
            else
            {
                VideoFormatOption exactFormat = allowExactFormat ? GetSelectedDetectedFormat() : null;
                if (exactFormat != null && string.Equals(txtUrl.Text.Trim(), _scannedUrl, StringComparison.Ordinal))
                {
                    string selector = exactFormat.HasAudio
                        ? exactFormat.Id
                        : exactFormat.Id + "+bestaudio/best";
                    arguments.Append("-f ").Append(QuoteArg(selector)).Append(" ");
                }
                else
                {
                    int maxHeight = GetSelectedMaxHeight();
                    if (maxHeight > 0)
                        arguments.Append("-S ").Append(QuoteArg("res:" + maxHeight)).Append(" ");
                }
            }

            return arguments;
        }

        private void AppendRuntimeSupportArguments(StringBuilder arguments)
        {
            string denoPath = FindExecutable("deno.exe");
            if (!string.IsNullOrEmpty(denoPath))
            {
                arguments.Append("--js-runtimes ")
                    .Append(QuoteArg("deno:" + denoPath))
                    .Append(" ");
            }

            string ffmpegDirectory = GetFfmpegDirectory();
            if (!string.IsNullOrEmpty(ffmpegDirectory))
            {
                arguments.Append("--ffmpeg-location ")
                    .Append(QuoteArg(ffmpegDirectory))
                    .Append(" ");
            }
        }

        private VideoFormatOption GetSelectedDetectedFormat()
        {
            if (!_formatScanLoaded || cmbDetectedFormat.SelectedItem == null)
                return null;

            VideoFormatOption option = cmbDetectedFormat.SelectedItem as VideoFormatOption;
            if (option == null || option.IsAutomatic)
                return null;
            return option;
        }

        private int GetSelectedMaxHeight()
        {
            switch (cmbQuality.SelectedIndex)
            {
                case 1: return 2160;
                case 2: return 1440;
                case 3: return 1080;
                case 4: return 720;
                case 5: return 480;
                case 6: return 360;
                default: return 0;
            }
        }

        private static string QuoteArg(string value)
        {
            if (value == null)
                return "\"\"";
            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        private async void btnShowFormats_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show(T("enter_url"), T("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!await EnsureComponentsForDownloadAsync())
                return;

            await ScanFormatsAsync(txtUrl.Text.Trim());
        }

        private async Task ScanFormatsAsync(string url)
        {
            if (_activeProcess != null || _installingComponents)
                return;

            _cancelRequested = false;
            _lastRunHad403 = false;
            SetProgress(0);
            AppendOutput(T("status_scanning") + " " + url, LogType.Info);

            StringBuilder arguments = new StringBuilder();
            arguments.Append("--ignore-config --no-warnings --skip-download --playlist-items 1 --dump-single-json ");
            AppendRuntimeSupportArguments(arguments);
            arguments.Append("-- ").Append(QuoteArg(url));

            try
            {
                using (Process process = new Process())
                {
                    _activeProcess = process;
                    process.StartInfo = CreateYtDlpStartInfo(arguments.ToString());
                    SetBusyState(true);
                    SetStatus(T("status_scanning"));

                    process.Start();
                    Task<string> stdoutTask = process.StandardOutput.ReadToEndAsync();
                    Task<string> stderrTask = process.StandardError.ReadToEndAsync();
                    await Task.Run(() => process.WaitForExit());
                    string stdout = await stdoutTask;
                    string stderr = await stderrTask;

                    if (_cancelRequested)
                    {
                        AppendOutput(T("cancelled"), LogType.Info);
                        return;
                    }

                    if (process.ExitCode != 0)
                    {
                        if (!string.IsNullOrWhiteSpace(stderr))
                        {
                            foreach (string line in SplitLines(stderr))
                            {
                                ObserveYtDlpLine(line);
                                AppendOutput(line, line.StartsWith("WARNING:", StringComparison.OrdinalIgnoreCase) ? LogType.Warning : LogType.Error);
                            }
                        }
                        AppendOutput(T("scan_failed") + ": " + process.ExitCode, LogType.Error);
                        if (_lastRunHad403)
                            AppendOutput(T("youtube_403_hint"), LogType.Warning);
                        return;
                    }

                    string title;
                    List<VideoFormatOption> formats = ParseFormatsJson(stdout, out title);
                    if (formats.Count == 0)
                    {
                        AppendOutput(T("no_video_formats"), LogType.Warning);
                        ResetDetectedFormats();
                        return;
                    }

                    _detectedFormats.Clear();
                    _detectedFormats.AddRange(formats);
                    _scannedUrl = url;
                    _scannedVideoTitle = string.IsNullOrWhiteSpace(title) ? url : title;
                    _formatScanLoaded = true;
                    RefreshDetectedFormatTexts();
                    SetProgress(100);
                    AppendOutput(string.Format(T("formats_found"), formats.Count, _scannedVideoTitle), LogType.Success);
                }
            }
            catch (Exception ex)
            {
                AppendOutput(T("scan_failed") + ": " + ex.Message, LogType.Error);
            }
            finally
            {
                _activeProcess = null;
                SetBusyState(false);
                UpdateModeControls();
            }
        }

        private ProcessStartInfo CreateYtDlpStartInfo(string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = GetYtDlpPath(),
                Arguments = arguments,
                WorkingDirectory = Application.StartupPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            startInfo.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
            return startInfo;
        }

        private List<VideoFormatOption> ParseFormatsJson(string json, out string title)
        {
            title = string.Empty;
            List<VideoFormatOption> results = new List<VideoFormatOption>();
            if (string.IsNullOrWhiteSpace(json))
                return results;

            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue,
                RecursionLimit = 256
            };

            Dictionary<string, object> root = serializer.DeserializeObject(json) as Dictionary<string, object>;
            Dictionary<string, object> info = GetFirstVideoInfo(root);
            if (info == null)
                return results;

            title = GetString(info, "title");
            object formatsObject;
            if (!info.TryGetValue("formats", out formatsObject))
                return results;

            IEnumerable formats = formatsObject as IEnumerable;
            if (formats == null)
                return results;

            foreach (object item in formats)
            {
                Dictionary<string, object> format = item as Dictionary<string, object>;
                if (format == null)
                    continue;

                string id = GetString(format, "format_id");
                string ext = GetString(format, "ext");
                string vcodec = GetString(format, "vcodec");
                string acodec = GetString(format, "acodec");
                int height = (int)Math.Round(GetDouble(format, "height"));
                int width = (int)Math.Round(GetDouble(format, "width"));
                double fps = GetDouble(format, "fps");

                bool hasVideo = !string.IsNullOrWhiteSpace(vcodec) &&
                                !string.Equals(vcodec, "none", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(vcodec, "images", StringComparison.OrdinalIgnoreCase);
                bool hasAudio = !string.IsNullOrWhiteSpace(acodec) &&
                                !string.Equals(acodec, "none", StringComparison.OrdinalIgnoreCase);

                if (!hasVideo || string.IsNullOrWhiteSpace(id) || string.Equals(ext, "mhtml", StringComparison.OrdinalIgnoreCase))
                    continue;

                string resolution = GetString(format, "resolution");
                if (string.IsNullOrWhiteSpace(resolution) || resolution.IndexOf("audio", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (height > 0)
                        resolution = height + "p";
                    else if (width > 0)
                        resolution = width + "w";
                    else
                        resolution = "?";
                }

                double exactSize = GetDouble(format, "filesize");
                double approximateSize = GetDouble(format, "filesize_approx");
                bool approximate = exactSize <= 0 && approximateSize > 0;
                double bytes = exactSize > 0 ? exactSize : approximateSize;

                results.Add(new VideoFormatOption
                {
                    Id = id,
                    Extension = string.IsNullOrWhiteSpace(ext) ? "?" : ext,
                    Resolution = resolution,
                    Height = height,
                    Fps = fps,
                    HasAudio = hasAudio,
                    CodecLabel = GetCodecLabel(vcodec),
                    FileSizeText = FormatFileSize(bytes, approximate),
                    Tbr = GetDouble(format, "tbr")
                });
            }

            results.Sort(delegate(VideoFormatOption left, VideoFormatOption right)
            {
                int byHeight = right.Height.CompareTo(left.Height);
                if (byHeight != 0) return byHeight;
                int byFps = right.Fps.CompareTo(left.Fps);
                if (byFps != 0) return byFps;
                int byAudio = right.HasAudio.CompareTo(left.HasAudio);
                if (byAudio != 0) return byAudio;
                int byBitrate = right.Tbr.CompareTo(left.Tbr);
                if (byBitrate != 0) return byBitrate;
                return string.Compare(left.Id, right.Id, StringComparison.OrdinalIgnoreCase);
            });

            return results;
        }

        private static Dictionary<string, object> GetFirstVideoInfo(Dictionary<string, object> root)
        {
            if (root == null)
                return null;
            if (root.ContainsKey("formats"))
                return root;

            object entriesObject;
            if (!root.TryGetValue("entries", out entriesObject))
                return root;

            IEnumerable entries = entriesObject as IEnumerable;
            if (entries == null)
                return root;

            foreach (object entry in entries)
            {
                Dictionary<string, object> dictionary = entry as Dictionary<string, object>;
                if (dictionary != null)
                    return dictionary;
            }
            return root;
        }

        private static string GetString(Dictionary<string, object> dictionary, string key)
        {
            object value;
            if (dictionary == null || !dictionary.TryGetValue(key, out value) || value == null)
                return string.Empty;
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static double GetDouble(Dictionary<string, object> dictionary, string key)
        {
            object value;
            if (dictionary == null || !dictionary.TryGetValue(key, out value) || value == null)
                return 0;
            try
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        private static string GetCodecLabel(string codec)
        {
            if (string.IsNullOrWhiteSpace(codec) || string.Equals(codec, "none", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            string lower = codec.ToLowerInvariant();
            if (lower.StartsWith("avc1") || lower.StartsWith("h264")) return "H.264";
            if (lower.StartsWith("av01")) return "AV1";
            if (lower.StartsWith("vp9") || lower.StartsWith("vp09")) return "VP9";
            if (lower.StartsWith("hev1") || lower.StartsWith("hvc1") || lower.StartsWith("h265")) return "H.265";
            int dot = codec.IndexOf('.');
            return dot > 0 ? codec.Substring(0, dot).ToUpperInvariant() : codec.ToUpperInvariant();
        }

        private static string FormatFileSize(double bytes, bool approximate)
        {
            if (bytes <= 0)
                return string.Empty;

            string prefix = approximate ? "≈" : string.Empty;
            double mib = bytes / 1024d / 1024d;
            if (mib >= 1024)
                return prefix + (mib / 1024d).ToString("0.##", CultureInfo.InvariantCulture) + " GiB";
            return prefix + mib.ToString(mib >= 100 ? "0" : "0.#", CultureInfo.InvariantCulture) + " MiB";
        }

        private static string[] SplitLines(string text)
        {
            return (text ?? string.Empty)
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private async void btnBatchDownload_Click(object sender, EventArgs e)
        {
            if (!await EnsureComponentsForDownloadAsync())
                return;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = T("text_files") + "|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder args = BuildCommonArguments(false);
                    args.Append("-a ").Append(QuoteArg(ofd.FileName));
                    await RunYtDlpAsync(args.ToString());
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(txtOutputPath.Text))
                    fbd.SelectedPath = txtOutputPath.Text;

                if (fbd.ShowDialog() == DialogResult.OK)
                    txtOutputPath.Text = fbd.SelectedPath;
            }
        }

        private async Task RunYtDlpAsync(string arguments, bool showProgress = true)
        {
            if (!File.Exists(GetYtDlpPath()))
            {
                AppendOutput(T("ytdlp_missing"), LogType.Error);
                return;
            }

            if (_activeProcess != null || _installingComponents)
                return;

            _cancelRequested = false;
            _lastRunHad403 = false;
            SetBusyState(true);
            if (showProgress)
                SetProgress(0);

            AppendOutput(T("starting") + ": yt-dlp " + arguments, LogType.Info);

            try
            {
                using (Process process = new Process())
                {
                    _activeProcess = process;
                    process.StartInfo = CreateYtDlpStartInfo(arguments);
                    SetBusyState(true);

                    process.OutputDataReceived += (s, ev) =>
                    {
                        if (string.IsNullOrEmpty(ev.Data)) return;
                        ObserveYtDlpLine(ev.Data);
                        AppendOutput(ev.Data, LogType.Normal);
                        if (showProgress) TryUpdateProgress(ev.Data);
                    };

                    process.ErrorDataReceived += (s, ev) =>
                    {
                        if (string.IsNullOrEmpty(ev.Data)) return;
                        ObserveYtDlpLine(ev.Data);
                        AppendOutput(ev.Data,
                            ev.Data.StartsWith("WARNING:", StringComparison.OrdinalIgnoreCase) ? LogType.Warning : LogType.Error);
                        if (showProgress) TryUpdateProgress(ev.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    await Task.Run(() => process.WaitForExit());

                    if (_cancelRequested)
                    {
                        AppendOutput(T("cancelled"), LogType.Info);
                    }
                    else if (process.ExitCode == 0)
                    {
                        if (showProgress) SetProgress(100);
                        AppendOutput(T("finished") + ": 0", LogType.Success);
                    }
                    else
                    {
                        AppendOutput(T("finished") + ": " + process.ExitCode, LogType.Error);
                        if (_lastRunHad403)
                            AppendOutput(T("youtube_403_hint"), LogType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendOutput(T("error") + ": " + ex.Message, LogType.Error);
            }
            finally
            {
                _activeProcess = null;
                SetBusyState(false);
                CheckDependenciesStatus(false);
            }
        }

        private void ObserveYtDlpLine(string line)
        {
            if (line.IndexOf("HTTP Error 403", StringComparison.OrdinalIgnoreCase) >= 0 ||
                line.IndexOf("403: Forbidden", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _lastRunHad403 = true;
            }
        }

        private void TryUpdateProgress(string line)
        {
            Match match = Regex.Match(line, @"\[download\]\s+(\d{1,3}(?:\.\d+)?)%");
            if (!match.Success) return;

            double value;
            if (double.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out value))
            {
                SetProgress((int)Math.Max(0, Math.Min(100, Math.Round(value))));
            }
        }

        private void SetProgress(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(SetProgress), value);
                return;
            }
            progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Maximum, value));
            labelProgressValue.Text = progressBar.Value + "%";
        }

        private void SetStatus(string text)
        {
            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke(new Action<string>(SetStatus), text);
                return;
            }
            labelStatus.Text = text;
        }

        private void SetBusyState(bool busy)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetBusyState), busy);
                return;
            }

            btnDownload.Enabled = !busy;
            btnBatchDownload.Enabled = !busy;
            btnBrowse.Enabled = !busy;
            btnUpdateYtDlp.Enabled = !busy;
            btnCancel.Enabled = busy && _activeProcess != null;
            chkAudioOnly.Enabled = !busy;
            cmbLanguage.Enabled = !busy;
            txtUrl.Enabled = !busy;
            txtOutputPath.Enabled = !busy;

            if (!busy)
            {
                labelStatus.Text = T("status_ready");
                UpdateModeControls();
            }
            else
            {
                btnShowFormats.Enabled = false;
                cmbQuality.Enabled = false;
                cmbAudioFormat.Enabled = false;
                cmbDetectedFormat.Enabled = false;

                if (_installingComponents)
                    labelStatus.Text = T("status_components");
                else
                    labelStatus.Text = T("status_downloading");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Process process = _activeProcess;
            if (process == null) return;

            _cancelRequested = true;
            try
            {
                if (!process.HasExited)
                    process.Kill();
            }
            catch (Exception ex)
            {
                AppendOutput(T("error") + ": " + ex.Message, LogType.Error);
            }
        }

        private async void btnUpdateYtDlp_Click(object sender, EventArgs e)
        {
            if (_activeProcess != null || _installingComponents)
                return;

            await InstallComponentsAsync(true);
        }

        private async Task InstallComponentsAsync(bool forceAll)
        {
            _installingComponents = true;
            SetBusyState(true);
            SetProgress(0);
            AppendOutput(T("updating"), LogType.Info);

            string tempYtDlp = Path.Combine(Path.GetTempPath(), "yt-dlp-" + Guid.NewGuid().ToString("N") + ".exe");
            string tempDenoZip = Path.Combine(Path.GetTempPath(), "deno-" + Guid.NewGuid().ToString("N") + ".zip");
            string tempFfmpegZip = Path.Combine(Path.GetTempPath(), "ffmpeg-" + Guid.NewGuid().ToString("N") + ".zip");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Always refresh yt-dlp when the user requests an update, and also when
                // automatic dependency setup is needed. Nightly is recommended by yt-dlp
                // for regular users because site-side changes can break stable releases.
                await DownloadFileAsync(YtDlpNightlyUrl, tempYtDlp, T("download_ytdlp"));
                File.Copy(tempYtDlp, GetYtDlpPath(), true);

                if (forceAll || string.IsNullOrEmpty(FindExecutable("deno.exe")))
                {
                    await DownloadFileAsync(DenoLatestUrl, tempDenoZip, T("download_deno"));
                    AppendOutput(T("extracting") + ": Deno", LogType.Info);
                    ExtractExecutableFromZip(tempDenoZip, "deno.exe", GetLocalDenoPath());
                }

                if (forceAll || !HasFfmpeg())
                {
                    await DownloadFileAsync(FfmpegLatestUrl, tempFfmpegZip, T("download_ffmpeg"));
                    AppendOutput(T("extracting") + ": FFmpeg", LogType.Info);
                    ExtractExecutableFromZip(tempFfmpegZip, "ffmpeg.exe", GetLocalFfmpegPath());
                    ExtractExecutableFromZip(tempFfmpegZip, "ffprobe.exe", GetLocalFfprobePath());
                }

                SetProgress(100);
                AppendOutput(T("updated"), LogType.Success);
            }
            catch (Exception ex)
            {
                AppendOutput(T("update_failed") + ": " + ex.Message, LogType.Error);
            }
            finally
            {
                TryDelete(tempYtDlp);
                TryDelete(tempDenoZip);
                TryDelete(tempFfmpegZip);
                _installingComponents = false;
                SetBusyState(false);
                CheckDependenciesStatus(true);
            }
        }

        private async Task DownloadFileAsync(string url, string destination, string label)
        {
            AppendOutput(label + "...", LogType.Info);
            SetStatus(label + "...");
            SetProgress(0);

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "YtDlpGUI/2.2");
                client.DownloadProgressChanged += (s, e) =>
                {
                    SetProgress(e.ProgressPercentage);
                    SetStatus(label + " — " + e.ProgressPercentage + "%");
                };
                await client.DownloadFileTaskAsync(new Uri(url), destination);
            }
        }

        private static void ExtractExecutableFromZip(string archivePath, string executableName, string destinationPath)
        {
            using (FileStream stream = File.OpenRead(archivePath))
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                ZipArchiveEntry selectedEntry = null;

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string normalizedEntryName = entry.FullName.Replace('\\', '/');
                    string fileName = normalizedEntryName.Substring(normalizedEntryName.LastIndexOf('/') + 1);
                    if (!string.Equals(fileName, executableName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    selectedEntry = entry;
                    if (normalizedEntryName.IndexOf("/bin/", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        break;
                    }
                }

                if (selectedEntry == null)
                    throw new InvalidDataException(executableName + " not found in downloaded archive.");

                string tempDestination = destinationPath + ".new";
                if (File.Exists(tempDestination))
                    File.Delete(tempDestination);

                using (Stream input = selectedEntry.Open())
                using (FileStream output = new FileStream(tempDestination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    input.CopyTo(output);
                }

                File.Copy(tempDestination, destinationPath, true);
                File.Delete(tempDestination);
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Temporary files will be cleaned by Windows later.
            }
        }

        private void AppendOutput(string text, LogType type)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action<string, LogType>(AppendOutput), text, type);
                return;
            }

            Color color;
            switch (type)
            {
                case LogType.Error: color = Color.FromArgb(255, 105, 120); break;
                case LogType.Warning: color = Color.FromArgb(255, 190, 92); break;
                case LogType.Success: color = Color.FromArgb(77, 218, 155); break;
                case LogType.Info: color = _brandBlueBright; break;
                default: color = Color.FromArgb(198, 221, 242); break;
            }

            txtOutput.SelectionStart = txtOutput.TextLength;
            txtOutput.SelectionColor = color;
            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.SelectionColor = txtOutput.ForeColor;
            txtOutput.ScrollToCaret();
        }

        private void lnkYtDlp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/yt-dlp/yt-dlp");
        }

        private void lnkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://my.ws-soft.ru/");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                AppendOutput(T("open_failed") + ": " + ex.Message, LogType.Error);
            }
        }

        private void LoadSettings()
        {
            string lang = "ru";
            int qualityIndex = 0;
            string audioFormat = "mp3";
            string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            try
            {
                string path = GetSettingsPath();
                if (File.Exists(path))
                {
                    foreach (string rawLine in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        int separator = rawLine.IndexOf('=');
                        if (separator <= 0) continue;
                        string key = rawLine.Substring(0, separator).Trim();
                        string value = rawLine.Substring(separator + 1);

                        if (key == "language") lang = value;
                        else if (key == "quality") int.TryParse(value, out qualityIndex);
                        else if (key == "audioFormat") audioFormat = value;
                        else if (key == "outputPath") outputPath = value;
                    }
                }
            }
            catch
            {
                // Use defaults if settings cannot be read.
            }

            cmbLanguage.SelectedIndex = LanguageIndexFromCode(lang);
            PopulateQualityCombo(qualityIndex);
            PopulateAudioFormatCombo(audioFormat);
            txtOutputPath.Text = outputPath;
        }

        private void SaveSettings()
        {
            try
            {
                string path = GetSettingsPath();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string[] lines =
                {
                    "language=" + CurrentLanguageCode,
                    "quality=" + Math.Max(0, cmbQuality.SelectedIndex),
                    "audioFormat=" + (cmbAudioFormat.SelectedItem == null ? "mp3" : cmbAudioFormat.SelectedItem.ToString()),
                    "outputPath=" + txtOutputPath.Text
                };
                File.WriteAllLines(path, lines, Encoding.UTF8);
            }
            catch
            {
                // Settings are optional; download functionality must keep working.
            }
        }

        private string GetSettingsPath()
        {
            return Path.Combine(Application.UserAppDataPath, "settings.ini");
        }

        private int LanguageIndexFromCode(string code)
        {
            switch (code)
            {
                case "be": return 1;
                case "ar-QA": return 2;
                case "en": return 3;
                case "de": return 4;
                case "zh-CN": return 5;
                default: return 0;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            if (_activeProcess != null)
            {
                try
                {
                    if (!_activeProcess.HasExited)
                        _activeProcess.Kill();
                }
                catch
                {
                    // Ignore cleanup errors during shutdown.
                }
            }
        }

        private sealed class VideoFormatOption
        {
            public string Id { get; set; } = string.Empty;
            public string Extension { get; set; } = string.Empty;
            public string Resolution { get; set; } = string.Empty;
            public int Height { get; set; }
            public double Fps { get; set; }
            public double Tbr { get; set; }
            public bool HasAudio { get; set; }
            public bool IsAutomatic { get; set; }
            public string CodecLabel { get; set; } = string.Empty;
            public string FileSizeText { get; set; } = string.Empty;
            public string DisplayText { get; set; } = string.Empty;

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private enum LogType
        {
            Normal,
            Info,
            Success,
            Warning,
            Error
        }
    }
}
