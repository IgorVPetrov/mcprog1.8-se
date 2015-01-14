namespace mcprog
{
    partial class AutoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoForm));
            this.chipToolStrip = new System.Windows.Forms.ToolStrip();
            this.chipSelectComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.programmerToolStrip = new System.Windows.Forms.ToolStrip();
            this.readButton = new System.Windows.Forms.ToolStripButton();
            this.programButton = new System.Windows.Forms.ToolStripButton();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.textBoxContainer = new System.Windows.Forms.ToolStripContainer();
            this.infoLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chipToolStrip.SuspendLayout();
            this.programmerToolStrip.SuspendLayout();
            this.textBoxContainer.ContentPanel.SuspendLayout();
            this.textBoxContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // chipToolStrip
            // 
            this.chipToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.chipToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chipSelectComboBox});
            this.chipToolStrip.Location = new System.Drawing.Point(0, 0);
            this.chipToolStrip.Name = "chipToolStrip";
            this.chipToolStrip.Size = new System.Drawing.Size(258, 25);
            this.chipToolStrip.TabIndex = 6;
            this.chipToolStrip.Text = "toolStrip1";
            // 
            // chipSelectComboBox
            // 
            this.chipSelectComboBox.Name = "chipSelectComboBox";
            this.chipSelectComboBox.Size = new System.Drawing.Size(232, 21);
            this.chipSelectComboBox.DropDownClosed += new System.EventHandler(this.chipSelectComboBox_DropDownClosed);
            // 
            // programmerToolStrip
            // 
            this.programmerToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.programmerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readButton,
            this.programButton,
            this.progressBar});
            this.programmerToolStrip.Location = new System.Drawing.Point(0, 25);
            this.programmerToolStrip.Name = "programmerToolStrip";
            this.programmerToolStrip.Size = new System.Drawing.Size(258, 25);
            this.programmerToolStrip.TabIndex = 7;
            this.programmerToolStrip.Text = "toolStrip1";
            // 
            // readButton
            // 
            this.readButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readButton.Image = ((System.Drawing.Image)(resources.GetObject("readButton.Image")));
            this.readButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(36, 22);
            this.readButton.Text = "Read";
            this.readButton.Click += new System.EventHandler(this.Read_Click);
            // 
            // programButton
            // 
            this.programButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.programButton.Image = ((System.Drawing.Image)(resources.GetObject("programButton.Image")));
            this.programButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.programButton.Name = "programButton";
            this.programButton.Size = new System.Drawing.Size(51, 22);
            this.programButton.Text = "Program";
            this.programButton.Click += new System.EventHandler(this.Write_Click);
            // 
            // progressBar
            // 
            this.progressBar.AutoSize = false;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(120, 10);
            // 
            // textBoxContainer
            // 
            this.textBoxContainer.BottomToolStripPanelVisible = false;
            // 
            // textBoxContainer.ContentPanel
            // 
            this.textBoxContainer.ContentPanel.Controls.Add(this.infoLayoutPanel);
            this.textBoxContainer.ContentPanel.Size = new System.Drawing.Size(258, 176);
            this.textBoxContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContainer.LeftToolStripPanelVisible = false;
            this.textBoxContainer.Location = new System.Drawing.Point(0, 50);
            this.textBoxContainer.Name = "textBoxContainer";
            this.textBoxContainer.RightToolStripPanelVisible = false;
            this.textBoxContainer.Size = new System.Drawing.Size(258, 176);
            this.textBoxContainer.TabIndex = 8;
            this.textBoxContainer.Text = "toolStripContainer1";
            this.textBoxContainer.TopToolStripPanelVisible = false;
            // 
            // infoLayoutPanel
            // 
            this.infoLayoutPanel.AutoScroll = true;
            this.infoLayoutPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.infoLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.infoLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.infoLayoutPanel.Name = "infoLayoutPanel";
            this.infoLayoutPanel.Size = new System.Drawing.Size(258, 176);
            this.infoLayoutPanel.TabIndex = 0;
            this.infoLayoutPanel.WrapContents = false;
            this.infoLayoutPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.infoLayoutPanel_ControlAdded);
            // 
            // AutoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 226);
            this.Controls.Add(this.textBoxContainer);
            this.Controls.Add(this.programmerToolStrip);
            this.Controls.Add(this.chipToolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoForm";
            this.Text = "Auto programmer";
            this.Load += new System.EventHandler(this.AutoForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AutoForm_FormClosed);
            this.chipToolStrip.ResumeLayout(false);
            this.chipToolStrip.PerformLayout();
            this.programmerToolStrip.ResumeLayout(false);
            this.programmerToolStrip.PerformLayout();
            this.textBoxContainer.ContentPanel.ResumeLayout(false);
            this.textBoxContainer.ResumeLayout(false);
            this.textBoxContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip chipToolStrip;
        private System.Windows.Forms.ToolStrip programmerToolStrip;
        private System.Windows.Forms.ToolStripComboBox chipSelectComboBox;
        private System.Windows.Forms.ToolStripButton readButton;
        private System.Windows.Forms.ToolStripButton programButton;
        private System.Windows.Forms.ToolStripContainer textBoxContainer;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.FlowLayoutPanel infoLayoutPanel;
    }
}