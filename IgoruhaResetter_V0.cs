using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mcprog
{
    public class IgoruhaResetter_V0 : MicrochipBootloaderHardware, IOneWireProgrammer, IM24CXXProgrammer, IRFIDProgrammer, IPresetting
    {

        #region Constant
        
        public const byte ONEWIRE_CHIP = 0x00;


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
                        if (_currentMemoryRegionsConfig.Count == 4)
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

        #region IOneWireProgrammer Members

        ICollection<string> IOneWireProgrammer.SupportedChips
        {
            get { return _OneWireChipSet.Keys; }
        }

        int IOneWireProgrammer.ExecuteCommandSet(byte[] request, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
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
                MemoryRegion region = new MemoryRegion(0, 256, 0);
                
                
                try
                {

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



                    if (!WriteFile(WriteHandle, request, 65, out wrBytes, IntPtr.Zero))
                    {

                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                    }

                    byte[] respdata = new byte[65];


                    if (!ReadFile(ReadHandle, respdata, 65, out wrBytes, IntPtr.Zero))
                    {

                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                    }
                    

                    region.WriteData(0,respdata);


                }
                catch (Exception e)
                {
                    
                    pcInfo.error = e;
                }
                pcInfo.Message = "Complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int IOneWireProgrammer.ExecuteCommandSet(List<byte[]> request, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
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
                MemoryRegion region = new MemoryRegion(0, (uint)(65*request.Count), 0);


                try
                {

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

                    List<byte> respdata = new List<byte>();
                    
                    int curreq=0;
                    
                    foreach (byte[] data in request)
                    {

                        if (!WriteFile(WriteHandle, data, 65, out wrBytes, IntPtr.Zero))
                        {

                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                        }

                        byte[] resp = new byte[65];


                        if (!ReadFile(ReadHandle, resp, 65, out wrBytes, IntPtr.Zero))
                        {

                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                        }

                        if (resp[1] != 0) throw new Exception("ONEWIRE ERROR :" + resp[1].ToString("X2"));


                        progressAct(100 * ++curreq / request.Count, 100);
                        
                        respdata.AddRange(resp);

                    }
                    region.WriteData(0, respdata.ToArray());


                }
                catch (Exception e)
                {

                    pcInfo.error = e;
                }
                pcInfo.Message = "Complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }



        
        #endregion

        #region IRFIDProgrammer Members

        int IRFIDProgrammer.Read(byte[] request, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
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
                MemoryRegion region = new MemoryRegion(0, 256, 0);


                try
                {

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

                    //byte[] request = new byte[65];

                    byte[] response = new byte[65];



                    if (!WriteFile(WriteHandle, request, 65, out wrBytes, IntPtr.Zero))
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                    if (!ReadFile(ReadHandle, response, 65, out wrBytes, IntPtr.Zero))
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    for (uint i = 0; i < 256; i += 64)
                    {

                        request[1] = Constants.RFID125_GET_DATA;
                        request[2] = (byte)i;
                        request[3] = 64;
                                          
                        
                        if (!WriteFile(WriteHandle, request, 65, out wrBytes, IntPtr.Zero))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }
                        if (!ReadFile(ReadHandle, response, 65, out wrBytes, IntPtr.Zero))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }

                        List<byte> respList = new List<byte>(response);
                        respList.RemoveAt(0);

                        region.WriteData(i, respList.ToArray());
                    }





                    


                }
                catch (Exception e)
                {

                    pcInfo.error = e;
                }
                pcInfo.Message = "Complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int IRFIDProgrammer.Write(List<byte[]> request, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
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
                MemoryRegion region = new MemoryRegion(0, 256, 0);


                try
                {

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
                    
                    byte[] respdata;

                    int step = 1;

                    foreach (byte[] req in request)
                    {

                        if (!WriteFile(WriteHandle, req, 65, out wrBytes, IntPtr.Zero))
                        {

                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                        }

                        respdata = new byte[65];


                        if (!ReadFile(ReadHandle, respdata, 65, out wrBytes, IntPtr.Zero))
                        {

                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                        }

                        if(respdata[1]!=0)throw new Exception("Write error: "+respdata[1].ToString("X2"));

                        progressAct(step++, request.Count);
                    }

                }
                catch (Exception e)
                {

                    pcInfo.error = e;
                }
                pcInfo.Message = "Complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int IRFIDProgrammer.Adjust(Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
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
                MemoryRegion region = new MemoryRegion(0, 256, 0);


                try
                {

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

                    byte[] request = new byte[65];

                    byte[] response = new byte[65];

                    byte maxTime = 0;
                    byte minTime = 0xFF;

                    for (int i = 0; i < 20; i++)
                    {

                        request[1] = Constants.RFID125_TEST;


                        if (!WriteFile(WriteHandle, request, 65, out wrBytes, IntPtr.Zero))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }

                        if (!ReadFile(ReadHandle, response, 65, out wrBytes, IntPtr.Zero))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }

                        if (response[3] > maxTime) maxTime = response[3];
                        if (response[3] < minTime) minTime = response[3];

                        progressAct(i, 20);
                    }

                    response[3] = maxTime;
                    response[4] = minTime;

                    region.WriteData(0, response);



                }
                catch (Exception e)
                {

                    pcInfo.error = e;
                }
                pcInfo.Message = "Complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
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
            

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };



            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                //MemoryRegion region = null;
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                I2CReadPacketsFactory pf = new I2CReadPacketsFactory(regionInfo);

                try
                {
                    ExchangeWithDevice(pf, progressAct);
                
                }
                catch (Exception e)
                {
                    
                    pcInfo.error = e;
                }
                pcInfo.Message = "Read complete";
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, 0, 1))); }, null);

                asyncOp.Post(delegate(object args) { completed(pcInfo, pf.GetRegion()); }, null);
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


            Action<int, int> progressAct = delegate(int val, int max)
            {
                asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(new ProgressBarData(0, val, max))); }, null);
            };

            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                I2CWritePacketsFactory pf = new I2CWritePacketsFactory(region);

                try
                {

                    ExchangeWithDevice(pf, progressAct);

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


                        UpdateCurrentMemRegConfig(harwareStream);

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

        public IgoruhaResetter_V0(string path)
        {
            _hardwarePath = path;
            infoForm.FormClosing += infoForm_FormClosing;
            infoForm._resetButton.Click += resetButton_Click;
        }
        
        #endregion

        #region Fields

        string _devName = "Igoruha Resetter V0";
        bool _isBusy = false;
        string _hardwarePath;
        EventHandler _busy;
        EventHandler _ready;
        HardwareMode _hardwareMode = HardwareMode.UnknownMode;

        IgoruhaResetterInfoForm infoForm = new IgoruhaResetterInfoForm();



        IDictionary<string, byte> _OneWireChipSet = new Dictionary<string, byte> 
        {
            {"ONEWIRE", ONEWIRE_CHIP},
            
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
        

        #endregion

        #region Methods

        private void infoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetHardware(_hardwarePath);

        }

        public int ExchangeWithDevice(IUSBPacketsFactory pf, Action<int, int> progress)
        {
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




            for (uint packnum = 0; packnum < pf.NumberOfPackets; packnum++)
            {


                byte[] request = pf.GetTxPacket(packnum);

                if (!WriteFile(WriteHandle, request, 65, out wrBytes, IntPtr.Zero))
                {

                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }

                byte[] respdata = new byte[65];


                if (!ReadFile(ReadHandle, respdata, 65, out wrBytes, IntPtr.Zero))
                {

                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
                pf.AddRxPacket(respdata);
                progress((int)packnum, (int)(pf.NumberOfPackets));

            }
            


            return 0;
        }

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






        }

        private void MemToArray(MemSafeHandle p, byte[] array)
        {
            for (int i = 1; i < 65; i++)
            {
                array[i - 1] = Marshal.ReadByte(p.DangerousGetHandle(), i);
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


            int currentPacketLength = 0;
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

        #endregion

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
   
    }
}
