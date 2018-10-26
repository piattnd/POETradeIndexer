using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;

namespace POETradeIndexer
{
    public class poe_requirements
    {
        public string name { get; set; }
        public List<String[]> values { get; set; }
        public int displayMode { get; set; }
        public int type { get; set; }
        public int progress { get; set; }
        public long itemId { get; set; }
        public int requirementValue { get; set; }
        public long requirementTypeId { get; set; }
        public bool isAdditionalRequirement { get; set; }
        public string requirementValueString { get; set; }
        public bool isNextLevelRequirement { get; set; }

        public void addRequirement()
        {
            string symbol = "";
            if (values.Count > 0)
            {
                symbol = parseRequirementSymbol(values[0][0]);
                this.requirementValueString = values[0][0];
                this.requirementValue = stripNonNumeric(values[0][0]);
            }
            poe_requirement_type myType = new poe_requirement_type(this.name, symbol);
            myType.addRequirementType();
            this.requirementTypeId = myType.id;
            insert();
        }

        private void insert()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "INSERT INTO POE_REQUIREMENT (ITEM_ID, REQUIREMENT_TYPE_ID, DISPLAY_MODE, " +
                    "TYPE, PROGRESS, IS_ADDITIONAL_REQUIREMENT, REQUIREMENT_VALUE, REQUIREMENT_VALUE_STRING, " +
                    " IS_NEXT_LEVEL_REQUIREMENT) VALUES (@ITEMID, @REQUIREMENTTYPEID, @DISPLAY_MODE, @TYPE, @PROGRESS," +
                    " @ISADDITIONALREQUIREMENT, @REQUIREMENTVALUE, @REQUIREMENTVALUESTRING, @ISNEXTLEVELREQUIREMENT)";

                cmd.Parameters.AddWithValue("@ITEMID", this.itemId);
                cmd.Parameters.AddWithValue("@REQUIREMENTTYPEID", this.requirementTypeId);
                cmd.Parameters.AddWithValue("@DISPLAY_MODE", this.displayMode);
                cmd.Parameters.AddWithValue("@TYPE", this.type);
                cmd.Parameters.AddWithValue("@PROGRESS", this.progress);
                if (isAdditionalRequirement)
                    cmd.Parameters.AddWithValue("@ISADDITIONALREQUIREMENT", 1);
                else
                    cmd.Parameters.AddWithValue("@ISADDITIONALREQUIREMENT", 0);
                cmd.Parameters.AddWithValue("@REQUIREMENTVALUE", requirementValue);
                cmd.Parameters.AddWithValue("@REQUIREMENTVALUESTRING", requirementValueString);
                if (isNextLevelRequirement)
                    cmd.Parameters.AddWithValue("@ISNEXTLEVELREQUIREMENT", 1);
                else
                    cmd.Parameters.AddWithValue("@ISNEXTLEVELREQUIREMENT", 0);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                myConn.close();
            }
        }

        private string parseRequirementSymbol(string value)
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

        private int stripNonNumeric(string input)
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
    }
}
