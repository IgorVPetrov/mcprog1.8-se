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
    public partial class CRUM921Form : Form, IHexBoxSetting
    {
        private ByteCollection _dataBytes = null;
        private FileWorker file = new FileWorker();
        private I921Programmer _programmer = null;

        public CRUM921Form()
        {
            InitializeComponent();
        }

        public CRUM921Form(I921Programmer prog)
        {
            _programmer = prog;
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

        private void CRUM921Form_Load(object sender, EventArgs e)
        {
            SetProgrammerReadyState();

            _programmer.Busy += BusyHandler;
            _programmer.Ready += ReadyHandler;


            infoPanel.Visible = false;
            _dataBytes = new ByteCollection((new MemoryRegion(0, 384, 0)).Data);
            hexBox.ByteProvider = new DynamicByteProvider(_dataBytes);
        }

        private void CRUM921Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }


        }

        private void CRUM921Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void readChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += CRUM921Form_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            MemoryRegionInfo regionInfo = new MemoryRegionInfo();
            regionInfo.Address = 0;
            regionInfo.Type = 1;
            regionInfo.Size = 384;
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
                        Text = "921 CRUM DUMP " + "It is read from CRUM";
                        infoPanel.SetOkState(pcInfo.Message);
                    }
                    this.FormClosing -= CRUM921Form_FormClosing;
                };


            infoPanel.SetInfoState("Starting process");

            _programmer.ReadChip("Mode 0", regionInfo, completed, infoPanel.GetProgressDelegate());


        }

        private void writeChipStripButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += CRUM921Form_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;

            Action<ProgrammingCompleteInfo> complete =
                delegate(ProgrammingCompleteInfo pcInfo)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);
                    }
                    else
                    {

                        infoPanel.SetOkState(pcInfo.Message);
                    }

                    this.FormClosing -= CRUM921Form_FormClosing;

                };



            infoPanel.SetInfoState("Starting process");
            MemoryRegion region = new MemoryRegion(0, 384, 1);
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

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 384, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "921 CRUM DUMP " + openFileDialog.FileName;
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

            MemoryRegion region = new MemoryRegion(0, 384, 0);
            region.WriteData(0, _dataBytes.GetBytes());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "921 CRUM DUMP " + saveFileDialog.FileName;
                        hexBox.ByteProvider.Changed += HexBoxChanged;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);

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
        

        private void HexBoxChanged(object sender, EventArgs e)
        {
            Text = "*" + Text;
            hexBox.ByteProvider.Changed -= HexBoxChanged;
        }

        private void CRUM921Form_DragOver(object sender, DragEventArgs e)
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

        private void CRUM921Form_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData("FileNameW"))[0];
            
            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 384, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "921 CRUM DUMP " + fileName;
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
    }
}
