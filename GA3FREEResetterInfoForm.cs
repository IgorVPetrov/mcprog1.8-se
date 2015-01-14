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
    public partial class GA3FREEResetterInfoForm : Form
    {
        public GA3FREEResetterInfoForm()
        {
            InitializeComponent();
        }
        public void SetBusyState()
        {
            if (!enablePowerCheckBox.Checked)
                enablePowerCheckBox.Enabled = false;
            resetButton.Enabled = false;
            //unlock3600.Enabled = false;
            //unlock3635.Enabled = false;
            M24CXX_VoltageComboBox.Enabled = false;
            XC01_VoltageComboBox.Enabled = false;
            AT88_VoltageComboBox.Enabled = false;
            //XC01_AddressComboBox.Enabled = false;
            //M24CXX_AddressComboBox.Enabled = false;
            //scanXC01AddressButton.Enabled = false;
            resetXC01OTPButton.Enabled = false;



        }
        public void SetReadyState()
        {
            enablePowerCheckBox.Enabled = true;
            resetButton.Enabled = true;
            //unlock3600.Enabled = true;
            //unlock3635.Enabled = true;
            M24CXX_VoltageComboBox.Enabled = true;
            XC01_VoltageComboBox.Enabled = true;
            AT88_VoltageComboBox.Enabled = true;
            //XC01_AddressComboBox.Enabled = true;
            //M24CXX_AddressComboBox.Enabled = true;
            //scanXC01AddressButton.Enabled = true;
            resetXC01OTPButton.Enabled = true;
        }    
    }
}
