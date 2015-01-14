using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public interface IOneWireProgrammer : IChipProgrammer
    {
        ICollection<string> SupportedChips
        {
            get;
        }
        int ExecuteCommandSet(byte[] data, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int ExecuteCommandSet(List<byte[]> data, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);
    
    }
}
