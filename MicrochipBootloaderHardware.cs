using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;


namespace mcprog
{


    public class MicrochipBootloaderHardware
    {
        #region Bootloader Commands
        public class QueryDeviceReq
        {
            public static byte[] getCommandPacket()
            {
                byte[] result = new byte[65];
                result[1] = QUERY_DEVICE;
                return result;
            }
        };

        public class EraseDeviceReq
        {
            public static byte[] getCommandPacket()
            {
                byte[] result = new byte[65];
                result[1] = ERASE_DEVICE;
                return result;
            }
        };

        public class ProgramCompleteReq
        {
            public static byte[] getCommandPacket()
            {
                byte[] result = new byte[65];
                result[0] = 0;
                result[1] = PROGRAM_COMPLETE;
                return result;
            }
        };

        public class ResetDeviceReq
        {
            public static byte[] getCommandPacket()
            {
                byte[] result = new byte[65];
                result[1] = RESET_DEVICE;
                return result;
            }
        };

        public class QueryResultsResp
        {
            public byte WindowsReserved;
            public byte Command;
            public byte BytesPerPacket;
            public byte DeviceFamily;
            public List<MemoryRegionInfo> MemoryRegionsInfo = new List<MemoryRegionInfo>();
            public byte[] Pad;
            QueryResultsResp(byte[] response)
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

            }
            public static QueryResultsResp getQueryResultsResp(byte[] response)
            {
                return new QueryResultsResp(response);
            }
        };

        public class ProgramDeviceReq
        {
            public static byte[] getCommandPacket(uint address, byte[] data)
            {
                byte[] result = new byte[65];
                result[0] = 0;
                result[1] = PROGRAM_DEVICE;
                ConvertToBytes(address, result, 2);
                result[6] = (byte)data.Length;
                for (int i = 0, j = result.Length - data.Length; i < data.Length; i++, j++)
                    result[j] = data[i];

                return result;
            }

            
        };

        public class GetDataReq
        {
            public static byte[] getCommandPacket(uint address, byte count)
            {
                byte[] result = new byte[65];
                result[1] = GET_DATA;
                ConvertToBytes(address, result, 2);
                result[6] = count;
                return result;
            }
        };

        public class GetDataResultsResp
        {
            public byte WindowsReserved;
            public byte Command;
            public uint Address;
            public byte BytesPerPacket;
            public byte[] Data;
            public GetDataResultsResp(byte[] resp)
            {
                Address = ConvertToUint(resp, 2);
                BytesPerPacket = resp[6];
                Data=new byte[BytesPerPacket];
                for (int i = 0, j = resp.Length - BytesPerPacket; i < BytesPerPacket; i++, j++)
                    Data[i] = resp[j];
            }
        };

        
        public class UnlockConfigReq
        {
            public static byte[] getCommandPacket(byte pass)
            {
                byte[] result = new byte[65];
                result[1] = SELECT_PASSWORD_SET;
                result[2] = pass;
                return result;
            }
        }
        #endregion

        #region Constants

        public const int TIME_OUT = 2000;

        //************* BOOTLOADER COMMANDS **************
        public const byte QUERY_DEVICE = 0x02;
        public const byte SELECT_PASSWORD_SET = 0x03;
        public const byte PROGRAM_DEVICE = 0x05;
        public const byte PROGRAM_COMPLETE = 0x06;
        public const byte GET_DATA = 0x07;
        public const byte RESET_DEVICE = 0x08;
        public const byte ERASE_DEVICE = 0x04;
        public const byte GET_ENCRYPTED_FF = 0xFF;

        //*************************************************

        //************* QUERY RESULTS *********************
        public const byte QUERY_IDLE = 0xFF;
        public const byte QUERY_RUNNING = 0x00;
        public const byte QUERY_SUCCESS = 0x01;
        public const byte QUERY_WRITE_FILE_FAILED = 0x02;
        public const byte QUERY_READ_FILE_FAILED = 0x03;
        public const byte QUERY_MALLOC_FAILED = 0x04;
        //*************************************************

        //************* PROGRAMMING RESULTS ***************
        public const byte PROGRAM_IDLE = 0xFF;
        public const byte PROGRAM_RUNNING = 0x00;
        public const byte PROGRAM_SUCCESS = 0x01;
        public const byte PROGRAM_WRITE_FILE_FAILED = 0x02;
        public const byte PROGRAM_READ_FILE_FAILED = 0x03;
        public const byte PROGRAM_RUNNING_ERASE = 0x05;
        public const byte PROGRAM_RUNNING_PROGRAM = 0x06;
        //*************************************************

        //************* ERASE RESULTS *********************
        public const byte ERASE_IDLE = 0xFF;
        public const byte ERASE_RUNNING = 0x00;
        public const byte ERASE_SUCCESS = 0x01;
        public const byte ERASE_WRITE_FILE_FAILED = 0x02;
        public const byte ERASE_READ_FILE_FAILED = 0x03;
        public const byte ERASE_VERIFY_FAILURE = 0x04;
        public const byte ERASE_POST_QUERY_FAILURE = 0x05;
        public const byte ERASE_POST_QUERY_RUNNING = 0x06;
        public const byte ERASE_POST_QUERY_SUCCESS = 0x07;
        //*************************************************

        //************* VERIFY RESULTS ********************
        public const byte VERIFY_IDLE = 0xFF;
        public const byte VERIFY_RUNNING = 0x00;
        public const byte VERIFY_SUCCESS = 0x01;
        public const byte VERIFY_WRITE_FILE_FAILED = 0x02;
        public const byte VERIFY_READ_FILE_FAILED = 0x03;
        public const byte VERIFY_MISMATCH_FAILURE = 0x04;
        //*************************************************

        //************* READ RESULTS **********************
        public const byte READ_IDLE = 0xFF;
        public const byte READ_RUNNING = 0x00;
        public const byte READ_SUCCESS = 0x01;
        public const byte READ_READ_FILE_FAILED = 0x02;
        public const byte READ_WRITE_FILE_FAILED = 0x03;
        //*************************************************

        //************* SELECT_PASSWORD_SET RESULTS *******
        public const byte SELECT_PASSWORD_SET_IDLE = 0xFF;
        public const byte SELECT_PASSWORD_SET_RUNNING = 0x00;
        public const byte SELECT_PASSWORD_SET_SUCCESS = 0x01;
        public const byte SELECT_PASSWORD_SET_FAILURE = 0x02;
        //*************************************************

        //************* BOOTLOADER STATES *****************
        public const byte BOOTLOADER_IDLE = 0xFF;
        public const byte BOOTLOADER_QUERY = 0x00;
        public const byte BOOTLOADER_PROGRAM = 0x01;
        public const byte BOOTLOADER_ERASE = 0x02;
        public const byte BOOTLOADER_VERIFY = 0x03;
        public const byte BOOTLOADER_READ = 0x04;
        public const byte BOOTLOADER_SELECT_PASSWORD_SET = 0x05;
        public const byte BOOTLOADER_RESET = 0x06;
        //*************************************************

        //************* RESET RESULTS *********************
        public const byte RESET_IDLE = 0xFF;
        public const byte RESET_RUNNING = 0x00;
        public const byte RESET_SUCCESS = 0x01;
        public const byte RESET_WRITE_FILE_FAILED = 0x02;
        //*************************************************

        //************* MEMORY REGION TYPES ***************
        public const byte MEMORY_REGION_PROGRAM_MEM = 0x01;
        public const byte MEMORY_REGION_EEDATA = 0x02;
        public const byte MEMORY_REGION_CONFIG = 0x03;
        public const byte MEMORY_REGION_END = 0xFF;
        
        //*************************************************

        
        #endregion

        #region Function
        
        internal static uint ConvertToUint(byte[] data, int startpos)
        {
            uint result = 0;
            for (int i = 0; i < 4; i++)
            {
                result += ((uint)data[startpos + i]) << (8 * i);
            }
            return result;
        }
        
        internal static void ConvertToBytes(uint data, byte[] dest, int startpos)
        {
            for (int i = startpos; i < startpos + 4; i++)
            {
                dest[i] = (byte)((data >> ((i-startpos) * 8)) & 0x000000FF);
            }
        }

        public virtual void UpdateCurrentMemRegConfig(FileStream hardwareFileStream)
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

            //hardwareFileStream.Write(QueryDeviceReq.getCommandPacket(), 0, 65);
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
            //hardwareFileStream.Read(respdata, 0, 65);
            QueryResultsResp response = QueryResultsResp.getQueryResultsResp(respdata);
            _currentMemoryRegionsConfig = response.MemoryRegionsInfo;
            _bytesPerPacket = (uint)response.BytesPerPacket;
        }

        public List<MemoryRegionInfo> GetCurrentRegionsConfig()
        {
            List<MemoryRegionInfo> result = new List<MemoryRegionInfo>();
            foreach (MemoryRegionInfo ri in _currentMemoryRegionsConfig)
                result.Add(ri.Copy());
            return result;
        }

        public int ProgramRegion(FileStream hardwareFileStream, MemoryRegion region, Action<int, int> progress)
        {
            uint currentPacketLength;
            uint noTransferredBytes = region.Size;
            uint currentAddress = region.Address;

            while (noTransferredBytes > 0)
            {
                currentPacketLength = _bytesPerPacket < noTransferredBytes ? _bytesPerPacket : noTransferredBytes;

                byte[] data = new byte[currentPacketLength];

                for(uint i=0,j=currentAddress-region.Address;i<currentPacketLength;i++,j++)
                {
                    data[i] = region.Data[j];
                }
                hardwareFileStream.Write(ProgramDeviceReq.getCommandPacket(currentAddress, data), 0, 65);

                progress((int)(region.Size - noTransferredBytes), (int)region.Size); 

                noTransferredBytes -= currentPacketLength;
                currentAddress += currentPacketLength;
            }
            hardwareFileStream.Write(ProgramCompleteReq.getCommandPacket(), 0, 65);


            return 0; 
        }

        public virtual int ReadRegion(FileStream hardwareFileStream, MemoryRegion region, Action<int, int> progress)
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

                GetDataResultsResp response = new GetDataResultsResp(rawdata);

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

        public void ResetHardware(FileStream hardwareFileStream)
        {
            hardwareFileStream.Write(ResetDeviceReq.getCommandPacket(), 0, 65);
        }

        public void ResetHardware(string path)
        {

            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            
            uint wrBytes;

            

            if (!WriteFile(asyncWriteHandle, ResetDeviceReq.getCommandPacket(), 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }
            
            asyncWriteHandle.Close();
        }

        public virtual void ResetHardware(){} 
        

        public void EraseHardware(FileStream hardwareFileStream)
        {
            hardwareFileStream.Write(EraseDeviceReq.getCommandPacket(), 0, 65);
        }
        
        public void EraseHardware(string path)
        {
            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            
            uint wrBytes;

            

            if (!WriteFile(asyncWriteHandle, EraseDeviceReq.getCommandPacket(), 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }
            
            asyncWriteHandle.Close();
        }

        public void SetTaskNumber(FileStream hardwareFileStream, byte taskNumber)
        {
            hardwareFileStream.Write(UnlockConfigReq.getCommandPacket(taskNumber), 0, 65);
        }

        public void SetTaskNumber(string path, byte taskNumber)
        {
            SafeFileHandle asyncWriteHandle = CreateFile(path, Win32HardwareIOSupport.GENERIC_WRITE,
                        Win32HardwareIOSupport.FILE_SHARE_WRITE | Win32HardwareIOSupport.FILE_SHARE_READ, IntPtr.Zero, Win32HardwareIOSupport.OPEN_EXISTING, 0, IntPtr.Zero);
            if (asyncWriteHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            
            uint wrBytes;



            if (!WriteFile(asyncWriteHandle, UnlockConfigReq.getCommandPacket(taskNumber), 65, out wrBytes, IntPtr.Zero))
            {

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }
            
            asyncWriteHandle.Close();
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ReadFile(
            SafeFileHandle hFile,
            IntPtr pBuffer,        
            uint NumberOfBytesToRead,  
            out uint lpNumberOfBytesRead,
            ref NativeOverlapped lpOverlapped 
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ReadFile(
            SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 65)] byte[] pBuffer,
            uint NumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool WriteFile(
            SafeFileHandle hFile,
            IntPtr lpBuffer,
            uint nNumberOfBytesToWrite, 
            out uint lpNumberOfBytesWritten,
            ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool WriteFile(
            SafeFileHandle hFile,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 65)]byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile
            ([MarshalAs(UnmanagedType.LPWStr)] string strName,
            uint nAccess,
            uint nShareMode,
            IntPtr lpSecurity,
            uint nCreationFlags,
            uint nAttributes,
            IntPtr lpTemplate
            );
        public virtual void ReadEEPROM(Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress){}
        public virtual void ProgramFirmware(List<MemoryRegion> regions, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress){}


        #endregion

        #region Fields
        
        protected List<MemoryRegionInfo> _currentMemoryRegionsConfig;

        protected uint _bytesPerPacket;
        
        #endregion




    }

}
