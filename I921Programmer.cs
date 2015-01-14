using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public interface I921Programmer : IChipProgrammer
    {
        ICollection<string> SupportedModes
        {
            get;
        }
        int ReadChip(string mode, MemoryRegionInfo regionInfo, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int ProgramChip(string mode, MemoryRegion region, System.Action<ProgrammingCompleteInfo> completed, System.Action<ProgrammingProgressInfo> progress);    
    }
}
