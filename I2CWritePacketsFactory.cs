using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    class I2CWritePacketsFactory : IUSBPacketsFactory
    {

        MemoryRegion region;
        uint bytesPerPacket = 16;
        uint lastPacketSize;
        uint numberOfPackets;
        int addressSize;

        public I2CWritePacketsFactory(MemoryRegion mreg)
        {
            region = mreg;
            lastPacketSize = mreg.Size % bytesPerPacket;
            numberOfPackets = mreg.Size / bytesPerPacket + (uint)(lastPacketSize == 0 ? 0 : 1);
            addressSize = mreg.Size > 2048 ? 3 : 2;

        }
        
        
        uint IUSBPacketsFactory.NumberOfPackets
        {
            get
            {
                return numberOfPackets;
            }
        }

        byte[] IUSBPacketsFactory.GetTxPacket(uint number)
        {
            byte[] packet = new byte[65];
            packet[1] = 0x0F;//I2C read command
            packet[9] = (byte)bytesPerPacket;//I2C packet data size
            packet[2] = 0x00;//Power indef
            if (number == 0)
            {
                packet[2] = 0x01;//Power ON
            }
            else if (number == numberOfPackets - 1)
            {
                packet[2] = 0x02;//Power OFF
                if (lastPacketSize > 0) packet[9] = (byte)lastPacketSize;//I2C packet data size
            }
            packet[4] = (byte)addressSize;//I2C address size

            uint address = number * bytesPerPacket;

            if (addressSize == 2)
            {
                packet[5] = (byte)((address >> 7) & (uint)0x0E | (uint)0xA0);//I2C first address
                packet[6] = (byte)(address & (uint)0xFF);//I2C second address
            }
            else
            {
                packet[5] = (byte)((address >> 15) & (uint)0x0E | (uint)0xA0);//I2C first address
                packet[6] = (byte)((address >> 8) & (uint)0xFF);//I2C second address
                packet[7] = (byte)(address & (uint)0xFF);//I2C third address
            }

            //byte[] packData = new byte[packet[9]];
            //region.ReadData(address, packData);
            //region.Data
            Array.Copy(region.Data, address, packet, 17, packet[9]);

            return packet;
        }

        void IUSBPacketsFactory.AddRxPacket(byte[] packet)
        {
            
            uint address = 0;

            if (addressSize == 2)
            {
                address = (((uint)(packet[5] & (byte)0x0E)) << 7) | (uint)packet[6];
            }
            else
            {
                address = (((uint)(packet[5] & (byte)0x0E)) << 15) | ((uint)packet[6] << 8) | (uint)packet[7];
            }

            byte[] readedData = new byte[packet[9]];
            Array.Copy(packet, 17, readedData, 0, packet[9]);
            byte[] sourceData = new byte[packet[9]];
            Array.Copy(region.Data, address, sourceData, 0, packet[9]);
            if (packet[8] != 0)
            {
                if (packet[8] == 0xAA)
                {
                    for (uint i = 0; i < (uint)packet[9]; i++)
                    {
                        if (readedData[i] != sourceData[i])
                            throw new Exception("I2C verification error at address: " + (address + i).ToString("X4") + "write: " + sourceData[i].ToString("X2") + "read: " + readedData[i].ToString("X2"));
                    }

                }
                else
                    throw new Exception("I2C write error " + packet[8].ToString("X2"));


            } 

        }
    }
}
