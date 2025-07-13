using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace YtDlpGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckYtDlpExistence();
        }

        private void CheckYtDlpExistence()
        {
            if (!File.Exists("yt-dlp.exe"))
            {
                AppendOutput("ОШИБКА: yt-dlp.exe не найден в папке приложения!", true);
                btnDownload.Enabled = false;
                btnShowFormats.Enabled = false;
                btnBatchDownload.Enabled = false;
            }
            else
            {
                AppendOutput("yt-dlp.exe найден. Готов к работе.", false);
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Введите URL видео", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string arguments = BuildArguments();
            await RunYtDlpAsync(arguments);
        }

        private string BuildArguments()
        {
            StringBuilder arguments = new StringBuilder();

            // Путь сохранения
            if (!string.IsNullOrWhiteSpace(txtOutputPath.Text))
            {
                arguments.Append($"-o \"{txtOutputPath.Text.Trim()}\" ");
            }

            // Только аудио
            if (chkAudioOnly.Checked)
            {
                arguments.Append("-x ");
            }

            // Лучшее качество
            if (chkBestQuality.Checked)
            {
                arguments.Append("-f bestvideo+bestaudio ");
            }

            // URL
            arguments.Append(txtUrl.Text.Trim());

            return arguments.ToString();
        }

        private async void btnShowFormats_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Введите URL видео", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await RunYtDlpAsync($"-F \"{txtUrl.Text.Trim()}\"");
        }

        private async void btnBatchDownload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Текстовые файлы|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await RunYtDlpAsync($"-a \"{ofd.FileName}\"");
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = Path.Combine(fbd.SelectedPath, "%(title)s.%(ext)s");
                }
            }
        }

        private async Task RunYtDlpAsync(string arguments)
        {
            if (!File.Exists("yt-dlp.exe"))
            {
                AppendOutput("ОШИБКА: yt-dlp.exe не найден!", true);
                return;
            }

            AppendOutput($"Запуск: yt-dlp {arguments}", false);

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "yt-dlp.exe",
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    };

                    process.EnableRaisingEvents = true;

                    process.OutputDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput(e.Data, false);
                    };

                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput(e.Data, true);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Асинхронное ожидание завершения процесса
                    await Task.Run(() => process.WaitForExit());

                    AppendOutput($"\r\nПроцесс завершен с кодом: {process.ExitCode}",
                        process.ExitCode == 0);
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"ОШИБКА: {ex.Message}", true);
            }
        }

        private void AppendOutput(string text, bool isError)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action(() => AppendOutput(text, isError)));
            }
            else
            {
                Color color = isError ? Color.Red : Color.Black;

                txtOutput.SelectionStart = txtOutput.TextLength;
                txtOutput.SelectionColor = color;
                txtOutput.AppendText(text + Environment.NewLine);
                txtOutput.SelectionColor = txtOutput.ForeColor;

                // Автоматическая прокрутка вниз
                txtOutput.ScrollToCaret();
            }
        }

        private void lnkYtDlp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/yt-dlp/yt-dlp",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                AppendOutput($"Не удалось открыть ссылку: {ex.Message}", true);
            }
        }

        private void lnkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://my.ws-soft.ru/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                AppendOutput($"Не удалось открыть ссылку: {ex.Message}", true);
            }
        }
    }
}