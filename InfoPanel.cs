using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace mcprog
{
    public partial class InfoPanel : FlowLayoutPanel
    {

        private enum InfoPanelSize
        {
            Normal,
            Info
        }
        InfoPanelSize _panelSize = InfoPanelSize.Normal;

    
        public InfoPanel()
        {
            InitializeComponent();
            
        }

        public InfoPanel(IContainer container)
        {
            container.Add(this);

            



            InitializeComponent();
        }

        private void InfoPanel_Resize(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Location = new Point((Parent.Width - Width) / 2, (Parent.Height - Height) / 2);

            }

        }

        private void InfoPanel_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Parent.Resize += InfoPanel_Resize;
                Location = new Point((Parent.Width - Width) / 2, (Parent.Height - Height) / 2);
            }
        }

        public Action<ProgrammingProgressInfo> GetProgressDelegate()
        {
            return (Action<ProgrammingProgressInfo>)delegate(ProgrammingProgressInfo ppInfo)
            {
                if (ppInfo.ProgressBarDataChanged)
                {
                    progressBar.Minimum = ppInfo.ProgressBarData.Minimum;
                    progressBar.Maximum = ppInfo.ProgressBarData.Maximum;
                    progressBar.Value = ppInfo.ProgressBarData.Value;
                }
                if (ppInfo.MessageChanged) label.Text = ppInfo.Message;
            };
        }

        public void SetProgressState(string info)
        {
            Visible = false;
            //BackColor = Color.White;
            setNormalSize();
            progressBar.Value = progressBar.Minimum;
            this.closeTimer.Stop();
            ForeColor = Color.Blue;
            progressBar.Visible = true;
            label.Visible = true;
            button.Visible = false;
            if (info != null)
            {
                label.Text = info;

            }
            Visible = true;
        }
        public void SetErrorState(string info)
        {
            Visible = false;
            setNormalSize();
            progressBar.Value = progressBar.Minimum;
            this.closeTimer.Stop();
            ForeColor = Color.Red;
            progressBar.Visible = false;
            label.Visible = true;
            button.Visible = true;
            if (info != null)
            {
                label.Text = info;
            }
            Visible = true;
        }
        public void SetOkState(string info)
        {
            Visible = false;
            setNormalSize();
            progressBar.Value = progressBar.Minimum;
            this.closeTimer.Stop();
            ForeColor = Color.Green;
           // ForeColor = Color.White;
            //BackColor = Color.Green;
            progressBar.Visible = false;
            label.Visible = true;
            button.Visible = true;
           // button.ForeColor = Color.White;
           // button.BackColor = Color.Green;
            
            if (info != null)
            {
                label.Text = info;
            }
            this.closeTimer.Interval = 700;
            this.closeTimer.Start();
            Visible = true;
        }
        public void SetInfoState(string info)
        {
            Visible = false;
            setInfoSize();
            progressBar.Value = progressBar.Minimum;
            //BackColor = Color.White;
            ForeColor = Color.Blue;
            progressBar.Visible = false;
            label.Visible = true;
            button.Visible = false;
            if (info != null)
            {
                label.Text = info;
            }
            
            Visible = true;
        }

        private void setNormalSize()
        {
            if (_panelSize == InfoPanelSize.Normal) return;
            _panelSize = InfoPanelSize.Normal;
            label.MaximumSize = new System.Drawing.Size(150, 700);
            label.MinimumSize = new System.Drawing.Size(150, 30);
            label.Size = new System.Drawing.Size(150, 30);
        }
        private void setInfoSize()
        {
            if (_panelSize == InfoPanelSize.Info) return;
            _panelSize = InfoPanelSize.Info;
            label.MaximumSize = new System.Drawing.Size(200, 200);
            label.MinimumSize = new System.Drawing.Size(200, 30);
            label.Size = new System.Drawing.Size(200, 30);
        }

        private void button_Click(object sender, EventArgs e)
        {
            this.closeTimer.Stop();
            this.Visible = false;
        }

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            this.closeTimer.Stop();
            this.Visible = false;
        }
    
    }
}
