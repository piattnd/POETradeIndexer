using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POETradeIndexer
{
    public class poe_enums
    {
        public enum valueTypes
        {
            whiteOrPhysical,
            blueForModifiedValue,
            fire,
            cold,
            lightning,
            chaos
        }

        public enum frameType
        {
            normal,
            magic,
            rare,
            unique,
            gem,
            currency,
            divinationCard,
            questItem,
            prophecy,
            relic
        }

        public enum modType
        {
            EXPLICIT=1,
            IMPLICIT=2,
            ENCHANT=3,
            CRAFTED=4,
            UTILITY=5,
            COSMETIC=6
        }
    }
}
