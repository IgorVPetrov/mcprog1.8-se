namespace mcprog
{
    partial class BootloaderForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BootloaderForm));
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.fileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveEEPROMFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFirmwareFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.readEEPROMButton = new System.Windows.Forms.ToolStripButton();
            this.writeFirmwareButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.resetButton = new System.Windows.Forms.ToolStripButton();
            this.editorContainer = new System.Windows.Forms.ToolStripContainer();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.hexBox = new Be.Windows.Forms.HexBox();
            this.mainToolStrip.SuspendLayout();
            this.editorContainer.ContentPanel.SuspendLayout();
            this.editorContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileButton,
            this.toolStripSeparator1,
            this.readEEPROMButton,
            this.writeFirmwareButton,
            this.toolStripSeparator2,
            this.resetButton});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(607, 25);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // fileButton
            // 
            this.fileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveEEPROMFileMenuItem,
            this.openFirmwareFileMenuItem});
            this.fileButton.Image = ((System.Drawing.Image)(resources.GetObject("fileButton.Image")));
            this.fileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileButton.Name = "fileButton";
            this.fileButton.Size = new System.Drawing.Size(36, 22);
            this.fileButton.Text = "File";
            // 
            // saveEEPROMFileMenuItem
            // 
            this.saveEEPROMFileMenuItem.Name = "saveEEPROMFileMenuItem";
            this.saveEEPROMFileMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveEEPROMFileMenuItem.Text = "Save EEPROM file";
            this.saveEEPROMFileMenuItem.Click += new System.EventHandler(this.saveEEPROMFileMenuItem_Click);
            // 
            // openFirmwareFileMenuItem
            // 
            this.openFirmwareFileMenuItem.Name = "openFirmwareFileMenuItem";
            this.openFirmwareFileMenuItem.Size = new System.Drawing.Size(171, 22);
            this.openFirmwareFileMenuItem.Text = "Open firmvare file";
            this.openFirmwareFileMenuItem.Click += new System.EventHandler(this.openFirmwareFileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // readEEPROMButton
            // 
            this.readEEPROMButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readEEPROMButton.Image = ((System.Drawing.Image)(resources.GetObject("readEEPROMButton.Image")));
            this.readEEPROMButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readEEPROMButton.Name = "readEEPROMButton";
            this.readEEPROMButton.Size = new System.Drawing.Size(80, 22);
            this.readEEPROMButton.Text = "Read EEPROM";
            this.readEEPROMButton.Click += new System.EventHandler(this.readEEPROMButton_Click);
            // 
            // writeFirmwareButton
            // 
            this.writeFirmwareButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.writeFirmwareButton.Image = ((System.Drawing.Image)(resources.GetObject("writeFirmwareButton.Image")));
            this.writeFirmwareButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.writeFirmwareButton.Name = "writeFirmwareButton";
            this.writeFirmwareButton.Size = new System.Drawing.Size(82, 22);
            this.writeFirmwareButton.Text = "Write firmware";
            this.writeFirmwareButton.Click += new System.EventHandler(this.writeFirmwareButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // resetButton
            // 
            this.resetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.resetButton.Image = ((System.Drawing.Image)(resources.GetObject("resetButton.Image")));
            this.resetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(39, 22);
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // editorContainer
            // 
            this.editorContainer.BottomToolStripPanelVisible = false;
            // 
            // editorContainer.ContentPanel
            // 
            this.editorContainer.ContentPanel.Controls.Add(this.infoPanel);
            this.editorContainer.ContentPanel.Controls.Add(this.hexBox);
            this.editorContainer.ContentPanel.Size = new System.Drawing.Size(607, 265);
            this.editorContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorContainer.LeftToolStripPanelVisible = false;
            this.editorContainer.Location = new System.Drawing.Point(0, 25);
            this.editorContainer.Name = "editorContainer";
            this.editorContainer.RightToolStripPanelVisible = false;
            this.editorContainer.Size = new System.Drawing.Size(607, 265);
            this.editorContainer.TabIndex = 1;
            this.editorContainer.Text = "toolStripContainer1";
            this.editorContainer.TopToolStripPanelVisible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // infoPanel
            // 
            this.infoPanel.AutoSize = true;
            this.infoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.infoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.infoPanel.Location = new System.Drawing.Point(217, 82);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 1;
            this.infoPanel.Visible = false;
            // 
            // hexBox
            // 
            this.hexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox.Encoding = ((System.Text.Encoding)(resources.GetObject("hexBox.Encoding")));
            this.hexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox.LineInfoForeColor = System.Drawing.Color.Brown;
            this.hexBox.LineInfoVisible = true;
            this.hexBox.Location = new System.Drawing.Point(0, 0);
            this.hexBox.Name = "hexBox";
            this.hexBox.ReadOnly = true;
            this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox.Size = new System.Drawing.Size(607, 265);
            this.hexBox.StartAddress = ((long)(0));
            this.hexBox.StringViewVisible = true;
            this.hexBox.TabIndex = 0;
            this.hexBox.UseFixedBytesPerLine = true;
            this.hexBox.VScrollBarVisible = true;
            // 
            // BootloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 290);
            this.Controls.Add(this.editorContainer);
            this.Controls.Add(this.mainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BootloaderForm";
            this.Text = "Bootloader";
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.editorContainer.ContentPanel.ResumeLayout(false);
            this.editorContainer.ContentPanel.PerformLayout();
            this.editorContainer.ResumeLayout(false);
            this.editorContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripContainer editorContainer;
        private Be.Windows.Forms.HexBox hexBox;
        private System.Windows.Forms.ToolStripDropDownButton fileButton;
        private System.Windows.Forms.ToolStripMenuItem saveEEPROMFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFirmwareFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton readEEPROMButton;
        private System.Windows.Forms.ToolStripButton writeFirmwareButton;
        private InfoPanel infoPanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton resetButton;
    }
}