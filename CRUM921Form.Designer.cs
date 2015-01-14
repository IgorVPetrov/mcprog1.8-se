namespace mcprog
{
    partial class CRUM921Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRUM921Form));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.readChipStripButton = new System.Windows.Forms.ToolStripButton();
            this.writeChipStripButton = new System.Windows.Forms.ToolStripButton();
            this.infoStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.hexBoxStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.hexBox = new Be.Windows.Forms.HexBox();
            this.toolStrip1.SuspendLayout();
            this.hexBoxStripContainer.ContentPanel.SuspendLayout();
            this.hexBoxStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileStripButton,
            this.toolStripSeparator1,
            this.readChipStripButton,
            this.writeChipStripButton,
            this.infoStripLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(618, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileStripButton
            // 
            this.fileStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.saveFileMenuItem});
            this.fileStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileStripButton.Name = "fileStripButton";
            this.fileStripButton.Size = new System.Drawing.Size(36, 22);
            this.fileStripButton.Text = "File";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(128, 22);
            this.openFileMenuItem.Text = "Open file";
            this.openFileMenuItem.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // saveFileMenuItem
            // 
            this.saveFileMenuItem.Name = "saveFileMenuItem";
            this.saveFileMenuItem.Size = new System.Drawing.Size(128, 22);
            this.saveFileMenuItem.Text = "Save file";
            this.saveFileMenuItem.Click += new System.EventHandler(this.saveFileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // readChipStripButton
            // 
            this.readChipStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readChipStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readChipStripButton.Name = "readChipStripButton";
            this.readChipStripButton.Size = new System.Drawing.Size(36, 22);
            this.readChipStripButton.Text = "Read";
            this.readChipStripButton.Click += new System.EventHandler(this.readChipStripButton_Click);
            // 
            // writeChipStripButton
            // 
            this.writeChipStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.writeChipStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.writeChipStripButton.Name = "writeChipStripButton";
            this.writeChipStripButton.Size = new System.Drawing.Size(37, 22);
            this.writeChipStripButton.Text = "Write";
            this.writeChipStripButton.Click += new System.EventHandler(this.writeChipStripButton_Click);
            // 
            // infoStripLabel
            // 
            this.infoStripLabel.ForeColor = System.Drawing.Color.Red;
            this.infoStripLabel.Name = "infoStripLabel";
            this.infoStripLabel.Size = new System.Drawing.Size(78, 22);
            this.infoStripLabel.Text = "toolStripLabel1";
            // 
            // hexBoxStripContainer
            // 
            this.hexBoxStripContainer.BottomToolStripPanelVisible = false;
            // 
            // hexBoxStripContainer.ContentPanel
            // 
            this.hexBoxStripContainer.ContentPanel.Controls.Add(this.infoPanel);
            this.hexBoxStripContainer.ContentPanel.Controls.Add(this.hexBox);
            this.hexBoxStripContainer.ContentPanel.Size = new System.Drawing.Size(618, 361);
            this.hexBoxStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBoxStripContainer.LeftToolStripPanelVisible = false;
            this.hexBoxStripContainer.Location = new System.Drawing.Point(0, 25);
            this.hexBoxStripContainer.Name = "hexBoxStripContainer";
            this.hexBoxStripContainer.RightToolStripPanelVisible = false;
            this.hexBoxStripContainer.Size = new System.Drawing.Size(618, 361);
            this.hexBoxStripContainer.TabIndex = 1;
            this.hexBoxStripContainer.Text = "toolStripContainer1";
            this.hexBoxStripContainer.TopToolStripPanelVisible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // infoPanel
            // 
            this.infoPanel.AutoSize = true;
            this.infoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.infoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.infoPanel.Location = new System.Drawing.Point(223, 130);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 1;
            // 
            // hexBox
            // 
            this.hexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox.Encoding = null;
            this.hexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox.LineInfoForeColor = System.Drawing.Color.Brown;
            this.hexBox.LineInfoVisible = true;
            this.hexBox.Location = new System.Drawing.Point(0, 0);
            this.hexBox.Name = "hexBox";
            this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox.Size = new System.Drawing.Size(618, 361);
            this.hexBox.StartAddress = ((long)(0));
            this.hexBox.StringViewVisible = true;
            this.hexBox.TabIndex = 0;
            this.hexBox.UseFixedBytesPerLine = true;
            this.hexBox.VScrollBarVisible = true;
            // 
            // CRUM921Form
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 386);
            this.Controls.Add(this.hexBoxStripContainer);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CRUM921Form";
            this.Text = "921 CRUM DUMP";
            this.Load += new System.EventHandler(this.CRUM921Form_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CRUM921Form_DragDrop);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CRUM921Form_FormClosed);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.CRUM921Form_DragOver);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.hexBoxStripContainer.ContentPanel.ResumeLayout(false);
            this.hexBoxStripContainer.ContentPanel.PerformLayout();
            this.hexBoxStripContainer.ResumeLayout(false);
            this.hexBoxStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripContainer hexBoxStripContainer;
        private Be.Windows.Forms.HexBox hexBox;
        private InfoPanel infoPanel;
        private System.Windows.Forms.ToolStripDropDownButton fileStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton readChipStripButton;
        private System.Windows.Forms.ToolStripButton writeChipStripButton;
        private System.Windows.Forms.ToolStripLabel infoStripLabel;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}