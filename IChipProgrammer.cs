using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace mcprog
{
    public interface IChipProgrammer
    {
        event EventHandler Busy;

        event EventHandler Ready;

        bool IsBusy
        {
            get;
        }

        Form GetServiceWindow();

        HardwareMode GetMode();

        //string ToString();
    }
}
