using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace mcprog
{
    public class FileWorker 
    {
        private List<BackgroundWorker> _workers=new List<BackgroundWorker>();
        private IDictionary<string, DoWorkEventHandler> _readDelegates;
        private IDictionary<string, DoWorkEventHandler> _writeDelegates;

        public event mcprog.FileWorkerCompleteEventHandler ReadCompleted;

        public event mcprog.FileWorkerCompleteEventHandler WriteCompleted;

        public FileWorker()
        {
            _readDelegates = new Dictionary<string, DoWorkEventHandler>()
            {
                {"hex", ReadHex},
                {"bin", ReadBin},
                {"eep", ReadPony},
                {"e2p", ReadPony},
                {"rom", ReadPony}
            };
            _writeDelegates = new Dictionary<string, DoWorkEventHandler>()
            {
                {"hex", WriteHex},
                {"bin", WriteBin}
            };        
        }
        /// <summary>
        /// read regions from file 
        /// </summary>
        /// <returns>eror code</returns>
        public void Read(List<MemoryRegion> regions, string path)
        {
            BackgroundWorker worker = new BackgroundWorker();
            _workers.Add(worker);
            worker.DoWork += _readDelegates[path.Substring(path.Length - 3, 3)];
            worker.RunWorkerCompleted += ReadWorkerCompleted;
            worker.RunWorkerAsync(new object[2]{regions, path}); 
        }

        /// <summary>
        /// write regions to file
        /// </summary>
        public void Write(List<MemoryRegion> regions, string path)
        {
            BackgroundWorker worker = new BackgroundWorker();
            _workers.Add(worker);
            worker.DoWork += _writeDelegates[path.Substring(path.Length - 3, 3)];
            worker.RunWorkerCompleted += WriteWorkerCompleted;
            worker.RunWorkerAsync(new object[2] { regions, path }); 
        }

        private void ReadHex(Object sender, DoWorkEventArgs args)
        {
            List<MemoryRegion> regions = (List<MemoryRegion>)(((object[])(args.Argument))[0]);
            string path = (string)(((object[])(args.Argument))[1]);
            
            
            
            IDictionary<uint, byte[]> readedData = new Dictionary<uint, byte[]>();

            StreamReader file = new StreamReader(path);
            try
            {
                int stringNum = 0;
                uint highAddr = 0;
                while (!file.EndOfStream)
                {
                    stringNum++;
                    string sourse = file.ReadLine();
                    sourse = sourse.Trim();
                    if (!((sourse.Substring(0, 1)).Contains(":")) || (sourse.Length < 3)) throw new InvalidDataException(" File : "+path+" Symbol ':' not found in string " + stringNum.ToString());
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
                        byte[] data=new byte[dataSize];
			    
                        switch(recordType)
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
						    if(dataSize>0)
						    {
                                for(int i=0;i<dataSize;i++)
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
            finally
            {
                file.Close();
            }
            args.Result = args.Argument;
        }

        private void ReadPony(object sender, DoWorkEventArgs args)
        {
            List<MemoryRegion> regions = (List<MemoryRegion>)(((object[])(args.Argument))[0]);
            string path = (string)(((object[])(args.Argument))[1]);
            BinaryReader file = new BinaryReader(File.OpenRead(path));
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
            finally
            {
                file.Close();
            }
            args.Result = args.Argument;
        }

        private void ReadBin(object sender, DoWorkEventArgs args)
        {
            List<MemoryRegion> regions = (List<MemoryRegion>)(((object[])(args.Argument))[0]);
            string path = (string)(((object[])(args.Argument))[1]);
            BinaryReader file = new BinaryReader(File.OpenRead(path));
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
            finally
            {
                file.Close();
            }
            args.Result = args.Argument;
        }

        private void WriteHex(object sender, DoWorkEventArgs args)
        {
            List<MemoryRegion> regions = (List<MemoryRegion>)(((object[])(args.Argument))[0]);
            string path = (string)(((object[])(args.Argument))[1]);
            args.Result = args.Argument;
            StreamWriter file = new StreamWriter(path);

            try
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
                    while ((count == 16) && (addr < (region.Address+region.Size)))
                    {
                        byte[] data = new byte[16];
                        count = region.ReadData(addr, data);
                        string temp = "";
                        for (int i = 0; i < count; i++)
                        {
                            temp = temp + HexStringConverter.ByteToHS2(data[i]);
                        }
                        temp=HexStringConverter.ByteToHS2((byte)count)+
                            HexStringConverter.UInt16ToHS4(lowAddr) + "00" + temp;
                        temp = ":" + temp + HexStringConverter.ByteToHS2(CalculateCheckSum(temp));
                        file.WriteLine(temp);
                        addr += 16;
                        lowAddr += 16;
                    }


                }
                file.WriteLine(":00000001FF");

            }
            finally
            {
                file.Close();
            }
            args.Result = args.Argument;
        }

        private void WriteBin(object sender, DoWorkEventArgs args)
        {
            List<MemoryRegion> regions = (List<MemoryRegion>)(((object[])(args.Argument))[0]);
            string path = (string)(((object[])(args.Argument))[1]);
            BinaryWriter file = new BinaryWriter(File.OpenWrite(path));

            try
            {
                file.Write(regions[0].Data);
            }
            finally
            {
                file.Close();
            }
            args.Result = args.Argument;
        }

        public void ReadWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _workers.Remove((BackgroundWorker) sender);
            if (e.Cancelled) return;

            List<MemoryRegion> regions=null;
            string path=null;

            if (e.Error == null)
            {
                regions = (List<MemoryRegion>)(((object[])(e.Result))[0]);
                path = (string)(((object[])(e.Result))[1]);
            }
            
            
            ReadCompleted(this, new FileWorkerCompleteEventArgs(path, e.Error, regions));
            
        }
        public void WriteWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _workers.Remove((BackgroundWorker)sender);
            if (e.Cancelled) return;
            List<MemoryRegion> regions = null;
            string path = null;

            if (e.Error == null)
            {
                regions = (List<MemoryRegion>)(((object[])(e.Result))[0]);
                path = (string)(((object[])(e.Result))[1]);
            }
            
            WriteCompleted(this, new FileWorkerCompleteEventArgs(path, e.Error, regions));
        }
        private byte CalculateCheckSum(string s)
        {
			UInt32 CalcCS=0;
			for(int i=0;i<s.Length;i+=2)
			{
				CalcCS+=HexStringConverter.HS2toByte(s.Substring(i,2));
			}
			return (Byte)((~CalcCS+1)& 0x000000FF);
        }
    }
}
