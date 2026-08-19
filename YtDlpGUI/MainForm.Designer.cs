namespace YtDlpGUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private GradientPanel panelHeader;
        private System.Windows.Forms.Label labelBrandMark;
        private System.Windows.Forms.Label labelBrandTitle;
        private System.Windows.Forms.Label labelBrandSubtitle;
        private System.Windows.Forms.Label labelHeaderVersion;
        private CardPanel panelSettings;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label labelQuality;
        private System.Windows.Forms.ComboBox cmbQuality;
        private System.Windows.Forms.CheckBox chkAudioOnly;
        private System.Windows.Forms.Label labelAudioFormat;
        private System.Windows.Forms.ComboBox cmbAudioFormat;
        private System.Windows.Forms.Label labelOutputPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowse;
        private CardPanel panelFormats;
        private System.Windows.Forms.Label labelDetectedFormat;
        private System.Windows.Forms.Label labelFormatHint;
        private System.Windows.Forms.ComboBox cmbDetectedFormat;
        private System.Windows.Forms.Button btnShowFormats;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnBatchDownload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdateYtDlp;
        private CardPanel panelProgress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label labelProgressValue;
        private ModernProgressBar progressBar;
        private System.Windows.Forms.Label labelStatus;
        private CardPanel panelLog;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.LinkLabel lnkYtDlp;
        private System.Windows.Forms.LinkLabel lnkAuthor;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new YtDlpGUI.GradientPanel();
            this.labelBrandMark = new System.Windows.Forms.Label();
            this.labelBrandTitle = new System.Windows.Forms.Label();
            this.labelBrandSubtitle = new System.Windows.Forms.Label();
            this.labelHeaderVersion = new System.Windows.Forms.Label();
            this.panelSettings = new YtDlpGUI.CardPanel();
            this.labelUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.labelQuality = new System.Windows.Forms.Label();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.chkAudioOnly = new System.Windows.Forms.CheckBox();
            this.labelAudioFormat = new System.Windows.Forms.Label();
            this.cmbAudioFormat = new System.Windows.Forms.ComboBox();
            this.labelOutputPath = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panelFormats = new YtDlpGUI.CardPanel();
            this.labelDetectedFormat = new System.Windows.Forms.Label();
            this.labelFormatHint = new System.Windows.Forms.Label();
            this.cmbDetectedFormat = new System.Windows.Forms.ComboBox();
            this.btnShowFormats = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnBatchDownload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdateYtDlp = new System.Windows.Forms.Button();
            this.panelProgress = new YtDlpGUI.CardPanel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.labelProgressValue = new System.Windows.Forms.Label();
            this.progressBar = new YtDlpGUI.ModernProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelLog = new YtDlpGUI.CardPanel();
            this.labelLog = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.lnkYtDlp = new System.Windows.Forms.LinkLabel();
            this.lnkAuthor = new System.Windows.Forms.LinkLabel();
            this.panelHeader.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.panelFormats.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelBrandMark);
            this.panelHeader.Controls.Add(this.labelBrandTitle);
            this.panelHeader.Controls.Add(this.labelBrandSubtitle);
            this.panelHeader.Controls.Add(this.labelHeaderVersion);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.EndColor = System.Drawing.Color.FromArgb(10, 91, 190);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 92);
            this.panelHeader.StartColor = System.Drawing.Color.FromArgb(4, 24, 51);
            this.panelHeader.TabIndex = 0;
            // 
            // labelBrandMark
            // 
            this.labelBrandMark.BackColor = System.Drawing.Color.FromArgb(0, 153, 255);
            this.labelBrandMark.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.labelBrandMark.ForeColor = System.Drawing.Color.White;
            this.labelBrandMark.Location = new System.Drawing.Point(22, 20);
            this.labelBrandMark.Name = "labelBrandMark";
            this.labelBrandMark.Size = new System.Drawing.Size(52, 52);
            this.labelBrandMark.TabIndex = 0;
            this.labelBrandMark.Text = "WS";
            this.labelBrandMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBrandTitle
            // 
            this.labelBrandTitle.AutoSize = true;
            this.labelBrandTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 19F, System.Drawing.FontStyle.Bold);
            this.labelBrandTitle.ForeColor = System.Drawing.Color.White;
            this.labelBrandTitle.Location = new System.Drawing.Point(90, 15);
            this.labelBrandTitle.Name = "labelBrandTitle";
            this.labelBrandTitle.Size = new System.Drawing.Size(136, 36);
            this.labelBrandTitle.TabIndex = 1;
            this.labelBrandTitle.Text = "YtDlpGUI";
            // 
            // labelBrandSubtitle
            // 
            this.labelBrandSubtitle.AutoSize = true;
            this.labelBrandSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandSubtitle.ForeColor = System.Drawing.Color.FromArgb(190, 220, 250);
            this.labelBrandSubtitle.Location = new System.Drawing.Point(94, 53);
            this.labelBrandSubtitle.Name = "labelBrandSubtitle";
            this.labelBrandSubtitle.Size = new System.Drawing.Size(231, 17);
            this.labelBrandSubtitle.TabIndex = 2;
            this.labelBrandSubtitle.Text = "Скачивание видео и аудио через yt-dlp";
            // 
            // labelHeaderVersion
            // 
            this.labelHeaderVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeaderVersion.BackColor = System.Drawing.Color.FromArgb(18, 65, 112);
            this.labelHeaderVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.labelHeaderVersion.ForeColor = System.Drawing.Color.FromArgb(220, 238, 255);
            this.labelHeaderVersion.Location = new System.Drawing.Point(786, 29);
            this.labelHeaderVersion.Name = "labelHeaderVersion";
            this.labelHeaderVersion.Size = new System.Drawing.Size(88, 30);
            this.labelHeaderVersion.TabIndex = 3;
            this.labelHeaderVersion.Text = "v2.2.0";
            this.labelHeaderVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.BackColor = System.Drawing.Color.FromArgb(10, 34, 61);
            this.panelSettings.BorderColor = System.Drawing.Color.FromArgb(24, 66, 104);
            this.panelSettings.Controls.Add(this.labelUrl);
            this.panelSettings.Controls.Add(this.txtUrl);
            this.panelSettings.Controls.Add(this.labelLanguage);
            this.panelSettings.Controls.Add(this.cmbLanguage);
            this.panelSettings.Controls.Add(this.labelQuality);
            this.panelSettings.Controls.Add(this.cmbQuality);
            this.panelSettings.Controls.Add(this.chkAudioOnly);
            this.panelSettings.Controls.Add(this.labelAudioFormat);
            this.panelSettings.Controls.Add(this.cmbAudioFormat);
            this.panelSettings.Controls.Add(this.labelOutputPath);
            this.panelSettings.Controls.Add(this.txtOutputPath);
            this.panelSettings.Controls.Add(this.btnBrowse);
            this.panelSettings.CornerRadius = 14;
            this.panelSettings.Location = new System.Drawing.Point(18, 108);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(864, 205);
            this.panelSettings.TabIndex = 1;
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelUrl.Location = new System.Drawing.Point(20, 15);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(124, 17);
            this.labelUrl.TabIndex = 0;
            this.labelUrl.Text = "URL видео";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrl.Location = new System.Drawing.Point(20, 39);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(824, 25);
            this.txtUrl.TabIndex = 1;
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(20, 80);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(46, 17);
            this.labelLanguage.TabIndex = 2;
            this.labelLanguage.Text = "Язык";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(20, 103);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(176, 25);
            this.cmbLanguage.TabIndex = 3;
            // 
            // labelQuality
            // 
            this.labelQuality.AutoSize = true;
            this.labelQuality.Location = new System.Drawing.Point(215, 80);
            this.labelQuality.Name = "labelQuality";
            this.labelQuality.Size = new System.Drawing.Size(70, 17);
            this.labelQuality.TabIndex = 4;
            this.labelQuality.Text = "Качество";
            // 
            // cmbQuality
            // 
            this.cmbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Location = new System.Drawing.Point(215, 103);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(190, 25);
            this.cmbQuality.TabIndex = 5;
            // 
            // chkAudioOnly
            // 
            this.chkAudioOnly.AutoSize = true;
            this.chkAudioOnly.Location = new System.Drawing.Point(430, 105);
            this.chkAudioOnly.Name = "chkAudioOnly";
            this.chkAudioOnly.Size = new System.Drawing.Size(117, 21);
            this.chkAudioOnly.TabIndex = 6;
            this.chkAudioOnly.Text = "Только аудио";
            this.chkAudioOnly.UseVisualStyleBackColor = false;
            // 
            // labelAudioFormat
            // 
            this.labelAudioFormat.AutoSize = true;
            this.labelAudioFormat.Location = new System.Drawing.Point(590, 80);
            this.labelAudioFormat.Name = "labelAudioFormat";
            this.labelAudioFormat.Size = new System.Drawing.Size(101, 17);
            this.labelAudioFormat.TabIndex = 7;
            this.labelAudioFormat.Text = "Формат аудио";
            // 
            // cmbAudioFormat
            // 
            this.cmbAudioFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAudioFormat.FormattingEnabled = true;
            this.cmbAudioFormat.Location = new System.Drawing.Point(590, 103);
            this.cmbAudioFormat.Name = "cmbAudioFormat";
            this.cmbAudioFormat.Size = new System.Drawing.Size(132, 25);
            this.cmbAudioFormat.TabIndex = 8;
            // 
            // labelOutputPath
            // 
            this.labelOutputPath.AutoSize = true;
            this.labelOutputPath.Location = new System.Drawing.Point(20, 145);
            this.labelOutputPath.Name = "labelOutputPath";
            this.labelOutputPath.Size = new System.Drawing.Size(131, 17);
            this.labelOutputPath.TabIndex = 9;
            this.labelOutputPath.Text = "Папка сохранения";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputPath.Location = new System.Drawing.Point(20, 168);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(681, 25);
            this.txtOutputPath.TabIndex = 10;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(713, 164);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(131, 33);
            this.btnBrowse.TabIndex = 11;
            this.btnBrowse.Text = "Обзор...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panelFormats
            // 
            this.panelFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFormats.BackColor = System.Drawing.Color.FromArgb(10, 34, 61);
            this.panelFormats.BorderColor = System.Drawing.Color.FromArgb(24, 66, 104);
            this.panelFormats.Controls.Add(this.labelDetectedFormat);
            this.panelFormats.Controls.Add(this.labelFormatHint);
            this.panelFormats.Controls.Add(this.cmbDetectedFormat);
            this.panelFormats.Controls.Add(this.btnShowFormats);
            this.panelFormats.CornerRadius = 14;
            this.panelFormats.Location = new System.Drawing.Point(18, 326);
            this.panelFormats.Name = "panelFormats";
            this.panelFormats.Size = new System.Drawing.Size(864, 94);
            this.panelFormats.TabIndex = 2;
            // 
            // labelDetectedFormat
            // 
            this.labelDetectedFormat.AutoSize = true;
            this.labelDetectedFormat.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelDetectedFormat.Location = new System.Drawing.Point(20, 13);
            this.labelDetectedFormat.Name = "labelDetectedFormat";
            this.labelDetectedFormat.Size = new System.Drawing.Size(228, 17);
            this.labelDetectedFormat.TabIndex = 0;
            this.labelDetectedFormat.Text = "Точный формат после сканирования";
            // 
            // labelFormatHint
            // 
            this.labelFormatHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFormatHint.ForeColor = System.Drawing.Color.FromArgb(137, 178, 218);
            this.labelFormatHint.Location = new System.Drawing.Point(430, 12);
            this.labelFormatHint.Name = "labelFormatHint";
            this.labelFormatHint.Size = new System.Drawing.Size(414, 18);
            this.labelFormatHint.TabIndex = 1;
            this.labelFormatHint.Text = "Сначала вставьте URL и нажмите «Сканировать форматы»";
            this.labelFormatHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbDetectedFormat
            // 
            this.cmbDetectedFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDetectedFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDetectedFormat.DropDownWidth = 820;
            this.cmbDetectedFormat.Enabled = false;
            this.cmbDetectedFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDetectedFormat.FormattingEnabled = true;
            this.cmbDetectedFormat.Location = new System.Drawing.Point(20, 44);
            this.cmbDetectedFormat.Name = "cmbDetectedFormat";
            this.cmbDetectedFormat.Size = new System.Drawing.Size(627, 25);
            this.cmbDetectedFormat.TabIndex = 2;
            // 
            // btnShowFormats
            // 
            this.btnShowFormats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowFormats.Location = new System.Drawing.Point(660, 39);
            this.btnShowFormats.Name = "btnShowFormats";
            this.btnShowFormats.Size = new System.Drawing.Size(184, 35);
            this.btnShowFormats.TabIndex = 3;
            this.btnShowFormats.Text = "Сканировать форматы";
            this.btnShowFormats.UseVisualStyleBackColor = false;
            this.btnShowFormats.Click += new System.EventHandler(this.btnShowFormats_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDownload.Location = new System.Drawing.Point(18, 438);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(168, 42);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "Скачать";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnBatchDownload
            // 
            this.btnBatchDownload.Location = new System.Drawing.Point(197, 438);
            this.btnBatchDownload.Name = "btnBatchDownload";
            this.btnBatchDownload.Size = new System.Drawing.Size(163, 42);
            this.btnBatchDownload.TabIndex = 4;
            this.btnBatchDownload.Text = "Пакетное скачивание";
            this.btnBatchDownload.UseVisualStyleBackColor = false;
            this.btnBatchDownload.Click += new System.EventHandler(this.btnBatchDownload_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(371, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 42);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdateYtDlp
            // 
            this.btnUpdateYtDlp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateYtDlp.Location = new System.Drawing.Point(666, 438);
            this.btnUpdateYtDlp.Name = "btnUpdateYtDlp";
            this.btnUpdateYtDlp.Size = new System.Drawing.Size(216, 42);
            this.btnUpdateYtDlp.TabIndex = 6;
            this.btnUpdateYtDlp.Text = "Обновить компоненты";
            this.btnUpdateYtDlp.UseVisualStyleBackColor = false;
            this.btnUpdateYtDlp.Click += new System.EventHandler(this.btnUpdateYtDlp_Click);
            // 
            // panelProgress
            // 
            this.panelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProgress.BackColor = System.Drawing.Color.FromArgb(10, 34, 61);
            this.panelProgress.BorderColor = System.Drawing.Color.FromArgb(24, 66, 104);
            this.panelProgress.Controls.Add(this.labelProgress);
            this.panelProgress.Controls.Add(this.labelProgressValue);
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Controls.Add(this.labelStatus);
            this.panelProgress.CornerRadius = 14;
            this.panelProgress.Location = new System.Drawing.Point(18, 496);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(864, 72);
            this.panelProgress.TabIndex = 7;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.labelProgress.Location = new System.Drawing.Point(20, 13);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(67, 15);
            this.labelProgress.TabIndex = 0;
            this.labelProgress.Text = "Прогресс";
            // 
            // labelProgressValue
            // 
            this.labelProgressValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgressValue.Location = new System.Drawing.Point(789, 13);
            this.labelProgressValue.Name = "labelProgressValue";
            this.labelProgressValue.Size = new System.Drawing.Size(55, 17);
            this.labelProgressValue.TabIndex = 1;
            this.labelProgressValue.Text = "0%";
            this.labelProgressValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.FillColor = System.Drawing.Color.FromArgb(0, 145, 255);
            this.progressBar.GlowColor = System.Drawing.Color.FromArgb(55, 190, 255);
            this.progressBar.Location = new System.Drawing.Point(20, 43);
            this.progressBar.Maximum = 100;
            this.progressBar.Minimum = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(824, 12);
            this.progressBar.TabIndex = 2;
            this.progressBar.TrackColor = System.Drawing.Color.FromArgb(13, 46, 78);
            this.progressBar.Value = 0;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.ForeColor = System.Drawing.Color.FromArgb(84, 190, 255);
            this.labelStatus.Location = new System.Drawing.Point(570, 13);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(205, 17);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.Text = "Готово";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelLog
            // 
            this.panelLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLog.BackColor = System.Drawing.Color.FromArgb(7, 24, 43);
            this.panelLog.BorderColor = System.Drawing.Color.FromArgb(24, 66, 104);
            this.panelLog.Controls.Add(this.labelLog);
            this.panelLog.Controls.Add(this.txtOutput);
            this.panelLog.CornerRadius = 14;
            this.panelLog.Location = new System.Drawing.Point(18, 582);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(864, 173);
            this.panelLog.TabIndex = 8;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelLog.Location = new System.Drawing.Point(20, 12);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(57, 17);
            this.labelLog.TabIndex = 0;
            this.labelLog.Text = "Журнал";
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(4, 17, 31);
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtOutput.Location = new System.Drawing.Point(20, 39);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(824, 116);
            this.txtOutput.TabIndex = 1;
            this.txtOutput.Text = "";
            // 
            // lnkYtDlp
            // 
            this.lnkYtDlp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkYtDlp.AutoSize = true;
            this.lnkYtDlp.Location = new System.Drawing.Point(23, 770);
            this.lnkYtDlp.Name = "lnkYtDlp";
            this.lnkYtDlp.Size = new System.Drawing.Size(92, 17);
            this.lnkYtDlp.TabIndex = 9;
            this.lnkYtDlp.TabStop = true;
            this.lnkYtDlp.Text = "yt-dlp GitHub";
            this.lnkYtDlp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkYtDlp_LinkClicked);
            // 
            // lnkAuthor
            // 
            this.lnkAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkAuthor.AutoSize = true;
            this.lnkAuthor.Location = new System.Drawing.Point(137, 770);
            this.lnkAuthor.Name = "lnkAuthor";
            this.lnkAuthor.Size = new System.Drawing.Size(48, 17);
            this.lnkAuthor.TabIndex = 10;
            this.lnkAuthor.TabStop = true;
            this.lnkAuthor.Text = "Автор";
            this.lnkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAuthor_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(4, 18, 34);
            this.ClientSize = new System.Drawing.Size(900, 800);
            this.Controls.Add(this.lnkAuthor);
            this.Controls.Add(this.lnkYtDlp);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.panelProgress);
            this.Controls.Add(this.btnUpdateYtDlp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBatchDownload);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.panelFormats);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.ForeColor = System.Drawing.Color.FromArgb(227, 240, 253);
            this.MinimumSize = new System.Drawing.Size(916, 839);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YtDlpGUI 2.2.0 — загрузчик видео";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panelFormats.ResumeLayout(false);
            this.panelFormats.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.panelLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
