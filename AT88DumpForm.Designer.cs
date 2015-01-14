using System.Text;
using System.Globalization;
using Be.Windows.Forms;
using System.Windows.Forms;
namespace mcprog
{
    partial class AT88DumpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AT88DumpForm));
            this.OptionToolStrip = new System.Windows.Forms.ToolStrip();
            this.FileMenuButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.OpenUserDataFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveUserDataFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenConfigDataFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveConfigDataFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.readCrumButton = new System.Windows.Forms.ToolStripButton();
            this.writeCrumButton = new System.Windows.Forms.ToolStripButton();
            this.infoStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.passwordSelectComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.DumpStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dumpTabControl = new System.Windows.Forms.TabControl();
            this.userDataEditorPage = new System.Windows.Forms.TabPage();
            this.configDataEditorPage = new System.Windows.Forms.TabPage();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.userDataHexBox = new Be.Windows.Forms.HexBox();
            this.configDataHexBox = new Be.Windows.Forms.HexBox();
            this.OptionToolStrip.SuspendLayout();
            this.DumpStripContainer.ContentPanel.SuspendLayout();
            this.DumpStripContainer.SuspendLayout();
            this.dumpTabControl.SuspendLayout();
            this.userDataEditorPage.SuspendLayout();
            this.configDataEditorPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionToolStrip
            // 
            this.OptionToolStrip.AutoSize = false;
            this.OptionToolStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OptionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.OptionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuButton,
            this.toolStripSeparator1,
            this.readCrumButton,
            this.writeCrumButton,
            this.infoStripLabel,
            this.toolStripButton1,
            this.passwordSelectComboBox});
            this.OptionToolStrip.Location = new System.Drawing.Point(0, 0);
            this.OptionToolStrip.Name = "OptionToolStrip";
            this.OptionToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.OptionToolStrip.Size = new System.Drawing.Size(624, 25);
            this.OptionToolStrip.TabIndex = 1;
            this.OptionToolStrip.Text = "toolStrip1";
            // 
            // FileMenuButton
            // 
            this.FileMenuButton.AutoToolTip = false;
            this.FileMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenUserDataFileMenuItem,
            this.SaveUserDataFileMenuItem,
            this.toolStripSeparator2,
            this.OpenConfigDataFileMenuItem,
            this.SaveConfigDataFileMenuItem});
            this.FileMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("FileMenuButton.Image")));
            this.FileMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileMenuButton.Name = "FileMenuButton";
            this.FileMenuButton.Size = new System.Drawing.Size(36, 22);
            this.FileMenuButton.Text = "File";
            this.FileMenuButton.ToolTipText = "File menu";
            // 
            // OpenUserDataFileMenuItem
            // 
            this.OpenUserDataFileMenuItem.Name = "OpenUserDataFileMenuItem";
            this.OpenUserDataFileMenuItem.Size = new System.Drawing.Size(174, 22);
            this.OpenUserDataFileMenuItem.Text = "Open user data file";
            this.OpenUserDataFileMenuItem.Click += new System.EventHandler(this.OpenUserDataFileMenuItem_Click);
            // 
            // SaveUserDataFileMenuItem
            // 
            this.SaveUserDataFileMenuItem.Name = "SaveUserDataFileMenuItem";
            this.SaveUserDataFileMenuItem.Size = new System.Drawing.Size(174, 22);
            this.SaveUserDataFileMenuItem.Text = "Save user data file";
            this.SaveUserDataFileMenuItem.Click += new System.EventHandler(this.SaveUserDataFileMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(171, 6);
            // 
            // OpenConfigDataFileMenuItem
            // 
            this.OpenConfigDataFileMenuItem.Name = "OpenConfigDataFileMenuItem";
            this.OpenConfigDataFileMenuItem.Size = new System.Drawing.Size(174, 22);
            this.OpenConfigDataFileMenuItem.Text = "Open config data file";
            this.OpenConfigDataFileMenuItem.Click += new System.EventHandler(this.OpenConfigDataFileMenuItem_Click);
            // 
            // SaveConfigDataFileMenuItem
            // 
            this.SaveConfigDataFileMenuItem.Name = "SaveConfigDataFileMenuItem";
            this.SaveConfigDataFileMenuItem.Size = new System.Drawing.Size(174, 22);
            this.SaveConfigDataFileMenuItem.Text = "Save config data file";
            this.SaveConfigDataFileMenuItem.Click += new System.EventHandler(this.SaveConfigDataFileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // readCrumButton
            // 
            this.readCrumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readCrumButton.Image = ((System.Drawing.Image)(resources.GetObject("readCrumButton.Image")));
            this.readCrumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readCrumButton.Name = "readCrumButton";
            this.readCrumButton.Size = new System.Drawing.Size(36, 22);
            this.readCrumButton.Text = "Read";
            this.readCrumButton.ToolTipText = "Read CRUM";
            this.readCrumButton.Click += new System.EventHandler(this.readCrumButton_Click);
            // 
            // writeCrumButton
            // 
            this.writeCrumButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.writeCrumButton.Image = ((System.Drawing.Image)(resources.GetObject("writeCrumButton.Image")));
            this.writeCrumButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.writeCrumButton.Name = "writeCrumButton";
            this.writeCrumButton.Size = new System.Drawing.Size(37, 22);
            this.writeCrumButton.Text = "Write";
            this.writeCrumButton.ToolTipText = "Write CRUM";
            this.writeCrumButton.Click += new System.EventHandler(this.writeCrumButton_Click);
            // 
            // infoStripLabel
            // 
            this.infoStripLabel.ForeColor = System.Drawing.Color.Red;
            this.infoStripLabel.Name = "infoStripLabel";
            this.infoStripLabel.Size = new System.Drawing.Size(78, 22);
            this.infoStripLabel.Text = "toolStripLabel1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // passwordSelectComboBox
            // 
            this.passwordSelectComboBox.AutoSize = false;
            this.passwordSelectComboBox.DropDownHeight = 140;
            this.passwordSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.passwordSelectComboBox.DropDownWidth = 400;
            this.passwordSelectComboBox.IntegralHeight = false;
            this.passwordSelectComboBox.Name = "passwordSelectComboBox";
            this.passwordSelectComboBox.Size = new System.Drawing.Size(400, 21);
            this.passwordSelectComboBox.ToolTipText = "Select CRUM type";
            this.passwordSelectComboBox.DropDownClosed += new System.EventHandler(this.passwordSelectComboBox_DropDownClosed);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin|pony files(*.eep;*.e2p;*.rom)|*.e" +
                "ep;*.e2p;*.rom)";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            // 
            // DumpStripContainer
            // 
            this.DumpStripContainer.BottomToolStripPanelVisible = false;
            // 
            // DumpStripContainer.ContentPanel
            // 
            this.DumpStripContainer.ContentPanel.Controls.Add(this.infoPanel);
            this.DumpStripContainer.ContentPanel.Controls.Add(this.dumpTabControl);
            this.DumpStripContainer.ContentPanel.Size = new System.Drawing.Size(624, 289);
            this.DumpStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DumpStripContainer.LeftToolStripPanelVisible = false;
            this.DumpStripContainer.Location = new System.Drawing.Point(0, 25);
            this.DumpStripContainer.Name = "DumpStripContainer";
            this.DumpStripContainer.RightToolStripPanelVisible = false;
            this.DumpStripContainer.Size = new System.Drawing.Size(624, 289);
            this.DumpStripContainer.TabIndex = 5;
            this.DumpStripContainer.Text = "toolStripContainer1";
            this.DumpStripContainer.TopToolStripPanelVisible = false;
            // 
            // dumpTabControl
            // 
            this.dumpTabControl.Controls.Add(this.userDataEditorPage);
            this.dumpTabControl.Controls.Add(this.configDataEditorPage);
            this.dumpTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dumpTabControl.Location = new System.Drawing.Point(0, 0);
            this.dumpTabControl.Name = "dumpTabControl";
            this.dumpTabControl.SelectedIndex = 0;
            this.dumpTabControl.Size = new System.Drawing.Size(624, 289);
            this.dumpTabControl.TabIndex = 0;
            this.dumpTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.dumpTabControl_Selected);
            this.dumpTabControl.Deselected += new System.Windows.Forms.TabControlEventHandler(this.dumpTabControl_Deselected);
            // 
            // userDataEditorPage
            // 
            this.userDataEditorPage.Controls.Add(this.userDataHexBox);
            this.userDataEditorPage.Location = new System.Drawing.Point(4, 22);
            this.userDataEditorPage.Name = "userDataEditorPage";
            this.userDataEditorPage.Padding = new System.Windows.Forms.Padding(3);
            this.userDataEditorPage.Size = new System.Drawing.Size(616, 263);
            this.userDataEditorPage.TabIndex = 0;
            this.userDataEditorPage.Text = "User data";
            this.userDataEditorPage.UseVisualStyleBackColor = true;
            // 
            // configDataEditorPage
            // 
            this.configDataEditorPage.Controls.Add(this.configDataHexBox);
            this.configDataEditorPage.Location = new System.Drawing.Point(4, 22);
            this.configDataEditorPage.Name = "configDataEditorPage";
            this.configDataEditorPage.Padding = new System.Windows.Forms.Padding(3);
            this.configDataEditorPage.Size = new System.Drawing.Size(616, 263);
            this.configDataEditorPage.TabIndex = 1;
            this.configDataEditorPage.Text = "Config data";
            this.configDataEditorPage.UseVisualStyleBackColor = true;
            // 
            // infoPanel
            // 
            this.infoPanel.AutoSize = true;
            this.infoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.infoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.infoPanel.Location = new System.Drawing.Point(226, 94);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 1;
            // 
            // userDataHexBox
            // 
            this.userDataHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDataHexBox.Encoding = ((System.Text.Encoding)(resources.GetObject("userDataHexBox.Encoding")));
            this.userDataHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userDataHexBox.LineInfoDigitsCnt = 2;
            this.userDataHexBox.LineInfoVisible = true;
            this.userDataHexBox.Location = new System.Drawing.Point(3, 3);
            this.userDataHexBox.Name = "userDataHexBox";
            this.userDataHexBox.SetPositionToZero = true;
            this.userDataHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.userDataHexBox.Size = new System.Drawing.Size(610, 257);
            this.userDataHexBox.StartAddress = ((long)(0));
            this.userDataHexBox.StringViewVisible = true;
            this.userDataHexBox.TabIndex = 0;
            this.userDataHexBox.UseFixedBytesPerLine = true;
            this.userDataHexBox.VScrollBarVisible = true;
            // 
            // configDataHexBox
            // 
            this.configDataHexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configDataHexBox.Encoding = ((System.Text.Encoding)(resources.GetObject("configDataHexBox.Encoding")));
            this.configDataHexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configDataHexBox.LineInfoDigitsCnt = 2;
            this.configDataHexBox.LineInfoVisible = true;
            this.configDataHexBox.Location = new System.Drawing.Point(3, 3);
            this.configDataHexBox.Name = "configDataHexBox";
            this.configDataHexBox.ReadOnly = true;
            this.configDataHexBox.SetPositionToZero = true;
            this.configDataHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.configDataHexBox.Size = new System.Drawing.Size(610, 257);
            this.configDataHexBox.StartAddress = ((long)(0));
            this.configDataHexBox.StringViewVisible = true;
            this.configDataHexBox.TabIndex = 0;
            this.configDataHexBox.UseFixedBytesPerLine = true;
            this.configDataHexBox.VScrollBarVisible = true;
            // 
            // AT88DumpForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 314);
            this.Controls.Add(this.DumpStripContainer);
            this.Controls.Add(this.OptionToolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AT88DumpForm";
            this.Text = "AT88 DUMP Empty";
            this.Load += new System.EventHandler(this.AT88DumpForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AT88DumpForm_DragDrop);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AT88DumpForm_FormClosed);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.AT88DumpForm_DragOver);
            this.OptionToolStrip.ResumeLayout(false);
            this.OptionToolStrip.PerformLayout();
            this.DumpStripContainer.ContentPanel.ResumeLayout(false);
            this.DumpStripContainer.ContentPanel.PerformLayout();
            this.DumpStripContainer.ResumeLayout(false);
            this.DumpStripContainer.PerformLayout();
            this.dumpTabControl.ResumeLayout(false);
            this.userDataEditorPage.ResumeLayout(false);
            this.configDataEditorPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip OptionToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton FileMenuButton;
        private System.Windows.Forms.ToolStripContainer DumpStripContainer;
        private System.Windows.Forms.TabControl dumpTabControl;
        private System.Windows.Forms.TabPage userDataEditorPage;
        private System.Windows.Forms.TabPage configDataEditorPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton readCrumButton;
        private System.Windows.Forms.ToolStripButton writeCrumButton;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private Be.Windows.Forms.HexBox userDataHexBox;
        private Be.Windows.Forms.HexBox configDataHexBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem OpenUserDataFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveUserDataFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem OpenConfigDataFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveConfigDataFileMenuItem;
        private System.Windows.Forms.ToolStripComboBox passwordSelectComboBox;
        private InfoPanel infoPanel;
        private ToolStripLabel infoStripLabel;
    }
}