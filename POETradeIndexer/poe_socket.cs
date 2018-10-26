using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeIndexer
{
    public class poe_socket
    {
        public int group { get; set; }
        public string attr { get; set; }
        public long itemId { get; set; }

        public void addSocket()
        {
            insert();
        }

        private void insert()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "INSERT INTO POE_SOCKET (ITEM_ID, GROUP_ID, ATTRIBUTE) " +
                    "VALUES (@ITEMID, @GROUPID, @ATTRIBUTE)";

                cmd.Parameters.AddWithValue("@ITEMID", this.itemId);
                cmd.Parameters.AddWithValue("@GROUPID", this.group);
                cmd.Parameters.AddWithValue("@ATTRIBUTE", this.attr);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                myConn.close();
            }
        }
    }
}
