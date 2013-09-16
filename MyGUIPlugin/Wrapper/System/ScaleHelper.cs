using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyGUIPlugin
{
    public static class ScaleHelper
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //When .net 4.5
        public static int Scaled(int originalValue)
        {
            return (int)(originalValue * CachedScaleFactor);
        }

        public static float CachedScaleFactor { get; internal set; }
    }
}
