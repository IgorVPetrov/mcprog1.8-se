using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mcprog
{
    public partial class MainForm : Form
    {
        private EncodingInfo[] _encodings = Encoding.GetEncodings();
        private IDictionary<String, int> _codepageNum = new Dictionary<String, int>();
        private IDictionary<int, String> _codepageDisplayName = new Dictionary<int, String>();
        private Encoding encoding=Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
        HardwareController _hardwareController = new HardwareController();
        Dictionary<IChipProgrammer, Form> _bootWindows = new Dictionary<IChipProgrammer, Form>();
        bool _programmersListIsRequested = false;
        List<IChipProgrammer> _programmersWithIPresetting = new List<IChipProgrammer>();
        private I921Programmer prog921 = new ALutovResetter();

        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
    
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32HardwareIOSupport.WM_DEVICECHANGE)
            {
                if ((m.WParam.ToInt32() == Win32HardwareIOSupport.DBT_DEVICEARRIVAL) || ((int)m.WParam == Win32HardwareIOSupport.DBT_DEVICEREMOVEPENDING) || ((int)m.WParam == Win32HardwareIOSupport.DBT_DEVICEREMOVECOMPLETE) || ((int)m.WParam == Win32HardwareIOSupport.DBT_CONFIGCHANGED))
                {
                    _hardwareController.UpdateListOfAttachedDevices();
                }
            };
            base.WndProc(ref m);
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (EncodingInfo ei in _encodings)
                if (!_codepageNum.ContainsKey(ei.DisplayName))
                {
                    _codepageNum.Add(ei.DisplayName, ei.CodePage);
                    _codepageDisplayName.Add(ei.CodePage, ei.DisplayName);
                    SelectCodepageComboBox.Items.Add(ei.DisplayName);
                }
            SelectCodepageComboBox.Text = _codepageDisplayName[CultureInfo.CurrentCulture.TextInfo.ANSICodePage];
            HexEditorFontDialog.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            _hardwareController.DeviceInServiceModeDetected += HardwareController_DeviceInServiceModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.EnableHidNotification(this.Handle);

            _hardwareController.UpdateListOfAttachedDevices();
        
        }
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _hardwareController.DisableHidNotification();

        }

        private void codepageSelectStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ((ToolStripComboBox)sender).GetCurrentParent().Focus();
        }
        
        private void AddAT88_Click(object sender, EventArgs e)
        {
            AT88DumpForm form = new AT88DumpForm(_hardwareController);
            form.MdiParent=this;
            form.HexFont = HexEditorFontDialog.Font;
            form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

        private void SetFontMenuItem_Click(object sender, EventArgs e)
        {
            if (HexEditorFontDialog.ShowDialog() == DialogResult.OK)
                foreach (Form f in MdiChildren)
                    if(f is IHexBoxSetting)
                         (f as IHexBoxSetting).HexFont = HexEditorFontDialog.Font;
        }

        private void SelectCodepageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                foreach (Form f in MdiChildren)
                    if (f is IHexBoxSetting)
                        (f as IHexBoxSetting).HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
        }

        private void Add24CXX_Click(object sender, EventArgs e)
        {
            M24CXXDumpForm form = new M24CXXDumpForm(_hardwareController);
            form.MdiParent = this;
            form.HexFont = HexEditorFontDialog.Font;
            form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

        private void addAuto_Click(object sender, EventArgs e)
        {
            AutoForm form = new AutoForm(_hardwareController);
            form.MdiParent = this;
   
            
            form.Show();
        }

        private void addInfo_Click(object sender, EventArgs e)
        {
            InfoForm form = new InfoForm();
            form.MdiParent = this;


            form.Show();
        }

        private void BootloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            foreach (IChipProgrammer icp in progsList)
            {
                if (_bootWindows.ContainsKey(icp)) continue;

                Form form = icp.GetServiceWindow(); 
                form.MdiParent = this;
                form.FormClosing += BootloaderForm_FormClosing;
                if (form is IHexBoxSetting)
                {
                    (form as IHexBoxSetting).HexFont = HexEditorFontDialog.Font;
                    (form as IHexBoxSetting).HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
                }
                form.Show();
                _bootWindows.Add(icp, form);

                
               
            }

        }

        private void HardwareController_DeviceInServiceModeDetected(object sender, HardwareControllerEventArgs e)
        {
            if (_programmersListIsRequested) return;
            ProgrammersManagementProc(e.ListOfDevices);
        }

        private void HardwareController_DeviceRemoved(object sender, HardwareControllerEventArgs e)
        {

            foreach (IChipProgrammer icp in e.ListOfDevices)
            {
                if (_bootWindows.ContainsKey(icp))
                {
                    _bootWindows[icp].FormClosing -= BootloaderForm_FormClosing;
                    _bootWindows[icp].Close();
                    _bootWindows.Remove(icp);
                }
                if (icp is IPresetting)
                {
                    _programmersWithIPresetting.Remove(icp);
                    IPresetting ip = icp as IPresetting;
                    ip.ClosePresettingWindow();

                }
            }  
            

        }

        private void HardwareController_DeviceInProgrammerModeDetected(object sender, HardwareControllerEventArgs e)
        {
            foreach (IChipProgrammer icp in e.ListOfDevices)
            {
                if (icp is IPresetting)
                {
                    if (_programmersWithIPresetting.Contains(icp)) continue;
                    _programmersWithIPresetting.Add(icp);
                    IPresetting ip = icp as IPresetting;
                    Form pw = ip.GetPresettingWindow();
                    pw.MdiParent = this;
                    pw.WindowState = FormWindowState.Minimized;
                    ip.ShowPresettingWindow();

                    
                }
            }


        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (IChipProgrammer prog in _programmersWithIPresetting)
            {
                if (prog.IsBusy)
                {
                    e.Cancel = true;
                    return;
                }
                (prog as IPresetting).ClosePresettingWindow();
            }
            e.Cancel = false;

        }

        private void addXerox0190Button_Click(object sender, EventArgs e)
        {
            Xerox0190Form form = new Xerox0190Form(_hardwareController);
            form.MdiParent = this;
            form.HexFont = HexEditorFontDialog.Font;
            form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

        private void add921Button_Click(object sender, EventArgs e)
        {
            CRUM921Form form = new CRUM921Form(prog921);
            form.MdiParent = this;
            form.HexFont = HexEditorFontDialog.Font;
            form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

        private void add1WireButton_Click(object sender, EventArgs e)
        {
            OneWireDumpForm form = new OneWireDumpForm(_hardwareController);
            form.MdiParent = this;
            //form.HexFont = HexEditorFontDialog.Font;
            //form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

        private void addRFIDButton_Click(object sender, EventArgs e)
        {
            
            RFIDDumpForm form = new RFIDDumpForm(_hardwareController);
            form.MdiParent = this;
            //form.HexFont = HexEditorFontDialog.Font;
            //form.HexEncoding = Encoding.GetEncoding(_codepageNum[(String)(SelectCodepageComboBox.SelectedItem)]);
            form.Show();
        }

    }
}
