using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.IO;

namespace mcprog
{
    public partial class M25CXXDumpForm : Form, IHexBoxSetting
    {
        private ByteCollection _dataBytes = null;
        private FileWorker file = new FileWorker();
        private IM24CXXProgrammer _programmer = null;
        
        private HardwareController _hardwareController=null;
        private bool _programmersListIsRequested = false;

        IDictionary<string, int> s02512 = new Dictionary<string, int> { { "24C00", 16 }, { "24C01", 128 }, { "24C02", 256 }, { "24C04", 512 }, { "24C08", 1024 }, { "24C16", 2048 }, { "24C32", 4096 }, { "24C64", 8192 }, { "24C128", 16384 }, { "24C256", 32768 }, { "24C512", 65536 } };
        string lastselect = "null";

        public M25CXXDumpForm(HardwareController controller)
        {
            InitializeComponent();
            _hardwareController = controller;
        }
        public M25CXXDumpForm()
        {
            InitializeComponent();
            
        }
        private void M24CXXDumpForm_Load(object sender, EventArgs e)
        {
            SetNoProgrammerState();
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _programmersListIsRequested = true;
            UpdateComboBox(s02512.Keys.ToArray());
            chipSelectStripComboBox.SelectedIndex = 1;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
            
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            infoPanel.Visible = false;
        }
        private void M24CXXDumpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;

        }

        #region IHexBoxSetting Members

        public Encoding HexEncoding
        {
            get
            {
                return hexBox1.Encoding;
            }
            set
            {
                hexBox1.Encoding = value;
                
            }
        }

        public Font HexFont
        {
            get
            {
                return hexBox1.Font;
            }
            set
            {
                hexBox1.Font = value;
               
            }
        }

        #endregion

        private void UpdateComboBox(string[] items)
        {
            string selit=null;
            try
            {
                selit = chipSelectStripComboBox.SelectedItem.ToString();
            }
            catch (Exception) { selit = "null"; }
            chipSelectStripComboBox.Items.Clear();
            chipSelectStripComboBox.Items.AddRange(items);
            if (chipSelectStripComboBox.Items.Contains(selit))
            {
                chipSelectStripComboBox.SelectedIndexChanged -= chipSelectStripComboBox_SelectedIndexChanged;
                chipSelectStripComboBox.SelectedItem = selit;
                chipSelectStripComboBox.SelectedIndexChanged += chipSelectStripComboBox_SelectedIndexChanged;
            }
            else
                chipSelectStripComboBox.SelectedItem = chipSelectStripComboBox.Items[0];

        }

        private void chipSelectStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chipSelectStripComboBox.SelectedItem.ToString() != lastselect)
            {
                _dataBytes = new ByteCollection((new MemoryRegion(0, (uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()], 0)).Data);
                hexBox1.ByteProvider = new DynamicByteProvider(_dataBytes);
                lastselect = chipSelectStripComboBox.SelectedItem.ToString();
                Text = "M24CXX DUMP " + "Empty";
            }
        }
        
        private void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;
            foreach (IChipProgrammer icp in progsList)
            {
                if (icp is IM24CXXProgrammer)
                {
                    _programmer = icp as IM24CXXProgrammer;

                    _programmer.Busy += BusyHandler;
                    _programmer.Ready += ReadyHandler;

                    UpdateComboBox(_programmer.SupportedChips.ToArray());
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
            if(_programmersListIsRequested)return;
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

                SetNoProgrammerState();
                _programmersListIsRequested = true;
                _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
                UpdateComboBox(s02512.Keys.ToArray());
                break;

            }

        }

        private void SetNoProgrammerState()
        {
            readChipStripButton.Visible = false;
            writeChipStripButton.Visible = false;
            infoStripLabel.Visible = true;
            infoStripLabel.Text = "NO RESETTER";
        }
        private void SetProgrammerBusyState()
        {
            readChipStripButton.Visible = false;
            writeChipStripButton.Visible = false;
            infoStripLabel.Visible = true;
            infoStripLabel.Text = "RESETTER IS BUSY";
        }
        private void SetProgrammerReadyState()
        {
            readChipStripButton.Visible = true;
            writeChipStripButton.Visible = true;
            infoStripLabel.Visible = false;
            infoStripLabel.Text = "";
        }

        private void chipSelectStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ((ToolStripComboBox)sender).GetCurrentParent().Focus();
        }

        private void HexBoxChanged(object sender, EventArgs e)
        {
            Text = "*" + Text;
            hexBox1.ByteProvider.Changed -= HexBoxChanged;
        }
        
        private void readChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += M24CXXDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            MemoryRegionInfo regionInfo = new MemoryRegionInfo();
            regionInfo.Address = 0;
            regionInfo.Type = 1;
            regionInfo.Size = (uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()];
            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);
                        
                    }
                    else
                    {
                        _dataBytes = new ByteCollection(region.Data);
                        hexBox1.ByteProvider = new DynamicByteProvider(_dataBytes);


                        hexBox1.ByteProvider.Changed += HexBoxChanged;
                        Text = "M24CXX DUMP " + "It is read from CRUM";
                        infoPanel.SetOkState("Read complete");
                    }
                    this.FormClosing -= M24CXXDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading");

            _programmer.ReadChip(chipSelectStripComboBox.SelectedItem.ToString(), regionInfo, completed, infoPanel.GetProgressDelegate());

        }

        private void writeChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += M24CXXDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;

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

                    this.FormClosing -= M24CXXDumpForm_FormClosing;

                };
            

            
            infoPanel.SetProgressState("Writing");
            MemoryRegion region = new MemoryRegion(0,(uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()],1);
            region.WriteData(0,_dataBytes.GetBytes());
            _programmer.ProgramChip(chipSelectStripComboBox.SelectedItem.ToString(), region, complete, infoPanel.GetProgressDelegate());

        }

        private void M24CXXDumpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            
            openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin|ponyprog files (*.eep;*.e2p;*.rom)|*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = (uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()], Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "24CXX DUMP " + openFileDialog.FileName;
                        _dataBytes = new ByteCollection(info.Regions[0].Data);
                        hexBox1.ByteProvider = new DynamicByteProvider(_dataBytes);
                        hexBox1.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);


        }

        private void saveFileMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            MemoryRegion region = new MemoryRegion(0, (uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()], 0);
            region.WriteData(0, _dataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "M24CXX DUMP " + saveFileDialog.FileName;
                        hexBox1.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);
        
        
        
        
        }

        private void M24CXXDumpForm_DragOver(object sender, DragEventArgs e)
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

        private void M24CXXDumpForm_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData("FileNameW"))[0];
            
            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = (uint)s02512[chipSelectStripComboBox.SelectedItem.ToString()], Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "M24CXX DUMP " + fileName;
                        _dataBytes = new ByteCollection(info.Regions[0].Data);
                        hexBox1.ByteProvider = new DynamicByteProvider(_dataBytes);
                        hexBox1.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(fileName, regionsInfo, complete);

        }
    }
}
