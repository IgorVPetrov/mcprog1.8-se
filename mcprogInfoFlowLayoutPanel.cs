using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mcprog
{
    public partial class mcprogInfoFlowLayoutPanel : FlowLayoutPanel
    {
        public mcprogInfoFlowLayoutPanel()
        {
            InitializeComponent();
            this.HScroll = true;
            this.VScroll = true;
        }
    }
}
