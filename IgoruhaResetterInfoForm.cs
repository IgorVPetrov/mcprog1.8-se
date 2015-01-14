using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mcprog
{
    public partial class IgoruhaResetterInfoForm : Form
    {
        public IgoruhaResetterInfoForm()
        {
            InitializeComponent();
        }
        public void SetBusyState()
        { 
            _resetButton.Enabled = false;
        }
        public void SetReadyState()
        {
            _resetButton.Enabled = true;
        }
    }
}
