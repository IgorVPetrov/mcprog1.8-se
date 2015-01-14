using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public interface IRFIDProgrammer : IChipProgrammer
    {
        int Read(byte[] data, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int Write(List<byte[]> data, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int Adjust(System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

    }
}
