using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Collections;

namespace mcprog
{
    public interface IM24CXXProgrammer : IChipProgrammer
    {
        ICollection<string> SupportedChips
        {
            get;
        }
        int ReadChip(string chip, MemoryRegionInfo regionInfo, System.Action<ProgrammingCompleteInfo, MemoryRegion> completed, System.Action<ProgrammingProgressInfo> progress);

        int ProgramChip(string chip, MemoryRegion region, System.Action<ProgrammingCompleteInfo> completed, System.Action<ProgrammingProgressInfo> progress);
    }
}
