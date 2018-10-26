namespace POEIndexerTester
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.stop_btn = new System.Windows.Forms.Button();
            this.status_lbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lastUpdate_lbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.start_btn = new System.Windows.Forms.Button();
            this.main_bgw = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // stop_btn
            // 
            this.stop_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop_btn.Location = new System.Drawing.Point(832, 12);
            this.stop_btn.Name = "stop_btn";
            this.stop_btn.Size = new System.Drawing.Size(93, 45);
            this.stop_btn.TabIndex = 11;
            this.stop_btn.Text = "Stop Process";
            this.stop_btn.UseVisualStyleBackColor = true;
            this.stop_btn.Click += new System.EventHandler(this.stop_btn_Click);
            // 
            // status_lbl
            // 
            this.status_lbl.Location = new System.Drawing.Point(73, 71);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(852, 20);
            this.status_lbl.TabIndex = 10;
            this.status_lbl.Text = "Stopped";
            this.status_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Status:";
            // 
            // lastUpdate_lbl
            // 
            this.lastUpdate_lbl.Location = new System.Drawing.Point(106, 114);
            this.lastUpdate_lbl.Name = "lastUpdate_lbl";
            this.lastUpdate_lbl.Size = new System.Drawing.Size(819, 23);
            this.lastUpdate_lbl.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Last Update:";
            // 
            // start_btn
            // 
            this.start_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_btn.Location = new System.Drawing.Point(12, 12);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(93, 45);
            this.start_btn.TabIndex = 6;
            this.start_btn.Text = "Start Process";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // main_bgw
            // 
            this.main_bgw.WorkerReportsProgress = true;
            this.main_bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.main_bgw_DoWork);
            this.main_bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.main_bgw_ProgressChanged);
            this.main_bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.main_bgw_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 151);
            this.Controls.Add(this.stop_btn);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lastUpdate_lbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.start_btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stop_btn;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lastUpdate_lbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start_btn;
        private System.ComponentModel.BackgroundWorker main_bgw;
    }
}

