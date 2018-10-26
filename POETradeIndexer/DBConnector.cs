using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace POETradeIndexer
{
    public class DBConnector:IDisposable
    {
        private MySqlConnection myConn;
        private bool disposed = false;
        private int retries = 5;
        private int currentTry = 1;
        public MySqlDataReader reader { get; private set; }
        public MySqlCommand cmd { get; private set; }

        public DBConnector()
        {
            
        }

        public void connect()
        {
            myConn = new MySqlConnection();
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
        }

        public bool isConnected()
        {
            if (myConn.State == System.Data.ConnectionState.Open)
                return true;
            else
                return false;
        }

        public MySqlCommand getSqlCommand()
        {
            if (this.myConn.State == System.Data.ConnectionState.Closed)
            {
                throw new Exception("Database Connection is not open.");
            }
            this.cmd = new MySqlCommand();
            this.cmd.Connection = myConn;
            return this.cmd;
        }

        public MySqlDataReader executeQuery(MySqlCommand cmd)
        {
            if (cmd.Connection.State == System.Data.ConnectionState.Closed)
            {
                throw new Exception("Database Connection is not open.");
            }

            try
            {
                this.reader = cmd.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return this.reader;
        }

        public void close()
        {
            if (this.myConn.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    this.myConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                //this.Dispose();
            }
        }

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
                if (this.reader != null)
                {
                    if (!this.reader.IsClosed)
                        this.reader.Close();
                    this.reader.Dispose();
                }
                if (this.cmd != null)
                    this.cmd.Dispose();
                //MySqlConnection.ClearPool(this.myConn);
                this.myConn.Dispose();
                disposed = true;
            }
        }
    }
}
