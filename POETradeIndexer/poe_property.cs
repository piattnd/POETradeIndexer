using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;

namespace POETradeIndexer
{
    public class poe_property
    {
        public string name { get; set; }
        public List<string[]> values { get; set; }
        public int displayMode { get; set; }
        public int type { get; set; }
        public decimal progress { get; set; }
        public long itemId { get; set; }
        public bool isAdditionalProperty { get; set; }
        public int propertyValue { get; set; }
        public string propertyValueString { get; set; }
        public long propertyTypeId { get; set; }

        public void addProperty()
        {
            string symbol = "";
            if (values.Count > 0)
            {
                //symbol = parsePropertySymbol(values[0][0]);
                this.propertyValueString = values[0][0];
                //this.propertyValue = stripNonNumeric(values[0][0]);
            }
            //poe_property_type myType = new poe_property_type(this.name, symbol);
            //myType.addPropertyType();
            //this.propertyTypeId = myType.id;
            insert();
        }

        private void insert()
        {
            if (this.itemId == 0)
            {
                Console.WriteLine("No itemId was found");
            }
            else
            {
                using (MySqlConnection myConn = new MySqlConnection("server=127.0.0.1;uid=root;pwd=Fc)3NATE;database=poe_trader;"))
                using (MySqlCommand cmd = myConn.CreateCommand())
                {
                    myConn.Open();

                    cmd.CommandText = "INSERT INTO POE_PROPERTY (ITEM_ID, PROPERTY_NAME, DISPLAY_MODE, " +
                        "TYPE, PROGRESS, IS_ADDITIONAL_PROPERTY, PROPERTY_VALUE_STRING) " +
                        "VALUES (@ITEMID, @PROPERTYNAME, @DISPLAY_MODE, @TYPE, @PROGRESS," +
                        " @ISADDITIONALPROPERTY, @PROPERTYVALUESTRING)";

                    cmd.Parameters.AddWithValue("@ITEMID", this.itemId);
                    cmd.Parameters.AddWithValue("@PROPERTYNAME", this.name);
                    cmd.Parameters.AddWithValue("@DISPLAY_MODE", this.displayMode);
                    cmd.Parameters.AddWithValue("@TYPE", this.type);
                    cmd.Parameters.AddWithValue("@PROGRESS", this.progress);
                    if (isAdditionalProperty)
                        cmd.Parameters.AddWithValue("@ISADDITIONALPROPERTY", 1);
                    else
                        cmd.Parameters.AddWithValue("@ISADDITIONALPROPERTY", 0);
                    //cmd.Parameters.AddWithValue("@PROPERTYVALUE", propertyValue);
                    cmd.Parameters.AddWithValue("@PROPERTYVALUESTRING", propertyValueString);
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int stripNonNumeric(string input)
        {
            if (!(input.Contains("/")))
            {
                if (input.Any(c => char.IsDigit(c)))
                {
                    try
                    {
                        return int.Parse(new string(input.Where(c => char.IsDigit(c)).ToArray()));
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        private string parsePropertySymbol(string value)
        {
            bool isNum = false;
            int pos = 0;
            bool intOnly = false;
            // check if we have just a number
            try
            {
                int.Parse(value);
                intOnly = true;
            }
            catch (Exception ex)
            {
                intOnly = false;
            }

            if (intOnly)
                return "";

            // check to make sure there are numbers in our value
            bool checkInt = value.Any(c => char.IsDigit(c));
            if (checkInt)
            {
                while (!isNum && pos < value.Length)
                {
                    string test = value.Substring(value.Length - pos - 1, 1);
                    try
                    {
                        long.Parse(test);
                        isNum = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        // nothing, it's not a number
                    }
                    pos++;
                }
                if (pos < 1)
                    return "";
                else
                    return value.Substring(value.Length - pos, pos);
            }
            else
                return "";
        }
    }
}
