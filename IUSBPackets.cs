using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    interface IUSBPacket
    {
        byte[] GetPacket();
        void SetPacket(byte[] packet);
    }
}
