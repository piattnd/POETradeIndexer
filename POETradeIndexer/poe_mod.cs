using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeIndexer
{
    public class poe_mod
    {
        public string[] modText { get; set; }
        public long itemId { get; set; }
        public int modType { get; set; }

        public poe_mod()
        {

        }

        public void process()
        {
            for(int i = 0; i<modText.Length; i++)
            {
                string theText = modText[i];
                insert(theText);
            }
        }

        private void insert(string modTextString)
        {
            using (MySqlConnection myConn = new MySqlConnection("server=127.0.0.1;uid=root;pwd=Fc)3NATE;database=poe_trader;"))
            using (MySqlCommand cmd = myConn.CreateCommand())
            {
                myConn.Open();
                cmd.CommandText = "INSERT INTO POE_MODS (ITEM_ID, MOD_TEXT, MOD_TYPE) " +
                    "VALUES (@ITEMID, @MODTEXT, @MODTYPE)";

                cmd.Parameters.AddWithValue("@ITEMID", this.itemId);
                cmd.Parameters.AddWithValue("@MODTEXT", modTextString);
                cmd.Parameters.AddWithValue("@MODTYPE", this.modType);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
