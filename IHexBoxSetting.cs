using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace mcprog
{
    interface IHexBoxSetting
    {
        Encoding HexEncoding
        {
            get;
            set;
        }

        Font HexFont
        {
            get;
            set;
        }
    }
}
