using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using POETradeIndexer;

namespace POEChangeGetService
{
    public partial class POEChangeGetService : ServiceBase
    {
        poe_change_getter myGetter;

        public POEChangeGetService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            changeGetter_bw.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            myGetter.stopRunning();
        }

        private void changeGetter_bw_DoWork(object sender, DoWorkEventArgs e)
        {
            myGetter = new poe_change_getter();
            myGetter.keepRunning = true;
            myGetter.start();
        }

        private void changeGetter_bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void changeGetter_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
