using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace mcprog
{
    public partial class AutoForm : Form
    {
        IChipProgrammer _programmer = null;

        HardwareController _hardwareController = new HardwareController();

        FileWorker _fileWorker = new FileWorker();

        IDictionary<string, Action<string, string>> _crumReaders = new Dictionary<string, Action<string, string>>();

        IDictionary<string, Action<string, string, string, string>> _crumWriters = new Dictionary<string, Action<string, string, string, string>>();

        IDictionary<string, Action<byte[]>> _dumpCoprrectors = new Dictionary<string, Action<byte[]>>();

        IDictionary<string, string> _messages = new Dictionary<string, string>()
        {
            { "Read", "Чтение" },
            { "Read complete", "Чтение завершилось успешно" },
            { "Programming", "Программирование" },
            { "Verification", "Проверка" },
            { "Programming complete", "Восстановление завершилось успешно" },
            { "Read user data", "Чтение данных" },
            { "Read config data", "Чтение конфигурации" }
        };

        IDictionary<string, int> s02512 = new Dictionary<string, int> { { "24C00", 16 }, { "24C01", 128 }, { "24C02", 256 }, { "24C04", 512 }, { "24C08", 1024 }, { "24C16", 2048 }, { "24C32", 4096 }, { "24C64", 8192 }, { "24C128", 16384 }, { "24C256", 32768 }, { "24C512", 65536 } };

        public struct SerialNoBounds
        {
            public int lowerBound;
            public int upperBound;
        }
        
        private bool _programmersListIsRequested = false;
        
        private bool thisBusy = false;

        XElement _crumSet=null;

        XElement _selectedChipNodes = null;

        public AutoForm(HardwareController controller)
        {
            InitializeComponent();
            _hardwareController = controller;
        }


        private void chipSelectComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ((ToolStripComboBox)sender).GetCurrentParent().Focus();
        }

        private void AutoForm_Load(object sender, EventArgs e)
        {

            #region Hardware Init

            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;

            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;

            _programmersListIsRequested = true;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);

            #endregion

            #region Chips Set Init
            try
            {

                string path = Application.ExecutablePath;
                path = path.Substring(0, path.LastIndexOf("\\"));
                Directory.SetCurrentDirectory(path);
                
                
                _crumSet = XElement.Load("dumps\\chipsset.xml");
                var names =
                    from chip in _crumSet.Elements("chip")
                    orderby (string)chip.Element("name")
                    select (string)chip.Element("name");
                string[] s = names.ToArray();

                chipSelectComboBox.Items.AddRange(s);
                chipSelectComboBox.SelectedIndex = 0;
            }
            catch (Exception ee)
            {
                _crumSet = null;
                SetAlarmText(ee.Message);
            }


            #endregion

            #region Executors Init

            _crumReaders.Add("24CXX", M24CXXRead);
            _crumReaders.Add("AT88", AT88Read);
            _crumWriters.Add("24CXX", M24CXXWrite);
            _crumWriters.Add("AT88", AT88Write);

            #endregion

            #region DumpCorrectors Init

            _dumpCoprrectors.Add("No", delegate(byte[] data) { });

            _dumpCoprrectors.Add("1", delegate(byte[] data) { AnsiSerialNoIncrement(53, 63, data); });

            _dumpCoprrectors.Add("2", delegate(byte[] data) { AnsiSerialNoIncrement(37, 47, data); });

            _dumpCoprrectors.Add("searching", delegate(byte[] data)
            { 
                ASCIIEncoding enc = new ASCIIEncoding();
                string marker = "CRUM-";
                string dumpstring = enc.GetString(data);
                dumpstring = dumpstring.ToUpper();
                List<SerialNoBounds> serials = new List<SerialNoBounds>();
                int startSearchIndex = 0;
                while ((startSearchIndex = dumpstring.IndexOf(marker, startSearchIndex)) >= 0)
                {
                    SerialNoBounds bound;
                    bound.lowerBound = startSearchIndex + 5;
                    bound.upperBound = startSearchIndex + 15;
                    serials.Add(bound);
                    startSearchIndex++;
                }

                AnsiMultiSerialsIncrement(serials, data);
            
            
            });

            _dumpCoprrectors.Add("custom", delegate(byte[] data) 
            {
                string serialDefs = _selectedChipNodes.Element("serialdefs").Value.ToString();

                Regex ubRegEx = new Regex("-([0-9a-fA-F]+)");
                Regex lbRegEx = new Regex("(^|,)([0-9a-fA-F]+)");

                MatchCollection ubMatches = ubRegEx.Matches(serialDefs);
                MatchCollection lbMatches = lbRegEx.Matches(serialDefs);

                List<SerialNoBounds> serials = new List<SerialNoBounds>();
                
                for (int i = 0; i < ubMatches.Count; i++)
                {
                    SerialNoBounds bound;
                    bound.upperBound = Convert.ToInt32(ubMatches[i].Groups[1].Value, 16);
                    bound.lowerBound = Convert.ToInt32(lbMatches[i].Groups[2].Value, 16);
                    serials.Add(bound);
                }


                AnsiMultiSerialsIncrement(serials, data);
                
                

            
            });
            
            
            
            #endregion
        }

        
        private void Read_Click(object sender, EventArgs e)
        {
            if (_crumSet == null)
            {
                SetAlarmText("No Dumps directory");
                return;

            }
            
            
            var selchip =
                from chip in _crumSet.Elements("chip")
                where (string)chip.Element("name") == chipSelectComboBox.SelectedItem.ToString()
                select chip;
            XElement nodes = selchip.ToArray()[0];
            string name = nodes.Element("name").Value.ToString();
            string type = nodes.Element("type").Value.ToString();
            string subtype = nodes.Element("subtype").Value.ToString();

            _crumReaders[type](subtype, name);
        }
         

        
        
        private void Write_Click(object sender, EventArgs e)
        {
            if (_crumSet == null)
            {
                SetAlarmText("No Dumps directory");
                return;

            }


            var selchip =
                from chip in _crumSet.Elements("chip")
                where (string)chip.Element("name") == chipSelectComboBox.SelectedItem.ToString()
                select chip;
            XElement nodes = selchip.ToArray()[0];
            _selectedChipNodes = nodes;
            string name = nodes.Element("name").Value.ToString();
            string type = nodes.Element("type").Value.ToString();
            string subtype = nodes.Element("subtype").Value.ToString();
            string serialnumtype = nodes.Element("serialnumtype").Value.ToString();
            string templatefile = nodes.Element("templatefile").Value.ToString();

            string path = Application.ExecutablePath;
            path = path.Substring(0, path.LastIndexOf("\\"));
            Directory.SetCurrentDirectory(path);

            _crumWriters[type](subtype, name, serialnumtype, templatefile);

        }

        private void AnsiMultiSerialsIncrement(List<SerialNoBounds> serials, byte[] data)
        {
            foreach (SerialNoBounds snbound in serials)
                AnsiSerialNoIncrement(snbound.lowerBound, snbound.upperBound, data);
        }


        private void AnsiSerialNoIncrement(int mostsbp, int leasbp, byte[] data)
        {
            if (++data[leasbp] > 0x39)
            {
                data[leasbp] = 0x30;
                if (--leasbp >= mostsbp) AnsiSerialNoIncrement(mostsbp, leasbp, data);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void M24CXXRead(string subtype, string name)
        {
            if ((_programmer == null) || !(_programmer is IM24CXXProgrammer))
            {
                SetAlarmText("Connect the compatible resetter");
                return; 
            }
            IM24CXXProgrammer prog = _programmer as IM24CXXProgrammer;

            if (prog.IsBusy)
            {
                SetInfoText("Resetter is busy");
                return;
            }

            this.FormClosing+=AutoForm_FormClosing;
            thisBusy = true;
            
            MemoryRegionInfo regionInfo = new MemoryRegionInfo();
            regionInfo.Address = 0;
            regionInfo.Type = 1;
            regionInfo.Size = (uint)s02512[subtype];

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    progressBar.Value = progressBar.Minimum;
                    if (pcInfo.error == null)
                    {
                        Encoding enc = Encoding.GetEncoding(1251);
                        TextBox textBox = new TextBox();
                        textBox.Multiline = true;
                        textBox.ReadOnly = true;
                        textBox.BackColor = Color.WhiteSmoke;
                        textBox.ScrollBars = ScrollBars.Vertical;
                        textBox.Size = new Size(150, 100);
                        List<string> strings = new List<string>();
                        for (int i = 0; i < region.Size; i += 16)
                        {
                            byte[] data = new byte[16];
                            for (int j = 0; j < 16; j++)
                                data[j] = region.Data[i + j] < (byte)0x20 ? (byte)0x2E : region.Data[i + j];
                            strings.Add(enc.GetString(data));
                        }
                        textBox.Lines = strings.ToArray();
                        
                        infoLayoutPanel.Controls.Add(textBox);
                        infoLayoutPanel.Controls.SetChildIndex(textBox, 0);
                        SetOkText("Read OK");
                    }
                    else
                    {
                        SetAlarmText(pcInfo.error.Message);
                    }
                    
                    thisBusy = false;
                    this.FormClosing -= AutoForm_FormClosing; 
                
                };

            prog.ReadChip(subtype, regionInfo, completed, Progress);

        }

        private void M24CXXWrite(string subtype, string name, string serialnumtype, string templatefile)
        {
            if ((_programmer == null) || !(_programmer is IM24CXXProgrammer))
            {
                SetAlarmText("Connect the compatible resetter");
                return;
            }
            IM24CXXProgrammer prog = _programmer as IM24CXXProgrammer;

            if (prog.IsBusy)
            {
                SetInfoText("Resetter is busy");
                return;
            }

            this.FormClosing += AutoForm_FormClosing;
            thisBusy = true;

            MemoryRegion region = null;
            Action<ProgrammingCompleteInfo> programmComplete =
                delegate(ProgrammingCompleteInfo pcInfo)
                {
                    progressBar.Value = progressBar.Minimum;
                    if (pcInfo.error != null)
                    {
                        if (pcInfo.error is VerificationException)
                        {
                            VerificationException ve = pcInfo.error as VerificationException;
                            SetAlarmText(
                                "Verification error At address: " + ve.ErrorAddress.ToString()
                                + " write: " + ve.WrittenByte.ToString()
                                + " read: " + ve.ReadByte.ToString());
                        }
                        else
                            SetAlarmText(pcInfo.error.Message);

                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 

                    }
                    else if (serialnumtype == "No")
                    {
                        
                        SetOkText(pcInfo.Message);
                        
                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 
                    }
                    else
                    {
                        Action<FileWorkerIOCompleteInfo> writeComplete =
                            delegate(FileWorkerIOCompleteInfo fwiocInfo)
                            {
                                if (fwiocInfo.Error != null)
                                {
                                    SetAlarmText(fwiocInfo.Error.Message);
                                }
                                else
                                {
                                    SetOkText(pcInfo.Message);
                                }
                                thisBusy = false;
                                this.FormClosing -= AutoForm_FormClosing; 

                            };

                        _dumpCoprrectors[serialnumtype](region.Data);
                        _fileWorker.Write(templatefile, new List<MemoryRegion>() { region }, writeComplete);

                    }


                };

            Action<FileWorkerIOCompleteInfo> readFileComplete =
                delegate(FileWorkerIOCompleteInfo fwiocInfo)
                {
                    if (fwiocInfo.Error != null)
                    {
                        SetAlarmText(fwiocInfo.Error.Message);
                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 
                    }
                    else
                    {
                        if (prog.IsBusy)
                        {
                            SetInfoText("Resetter is busy");
                            thisBusy = false;
                            this.FormClosing -= AutoForm_FormClosing; 
                            return;
                        }
                        region = fwiocInfo.Regions[0];
                        prog.ProgramChip(subtype, fwiocInfo.Regions[0], programmComplete, Progress);
                    }
                };
            MemoryRegionInfo regionInfo = new MemoryRegionInfo() { Address = 0, Size = 0, Type = 1 };
            
            regionInfo.Size = (uint)s02512[subtype];


            _fileWorker.Read(templatefile, new List<MemoryRegionInfo>() { regionInfo }, readFileComplete);






        }

        public void AT88Read(string subtype, string name)
        {

            if ((_programmer == null) || !(_programmer is IAT88Programmer))
            {
                SetAlarmText("Connect the compatible resetter");
                return;
            }
            IAT88Programmer prog = _programmer as IAT88Programmer;

            if (prog.IsBusy)
            {
                SetInfoText("Resetter is busy");
                return;
            }

            this.FormClosing += AutoForm_FormClosing;
            thisBusy = true;

            MemoryRegionInfo regionInfo = new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 };

            Action<ProgrammingCompleteInfo, List<MemoryRegion>> completed =
                delegate(ProgrammingCompleteInfo pcInfo, List<MemoryRegion> regions)
                {
                    progressBar.Value = progressBar.Minimum;
                    if (pcInfo.error == null)
                    {
                        Encoding enc = Encoding.GetEncoding(1251);
                        TextBox textBox = new TextBox();
                        textBox.Multiline = true;
                        textBox.ReadOnly = true;
                        textBox.BackColor = Color.WhiteSmoke;
                        textBox.ScrollBars = ScrollBars.Vertical;
                        textBox.Size = new Size(150, 100);
                        List<string> strings = new List<string>();
                        for (int i = 0; i < regions[0].Size; i += 16)
                        {
                            byte[] data = new byte[16];
                            for (int j = 0; j < 16; j++)
                                data[j] = regions[0].Data[i + j] < (byte)0x20 ? (byte)0x2E : regions[0].Data[i + j];
                            strings.Add(enc.GetString(data));
                        }
                        if (regions.Count == 2)
                        {
                            strings.Add("");
                            strings.Add("Configuration data");
                            strings.Add("");
                            for (int i = 0; i < regions[1].Size; i += 16)
                            {
                                byte[] data = new byte[16];
                                for (int j = 0; j < 16; j++)
                                    data[j] = regions[1].Data[i + j] < (byte)0x20 ? (byte)0x2E : regions[1].Data[i + j];
                                strings.Add(enc.GetString(data));
                            }
                        }
                        textBox.Lines = strings.ToArray();

                        infoLayoutPanel.Controls.Add(textBox);
                        infoLayoutPanel.Controls.SetChildIndex(textBox, 0);
                        SetOkText("Read OK");
                    }
                    else
                    {
                        try
                        {
                            SetAlarmText(_messages[pcInfo.error.Message]);
                        }
                        catch (KeyNotFoundException)
                        {
                            SetAlarmText(pcInfo.error.Message);
                        }
                    }

                    thisBusy = false;
                    this.FormClosing -= AutoForm_FormClosing; 

                };

            prog.ReadChip(subtype, new List<MemoryRegionInfo>() { regionInfo }, completed, Progress);
        }

        public void AT88Write(string subtype, string name, string serialnumtype, string templatefile)
        {
            if ((_programmer == null) || !(_programmer is IAT88Programmer))
            {
                SetAlarmText("Connect the compatible resetter");
                return;
            }
            IAT88Programmer prog = _programmer as IAT88Programmer;

            if (prog.IsBusy)
            {
                SetInfoText("Resetter is busy");
                return;
            }

            this.FormClosing += AutoForm_FormClosing;
            thisBusy = true;

            List<MemoryRegion> regions = null;
            Action<ProgrammingCompleteInfo> programmComplete =
                delegate(ProgrammingCompleteInfo pcInfo)
                {
                    progressBar.Value = progressBar.Minimum;
                    if (pcInfo.error != null)
                    {
                        if (pcInfo.error is VerificationException)
                        {
                            VerificationException ve = pcInfo.error as VerificationException;
                            SetAlarmText(
                                "Verification error At address: " + ve.ErrorAddress.ToString()
                                + " write: " + ve.WrittenByte.ToString()
                                + " read: " + ve.ReadByte.ToString());
                        }
                        else
                            SetAlarmText(pcInfo.error.Message);

                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 

                    }
                    else if (serialnumtype == "No")
                    {

                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 
                        SetOkText(pcInfo.Message);
                        
                    }
                    else
                    {
                        Action<FileWorkerIOCompleteInfo> writeComplete =
                            delegate(FileWorkerIOCompleteInfo fwiocInfo)
                            {
                                

                                SetOkText(pcInfo.Message);
                                thisBusy = false;
                                this.FormClosing -= AutoForm_FormClosing; 
                            };

                        _dumpCoprrectors[serialnumtype](regions[0].Data);
                        _fileWorker.Write(templatefile, regions, writeComplete);

                    }


                };

            Action<FileWorkerIOCompleteInfo> readFileComplete =
                delegate(FileWorkerIOCompleteInfo fwiocInfo)
                {
                    if (fwiocInfo.Error != null)
                    {
                        thisBusy = false;
                        this.FormClosing -= AutoForm_FormClosing; 
                        SetAlarmText(fwiocInfo.Error.Message);
                    }
                    else
                    {
                        if (prog.IsBusy)
                        {
                            thisBusy = false;
                            this.FormClosing -= AutoForm_FormClosing; 
                            SetInfoText("Resetter is busy");
                            return;
                        }
                        regions = fwiocInfo.Regions;
                        prog.ProgramChip(subtype, fwiocInfo.Regions, programmComplete, Progress);
                    }
                };
            MemoryRegionInfo regionInfo = new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 };

            _fileWorker.Read(templatefile, new List<MemoryRegionInfo>() { regionInfo }, readFileComplete);

        }

        public void SetAlarmText(string text)
        {

            Label l = new Label();
            l.Text =text;
            
            l.Margin=new Padding(0,0,0,5);
            l.AutoSize = true;
            l.ForeColor = Color.Red;
            infoLayoutPanel.Controls.Add(l);
            infoLayoutPanel.Controls.SetChildIndex(l, 0);
        }
        public void SetOkText(string text)
        {
            Label l = new Label();
            l.Text = text;
            
            l.Margin = new Padding(0, 0, 0, 5);
            l.AutoSize = true;
            l.ForeColor = Color.Green;
            infoLayoutPanel.Controls.Add(l);
            infoLayoutPanel.Controls.SetChildIndex(l, 0);
        }
        public void SetInfoText(string text)
        {
            Label l = new Label();
            l.Text = text;
            l.Margin = new Padding(0, 0, 0, 5);
            
            l.AutoSize = true;
            l.ForeColor = Color.Blue;
            infoLayoutPanel.Controls.Add(l);
            infoLayoutPanel.Controls.SetChildIndex(l, 0);
        }
        
        private void Progress(ProgrammingProgressInfo ppInfo)
        {

            if (ppInfo.ProgressBarDataChanged)
            {
                progressBar.Minimum = ppInfo.ProgressBarData.Minimum;
                progressBar.Maximum = ppInfo.ProgressBarData.Maximum;
                progressBar.Value = ppInfo.ProgressBarData.Value;
            }
            if (ppInfo.MessageChanged)
            {  
                SetInfoText(ppInfo.Message);  
            }
        }

        private void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;

            foreach (IChipProgrammer icp in progsList)
            {
                _programmer = icp;

                _programmer.Busy += BusyHandler;
                _programmer.Ready += ReadyHandler;
                   
                SetProgrammerReadyState();

                
                return;
                
            }
            SetNoProgrammerState();
            
            
        }
        private void BusyHandler(object sender, EventArgs e)
        {
            SetProgrammerBusyState();
        }
        private void ReadyHandler(object sender, EventArgs e)
        {
            SetProgrammerReadyState();
        }

        private void HardwareController_DeviceInProgrammerModeDetected(object sender, HardwareControllerEventArgs e)
        {
            if (_programmersListIsRequested) return;
            ProgrammersManagementProc(e.ListOfDevices);
        }

        private void HardwareController_DeviceRemoved(object sender, HardwareControllerEventArgs e)
        {
            if (_programmer == null) return;
            foreach (IChipProgrammer icp in e.ListOfDevices)
            {
                if (icp != _programmer) continue;
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
                _programmer = null;
               // SetNoProgrammerState();
                _programmersListIsRequested = true;
                _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
                
                break;

            }

        }

        private void SetNoProgrammerState()
        {
            readButton.Visible = false;
            progressBar.Visible = false;
            programButton.Visible = false;
            SetAlarmText("Connect the compatible resetter");
        }
        private void SetProgrammerBusyState()
        {
            
            readButton.Visible = false;
            programButton.Visible = false;
            if (thisBusy)
            {
                
                progressBar.Visible = true;
            }
            else
            {
                SetInfoText("Resetter is busy");
                progressBar.Visible = false;
            }
            
        }
        private void SetProgrammerReadyState()
        {
            progressBar.Visible = false;
            readButton.Visible = true;
            programButton.Visible = true;
            
            SetOkText("Resetter is ready");
        }

        private void AutoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;
        }

        private void AutoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void infoLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            if (infoLayoutPanel.Controls.Count >= 30)
            {
                infoLayoutPanel.Controls.RemoveAt(28);
            }
        }
    
    
    }
}
