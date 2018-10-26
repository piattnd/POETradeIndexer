namespace POEChangeGetService
{
    partial class POEChangeGetService
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
            this.changeGetter_bw = new System.ComponentModel.BackgroundWorker();
            // 
            // changeGetter_bw
            // 
            this.changeGetter_bw.WorkerReportsProgress = true;
            this.changeGetter_bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.changeGetter_bw_DoWork);
            this.changeGetter_bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.changeGetter_bw_ProgressChanged);
            this.changeGetter_bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.changeGetter_bw_RunWorkerCompleted);
            // 
            // POEChangeGetService
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.ComponentModel.BackgroundWorker changeGetter_bw;
    }
}
