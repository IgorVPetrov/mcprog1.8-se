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
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace mcprog
{
    
    
    public partial class OneWireDumpForm : Form, IHexBoxSetting
    {

        #region Fields
        private FileWorker file = new FileWorker();
        private IOneWireProgrammer _programmer = null;
        private HardwareController _hardwareController = null;
        private bool _programmersListIsRequested = false;

        #endregion



        #region CRC calc items

        byte[] CRC8_Tab = new byte[256]
        {
            0, 94, 188, 226, 97, 63, 221, 131, 194, 156, 126, 32, 163, 253, 31, 65,
            157, 195, 33, 127, 252, 162, 64, 30, 95, 1, 227, 189, 62, 96, 130, 220,
            35, 125, 159, 193, 66, 28, 254, 160, 225, 191, 93, 3, 128, 222, 60, 98,
            190, 224, 2, 92, 223, 129, 99, 61, 124, 34, 192, 158, 29, 67, 161, 255,
            70, 24, 250, 164, 39, 121, 155, 197, 132, 218, 56, 102, 229, 187, 89, 7,
            219, 133, 103, 57, 186, 228, 6, 88, 25, 71, 165, 251, 120, 38, 196, 154,
            101, 59, 217, 135, 4, 90, 184, 230, 167, 249, 27, 69, 198, 152, 122, 36,
            248, 166, 68, 26, 153, 199, 37, 123, 58, 100, 134, 216, 91, 5, 231, 185, 
            140, 210, 48, 110, 237, 179, 81, 15, 78, 16, 242, 172, 47, 113, 147, 205,
            17, 79, 173, 243, 112, 46, 204, 146, 211, 141, 111, 49, 178, 236, 14, 80, 
            175, 241, 19, 77, 206, 144, 114, 44, 109, 51, 209, 143, 12, 82, 176, 238, 
            50, 108, 142, 208, 83, 13, 239, 177, 240, 174, 76, 18, 145, 207, 45, 115, 
            202, 148, 118, 40, 171, 245, 23, 73, 8, 86, 180, 234, 105, 55, 213, 139, 
            87, 9, 235, 181, 54, 104, 138, 212, 149, 203, 41, 119, 244, 170, 72, 22, 
            233, 183, 85, 11, 136, 214, 52, 106, 43, 117, 151, 201, 74, 20, 246, 168, 
            116, 42, 200, 150, 21, 75, 169, 247, 182, 232, 10, 84, 215, 137, 107, 53

        };

        byte CRC8_Cal(List<byte> seq)
        {
            byte crc = 0;

            foreach (byte b in seq) crc = CRC8_Tab[crc ^ b];

            return crc;
        }


        
        
        #endregion

        #region Ctors
        public OneWireDumpForm()
        {
            InitializeComponent();
            InitializeHexBoxes();
        }

        public OneWireDumpForm(HardwareController controller)
        {
            _hardwareController = controller;
            InitializeComponent();
            InitializeHexBoxes();
        }


        
        
        
        #endregion

        #region Functions

        List<byte[]> ds24XX_GetReadRequestPackets(byte command_code, int addr, int len, int pack_size)
        {
            List<byte[]> result = new List<byte[]>();

            while (len > 0)
            {
                int curr_pack_size = len > pack_size ? pack_size : len;
                byte[] pack = new byte[65];
                pack[1] = Constants.ONEWIRE_COMMAND_2;
                pack[2] = 0xF0;
                pack[3] = 0x02;
                pack[4] = (byte)curr_pack_size;
                pack[5] = (byte)(addr & 0xFF);
                pack[6] = (byte)((addr >> 8) & 0xFF);

                result.Add(pack);

                addr += curr_pack_size;
                len -= curr_pack_size;
            }
            return result;
        }

        byte[] ds24XX_GetReadedData(MemoryRegion region, int len, int pack_size)
        {
            List<byte> data = new List<byte>();
            int addr = 2;

            while (len > 0)
            {
                int curr_pack_size = len > pack_size ? pack_size : len;
                for (int i = 0; i < curr_pack_size; i++) data.Add(region.Data[addr + i]);
                addr += 65;
                len -= curr_pack_size;
            }
            
            
            return data.ToArray();
        }
        
        void InitializeHexBoxes()
        {
            ds1820_SN_HexBox.ByteProvider = new DynamicByteProvider(new byte[8]);
            ds1820_SCRATCHPAD_HexBox.ByteProvider = new DynamicByteProvider(new byte[8]);
            ds1820_EEPROM_HexBox.ByteProvider = new DynamicByteProvider(new byte[2]);
            gen_SC_HexBox.ByteProvider = new DynamicByteProvider(new byte[8]);
            gen_DATA_HexBox.ByteProvider = new DynamicByteProvider(new byte[0x98]);
            rw1990_HexBox.ByteProvider = new DynamicByteProvider(new byte[8]);
            rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
            rw1990_HexBox.SetPositionToZero = false;

            Encoding defEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            ds1820_SN_HexBox.Encoding = defEncoding;
            ds1820_SCRATCHPAD_HexBox.Encoding = defEncoding;
            ds1820_EEPROM_HexBox.Encoding = defEncoding;
            gen_SC_HexBox.Encoding = defEncoding;
            gen_DATA_HexBox.Encoding = defEncoding;
            rw1990_HexBox.Encoding = defEncoding;
            
        }
        
        private void SetNoProgrammerState()
        {
            tabControl.Enabled = false;
        }
        private void SetProgrammerBusyState()
        {
            tabControl.Enabled = false;
        }
        private void SetProgrammerReadyState()
        {
            tabControl.Enabled = true;
        }


        #endregion

        #region IHexBoxSetting Members

        public Encoding HexEncoding
        {
            get
            {
                return ds1820_SN_HexBox.Encoding;
            }
            set
            {
                ds1820_SN_HexBox.Encoding = value;
                ds1820_SCRATCHPAD_HexBox.Encoding = value;
                ds1820_EEPROM_HexBox.Encoding = value;
                gen_SC_HexBox.Encoding = value;
            }
        }

        public Font HexFont
        {
            get
            {
                return ds1820_SN_HexBox.Font;
            }
            set
            {
                ds1820_SN_HexBox.Font = value;

            }
        }

        #endregion

        #region GUI Event Handlers


        private void ds1820_READ_SC_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 1;//1 command set

            commands[3] = 0x33;//RomCommand READ ROM
            commands[4] = 0x08;//RomCommandDataSize
            commands[5] = 0xBE;//FuncCommand READ SCRATCHPAD
            commands[6] = 0x09;//FuncCommandDataSize

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {
                        

                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Read serial code complete");
                            byte[] ds_sn = new byte[8];
                            for (int i = 0; i < 8; i++) ds_sn[i] = region.Data[6 + i];

                            List<byte> seq = new List<byte>();
                            for (int i = 0; i < 7; i++) seq.Add(ds_sn[i]);
                            byte crc = CRC.CRC8_Calc(seq);

                            if (crc == ds_sn[7])
                            {
                                infoPanel.SetOkState("Read serial code complete");
                            }
                            else
                            {
                                infoPanel.SetErrorState("OW_ERROR : CRC ERROR");
                            }
                            
                            ds1820_SN_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);
                        
                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading serial code");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void ds1820_READ_SP_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 1;//1 command set

            commands[3] = 0xCC;//RomCommand SKIP ROM
            commands[4] = 0x00;//RomCommandDataSize
            commands[5] = 0xBE;//FuncCommand READ SCRATCHPAD
            commands[6] = 0x09;//FuncCommandDataSize

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Read scratchpad complete");
                            byte[] ds_sn = new byte[8];
                            for (int i = 0; i < 8; i++) ds_sn[i] = region.Data[6 + i];


                            List<byte> seq = new List<byte>();
                            for (int i = 0; i < 8; i++) seq.Add(ds_sn[i]);
                            byte crc = CRC.CRC8_Calc(seq);

                            if (crc == region.Data[14])
                            {
                                infoPanel.SetOkState("Read scratchpad complete");
                            }
                            else
                            {
                                infoPanel.SetErrorState("OW_ERROR : CRC ERROR");
                            }
                            
                            
                            
                            ds1820_SCRATCHPAD_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);

                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading scratchpad");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void ds1820_WRITE_SP_Button_Click(object sender, EventArgs e)
        {

        }

        private void ds1820_COPY_SP_Button_Click(object sender, EventArgs e)
        {

        }

        private void ds1820_READ_EE_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 1;//1 command set

            commands[3] = 0xCC;//RomCommand SKIP ROM
            commands[4] = 0x00;//RomCommandDataSize
            commands[5] = 0xBE;//FuncCommand READ SCRATCHPAD
            commands[6] = 0x09;//FuncCommandDataSize

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Read EEPROM complete");
                            byte[] ds_sn = new byte[2];
                            for (int i = 0; i < 2; i++) ds_sn[i] = region.Data[8 + i];
                            ds1820_EEPROM_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);

                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("EEPROM scratchpad");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void ds1820_WRITE_EE_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 2;//1 command set

            commands[3] = 0xCC;//RomCommand SKIP ROM
            commands[4] = 0x00;//RomCommandDataSize
            commands[5] = 0x4E;//FuncCommand WRITE SCRATCHPAD
            commands[6] = 0x82;//FuncCommandDataSize

            commands[7] = 0xCC;//RomCommand SKIP ROM
            commands[8] = 0x00;//RomCommandDataSize
            commands[9] = 0x48;//FuncCommand COPY SCRATCHPAD
            commands[10] = 0x00;//FuncCommandDataSize            

            byte[] data = ((DynamicByteProvider)(ds1820_EEPROM_HexBox.ByteProvider)).Bytes.ToArray();

            commands[19] = data[0];
            commands[20] = data[1];

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Write EEPROM complete");
                           


                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Writing EEPROM");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void ds1820_READ_TEMP_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 2;//1 command set

            commands[3] = 0xCC;//RomCommand READ ROM
            commands[4] = 0x00;//RomCommandDataSize
            commands[5] = 0x44;//FuncCommand READ SCRATCHPAD
            commands[6] = 0x00;//FuncCommandDataSize

            commands[7] = 0xCC;//RomCommand READ ROM
            commands[8] = 0x00;//RomCommandDataSize
            commands[9] = 0xBE;//FuncCommand READ SCRATCHPAD
            commands[10] = 0x09;//FuncCommandDataSize            
            
            
            
            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Read temperature complete");
                            byte[] ds_sn = new byte[8];
                            for (int i = 0; i < 8; i++) ds_sn[i] = region.Data[6 + i];
                            
                            Int16 tregs = (Int16)((UInt16)ds_sn[0] | ((UInt16)ds_sn[1]<<8));

                            try
                            {
                                double temp = (double)(tregs/2) - 0.25 + (ds_sn[7] - ds_sn[6]) / ds_sn[7];

                                ds1820_TEMP_Label.Text = temp.ToString("F2");
                            }
                            catch
                            {
                                ds1820_TEMP_Label.Text = "ERR";
                            }

                            
                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading temperature");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void gen_READ_SC_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.ONEWIRE_COMMAND;//QUERY_DEVICE
            commands[2] = 1;//1 command set

            commands[3] = 0x33;//RomCommand READ ROM
            commands[4] = 0x08;//RomCommandDataSize
            commands[5] = 0x00;//FuncCommand READ SCRATCHPAD
            commands[6] = 0x00;//FuncCommandDataSize

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[5] == 0)
                        {
                            infoPanel.SetOkState("Read serial code complete");
                            byte[] ds_sn = new byte[8];
                            for (int i = 0; i < 8; i++) ds_sn[i] = region.Data[6 + i];

                            List<byte> seq = new List<byte>();
                            for (int i = 0; i < 7; i++) seq.Add(ds_sn[i]);
                            byte crc = CRC.CRC8_Calc(seq);

                            if (crc == ds_sn[7])
                            {
                                infoPanel.SetOkState("Read serial code complete");
                            }
                            else
                            {
                                infoPanel.SetErrorState("OW_ERROR : CRC ERROR");
                            }

                            gen_SC_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);

                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[5].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading serial code");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void gen_READ_DATA_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;

            List<byte[]> commands = ds24XX_GetReadRequestPackets(0xF0, 0, 0x98, 8);

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {
                        gen_DATA_HexBox.ByteProvider=new DynamicByteProvider(ds24XX_GetReadedData(region, 0x98, 8));
                        infoPanel.SetOkState("Read EEPROM OK");
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading EEPROM");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());


        }

        private void rw1990_Read_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.DS1990_READ;//READ_DS1990

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[1] == 0)
                        {
                            infoPanel.SetOkState("Read serial code complete");
                            byte[] ds_sn = new byte[8];
                            for (int i = 0; i < 8; i++) ds_sn[i] = region.Data[2 + i];

                            List<byte> seq = new List<byte>();
                            for (int i = 0; i < 7; i++) seq.Add(ds_sn[i]);
                            byte crc = CRC.CRC8_Calc(seq);
                            //byte crc2 = CRC.CRC8_Calc2(seq);
                            if (crc == ds_sn[7])
                            {
                                infoPanel.SetOkState("Read serial code complete");
                            }
                            else
                            {
                                infoPanel.SetErrorState("OW_ERROR : CRC ERROR");
                            }

                            rw1990_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);
                            rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[1].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Reading serial code");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void rw1990_Write_Button_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.RW1990_WRITE;//RW1990_WRITE
          
            byte[] data = ((DynamicByteProvider)(rw1990_HexBox.ByteProvider)).Bytes.ToArray();

            for (int i = 0; i < 8; i++) commands[5 + i] = invertWriteBytesCheckBox.Checked ? (byte)~data[i] : (byte)data[i];


            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[1] == 0)
                        {
                            bool verify = true;
                            for (int i = 0; i < 8; i++)
                            {
                                if (data[i] != region.Data[2 + i])
                                {
                                    verify = false;
                                    break;
                                }
                            }
                            if (verify)
                                infoPanel.SetOkState("Write RW1990 complete");
                            else
                            {
                                string wr = "Write : ";
                                string rd = "Read : ";
                                for (int i = 0; i < 8; i++)
                                {
                                    wr = wr + data[i].ToString("X2") + " ";
                                    rd = rd + region.Data[2 + i].ToString("X2") + " ";
                                }
                                infoPanel.SetErrorState("VERIFICATION_ERROR : " + wr + rd);
                            }
                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[1].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };


            infoPanel.SetProgressState("Writing RW1990");

            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }

        private void rw1990_Verify_Btn_Click(object sender, EventArgs e)
        {
            this.FormClosing += OneWireDumpForm_FormClosing;
            if (_programmer.IsBusy || (_programmer == null)) return;
            byte[] commands = new byte[65];

            commands[1] = Constants.DS1990_READ;//READ_DS1990

            Action<ProgrammingCompleteInfo, MemoryRegion> completed =
                delegate(ProgrammingCompleteInfo pcInfo, MemoryRegion region)
                {
                    if (pcInfo.error != null)
                    {
                        infoPanel.SetErrorState(pcInfo.error.Message);

                    }
                    else
                    {


                        if (region.Data[1] == 0)
                        {
                            infoPanel.SetOkState("Read serial code complete");
                            byte[] ds_sn = new byte[8];
                            string readed = "";
                            for (int i = 0; i < 8; i++)
                            {
                                ds_sn[i] = region.Data[2 + i];
                                readed = readed + " " + ds_sn[i].ToString("X2");
                            }

                            byte[] data = ((DynamicByteProvider)(rw1990_HexBox.ByteProvider)).Bytes.ToArray();
                            //List<byte> seq = new List<byte>();
                            bool verOk = true;
                            for (int i = 0; i < 8; i++)
                            {
                                if (ds_sn[i] != data[i])
                                {
                                    verOk = false;
                                    break;
                                }
                            }
                            //byte crc = CRC.CRC8_Calc(seq);
                            //byte crc2 = CRC.CRC8_Calc2(seq);
                            if (verOk)
                            {
                                infoPanel.SetOkState("Verification complete");
                            }
                            else
                            {
                                infoPanel.SetErrorState("Verification error. Read" + readed);
                            }

                            //rw1990_HexBox.ByteProvider = new DynamicByteProvider(ds_sn);
                            //rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
                        }
                        else
                        {
                            infoPanel.SetErrorState("OW_ERROR : " + region.Data[1].ToString("X2"));
                        }
                    }
                    this.FormClosing -= OneWireDumpForm_FormClosing;
                };
            _programmer.ExecuteCommandSet(commands, completed, infoPanel.GetProgressDelegate());
        }
        
        private void rw1990_EditorChanged(object sender, EventArgs e)
        {
            if (autoCorrectDumpCheckBox.Checked)
            {
                byte[] data = ((DynamicByteProvider)(rw1990_HexBox.ByteProvider)).Bytes.ToArray();
                data[0] = 0x01;
                List<byte> seq = new List<byte>();
                for (int i = 0; i < 7; i++) seq.Add(data[i]);
                data[7] = CRC.CRC8_Calc(seq);
                rw1990_HexBox.ByteProvider = new DynamicByteProvider(data);
                rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
            }
        }

        private void autoCorrectDumpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoCorrectDumpCheckBox.Checked)
            {
                byte[] data = ((DynamicByteProvider)(rw1990_HexBox.ByteProvider)).Bytes.ToArray();
                data[0] = 0x01;
                List<byte> seq = new List<byte>();
                for (int i = 0; i < 7; i++) seq.Add(data[i]);
                data[7] = CRC.CRC8_Calc(seq);
                rw1990_HexBox.ByteProvider = new DynamicByteProvider(data);
                rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
            }
        } 

        private void OneWireDumpForm_Load(object sender, EventArgs e)
        {
            _hardwareController.DeviceInProgrammerModeDetected += HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved += HardwareController_DeviceRemoved;
            _programmersListIsRequested = true;
            _hardwareController.GetListOfDevicesInProgrammerMode(ProgrammersManagementProc);
            tabControl.Enabled = false;
            infoPanel.Visible = false;
        }
        private void OneWireDumpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_programmer != null)
            {
                _programmer.Busy -= BusyHandler;
                _programmer.Ready -= ReadyHandler;
            }
            _hardwareController.DeviceInProgrammerModeDetected -= HardwareController_DeviceInProgrammerModeDetected;
            _hardwareController.DeviceRemoved -= HardwareController_DeviceRemoved;
        }
        private void OneWireDumpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void openFile_Btn_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin|ponyprog files (*.eep;*.e2p;*.rom)|*.eep;*.e2p;*.rom";
            openFileDialog.Filter = "dump files (.hex .bin .eep .e2p .rom)|*.hex;*.bin;*.eep;*.e2p;*.rom";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            List<MemoryRegionInfo> regionsInfo = new List<MemoryRegionInfo>() { new MemoryRegionInfo() { Address = 0, Size = 8, Type = 1 } };
            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "iButton DUMP " + openFileDialog.FileName;
                        if (info.Regions[0].Data.Length != 8)
                            infoPanel.SetErrorState("File size isn't equal 8");
                        else
                        {
                            rw1990_HexBox.ByteProvider = new DynamicByteProvider(info.Regions[0].Data);
                            rw1990_HexBox.ByteProvider.Changed += rw1990_EditorChanged;
                        }
   
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Read(openFileDialog.FileName, regionsInfo, complete);
        }
        private void saveFile_Btn_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "hex files (*.hex)|*.hex|bin files (*.bin)|*.bin";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            MemoryRegion region = new MemoryRegion(0, 8, 0);
            region.WriteData(0, ((DynamicByteProvider)(rw1990_HexBox.ByteProvider)).Bytes.ToArray());
            List<MemoryRegion> regions = new List<MemoryRegion>() { region };

            Action<FileWorkerIOCompleteInfo> complete =
                delegate(FileWorkerIOCompleteInfo info)
                {
                    if (info.Error == null)
                    {
                        Text = "M24CXX DUMP " + saveFileDialog.FileName;
                    }
                    else
                    {
                        infoPanel.SetErrorState(info.Error.Message);
                    }
                };
            file.Write(saveFileDialog.FileName, regions, complete);
        }


        #endregion

        #region Hardware functions and eventhandlers
        
        public void ProgrammersManagementProc(List<IChipProgrammer> progsList)
        {
            _programmersListIsRequested = false;
            if (_programmer != null) return;
            foreach (IChipProgrammer icp in progsList)
            {
                if (icp is IOneWireProgrammer)
                {
                    _programmer = icp as IOneWireProgrammer;

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
