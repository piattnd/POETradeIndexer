using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeIndexer
{
    public class poe_property_type
    {
        public long id { get; set; }
        public string name { get; set; }
        public string typeSymbol { get; set; }

        public poe_property_type(string name, string typeSymbol)
        {
            this.name = name;
            this.typeSymbol = typeSymbol;
        }

        public void addPropertyType()
        {
            if (!exists())
            {
                insert();
            }
        }

        private void insert()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "INSERT INTO POE_PROPERTY_TYPE (NAME, TYPE_SYMBOL) " +
                    "VALUES (@NAME, @TYPESYMBOL)";

                cmd.Parameters.AddWithValue("@NAME", this.name);
                cmd.Parameters.AddWithValue("@TYPESYMBOL", this.typeSymbol);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT LAST_INSERT_ID()";
                cmd.Prepare();
                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.id = reader.GetInt64(0);
                }

                myConn.close();
            }
        }

        private bool exists()
        {
            bool ret = false;

            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT ID FROM POE_PROPERTY_TYPE WHERE NAME=@NAME";

                cmd.Parameters.AddWithValue("@NAME", this.name);

                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.id = reader.GetInt64(0);
                    ret = true;
                }
                myConn.close();
            }

            return ret;
        }
    }
}
