﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.Linq;


namespace mcprog
{
    class GenaAlfaResetter_rev5 : MicrochipBootloaderHardware, IAT88Programmer, IM24CXXProgrammer, IXerox0190Programmer, IPresetting
    {
        #region Constant
        //************* DEVICE SELECT TABLE (for future or reserve if comment)
        public const byte PASSWORD_NOT_SELECTED = 0x00;
        public const byte SELECTED_PASS_1 = 0x01;
        public const byte SELECTED_PASS_2 = 0x02;
        public const byte SELECTED_PASS_3 = 0x03;
        public const byte SELECTED_PASS_4 = 0x04;
        public const byte SELECTED_PASS_5 = 0x05;
        public const byte SELECTED_PASS_6 = 0x06;
        public const byte SELECTED_PASS_7 = 0x07;
        //public const byte SELECTED_PASS_8=0x08;
        public const byte XEROX90_SELECTED = 0x10;

        public const byte SS02_SELECTED = 0x0A;

        public const byte UNLOCK_3600 = 0x08;
        public const byte UNLOCK_3635 = 0x09;

        public const byte SELECTED_24C00 = 0x13;
        public const byte SELECTED_24C01 = 0x14;
        public const byte SELECTED_24C02 = 0x15;
        public const byte SELECTED_24C04 = 0x16;
        public const byte SELECTED_24C08 = 0x17;
        public const byte SELECTED_24C16 = 0x18;
        public const byte SELECTED_24C32 = 0x19;
        public const byte SELECTED_24C64 = 0x1A;
        public const byte SELECTED_24C128 = 0x1B;
        public const byte SELECTED_24C256 = 0x1C;
        public const byte SELECTED_24C512 = 0x1D;

        public const byte I2C_PWR_ON_TEST = 0x1F;
        //public const byte SELECTED_X24C00=0x1E;
        //public const byte SELECTED_X24C01=0x1F;
        public const byte PWR_2_50_V = 0x20;
        public const byte PWR_2_66_V = 0x21;
        public const byte PWR_2_83_V = 0x22;
        public const byte PWR_3_00_V = 0x23;
        public const byte PWR_3_16_V = 0x24;
        public const byte PWR_3_33_V = 0x25;
        public const byte PWR_3_50_V = 0x26;
        public const byte PWR_3_66_V = 0x27;
        public const byte PWR_3_83_V = 0x28;
        public const byte PWR_4_00_V = 0x29;
        public const byte PWR_4_16_V = 0x2A;
        public const byte PWR_4_33_V = 0x2B;
        public const byte PWR_4_50_V = 0x2C;
        

        //public const byte SELECTED_XEROX90=0x20;
        //public const byte SELECTED_XEROX90_RESTORER=0x21;
        public const byte SELECTED_POWER_I2C_ON = 0x2D;
        public const byte SELECTED_POWER_I2C_OFF = 0x2E;
        public const byte SELECTED_POWER_I2C_RESET = 0x2F;


        #endregion

        #region Fields

        #region Hardware errors dictionary

        Dictionary<byte, string> _hardwareErrors = new Dictionary<byte, string>
        {
            {0x01,"(01) DS1820 is not present in bus"},
            {0x02,"(02) DS1820 CRC error"},
            {0x03,"(03) DS1820 test not pased"},
            {0x04,"(04) DS1820 not carrect or short to gnd"},
            {0x05,"(05) AT88 password counters is end (insert CRUM to printer)"},
            {0x06,"(06) Autentication error (vrong password selected?)"},
            {0x07,"(07) DS1820 CRC error"},
            {0x08,"(08) DS1820 CRC error"},
            {0x09,"(09) DS1820 CRC error"},
            {0x0A,"(0A) Error in PIC counters"},
            {0x0B,"(0B) PIC security error (reinstall the bootloader)"},
            {0x0C,"(0C) DS1820 is not present in bus"},
            {0x0D,"(0D) DS1820 is not present in bus"},
            {0x0E,"(0E) DS1820 is not present in bus"},
            {0x0F,"(0F) DS1820 is not present in bus"},
            {0x10,"(10) DS1820 is not present in bus"},
            {0x11,"(11) DS1820 is not present in bus"},
            {0x12,"(12) DS1820 is not present in bus"},
            {0x13,"(13) Chip ASK error (may be chip is disconnected)"},
            {0x14,"(14) Chip second comand error (try to read locked zone?)"},
            {0x15,"(15) Chip send data error (may be bad contact in chip)"},
            {0x16,"(16) Invalid device number (reinstall firmware)"},
            {0x17,"(17) Password not accepted by AT88 (wrong password)"},
            {0x18,"(18) DS1820 bus is short to gnd"},
            {0x19,"(19) Write cycle is to long"},
            {0x1A,"(1A) DAT pin is short to gnd to long"},
            {0x1B,"(1B) Chip not redy at Power Up (may be chip is disconnected)"},
            {0x1C,"(1C) This SS02-1 chip version is not supperted by resetter"}
        };

        #endregion

        string _devName = "GenaAlfa Resetter v5";
        bool _isBusy = false;
        string _hardwarePath;
        byte _busSpeed = 128;
        EventHandler _busy;
        EventHandler _ready;
        HardwareMode _hardwareMode = HardwareMode.UnknownMode;
        GA5ResetterInfoForm infoForm = new GA5ResetterInfoForm();
        byte _newCoreResetter = 0;
        
        
        
        
        byte _crumVoltage = PWR_2_50_V;

        byte _24CXX_CrumVoltage = PWR_4_50_V;
        byte _AT88_CrumVoltage = PWR_3_33_V;
        byte _XC01_CrumVoltage = PWR_4_50_V;

        byte _24CXX_CrumAddress = 0xA0;
        byte _AT88_CrumAddress = 0xB0;
        byte _XC01_CrumAddress = 0xA0;


        string[] _24CXX_ChipAddresses = {"A0","A2","A4","A6","A8","AA","AC","AE"};
        

        IDictionary<string, byte> _at88PasswordSet = new Dictionary<string, byte> 
        { 
            {"Read Without Present Of Passwords (Safe Mode)", PASSWORD_NOT_SELECTED}, 
            {"Samsung 3050, 5525, 5530, 3470, XeroxPhaser 3300, 3428, 3435, Dell 1815", SELECTED_PASS_1},
            {"Samsung SCX-4725, Xerox Phaser 3200", SELECTED_PASS_2}, 
            {"Samsung ML-4550, Xerox Phaser 3600", SELECTED_PASS_3}, 
            {"Samsung ML-1630/1631, SF 560/565, SCX-4500", SELECTED_PASS_4}, 
            {"Samsung ML-2850, Xerox Phaser 3250", SELECTED_PASS_5}, 
            {"Samsung CLP-660/610, Xerox Phaser 3635", SELECTED_PASS_6}, 
            {"Samsung CLP-350", SELECTED_PASS_7}, 
            {"SS02-1", SS02_SELECTED}
        };
        IDictionary<string, byte> _m24CXXChipSet = new Dictionary<string, byte> 
        {
            {"24C00", SELECTED_24C00},
            {"24C01", SELECTED_24C01},
            {"24C02", SELECTED_24C02},
            {"24C04", SELECTED_24C04},
            {"24C08", SELECTED_24C08},
            {"24C16", SELECTED_24C16},
            {"24C32", SELECTED_24C32},
            {"24C64", SELECTED_24C64},
            {"24C128", SELECTED_24C128},
            {"24C256", SELECTED_24C256},
            {"24C512", SELECTED_24C512}
        };

        IDictionary<string, byte> _Xerox0190ModeSet = new Dictionary<string, byte>
        {
            {"Mode 0", 0}

        };

        IDictionary<string, byte> _voltageDict = new Dictionary<string, byte>
        {
            {"2.50 V",PWR_2_50_V},
            {"2.66 V",PWR_2_66_V},
            {"2.83 V",PWR_2_83_V},
            {"3.00 V",PWR_3_00_V},
            {"3.16 V",PWR_3_16_V},
            {"3.33 V",PWR_3_33_V},
            {"3.50 V",PWR_3_50_V},
            {"3.66 V",PWR_3_66_V},
            {"3.83 V",PWR_3_83_V},
            {"4.00 V",PWR_4_00_V},
            {"4.16 V",PWR_4_16_V},
            {"4.33 V",PWR_4_33_V}, 
            {"4.50 V",PWR_4_50_V}
            

        };
        IDictionary<byte, string> _voltageStringDict = new Dictionary<byte, string>
        {
            {PWR_2_50_V,"2.50 V"},
            {PWR_2_66_V,"2.66 V"},
            {PWR_2_83_V,"2.83 V"},
            {PWR_3_00_V,"3.00 V"},
            {PWR_3_16_V,"3.16 V"},
            {PWR_3_33_V,"3.33 V"},
            {PWR_3_50_V,"3.50 V"},
            {PWR_3_66_V,"3.66 V"},
            {PWR_3_83_V,"3.83 V"},
            {PWR_4_00_V,"4.00 V"},
            {PWR_4_16_V,"4.16 V"},
            {PWR_4_33_V,"4.33 V"}, 
            {PWR_4_50_V,"4.50 V"}

        };





        #endregion 

        #region IAT88Programmer Members

        ICollection<string> IAT88Programmer.SupportedChips
        {
            get { return _at88PasswordSet.Keys; }
        }
       
        int IAT88Programmer.ReadChip(string crumType, List<MemoryRegionInfo> regionsInfo, Action<ProgrammingCompleteInfo, List<MemoryRegion>> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                List<MemoryRegion> regions = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();
                byte[] password = new byte[11];
                uint packsize = 0x20;
                try
                {

                    if (crumType == "SS02-1")
                        if (_newCoreResetter < 0x08) throw new Exception("This function isn't supported by your version of firmware.");
                        else
                        {

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet[crumType], _AT88_CrumAddress));
                            UpdateCurrentMemRegConfig(_hardwarePath);
                            packsize = _bytesPerPacket;
                            
                            
                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet["Read Without Present Of Passwords (Safe Mode)"], _AT88_CrumAddress));

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_AT88_CrumVoltage, _AT88_CrumAddress));

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _AT88_CrumAddress));

                            UpdateCurrentMemRegConfig(_hardwarePath);

                            _bytesPerPacket = packsize;
                            
                            if ((_currentMemoryRegionsConfig.Count != 2))
                            {
                                throw new Exception("Invalid resetter configuration");
                            }
                            regions = new List<MemoryRegion>() { new MemoryRegion(_currentMemoryRegionsConfig[0]), new MemoryRegion(_currentMemoryRegionsConfig[1]) };

                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Preread user data")); }, null);
                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                            ReadRegion(_hardwarePath, regions[0], progressAct);

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_RESET, _AT88_CrumAddress));

                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Preread config data")); }, null);
                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                            ReadRegion(_hardwarePath, regions[1], progressAct);

                            
                            
                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));
                            
                            byte[] passreq = new byte[8];

                            Array.Copy(regions[1].Data, 0x10, passreq, 0, 8);

                            if ((passreq[6] != 0x90) || (passreq[7] != 0x1B)) throw new Exception("It not SS02-1 CRUM");

                            SS02DynpassGen passgen = new SS02DynpassGen(passreq);             
                            
                            //byte[] password = new byte[11];

                            //using( FileStream passfile = File.Open("pass.bin", FileMode.Open))
                            //{
                                //for(int i=0;i<11;i++)password[i]=(byte)passfile.ReadByte();

                            //};

                            Array.Copy(passgen.SecretSeed, 0, password, 0, 8);
                            Array.Copy(passgen.WritePass, 0, password, 8, 3);

                        }
                    
                    //SetTaskNumber(_hardwarePath, _at88PasswordSet[crumType]);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet[crumType], _AT88_CrumAddress), password);

                    UpdateCurrentMemRegConfig(_hardwarePath);
                    packsize = _bytesPerPacket;
                    
                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_AT88_CrumVoltage, _AT88_CrumAddress), password);


                    //UpdateCurrentMemRegConfig(_hardwarePath);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _AT88_CrumAddress), password);

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    _bytesPerPacket = packsize;

                    if ((_currentMemoryRegionsConfig.Count != 2))
                    {
                        throw new Exception("Invalid resetter configuration");
                    }
                    regions = new List<MemoryRegion>() { new MemoryRegion(_currentMemoryRegionsConfig[0]), new MemoryRegion(_currentMemoryRegionsConfig[1]) };

                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read user data")); }, null);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);



                    ReadRegion(_hardwarePath, regions[0], progressAct);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_RESET);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_RESET, _AT88_CrumAddress));

                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read config data")); }, null);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                    ReadRegion(_hardwarePath, regions[1], progressAct);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    
                   
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));

                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));

                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                asyncOp.Post(delegate(object args) { completed(pcInfo, regions); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }
       
        int IAT88Programmer.ProgramChip(string crumType, List<MemoryRegion> regions, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                byte[] password = new byte[11];

                uint packsize = 0x20;

                try
                {
                    if (crumType == "SS02-1")
                        if (_newCoreResetter < 0x08) throw new Exception("This function isn't supported by your version of firmware.");
                        else
                        {
                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet[crumType], _AT88_CrumAddress));
                            UpdateCurrentMemRegConfig(_hardwarePath);
                            packsize = _bytesPerPacket;
                            
                            
                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet["Read Without Present Of Passwords (Safe Mode)"], _AT88_CrumAddress));

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_AT88_CrumVoltage, _AT88_CrumAddress));

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _AT88_CrumAddress));

                            UpdateCurrentMemRegConfig(_hardwarePath);

                            _bytesPerPacket = packsize;

                            if ((_currentMemoryRegionsConfig.Count != 2))
                            {
                                throw new Exception("Invalid resetter configuration");
                            }
                            List<MemoryRegion> pr_regions = new List<MemoryRegion>() { new MemoryRegion(_currentMemoryRegionsConfig[0]), new MemoryRegion(_currentMemoryRegionsConfig[1]) };

                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Preread user data")); }, null);
                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                            ReadRegion(_hardwarePath, pr_regions[0], progressAct);

                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_RESET, _AT88_CrumAddress));

                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Preread config data")); }, null);
                            asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                            ReadRegion(_hardwarePath, pr_regions[1], progressAct);
                            
                            SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));

                            byte[] passreq = new byte[8];

                            Array.Copy(pr_regions[1].Data, 0x10, passreq, 0, 8);

                            if ((passreq[6] != 0x90) || (passreq[7] != 0x1B)) throw new Exception("It not SS02-1 CRUM");

                            SS02DynpassGen passgen = new SS02DynpassGen(passreq);

                            Array.Copy(passgen.SecretSeed, 0, password, 0, 8);
                            Array.Copy(passgen.WritePass, 0, password, 8, 3);

                            

                        }

                    
                    //SetTaskNumber(_hardwarePath, _at88PasswordSet[crumType]);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_at88PasswordSet[crumType], _AT88_CrumAddress),password);
                    UpdateCurrentMemRegConfig(_hardwarePath);
                    packsize = _bytesPerPacket;


                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_AT88_CrumVoltage, _AT88_CrumAddress), password);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _AT88_CrumAddress), password);

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    _bytesPerPacket = packsize;
                    
                    if ((_currentMemoryRegionsConfig.Count != 2))
                    {
                        throw new Exception("Invalid resetter configuration");
                    }

                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Programming")); }, null);

                    ProgramRegion(_hardwarePath, regions[0], progressAct);

                    MemoryRegion testRegion = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress), password);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _AT88_CrumAddress), password);


                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification")); }, null);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                    ReadRegion(_hardwarePath, testRegion, progressAct);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    for (int i = 0; i < regions[0].Size; i++)
                        if (regions[0].Data[i] != testRegion.Data[i])
                        {
                            throw new VerificationException(regions[0].Address + (uint)i, regions[0].Data[i], testRegion.Data[i]);
                        }


                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _AT88_CrumAddress));

                    pcInfo.error = e;
                }
                pcInfo.Message = "Programming complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                asyncOp.Post(delegate(object args) { completed(pcInfo); }, null);
                _isBusy = false;

                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        #endregion

        #region IChipProgrammer Members

        event EventHandler IChipProgrammer.Busy
        {
            add
            {
                _busy = (EventHandler)Delegate.Combine(_busy, value);
            }
            remove
            {
                _busy = (EventHandler)Delegate.Remove(_busy, value);
            }
        }

        event EventHandler IChipProgrammer.Ready
        {
            add
            {
                _ready = (EventHandler)Delegate.Combine(_ready, value);
            }
            remove
            {
                _ready = (EventHandler)Delegate.Remove(_ready, value);
            }
        }

        bool IChipProgrammer.IsBusy
        {
            get { return _isBusy; }
        }

        System.Windows.Forms.Form IChipProgrammer.GetServiceWindow()
        {
            return new BootloaderForm(this as MicrochipBootloaderHardware);
        }
        
        HardwareMode IChipProgrammer.GetMode()
        {
            if (!_isBusy)
            {
                _isBusy = true;
                OnBusy();
                try
                {

                    UpdateCurrentMemRegConfig(_hardwarePath);
                    if (_currentMemoryRegionsConfig.Count == 4)
                        _hardwareMode = HardwareMode.ServiceMode;
                    else
                        _hardwareMode = HardwareMode.ProgrammerMode;

                }
                catch (Exception)
                {
                    _hardwareMode = HardwareMode.UnknownMode;
                }
                _isBusy = false;
                OnReady();
            }
            return _hardwareMode;
        }

        #endregion

        #region IM24CXXProgrammer Members

        ICollection<string> IM24CXXProgrammer.SupportedChips
        {
            get { return _m24CXXChipSet.Keys; }
        }

        /*
        int IM24CXXProgrammer.ReadChip(string chip, MemoryRegionInfo regionInfo, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;


            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                MemoryRegion region = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        SetTaskNumber(harwareStream, _m24CXXChipSet[chip]);

                        SetTaskNumber(harwareStream, _crumVoltage);

                        SetTaskNumber(harwareStream, SELECTED_POWER_I2C_ON);

                        UpdateCurrentMemRegConfig(harwareStream);

                        if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != regionInfo))
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }
                        region = new MemoryRegion(_currentMemoryRegionsConfig[0]);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read")); }, null);
                        ReadRegion(harwareStream, region, progressAct);

                        SetTaskNumber(harwareStream, SELECTED_POWER_I2C_OFF);
                    }

                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }
        */

        int IM24CXXProgrammer.ReadChip(string chip, MemoryRegionInfo regionInfo, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;


            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            
            
            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                MemoryRegion region = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                

                try
                {
                   
                    //SetTaskNumber(_hardwarePath, _m24CXXChipSet[chip]);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_m24CXXChipSet[chip], _24CXX_CrumAddress));

                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_24CXX_CrumVoltage, _24CXX_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);
                    
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _24CXX_CrumAddress));

                    if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != regionInfo))
                    {
                        Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                        Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                        string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                            e2.Message.Substring(0, e2.Message.IndexOf("("));
                        throw new Exception(message);
                    }
                    
                    region = new MemoryRegion(_currentMemoryRegionsConfig[0]);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read")); }, null);
                    ReadRegion(_hardwarePath, region, progressAct);
                    
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _24CXX_CrumAddress));

                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _24CXX_CrumAddress));
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        /*
        int IM24CXXProgrammer.ProgramChip(string chip, MemoryRegion region, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());


            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        SetTaskNumber(harwareStream, _m24CXXChipSet[chip]);

                        SetTaskNumber(harwareStream, _crumVoltage);

                        SetTaskNumber(harwareStream, SELECTED_POWER_I2C_ON);

                        UpdateCurrentMemRegConfig(harwareStream);

                        if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != region.GetRegionInfo()))
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }


                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Programming")); }, null);

                        ProgramRegion(harwareStream, region, progressAct);

                        MemoryRegion testRegion = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                        ReadRegion(harwareStream, testRegion, progressAct);

                        SetTaskNumber(harwareStream, SELECTED_POWER_I2C_OFF);

                        UpdateCurrentMemRegConfig(harwareStream);

                        for (int i = 0; i < region.Size; i++)
                            if (region.Data[i] != testRegion.Data[i])
                            {
                                throw new VerificationException(region.Address + (uint)i, region.Data[i], testRegion.Data[i]);
                            }
                    }

                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                pcInfo.Message = "Programming complete";
                asyncOp.Post(delegate(object args) { completed(pcInfo); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        } 
        */

        int IM24CXXProgrammer.ProgramChip(string chip, MemoryRegion region, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());


            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                try
                {

                    //SetTaskNumber(_hardwarePath, _m24CXXChipSet[chip]);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_m24CXXChipSet[chip], _24CXX_CrumAddress));

                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_24CXX_CrumVoltage, _24CXX_CrumAddress));

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _24CXX_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != region.GetRegionInfo()))
                    {
                        Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                        Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                        string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                            e2.Message.Substring(0, e2.Message.IndexOf("("));
                        throw new Exception(message);
                    }


                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Programming")); }, null);

                    ProgramRegion(_hardwarePath, region, progressAct);

                    MemoryRegion testRegion = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification")); }, null);
                    asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                    Thread.Sleep(300);


                    ReadRegion(_hardwarePath, testRegion, progressAct);

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _24CXX_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    for (int i = 0; i < region.Size; i++)
                        if (region.Data[i] != testRegion.Data[i])
                        {
                            throw new VerificationException(region.Address + (uint)i, region.Data[i], testRegion.Data[i]);
                        }


                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _24CXX_CrumAddress));

                    pcInfo.error = e;
                }
                pcInfo.Message = "Programming complete";
                asyncOp.Post(delegate(object args) { completed(pcInfo); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        #endregion

        #region IXerox0190Programmer Members

        ICollection<string> IXerox0190Programmer.SupportedModes
        {
            get { return _Xerox0190ModeSet.Keys; }
        }

        int IXerox0190Programmer.ReadChip(string mode, MemoryRegionInfo regionInfo, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;


            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                MemoryRegion region = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                //List<MemoryRegion> virtualRegions = new List<MemoryRegion>();
                IDictionary<string, MemoryRegion> virtualRegions = new Dictionary<string, MemoryRegion>
                {
                    {"Read addresses from 00 to AF", new MemoryRegion(0x00, 0xB0, 1)},
                    {"Read addresses from B8 to FF", new MemoryRegion(0xB8, 0x48, 1)}
                };
                try
                {

                    //SetTaskNumber(_hardwarePath, XEROX90_SELECTED);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(XEROX90_SELECTED, _XC01_CrumAddress));

                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_XC01_CrumVoltage, _XC01_CrumAddress));

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _XC01_CrumAddress));
                    
                    UpdateCurrentMemRegConfig(_hardwarePath);


                    region = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                    if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != regionInfo))
                    {
                        Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                        Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                        string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                            e2.Message.Substring(0, e2.Message.IndexOf("("));
                        throw new Exception(message);
                    }

                    foreach (KeyValuePair<string, MemoryRegion> vr in virtualRegions)
                    {
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(vr.Key)); }, null);
                        ReadRegion(_hardwarePath, vr.Value, progressAct);
                    }

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _XC01_CrumAddress));

                    foreach (MemoryRegion vreg in virtualRegions.Values)
                    {
                        region.WriteData(vreg.Address, vreg.Data);
                    }
                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _XC01_CrumAddress));
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int IXerox0190Programmer.ProgramChip(string mode, MemoryRegion region, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());


            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                IDictionary<string, MemoryRegion> virtualRegions = new Dictionary<string, MemoryRegion>
                {
                    {"addresses from 00 to AF", new MemoryRegion(0x00, 0xB0, 1)},
                    {"addresses from B8 to FF", new MemoryRegion(0xB8, 0x48, 1)}
                };

                foreach (MemoryRegion vreg in virtualRegions.Values)
                {
                    region.ReadData(vreg.Address, vreg.Data);
                }

                try
                {

                    //SetTaskNumber(_hardwarePath, XEROX90_SELECTED);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(XEROX90_SELECTED, _XC01_CrumAddress));

                    //SetTaskNumber(_hardwarePath, _crumVoltage);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(_XC01_CrumVoltage, _XC01_CrumAddress));

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_ON);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_ON, _XC01_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    if ((_currentMemoryRegionsConfig.Count != 1) || (_currentMemoryRegionsConfig[0] != region.GetRegionInfo()))
                    {
                        Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                        Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                        string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                            e2.Message.Substring(0, e2.Message.IndexOf("("));
                        throw new Exception(message);
                    }

                    foreach (KeyValuePair<string, MemoryRegion> vr in virtualRegions)
                    {
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Programming "+vr.Key)); }, null);
                        ProgramRegion(_hardwarePath, vr.Value, progressAct);
                    }
 

                    MemoryRegion testRegion = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                    IDictionary<string, MemoryRegion> testVirtualRegions = new Dictionary<string, MemoryRegion>
                    {
                        {"addresses from 00 to AF", new MemoryRegion(0x00, 0xB0, 1)},
                        {"addresses from B8 to FF", new MemoryRegion(0xB8, 0x48, 1)}
                    };


                   // asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification")); }, null);
                    //asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                    //ReadRegion(_hardwarePath, testRegion, progressAct);

                    foreach (KeyValuePair<string, MemoryRegion> vr in testVirtualRegions)
                    {
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification "+vr.Key)); }, null);
                        ReadRegion(_hardwarePath, vr.Value, progressAct);
                    }

                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _XC01_CrumAddress));

                    UpdateCurrentMemRegConfig(_hardwarePath);

                    

                    foreach (KeyValuePair<string, MemoryRegion> vr in virtualRegions)
                    {
                        for (int i = 0; i < vr.Value.Size; i++)
                            if (vr.Value.Data[i] != testVirtualRegions[vr.Key].Data[i])
                            {
                                throw new VerificationException(vr.Value.Address + (uint)i, vr.Value.Data[i], testVirtualRegions[vr.Key].Data[i]);
                            }
                    }



                }
                catch (Exception e)
                {
                    //SetTaskNumber(_hardwarePath, SELECTED_POWER_I2C_OFF);
                    SetCrum(_hardwarePath, CrumSelect.GetCrumSelect(SELECTED_POWER_I2C_OFF, _XC01_CrumAddress));
                    pcInfo.error = e;
                }
                pcInfo.Message = "Programming complete";
                asyncOp.Post(delegate(object args) { completed(pcInfo); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        #endregion

        #region IPresetting Members

        System.Windows.Forms.Form IPresetting.GetPresettingWindow()
        {
            
            return infoForm;
        }

        void IPresetting.ClosePresettingWindow()
        {
            infoForm.FormClosing -= infoForm_FormClosing;
            infoForm.Close();
        }

        void IPresetting.HidePresettingWindow()
        {
            infoForm.Hide();
        }

        void IPresetting.ShowPresettingWindow()
        {
            infoForm.Show();
        }

        #endregion

        #region Ctors
        public GenaAlfaResetter_rev5(string path)
        {
            _hardwarePath = path;
            //infoForm.voltageSelectComboBox.SelectedIndexChanged+=VoltageSelectComboBox_SelectedIndexChanged;
            infoForm.FormClosing += infoForm_FormClosing;
            //infoForm.voltageSelectComboBox.Items.AddRange(_voltageDict.Keys.ToArray());
            
            infoForm.enablePowerCheckBox.CheckStateChanged += new EventHandler(enablePowerCheckBox_CheckStateChanged);
            infoForm.resetButton.Click += resetButton_Click;
            infoForm.unlock3600.Click += unlock3600_Click;
            infoForm.unlock3635.Click += unlock3635_Click;
            
            infoForm.M24CXX_VoltageComboBox.Items.AddRange(_voltageDict.Keys.ToArray());
            infoForm.M24CXX_VoltageComboBox.SelectedItem = _voltageStringDict[PWR_4_50_V];
            infoForm.M24CXX_VoltageComboBox.SelectedValueChanged += M24CXX_VoltageComboBox_SelectedIndexChanged;
            
            infoForm.AT88_VoltageComboBox.Items.AddRange(_voltageDict.Keys.ToArray());
            infoForm.AT88_VoltageComboBox.SelectedItem = _voltageStringDict[PWR_3_33_V];
            infoForm.AT88_VoltageComboBox.SelectedValueChanged += AT88_VoltageComboBox_SelectedIndexChanged;
            
            infoForm.XC01_VoltageComboBox.Items.AddRange(_voltageDict.Keys.ToArray());
            infoForm.XC01_VoltageComboBox.SelectedItem = _voltageStringDict[PWR_4_50_V];
            infoForm.XC01_VoltageComboBox.SelectedValueChanged += XC01_VoltageComboBox_SelectedIndexChanged;
            
            infoForm.M24CXX_AddressComboBox.Items.AddRange(_24CXX_ChipAddresses);
            infoForm.M24CXX_AddressComboBox.SelectedItem = "A0";
            infoForm.M24CXX_AddressComboBox.SelectedValueChanged += M24CXX_AddressComboBox_SelectedIndexChanged;
            infoForm.scan24CXXAddressButton.Click +=scan24CXXAddressButton_Click; 

            infoForm.XC01_AddressComboBox.Items.AddRange(_24CXX_ChipAddresses);
            infoForm.XC01_AddressComboBox.SelectedItem = "A0";
            infoForm.XC01_AddressComboBox.SelectedValueChanged += XC01_AddressComboBox_SelectedIndexChanged;
            infoForm.scanXC01AddressButton.Click += scanXC01AddressButton_Click; 
        
        }

        
        #endregion

        #region Service mode members

        public override void ReadEEPROM(Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {

            MemoryRegionInfo regionInfo = new MemoryRegionInfo() { Address = 0xF00000, Size = 256, Type = 2 };



            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                MemoryRegion region = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {


                        UpdateCurrentMemRegConfig(harwareStream);

                        if (_currentMemoryRegionsConfig.Count != 4)
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }
                        region = new MemoryRegion(regionInfo);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ReadRegion(harwareStream, region, progressAct);
                    }

                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.PostOperationCompleted(delegate(object args) { completed(pcInfo, region); }, null);

            };

            (new Thread(start)).Start();


        }

        public override void ProgramFirmware(List<MemoryRegion> regions, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {


                        //UpdateCurrentMemRegConfig(harwareStream);

                        if (_currentMemoryRegionsConfig.Count != 4)
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }

                        EraseHardware(harwareStream);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Config data programming")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ProgramRegion(harwareStream, regions[1], progressAct);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Program data programming")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ProgramRegion(harwareStream, regions[0], progressAct);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Setting data programming")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ProgramRegion(harwareStream, regions[2], progressAct);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("EEPROM programming")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ProgramRegion(harwareStream, regions[3], progressAct);

                    }

                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                pcInfo.Message = "Programming complete";
                asyncOp.PostOperationCompleted(delegate(object args) { completed(pcInfo); }, null);

            };

            (new Thread(start)).Start();



        }

        #endregion

        #region OnEvent methods

        protected virtual void OnBusy()
        {
            
            infoForm.SetBusyState();
            EventHandler localHandler = _busy;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }
        }

        protected virtual void OnReady()
        {

            infoForm.SetReadyState();
            
            EventHandler localHandler = _ready;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }

        }

        #endregion

        #region Methods

        #region Event Handlers

        private void VoltageSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _crumVoltage = _voltageDict[(string)(((ComboBox)sender).SelectedItem)];
            if (infoForm.enablePowerCheckBox.Checked)
            {
                //_isBusy = true;
                //OnBusy();
                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        SetTaskNumber(_hardwarePath, _crumVoltage);
                        //SetTaskNumber(harwareStream, I2C_PWR_ON_TEST);
                    }

                }
                catch (Exception)
                {
                    //_isBusy = false;
                    //OnReady();

                }


            }
        
        
        }

        private void M24CXX_VoltageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _24CXX_CrumVoltage = _voltageDict[infoForm.M24CXX_VoltageComboBox.SelectedItem.ToString()];
        }
        
        private void AT88_VoltageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AT88_CrumVoltage = _voltageDict[infoForm.AT88_VoltageComboBox.SelectedItem.ToString()];
        }
        
        private void XC01_VoltageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _XC01_CrumVoltage = _voltageDict[infoForm.XC01_VoltageComboBox.SelectedItem.ToString()];
        }
        
        private void M24CXX_AddressComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _24CXX_CrumAddress = Convert.ToByte(infoForm.M24CXX_AddressComboBox.SelectedItem.ToString(), 16);
        }
        
        private void XC01_AddressComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _XC01_CrumAddress = Convert.ToByte(infoForm.XC01_AddressComboBox.SelectedItem.ToString(), 16);
        }
        
        private void infoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void enablePowerCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (infoForm.enablePowerCheckBox.Checked)
            {
                _isBusy = true;
                OnBusy();
                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        //SetTaskNumber(_hardwarePath, _crumVoltage);
                        SetTaskNumber(harwareStream, I2C_PWR_ON_TEST);
                    }

                }
                catch (Exception)
                {
                    _isBusy = false;
                    OnReady();

                }


            }
            else
            {
                //_isBusy = false;
                //OnReady();
                try
                {
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        SetTaskNumber(harwareStream, SELECTED_POWER_I2C_OFF);
                    }

                }
                catch (Exception)
                {


                }
                _isBusy = false;
                OnReady();

            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetHardware(_hardwarePath);

        }

        private void unlock3635_Click(object sender, EventArgs e)
        {
            SetTaskNumber(_hardwarePath, UNLOCK_3635);
        }

        private void unlock3600_Click(object sender, EventArgs e)
        {
            SetTaskNumber(_hardwarePath, UNLOCK_3600);
        }

        private void scan24CXXAddressButton_Click(object sender, EventArgs e)
        {
            
            _isBusy = true;

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                byte busAddr = 0;

                try
                {
                    busAddr = ScanChipBusAddress(0xA0, 0xB0, PWR_4_50_V);
                    
                    string bas = (busAddr&0xFE).ToString("X2");
                    
                    if(busAddr > 0)asyncOp.Post(delegate(object args) 
                    {
                        infoForm.M24CXX_AddressComboBox.SelectedItem = (string)(((object[])args)[0]);
                    }, new object[] { bas });

                }
                catch (Exception)
                {
                    
                }
                
                _isBusy = false;
                asyncOp.Post(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();


        }

        private void scanXC01AddressButton_Click(object sender, EventArgs e)
        {

            _isBusy = true;

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                byte busAddr = 0;

                try
                {
                    busAddr = ScanChipBusAddress(0xA0, 0xB0, PWR_4_50_V);

                    string bas = (busAddr & 0xFE).ToString("X2");

                    if (busAddr > 0) asyncOp.Post(delegate(object args)
                    {
                        infoForm.XC01_AddressComboBox.SelectedItem = (string)(((object[])args)[0]);
                    }, new object[] { bas });

                }
                catch (Exception)
                {

                }

                _isBusy = false;
                asyncOp.Post(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();


        }

        
        #endregion

        public void UpdateCurrentMemRegConfig(string path)
        {

            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                 Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            SafeFileHandle asyncReadHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_READ,
                Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

            if (asyncReadHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            uint wrBytes;



            if (!WriteFile(asyncWriteHandle, QueryDeviceReq.getCommandPacket(), 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }
            
            byte[] respdata = new byte[65];


            if (!ReadFile(asyncReadHandle, respdata, 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }
            
            QueryResultsResp3 response = QueryResultsResp3.getQueryResultsResp(respdata);
            _currentMemoryRegionsConfig = response.MemoryRegionsInfo;
            _bytesPerPacket = (uint)response.BytesPerPacket;
            _busSpeed = response.BusSpeed;
            _newCoreResetter = response.NewCoreResetter;

            if (response.infoIsTrue)
            {
                if (infoForm.InvokeRequired)
                {
                    infoForm.Invoke(
                        (Action)delegate()
                        {
                            string s = "";
                            foreach (byte b in response.DS_Number)
                                s += b.ToString("X2");
                            infoForm.hardwareNumberLabel.Text = s;
                            s = "";
                            foreach (byte b in response.FirmwareVersion)
                                s += b.ToString("X2");
                            infoForm.firmwareVersionLabel.Text = s;
                            infoForm.writesCounterLabel.Text = (response.WritesCounterHi * 256 + response.WritesCounterLo).ToString();
                            infoForm.curretVoltageLabel.Text = _voltageStringDict[response.CrumVoltage];


                        }
                    );



                }
                else
                {
                    string s = "";
                    foreach (byte b in response.DS_Number)
                        s += b.ToString("X2");
                    infoForm.hardwareNumberLabel.Text = s;
                    s = "";
                    foreach (byte b in response.FirmwareVersion)
                        s += b.ToString("X2");
                    infoForm.firmwareVersionLabel.Text = s;
                    infoForm.writesCounterLabel.Text = (response.WritesCounterHi * 256 + response.WritesCounterLo).ToString();
                    infoForm.curretVoltageLabel.Text = _voltageStringDict[response.CrumVoltage];

                }
            }
            asyncReadHandle.Close();
            asyncWriteHandle.Close();
        }

        private void MemToArray(MemSafeHandle p, byte[] array)
        {
            for (int i = 0; i < 65; i++)
            {
                array[i] = Marshal.ReadByte(p.DangerousGetHandle(), i);
            }


        }
        
        private void ArrayToMem(byte[] array, MemSafeHandle p)
        {
            for (int i = 0; i < 65; i++)
            {
                Marshal.WriteByte(p.DangerousGetHandle(), i, array[i]);
            }

        }

        public int ReadRegion(string path, MemoryRegion region, Action<int, int> progress)
        {

            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, Win32HardwareIOSupport.FILE_FLAG_OVERLAPPED | Win32HardwareIOSupport.FILE_FLAG_NO_BUFFERING, IntPtr.Zero);

            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            SafeFileHandle asyncReadHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_READ | Win32HardwareIOSupport.GENERIC_WRITE,
                Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, Win32HardwareIOSupport.FILE_FLAG_OVERLAPPED | Win32HardwareIOSupport.FILE_FLAG_NO_BUFFERING, IntPtr.Zero);

            if (asyncReadHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }


            int currentPacketLength=0;
            uint trBytes;
            int pendingPackets = 0;
            int noRequestedBytes = (int)region.Size;
            int noReadedBytes = (int)region.Size;
            uint currentAddress = region.Address;
            byte lastError = 0;


            EventWaitHandle hWrite1Event = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventWaitHandle hWrite2Event = new EventWaitHandle(false, EventResetMode.AutoReset);
            EventWaitHandle hReadEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            
            NativeOverlapped write1Over = new NativeOverlapped();
            write1Over.OffsetHigh = 0;
            write1Over.OffsetLow = 0;
            write1Over.EventHandle = hWrite1Event.SafeWaitHandle.DangerousGetHandle();
            
            NativeOverlapped write2Over = new NativeOverlapped();
            write2Over.OffsetHigh = 0;
            write2Over.OffsetLow = 0;
            write2Over.EventHandle = hWrite2Event.SafeWaitHandle.DangerousGetHandle();
            
            NativeOverlapped readOver = new NativeOverlapped();
            readOver.OffsetHigh = 0;
            readOver.OffsetLow = 0;
            readOver.EventHandle = hReadEvent.SafeWaitHandle.DangerousGetHandle();

            MemSafeHandle pWrite1 = new MemSafeHandle(Marshal.AllocHGlobal(65));
            MemSafeHandle pWrite2 = new MemSafeHandle(Marshal.AllocHGlobal(65));
            MemSafeHandle pRead = new MemSafeHandle(Marshal.AllocHGlobal(65));



            currentPacketLength = (int)_bytesPerPacket < noRequestedBytes ? (int)_bytesPerPacket : noRequestedBytes;
            ArrayToMem(GetDataReq.getCommandPacket(currentAddress, (byte)currentPacketLength), pWrite1);
            if (!WriteFile(asyncWriteHandle, pWrite1.DangerousGetHandle(),
                65, out trBytes, ref write1Over))
            {
                if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
            else
            {
                hWrite1Event.Set();
            }
            pendingPackets++;
            noRequestedBytes -= currentPacketLength;
            currentAddress += (uint)currentPacketLength;
            if (noRequestedBytes > 0)
            {
                currentPacketLength = (int)_bytesPerPacket < noRequestedBytes ? (int)_bytesPerPacket : noRequestedBytes;
                ArrayToMem(GetDataReq.getCommandPacket(currentAddress, (byte)currentPacketLength), pWrite2);
                if (!WriteFile(asyncWriteHandle, pWrite2.DangerousGetHandle(),
                    65, out trBytes, ref write2Over))
                {
                    if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                }
                else
                {
                    hWrite2Event.Set();
                }
                pendingPackets++;
                noRequestedBytes -= currentPacketLength;
                currentAddress += (uint)currentPacketLength;
            }

            byte[] rawdata = new byte[65];

            if (!ReadFile(asyncReadHandle, pRead.DangerousGetHandle(),
                    65, out trBytes, ref readOver))
            {
                if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
            else
            {
                hReadEvent.Set();
            }
            
            
            
            while (noReadedBytes > 0)
            {
                if (WaitHandle.WaitAny(new WaitHandle[] { hWrite1Event, hWrite2Event, hReadEvent }, 10000) == WaitHandle.WaitTimeout)
                {
                    throw new Exception("Hardware doesn't answer.  Read time out. no readed bytes:" + noReadedBytes.ToString()
                        + " pending packets:" + pendingPackets.ToString()
                        + " no requested bytes:" + noRequestedBytes.ToString()
                        + " current addr:" + currentAddress.ToString());

                }

                if ((write1Over.InternalLow.ToInt32() != 0x103) && (pendingPackets < 10) && (noRequestedBytes > 0))
                {
                    write1Over.OffsetHigh = 0;
                    write1Over.OffsetLow = 0;
                    write1Over.InternalLow = IntPtr.Zero;
                    
                    write1Over.InternalHigh = IntPtr.Zero;
                    
                    currentPacketLength = (int)_bytesPerPacket < noRequestedBytes ? (int)_bytesPerPacket : noRequestedBytes;
                    ArrayToMem(GetDataReq.getCommandPacket(currentAddress, (byte)currentPacketLength), pWrite1);
                    if (!WriteFile(asyncWriteHandle, pWrite1.DangerousGetHandle(),
                        65, out trBytes, ref write1Over))
                    {
                        if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }
                    }
                    else
                    {
                        hWrite1Event.Set();
                    }
                    pendingPackets++;
                    noRequestedBytes -= currentPacketLength;
                    currentAddress += (uint)currentPacketLength;

                }

                if ((write2Over.InternalLow.ToInt32() != 0x103) && (pendingPackets < 10) && (noRequestedBytes > 0))
                {
                    write2Over.OffsetHigh = 0;
                    write2Over.OffsetLow = 0;
                    write2Over.InternalLow = IntPtr.Zero;
                    
                    write2Over.InternalHigh = IntPtr.Zero;
                    
                    currentPacketLength = (int)_bytesPerPacket < noRequestedBytes ? (int)_bytesPerPacket : noRequestedBytes;
                    ArrayToMem(GetDataReq.getCommandPacket(currentAddress, (byte)currentPacketLength), pWrite2);
                    if (!WriteFile(asyncWriteHandle, pWrite2.DangerousGetHandle(),
                        65, out trBytes, ref write2Over))
                    {
                        if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }
                    }
                    else
                    {
                        hWrite2Event.Set();
                    }
                    pendingPackets++;
                    noRequestedBytes -= currentPacketLength;
                    currentAddress += (uint)currentPacketLength;

                }
                if ((readOver.InternalLow.ToInt32() != 0x103))
                {

                    MemToArray(pRead, rawdata);
                    GetDataResultsResp3 response = new GetDataResultsResp3(rawdata);

                    pendingPackets--;
                    lastError = response.LastError;
                    noReadedBytes -= response.BytesPerPacket;

                    for (uint i = 0, j = response.Address - region.Address; i < response.BytesPerPacket; i++, j++)
                    {
                        region.Data[j] = response.Data[i];
                    }
                    progress((int)(region.Size - noReadedBytes), (int)region.Size);

                    if (pendingPackets > 0)
                    {
                        readOver.OffsetHigh = 0;
                        readOver.OffsetLow = 0;
                        readOver.InternalLow = IntPtr.Zero;
                        

                        readOver.InternalHigh = IntPtr.Zero;
                        if (!ReadFile(asyncReadHandle, pRead.DangerousGetHandle(),
                            65, out trBytes, ref readOver))
                        {
                            if (Marshal.GetLastWin32Error() != Win32HardwareIOSupport.ERROR_IO_PENDING)
                            {
                                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                            }
                        }
                        else
                        {
                            hReadEvent.Set();
                        }

                    }

                }


            }

            asyncWriteHandle.Close();
            asyncReadHandle.Close();
            if (lastError != 0)
            {
                string error = null;
                try
                {
                    error = _hardwareErrors[lastError];
                }
                catch (KeyNotFoundException)
                {
                    error = "Unknown hardware error: " + lastError.ToString();
                }
                throw new Exception(error);
            }
            return 0;
        }
       
        public int ProgramRegion(string path, MemoryRegion region, Action<int, int> progress)
        {
            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            int noWriteBytes = (int)region.Size;
            uint currentAddress = region.Address;
            int currentPacketLength;
            uint trBytes;
            

            while (noWriteBytes > 0)
            {

                currentPacketLength = (int)_bytesPerPacket < noWriteBytes ? (int)_bytesPerPacket : noWriteBytes;
                byte[] data = new byte[currentPacketLength];
                for (uint i = 0, j = currentAddress - region.Address; i < currentPacketLength; i++, j++)
                {
                    data[i] = region.Data[j];
                }
                if (!WriteFile(asyncWriteHandle, ProgramDeviceReq.getCommandPacket(currentAddress, data),
                65, out trBytes, IntPtr.Zero))
                {

                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
                noWriteBytes -= currentPacketLength;
                currentAddress += (uint)currentPacketLength;
                progress((int)(region.Size - noWriteBytes), (int)region.Size);
            }



            if (!WriteFile(asyncWriteHandle, ProgramCompleteReq.getCommandPacket(),
                    65, out trBytes, IntPtr.Zero))
            {


                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            
            asyncWriteHandle.Close();
            
            return 0;
        }

        public void SetCrum(string path, CrumSelect crs)
        {
            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            uint wrBytes;

            byte[] trbytes = new byte[65];
            trbytes[1] = crs.Command;
            trbytes[2] = crs.PassIndex;
            trbytes[5] = crs.I2CBusAddr;
            trbytes[6] = _busSpeed;

            if (!WriteFile(asyncWriteHandle, trbytes, 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            asyncWriteHandle.Close();

        }

        public void SetCrum(string path, CrumSelect crs, byte[] password)
        {
            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            uint wrBytes;

            byte[] trbytes = new byte[65];
            trbytes[1] = crs.Command;
            trbytes[2] = crs.PassIndex;
            trbytes[5] = crs.I2CBusAddr;
            trbytes[6] = _busSpeed;

            Array.Copy(password, 0, trbytes, 7, password.Length);

            if (!WriteFile(asyncWriteHandle, trbytes, 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            asyncWriteHandle.Close();

        }

        private byte ScanChipBusAddress(byte startAddr, byte endAddr, byte voltage)
        {
            byte result = 0;
            byte[] wrdata = new byte[65];
            wrdata[0] = 0;
            wrdata[1] = 0x14;
            wrdata[2] = 0;
            wrdata[3] = startAddr;
            wrdata[4] = endAddr;
            wrdata[5] = voltage;

            SafeFileHandle WriteHandle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE,
                Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

            if (WriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            SafeFileHandle ReadHandle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_READ,
                Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

            if (ReadHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            uint wrBytes;



            if (!WriteFile(WriteHandle, wrdata, 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            byte[] rddata = new byte[65];


            if (!ReadFile(ReadHandle, rddata, 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            result = rddata[3];

            return result;
        }
        
        #endregion

        #region Overriden

        public override void UpdateCurrentMemRegConfig(FileStream hardwareFileStream)
        {

            Thread th = new Thread(
                delegate()
                {
                    hardwareFileStream.Write(QueryDeviceReq.getCommandPacket(), 0, 65);
                }
                );
            th.Start();
            if (!th.Join(TIME_OUT))
            {
                th.Abort();
                new Exception("Attached hardware not answer");
            }
            byte[] respdata = new byte[65];
            th = new Thread(
                delegate()
                {
                    hardwareFileStream.Read(respdata, 0, 65);
                }
                );
            th.Start();
            if (!th.Join(TIME_OUT))
            {
                th.Abort();
                new Exception("Attached hardware not answer");
            }
            QueryResultsResp3 response = QueryResultsResp3.getQueryResultsResp(respdata);
            _currentMemoryRegionsConfig = response.MemoryRegionsInfo;
            _bytesPerPacket = (uint)response.BytesPerPacket;

            if (response.infoIsTrue)
            {
                if (infoForm.InvokeRequired)
                {
                    infoForm.Invoke(
                        (Action)delegate()
                        {
                            string s = "";
                            foreach (byte b in response.DS_Number)
                                s += b.ToString("X2");
                            infoForm.hardwareNumberLabel.Text = s;
                            s = "";
                            foreach (byte b in response.FirmwareVersion)
                                s += b.ToString("X2");
                            infoForm.firmwareVersionLabel.Text = s;
                            infoForm.writesCounterLabel.Text = (response.WritesCounterHi * 256 + response.WritesCounterLo).ToString();
                            //infoForm.voltageSelectComboBox.SelectedIndexChanged -= VoltageSelectComboBox_SelectedIndexChanged;
                            
                            try
                            {
                                //infoForm.voltageSelectComboBox.SelectedItem = _voltageStringDict[response.CrumVoltage];
                                _crumVoltage = response.CrumVoltage;
                            }
                            catch (KeyNotFoundException)
                            {
                               // infoForm.voltageSelectComboBox.SelectedItem = _voltageStringDict[PWR_3_33_V];
                                _crumVoltage = PWR_3_33_V;
                            }
                            //infoForm.voltageSelectComboBox.SelectedIndexChanged += VoltageSelectComboBox_SelectedIndexChanged;

                        }
                    );



                }
                else
                {
                    string s = "";
                    foreach (byte b in response.DS_Number)
                        s += b.ToString("X2");
                    infoForm.hardwareNumberLabel.Text = s;
                    s = "";
                    foreach (byte b in response.FirmwareVersion)
                        s += b.ToString("X2");
                    infoForm.writesCounterLabel.Text = (response.WritesCounterHi * 256 + response.WritesCounterLo).ToString();
                    //infoForm.voltageSelectComboBox.SelectedIndexChanged -= VoltageSelectComboBox_SelectedIndexChanged;
                    
                    try
                    {
                        //infoForm.voltageSelectComboBox.SelectedItem = _voltageStringDict[response.CrumVoltage];
                        _crumVoltage = response.CrumVoltage;
                    }
                    catch (KeyNotFoundException)
                    {
                        //infoForm.voltageSelectComboBox.SelectedItem = _voltageStringDict[PWR_3_33_V];
                        _crumVoltage = PWR_3_33_V;
                    }
                    //infoForm.voltageSelectComboBox.SelectedIndexChanged += VoltageSelectComboBox_SelectedIndexChanged;
                }
            }
        }
       
        public override int ReadRegion(FileStream hardwareFileStream, MemoryRegion region, Action<int, int> progress)
        {
            uint currentPacketLength;
            uint noTransferredBytes = region.Size;
            uint currentAddress = region.Address;

            while (noTransferredBytes > 0)
            {
                currentPacketLength = _bytesPerPacket < noTransferredBytes ? _bytesPerPacket : noTransferredBytes;

                hardwareFileStream.Write(GetDataReq.getCommandPacket(currentAddress, (byte)currentPacketLength), 0, 65);

                byte[] rawdata = new byte[65];

                hardwareFileStream.Read(rawdata, 0, 65);

                GetDataResultsResp3 response = new GetDataResultsResp3(rawdata);
                
                if (response.LastError != 0)
                {
                    string error=null;
                    try
                    {
                        error = _hardwareErrors[response.LastError];
                    }
                    catch (KeyNotFoundException)
                    {
                        error = "Unknown hardware error: " + response.LastError.ToString();
                    }
                    throw new Exception(error);
                }

                for (uint i = 0, j = currentAddress - region.Address; i < response.BytesPerPacket; i++, j++)
                {
                    region.Data[j] = response.Data[i];
                }

                progress((int)(region.Size - noTransferredBytes), (int)region.Size);

                noTransferredBytes -= response.BytesPerPacket;
                currentAddress += response.BytesPerPacket;
            }

            return 0;
        } 
        
        public override void ResetHardware()
        {
            ResetHardware(_hardwarePath);
        }
        
        public override string ToString()
        {
            return _devName;
        }

        #endregion

        #region Addition bootloader commands

        public class GetDataResultsResp3
        {
            public byte WindowsReserved;
            public byte Command;
            public uint Address;
            public byte BytesPerPacket;
            public byte ucCntrl;
            public byte LastError;
            public byte[] Data;
            public GetDataResultsResp3(byte[] resp)
            {
                Address = ConvertToUint(resp, 2);
                BytesPerPacket = resp[6];
                ucCntrl = resp[7];
                LastError = resp[8];
                Data = new byte[BytesPerPacket];
                for (int i = 0, j = resp.Length - BytesPerPacket; i < BytesPerPacket; i++, j++)
                    Data[i] = resp[j];
            }
        };

        public class QueryResultsResp3
        {
            public byte WindowsReserved;
            public byte Command;
            public byte BytesPerPacket;
            public List<MemoryRegionInfo> MemoryRegionsInfo = new List<MemoryRegionInfo>();

            public byte[] DS_Number;
            public byte WritesCounterHi;
            public byte WritesCounterLo;
            public byte BusSpeed;
            public byte DeviceLocking;
            public byte CrumVoltage;
            public byte NewCoreResetter;
            public byte[] FirmwareVersion;
            public bool infoIsTrue = false;

            QueryResultsResp3(byte[] response)
            {
                BytesPerPacket = response[2];
                int regTypePos = 4;
                while (response[regTypePos] != 0xFF)
                {
                    MemoryRegionInfo mr = new MemoryRegionInfo();
                    mr.Address = ConvertToUint(response, regTypePos + 1);
                    mr.Size = ConvertToUint(response, regTypePos + 5);
                    mr.Type = response[regTypePos];
                    MemoryRegionsInfo.Add(mr);
                    regTypePos += 9;
                }
                if (MemoryRegionsInfo.Count < 3)
                {
                    infoIsTrue = true;
                    DS_Number = new byte[8];
                    Array.Copy(response, 23, DS_Number, 0, 8);
                    WritesCounterHi = response[31];
                    WritesCounterLo = response[32];
                    BusSpeed = response[33];
                    DeviceLocking = response[34];
                    NewCoreResetter = response[36];
                    CrumVoltage = (byte)((int)response[35] - 1);
                    if ((CrumVoltage < 0x20) || (CrumVoltage > 0x2C)) CrumVoltage = 0x25;
                    FirmwareVersion = new byte[6];
                    Array.Copy(response, 37, FirmwareVersion, 0, 6);
                }

            }
            public static QueryResultsResp3 getQueryResultsResp(byte[] response)
            {
                return new QueryResultsResp3(response);
            }
        };

        public class MemSafeHandle : SafeHandle
        {
            public MemSafeHandle(IntPtr p)
                : base(IntPtr.Zero, true)
            {
                handle = p;
            }
            public override bool IsInvalid
            {
                get
                {
                    if (handle == IntPtr.Zero) return true;
                    else return false;
                }
            }
            protected override bool ReleaseHandle()
            {
                Marshal.FreeHGlobal(handle);
                return true;
            }
        }

        public class CrumSelect
        {
            public byte Command;
            public byte PassIndex;
            public byte NotUsed_1;
            public byte NotUsed_2;
            public byte I2CBusAddr;

            public static CrumSelect GetCrumSelect(byte passIndex, byte i2CBusAddr)
            {
                CrumSelect crs = new CrumSelect();
                crs.Command = SELECT_PASSWORD_SET;
                crs.PassIndex = passIndex;
                crs.I2CBusAddr = i2CBusAddr;
                return crs;

            }
        
        
        }




        #endregion    
    }
}
