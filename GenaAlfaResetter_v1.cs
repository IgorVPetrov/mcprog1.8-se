using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;

namespace mcprog
{
    public class GenaAlfaResetter_v1 : MicrochipBootloaderHardware, IAT88Programmer, IM24CXXProgrammer
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
        public const byte SELECTED_PASS_8 = 0x08;
        public const byte SELECTED_24C04 = 0x09;
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
                        SetTaskNumber(harwareStream, _at88PasswordSet[crumType]);

                        UpdateCurrentMemRegConfig(harwareStream);

                        if ( (_currentMemoryRegionsConfig.Count != 2) )
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }
                        regions = new List<MemoryRegion>() { new MemoryRegion(_currentMemoryRegionsConfig[0]), new MemoryRegion(_currentMemoryRegionsConfig[1]) };
                        
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read user data")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ReadRegion(harwareStream, regions[0], progressAct);
                        
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Read config data")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        ReadRegion(harwareStream, regions[1], progressAct);
                    }

                }
                catch (Exception e)
                {
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
                        SetTaskNumber(harwareStream, _at88PasswordSet[crumType]);

                        UpdateCurrentMemRegConfig(harwareStream);

                        if ( (_currentMemoryRegionsConfig.Count != 2) )
                        {
                            Exception e1 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(4319) | 0x80070000)));
                            Exception e2 = Marshal.GetExceptionForHR((int)((Convert.ToUInt32(1784) | 0x80070000)));
                            string message = e1.Message.Substring(0, e1.Message.IndexOf("(")) +
                                e2.Message.Substring(0, e2.Message.IndexOf("("));
                            throw new Exception(message);
                        }

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Programming")); }, null);

                        ProgramRegion(harwareStream, regions[0], progressAct);

                        MemoryRegion testRegion = new MemoryRegion(_currentMemoryRegionsConfig[0]);

                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo("Verification")); }, null);
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                        ReadRegion(harwareStream, testRegion, progressAct);

                        for (int i = 0; i < regions[0].Size; i++)
                            if (regions[0].Data[i] != testRegion.Data[i])
                            {
                                throw new VerificationException(regions[0].Address + (uint)i, regions[0].Data[i], testRegion.Data[i]);
                            }
                    }

                }
                catch (Exception e)
                {
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
                    SafeFileHandle handle = CreateFile(_hardwarePath, Win32HardwareIOSupport.GENERIC_WRITE | Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);

                    if (handle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    using (FileStream harwareStream = new FileStream(handle, FileAccess.ReadWrite, 65))
                    {
                        UpdateCurrentMemRegConfig(harwareStream);
                        if(_currentMemoryRegionsConfig.Count == 4)
                            _hardwareMode = HardwareMode.ServiceMode;
                        else
                            _hardwareMode = HardwareMode.ProgrammerMode;
                    }
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

        int IM24CXXProgrammer.ReadChip(string chip, MemoryRegionInfo regionInfo, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {

            _isBusy = true;
            
            regionInfo.Address = 0;
            regionInfo.Size = 512;

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0,val,max))); }, null);
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
                    }

                }
                catch(Exception e)
                {
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);
                
                if (chip == "24C02")
                {
                    byte[] datablock = new byte[256];
                    region.ReadData(0, datablock);
                    region = new MemoryRegion(0, 256, 1);
                    region.WriteData(0, datablock);
                } 
                
                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int IM24CXXProgrammer.ProgramChip(string chip, MemoryRegion region, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());
            
            if (chip == "24C02")
            {
                byte[] regdata = new byte[256];
                region.ReadData(0, regdata);
                region = new MemoryRegion(0, 512, 1);
                region.WriteData(0, regdata);
            }


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

                        for(int i=0;i<region.Size;i++)
                            if (region.Data[i] != testRegion.Data[i])
                            {
                                throw new VerificationException(region.Address + (uint)i, region.Data[i], testRegion.Data[i]);
                            }
                    }

                }
                catch(VerificationException ve)
                {
                    if ((chip == "24C02") && (ve.ErrorAddress >= 256))
                    {

                    }
                    else
                    {
                        pcInfo.error = ve;
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

        #region Ctors
        
        public GenaAlfaResetter_v1(string path)
        {
            _hardwarePath = path;
        }
        
        #endregion

        #region OnEvent methods

        protected virtual void OnBusy()
        {
            EventHandler localHandler = _busy;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }
        }
        
        protected virtual void OnReady()
        {
            EventHandler localHandler = _ready;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }
        }

        #endregion

        #region Overriden 
        
        public override void ResetHardware()
        {
            ResetHardware(_hardwarePath);
        }
        
        public override string ToString()
        {
            return _devName;
        }
        #endregion
        
        #region Fields
        
        string _devName = "GenaAlfa Resetter v1";
        bool _isBusy = false;
        string _hardwarePath;
        EventHandler _busy;
        EventHandler _ready;
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
        };
        IDictionary<string, byte> _m24CXXChipSet = new Dictionary<string, byte> 
        {
            {"24C04", SELECTED_24C04},
            {"24C02", SELECTED_24C04}
        };

        HardwareMode _hardwareMode = HardwareMode.UnknownMode;
        

        #endregion

    }
}
