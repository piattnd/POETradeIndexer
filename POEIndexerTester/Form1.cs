using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POETradeIndexer;

namespace POEIndexerTester
{
    public partial class Form1 : Form
    {
        Controller myController;

        public Form1()
        {
            InitializeComponent();
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            this.status_lbl.Text = "Started";

            main_bgw.RunWorkerAsync();
        }

        private void stop_btn_Click(object sender, EventArgs e)
        {
            myController.stopRunning();
        }

        private void main_bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            myController = new Controller(main_bgw);
            myController.startRunning();
            myController.processPoeUpdate();
        }

        private void main_bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lastUpdate_lbl.Text = e.UserState.ToString();
        }

        private void main_bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.status_lbl.Text = "Stopped";
        }
    }
}
