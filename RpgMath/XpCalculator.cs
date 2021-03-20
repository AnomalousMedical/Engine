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
            long xLv = ((level - 2) / 10) * 10 + 2;
            long sLv = level - xLv;

            var mod = xpTable.GetRank(archetype, level);
            var start = xpTable.GetStart(level, mod);

            var xp = start;
            if(sLv > 0)
            {
                for(var i = 0; i < sLv; ++i)
                {
                    var sum = xLv + i;
                    xp += mod * sum * sum / 10;
                }
            }

            return xp;
        }
    }
}
