//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POETraderWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class POE_ITEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POE_ITEM()
        {
            this.POE_PROPERTY = new HashSet<POE_PROPERTY>();
            this.POE_MODS = new HashSet<POE_MODS>();
            this.POE_REQUIREMENT = new HashSet<POE_REQUIREMENT>();
            this.POE_SOCKET = new HashSet<POE_SOCKET>();
        }

        public long UNIQUE_ID { get; set; }
        public string ID { get; set; }
        public Nullable<long> STASH_ID { get; set; }

        [Display(Name = "Item Name: ")]
        public string ITEM_NAME { get; set; }

        [Display(Name = "Verified: ")]
        public Nullable<sbyte> VERIFIED { get; set; }

        [Display(Name = "Item Level: ")]
        public Nullable<int> ITEM_LEVEL { get; set; }

        [Display(Name = "Type Line: ")]
        public string TYPE_LINE { get; set; }

        [Display(Name = "Identified: ")]
        public Nullable<sbyte> IDENTIFIED { get; set; }

        [Display(Name = "Corrputed: ")]
        public Nullable<sbyte> CORRUPTED { get; set; }

        [Display(Name = "Locked To Char: ")]
        public Nullable<sbyte> LOCKED_TO_CHAR { get; set; }

        [Display(Name = "Note: ")]
        public string NOTE { get; set; }

        [Display(Name = "Frame Type: ")]
        public Nullable<short> FRAME_TYPE { get; set; }

        [Display(Name = "Position X: ")]
        public Nullable<int> STASH_POS_X { get; set; }

        [Display(Name = "Position Y: ")]
        public Nullable<int> STASH_POS_Y { get; set; }

        [Display(Name = "Secondary Description: ")]
        public string SEC_DESCRIPTION { get; set; }

        [Display(Name = "Primary Description: ")]
        public string DESCRIPTION_TEXT { get; set; }

        [Display(Name = "Duplicated: ")]
        public Nullable<sbyte> DUPLICATED { get; set; }

        [Display(Name = "Max Stack Size: ")]
        public Nullable<int> MAX_STACK_SIZE { get; set; }

        [Display(Name = "Stack Size: ")]
        public Nullable<int> STACK_SIZE { get; set; }

        [Display(Name = "Is Support: ")]
        public Nullable<sbyte> SUPPORT { get; set; }

        [Display(Name = "Slot Width: ")]
        public Nullable<int> SLOT_WIDTH { get; set; }

        [Display(Name = "Slot Height: ")]
        public Nullable<int> SLOT_HEIGHT { get; set; }

        [Display(Name = "Icon: ")]
        public string ICON { get; set; }

        [Display(Name = "League: ")]
        public string LEAGUE { get; set; }

        [Display(Name = "Inventory ID: ")]
        public string INVENTORY_ID { get; set; }

        [Display(Name = "Art File Name: ")]
        public string ART_FILE_NAME { get; set; }

        [Display(Name = "Talisman Tier: ")]
        public Nullable<sbyte> TALISMAN_TIER { get; set; }

        [Display(Name = "Prophecy Difficulty: ")]
        public string PROPH_DIFF_TEXT { get; set; }

        [Display(Name = "Prophecy Text: ")]
        public string PROPHECY_TEXT { get; set; }

        [Display(Name = "Is Relic: ")]
        public Nullable<sbyte> IS_RELIC { get; set; }

        [Display(Name = "Socketed To Item: ")]
        public Nullable<long> SOCKETED_TO_ITEM { get; set; }

        [Display(Name = "Item Added: ")]
        public Nullable<System.DateTime> ADDED_TS { get; set; }

        public string CONCAT_FLAVOUR_TEXT { get; set; }
        public Nullable<long> CHANGE_ID { get; set; }
    
        public virtual POE_CHANGE POE_CHANGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POE_PROPERTY> POE_PROPERTY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POE_MODS> POE_MODS { get; set; }
        public virtual POE_STASH POE_STASH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POE_REQUIREMENT> POE_REQUIREMENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POE_SOCKET> POE_SOCKET { get; set; }

        public string getAge()
        {
            string ret = "";
            int theAge = Convert.ToInt32(Math.Round((DateTime.Now - (DateTime)this.ADDED_TS).TotalMinutes, 0));
            if (theAge >= 60)
            {
                // conver minute to hour
                int hourAge = Convert.ToInt32((double)theAge / 60);
                if (hourAge >= 24)
                {
                    int dayAge = Convert.ToInt32((double)hourAge / 24);
                    return dayAge.ToString() + "day(s) ago";
                }
                else
                {
                    return hourAge.ToString() + " hour(s) ago";
                }
            }
            else
            {
                return theAge.ToString() + " minute(s) ago";
            }
        }
    }
}