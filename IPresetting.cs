using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace mcprog
{
    public interface IPresetting
    {
        Form GetPresettingWindow();
        void ClosePresettingWindow();
        void HidePresettingWindow();
        void ShowPresettingWindow();
    }
}
