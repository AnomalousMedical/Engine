using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class XpCalculator
    {
        private XpTable xpTable = new XpTable();

        public long GetXpNeeded(Archetype archetype, long level)
        {
            var mod = xpTable.GetRank(archetype, level);

            long xp = 0;
            for(var i = 0; i < level; ++i)
            {
                xp += mod * i * i / 10;
            }

            return xp;
        }
    }
}
