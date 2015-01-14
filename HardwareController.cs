using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace mcprog
{
    
    
    
    
    
    
    public class HardwareController : Win32HardwareIOSupport
    {

        #region Fields
        
        
        Guid guid = new Guid(0x4d1e55b2, 0xf16f, 0x11cf, 0x88, 0xcb, 0x00, 0x11, 0x11, 0x00, 0x00, 0x30);        
        SP_DEVINFO_DATA SpDevinfoData = new SP_DEVINFO_DATA();
        SP_DEVICE_INTERFACE_DATA SpDeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
        SP_DEVICE_INTERFACE_DETAIL_DATA SpDeviceInterfaceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
        IDictionary<string, IChipProgrammer> _programmers=new Dictionary<string, IChipProgrammer>();



        List<IChipProgrammer> _progsInProgrammerMode = new List<IChipProgrammer>();
        List<IChipProgrammer> _progsInServiceMode = new List<IChipProgrammer>();
        
        #endregion

        #region Programmer Fabrics

        delegate IChipProgrammer ProgrammerFabric(string path);
        IDictionary<string, ProgrammerFabric> _programmerFabrics = new Dictionary<string, ProgrammerFabric>()
        {
            {"VID_04D8&PID_003C&REV_0002",
                delegate(string path)
                {
                    return new GenaAlfaResetter_v1(path);
                }
            },
            {"VID_04D8&PID_003C&REV_0003",
                delegate(string path)
                {
                    byte[] wrdata = new byte[65];
                    wrdata[0] = 0;
                    wrdata[1] = MicrochipBootloaderHardware.QUERY_DEVICE;
                    SafeFileHandle WriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
                    if (WriteHandle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                    SafeFileHandle ReadHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_READ,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
                    if (ReadHandle.IsInvalid)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                    uint wrBytes;
                    if (!MicrochipBootloaderHardware.WriteFile(WriteHandle, wrdata, 65, out wrBytes, IntPtr.Zero))
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                    byte[] rddata = new byte[65];
                    if (!MicrochipBootloaderHardware.ReadFile(ReadHandle, rddata, 65, out wrBytes, IntPtr.Zero))
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                    IChipProgrammer result = null;
                    if( rddata[33] == 0 )
                    {
                        result = new GenaAlfaResetter_rev3free(path);
                    }
                    else
                    {
                        result = new GenaAlfaResetter_v2(path);
                    }
                    return result;
                }
            },
            {"VID_04D8&PID_003C&REV_0004",
                delegate(string path)
                {
                    return new GenaAlfaResetter_v3(path);
                }
            },
            {"VID_04D8&PID_003C&REV_0005",
                delegate(string path)
                {
                    return new GenaAlfaResetter_rev5(path);
                }
            },        
        
            {"VID_04D8&PID_003C&REV_7770",
                delegate(string path)
                {
                    return new IgoruhaResetter_V0(path);
                }
            },          
        
        
        
        
        };
        
        
        
        #endregion
        
        #region HID Notification Registration

        IntPtr _regHandle;

        public bool EnableHidNotification(IntPtr hWnd)
        {
            DeviceBroadcastInterface oInterfaceIn = new DeviceBroadcastInterface();
            oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);
            oInterfaceIn.ClassGuid = guid;
            oInterfaceIn.DeviceType = DEVTYP_DEVICEINTERFACE;
            oInterfaceIn.Reserved = 0;
            _regHandle = RegisterDeviceNotification(hWnd, oInterfaceIn, DEVICE_NOTIFY_WINDOW_HANDLE);
            if (_regHandle == IntPtr.Zero) return false;
            return true;
        }
        public bool DisableHidNotification()
        {
            return UnregisterDeviceNotification(_regHandle);
        }
		 
	    #endregion


     
        public void UpdateListOfAttachedDevices()
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            ThreadStart start =
                delegate()
                {

                    Monitor.Enter(_programmers);
                    
                    try
                    {

                        #region Search of devices

                        SafeDevInfoTableHandle DeviceInfoTable = SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
                        if (DeviceInfoTable.IsInvalid)
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }

                        SpDevinfoData.cbSize = (UInt32)Marshal.SizeOf(SpDevinfoData);
                        SpDeviceInterfaceData.Size = (UInt32)Marshal.SizeOf(SpDeviceInterfaceData);
                        SpDeviceInterfaceDetailData.Size = 6;

                        IDictionary<string, string> actualProgs = new Dictionary<string, string>();

                        for (UInt32 interface_index = 0;
                        SetupDiEnumDeviceInterfaces(DeviceInfoTable, new IntPtr(0),
                            ref guid, interface_index, ref SpDeviceInterfaceData); interface_index++)
                        {
                            if (!SetupDiEnumDeviceInfo(DeviceInfoTable, interface_index, ref SpDevinfoData)) continue;

                            uint recSize = 0;
                            bool res = SetupDiGetDeviceRegistryProperty(DeviceInfoTable, ref SpDevinfoData,
                                SPDRP_HARDWAREID, new IntPtr(0), new IntPtr(0), 0, ref recSize);
                            if (!res)
                            {
                                if((uint)Marshal.GetLastWin32Error()!= ERROR_INSUFFICIENT_BUFFER)
                                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                            }
                            
                            char[] PropertyBuffer = new char[recSize / 2];

                            res = SetupDiGetDeviceRegistryProperty(DeviceInfoTable, ref SpDevinfoData,
                                SPDRP_HARDWAREID, new IntPtr(0), PropertyBuffer, recSize, new IntPtr(0));
                            if (!res)
                            {     
                                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                            }

                            string deviceName = (new string(PropertyBuffer)).ToUpperInvariant();
                            

                            foreach (string str in _programmerFabrics.Keys)
                            {
                                if (deviceName.Contains(str))
                                {
                                    deviceName = string.Copy(str);
                                    break;
                                }
                            }

                            //deviceName = deviceName.Substring(0, deviceName.IndexOf("\0"));

                            if (_programmerFabrics.ContainsKey(deviceName))
                            {
                                uint structSize = 0;

                                res = SetupDiGetDeviceInterfaceDetail(DeviceInfoTable, ref SpDeviceInterfaceData,
                                    new IntPtr(0), 0, ref structSize, new IntPtr(0));
                                if (!res)
                                {
                                    if ((uint)Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
                                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                                }
                                
                                
                                res = SetupDiGetDeviceInterfaceDetail(DeviceInfoTable, ref SpDeviceInterfaceData,
                                    ref SpDeviceInterfaceDetailData, structSize,
                                    new IntPtr(0), new IntPtr(0));
                                if (!res)
                                {
                                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                                }


                                actualProgs.Add(SpDeviceInterfaceDetailData.DevicePath, deviceName);

                            }

                        }

                        #endregion


                        List<string> remProgs = new List<string>();
                        //int lastNumberOfProgs = _programmers.Count;
                        //List<IChipProgrammer> progsInSMode = new List<IChipProgrammer>();
                        //List<IChipProgrammer> removedProgs = new List<IChipProgrammer>();


                        //List<IChipProgrammer> _progsInResetterMode = new List<IChipProgrammer>();
                        //List<IChipProgrammer> _progsInServiceMode = new List<IChipProgrammer>();
                        List<IChipProgrammer> _removedProgs = new List<IChipProgrammer>();
                        int progsInProgrammerModeCount = _progsInProgrammerMode.Count;
                        int progsInServiceModeCount = _progsInServiceMode.Count;
                        
                        foreach (KeyValuePair<string, IChipProgrammer> kvp in _programmers)
                            if (actualProgs.ContainsKey(kvp.Key)) actualProgs.Remove(kvp.Key);
                            else remProgs.Add(kvp.Key);


                        foreach (string remProg in remProgs)
                        {
                            _progsInProgrammerMode.Remove(_programmers[remProg]);
                            _progsInServiceMode.Remove(_programmers[remProg]);
                            _removedProgs.Add(_programmers[remProg]);
                            _programmers.Remove(remProg);
                        }
                        foreach (KeyValuePair<string, string> kvp in actualProgs)
                        {
                            #region Inquiry again attached devices

                            IChipProgrammer prog = _programmerFabrics[kvp.Value](kvp.Key);
                            HardwareMode hMode = HardwareMode.UnknownMode;
                            Thread th = new Thread(
                                delegate()
                                {
                                    hMode = prog.GetMode();
                                }
                            );
                            th.Start();
                            //th.Join();
                            if (th.Join(2000))
                            {
                                switch (hMode)
                                {
                                    case HardwareMode.ProgrammerMode :
                                        _programmers.Add(kvp.Key, prog);
                                        _progsInProgrammerMode.Add(prog);
                                        break;

                                    case HardwareMode.ServiceMode :
                                        _programmers.Add(kvp.Key, prog);
                                        _progsInServiceMode.Add(prog);
                                        break;

                                    case HardwareMode.UnknownMode :
                                        asyncOp.Post(
                                            delegate(object args)
                                            {

                                                HardwareControllerEventArgs hce = new HardwareControllerEventArgs();
                                                hce.Error = new Exception("Attached hardware in unknown mode");

                                                OnHardwareDetectionError(hce);
                                            },
                                            null
                                        );
                                        break;
                                }                             
                            }
                            else
                            {
                                th.Abort();
                                asyncOp.Post(
                                    delegate(object args)
                                    {
                                        HardwareControllerEventArgs hce = new HardwareControllerEventArgs();
                                        hce.Error = new Exception("Attached hardware not answer");

                                        OnHardwareDetectionError(hce);
                                    },
                                    null
                                );
                            }

                            #endregion
                        }

                        if (_removedProgs.Count != 0)
                        {
                            asyncOp.Post(
                                delegate(object args)
                                {
                                    HardwareControllerEventArgs hce = new HardwareControllerEventArgs();

                                    hce.ListOfDevices = new List<IChipProgrammer>(_removedProgs.ToArray());
                                    OnDeviceRemoved(hce);
                                },
                                null
                            );
                        }
                        if (_progsInServiceMode.Count > progsInServiceModeCount)
                        {
                            asyncOp.Post(
                                delegate(object args)
                                {
                                    HardwareControllerEventArgs hce = new HardwareControllerEventArgs();
                                    hce.ListOfDevices = new List<IChipProgrammer>(_progsInServiceMode.ToArray());
                                    OnDeviceInServiceModeDetected(hce);
                                },
                                null
                            );
                        }
                        if (_progsInProgrammerMode.Count > progsInProgrammerModeCount)
                        {
                            asyncOp.Post(
                                delegate(object args)
                                {
                                    HardwareControllerEventArgs hce = new HardwareControllerEventArgs();
                                    hce.ListOfDevices = new List<IChipProgrammer>(_progsInProgrammerMode.ToArray());
                                    OnDeviceInProgrammerModeDetected(hce);
                                },
                                null
                            );
                        }
                        


                    }
                    catch (Exception e) 
                    {
                        asyncOp.PostOperationCompleted(
                                delegate(object args)
                                {
                                    HardwareControllerEventArgs hce = new HardwareControllerEventArgs();
                                    hce.Error = e;       
                                    OnHardwareDetectionError(hce);
                                },
                                null
                            );
                    }

                    Monitor.PulseAll(_programmers);
                    Monitor.Exit(_programmers);
                };

            (new Thread(start)).Start();
           
            
        }

        protected virtual void OnDeviceInProgrammerModeDetected(HardwareControllerEventArgs e)
        {
            HardwareControllerEventHandler localHandler = DeviceInProgrammerModeDetected;
            if (localHandler != null)
            {
                localHandler(this, e);
            }  
        }
        
        protected virtual void OnDeviceRemoved(HardwareControllerEventArgs e)
        {
            HardwareControllerEventHandler localHandler = DeviceRemoved;
            if (localHandler != null)
            {
                localHandler(this, e);
            } 
            
        }

        protected virtual void OnDeviceInServiceModeDetected(HardwareControllerEventArgs e)
        {
            HardwareControllerEventHandler localHandler = DeviceInServiceModeDetected;
            if (localHandler != null)
            {
                localHandler(this, e);
            }
            
        }

        protected virtual void OnHardwareDetectionError(HardwareControllerEventArgs e)
        {
            HardwareControllerEventHandler localHandler = HardwareDetectionError;
            if (localHandler != null)
            {
                localHandler(this, e);
            }
            

        }


        public event HardwareControllerEventHandler DeviceInProgrammerModeDetected;

        public event HardwareControllerEventHandler DeviceRemoved;

        public event HardwareControllerEventHandler DeviceInServiceModeDetected;

        public event HardwareControllerEventHandler HardwareDetectionError;

        public void GetListOfDevicesInProgrammerMode(Action<List<IChipProgrammer>> complete)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());
            ThreadStart start = 
                delegate()
                {
                    Monitor.Enter(_programmers);
                    asyncOp.PostOperationCompleted(
                        delegate(object args)
                        {
                            complete(new List<IChipProgrammer>(_progsInProgrammerMode.ToArray()));
                        },
                        null
                    );
                    Monitor.PulseAll(_programmers);
                    Monitor.Exit(_programmers);
                };
            (new Thread(start)).Start();
        }
        
        public void GetListOfDevicesInServiceMode(Action<List<IChipProgrammer>> complete)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());
            ThreadStart start =
                delegate()
                {
                    Monitor.Enter(_programmers);
                    asyncOp.PostOperationCompleted(
                        delegate(object args)
                        {
                            complete(new List<IChipProgrammer>(_progsInServiceMode.ToArray()));
                        },
                        null
                    );
                    Monitor.PulseAll(_programmers);
                    Monitor.Exit(_programmers);
                };
            (new Thread(start)).Start();
        }     
    
    }

       
    public enum HardwareMode
    {
        ServiceMode,
        ProgrammerMode,
        UnknownMode
    }

    public class HardwareControllerEventArgs
    {
        private List<IChipProgrammer> _listOfDevices;

        private Exception _error;

        public Exception Error
        {
            get { return _error; }
            set { _error= value; }
        }

        public HardwareControllerEventArgs()
        {
            _error = null;
            _listOfDevices = null;
        }
        public List<IChipProgrammer> ListOfDevices
        {
            get { return _listOfDevices; }
            set { _listOfDevices = value; }
        }
    }

    public delegate void HardwareControllerEventHandler(object sender, HardwareControllerEventArgs e);

    
    
}
