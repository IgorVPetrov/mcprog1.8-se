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
    public partial class Xerox0190Form : Form, IHexBoxSetting
    {


        private ByteCollection _dataBytes = null;
        private FileWorker file = new FileWorker();
        private IXerox0190Programmer _programmer = null;

        private HardwareController _hardwareController = null;
        private bool _programmersListIsRequested = false;


        public Xerox0190Form(HardwareController controller)
        {
            InitializeComponent();
            _hardwareController = controller;
        } 

        public Xerox0190Form()
        {
            InitializeComponent();
        }

        #region IHexBoxSetting Members

        public Encoding HexEncoding
        {
            get
            {
                return hexBox.Encoding;
            }
            set
            {
                hexBox.Encoding = value;

            }
        }

        public Font HexFont
        {
            get
            {
                return hexBox.Font;
            }
            set
            {
                hexBox.Font = value;

            }
        }

        #endregion

        private void Xerox0190Form_Load(object sender, EventArgs e)
        {
            SetNoProgrammerState();
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _programmersListIsRequested = true;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);

            infoPanel.Visible = false;
            _dataBytes = new ByteCollection((new MemoryRegion(0, 256, 0)).Data);
            hexBox.ByteProvider = new DynamicByteProvider(_dataBytes);
        }

        private void Xerox0190Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;

        }

        private void Xerox0190Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void readChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += Xerox0190Form_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            MemoryRegionInfo regionInfo = new MemoryRegionInfo();
            regionInfo.Address = 0;
            regionInfo.Type = 1;
            regionInfo.Size = 256;
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
                        hexBox.ByteProvider = new DynamicByteProvider(_dataBytes);


                        hexBox.ByteProvider.Changed += HexBoxChanged;
                        Text = "XEROX 01/90 DUMP " + "It is read from CRUM";
                        infoPanel.SetOkState("Read complete");
                    }
                    this.FormClosing -= Xerox0190Form_FormClosing;
                };


            infoPanel.SetProgressState("Reading");

            _programmer.ReadChip("Mode 0", regionInfo, completed, infoPanel.GetProgressDelegate());


        }

        private void writeChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += Xerox0190Form_FormClosing;
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

                    this.FormClosing -= Xerox0190Form_FormClosing;

                };



            infoPanel.SetProgressState("Writing");
            MemoryRegion region = new MemoryRegion(0, 256, 1);
            region.WriteData(0, _dataBytes.GetBytes());
            _programmer.ProgramChip("Mode 0", region, complete, infoPanel.GetProgressDelegate());

        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin|ponyprog files (*.eep;*.e2p;*.rom)|*.eep;*.e2p;*.rom";
            openFileDialog.Filter = "dump files (.hex .bin .eep .e2p .rom)|*.hex;*.bin;*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "XEROX 01/90 DUMP " + openFileDialog.FileName;
                        _dataBytes = new ByteCollection(info.Regions[0].Data);
                        hexBox.ByteProvider = new DynamicByteProvider(_dataBytes);
                        hexBox.ByteProvider.Changed += HexBoxChanged;
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

            MemoryRegion region = new MemoryRegion(0, 256, 0);
            region.WriteData(0, _dataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "XEROX 01/90 DUMP " + saveFileDialog.FileName;
                        hexBox.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);

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

                SetNoProgrammerState();
                _programmersListIsRequested = true;
                _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
                
                break;

            }

        }

        private void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;
            foreach (IChipProgrammer icp in progsList)
            {
                if (icp is IXerox0190Programmer)
                {
                    _programmer = icp as IXerox0190Programmer;

                    _programmer.Busy += BusyHandler;
                    _programmer.Ready += ReadyHandler;

                    
                    SetProgrammerReadyState();

                    return;
                }
            }

        }

        private void HexBoxChanged(object sender, EventArgs e)
        {
            Text = "*" + Text;
            hexBox.ByteProvider.Changed -= HexBoxChanged;
        }

        private void Xerox0190Form_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData("FileNameW"))[0];
            
            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 256, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "XEROX 01/90 DUMP " + fileName;
                        _dataBytes = new ByteCollection(info.Regions[0].Data);
                        hexBox.ByteProvider = new DynamicByteProvider(_dataBytes);
                        hexBox.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(fileName, regionsInfo, complete);
        }

        private void Xerox0190Form_DragOver(object sender, DragEventArgs e)
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
