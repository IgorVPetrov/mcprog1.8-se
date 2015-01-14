namespace mcprog
{
    partial class InfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backwardNavigButton = new System.Windows.Forms.ToolStripButton();
            this.forwardNavigButton = new System.Windows.Forms.ToolStripButton();
            this.homeNavigButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(0, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(566, 346);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backwardNavigButton,
            this.forwardNavigButton,
            this.homeNavigButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(566, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // backwardNavigButton
            // 
            this.backwardNavigButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backwardNavigButton.Image = ((System.Drawing.Image)(resources.GetObject("backwardNavigButton.Image")));
            this.backwardNavigButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backwardNavigButton.Name = "backwardNavigButton";
            this.backwardNavigButton.Size = new System.Drawing.Size(61, 22);
            this.backwardNavigButton.Text = "<< Назад";
            this.backwardNavigButton.Click += new System.EventHandler(this.backwardNavigButton_Click);
            // 
            // forwardNavigButton
            // 
            this.forwardNavigButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.forwardNavigButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardNavigButton.Image")));
            this.forwardNavigButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardNavigButton.Name = "forwardNavigButton";
            this.forwardNavigButton.Size = new System.Drawing.Size(67, 22);
            this.forwardNavigButton.Text = "Вперёд >>";
            this.forwardNavigButton.Click += new System.EventHandler(this.forwardNavigButton_Click);
            // 
            // homeNavigButton
            // 
            this.homeNavigButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.homeNavigButton.Image = ((System.Drawing.Image)(resources.GetObject("homeNavigButton.Image")));
            this.homeNavigButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.homeNavigButton.Name = "homeNavigButton";
            this.homeNavigButton.Size = new System.Drawing.Size(48, 22);
            this.homeNavigButton.Text = "Начало";
            this.homeNavigButton.Click += new System.EventHandler(this.homeNavigButton_Click);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(566, 371);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.webBrowser);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Info                           http://igoruha.org/at88resetter.php";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InfoForm_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton backwardNavigButton;
        private System.Windows.Forms.ToolStripButton forwardNavigButton;
        private System.Windows.Forms.ToolStripButton homeNavigButton;






    }
}