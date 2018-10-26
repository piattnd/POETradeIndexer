using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace POETradeIndexer
{
    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }

    public class poe_change_getter
    {
        public Boolean keepRunning = true;
        private string nextUpdateId = "";
        public poe_change_getter()
        {
            
        }

        public void start()
        {
            getNextChangeData();
        }

        public void stopRunning()
        {
            this.keepRunning = false;
        }

        private void getNextChangeData()
        {
            while (keepRunning)
            {
                DateTime startTime = DateTime.Now;
                this.nextUpdateId = getNextChangeID();
                string result = "";

                using (MyWebClient client = new MyWebClient())
                {
                    
                    if (nextUpdateId != "")
                    {
                        Console.WriteLine(DateTime.Now + "--Fetching next update");
                        string url = "http://www.pathofexile.com/api/public-stash-tabs?id=" + nextUpdateId;
                        result = client.DownloadString(url);
                        //Console.WriteLine(DateTime.Now + "-- Got update");
                    }
                    else
                        result = client.DownloadString("http://www.pathofexile.com/api/public-stash-tabs");
                }
                if (result.StartsWith("{\"error\":{\"message\""))
                {
                    poe_logger.logError("Got an error with the JSON data: " + result);
                }
                else
                {
                    //Console.WriteLine(DateTime.Now + "--calling change insert.");
                    insertNextChangeData(result);
                    //Console.WriteLine(DateTime.Now + "--Insert finished.");
                }
                DateTime endTime = DateTime.Now;

                int delay = 0;
                var diffInSec = (endTime - startTime).TotalSeconds;
                if (diffInSec >1)
                {
                    // we had a long get, set to 10 ms
                    delay = 10;
                }
                else
                {
                    // short get, set to 1000ms
                    delay = 1000;
                }
                result = "";
                Thread.Sleep(delay);
            }
        }

        private void insertNextChangeData(string jsonData)
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();
            poe_change nextChange = JsonConvert.DeserializeObject<poe_change>(jsonData);

            if (myConn.isConnected())
            {
                //Console.WriteLine(DateTime.Now + "--Beginning JSON Insert.");
                MySqlCommand cmd = myConn.getSqlCommand();
                // first update the previous entry to include the json data.
                cmd.CommandText = "UPDATE POE_CHANGE SET JSON_DATA=@JSONDATA, JSON_DATA_RETRIEVED=1, IN_PROCESS=0 WHERE NEXT_CHANGE_ID=@NEXTCHANGEID;";
                cmd.Parameters.AddWithValue("@JSONDATA", jsonData);
                cmd.Parameters.AddWithValue("@NEXTCHANGEID", this.nextUpdateId);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                //Console.WriteLine(DateTime.Now + "--Json Insert Complete.");
                cmd.Dispose();

                // now insert the next change ID.
                MySqlCommand cmd2 = myConn.getSqlCommand();
                cmd2.CommandText = "INSERT INTO POE_CHANGE (NEXT_CHANGE_ID, CHANGE_RECEIVED, PROCESSED, JSON_DATA_RETRIEVED) " +
                    "VALUES (@NEXTCHANGEID, @CHANGERECEIVED, 0, 0);";
                cmd2.Parameters.AddWithValue("@NEXTCHANGEID", nextChange.next_change_id);
                cmd2.Parameters.AddWithValue("@CHANGERECEIVED", DateTime.Now);
                cmd2.Prepare();

                cmd2.ExecuteNonQuery();
                //Console.WriteLine(DateTime.Now + "--Insert new change done");
                cmd2.Dispose();
                myConn.close();
            }
            nextChange.Dispose();

        }

        private string getNextChangeID()
        {
            string ret = "";

            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT NEXT_CHANGE_ID FROM POE_CHANGE WHERE ID = (SELECT MAX(id) FROM POE_CHANGE);";
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        ret = reader.GetString(0);
                    }
                }
                myConn.close();
            }
            return ret;
        }
    }
}
