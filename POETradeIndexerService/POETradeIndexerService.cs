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

namespace POETradeIndexerService
{
    public partial class POETradeIndexerService : ServiceBase
    {
        Controller myController;
        poe_change_getter myGetter;

        public POETradeIndexerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            main_bgw.RunWorkerAsync();
            changeGetter_bw.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            myController.stopRunning();
        }

        private void main_bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            myController = new Controller(main_bgw);
            myController.startRunning();
            myController.processPoeUpdate();
        }

        private void changeGetter_bw_DoWork(object sender, DoWorkEventArgs e)
        {
            myGetter = new poe_change_getter();
            myGetter.keepRunning = true;
            myGetter.start();
        }
    }
}
