namespace mcprog
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenuToolStrip = new System.Windows.Forms.ToolStrip();
            this.Options = new System.Windows.Forms.ToolStripDropDownButton();
            this.HexEditorSettingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectCodepageComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.addInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddAT88Button = new System.Windows.Forms.ToolStripButton();
            this.Add24CXXButton = new System.Windows.Forms.ToolStripButton();
            this.addXerox0190Button = new System.Windows.Forms.ToolStripButton();
            this.add921Button = new System.Windows.Forms.ToolStripButton();
            this.add1WireButton = new System.Windows.Forms.ToolStripButton();
            this.addRFIDButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addAutoButton = new System.Windows.Forms.ToolStripButton();
            this.HexEditorFontDialog = new System.Windows.Forms.FontDialog();
            this.MainMenuToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuToolStrip
            // 
            this.MainMenuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Options,
            this.addInfo,
            this.toolStripSeparator1,
            this.AddAT88Button,
            this.Add24CXXButton,
            this.addXerox0190Button,
            this.add921Button,
            this.add1WireButton,
            this.addRFIDButton,
            this.toolStripSeparator2,
            this.addAutoButton});
            this.MainMenuToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuToolStrip.Name = "MainMenuToolStrip";
            this.MainMenuToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainMenuToolStrip.Size = new System.Drawing.Size(702, 25);
            this.MainMenuToolStrip.TabIndex = 2;
            this.MainMenuToolStrip.Text = "toolStrip1";
            // 
            // Options
            // 
            this.Options.AutoToolTip = false;
            this.Options.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HexEditorSettingMenuItem});
            this.Options.Image = ((System.Drawing.Image)(resources.GetObject("Options.Image")));
            this.Options.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(57, 22);
            this.Options.Text = "Options";
            // 
            // HexEditorSettingMenuItem
            // 
            this.HexEditorSettingMenuItem.BackColor = System.Drawing.SystemColors.Window;
            this.HexEditorSettingMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectFontMenuItem,
            this.SelectCodepageComboBox});
            this.HexEditorSettingMenuItem.ImageTransparentColor = System.Drawing.Color.Red;
            this.HexEditorSettingMenuItem.Name = "HexEditorSettingMenuItem";
            this.HexEditorSettingMenuItem.Size = new System.Drawing.Size(161, 22);
            this.HexEditorSettingMenuItem.Text = "Hex-editor setting";
            // 
            // SelectFontMenuItem
            // 
            this.SelectFontMenuItem.Name = "SelectFontMenuItem";
            this.SelectFontMenuItem.Size = new System.Drawing.Size(260, 22);
            this.SelectFontMenuItem.Text = "Select font";
            this.SelectFontMenuItem.Click += new System.EventHandler(this.SetFontMenuItem_Click);
            // 
            // SelectCodepageComboBox
            // 
            this.SelectCodepageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectCodepageComboBox.DropDownWidth = 200;
            this.SelectCodepageComboBox.Name = "SelectCodepageComboBox";
            this.SelectCodepageComboBox.Size = new System.Drawing.Size(200, 21);
            this.SelectCodepageComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectCodepageComboBox_SelectedIndexChanged);
            this.SelectCodepageComboBox.DropDownClosed += new System.EventHandler(this.codepageSelectStripComboBox_DropDownClosed);
            // 
            // addInfo
            // 
            this.addInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addInfo.Image = ((System.Drawing.Image)(resources.GetObject("addInfo.Image")));
            this.addInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addInfo.Name = "addInfo";
            this.addInfo.Size = new System.Drawing.Size(31, 22);
            this.addInfo.Text = "Info";
            this.addInfo.Click += new System.EventHandler(this.addInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // AddAT88Button
            // 
            this.AddAT88Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddAT88Button.Image = ((System.Drawing.Image)(resources.GetObject("AddAT88Button.Image")));
            this.AddAT88Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddAT88Button.Name = "AddAT88Button";
            this.AddAT88Button.Size = new System.Drawing.Size(36, 22);
            this.AddAT88Button.Text = "AT88";
            this.AddAT88Button.ToolTipText = "Add new AT88 dump window";
            this.AddAT88Button.Click += new System.EventHandler(this.AddAT88_Click);
            // 
            // Add24CXXButton
            // 
            this.Add24CXXButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Add24CXXButton.Image = ((System.Drawing.Image)(resources.GetObject("Add24CXXButton.Image")));
            this.Add24CXXButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Add24CXXButton.Name = "Add24CXXButton";
            this.Add24CXXButton.Size = new System.Drawing.Size(42, 22);
            this.Add24CXXButton.Text = "24CXX";
            this.Add24CXXButton.ToolTipText = "Add new 24CXX dump window";
            this.Add24CXXButton.Click += new System.EventHandler(this.Add24CXX_Click);
            // 
            // addXerox0190Button
            // 
            this.addXerox0190Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addXerox0190Button.Image = ((System.Drawing.Image)(resources.GetObject("addXerox0190Button.Image")));
            this.addXerox0190Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addXerox0190Button.Name = "addXerox0190Button";
            this.addXerox0190Button.Size = new System.Drawing.Size(70, 22);
            this.addXerox0190Button.Text = "Xerox 01/90";
            this.addXerox0190Button.Click += new System.EventHandler(this.addXerox0190Button_Click);
            // 
            // add921Button
            // 
            this.add921Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.add921Button.Image = ((System.Drawing.Image)(resources.GetObject("add921Button.Image")));
            this.add921Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add921Button.Name = "add921Button";
            this.add921Button.Size = new System.Drawing.Size(29, 22);
            this.add921Button.Text = "921";
            this.add921Button.Click += new System.EventHandler(this.add921Button_Click);
            // 
            // add1WireButton
            // 
            this.add1WireButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.add1WireButton.Image = ((System.Drawing.Image)(resources.GetObject("add1WireButton.Image")));
            this.add1WireButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add1WireButton.Name = "add1WireButton";
            this.add1WireButton.Size = new System.Drawing.Size(62, 22);
            this.add1WireButton.Text = "ONE WIRE";
            this.add1WireButton.Click += new System.EventHandler(this.add1WireButton_Click);
            // 
            // addRFIDButton
            // 
            this.addRFIDButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addRFIDButton.Image = ((System.Drawing.Image)(resources.GetObject("addRFIDButton.Image")));
            this.addRFIDButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addRFIDButton.Name = "addRFIDButton";
            this.addRFIDButton.Size = new System.Drawing.Size(53, 22);
            this.addRFIDButton.Text = "RFID125";
            this.addRFIDButton.Click += new System.EventHandler(this.addRFIDButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // addAutoButton
            // 
            this.addAutoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addAutoButton.Image = ((System.Drawing.Image)(resources.GetObject("addAutoButton.Image")));
            this.addAutoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addAutoButton.Name = "addAutoButton";
            this.addAutoButton.Size = new System.Drawing.Size(34, 22);
            this.addAutoButton.Text = "Auto";
            this.addAutoButton.Click += new System.EventHandler(this.addAuto_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 482);
            this.Controls.Add(this.MainMenuToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "Multi CRUMs Programmer V1.8 Special Edition";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Click += new System.EventHandler(this.addXerox0190Button_Click);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MainMenuToolStrip.ResumeLayout(false);
            this.MainMenuToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainMenuToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton Options;
        private System.Windows.Forms.ToolStripButton AddAT88Button;
        private System.Windows.Forms.ToolStripButton Add24CXXButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem HexEditorSettingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectFontMenuItem;
        private System.Windows.Forms.FontDialog HexEditorFontDialog;
        private System.Windows.Forms.ToolStripComboBox SelectCodepageComboBox;
        private System.Windows.Forms.ToolStripButton addAutoButton;
        private System.Windows.Forms.ToolStripButton addInfo;
        private System.Windows.Forms.ToolStripButton addXerox0190Button;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton add921Button;
        private System.Windows.Forms.ToolStripButton add1WireButton;
        private System.Windows.Forms.ToolStripButton addRFIDButton;


    }
}

