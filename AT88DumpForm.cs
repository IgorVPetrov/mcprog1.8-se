using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;



namespace mcprog
{
    public partial class AT88DumpForm : Form, IHexBoxSetting
    {
        
        private ByteCollection _userDataBytes = new ByteCollection((new MemoryRegion(0,256,0)).Data);
        private ByteCollection _configDataBytes = new ByteCollection((new MemoryRegion(0, 256, 0)).Data);
        private FileWorker file = new FileWorker();
        private IAT88Programmer _programmer = null;
        
        private HardwareController _hardwareController = null;
        private bool _programmersListIsRequested = false;
        
        //string lastselect = "null";
        IDictionary<TabPage, string> sptext = new Dictionary<TabPage, string>();


        public AT88DumpForm(HardwareController controller)
        {
            InitializeComponent();
            _hardwareController = controller;
        }

        private void SetNoProgrammerState()
        {
            readCrumButton.Visible = false;
            writeCrumButton.Visible = false;
            infoStripLabel.Visible = true;
            infoStripLabel.Text = "NO RESETTER";
        }
        private void SetProgrammerBusyState()
        {
            readCrumButton.Visible = false;
            writeCrumButton.Visible = false;
            infoStripLabel.Visible = true;
            infoStripLabel.Text = "RESETTER IS BUSY";
        }
        private void SetProgrammerReadyState()
        {
            readCrumButton.Visible = true;
            writeCrumButton.Visible = true;
            infoStripLabel.Visible = false;
            infoStripLabel.Text = "";
        }

        private void AT88DumpForm_Load(object sender, EventArgs e)
        {
            infoPanel.Visible = false;
            SetNoProgrammerState();
            userDataHexBox.ByteProvider = new DynamicByteProvider(_userDataBytes);
            configDataHexBox.ByteProvider = new StaticByteProvider(_configDataBytes);
            configDataHexBox.StartAddress = 0x100;
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _programmersListIsRequested = true;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
            foreach (TabPage tp in dumpTabControl.TabPages)
                sptext[tp] = "AT88 DUMP Empty";
        }
        
        private void AT88DumpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;
        }

        private void AT88DumpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        
        private void passwordSelectComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ((ToolStripComboBox)sender).GetCurrentParent().Focus();
        }


        #region IHexBoxSetting Members

        public Encoding HexEncoding
        {
            get
            {
                return userDataHexBox.Encoding;
            }
            set
            {
                userDataHexBox.Encoding = value;
                configDataHexBox.Encoding = value;
            }
        }

        public Font HexFont
        {
            get
            {
                return userDataHexBox.Font;
            }
            set
            {
                userDataHexBox.Font=value;
                configDataHexBox.Font = value;
            }
        }

        #endregion

        private void OpenUserDataFileMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            openFileDialog.Filter = "dump files (.hex .bin .eep .e2p .rom)|*.hex;*.bin;*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            //List<MemoryRegion> regions = new List<MemoryRegion> (){ new MemoryRegion(0, 256, 1) };
            //List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { regions[0].GetRegionInfo()};
            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo(){Address=0, Size=256, Type=1} };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        _userDataBytes = new ByteCollection(info.Regions[0].Data);
                        userDataHexBox.ByteProvider = new DynamicByteProvider(_userDataBytes);
                        Text = "AT88 DUMP " + openFileDialog.FileName;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);
        }

        private void SaveUserDataFileMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            MemoryRegion region = new MemoryRegion(0, 256, 0);
            region.WriteData(0, _userDataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region  };
           
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error != null)
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);
        }

        private void OpenConfigDataFileMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            openFileDialog.Filter = "dump files (.hex .bin .eep .e2p .rom)|*.hex;*.bin;*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            //List<MemoryRegion> regions = new List<MemoryRegion>() { new MemoryRegion(0x100, 256, 3) };
            //List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { regions[0].GetRegionInfo() };
            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 256, Size = 256, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        _configDataBytes = new ByteCollection(info.Regions[0].Data);
                        configDataHexBox.ByteProvider = new StaticByteProvider(_configDataBytes);
                        Text = "AT88 DUMP " + openFileDialog.FileName;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }

                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);
        }

        private void SaveConfigDataFileMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            MemoryRegion region = new MemoryRegion(0x100, 256, 0);
            region.WriteData(0x100, _configDataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error != null)
                    {
                        
                        infoPanel.SetErrorState(info.Error.Message);
                    }

                };
            file.Write(saveFileDialog.FileName, regions, complete);
        }

        private void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;
            foreach (IChipProgrammer icp in progsList)
            {
                if (icp is IAT88Programmer)
                {
                    _programmer = icp as IAT88Programmer;

                    _programmer.Busy += BusyHandler;
                    _programmer.Ready += ReadyHandler;

                    passwordSelectComboBox.Items.AddRange(_programmer.SupportedChips.ToArray());
                    passwordSelectComboBox.SelectedIndex = 0;
                    SetProgrammerReadyState();



                    return;
                }
            }

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

                passwordSelectComboBox.Items.Clear();
                
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
                
                _programmer = null;
                SetNoProgrammerState();
                _programmersListIsRequested = true;
                _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
                
                break;

            }

        }

        private void HexBoxChanged(object sender, EventArgs e)
        {
            Text = "*" + Text;
            userDataHexBox.ByteProvider.Changed -= HexBoxChanged;
        }
                
        private void readCrumButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += AT88DumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 }, new MemoryRegionInfo() { Address = 256, Size = 256, Type = 1 } };

            Action<ProgrammingCompleteInfo, List<MemoryRegion>> completed =
                delegate(ProgrammingCompleteInfo pcInfo, List<MemoryRegion> regions)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);
                    }
                    else
                    {
                        _userDataBytes = new ByteCollection(regions[0].Data);
                        userDataHexBox.ByteProvider = new DynamicByteProvider(_userDataBytes);
                        userDataHexBox.ByteProvider.Changed += HexBoxChanged;
                        _configDataBytes = new ByteCollection(regions[1].Data);
                        configDataHexBox.ByteProvider = new DynamicByteProvider(_configDataBytes);
                        foreach (TabPage tp in dumpTabControl.TabPages)
                            sptext[tp] = "AT88 DUMP It is read from CRUM";
                        Text = "AT88 DUMP " + "It is read from CRUM";
                        infoPanel.SetOkState("Read complete");
                    }
                    this.FormClosing -= AT88DumpForm_FormClosing;

                };
            
            

            infoPanel.SetProgressState("Reading");

            _programmer.ReadChip(passwordSelectComboBox.SelectedItem.ToString(), regionsInfo, completed, infoPanel.GetProgressDelegate());
        }

        private void writeCrumButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += AT88DumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;

            MemoryRegion udr = new MemoryRegion(0, 256, 1);
            udr.WriteData(0, _userDataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion> { udr };
            
            Action<ProgrammingCompleteInfo> complete =
                delegate(ProgrammingCompleteInfo pcInfo)
                {
                    
                    if (pcInfo.error != null)
                    {
                        if (pcInfo.error is VerificationException)
                        {
                            VerificationException ve = pcInfo.error as VerificationException;
                            infoPanel.SetErrorState(
                                "Verification error. At address: " + ve.ErrorAddress.ToString("X")
                                + " write: " + ve.WrittenByte.ToString("X")
                                + " read: " + ve.ReadByte.ToString("X"));
                        }
                        else
                            infoPanel.SetErrorState(pcInfo.error.Message);
                    }
                    else
                    {
                        infoPanel.SetOkState("Write complete");

                    }

                    this.FormClosing -= AT88DumpForm_FormClosing;
                };

            

            infoPanel.SetProgressState("Writing");
            _programmer.ProgramChip(passwordSelectComboBox.SelectedItem.ToString(), regions, complete, infoPanel.GetProgressDelegate());

        }

        private void dumpTabControl_Selected(object sender, TabControlEventArgs e)
        {
            this.Text = sptext[e.TabPage];
        }

        private void dumpTabControl_Deselected(object sender, TabControlEventArgs e)
        {
            sptext[e.TabPage] = this.Text;

        }

        private void AT88DumpForm_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData("FileNameW"))[0];

            List<MemoryRegionInfo> regionsInfo;
            if (dumpTabControl.SelectedTab == configDataEditorPage)
            {
                regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 256, Size = 256, Type = 1 } };
            }
            else
            {
                regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 } };
            }


            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        if (dumpTabControl.SelectedTab == configDataEditorPage)
                        {
                            _configDataBytes = new ByteCollection(info.Regions[0].Data);
                            configDataHexBox.ByteProvider = new StaticByteProvider(_configDataBytes);
                        }
                        else
                        {
                            _userDataBytes = new ByteCollection(info.Regions[0].Data);
                            userDataHexBox.ByteProvider = new DynamicByteProvider(_userDataBytes);
                        }
                        Text = "AT88 DUMP " + fileName;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }

                };
            file.Read(fileName, regionsInfo, complete);
        }

        private void AT88DumpForm_DragOver(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();

            string fileName = "";
            foreach (string format in formats)
            {
                if (format == "FileNameW")
                {
                    fileName = ((string[])e.Data.GetData("FileNameW"))[0];
                    break;
                }
            }
            if (fileName != "")
            {
                FileInfo fi = new FileInfo(fileName);
                if (fi.Attributes != FileAttributes.Directory)
                {

                    fileName = fi.Name;

                    string ext = fileName.Substring(fileName.Length - 3);

                    if ((ext == "hex") || (ext == "bin") || (ext == "eep") || (ext == "e2p") || (ext == "rom")) e.Effect = DragDropEffects.Copy;

                }


            }
        }
       
    
    
    
    
    
    }




}
