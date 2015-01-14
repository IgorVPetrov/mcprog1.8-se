using System;
using System.Collections.Generic;
using System.Text;

namespace mcprog
{
    public class MemoryRegion
    {
        private uint _address;

        public uint Address
        {
            get { return _address; }
        }
        private int _type;

        public int Type
        {
            get { return _type; }
        }
        private uint _size;

        public uint Size
        {
            get { return _size; }
        }
        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
        }

        public MemoryRegion(uint address, uint size, int type)
        {
            _address = address;
            _size = size;
            _type = type;
            _data=new byte[_size];
            for (uint i = 0; i < _size; i++) _data[i] = 0xFF;
            
        }

        public uint WriteData(uint addr, byte[] data)
        {
            uint deltaaddr = addr - _address;
            if ((deltaaddr < 0) || (deltaaddr >= _size)) return 0;
            uint limit = (deltaaddr + data.Length) < _size ? (uint)data.Length : (_size - deltaaddr);
            for (uint i = 0; i < limit; i++)
                _data[deltaaddr + i] = data[i];
            return limit;
        }

        public uint ReadData(uint addr, byte[] data)
        {
            uint deltaaddr = addr - _address;
            if ((deltaaddr < 0) || (deltaaddr >= _size)) return 0;
            uint limit = (deltaaddr + data.Length) < _size ? (uint)data.Length : (_size - deltaaddr);
            for (uint i = 0; i < limit; i++)
                data[i] = _data[deltaaddr + i];
            return limit;
        }
    }

    public class MemoryRegionInfo : IEquatable<MemoryRegionInfo>
    {
        public byte Type;
        public uint Address;
        public uint Size;

        public bool Equals(MemoryRegionInfo other)
        {
            return (this.Type == other.Type) &&
                (this.Address == other.Address) &&
                (this.Size == other.Size);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is MemoryRegionInfo))
                throw new InvalidCastException("The 'obj' argument is not a MemoryRegionInfo object.");
            else
                return Equals(obj as MemoryRegionInfo);
        }

        public override int GetHashCode()
        {
            return (int)(this.Type + this.Address + this.Size);
        }

        public static bool operator ==(MemoryRegionInfo region1, MemoryRegionInfo region2)
        {
            return region1.Equals(region2);
        }

        public static bool operator !=(MemoryRegionInfo region1, MemoryRegionInfo region2)
        {
            return (!region1.Equals(region2));
        }

    }

}
