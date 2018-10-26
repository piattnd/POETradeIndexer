namespace POETradeIndexerService
{
    partial class POETradeIndexerService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.main_bgw = new System.ComponentModel.BackgroundWorker();
            this.changeGetter_bw = new System.ComponentModel.BackgroundWorker();
            // 
            // main_bgw
            // 
            this.main_bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.main_bgw_DoWork);
            // 
            // changeGetter_bw
            // 
            this.changeGetter_bw.WorkerReportsProgress = true;
            this.changeGetter_bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.changeGetter_bw_DoWork);
            // 
            // POETradeIndexerService
            // 
            this.ServiceName = "POEIndexerService";

        }

        #endregion

        private System.ComponentModel.BackgroundWorker main_bgw;
        private System.ComponentModel.BackgroundWorker changeGetter_bw;
    }
}
