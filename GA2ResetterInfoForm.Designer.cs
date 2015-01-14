namespace mcprog
{
    partial class GA2ResetterInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GA2ResetterInfoForm));
            this.voltageSelectComboBox = new System.Windows.Forms.ComboBox();
            this.enablePowerCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hardwareNumberLabel = new System.Windows.Forms.Label();
            this.writesCounterLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.unlock3600 = new System.Windows.Forms.Button();
            this.unlock3635 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // voltageSelectComboBox
            // 
            this.voltageSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voltageSelectComboBox.FormattingEnabled = true;
            this.voltageSelectComboBox.Location = new System.Drawing.Point(12, 12);
            this.voltageSelectComboBox.Name = "voltageSelectComboBox";
            this.voltageSelectComboBox.Size = new System.Drawing.Size(73, 21);
            this.voltageSelectComboBox.TabIndex = 0;
            // 
            // enablePowerCheckBox
            // 
            this.enablePowerCheckBox.AutoSize = true;
            this.enablePowerCheckBox.Location = new System.Drawing.Point(12, 49);
            this.enablePowerCheckBox.Name = "enablePowerCheckBox";
            this.enablePowerCheckBox.Size = new System.Drawing.Size(114, 17);
            this.enablePowerCheckBox.TabIndex = 1;
            this.enablePowerCheckBox.Text = "Test CRUM power";
            this.enablePowerCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Hardware number :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Writes counter :";
            // 
            // hardwareNumberLabel
            // 
            this.hardwareNumberLabel.AutoSize = true;
            this.hardwareNumberLabel.Location = new System.Drawing.Point(124, 78);
            this.hardwareNumberLabel.Name = "hardwareNumberLabel";
            this.hardwareNumberLabel.Size = new System.Drawing.Size(35, 13);
            this.hardwareNumberLabel.TabIndex = 4;
            this.hardwareNumberLabel.Text = "label3";
            // 
            // writesCounterLabel
            // 
            this.writesCounterLabel.AutoSize = true;
            this.writesCounterLabel.Location = new System.Drawing.Point(124, 102);
            this.writesCounterLabel.Name = "writesCounterLabel";
            this.writesCounterLabel.Size = new System.Drawing.Size(35, 13);
            this.writesCounterLabel.TabIndex = 5;
            this.writesCounterLabel.Text = "label3";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(145, 12);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 6;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // unlock3600
            // 
            this.unlock3600.Location = new System.Drawing.Point(12, 140);
            this.unlock3600.Name = "unlock3600";
            this.unlock3600.Size = new System.Drawing.Size(208, 23);
            this.unlock3600.TabIndex = 7;
            this.unlock3600.Text = "Change protection Xerox Phaser 3600";
            this.unlock3600.UseVisualStyleBackColor = true;
            // 
            // unlock3635
            // 
            this.unlock3635.Location = new System.Drawing.Point(12, 175);
            this.unlock3635.Name = "unlock3635";
            this.unlock3635.Size = new System.Drawing.Size(208, 23);
            this.unlock3635.TabIndex = 8;
            this.unlock3635.Text = "Change protection Xerox Phaser 3635";
            this.unlock3635.UseVisualStyleBackColor = true;
            // 
            // GA2ResetterInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(232, 210);
            this.Controls.Add(this.unlock3635);
            this.Controls.Add(this.unlock3600);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.writesCounterLabel);
            this.Controls.Add(this.hardwareNumberLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enablePowerCheckBox);
            this.Controls.Add(this.voltageSelectComboBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GA2ResetterInfoForm";
            this.Text = "Resetter control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox voltageSelectComboBox;
        public System.Windows.Forms.CheckBox enablePowerCheckBox;
        public System.Windows.Forms.Label hardwareNumberLabel;
        public System.Windows.Forms.Label writesCounterLabel;
        public System.Windows.Forms.Button resetButton;
        public System.Windows.Forms.Button unlock3600;
        public System.Windows.Forms.Button unlock3635;
    }
}