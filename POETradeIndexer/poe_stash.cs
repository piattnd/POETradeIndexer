using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Threading;

namespace POETradeIndexer
{
    public class poe_stash:IDisposable
    {
        private bool disposed = false;
        public string accountName { get; set; }
        public string lastCharacterName { get; set; }
        public string id { get; set; }
        public string stash { get; set; }
        public string stashType { get; set; }
        public List<poe_item> items { get; set; }
        public bool Public { get; set; }
        public long accountId { get; set; }
        public long stashId { get; set; }
        public DateTime lastItemAdded { get; set; }

        public long changeId { get; set; }

        public void addStash()
        {
            //Console.WriteLine("Processing stash: " + this.stash);
            this.addAccount();

            if (stashExists())
            {
                // update the stash name
                updateStashName();
            }
            else
            {
                DBConnector myConn = new DBConnector();
                myConn.connect();

                if (myConn.isConnected())
                {
                    MySqlCommand cmd = myConn.getSqlCommand();
                    cmd.CommandText = "INSERT INTO POE_STASH (ID,STASH_NAME,STASH_TYPE,ACCOUNT_ID) " +
                        "VALUES (@STASHID, @STASHNAME, @STASHTYPE, @ACCOUNTID)";

                    cmd.Parameters.AddWithValue("@STASHID", this.id);
                    cmd.Parameters.AddWithValue("@STASHNAME", this.stash);
                    cmd.Parameters.AddWithValue("@STASHTYPE", this.stashType);
                    cmd.Parameters.AddWithValue("@ACCOUNTID", this.accountId);
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    this.stashId = cmd.LastInsertedId;
                    cmd.Dispose();
                    myConn.close();
                }
            }
            foreach (poe_item myItem in items)
            {
                //Console.WriteLine("Adding item: " + myItem.id);
                if (this.changeId == 0)
                {
                    Console.WriteLine("It was 0");
                }
                myItem.changeId = this.changeId;
                myItem.uniqueStashId = this.stashId;
                myItem.addItem();
                //Console.WriteLine("Finished adding item: " + myItem.id);
            }
            StashProcessingFinishedEventArgs a = new StashProcessingFinishedEventArgs();
            a.theStash = this;
            //Console.WriteLine("Triggering stash complete event for stash: " + stash);
            OnStashProcessingFinished(a);
        }

        public void getAccountByName(string accountName)
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT ID, ACCOUNT_NAME, LAST_CHAR_NAME, LAST_ITEM_ADDED FROM POE_ACCOUNT WHERE ID=@ACCOUNTNAME";

                cmd.Parameters.AddWithValue("@ACCOUNTNAME", this.accountName);
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.accountId = reader.GetInt64(0);
                    this.accountName = reader.GetString(1);
                    this.lastCharacterName = reader.GetString(2);
                    this.lastItemAdded = reader.GetDateTime(3);
                }
                myConn.close();
            }
        }

        public bool stashExists()
        {
            bool ret = false;
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT STASH_UNIQUE_ID FROM POE_STASH WHERE ID=@STASHID";

                cmd.Parameters.AddWithValue("@STASHID", this.id);
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.stashId = reader.GetInt64(0);
                    ret = true;
                }
                myConn.close();

            }
            return ret;
        }

        public void updateStashName()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "UPDATE POE_STASH SET STASH_NAME=@STASHNAME WHERE ID=@STASHID";

                cmd.Parameters.AddWithValue("@STASHNAME", this.stash);
                cmd.Parameters.AddWithValue("@STASHID", this.id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                myConn.close();
            }
        }

        private void addAccount()
        {
            if (accountExists())
                updateLastChar();
            else
            {
                DBConnector myConn = new DBConnector();
                myConn.connect();

                if (myConn.isConnected())
                {
                    MySqlCommand cmd = myConn.getSqlCommand();
                    cmd.CommandText = "INSERT INTO POE_ACCOUNT (ACCOUNT_NAME,LAST_CHAR_NAME,LAST_ITEM_ADDED) " +
                        "VALUES (@ACCOUNTNAME, @LASTCHAR, NOW())";

                    cmd.Parameters.AddWithValue("@ACCOUNTNAME", this.accountName);
                    cmd.Parameters.AddWithValue("@LASTCHAR", this.lastCharacterName);
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    this.accountId = cmd.LastInsertedId;
                    cmd.Dispose();
                    myConn.close();
                }
            }
        }

        public bool accountExists()
        {
            bool ret = false;
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT ID FROM POE_ACCOUNT WHERE ACCOUNT_NAME=@ACCOUNTNAME";

                cmd.Parameters.AddWithValue("@ACCOUNTNAME", this.accountName);
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.accountId = reader.GetInt64(0);
                    ret = true;
                }
                myConn.close();
            }
            return ret;
        }

        public void updateLastChar()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "UPDATE POE_ACCOUNT SET LAST_CHAR_NAME=@LASTCHAR WHERE ACCOUNT_NAME=@ACCOUNTNAME";

                cmd.Parameters.AddWithValue("@ACCOUNTNAME", this.accountName);
                cmd.Parameters.AddWithValue("@LASTCHAR", this.lastCharacterName);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                myConn.close();
            }
        }

        protected virtual void OnStashProcessingFinished(StashProcessingFinishedEventArgs e)
        {
            EventHandler<StashProcessingFinishedEventArgs> handler = stashProcessingFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<StashProcessingFinishedEventArgs> stashProcessingFinished;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.accountName = null;
                this.lastCharacterName = null;
                this.id = null;
                this.stash = null;
                this.stashType = null;
                this.items.Clear();
                this.items = null;
                this.accountId = 0;
                this.stashId = 0;
                disposed = true;
            }
        }
    }

    public class StashProcessingFinishedEventArgs : EventArgs
    {
        public poe_stash theStash { get; set; }
    }
}
