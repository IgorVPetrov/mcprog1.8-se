using System;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Permissions;
using System.Security;
using System.Runtime.ConstrainedExecution;


namespace mcprog
{
	/// <summary>
	/// Class that wraps USB API calls and structures.
	/// </summary>
    public class Win32HardwareIOSupport
    {
        #region Structures
        /// <summary>
        /// An overlapped structure used for overlapped IO operations. The structure is
        /// only used by the OS to keep state on pending operations. You don't need to fill anything in if you
        /// unless you want a Windows event to fire when the operation is complete.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct Overlapped
        {
            public uint Internal;
            public uint InternalHigh;
            public uint Offset;
            public uint OffsetHigh;
            public IntPtr Event;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            Guid ClassGuid;
            public uint DevInst;
            IntPtr Reserved;
        }
        
		/// <summary>
		/// Provides details about a single USB device
		/// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct SP_DEVICE_INTERFACE_DATA
        {
            public uint Size;
            public Guid InterfaceClassGuid;
            public uint Flags;
            IntPtr Reserved;
        }
		
		/// <summary>
		/// Access to the path for a device
		/// </summary>
        [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode, Pack = 1)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
		/// <summary>
		/// Used when registering a window to receive messages about devices added or removed from the system.
		/// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class DeviceBroadcastInterface
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public Guid ClassGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }		
        #endregion

        #region Constants
        public const uint SPDRP_HARDWAREID = 0x1;

        public const uint SPDRP_ADDRESS = 0x0000001C;

        public const uint SPDRP_LOCATION_INFORMATION = 0x0000000D;

        public const uint SPDRP_LOCATION_PATHS = 0x00000023;

        public const uint SPDRP_FRIENDLYNAME = 0x0000000C;

        public const uint SPDRP_UI_NUMBER = 0x00000010;

        public const uint SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D;

        public const uint SPDRP_BUSTYPEGUID = 0x00000013;

        public const uint SPDRP_BUSNUMBER = 0x00000015;

        public const uint SPDRP_DEVICEDESC = 0x00000000;

        public const uint SPDRP_DRIVER = 0x00000009;

        public const uint SPDRP_EXCLUSIVE = 0x0000001A;

        public const uint SPDRP_ENUMERATOR_NAME = 0x00000016;

        public const uint ERROR_INSUFFICIENT_BUFFER = 122;

        public const uint SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E;
		/// <summary>Windows message sent when a device is inserted or removed</summary>
		public const int WM_DEVICECHANGE = 0x0219;
		/// <summary>WParam for above : A device was inserted</summary>
        public const int DBT_DEVICEARRIVAL = 0x8000;

        public const int DBT_DEVICEREMOVEPENDING = 0x8003;

        public const int DBT_CONFIGCHANGED = 0x0018;

		/// <summary>WParam for above : A device was removed</summary>
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
		/// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>
        public const int DIGCF_PRESENT = 0x02;
		/// <summary>Used in SetupDiClassDevs to get device interface details</summary>
        public const int DIGCF_DEVICEINTERFACE = 0x10;
		/// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        public const int DEVTYP_DEVICEINTERFACE = 0x05;
		/// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        /// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>
        public const uint PURGE_TXABORT = 0x01;
        /// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>
        public const uint PURGE_RXABORT = 0x02;
        /// <summary>Purges Win32 transmit buffer by clearing it.</summary>
        public const uint PURGE_TXCLEAR = 0x04;
        /// <summary>Purges Win32 receive buffer by clearing it.</summary>
        public const uint PURGE_RXCLEAR = 0x08;
        /// <summary>CreateFile : Open file for read</summary>
        public const uint GENERIC_READ = 0x80000000;
        /// <summary>CreateFile : Open file for write</summary>
        public const uint GENERIC_WRITE = 0x40000000;
        /// <summary>CreateFile : file share for write</summary>
        public const uint FILE_SHARE_WRITE = 0x2;
        /// <summary>CreateFile : file share for read</summary>
        public const uint FILE_SHARE_READ = 0x1;
        /// <summary>CreateFile : Open handle for overlapped operations</summary>
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        /// <summary>CreateFile : Resource to be "created" must exist</summary>
        public const uint OPEN_EXISTING = 3;
        /// <summary>CreateFile : Resource will be "created" or existing will be used</summary>
        public const uint OPEN_ALWAYS = 4;
        /// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>
        public const int ERROR_IO_PENDING = 997;
        /// <summary>Infinite timeout</summary>
        public const uint INFINITE = 0xFFFFFFFF;
        /// <summary>Simple representation of a null handle : a closed stream will get this handle. Note it is public for comparison by higher level classes.</summary>
        public static IntPtr NullHandle = IntPtr.Zero;
        /// <summary>Simple representation of the handle returned when CreateFile fails.</summary>
        protected static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        #endregion

        #region P/Invoke
		
		/// <summary>
		/// Allocates an InfoSet memory block within Windows that contains details of devices.
		/// </summary>
		/// <param name="gClass">Class guid (e.g. HID guid)</param>
		/// <param name="strEnumerator">Not used</param>
		/// <param name="hParent">Not used</param>
		/// <param name="nFlags">Type of device details required (DIGCF_ constants)</param>
		/// <returns>A reference to the InfoSet</returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet=CharSet.Unicode)]
        protected static extern SafeDevInfoTableHandle SetupDiGetClassDevs
            (ref Guid gClass, 
            [MarshalAs(UnmanagedType.LPWStr)] string strEnumerator,
            IntPtr hParent, 
            uint nFlags
            );


        [DllImport("setupapi.dll", SetLastError = true, CharSet=CharSet.Unicode)]
        protected static extern bool SetupDiGetDeviceRegistryProperty
            (
            SafeDevInfoTableHandle DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            uint Property,
            IntPtr PropertyRegDataType,
            IntPtr PropertyBuffer,
            uint PropertyBufferSize,
            ref uint RequiredSize
            );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern bool SetupDiGetDeviceRegistryProperty
            (
            SafeDevInfoTableHandle DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            uint Property,
            IntPtr PropertyRegDataType,
            [MarshalAs(UnmanagedType.LPArray)] char[] PropertyBuffer,
            uint PropertyBufferSize,
            IntPtr RequiredSize
            );


        [DllImport("setupapi.dll", SetLastError = true, CharSet=CharSet.Unicode)]
        protected static extern bool SetupDiEnumDeviceInfo
            (
            SafeDevInfoTableHandle DeviceInfoSet,
            uint MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData
            );

        
        
        /// <summary>
		/// Frees InfoSet allocated in call to above.
		/// </summary>
		/// <param name="lpInfoSet">Reference to InfoSet</param>
		/// <returns>true if successful</returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern bool SetupDiDestroyDeviceInfoList
            (IntPtr lpInfoSet
            );
		/// <summary>
		/// Gets the DeviceInterfaceData for a device from an InfoSet.
		/// </summary>
		/// <param name="lpDeviceInfoSet">InfoSet to access</param>
		/// <param name="nDeviceInfoData">Not used</param>
		/// <param name="gClass">Device class guid</param>
		/// <param name="nIndex">Index into InfoSet for device</param>
		/// <param name="oInterfaceData">DeviceInterfaceData to fill with data</param>
		/// <returns>True if successful, false if not (e.g. when index is passed end of InfoSet)</returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern bool SetupDiEnumDeviceInterfaces
            (SafeDevInfoTableHandle lpDeviceInfoSet,
            IntPtr DeviceInfoData, 
            ref Guid gClass, 
            uint nIndex,
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData
            );
        /// <summary>
        /// SetupDiGetDeviceInterfaceDetail
        /// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.
        /// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)
        /// and once again when you've allocated the required space.
        /// </summary>
        /// <param name="lpDeviceInfoSet">InfoSet to access</param>
        /// <param name="oInterfaceData">DeviceInterfaceData to use</param>
        /// <param name="oDetailData">DeviceInterfaceDetailData to fill with data</param>
        /// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>
        /// <param name="nRequiredSize">The required size of the above when above is set as zero</param>
        /// <param name="lpDeviceInfoData">Not used</param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern bool SetupDiGetDeviceInterfaceDetail
            (SafeDevInfoTableHandle lpDeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA oDetailData, 
            uint nDeviceInterfaceDetailDataSize,
            IntPtr nRequiredSize, 
            IntPtr lpDeviceInfoData
            );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern bool SetupDiGetDeviceInterfaceDetail
            (SafeDevInfoTableHandle lpDeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData,
            IntPtr oDetailData,
            uint nDeviceInterfaceDetailDataSize,
            ref uint nRequiredSize,
            IntPtr lpDeviceInfoData
            );
		/// <summary>
		/// Registers a window for device insert/remove messages
		/// </summary>
		/// <param name="hwnd">Handle to the window that will receive the messages</param>
        /// <param name="oInterface">DeviceBroadcastInterrface structure</param>
		/// <param name="nFlags">set to DEVICE_NOTIFY_WINDOW_HANDLE</param>
		/// <returns>A handle used when unregistering</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern IntPtr RegisterDeviceNotification
            (IntPtr hwnd, 
            DeviceBroadcastInterface oInterface, 
            uint nFlags
            );
		/// <summary>
		/// Unregister from above.
		/// </summary>
		/// <param name="hHandle">Handle returned in call to RegisterDeviceNotification</param>
		/// <returns>True if success</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern bool UnregisterDeviceNotification
            (IntPtr hHandle
            );
		
		/// <summary>
		/// Creates/opens a file, serial port, USB device... etc
		/// </summary>
		/// <param name="strName">Path to object to open</param>
		/// <param name="nAccess">Access mode. e.g. Read, write</param>
		/// <param name="nShareMode">Sharing mode</param>
		/// <param name="lpSecurity">Security details (can be null)</param>
		/// <param name="nCreationFlags">Specifies if the file is created or opened</param>
		/// <param name="nAttributes">Any extra attributes? e.g. open overlapped</param>
		/// <param name="lpTemplate">Not used</param>
		/// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern SafeFileHandle CreateFile
            ([MarshalAs(UnmanagedType.LPWStr)] string strName, 
            uint nAccess, 
            uint nShareMode, 
            IntPtr lpSecurity, 
            uint nCreationFlags, 
            uint nAttributes, 
            IntPtr lpTemplate
            );
		/// <summary>
		/// Closes a window handle. File handles, event handles, mutex handles... etc
		/// </summary>
		/// <param name="hFile">Handle to close</param>
		/// <returns>True if successful.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)] 
        protected static extern int CloseHandle
            (IntPtr hFile
            );
        #endregion

        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected class SafeDevInfoTableHandle : SafeHandleMinusOneIsInvalid
        {
            private SafeDevInfoTableHandle()
                : base(true)
            {
            }
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            override protected bool ReleaseHandle()
            {
                return SetupDiDestroyDeviceInfoList(handle);
            }


        }
		
    }
    
    
}
