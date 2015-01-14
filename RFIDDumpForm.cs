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
using System.Globalization;
using System.Threading;

namespace mcprog
{
    public partial class RFIDDumpForm : Form, IHexBoxSetting
    {

        #region Fields
        private FileWorker file = new FileWorker();
        private IRFIDProgrammer _programmer = null;
        private HardwareController _hardwareController = null;
        private bool _programmersListIsRequested = false;

        #endregion

        #region Functions

        

        private void SetNoProgrammerState()
        {
            adjustButton.Enabled = false;
            readButton.Enabled = false;
            writeButton.Enabled = false;
            openFileButton.Enabled = true;
            saveFileButton.Enabled = true;
        }
        private void SetProgrammerBusyState()
        {
            adjustButton.Enabled = false;
            readButton.Enabled = false;
            writeButton.Enabled = false;
            openFileButton.Enabled = false;
            saveFileButton.Enabled = false;
        }
        private void SetProgrammerReadyState()
        {
            adjustButton.Enabled = true;
            readButton.Enabled = true;
            writeButton.Enabled = true;
            openFileButton.Enabled = true;
            saveFileButton.Enabled = true;
        }

        void InitializeHexBoxes()
        {
            hexBox.ByteProvider = new DynamicByteProvider(new byte[5]);
            

            Encoding defEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            hexBox.Encoding = defEncoding;
            

        }
        byte[] GetWriteSequense(string block)
        {
            byte[] result = new byte[block.Length / 8 + ((block.Length % 8) == 0 ? 0 : 1)];
            for(int i=0;i<result.Length;i++)result[i]=0;
            for (int i = 0; i < block.Length; i++)
            {
                if (block.Substring(i, 1) == "1")
                {
                    result[i / 8] |= (byte)(1 << (i % 8));

                }


            }

            return result;
        }
        #endregion

        #region GUI event handlers
        
        private void RFIDDumpForm_Load(object sender, EventArgs e)
        {
            SetNoProgrammerState();
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _programmersListIsRequested = true;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
            //tabControl.Enabled = false;
            infoPanel.Visible = false;
        }
        private void RFIDDumpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;
        }
        private void RFIDDumpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void adjustButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += RFIDDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            
            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {

                        if (region.Data[1] != 0)
                        {
                            infoPanel.SetErrorState("No RF");

                        }
                        else
                        {

                            int freq = (int)(240000 / (region.Data[3]*10+region.Data[4]*10));

                            infoPanel.SetOkState("Frequency " + freq.ToString()+" kHz");
                        }
                            
                        
                    }
                    this.FormClosing -= RFIDDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Test RF");

            _programmer.Adjust(completed, infoPanel.GetProgressDelegate());

        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += RFIDDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            
            string ems = EM4100Support.Get64BitSequense(((DynamicByteProvider)(hexBox.ByteProvider)).Bytes.ToArray());
            string block1 = "100" + ems.Substring(0, 32) + "001";
            string block2 = "100" + ems.Substring(32) + "010";

            string Coding = "01000";
            string DataRate = "101";

            if (rf16datarateRadioButton.Checked) DataRate = "001";
            else if (rf32datarateRadioButton.Checked) DataRate = "010";
            else if (rf64datarateRadioButton.Checked) DataRate = "101";

            if (manchesterRadioButton.Checked) Coding = "01000";
            else if (biphaseRadioButton.Checked) Coding = "10000";

            string block0 = "10000010000000" + DataRate + "0" + Coding + "000001000000000";

            byte[] block1bytes = GetWriteSequense(block1);
            byte[] block2bytes = GetWriteSequense(block2);
            byte[] block0bytes = GetWriteSequense(block0);

            List<byte[]> request = new List<byte[]>();

            byte[] req1 = new byte[65];
            req1[1] = 0x13;
            req1[2] = 0x23;
            req1[3] = 0x26;
            req1[4] = 38;

            Array.Copy(block1bytes, 0, req1, 5, block1bytes.Length);
            request.Add(req1);

            byte[] req2 = new byte[65];
            req2[1] = 0x13;
            req2[2] = 0x23;
            req2[3] = 0x26;
            req2[4] = 38;

            Array.Copy(block2bytes, 0, req2, 5, block2bytes.Length);
            request.Add(req2);

            byte[] req0 = new byte[65];
            req0[1] = 0x13;
            req0[2] = 0x23;
            req0[3] = 0x26;
            req0[4] = 38;

            Array.Copy(block0bytes, 0, req0, 5, block0bytes.Length);
            request.Add(req0);

            Action<ProgrammingCompleteInfo, MemoryRegion> completedRead =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {
                        RFDemodulator demod = new RFDemodulator(region.Data);
                        if (!demod.IsValid)
                        {
                            infoPanel.SetErrorState("No transponder");
                        }
                        else
                        {
                            EM4100Support em = new EM4100Support(demod);
                            em.DecoderProcess();
                            if (em.IsValid)
                            {
                                byte[] wdata = ((DynamicByteProvider)(hexBox.ByteProvider)).Bytes.ToArray();
                                bool ok = true;
                                for(int i=0;i<5;i++)
                                    if (wdata[i] != em.Data[i])
                                    {
                                        infoPanel.SetErrorState("Verification error");
                                        ok = false;
                                        break;
                                    }  
                                                          
                                if(ok)infoPanel.SetOkState("Write complete");
                                
                            }
                            else
                            {
                                infoPanel.SetErrorState("Verification error " + em.ErrorInfo);
                            }

                        }


                    }
                    this.FormClosing -= RFIDDumpForm_FormClosing;
                };

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);
                        this.FormClosing -= RFIDDumpForm_FormClosing;
                    }
                    else
                    {
                        
                        
                        infoPanel.SetProgressState("Verification");
                        byte[] request2 = new byte[65];

                        request2[1] = Constants.RFID125_READ;
                        request2[2] = 0x23;
                        request2[3] = 0x26;

                        //Thread.Sleep(500);
                        _programmer.Read(request2, completedRead, infoPanel.GetProgressDelegate());
                    }
                    
                };


            infoPanel.SetProgressState("Write");

            _programmer.Write(request, completed, infoPanel.GetProgressDelegate());
        
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            this.FormClosing += RFIDDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] request = new byte[65];

            request[1] = Constants.RFID125_READ;
            request[2] = 0x23;
            request[3] = 0x26;
            

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {
                        RFDemodulator demod = new RFDemodulator(region.Data);
                        if (!demod.IsValid)
                        {
                            infoPanel.SetErrorState("No transponder");
                        }
                        else
                        {
                            EM4100Support em = new EM4100Support(demod);
                            em.DecoderProcess();                                           
                            if (em.IsValid)
                            {
                                infoPanel.SetOkState("Read complete");
                                this.hexBox.ByteProvider = new DynamicByteProvider(em.Data);
                                if (demod.DataRate == 16) rf16datarateRadioButton.Checked = true;
                                else if (demod.DataRate == 32) rf32datarateRadioButton.Checked = true;
                                else if (demod.DataRate == 64) rf64datarateRadioButton.Checked = true;

                                if (em.CodingMethod == "Manchester") manchesterRadioButton.Checked = true;
                                if (em.CodingMethod == "Bi-phase") biphaseRadioButton.Checked = true;

                                hexBox.ByteProvider.Changed += HexBoxChanged;
                                Text = "EM4100 DUMP " + "It is read from transponder";
                            }
                            else
                            {
                                infoPanel.SetErrorState(em.ErrorInfo);
                            }

                        }


                    }
                    this.FormClosing -= RFIDDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Read");

            _programmer.Read(request, completed, infoPanel.GetProgressDelegate());
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin|ponyprog files (*.eep;*.e2p;*.rom)|*.eep;*.e2p;*.rom";
            openFileDialog.Filter = "dump files (.hex .bin .eep .e2p .rom)|*.hex;*.bin;*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 6, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "EM4100 DUMP " + openFileDialog.FileName;
                        if (info.Regions[0].Data.Length != 6)
                            infoPanel.SetErrorState("File size isn't equal 6");
                        else
                        {
                            byte[] rdata = new byte[5];
                            info.Regions[0].ReadData(0, rdata);
                            hexBox.ByteProvider = new DynamicByteProvider(rdata);
                            hexBox.ByteProvider.Changed += HexBoxChanged;
                            Text = "EM4100 DUMP " + openFileDialog.FileName;

                            byte setting = info.Regions[0].Data[5];

                            if ((setting & (byte)0xF0) == 0x10) manchesterRadioButton.Checked = true;
                            else if ((setting & (byte)0xF0) == 0x20) biphaseRadioButton.Checked = true;

                            if ((setting & (byte)0x0F) == 0x01) rf16datarateRadioButton.Checked = true;
                            else if ((setting & (byte)0x0F) == 0x02) rf32datarateRadioButton.Checked = true;
                            else if ((setting & (byte)0x0F) == 0x03) rf64datarateRadioButton.Checked = true;

                        }

                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            MemoryRegion region = new MemoryRegion(0, 6, 0);
            region.WriteData(0, ((DynamicByteProvider)(hexBox.ByteProvider)).Bytes.ToArray());
            byte setting=0x00;
            
            if (manchesterRadioButton.Checked) setting |= 0x10;
            else if (biphaseRadioButton.Checked) setting |= 0x20;

            if (rf16datarateRadioButton.Checked) setting |= 0x01;
            else if (rf32datarateRadioButton.Checked) setting |= 0x02;
            else if (rf64datarateRadioButton.Checked) setting = 0x03;

            region.Data[5] = setting;

            List<MemoryRegion> regions = new List<MemoryRegion>() { region };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "EM4100 DUMP " + saveFileDialog.FileName;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);
        }

        private void HexBoxChanged(object sender, EventArgs e)
        {
            Text = "*" + Text;
            hexBox.ByteProvider.Changed -= HexBoxChanged;
        }

        #endregion

        #region Ctors
        public RFIDDumpForm()
        {
            InitializeComponent();
            InitializeHexBoxes();
        }

        public RFIDDumpForm(HardwareController controller)
        {
            _hardwareController = controller;
            InitializeComponent();
            InitializeHexBoxes();
        }


        
        
        
        #endregion

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

        #region Hardware functions and eventhandlers

        public void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;
            foreach (IChipProgrammer icp in progsList)
            {
                if (icp is IRFIDProgrammer)
                {
                    _programmer = icp as IRFIDProgrammer;

                    _programmer.Busy += BusyHandler;
                    _programmer.Ready += ReadyHandler;


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

                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;

                _programmer = null;

                SetNoProgrammerState();
                _programmersListIsRequested = true;
                _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);

                break;

            }

        }

        #endregion

        

        



        

        

    }
}
