using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace POETradeIndexer
{
    public partial class Form1 : Form
    {
        Controller myController;
        poe_change_getter myGetter;

        public Form1()
        {
            InitializeComponent();
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

        private void start_btn_Click(object sender, EventArgs e)
        {
            this.status_lbl.Text = "Started";
            main_bgw.RunWorkerAsync();
            changeGetter_bw.RunWorkerAsync();
        }

        private void stop_btn_Click(object sender, EventArgs e)
        {
            myController.stopRunning();
            myGetter.stopRunning();
        }

        private void changeGetter_bw_DoWork_1(object sender, DoWorkEventArgs e)
        {
        myGetter = new poe_change_getter();
            myGetter.keepRunning = true;
            myGetter.start();
        }
    }
}
