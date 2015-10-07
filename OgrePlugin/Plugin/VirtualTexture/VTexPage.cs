using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class VTexPage
    {
        public VTexPage(byte x, byte y, byte mip, byte indirectionTexId)
        {
            this.x = x;
            this.y = y;
            this.mip = mip;
            this.indirectionTexId = indirectionTexId;
            this.hashCode = (int)((uint)(mip << 24) + (uint)(indirectionTexId << 16) + (uint)(x << 8) + (uint)y); //Due to the nature of this class, these are always unique
        }

        public readonly byte x;
        public readonly byte y;

        /// <summary>
        /// The mip level, note that this is sorted backwards from the way 3d apis define it, 
        /// e.g. 0 is the highest (smallest) mip level and X is the lowest (largest) mip level.
        /// </summary>
        public readonly byte mip;
        public readonly byte indirectionTexId;
        private readonly int hashCode;

        public static bool operator ==(VTexPage p1, VTexPage p2)
        {
            Object o1 = p1;
            Object o2 = p2;
            if(o1 == null || o2 == null)
            {
                return o1 == o2;
            }
            return p1.hashCode == p2.hashCode;
        }

        public static bool operator !=(VTexPage p1, VTexPage p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            VTexPage other = obj as VTexPage;
            if(other != null)
            {
                return this.hashCode == other.hashCode;
            }
            return false;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1} m: {2} id: {3}", x, y, mip, indirectionTexId);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
