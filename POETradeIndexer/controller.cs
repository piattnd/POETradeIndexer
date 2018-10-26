using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using POETradeIndexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace POETradeIndexer
{
    public class Controller
    {
        // this is the main class for controlling operations.
        private bool keepRunning = true;
        private int maxThreads = 5;
        private int threadStartDelay = 4000;
        private long changeId;

        BackgroundWorker myBw;
        ArrayList changeList = new ArrayList();

        public void stopRunning()
        {
            this.keepRunning = false;
            poe_logger.logInfo("Told to stop running");
        }

        public void startRunning()
        {
            this.keepRunning = true;
            poe_logger.logInfo("Told to start running");
        }

        public Controller(BackgroundWorker theBw)
        {
            this.myBw = theBw;
        }

        public void processPoeUpdate()
        {
            poe_logger.logInfo("Starting up");
            while (keepRunning)
            {
                bool fullDelay = false;

                if (changeList.Count < 5)
                {
                    poe_logger.logInfo("Getting Json");
                    string[] getJsonVals = getNextJson();
                    string nextJson = getJsonVals[0];
                    string currentChangeId = getJsonVals[1];
                    
                    //poe_logger.logInfo("Start processing next change: " + nextUpdateId);
                    if (nextJson != "")
                    {
                        //poe_logger.logInfo(nextJson.Substring(0,100));
                        Console.WriteLine(nextJson.Substring(0, 100));
                        poe_change myChange = JsonConvert.DeserializeObject<poe_change>(nextJson);
                        //lock (changeList.SyncRoot) { changeList.Add(currentChangeId); }
                        myChange.changeId = changeId;
                        myChange.currentChangeId = currentChangeId;
                        //myChange.Process();
                        Thread myThread = new Thread(new ThreadStart(myChange.Process));
                        myThread.Start();
                        lock (changeList.SyncRoot) { changeList.Add(myThread); }
                        myChange.changeProcessingFinished += changeFinishedProcessing;
                        nextJson = "";
                        fullDelay = true;
                    }
                    else
                    {
                        //Console.WriteLine("No json found, delaying 1000 ms and retrying");
                        //Thread.Sleep(1000);
                        //processPoeUpdate();
                        fullDelay = false;
                    }
                }
                else
                {
                    //poe_logger.logInfo("Max number of threads started");
                    Console.WriteLine("Max number of threads started");
                    ArrayList removeThreads = new ArrayList();
                    lock (changeList.SyncRoot)
                    {
                        foreach (Thread theThread in changeList)
                        {
                            if (theThread.ThreadState == ThreadState.Stopped)
                                removeThreads.Add(theThread);
                            //Console.WriteLine("Thread: " + theThread.ManagedThreadId + " is in state: " + theThread.ThreadState);
                        }
                    }

                    foreach (Thread aThread in removeThreads)
                    {
                        lock (changeList.SyncRoot)
                        {
                            changeList.Remove(aThread);
                        }
                    }
                }
                if (fullDelay)
                    Thread.Sleep(poe_throttler.calcChangeProcessDelay());
                else
                    Thread.Sleep(1000);
            }
        }

        private void processStash(poe_stash myStash)
        {
            if (myStash.Public)
            {
                myStash.addStash();
                myBw.ReportProgress(0, "Stash ID: " + myStash.id);
            }
        }

        private string[] getNextJson()
        {
            int retries = 5;
            int currentTry = 1;
            string ret = "";
            string ret2 = "";
            string ret3 = "";

            MySqlConnection myConn = new MySqlConnection();
            while (myConn.State != System.Data.ConnectionState.Open && currentTry < retries)
            {
                myConn.ConnectionString = "server=127.0.0.1;uid=root;pwd=Fc)3NATE;database=poe_trader;MaximumPoolsize=500;";
                try
                {
                    myConn.Open();
                }
                catch (MySqlException ex)
                {
                    poe_logger.logError(ex.StackTrace);
                    //throw ex;
                }
                currentTry++;
            }

            if (myConn.State == System.Data.ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = myConn;
                cmd.CommandText = "SELECT JSON_DATA,NEXT_CHANGE_ID, ID FROM POE_CHANGE WHERE ID = (SELECT MAX(id) FROM POE_CHANGE WHERE PROCESSED=0 AND JSON_DATA_RETRIEVED=1 AND IN_PROCESS=0);";
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        ret = reader.GetString(0);
                        ret2 = reader.GetString(1);
                        this.changeId = reader.GetInt64(2);
                        //ret3 = reader.GetInt64(2).ToString();
                    }
                }
                cmd.Dispose();
                reader.Close();
                reader.Dispose();
                myConn.Close();
                try
                {
                    MySqlConnection.ClearPool(myConn);
                }
                catch (Exception ex)
                {
                    poe_logger.logError("Exception caught while removing from MySQL Pool: " + ex.StackTrace);
                }
                //myConn.Dispose();
            }

            String[] val = new String[3] { ret, ret2, ret3 };
            return val;
        }

        public void changeFinishedProcessing(object sender, ChangeProcessingFinishedEventArgs e)
        {
            //e.theChange.insert();
            poe_logger.logInfo("Finished processing change ID: " + e.theChange.next_change_id);
            Console.WriteLine("Finished processing change ID: " + e.theChange.next_change_id);
            lock (changeList.SyncRoot) { changeList.Remove(e.theThread); }

            e.theChange.Dispose();
            //e.theThread.Abort();
            //Console.WriteLine("Thread Count: " + threadList.Count);

            //if (keepRunning)
                //processPoeUpdate();
        }
    }

}
