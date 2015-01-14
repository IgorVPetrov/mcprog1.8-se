using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Collections;

namespace mcprog
{
    public interface IAT88Programmer : IChipProgrammer
    {
        ICollection<string> SupportedChips
        {
            get;
        }

        int ReadChip(string crumType, List<MemoryRegionInfo> regions, System.Action<ProgrammingCompleteInfo, System.Collections.Generic.List<MemoryRegion>> completed, System.Action<ProgrammingProgressInfo> progress);

        int ProgramChip(string crumType, List<MemoryRegion> regions, System.Action<ProgrammingCompleteInfo> completed, System.Action<ProgrammingProgressInfo> progress);
    }
}
