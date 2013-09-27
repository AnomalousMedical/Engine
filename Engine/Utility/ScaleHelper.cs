using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Engine
{
    public static class ScaleHelper
    {
        static ScaleHelper()
        {
            ScaleFactor = 1.0f;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //When .net 4.5
        public static int Scaled(int originalValue)
        {
            return (int)(originalValue * ScaleFactor);
        }

        public static float ScaleFactor { get; private set; }

        /// <summary>
        /// Call this to set the scale factor, note that this should only be called while the engine is being setup at the beginning and
        /// never again after that.
        /// </summary>
        /// <param name="scale"></param>
        public static void _setScaleFactor(float scale)
        {
            ScaleFactor = scale;
        }
    }
}
