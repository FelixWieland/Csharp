namespace Labyrinth {
    partial class Form1 {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.generate_btn = new System.Windows.Forms.Button();
            this.loadlevel_btn = new System.Windows.Forms.Button();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.showLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // generate_btn
            // 
            this.generate_btn.Location = new System.Drawing.Point(12, 12);
            this.generate_btn.Name = "generate_btn";
            this.generate_btn.Size = new System.Drawing.Size(75, 23);
            this.generate_btn.TabIndex = 0;
            this.generate_btn.Text = "Generieren";
            this.generate_btn.UseVisualStyleBackColor = true;
            this.generate_btn.Click += new System.EventHandler(this.generate_btn_Click);
            // 
            // loadlevel_btn
            // 
            this.loadlevel_btn.Location = new System.Drawing.Point(93, 12);
            this.loadlevel_btn.Name = "loadlevel_btn";
            this.loadlevel_btn.Size = new System.Drawing.Size(75, 23);
            this.loadlevel_btn.TabIndex = 1;
            this.loadlevel_btn.Text = "Laden..";
            this.loadlevel_btn.UseVisualStyleBackColor = true;
            this.loadlevel_btn.Click += new System.EventHandler(this.loadlevel_btn_Click);
            // 
            // browser
            // 
            this.browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browser.Location = new System.Drawing.Point(11, 41);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(897, 701);
            this.browser.TabIndex = 2;
            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.browser_DocumentCompleted);
            // 
            // showLog
            // 
            this.showLog.Location = new System.Drawing.Point(175, 12);
            this.showLog.Name = "showLog";
            this.showLog.Size = new System.Drawing.Size(75, 23);
            this.showLog.TabIndex = 3;
            this.showLog.Text = "Zeige Log";
            this.showLog.UseVisualStyleBackColor = true;
            this.showLog.Click += new System.EventHandler(this.showLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 754);
            this.Controls.Add(this.showLog);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.loadlevel_btn);
            this.Controls.Add(this.generate_btn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Labyrinth";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button generate_btn;
        private System.Windows.Forms.Button loadlevel_btn;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Button showLog;
    }
}

