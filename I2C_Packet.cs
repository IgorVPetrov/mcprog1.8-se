using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    class I2C_Packet : IUSBPacket
    {
        byte _Command;
        
        byte _I2C_PowerControl;
        
        byte _I2C_NumberOfPackets;
        
        byte _I2C_AddressSize=2;
        
        byte _I2C_FirstAddress=0xA0;
        
        byte _I2C_SecondAddress=0;
        
        byte _I2C_ThirdAddress=0;
        
        byte _I2C_Result;
        
        byte _I2C_DataSize;
        
        byte[] _I2C_Reserved = new byte[7];
        
        byte[] _I2C_Data = new byte[48];
        
        public uint FullAddress
        {
            set
            {
                if (_I2C_AddressSize == 2)
                {
                    _I2C_FirstAddress = (byte)((value >> 7) & (uint)0x0E | (uint)0xA0);
                    _I2C_SecondAddress = (byte)(value & (uint)0xFF);
                }
                else
                {
                    _I2C_FirstAddress = (byte)((value >> 15) & (uint)0x0E | (uint)0xA0);
                    _I2C_SecondAddress = (byte)((value >> 8) & (uint)0xFF);
                    _I2C_ThirdAddress = (byte)(value & (uint)0xFF);
                }
            }
            get
            {
                if (_I2C_AddressSize == 2)
                {
                    return (((uint)(_I2C_FirstAddress & (byte)0x0E)) << 7) | (uint)_I2C_SecondAddress;
                }
                else
                {
                    return (((uint)(_I2C_FirstAddress & (byte)0x0E)) << 15) | ((uint)_I2C_SecondAddress << 8) | (uint)_I2C_ThirdAddress; 
                }
            }

        }    
        
        public byte Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        public byte I2C_PowerControl
        {
            get { return _I2C_PowerControl; }
            set { _I2C_PowerControl = value; }
        }
        
        public byte I2C_NumberOfPackets
        {
            get { return _I2C_NumberOfPackets; }
            set { _I2C_NumberOfPackets = value; }
        }
        
        public byte I2C_AddressSize
        {
            get { return _I2C_AddressSize; }
            set 
            {
                if ((value < 2) || (value > 3))
                    throw new Exception("Invalid I2C_AddressSize");
                _I2C_AddressSize = value; 
            }
        }
        
        public byte I2C_FirstAddress
        {
            get { return _I2C_FirstAddress; }
            set { _I2C_FirstAddress = value; }
        }
        
        public byte I2C_SecondAddress
        {
            get { return _I2C_SecondAddress; }
            set { _I2C_SecondAddress = value; }
        }
        
        public byte I2C_ThirdAddress
        {
            get { return _I2C_ThirdAddress; }
            set { _I2C_ThirdAddress = value; }
        }
        
        public byte I2C_Result
        {
            get { return _I2C_Result; }
            set { _I2C_Result = value; }
        }
        
        public byte I2C_DataSize
        {
            get { return _I2C_DataSize; }
            set
            {
                if (value > 48)
                    throw new Exception("I2C_DataSize > 48");
                _I2C_DataSize = value;
            }
        }
        
        public byte[] I2C_Data
        {
            get
            {
                byte[] res = new byte[_I2C_DataSize];
                Array.Copy(_I2C_Data, 0, res, 0, _I2C_DataSize);
                return res;
            }
            set
            {
                int len = value.Length < _I2C_Data.Length ? value.Length : _I2C_Data.Length;
                Array.Copy(value, 0, _I2C_Data, 0, len);
            }
        }
        
        public byte[] I2C_Reserved
        {
            get
            {
                byte[] res = new byte[7];
                Array.Copy(_I2C_Reserved, 0, res, 0, 7);
                return res;
            }
            set
            {
                int len = value.Length < _I2C_Reserved.Length ? value.Length : _I2C_Reserved.Length;
                Array.Copy(value, 0, _I2C_Reserved, 0, len);
            }
        }
        
        void IUSBPacket.SetPacket(byte[] packet)
        {
            if (packet.Length != 65)
                throw new Exception("packet.Length != 65");
            Command = packet[1];
            I2C_PowerControl = packet[2];
            I2C_NumberOfPackets = packet[3];
            I2C_AddressSize = packet[4];
            I2C_FirstAddress = packet[5];
            I2C_SecondAddress = packet[6];
            I2C_ThirdAddress = packet[7];
            I2C_Result = packet[8];
            I2C_DataSize = packet[9];
            Array.Copy(packet, 10, _I2C_Reserved, 0, 7);
            Array.Copy(packet, 17, _I2C_Data, 0, 48);
        }
        
        byte[] IUSBPacket.GetPacket()
        {
            byte[] packet = new byte[65];
            packet[1] = Command;
            packet[2] = I2C_PowerControl;
            packet[3] = I2C_NumberOfPackets;
            packet[4] = I2C_AddressSize;
            packet[5] = I2C_FirstAddress;
            packet[6] = I2C_SecondAddress;
            packet[7] = I2C_ThirdAddress;
            packet[8] = I2C_Result;
            packet[9] = I2C_DataSize;
            Array.Copy(_I2C_Reserved, 0, packet, 10, 7);
            Array.Copy(_I2C_Data, 0, packet, 17, 48);
            return packet;
        }

    }
}
