using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System.Threading;

namespace POETradeIndexer
{
    public class poe_change : IDisposable
    {
        public string next_change_id { get; set; }
        public List<poe_stash> stashes { get; set; }
        public long changeId { get; set; }
        private bool keepRunning = true;
        bool disposed = false;
        bool doneQueuing = false;
        private ArrayList stashList = new ArrayList();
        public string currentChangeId { get; set; }
        private int processedStashes = 0;
        private int ignoredStashes = 0;
        public static poe_logger myLogger = new poe_logger();

        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Stop()
        {
            keepRunning = false;
        }

        public void Process()
        {
            inProgress();
            Console.WriteLine("starting processing change: " + this.next_change_id);
            poe_logger.logInfo("Start processing change: " + this.next_change_id);
            if (this.stashes.Count == 0)
                poe_logger.logError("The stash count was 0");

            TaskScheduler myScheduler = TaskScheduler.Default;
            TaskFactory factory = new TaskFactory(myScheduler);
            CancellationTokenSource cts = new CancellationTokenSource();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < this.stashes.Count; i++)
            {
                poe_stash myStash = this.stashes[i];
                if (myStash.Public && myStash.stashType=="PremiumStash")
                {
                    Task t = factory.StartNew(() =>
                    {
                        myStash.changeId = this.changeId;
                        myStash.addStash();
                    }, cts.Token);
                    tasks.Add(t);
                    /*processedStashes++;
                    myStash.changeId = this.changeId;
                    lock (stashList.SyncRoot) { stashList.Add(myStash.id); }
                    if (!stashList.Contains(myStash.id))
                        //poe_logger.logError("Failure to add to stash list: " + myStash.id);
                        Console.WriteLine("Failure to add to stash list: " + myStash.id);
                    Thread stashThread = new Thread(new ThreadStart(myStash.addStash));
                    myStash.stashProcessingFinished += stashFinishedProcessing;
                    //lock (stashList.SyncRoot) { stashList.Add(stashThread); }
                    stashThread.Start();
                    //myStash.addStash();*/
                }
                /*if (!keepRunning)
                {
                    break;
                }*/
                //Thread.Sleep(10);
            }

            Task.WaitAll(tasks.ToArray());
            cts.Dispose();

            doneQueuing = true;
            ChangeProcessingFinishedEventArgs a = new ChangeProcessingFinishedEventArgs();
            a.theThread = Thread.CurrentThread;
            a.theChange = this;
            poe_logger.logInfo("Calling Change Process Finished");
            complete();
            OnChangeProcessingFinished(a);
        }

        public void complete()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();

                cmd.CommandText = "UPDATE POE_CHANGE SET PROCESSED=1, STASH_COUNT=@STASHCOUNT, CHANGE_PROCESSED=@CHANGEPROCESSED, JSON_DATA=null WHERE NEXT_CHANGE_ID=@NEXTCHANGEID;";

                cmd.Parameters.AddWithValue("@NEXTCHANGEID", this.currentChangeId);
                cmd.Parameters.AddWithValue("@STASHCOUNT", this.stashes.Count);
                cmd.Parameters.AddWithValue("@CHANGEPROCESSED", DateTime.Now);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                myConn.close();
            }
        }

        public void inProgress()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();

                cmd.CommandText = "UPDATE POE_CHANGE SET IN_PROCESS=1, PROCESS_START=@PROCESSSTART WHERE NEXT_CHANGE_ID=@NEXTCHANGEID;";

                cmd.Parameters.AddWithValue("@NEXTCHANGEID", this.currentChangeId);
                cmd.Parameters.AddWithValue("@PROCESSSTART", DateTime.Now);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        public void stashFinishedProcessing(object sender, StashProcessingFinishedEventArgs e)
        {
            ArrayList tempStashList = ArrayList.Synchronized(stashList);

            if (!tempStashList.Contains(e.theStash.id))
            {
                poe_logger.logError("The ID that ended did not exist in the array list, retrying...: " + e.theStash.id);
                for (int x=0;x <= 5;x++)
                {
                    if (stashList.Contains(e.theStash.id))
                        break;
                    else
                        poe_logger.logError("Still don't see ID in arraylist");
                }
            }
            lock (stashList.SyncRoot) { stashList.Remove(e.theStash.id); }
            if (stashList.Count == 0 && doneQueuing)
            {
                ChangeProcessingFinishedEventArgs a = new ChangeProcessingFinishedEventArgs();
                a.theThread = Thread.CurrentThread;
                a.theChange = this;
                poe_logger.logInfo("Calling Change Process Finished");
                complete();
                OnChangeProcessingFinished(a);
            }
            else
                ignoredStashes++;
            e.theStash.Dispose();
        }


        protected virtual void OnChangeProcessingFinished(ChangeProcessingFinishedEventArgs e)
        {
            EventHandler<ChangeProcessingFinishedEventArgs> handler = changeProcessingFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ChangeProcessingFinishedEventArgs> changeProcessingFinished;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            stashes.Clear();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class ChangeProcessingFinishedEventArgs : EventArgs
    {
        public Thread theThread { get; set; }
        public poe_change theChange { get; set; }
    }
}
