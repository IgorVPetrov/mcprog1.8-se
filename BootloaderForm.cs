using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace mcprog
{
    public partial class BootloaderForm : Form, IHexBoxSetting
    {



        List<MemoryRegion> _firmware = null;
        FileWorker file = new FileWorker();
        MemoryRegion _eepromRegion = null;
        MicrochipBootloaderHardware _hardware = null;
        
        
        public BootloaderForm(MicrochipBootloaderHardware hardware)
        {
            InitializeComponent();
            _hardware = hardware;
            hexBox.ByteProvider = new StaticByteProvider((new MemoryRegion(0, 256, 1)).Data);
            infoPanel.Visible = false;
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
                ;
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

        private void openFirmwareFileMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "hex files (*.hex)|*.hex";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegionInfo> regionsInfo = _hardware.GetCurrentRegionsConfig();
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        _firmware = info.Regions;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);
        
        
        
        
        
        
        }

        private void saveEEPROMFileMenuItem_Click(object sender, EventArgs e)
        {
            if (_eepromRegion == null)
            {
                infoPanel.SetErrorState("Read EEPROM at first");
                return;
            }
            saveFileDialog.Filter = "hex files (*.hex)|*.hex";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegion> regions = new List<MemoryRegion>() { _eepromRegion };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);
        




        }

        private void readEEPROMButton_Click(object sender, EventArgs e)
        {
            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);
                    }
                    else
                    {
                        
                        hexBox.ByteProvider = new StaticByteProvider(new ByteCollection(region.Data));
                        _eepromRegion = region;

                        
                        
                        infoPanel.SetOkState("Read complete");
                    }
                    SetReadyState();
                };
            SetBusyState();
            infoPanel.SetProgressState("Reading EEPROM");
            _hardware.ReadEEPROM(completed, infoPanel.GetProgressDelegate());

        }

        private void writeFirmwareButton_Click(object sender, EventArgs e)
        {
            if (_firmware == null)
            {
                infoPanel.SetErrorState("Open firmware file at first");
                return;
            }
            SetBusyState();
            Action<ProgrammingCompleteInfo> complete =
                delegate(ProgrammingCompleteInfo pcInfo)
                {
                    if (pcInfo.error != null)
                    {

                            infoPanel.SetErrorState(pcInfo.error.Message);
                    }
                    else
                    {

                        infoPanel.SetOkState("Firmware programming  complete");
                    }

                    SetReadyState();

                };
            infoPanel.SetProgressState("Firmware programming");
            _hardware.ProgramFirmware(_firmware, complete, infoPanel.GetProgressDelegate());


        }
        
        private void SetReadyState()
        {
            readEEPROMButton.Enabled = true;
            writeFirmwareButton.Enabled = true;
            resetButton.Enabled = true;


        }
        private void SetBusyState()
        {
            readEEPROMButton.Enabled = false;
            writeFirmwareButton.Enabled = false;
            resetButton.Enabled = false;

        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            SetBusyState();
            _hardware.ResetHardware();
        }

        
    }
}
