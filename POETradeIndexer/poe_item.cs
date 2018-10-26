using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;

namespace POETradeIndexer
{
    public class poe_item
    {
        #region class level variables
        public Boolean verified { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int ilvl { get; set; }
        public string icon { get; set; }
        public string league { get; set; }
        public string id { get; set; }
        public List<poe_socket> sockets { get; set; }
        public string name { get; set; }
        public string typeLine { get; set; }
        public Boolean identified { get; set; }
        public Boolean corrupted { get; set; }
        public Boolean lockedToChar { get; set; }
        public string note { get; set; }
        public List<poe_property> properties { get; set; }
        public List<poe_requirements> requirements { get; set; }
        public string[] explicitMods { get; set; }
        public string[] implicitMods { get; set; }
        public string[] enchantMods { get; set; }
        public string[] craftedMods { get; set; }
        public string[] flavourText { get; set; }
        public int frameType { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string inventoryId { get; set; }
        public List<poe_item> socketedItems { get; set; }
        public List<poe_property> additionalProperties { get; set; }
        public string secDescriptionText { get; set; }
        public string descrText { get; set; }
        public string artFileName { get; set; }
        public bool duplicated { get; set; }
        public int maxStackSize { get; set; }
        public List<poe_requirements> nextLevelRequirements { get; set; }
        public int stackSize { get; set; }
        public int talismanTier { get; set; }
        public string[] utilityModes { get; set; }
        public bool support { get; set; }
        public string[] cosmeticMods { get; set; }
        public string prophecyDiffText { get; set; }
        public string prophecyText { get; set; }
        public Boolean isRelic { get; set; }
        public long uniqueId { get; set; }
        public long uniqueStashId { get; set; }
        public long socketedToItem { get; set; }
        public DateTime added_ts { get; set; }
        public string concatFlavourText { get; set; }
        public long changeId { get; set; }
        #endregion

        public void addItem()
        {
            if (itemExists())
            {
                resetAssociatedSocketedItems();
                resetAssociatedProperties();
                resetAssociatedRequirements();
                resetAssociatedSockets();
                resetAssociatedMods();
                updateItem();
            }
            else
            {
                insertItem();
            }

            for (int i = 0; i<sockets.Count; i++)
            {
                poe_socket mySocket = sockets[i];
                mySocket.itemId = this.uniqueId;
                mySocket.addSocket();
            }

            if (properties != null)
            {
                for (int i=0; i<properties.Count; i++)
                {
                    poe_property myProperty = properties[i];
                    myProperty.itemId = this.uniqueId;
                    myProperty.addProperty();
                }
            }

            if (requirements != null)
            {
                foreach (poe_requirements myRequirement in requirements)
                {
                    myRequirement.itemId = this.uniqueId;
                    myRequirement.isAdditionalRequirement = false;
                    myRequirement.isNextLevelRequirement = false;
                    myRequirement.addRequirement();
                }
            }

            if (socketedItems != null)
            {
                foreach (poe_item mySocketedItem in socketedItems)
                {
                    mySocketedItem.uniqueStashId = this.uniqueStashId;
                    mySocketedItem.changeId = this.changeId;
                    mySocketedItem.socketedToItem = this.uniqueId;
                    mySocketedItem.addItem();
                }
            }

            if (additionalProperties != null)
            {
                foreach (poe_property myAdditionalProperty in additionalProperties)
                {
                    myAdditionalProperty.itemId = this.uniqueId;
                    myAdditionalProperty.isAdditionalProperty = true;
                    myAdditionalProperty.addProperty();
                }
            }

            if (nextLevelRequirements != null)
            {
                foreach (poe_requirements myNextLevelRequirement in nextLevelRequirements)
                {
                    myNextLevelRequirement.isAdditionalRequirement = false;
                    myNextLevelRequirement.isNextLevelRequirement = true;
                }
            }
                
            if (explicitMods != null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.EXPLICIT;
                theMod.modText = explicitMods;
                theMod.process();
            }

            if (implicitMods !=null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.IMPLICIT;
                theMod.modText = implicitMods;
                theMod.process();
            }

            if (enchantMods != null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.ENCHANT;
                theMod.modText = enchantMods;
                theMod.process();
            }

            if (craftedMods != null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.CRAFTED;
                theMod.modText = craftedMods;
                theMod.process();
            }

            if (utilityModes != null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.UTILITY;
                theMod.modText = utilityModes;
                theMod.process();
            }

            if (cosmeticMods != null)
            {
                poe_mod theMod = new poe_mod();
                theMod.itemId = this.uniqueId;
                theMod.modType = (int)poe_enums.modType.COSMETIC;
                theMod.modText = cosmeticMods;
                theMod.process();
            }
                
            if (flavourText != null)
            {
                this.concatFlavourText = String.Join(" ", flavourText);
            }
        }

        private void insertItem()
        {
            using (MySqlConnection myConn = new MySqlConnection("server=127.0.0.1;uid=root;pwd=Fc)3NATE;database=poe_trader;"))
            using (MySqlCommand cmd = myConn.CreateCommand())
            {
                myConn.Open();

                cmd.CommandText = "INSERT INTO POE_ITEM (ID, STASH_ID, ITEM_NAME, VERIFIED, ITEM_LEVEL, TYPE_LINE," +
                    " IDENTIFIED, CORRUPTED, LOCKED_TO_CHAR, NOTE, FRAME_TYPE, STASH_POS_X, STASH_POS_Y, SEC_DESCRIPTION," +
                    " DESCRIPTION_TEXT, DUPLICATED, MAX_STACK_SIZE, STACK_SIZE, SUPPORT, SLOT_WIDTH, SLOT_HEIGHT, ICON, LEAGUE, " +
                    " INVENTORY_ID, ART_FILE_NAME, TALISMAN_TIER, PROPH_DIFF_TEXT, PROPHECY_TEXT, IS_RELIC, SOCKETED_TO_ITEM, ADDED_TS, CONCAT_FLAVOUR_TEXT, CHANGE_ID) " +
                    "VALUES (@ID, @STASHID, @ITEMNAME, @VERIFIED, @ITEMLEVEL, @TYPELINE, @IDENTIFIED, @CORRUPTED, @LOCKEDTOCHAR," +
                    " @NOTE, @FRAMETYPE, @STASHPOSX, @STASHPOSY, @SECDESCRIPTION, @DESCRIPTIONTEXT, @DUPLICATED, @MAXSTACKSIZE," +
                    " @STACKSIZE, @SUPPORT, @SLOTWIDTH, @SLOTHEIGHT, @ICON, @LEAGUE, @INVENTORYID, @ARTFILENAME, @TALISMANTIER," +
                    " @PROPHDIFFTEXT, @PROPHECYTEXT, @ISRELIC, @SOCKETEDTOITEM, @ADDEDTS, @CONCATFLAVOURTEXT, @CHANGEID)";

                cmd.Parameters.AddWithValue("@ID", this.id);
                cmd.Parameters.AddWithValue("@STASHID", this.uniqueStashId);
                cmd.Parameters.AddWithValue("@ITEMNAME", this.name.Replace("<<set:MS>><<set:M>><<set:S>>",""));
                if (verified)
                    cmd.Parameters.AddWithValue("@VERIFIED", 1);
                else
                    cmd.Parameters.AddWithValue("@VERIFIED", 0);
                cmd.Parameters.AddWithValue("@ITEMLEVEL", this.ilvl);
                cmd.Parameters.AddWithValue("@TYPELINE", this.typeLine.Replace("<<set:MS>><<set:M>><<set:S>>",""));
                if (identified)
                    cmd.Parameters.AddWithValue("@IDENTIFIED", 1);
                else
                    cmd.Parameters.AddWithValue("@IDENTIFIED", 0);
                if (corrupted)
                    cmd.Parameters.AddWithValue("@CORRUPTED", 1);
                else
                    cmd.Parameters.AddWithValue("@CORRUPTED", 0);
                if (lockedToChar)
                    cmd.Parameters.AddWithValue("@LOCKEDTOCHAR", 1);
                else
                    cmd.Parameters.AddWithValue("@LOCKEDTOCHAR", 0);
                cmd.Parameters.AddWithValue("@NOTE", this.note);
                cmd.Parameters.AddWithValue("@FRAMETYPE", this.frameType);
                cmd.Parameters.AddWithValue("@STASHPOSX", this.x);
                cmd.Parameters.AddWithValue("@STASHPOSY", this.y);
                cmd.Parameters.AddWithValue("@SECDESCRIPTION", this.secDescriptionText);
                cmd.Parameters.AddWithValue("@DESCRIPTIONTEXT", this.descrText);
                if (duplicated)
                    cmd.Parameters.AddWithValue("@DUPLICATED", 1);
                else
                    cmd.Parameters.AddWithValue("@DUPLICATED", 0);
                cmd.Parameters.AddWithValue("@MAXSTACKSIZE", this.maxStackSize);
                cmd.Parameters.AddWithValue("@STACKSIZE", this.stackSize);
                if (support)
                    cmd.Parameters.AddWithValue("@SUPPORT", 1);
                else
                    cmd.Parameters.AddWithValue("@SUPPORT", 0);
                cmd.Parameters.AddWithValue("@SLOTWIDTH", this.width);
                cmd.Parameters.AddWithValue("@SLOTHEIGHT", this.height);
                cmd.Parameters.AddWithValue("@ICON", this.icon);
                cmd.Parameters.AddWithValue("@LEAGUE", this.league);
                cmd.Parameters.AddWithValue("@INVENTORYID", this.inventoryId);
                cmd.Parameters.AddWithValue("@ARTFILENAME", this.artFileName);
                cmd.Parameters.AddWithValue("@TALISMANTIER", this.talismanTier);
                cmd.Parameters.AddWithValue("@PROPHDIFFTEXT", this.prophecyDiffText);
                cmd.Parameters.AddWithValue("@PROPHECYTEXT", this.prophecyText);
                if (isRelic)
                    cmd.Parameters.AddWithValue("@ISRELIC", 1);
                else
                    cmd.Parameters.AddWithValue("@ISRELIC", 0);
                cmd.Parameters.AddWithValue("@SOCKETEDTOITEM", this.socketedToItem);
                cmd.Parameters.AddWithValue("@ADDEDTS", DateTime.Now);
                cmd.Parameters.AddWithValue("@CONCATFLAVOURTEXT", this.concatFlavourText);
                cmd.Parameters.AddWithValue("@CHANGEID", this.changeId);
                cmd.Prepare();

                try
                {
                    cmd.ExecuteNonQuery();
                    this.uniqueId = cmd.LastInsertedId;
                }
                catch (Exception ex)
                {
                    poe_logger.logError("Error while inserting item." + ex.StackTrace);
                }
            }
        }

        private void resetAssociatedSocketedItems()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_ITEM WHERE SOCKETED_TO_ITEM=@ITEMID";
                cmd.Parameters.AddWithValue("@ITEMID", this.uniqueId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        private void resetAssociatedRequirements()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_REQUIREMENT WHERE ITEM_ID=@ITEMID";
                cmd.Parameters.AddWithValue("@ITEMID", this.uniqueId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        private void resetAssociatedSockets()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_SOCKET WHERE ITEM_ID=@ITEMID";
                cmd.Parameters.AddWithValue("@ITEMID", this.uniqueId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        private void resetAssociatedProperties()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_PROPERTY WHERE ITEM_ID=@ITEMID";
                cmd.Parameters.AddWithValue("@ITEMID", this.uniqueId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        private void resetAssociatedMods()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_MODS WHERE ITEM_ID=@ITEMID";
                cmd.Parameters.AddWithValue("@ITEMID", this.uniqueId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        private void deleteItem()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "DELETE FROM POE_ITEM WHERE UNIQUE_ID=@UNIQUEID";
                cmd.Parameters.AddWithValue("@UNIQUEID", this.uniqueId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }
        
        private void updateItem()
        {
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "UPDATE POE_ITEM SET ID=@ID, STASH_ID=@STASHID, ITEM_NAME=@ITEMNAME, VERIFIED=@VERIFIED, ITEM_LEVEL=@ITEMLEVEL, TYPE_LINE=@TYPELINE," +
                    " IDENTIFIED=@IDENTIFIED, CORRUPTED=@CORRUPTED, LOCKED_TO_CHAR=@LOCKEDTOCHAR, NOTE=@NOTE, FRAME_TYPE=@FRAMETYPE, STASH_POS_X=@STASHPOSX, STASH_POS_Y=@STASHPOSY, SEC_DESCRIPTION=@SECDESCRIPTION," +
                    " DESCRIPTION_TEXT=@DESCRIPTIONTEXT, DUPLICATED=@DUPLICATED, MAX_STACK_SIZE=@MAXSTACKSIZE, STACK_SIZE=@STACKSIZE, SUPPORT=@SUPPORT, SLOT_WIDTH=@SLOTWIDTH, SLOT_HEIGHT=@SLOTHEIGHT, ICON=@ICON, LEAGUE=@LEAGUE, " +
                    " INVENTORY_ID=@INVENTORYID, ART_FILE_NAME=@ARTFILENAME, TALISMAN_TIER=@TALISMANTIER, PROPH_DIFF_TEXT=@PROPHDIFFTEXT, PROPHECY_TEXT=@PROPHECYTEXT, IS_RELIC=@ISRELIC, SOCKETED_TO_ITEM=@SOCKETEDTOITEM, " +
                    " ADDED_TS=@ADDEDTS, CONCAT_FLAVOUR_TEXT=@CONCATFLAVOURTEXT, CHANGE_ID=@CHANGEID WHERE UNIQUE_ID=@UNIQUEID";

                cmd.Parameters.AddWithValue("@ID", this.id);
                cmd.Parameters.AddWithValue("@STASHID", this.uniqueStashId);
                cmd.Parameters.AddWithValue("@ITEMNAME", this.name);
                if (verified)
                    cmd.Parameters.AddWithValue("@VERIFIED", 1);
                else
                    cmd.Parameters.AddWithValue("@VERIFIED", 0);
                cmd.Parameters.AddWithValue("@ITEMLEVEL", this.ilvl);
                cmd.Parameters.AddWithValue("@TYPELINE", this.typeLine);
                if (identified)
                    cmd.Parameters.AddWithValue("@IDENTIFIED", 1);
                else
                    cmd.Parameters.AddWithValue("@IDENTIFIED", 0);
                if (corrupted)
                    cmd.Parameters.AddWithValue("@CORRUPTED", 1);
                else
                    cmd.Parameters.AddWithValue("@CORRUPTED", 0);
                if (lockedToChar)
                    cmd.Parameters.AddWithValue("@LOCKEDTOCHAR", 1);
                else
                    cmd.Parameters.AddWithValue("@LOCKEDTOCHAR", 0);
                cmd.Parameters.AddWithValue("@NOTE", this.note);
                cmd.Parameters.AddWithValue("@FRAMETYPE", this.frameType);
                cmd.Parameters.AddWithValue("@STASHPOSX", this.x);
                cmd.Parameters.AddWithValue("@STASHPOSY", this.y);
                cmd.Parameters.AddWithValue("@SECDESCRIPTION", this.secDescriptionText);
                cmd.Parameters.AddWithValue("@DESCRIPTIONTEXT", this.descrText);
                if (duplicated)
                    cmd.Parameters.AddWithValue("@DUPLICATED", 1);
                else
                    cmd.Parameters.AddWithValue("@DUPLICATED", 0);
                cmd.Parameters.AddWithValue("@MAXSTACKSIZE", this.maxStackSize);
                cmd.Parameters.AddWithValue("@STACKSIZE", this.stackSize);
                if (support)
                    cmd.Parameters.AddWithValue("@SUPPORT", 1);
                else
                    cmd.Parameters.AddWithValue("@SUPPORT", 0);
                cmd.Parameters.AddWithValue("@SLOTWIDTH", this.width);
                cmd.Parameters.AddWithValue("@SLOTHEIGHT", this.height);
                cmd.Parameters.AddWithValue("@ICON", this.icon);
                cmd.Parameters.AddWithValue("@LEAGUE", this.league);
                cmd.Parameters.AddWithValue("@INVENTORYID", this.inventoryId);
                cmd.Parameters.AddWithValue("@ARTFILENAME", this.artFileName);
                cmd.Parameters.AddWithValue("@TALISMANTIER", this.talismanTier);
                cmd.Parameters.AddWithValue("@PROPHDIFFTEXT", this.prophecyDiffText);
                cmd.Parameters.AddWithValue("@PROPHECYTEXT", this.prophecyText);
                if (isRelic)
                    cmd.Parameters.AddWithValue("@ISRELIC", 1);
                else
                    cmd.Parameters.AddWithValue("@ISRELIC", 0);
                cmd.Parameters.AddWithValue("@UNIQUEID", this.uniqueId);
                cmd.Parameters.AddWithValue("SOCKETEDTOITEM", this.socketedToItem);
                cmd.Parameters.AddWithValue("@ADDEDTS", DateTime.Now);
                cmd.Parameters.AddWithValue("@CONCATFLAVOURTEXT", this.concatFlavourText);
                cmd.Parameters.AddWithValue("@CHANGEID", this.changeId);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myConn.close();
            }
        }

        public bool itemExists()
        {
            bool ret = false;
            DBConnector myConn = new DBConnector();
            myConn.connect();

            if (myConn.isConnected())
            {
                MySqlCommand cmd = myConn.getSqlCommand();
                cmd.CommandText = "SELECT UNIQUE_ID FROM POE_ITEM WHERE ID=@ID";

                cmd.Parameters.AddWithValue("@ID", this.id);
                cmd.Prepare();

                MySqlDataReader reader = myConn.executeQuery(cmd);

                if (reader.HasRows)
                {
                    reader.Read();
                    this.uniqueId = reader.GetInt64(0);
                    ret = true;
                }
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                myConn.close();
            }
            return ret;
        }

    }
}
