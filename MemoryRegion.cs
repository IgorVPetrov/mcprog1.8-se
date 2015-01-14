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
        private byte _type;

        public byte Type
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

        public MemoryRegion(uint address, uint size, byte type)
        {
            _address = address;
            _size = size;
            _type = type;
            _data=new byte[_size];
            for (uint i = 0; i < _size; i++) _data[i] = 0xFF;
            
        }

        public MemoryRegion(MemoryRegionInfo regInfo)
        {
            _address = regInfo.Address;
            _size = regInfo.Size;
            _type = regInfo.Type;
            _data = new byte[_size];
            for (uint i = 0; i < _size; i++) _data[i] = 0xFF;
        }

        public int WriteData(uint addr, byte[] data)
        {
            int deltaaddr = (int)addr - (int)_address;
            if ((deltaaddr < 0) || (deltaaddr >= (int)_size)) return 0;
            int limit = (deltaaddr + data.Length) < (int)_size ? data.Length : (int)(_size - deltaaddr);
            for (int i = 0; i < limit; i++)
                _data[deltaaddr + i] = data[i];
            return limit;
        }

        public int ReadData(uint addr, byte[] data)
        {
            int deltaaddr = (int)addr - (int)_address;
            if ((deltaaddr < 0) || (deltaaddr >= (int)_size)) return 0;
            int limit = (deltaaddr + data.Length) < (int)_size ? data.Length : (int)(_size - deltaaddr);
            for (int i = 0; i < limit; i++)
                data[i] = _data[deltaaddr + i];
            return limit;
        }

        public MemoryRegionInfo GetRegionInfo()
        {
            MemoryRegionInfo info = new MemoryRegionInfo();
            info.Address = _address;
            info.Size = _size;
            info.Type = _type;
            return info;
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

        public MemoryRegionInfo Copy()
        {
            return new MemoryRegionInfo() { Address = this.Address, Size = this.Size, Type = this.Type };
        }

    }
}
