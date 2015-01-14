using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public interface IM25CXXProgrammer
    {
        ICollection<string> SupportedChips
        {
            get;
        }
        int ReadChip(string chip, MemoryRegionInfo regionInfo, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int ProgramChip(string chip, MemoryRegion region, System.Action<ProgrammingCompleteInfo> completed, System.Action<ProgrammingProgressInfo> progress);
    }
}
