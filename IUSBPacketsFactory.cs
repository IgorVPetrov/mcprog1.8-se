using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public interface IUSBPacketsFactory
    {
        uint NumberOfPackets
        {
            get;
        }
        byte[] GetTxPacket(uint number);
        void AddRxPacket(byte[] packet);
    }
}
