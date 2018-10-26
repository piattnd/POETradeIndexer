using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeIndexer
{
    public class poe_throttler
    {

        public static int calcChangeProcessDelay()
        {
            // this method will calculate the appropriate delay before starting another change process thread.
            // we will evaluate the length of time it took for the most recent change to be processed
            int theVal = 0;
            int returnVal = 0;

            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();

                cmd.CommandText = "SELECT TIME_TO_SEC(TIMEDIFF(CHANGE_PROCESSED,PROCESS_START)) FROM POE_CHANGE WHERE ID IN ( SELECT MAX(ID) FROM POE_CHANGE WHERE JSON_DATA_RETRIEVED = 1 AND PROCESSED = 1); ";
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    theVal = reader.GetInt32(0);
                }
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                myConn.close();
            }

            // if it's only taking 5 seconds or less, set the delay to 1000 ms
            if (theVal <= 3)
            {
                returnVal = 1000;
            }
            else if (theVal > 3 && theVal <= 6)
            {
                returnVal = 2000;
            }
            else if (theVal > 6 && theVal <= 15)
            {
                returnVal = 3000;
            }
            else if (theVal > 15 && theVal <= 20)
            {
                returnVal = 4000;
            }
            else
            {
                returnVal = 5000;
            }
            return returnVal;
        }        
    }
}
