namespace mcprog
{
    partial class RFIDDumpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RFIDDumpForm));
            this.adjustButton = new System.Windows.Forms.Button();
            this.readButton = new System.Windows.Forms.Button();
            this.codingGroupBox = new System.Windows.Forms.GroupBox();
            this.biphaseRadioButton = new System.Windows.Forms.RadioButton();
            this.manchesterRadioButton = new System.Windows.Forms.RadioButton();
            this.datarateGroupBox = new System.Windows.Forms.GroupBox();
            this.rf64datarateRadioButton = new System.Windows.Forms.RadioButton();
            this.rf32datarateRadioButton = new System.Windows.Forms.RadioButton();
            this.rf16datarateRadioButton = new System.Windows.Forms.RadioButton();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.hexBox = new Be.Windows.Forms.HexBox();
            this.writeButton = new System.Windows.Forms.Button();
            this.openFileButton = new System.Windows.Forms.Button();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.codingGroupBox.SuspendLayout();
            this.datarateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // adjustButton
            // 
            this.adjustButton.Location = new System.Drawing.Point(379, 12);
            this.adjustButton.Name = "adjustButton";
            this.adjustButton.Size = new System.Drawing.Size(46, 42);
            this.adjustButton.TabIndex = 1;
            this.adjustButton.Text = "Test";
            this.adjustButton.UseVisualStyleBackColor = true;
            this.adjustButton.Click += new System.EventHandler(this.adjustButton_Click);
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(253, 12);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(46, 42);
            this.readButton.TabIndex = 4;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // codingGroupBox
            // 
            this.codingGroupBox.Controls.Add(this.biphaseRadioButton);
            this.codingGroupBox.Controls.Add(this.manchesterRadioButton);
            this.codingGroupBox.Location = new System.Drawing.Point(12, 69);
            this.codingGroupBox.Name = "codingGroupBox";
            this.codingGroupBox.Size = new System.Drawing.Size(93, 70);
            this.codingGroupBox.TabIndex = 5;
            this.codingGroupBox.TabStop = false;
            this.codingGroupBox.Text = "Coding";
            // 
            // biphaseRadioButton
            // 
            this.biphaseRadioButton.AutoSize = true;
            this.biphaseRadioButton.Location = new System.Drawing.Point(6, 42);
            this.biphaseRadioButton.Name = "biphaseRadioButton";
            this.biphaseRadioButton.Size = new System.Drawing.Size(66, 17);
            this.biphaseRadioButton.TabIndex = 1;
            this.biphaseRadioButton.Text = "Bi-phase";
            this.biphaseRadioButton.UseVisualStyleBackColor = true;
            // 
            // manchesterRadioButton
            // 
            this.manchesterRadioButton.AutoSize = true;
            this.manchesterRadioButton.Checked = true;
            this.manchesterRadioButton.Location = new System.Drawing.Point(6, 19);
            this.manchesterRadioButton.Name = "manchesterRadioButton";
            this.manchesterRadioButton.Size = new System.Drawing.Size(81, 17);
            this.manchesterRadioButton.TabIndex = 0;
            this.manchesterRadioButton.TabStop = true;
            this.manchesterRadioButton.Text = "Manchester";
            this.manchesterRadioButton.UseVisualStyleBackColor = true;
            // 
            // datarateGroupBox
            // 
            this.datarateGroupBox.Controls.Add(this.rf64datarateRadioButton);
            this.datarateGroupBox.Controls.Add(this.rf32datarateRadioButton);
            this.datarateGroupBox.Controls.Add(this.rf16datarateRadioButton);
            this.datarateGroupBox.Location = new System.Drawing.Point(126, 69);
            this.datarateGroupBox.Name = "datarateGroupBox";
            this.datarateGroupBox.Size = new System.Drawing.Size(74, 100);
            this.datarateGroupBox.TabIndex = 0;
            this.datarateGroupBox.TabStop = false;
            this.datarateGroupBox.Text = "Data Rate";
            // 
            // rf64datarateRadioButton
            // 
            this.rf64datarateRadioButton.AutoSize = true;
            this.rf64datarateRadioButton.Checked = true;
            this.rf64datarateRadioButton.Location = new System.Drawing.Point(7, 67);
            this.rf64datarateRadioButton.Name = "rf64datarateRadioButton";
            this.rf64datarateRadioButton.Size = new System.Drawing.Size(56, 17);
            this.rf64datarateRadioButton.TabIndex = 2;
            this.rf64datarateRadioButton.TabStop = true;
            this.rf64datarateRadioButton.Text = "RF/64";
            this.rf64datarateRadioButton.UseVisualStyleBackColor = true;
            // 
            // rf32datarateRadioButton
            // 
            this.rf32datarateRadioButton.AutoSize = true;
            this.rf32datarateRadioButton.Location = new System.Drawing.Point(7, 43);
            this.rf32datarateRadioButton.Name = "rf32datarateRadioButton";
            this.rf32datarateRadioButton.Size = new System.Drawing.Size(56, 17);
            this.rf32datarateRadioButton.TabIndex = 1;
            this.rf32datarateRadioButton.Text = "RF/32";
            this.rf32datarateRadioButton.UseVisualStyleBackColor = true;
            // 
            // rf16datarateRadioButton
            // 
            this.rf16datarateRadioButton.AutoSize = true;
            this.rf16datarateRadioButton.Location = new System.Drawing.Point(7, 19);
            this.rf16datarateRadioButton.Name = "rf16datarateRadioButton";
            this.rf16datarateRadioButton.Size = new System.Drawing.Size(56, 17);
            this.rf16datarateRadioButton.TabIndex = 0;
            this.rf16datarateRadioButton.Text = "RF/16";
            this.rf16datarateRadioButton.UseVisualStyleBackColor = true;
            // 
            // infoPanel
            // 
            this.infoPanel.AutoSize = true;
            this.infoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.infoPanel.BackColor = System.Drawing.Color.White;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.infoPanel.Location = new System.Drawing.Point(138, 90);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 6;
            this.infoPanel.TabStop = true;
            // 
            // hexBox
            // 
            this.hexBox.BytesPerLine = 5;
            this.hexBox.Encoding = null;
            this.hexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox.LineInfoDigitsCnt = 2;
            this.hexBox.LineInfoVisible = true;
            this.hexBox.Location = new System.Drawing.Point(12, 12);
            this.hexBox.Name = "hexBox";
            this.hexBox.SetPositionToZero = true;
            this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox.Size = new System.Drawing.Size(220, 42);
            this.hexBox.StartAddress = ((long)(0));
            this.hexBox.StringViewVisible = true;
            this.hexBox.TabIndex = 2;
            this.hexBox.UseFixedBytesPerLine = true;
            // 
            // writeButton
            // 
            this.writeButton.Location = new System.Drawing.Point(316, 12);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(46, 42);
            this.writeButton.TabIndex = 7;
            this.writeButton.Text = "Write";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(253, 69);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(46, 42);
            this.openFileButton.TabIndex = 8;
            this.openFileButton.Text = "Open File";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(316, 69);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(46, 42);
            this.saveFileButton.TabIndex = 9;
            this.saveFileButton.Text = "Save File";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // RFIDDumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(441, 247);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.saveFileButton);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.datarateGroupBox);
            this.Controls.Add(this.codingGroupBox);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.hexBox);
            this.Controls.Add(this.adjustButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RFIDDumpForm";
            this.Text = "EM4100 DUMP";
            this.Load += new System.EventHandler(this.RFIDDumpForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RFIDDumpForm_FormClosed);
            this.codingGroupBox.ResumeLayout(false);
            this.codingGroupBox.PerformLayout();
            this.datarateGroupBox.ResumeLayout(false);
            this.datarateGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InfoPanel infoPanel;
        private System.Windows.Forms.Button adjustButton;
        private Be.Windows.Forms.HexBox hexBox;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.GroupBox codingGroupBox;
        private System.Windows.Forms.RadioButton biphaseRadioButton;
        private System.Windows.Forms.RadioButton manchesterRadioButton;
        private System.Windows.Forms.GroupBox datarateGroupBox;
        private System.Windows.Forms.RadioButton rf16datarateRadioButton;
        private System.Windows.Forms.RadioButton rf64datarateRadioButton;
        private System.Windows.Forms.RadioButton rf32datarateRadioButton;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}