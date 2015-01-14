using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;

namespace mcprog
{
    public partial class InfoForm : Form
    {

        bool redirect = false;
        
        
        public InfoForm()
        {
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            string path = Application.ExecutablePath;
            path = path.Substring(0, path.LastIndexOf("\\")) + "\\info\\info.htm";
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
                webBrowser.Url = uri;
            }
            else
            {
                string page = mcprog.Properties.Resources.alterPage;
                
                webBrowser.DocumentText = page;

            }
            webBrowser.CanGoBackChanged += webBrowser_CanGoBackChanged;
            webBrowser.CanGoForwardChanged += webBrowser_CanGoForwardChanged;
            backwardNavigButton.Enabled = false;
            forwardNavigButton.Enabled = false;
        }

        private void InfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.webBrowser.Dispose();
        }

        private void backwardNavigButton_Click(object sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void forwardNavigButton_Click(object sender, EventArgs e)
        {
            webBrowser.GoForward();
        }

        private void homeNavigButton_Click(object sender, EventArgs e)
        {
            webBrowser.Url = new Uri(Directory.GetCurrentDirectory() + "\\info\\info.htm", UriKind.RelativeOrAbsolute);
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!redirect&&(e.Url.ToString().Contains("http")))
            {
                e.Cancel = true;
                redirect = true;
                webBrowser.Navigate(e.Url, true);
            }
            if (redirect) redirect = false;
        }
        private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            backwardNavigButton.Enabled = webBrowser.CanGoBack;
        }
        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            forwardNavigButton.Enabled = webBrowser.CanGoForward;
        }
       

        

    }
}
