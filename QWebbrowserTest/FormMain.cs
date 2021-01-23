using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QWebbrowserTest
{
    public partial class FormMain : Form
    {
        private bool isNavigating = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void UrlTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                string url = urlTextBox.Text;
                try
                {
                    browser.Url = new Uri(url);
                }
                catch (UriFormatException exc)
                {
                    if (!url.Contains("://")) {
                        try {
                            browser.Url = new Uri("https://" + url);
                        }
                        catch (UriFormatException exception)
                        {
                            MessageBox.Show(exception.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(exc.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void PrevButtonClick(object sender, EventArgs e)
        {
            browser.GoBack();
        }

        private void NextButtonClick(object sender, EventArgs e)
        {
            browser.GoForward();
        }

        private void RefreshButtonClick(object sender, EventArgs e)
        {
            if (this.refreshButton.Text == "r")
            {
                browser.Stop();
            } else if (this.refreshButton.Text == "q")
            {
                browser.Refresh();
            }
        }

        private void BrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.urlTextBox.Text = e.Url.ToString();
            this.prevButton.Enabled = browser.CanGoBack;
            this.nextButton.Enabled = browser.CanGoForward;
        }

        private void BrowserProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            this.pageLoadProgress.Maximum = 1000;
            this.pageLoadProgress.Value = (int)(1000 * e.CurrentProgress / e.MaximumProgress);
            if (e.CurrentProgress == e.MaximumProgress)
            {
                pageLoadProgress.Value = 0;
            }
        }

        private void browser_NewWindow(object sender, CancelEventArgs e)
        {
            new FormMain();
        }

        private void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            this.refreshButton.Text = "r";
            this.isNavigating = true;
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.isNavigating = false;
            this.refreshButton.Text = "q";
        }
    }
}
