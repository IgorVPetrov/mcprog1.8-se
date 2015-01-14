using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;


namespace mcprog
{
    partial class OneWireDumpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneWireDumpForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.iButtonPage = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.rw1990_Verify_Btn = new System.Windows.Forms.Button();
            this.invertWriteBytesCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCorrectDumpCheckBox = new System.Windows.Forms.CheckBox();
            this.saveFile_Btn = new System.Windows.Forms.Button();
            this.openFile_Btn = new System.Windows.Forms.Button();
            this.rw1990_Write_Button = new System.Windows.Forms.Button();
            this.rw1990_Read_Button = new System.Windows.Forms.Button();
            this.ds1820Page = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ds1820_READ_TEMP_Button = new System.Windows.Forms.Button();
            this.ds1820_TEMP_Label = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ds1820_WRITE_EE_Button = new System.Windows.Forms.Button();
            this.ds1820_READ_EE_Button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ds1820_COPY_SP_Button = new System.Windows.Forms.Button();
            this.ds1820_WRITE_SP_Button = new System.Windows.Forms.Button();
            this.ds1820_READ_SP_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ds1820_READ_SC_Button = new System.Windows.Forms.Button();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.gen_READ_DATA_Button = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gen_READ_SC_Button = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.infoPanel = new mcprog.InfoPanel(this.components);
            this.rw1990_HexBox = new Be.Windows.Forms.HexBox();
            this.ds1820_EEPROM_HexBox = new Be.Windows.Forms.HexBox();
            this.ds1820_SCRATCHPAD_HexBox = new Be.Windows.Forms.HexBox();
            this.ds1820_SN_HexBox = new Be.Windows.Forms.HexBox();
            this.gen_DATA_HexBox = new Be.Windows.Forms.HexBox();
            this.gen_SC_HexBox = new Be.Windows.Forms.HexBox();
            this.tabControl.SuspendLayout();
            this.iButtonPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.ds1820Page.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.iButtonPage);
            this.tabControl.Controls.Add(this.ds1820Page);
            this.tabControl.Controls.Add(this.generalPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(561, 357);
            this.tabControl.TabIndex = 1;
            // 
            // iButtonPage
            // 
            
            this.iButtonPage.Controls.Add(this.groupBox6);
            this.iButtonPage.Location = new System.Drawing.Point(4, 22);
            this.iButtonPage.Name = "iButtonPage";
            this.iButtonPage.Padding = new System.Windows.Forms.Padding(3);
            this.iButtonPage.Size = new System.Drawing.Size(553, 331);
            this.iButtonPage.TabIndex = 1;
            this.iButtonPage.Text = "iButton";
            this.iButtonPage.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.rw1990_Verify_Btn);
            this.groupBox6.Controls.Add(this.invertWriteBytesCheckBox);
            this.groupBox6.Controls.Add(this.openFile_Btn);
            this.groupBox6.Controls.Add(this.autoCorrectDumpCheckBox);
            this.groupBox6.Controls.Add(this.saveFile_Btn);
            this.groupBox6.Controls.Add(this.rw1990_Write_Button);
            this.groupBox6.Controls.Add(this.rw1990_Read_Button);
            this.groupBox6.Controls.Add(this.rw1990_HexBox);
            this.groupBox6.Location = new System.Drawing.Point(11, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(528, 141);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "RW1990";
            // 
            // rw1990_Verify_Btn
            // 
            this.rw1990_Verify_Btn.Location = new System.Drawing.Point(463, 19);
            this.rw1990_Verify_Btn.Name = "rw1990_Verify_Btn";
            this.rw1990_Verify_Btn.Size = new System.Drawing.Size(46, 42);
            this.rw1990_Verify_Btn.TabIndex = 8;
            this.rw1990_Verify_Btn.Text = "Verify";
            this.rw1990_Verify_Btn.UseVisualStyleBackColor = true;
            this.rw1990_Verify_Btn.Click += new System.EventHandler(this.rw1990_Verify_Btn_Click);
            // 
            // invertWriteBytesCheckBox
            // 
            this.invertWriteBytesCheckBox.AutoSize = true;
            this.invertWriteBytesCheckBox.Checked = true;
            this.invertWriteBytesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.invertWriteBytesCheckBox.Location = new System.Drawing.Point(6, 104);
            this.invertWriteBytesCheckBox.Name = "invertWriteBytesCheckBox";
            this.invertWriteBytesCheckBox.Size = new System.Drawing.Size(124, 17);
            this.invertWriteBytesCheckBox.TabIndex = 7;
            this.invertWriteBytesCheckBox.Text = "To invert write bytes ";
            this.invertWriteBytesCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoCorrectDumpCheckBox
            // 
            this.autoCorrectDumpCheckBox.AutoSize = true;
            this.autoCorrectDumpCheckBox.Checked = true;
            this.autoCorrectDumpCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCorrectDumpCheckBox.Location = new System.Drawing.Point(6, 79);
            this.autoCorrectDumpCheckBox.Name = "autoCorrectDumpCheckBox";
            this.autoCorrectDumpCheckBox.Size = new System.Drawing.Size(139, 17);
            this.autoCorrectDumpCheckBox.TabIndex = 6;
            this.autoCorrectDumpCheckBox.Text = "Auto correction of dump";
            this.autoCorrectDumpCheckBox.UseVisualStyleBackColor = true;
            this.autoCorrectDumpCheckBox.CheckedChanged += new System.EventHandler(this.autoCorrectDumpCheckBox_CheckedChanged);
            // 
            // saveFile_Btn
            // 
            this.saveFile_Btn.Location = new System.Drawing.Point(400, 79);
            this.saveFile_Btn.Name = "saveFile_Btn";
            this.saveFile_Btn.Size = new System.Drawing.Size(46, 42);
            this.saveFile_Btn.TabIndex = 5;
            this.saveFile_Btn.Text = "Save File";
            this.saveFile_Btn.UseVisualStyleBackColor = true;
            this.saveFile_Btn.Click += new System.EventHandler(this.saveFile_Btn_Click);
            // 
            // openFile_Btn
            // 
            this.openFile_Btn.Location = new System.Drawing.Point(338, 79);
            this.openFile_Btn.Name = "openFile_Btn";
            this.openFile_Btn.Size = new System.Drawing.Size(46, 42);
            this.openFile_Btn.TabIndex = 4;
            this.openFile_Btn.Text = "Open File";
            this.openFile_Btn.UseVisualStyleBackColor = true;
            this.openFile_Btn.Click += new System.EventHandler(this.openFile_Btn_Click);
            // 
            // rw1990_Write_Button
            // 
            this.rw1990_Write_Button.Location = new System.Drawing.Point(400, 19);
            this.rw1990_Write_Button.Name = "rw1990_Write_Button";
            this.rw1990_Write_Button.Size = new System.Drawing.Size(46, 42);
            this.rw1990_Write_Button.TabIndex = 3;
            this.rw1990_Write_Button.Text = "Write";
            this.rw1990_Write_Button.UseVisualStyleBackColor = true;
            this.rw1990_Write_Button.Click += new System.EventHandler(this.rw1990_Write_Button_Click);
            // 
            // rw1990_Read_Button
            // 
            this.rw1990_Read_Button.Location = new System.Drawing.Point(338, 19);
            this.rw1990_Read_Button.Name = "rw1990_Read_Button";
            this.rw1990_Read_Button.Size = new System.Drawing.Size(46, 42);
            this.rw1990_Read_Button.TabIndex = 1;
            this.rw1990_Read_Button.Text = "Read";
            this.rw1990_Read_Button.UseVisualStyleBackColor = true;
            this.rw1990_Read_Button.Click += new System.EventHandler(this.rw1990_Read_Button_Click);
            // 
            // ds1820Page
            // 
            this.ds1820Page.Controls.Add(this.groupBox4);
            this.ds1820Page.Controls.Add(this.groupBox3);
            this.ds1820Page.Controls.Add(this.groupBox2);
            this.ds1820Page.Controls.Add(this.groupBox1);
            this.ds1820Page.Location = new System.Drawing.Point(4, 22);
            this.ds1820Page.Name = "ds1820Page";
            this.ds1820Page.Padding = new System.Windows.Forms.Padding(3);
            this.ds1820Page.Size = new System.Drawing.Size(553, 331);
            this.ds1820Page.TabIndex = 1;
            this.ds1820Page.Text = "DS1820";
            this.ds1820Page.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ds1820_READ_TEMP_Button);
            this.groupBox4.Controls.Add(this.ds1820_TEMP_Label);
            this.groupBox4.Location = new System.Drawing.Point(315, 176);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 75);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TEMPERATURE";
            // 
            // ds1820_READ_TEMP_Button
            // 
            this.ds1820_READ_TEMP_Button.Location = new System.Drawing.Point(154, 19);
            this.ds1820_READ_TEMP_Button.Name = "ds1820_READ_TEMP_Button";
            this.ds1820_READ_TEMP_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_READ_TEMP_Button.TabIndex = 1;
            this.ds1820_READ_TEMP_Button.Text = "Read";
            this.ds1820_READ_TEMP_Button.UseVisualStyleBackColor = true;
            this.ds1820_READ_TEMP_Button.Click += new System.EventHandler(this.ds1820_READ_TEMP_Button_Click);
            // 
            // ds1820_TEMP_Label
            // 
            this.ds1820_TEMP_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ds1820_TEMP_Label.Location = new System.Drawing.Point(17, 19);
            this.ds1820_TEMP_Label.Name = "ds1820_TEMP_Label";
            this.ds1820_TEMP_Label.Size = new System.Drawing.Size(122, 42);
            this.ds1820_TEMP_Label.TabIndex = 0;
            this.ds1820_TEMP_Label.Text = "?";
            this.ds1820_TEMP_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ds1820_WRITE_EE_Button);
            this.groupBox3.Controls.Add(this.ds1820_READ_EE_Button);
            this.groupBox3.Controls.Add(this.ds1820_EEPROM_HexBox);
            this.groupBox3.Location = new System.Drawing.Point(11, 176);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 75);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "EEPROM";
            // 
            // ds1820_WRITE_EE_Button
            // 
            this.ds1820_WRITE_EE_Button.Location = new System.Drawing.Point(204, 19);
            this.ds1820_WRITE_EE_Button.Name = "ds1820_WRITE_EE_Button";
            this.ds1820_WRITE_EE_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_WRITE_EE_Button.TabIndex = 2;
            this.ds1820_WRITE_EE_Button.Text = "Write\r\n";
            this.ds1820_WRITE_EE_Button.UseVisualStyleBackColor = true;
            this.ds1820_WRITE_EE_Button.Click += new System.EventHandler(this.ds1820_WRITE_EE_Button_Click);
            // 
            // ds1820_READ_EE_Button
            // 
            this.ds1820_READ_EE_Button.Location = new System.Drawing.Point(143, 19);
            this.ds1820_READ_EE_Button.Name = "ds1820_READ_EE_Button";
            this.ds1820_READ_EE_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_READ_EE_Button.TabIndex = 1;
            this.ds1820_READ_EE_Button.Text = "Read";
            this.ds1820_READ_EE_Button.UseVisualStyleBackColor = true;
            this.ds1820_READ_EE_Button.Click += new System.EventHandler(this.ds1820_READ_EE_Button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ds1820_COPY_SP_Button);
            this.groupBox2.Controls.Add(this.ds1820_WRITE_SP_Button);
            this.groupBox2.Controls.Add(this.ds1820_READ_SP_Button);
            this.groupBox2.Controls.Add(this.ds1820_SCRATCHPAD_HexBox);
            this.groupBox2.Location = new System.Drawing.Point(11, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(521, 74);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SCRATCHPAD";
            // 
            // ds1820_COPY_SP_Button
            // 
            this.ds1820_COPY_SP_Button.Location = new System.Drawing.Point(458, 19);
            this.ds1820_COPY_SP_Button.Name = "ds1820_COPY_SP_Button";
            this.ds1820_COPY_SP_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_COPY_SP_Button.TabIndex = 3;
            this.ds1820_COPY_SP_Button.Text = "Copy";
            this.ds1820_COPY_SP_Button.UseVisualStyleBackColor = true;
            this.ds1820_COPY_SP_Button.Click += new System.EventHandler(this.ds1820_COPY_SP_Button_Click);
            // 
            // ds1820_WRITE_SP_Button
            // 
            this.ds1820_WRITE_SP_Button.Location = new System.Drawing.Point(397, 19);
            this.ds1820_WRITE_SP_Button.Name = "ds1820_WRITE_SP_Button";
            this.ds1820_WRITE_SP_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_WRITE_SP_Button.TabIndex = 2;
            this.ds1820_WRITE_SP_Button.Text = "Write";
            this.ds1820_WRITE_SP_Button.UseVisualStyleBackColor = true;
            this.ds1820_WRITE_SP_Button.Click += new System.EventHandler(this.ds1820_WRITE_SP_Button_Click);
            // 
            // ds1820_READ_SP_Button
            // 
            this.ds1820_READ_SP_Button.Location = new System.Drawing.Point(336, 19);
            this.ds1820_READ_SP_Button.Name = "ds1820_READ_SP_Button";
            this.ds1820_READ_SP_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_READ_SP_Button.TabIndex = 1;
            this.ds1820_READ_SP_Button.Text = "Read";
            this.ds1820_READ_SP_Button.UseVisualStyleBackColor = true;
            this.ds1820_READ_SP_Button.Click += new System.EventHandler(this.ds1820_READ_SP_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ds1820_SN_HexBox);
            this.groupBox1.Controls.Add(this.ds1820_READ_SC_Button);
            this.groupBox1.Location = new System.Drawing.Point(11, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 74);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SERIAL CODE";
            // 
            // ds1820_READ_SC_Button
            // 
            this.ds1820_READ_SC_Button.Location = new System.Drawing.Point(336, 19);
            this.ds1820_READ_SC_Button.Name = "ds1820_READ_SC_Button";
            this.ds1820_READ_SC_Button.Size = new System.Drawing.Size(46, 42);
            this.ds1820_READ_SC_Button.TabIndex = 1;
            this.ds1820_READ_SC_Button.Text = "Read";
            this.ds1820_READ_SC_Button.UseVisualStyleBackColor = true;
            this.ds1820_READ_SC_Button.Click += new System.EventHandler(this.ds1820_READ_SC_Button_Click);
            // 
            // generalPage
            // 
            this.generalPage.Controls.Add(this.gen_READ_DATA_Button);
            this.generalPage.Controls.Add(this.gen_DATA_HexBox);
            this.generalPage.Controls.Add(this.groupBox5);
            this.generalPage.Location = new System.Drawing.Point(4, 22);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(553, 331);
            this.generalPage.TabIndex = 2;
            this.generalPage.Text = "DS2432";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // gen_READ_DATA_Button
            // 
            this.gen_READ_DATA_Button.Location = new System.Drawing.Point(11, 102);
            this.gen_READ_DATA_Button.Name = "gen_READ_DATA_Button";
            this.gen_READ_DATA_Button.Size = new System.Drawing.Size(46, 42);
            this.gen_READ_DATA_Button.TabIndex = 3;
            this.gen_READ_DATA_Button.Text = "Read";
            this.gen_READ_DATA_Button.UseVisualStyleBackColor = true;
            this.gen_READ_DATA_Button.Click += new System.EventHandler(this.gen_READ_DATA_Button_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gen_READ_SC_Button);
            this.groupBox5.Controls.Add(this.gen_SC_HexBox);
            this.groupBox5.Location = new System.Drawing.Point(11, 16);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(398, 74);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "SERIAL CODE";
            // 
            // gen_READ_SC_Button
            // 
            this.gen_READ_SC_Button.Location = new System.Drawing.Point(336, 19);
            this.gen_READ_SC_Button.Name = "gen_READ_SC_Button";
            this.gen_READ_SC_Button.Size = new System.Drawing.Size(46, 42);
            this.gen_READ_SC_Button.TabIndex = 1;
            this.gen_READ_SC_Button.Text = "Read";
            this.gen_READ_SC_Button.UseVisualStyleBackColor = true;
            this.gen_READ_SC_Button.Click += new System.EventHandler(this.gen_READ_SC_Button_Click);
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
            this.infoPanel.Location = new System.Drawing.Point(336, 222);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.infoPanel.Size = new System.Drawing.Size(172, 101);
            this.infoPanel.TabIndex = 0;
            this.infoPanel.Visible = false;
            // 
            // rw1990_HexBox
            // 
            this.rw1990_HexBox.BytesPerLine = 8;
            this.rw1990_HexBox.Encoding = null;
            this.rw1990_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rw1990_HexBox.LineInfoDigitsCnt = 2;
            this.rw1990_HexBox.LineInfoVisible = true;
            this.rw1990_HexBox.Location = new System.Drawing.Point(6, 19);
            this.rw1990_HexBox.Name = "rw1990_HexBox";
            this.rw1990_HexBox.SetPositionToZero = true;
            this.rw1990_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.rw1990_HexBox.Size = new System.Drawing.Size(314, 42);
            this.rw1990_HexBox.StartAddress = ((long)(0));
            this.rw1990_HexBox.StringViewVisible = true;
            this.rw1990_HexBox.TabIndex = 0;
            this.rw1990_HexBox.UseFixedBytesPerLine = true;
            // 
            // ds1820_EEPROM_HexBox
            // 
            this.ds1820_EEPROM_HexBox.BytesPerLine = 2;
            this.ds1820_EEPROM_HexBox.Encoding = null;
            this.ds1820_EEPROM_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ds1820_EEPROM_HexBox.LineInfoDigitsCnt = 2;
            this.ds1820_EEPROM_HexBox.LineInfoVisible = true;
            this.ds1820_EEPROM_HexBox.Location = new System.Drawing.Point(6, 19);
            this.ds1820_EEPROM_HexBox.Name = "ds1820_EEPROM_HexBox";
            this.ds1820_EEPROM_HexBox.SetPositionToZero = true;
            this.ds1820_EEPROM_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.ds1820_EEPROM_HexBox.Size = new System.Drawing.Size(121, 42);
            this.ds1820_EEPROM_HexBox.StartAddress = ((long)(0));
            this.ds1820_EEPROM_HexBox.StringViewVisible = true;
            this.ds1820_EEPROM_HexBox.TabIndex = 0;
            this.ds1820_EEPROM_HexBox.UseFixedBytesPerLine = true;
            // 
            // ds1820_SCRATCHPAD_HexBox
            // 
            this.ds1820_SCRATCHPAD_HexBox.BytesPerLine = 8;
            this.ds1820_SCRATCHPAD_HexBox.Encoding = null;
            this.ds1820_SCRATCHPAD_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ds1820_SCRATCHPAD_HexBox.LineInfoDigitsCnt = 2;
            this.ds1820_SCRATCHPAD_HexBox.LineInfoVisible = true;
            this.ds1820_SCRATCHPAD_HexBox.Location = new System.Drawing.Point(6, 19);
            this.ds1820_SCRATCHPAD_HexBox.Name = "ds1820_SCRATCHPAD_HexBox";
            this.ds1820_SCRATCHPAD_HexBox.SetPositionToZero = true;
            this.ds1820_SCRATCHPAD_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.ds1820_SCRATCHPAD_HexBox.Size = new System.Drawing.Size(314, 42);
            this.ds1820_SCRATCHPAD_HexBox.StartAddress = ((long)(0));
            this.ds1820_SCRATCHPAD_HexBox.StringViewVisible = true;
            this.ds1820_SCRATCHPAD_HexBox.TabIndex = 0;
            this.ds1820_SCRATCHPAD_HexBox.UseFixedBytesPerLine = true;
            // 
            // ds1820_SN_HexBox
            // 
            this.ds1820_SN_HexBox.BytesPerLine = 8;
            this.ds1820_SN_HexBox.Encoding = null;
            this.ds1820_SN_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ds1820_SN_HexBox.LineInfoDigitsCnt = 2;
            this.ds1820_SN_HexBox.LineInfoVisible = true;
            this.ds1820_SN_HexBox.Location = new System.Drawing.Point(6, 19);
            this.ds1820_SN_HexBox.Name = "ds1820_SN_HexBox";
            this.ds1820_SN_HexBox.ReadOnly = true;
            this.ds1820_SN_HexBox.SetPositionToZero = true;
            this.ds1820_SN_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.ds1820_SN_HexBox.Size = new System.Drawing.Size(314, 42);
            this.ds1820_SN_HexBox.StartAddress = ((long)(0));
            this.ds1820_SN_HexBox.StringViewVisible = true;
            this.ds1820_SN_HexBox.TabIndex = 2;
            this.ds1820_SN_HexBox.UseFixedBytesPerLine = true;
            // 
            // gen_DATA_HexBox
            // 
            this.gen_DATA_HexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gen_DATA_HexBox.Encoding = null;
            this.gen_DATA_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gen_DATA_HexBox.LineInfoDigitsCnt = 2;
            this.gen_DATA_HexBox.LineInfoVisible = true;
            this.gen_DATA_HexBox.Location = new System.Drawing.Point(11, 156);
            this.gen_DATA_HexBox.Name = "gen_DATA_HexBox";
            this.gen_DATA_HexBox.SetPositionToZero = true;
            this.gen_DATA_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.gen_DATA_HexBox.Size = new System.Drawing.Size(492, 167);
            this.gen_DATA_HexBox.StartAddress = ((long)(0));
            this.gen_DATA_HexBox.StringViewVisible = true;
            this.gen_DATA_HexBox.TabIndex = 2;
            this.gen_DATA_HexBox.UseFixedBytesPerLine = true;
            this.gen_DATA_HexBox.VScrollBarVisible = true;
            // 
            // gen_SC_HexBox
            // 
            this.gen_SC_HexBox.BytesPerLine = 8;
            this.gen_SC_HexBox.Encoding = null;
            this.gen_SC_HexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gen_SC_HexBox.LineInfoDigitsCnt = 2;
            this.gen_SC_HexBox.LineInfoVisible = true;
            this.gen_SC_HexBox.Location = new System.Drawing.Point(6, 19);
            this.gen_SC_HexBox.Name = "gen_SC_HexBox";
            this.gen_SC_HexBox.ReadOnly = true;
            this.gen_SC_HexBox.SetPositionToZero = true;
            this.gen_SC_HexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.gen_SC_HexBox.Size = new System.Drawing.Size(314, 42);
            this.gen_SC_HexBox.StartAddress = ((long)(0));
            this.gen_SC_HexBox.StringViewVisible = true;
            this.gen_SC_HexBox.TabIndex = 0;
            this.gen_SC_HexBox.UseFixedBytesPerLine = true;
            // 
            // OneWireDumpForm
            // 
            this.Controls.Add(this.infoPanel);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(561, 357);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OneWireDumpForm";
            this.Text = "ONE WIRE DUMP";
            this.Load += new System.EventHandler(this.OneWireDumpForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OneWireDumpForm_FormClosed);
            this.tabControl.ResumeLayout(false);
            this.iButtonPage.ResumeLayout(false);
            this.iButtonPage.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ds1820Page.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage ds1820Page;
        private System.Windows.Forms.TabPage iButtonPage;
        private System.Windows.Forms.Button ds1820_READ_SC_Button;
        private Be.Windows.Forms.HexBox ds1820_SN_HexBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private HexBox ds1820_SCRATCHPAD_HexBox;
        private Button ds1820_READ_SP_Button;
        private Button ds1820_WRITE_SP_Button;
        private Button ds1820_COPY_SP_Button;
        private GroupBox groupBox3;
        private HexBox ds1820_EEPROM_HexBox;
        private Button ds1820_WRITE_EE_Button;
        private Button ds1820_READ_EE_Button;
        private GroupBox groupBox4;
        private Button ds1820_READ_TEMP_Button;
        private Label ds1820_TEMP_Label;
        private InfoPanel infoPanel;
        private TabPage generalPage;
        private GroupBox groupBox5;
        private HexBox gen_SC_HexBox;
        private Button gen_READ_SC_Button;
        private HexBox gen_DATA_HexBox;
        private Button gen_READ_DATA_Button;
        private GroupBox groupBox6;
        private HexBox rw1990_HexBox;
        private Button rw1990_Read_Button;
        private Button rw1990_Write_Button;
        private Button openFile_Btn;
        private Button saveFile_Btn;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        private CheckBox invertWriteBytesCheckBox;
        private CheckBox autoCorrectDumpCheckBox;
        private Button rw1990_Verify_Btn;
    }
}