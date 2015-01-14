using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Threading;


namespace mcprog
{
    public class FileWorker
    {
                
        public class HexStringConverter
        {
            private static IDictionary<string, byte> hston = new Dictionary<string, byte>();
            private static IDictionary<byte, string> ntohs = new Dictionary<byte, string>();

            static HexStringConverter()
            {
                string[] hex = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
                for (byte i = 0; i < 16; i++)
                {
                    hston.Add(hex[i], i);
                    ntohs.Add(i, hex[i]);
                }
            }

            public static Byte HS2toByte(string s)
            {
                return (byte)((hston[s.Substring(0, 1)] << 4) + hston[s.Substring(1, 1)]);
            }

            public static string ByteToHS2(byte b)
            {
                return ntohs[(byte)(b / 16)] + ntohs[(byte)(b % 16)];
            }

            public static UInt16 HS4toUInt16(string s)
            {
                ushort res = hston[s.Substring(0, 1)];
                for (int i = 1; i < 4; i++)
                    res = (ushort)((res << 4) + hston[s.Substring(i, 1)]);
                return res;
            }

            public static string UInt16ToHS4(ushort b)
            {
                String s = "";
                for (int i = 0; i < 4; i++)
                {
                    s = ntohs[(byte)(b % 16)] + s;
                    b /= 16;
                };
                return s;
            }
        }

        private byte CalculateCheckSum(string s)
        {
            UInt32 CalcCS = 0;
            for (int i = 0; i < s.Length; i += 2)
            {
                CalcCS += HexStringConverter.HS2toByte(s.Substring(i, 2));
            }
            return (Byte)((~CalcCS + 1) & 0x000000FF);
        }
        
        public void Read(string path, List<MemoryRegionInfo> regionsInfo, Action<FileWorkerIOCompleteInfo> complete)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            IDictionary<string, Action> readers = new Dictionary<string, Action>();

            FileWorkerIOCompleteInfo completeInfo = new FileWorkerIOCompleteInfo();

            
            
            completeInfo.RegionsInfo = regionsInfo;
            completeInfo.Path = path;
            
            List<MemoryRegion> regions = new List<MemoryRegion>();
            foreach (MemoryRegionInfo info in regionsInfo)
                regions.Add(new MemoryRegion(info));
 
            readers.Add("hex",
            #region hexdelegate
                delegate()
                {
                    IDictionary<uint, byte[]> readedData = new Dictionary<uint, byte[]>();

                    using (StreamReader file = new StreamReader(path))
                    {
                        int stringNum = 0;
                        uint highAddr = 0;
                        while (!file.EndOfStream)
                        {
                            stringNum++;
                            string sourse = file.ReadLine();
                            sourse = sourse.Trim();
                            if (!((sourse.Substring(0, 1)).Contains(":")) || (sourse.Length < 3)) throw new InvalidDataException(" File : " + path + " Symbol ':' not found in string " + stringNum.ToString());
                            try
                            {
                                byte dataSize = HexStringConverter.HS2toByte(sourse.Substring(1, 2));
                                ushort lowAddr = HexStringConverter.HS4toUInt16(sourse.Substring(3, 4));
                                byte recordType = HexStringConverter.HS2toByte(sourse.Substring(7, 2));
                                byte checkSum = HexStringConverter.HS2toByte(sourse.Substring(sourse.Length - 2, 2));

                                uint CalcCS = 0;
                                for (int i = 0; i < (dataSize + 4); i++)
                                {
                                    CalcCS += HexStringConverter.HS2toByte(sourse.Substring(i * 2 + 1, 2));
                                }
                                CalcCS = (~CalcCS + 1) & 0x000000FF;
                                if (CalcCS != (uint)checkSum) throw new InvalidDataException(" File : " + path + " Check sum error in string " + stringNum.ToString());
                                byte[] data = new byte[dataSize];

                                switch (recordType)
                                {
                                    case 0x04:
                                        if (dataSize == 1)
                                            highAddr = ((uint)(HexStringConverter.HS2toByte(sourse.Substring(9, 2)))) << 16;
                                        else if (dataSize == 2)
                                            highAddr = ((uint)(HexStringConverter.HS4toUInt16(sourse.Substring(9, 4)))) << 16;
                                        else throw new InvalidDataException(" File : " + path + " Invalid high address record in string " + stringNum.ToString());
                                        break;

                                    case 0x01:
                                        break;

                                    case 0x00:
                                        if (dataSize > 0)
                                        {
                                            for (int i = 0; i < dataSize; i++)
                                            {
                                                data[i] = HexStringConverter.HS2toByte(sourse.Substring(9 + i * 2, 2));
                                            }
                                            readedData.Add(highAddr + HexStringConverter.HS4toUInt16(sourse.Substring(3, 4)), data);
                                        }
                                        break;
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                throw new InvalidDataException(" File : " + path + " Incorrect symbol found in string " + stringNum.ToString());
                            }
                        }

                        foreach (KeyValuePair<uint, byte[]> line in readedData)
                            foreach (MemoryRegion region in regions)
                                if (region.WriteData(line.Key, line.Value) > 0) break;
                        

                    }

                }
            #endregion
            );
            readers.Add("bin",
            #region bindelegate
                delegate()
                {
                    using(BinaryReader file = new BinaryReader(File.OpenRead(path)))
                    {
                    file.BaseStream.Position = 0;
                    try
                    {
                        for (int i = 0; i < regions[0].Data.Length; i++)
                        {
                            regions[0].Data[i] = file.ReadByte();
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        throw new InvalidDataException(" File : " + path + " Size too small");
                    }
                    
                    }
                    
                }
            #endregion
            );
            readers.Add("eep",
            #region ponydelegate
                delegate()
                {
                    using(BinaryReader file = new BinaryReader(File.OpenRead(path)))
                    {
                    file.BaseStream.Position = 152;
                    try
                    {
                        for (int i = 0; i < regions[0].Data.Length; i++)
                        {
                            regions[0].Data[i] = file.ReadByte();
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        throw new InvalidDataException(" File : " + path + " Size too small");
                    }
                    
                    }
                }
            #endregion
            );
            readers.Add("e2p", readers["eep"]);
            readers.Add("rom", readers["eep"]);
 
            ThreadStart start = delegate()
            {
                try
                {
                    readers[path.Substring(path.Length - 3, 3).ToLower()]();
                    completeInfo.Regions = regions;
                }
                catch (KeyNotFoundException)
                {
                    completeInfo.Error = new Exception("Not supported file type");
                }
                catch (Exception e)
                {
                    completeInfo.Error = e;
                }
                
                asyncOp.PostOperationCompleted(delegate(object arg) { complete(completeInfo); }, null);
            };
            (new Thread(start)).Start();
                
        }
        public void Write(string path, List<MemoryRegion> regions, Action<FileWorkerIOCompleteInfo> complete)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            IDictionary<string, Action> writers = new Dictionary<string, Action>();

            FileWorkerIOCompleteInfo completeInfo = new FileWorkerIOCompleteInfo();
            completeInfo.Path = path;

            writers.Add("hex",
            #region hexdelegate
            delegate()
                {
                    using(StreamWriter file = new StreamWriter(path))
                    {
                        foreach (MemoryRegion region in regions)
                        {
                            ushort lowAddr = (ushort)(region.Address & 0x0000FFFF);
                            ushort highAddr = (ushort)(region.Address >> 16);
                            if (highAddr > 0)
                            {
                                String temp = "02000004" + HexStringConverter.UInt16ToHS4(highAddr);
                                temp = ":" + temp + HexStringConverter.ByteToHS2(CalculateCheckSum(temp));
                                file.WriteLine(temp);
                            }
                            uint count = 16;
                            uint addr = region.Address;
                            while ((count == 16) && (addr < (region.Address + region.Size)))
                            {
                                byte[] data = new byte[16];
                                count = (uint)region.ReadData(addr, data);
                                string temp = "";
                                for (int i = 0; i < count; i++)
                                {
                                    temp = temp + HexStringConverter.ByteToHS2(data[i]);
                                }
                                temp = HexStringConverter.ByteToHS2((byte)count) +
                                    HexStringConverter.UInt16ToHS4(lowAddr) + "00" + temp;
                                temp = ":" + temp + HexStringConverter.ByteToHS2(CalculateCheckSum(temp));
                                file.WriteLine(temp);
                                addr += 16;
                                lowAddr += 16;
                            }

                        }
                        file.WriteLine(":00000001FF");

                    }                   
                }
            #endregion
            );
            writers.Add("bin",
            #region bindelegate
            delegate()
                {
                    using(BinaryWriter file = new BinaryWriter(File.OpenWrite(path)))
                    {
                        file.Write(regions[0].Data);
                    } 
                }
            #endregion
            );

            ThreadStart start = delegate()
            {
                try
                {
                    writers[path.Substring(path.Length - 3, 3).ToLower()]();
                    completeInfo.Info = "Write complete";
                }
                catch (KeyNotFoundException)
                {
                    completeInfo.Error = new Exception("Not supported file type");
                }
                catch (Exception e)
                {
                    completeInfo.Error = e;
                }
                asyncOp.PostOperationCompleted(delegate(object arg) { complete(completeInfo); }, null);
            };
            (new Thread(start)).Start();

        }



    }

    public class FileWorkerIOCompleteInfo
    {
        public string Info=null;
        public string Path=null;
        public Exception Error=null;
        public List<MemoryRegion> Regions=null;
        public List<MemoryRegionInfo> RegionsInfo=null;
    }
}
