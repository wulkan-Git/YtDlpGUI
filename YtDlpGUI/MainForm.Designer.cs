namespace YtDlpGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAudioOnly = new System.Windows.Forms.CheckBox();
            this.btnShowFormats = new System.Windows.Forms.Button();
            this.chkBestQuality = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnBatchDownload = new System.Windows.Forms.Button();
            this.lnkYtDlp = new System.Windows.Forms.LinkLabel();
            this.lnkAuthor = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(230)))), ((int)(((byte)(103)))));
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrl.Location = new System.Drawing.Point(15, 30);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(550, 20);
            this.txtUrl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(230)))), ((int)(((byte)(57)))));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL видео";
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDownload.Location = new System.Drawing.Point(23, 108);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(116, 30);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Скачать";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(230)))), ((int)(((byte)(103)))));
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutput.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(0)))));
            this.txtOutput.Location = new System.Drawing.Point(15, 157);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(650, 250);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(230)))), ((int)(((byte)(57)))));
            this.label2.Location = new System.Drawing.Point(12, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Вывод";
            // 
            // chkAudioOnly
            // 
            this.chkAudioOnly.AutoSize = true;
            this.chkAudioOnly.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkAudioOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(230)))), ((int)(((byte)(57)))));
            this.chkAudioOnly.Location = new System.Drawing.Point(15, 60);
            this.chkAudioOnly.Name = "chkAudioOnly";
            this.chkAudioOnly.Size = new System.Drawing.Size(129, 20);
            this.chkAudioOnly.TabIndex = 5;
            this.chkAudioOnly.Text = "Только аудио (-x)";
            this.chkAudioOnly.UseVisualStyleBackColor = true;
            // 
            // btnShowFormats
            // 
            this.btnShowFormats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnShowFormats.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowFormats.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnShowFormats.Location = new System.Drawing.Point(145, 108);
            this.btnShowFormats.Name = "btnShowFormats";
            this.btnShowFormats.Size = new System.Drawing.Size(156, 30);
            this.btnShowFormats.TabIndex = 6;
            this.btnShowFormats.Text = "Показать форматы ";
            this.btnShowFormats.UseVisualStyleBackColor = false;
            this.btnShowFormats.Click += new System.EventHandler(this.btnShowFormats_Click);
            // 
            // chkBestQuality
            // 
            this.chkBestQuality.AutoSize = true;
            this.chkBestQuality.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkBestQuality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(230)))), ((int)(((byte)(57)))));
            this.chkBestQuality.Location = new System.Drawing.Point(15, 85);
            this.chkBestQuality.Name = "chkBestQuality";
            this.chkBestQuality.Size = new System.Drawing.Size(135, 20);
            this.chkBestQuality.TabIndex = 7;
            this.chkBestQuality.Text = "Лучшее качество ";
            this.chkBestQuality.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(230)))), ((int)(((byte)(57)))));
            this.label3.Location = new System.Drawing.Point(250, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Путь сохранения";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(230)))), ((int)(((byte)(103)))));
            this.txtOutputPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputPath.Location = new System.Drawing.Point(250, 80);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(315, 20);
            this.txtOutputPath.TabIndex = 9;
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowse.Location = new System.Drawing.Point(578, 76);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(46, 23);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBatchDownload
            // 
            this.btnBatchDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnBatchDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBatchDownload.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBatchDownload.Location = new System.Drawing.Point(311, 108);
            this.btnBatchDownload.Name = "btnBatchDownload";
            this.btnBatchDownload.Size = new System.Drawing.Size(156, 30);
            this.btnBatchDownload.TabIndex = 11;
            this.btnBatchDownload.Text = "Пакетное скачивание";
            this.btnBatchDownload.UseVisualStyleBackColor = false;
            this.btnBatchDownload.Click += new System.EventHandler(this.btnBatchDownload_Click);
            // 
            // lnkYtDlp
            // 
            this.lnkYtDlp.AutoSize = true;
            this.lnkYtDlp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(230)))), ((int)(((byte)(103)))));
            this.lnkYtDlp.Location = new System.Drawing.Point(15, 410);
            this.lnkYtDlp.Name = "lnkYtDlp";
            this.lnkYtDlp.Size = new System.Drawing.Size(68, 13);
            this.lnkYtDlp.TabIndex = 12;
            this.lnkYtDlp.TabStop = true;
            this.lnkYtDlp.Text = "yt-dlp GitHub";
            this.lnkYtDlp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkYtDlp_LinkClicked);
            // 
            // lnkAuthor
            // 
            this.lnkAuthor.AutoSize = true;
            this.lnkAuthor.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(230)))), ((int)(((byte)(103)))));
            this.lnkAuthor.Location = new System.Drawing.Point(100, 410);
            this.lnkAuthor.Name = "lnkAuthor";
            this.lnkAuthor.Size = new System.Drawing.Size(37, 13);
            this.lnkAuthor.TabIndex = 13;
            this.lnkAuthor.TabStop = true;
            this.lnkAuthor.Text = "Автор";
            this.lnkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAuthor_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(684, 441);
            this.Controls.Add(this.lnkAuthor);
            this.Controls.Add(this.lnkYtDlp);
            this.Controls.Add(this.btnBatchDownload);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkBestQuality);
            this.Controls.Add(this.btnShowFormats);
            this.Controls.Add(this.chkAudioOnly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "yt-dlp GUI - WULK@N";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAudioOnly;
        private System.Windows.Forms.Button btnShowFormats;
        private System.Windows.Forms.CheckBox chkBestQuality;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnBatchDownload;
        private System.Windows.Forms.LinkLabel lnkYtDlp;
        private System.Windows.Forms.LinkLabel lnkAuthor;
    }
}