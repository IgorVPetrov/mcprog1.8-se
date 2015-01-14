using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    class I2CReadPacketsFactory: IUSBPacketsFactory
    {

        MemoryRegion region;
        uint bytesPerPacket = 48;
        uint lastPacketSize;
        uint numberOfPackets;
        int addressSize;

        public I2CReadPacketsFactory(MemoryRegionInfo reginfo)
        {
            region = new MemoryRegion(reginfo);
            lastPacketSize = reginfo.Size % bytesPerPacket;
            numberOfPackets = reginfo.Size / bytesPerPacket + (uint)(lastPacketSize == 0 ? 0 : 1);
            addressSize = reginfo.Size > 2048 ? 3 : 2;



        }
        public MemoryRegion GetRegion()
        {


            return region;
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
            packet[1] = 0x0E;//I2C read command
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

            if ( addressSize == 2)
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





            return packet;
        }
        
        void IUSBPacketsFactory.AddRxPacket(byte[] packet)
        {
            if (packet[8] != 0)
                throw new Exception("I2C read error " + packet[8].ToString("X2"));

            uint address = 0;
            
            if (addressSize == 2)
            {
                address = (((uint)(packet[5] & (byte)0x0E)) << 7) | (uint)packet[6];
            }
            else
            {
                address = (((uint)(packet[5] & (byte)0x0E)) << 15) | ((uint)packet[6] << 8) | (uint)packet[7];
            }

            byte[] packData = new byte[packet[9]];
            Array.Copy(packet, 17, packData, 0, packet[9]);
            region.WriteData(address, packData);

        }

    }
}
