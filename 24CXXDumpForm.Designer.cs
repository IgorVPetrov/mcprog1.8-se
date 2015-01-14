using System;
using System.Windows.Forms;

using Be.Windows.Forms;
namespace mcprog
{
    partial class M24CXXDumpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(M24CXXDumpForm));
            this.EditorToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chipSelectStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.readChipStripButton = new System.Windows.Forms.ToolStripButton();
            this.writeChipStripButton = new System.Windows.Forms.ToolStripButton();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.fileStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.EditorToolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.EditorToolStripContainer.ContentPanel.SuspendLayout();
            this.EditorToolStripContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditorToolStripContainer
            // 
            // 
            // EditorToolStripContainer.BottomToolStripPanel
            // 
            this.EditorToolStripContainer.BottomToolStripPanel.Controls.Add(this.toolStrip1);
            this.EditorToolStripContainer.BottomToolStripPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.EditorToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // EditorToolStripContainer.ContentPanel
            // 
            this.EditorToolStripContainer.ContentPanel.Controls.Add(this.infoPanel);
            this.EditorToolStripContainer.ContentPanel.Controls.Add(this.hexBox1);
            this.EditorToolStripContainer.ContentPanel.Size = new System.Drawing.Size(617, 276);
            this.EditorToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorToolStripContainer.LeftToolStripPanelVisible = false;
            this.EditorToolStripContainer.Location = new System.Drawing.Point(0, 25);
            this.EditorToolStripContainer.Name = "EditorToolStripContainer";
            this.EditorToolStripContainer.RightToolStripPanelVisible = false;
            this.EditorToolStripContainer.Size = new System.Drawing.Size(617, 276);
            this.EditorToolStripContainer.TabIndex = 1;
            this.EditorToolStripContainer.Text = "toolStripContainer1";
            this.EditorToolStripContainer.TopToolStripPanelVisible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.toolStrip1.Location = new System.Drawing.Point(23, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(114, 50);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 50);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // chipSelectStripComboBox
            // 
            this.chipSelectStripComboBox.AutoSize = false;
            this.chipSelectStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chipSelectStripComboBox.Name = "chipSelectStripComboBox";
            this.chipSelectStripComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chipSelectStripComboBox.Size = new System.Drawing.Size(60, 21);
            this.chipSelectStripComboBox.ToolTipText = "Select chip";
            this.chipSelectStripComboBox.SelectedIndexChanged += new System.EventHandler(this.chipSelectStripComboBox_SelectedIndexChanged);
            this.chipSelectStripComboBox.DropDownClosed += new System.EventHandler(this.chipSelectStripComboBox_DropDownClosed);
            // 
            // readChipStripButton
            // 
            this.readChipStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readChipStripButton.Image = ((System.Drawing.Image)(resources.GetObject("readChipStripButton.Image")));
            this.readChipStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readChipStripButton.Name = "readChipStripButton";
            this.readChipStripButton.Size = new System.Drawing.Size(36, 22);
            this.readChipStripButton.Text = "Read";
            this.readChipStripButton.Click += new System.EventHandler(this.readChipStripButton_Click);
            // 
            // writeChipStripButton
            // 
            this.writeChipStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.writeChipStripButton.Image = ((System.Drawing.Image)(resources.GetObject("writeChipStripButton.Image")));
            this.writeChipStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.writeChipStripButton.Name = "writeChipStripButton";
            this.writeChipStripButton.Size = new System.Drawing.Size(37, 22);
            this.writeChipStripButton.Text = "Write";
            this.writeChipStripButton.Click += new System.EventHandler(this.writeChipStripButton_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileStripButton,
            this.toolStripSeparator1,
            this.chipSelectStripComboBox,
            this.readChipStripButton,
            this.writeChipStripButton,
            this.infoStripLabel,
            this.toolStripSeparator2});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainToolStrip.Size = new System.Drawing.Size(617, 25);
            this.mainToolStrip.TabIndex = 0;
            // 
            // fileStripButton
            // 
            this.fileStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.saveFileMenuItem});
            this.fileStripButton.Image = ((System.Drawing.Image)(resources.GetObject("fileStripButton.Image")));
            this.fileStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileStripButton.Name = "fileStripButton";
            this.fileStripButton.Size = new System.Drawing.Size(36, 22);
            this.fileStripButton.Text = "File";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(117, 22);
            this.openFileMenuItem.Text = "Open file";
            this.openFileMenuItem.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // saveFileMenuItem
            // 
            this.saveFileMenuItem.Name = "saveFileMenuItem";
            this.saveFileMenuItem.Size = new System.Drawing.Size(117, 22);
            this.saveFileMenuItem.Text = "Save file";
            this.saveFileMenuItem.Click += new System.EventHandler(this.saveFileMenuItem_Click);
            // 
            // infoStripLabel
            // 
            this.infoStripLabel.ForeColor = System.Drawing.Color.Red;
            this.infoStripLabel.Name = "infoStripLabel";
            this.infoStripLabel.Size = new System.Drawing.Size(78, 22);
            this.infoStripLabel.Text = "toolStripLabel1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            this.infoPanel.Location = new System.Drawing.Point(222, 87);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 1;
            // 
            // hexBox1
            // 
            this.hexBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox1.Encoding = ((System.Text.Encoding)(resources.GetObject("hexBox1.Encoding")));
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.LineInfoDigitsCnt = 2;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(0, 0);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(617, 276);
            this.hexBox1.StartAddress = ((long)(0));
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 0;
            this.hexBox1.UseFixedBytesPerLine = true;
            
            this.hexBox1.VScrollBarVisible = true;
            // 
            // M24CXXDumpForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 301);
            this.Controls.Add(this.EditorToolStripContainer);
            this.Controls.Add(this.mainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "M24CXXDumpForm";
            this.Text = "M24CXX DUMP";
            this.Load += new System.EventHandler(this.M24CXXDumpForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.M24CXXDumpForm_DragDrop);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.M24CXXDumpForm_FormClosed);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.M24CXXDumpForm_DragOver);
            this.EditorToolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.EditorToolStripContainer.BottomToolStripPanel.PerformLayout();
            this.EditorToolStripContainer.ContentPanel.ResumeLayout(false);
            this.EditorToolStripContainer.ContentPanel.PerformLayout();
            this.EditorToolStripContainer.ResumeLayout(false);
            this.EditorToolStripContainer.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer EditorToolStripContainer;
        private Be.Windows.Forms.HexBox hexBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox chipSelectStripComboBox;
        private System.Windows.Forms.ToolStripButton readChipStripButton;
        private System.Windows.Forms.ToolStripButton writeChipStripButton;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        
        private ToolStripLabel infoStripLabel;
        private ToolStripDropDownButton fileStripButton;
        private ToolStripMenuItem openFileMenuItem;
        private ToolStripMenuItem saveFileMenuItem;
        private InfoPanel infoPanel;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private ToolStrip toolStrip1;
        private ToolStripTextBox toolStripTextBox1;
    }
}