using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Engine
{
    public static class ScaleHelper
    {
        private static float scaleFactor;
        private static float inverseScaleFactor;

        static ScaleHelper()
        {
            scaleFactor = 1.0f;
            inverseScaleFactor = 1.0f;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //When .net 4.5
        public static int Scaled(int originalValue)
        {
            int scaled = (int)(originalValue * scaleFactor);
            //Keep at least 1 if the original value wasn't 0.
            if(originalValue > 0 && scaled == 0)
            {
                scaled = 1;
            }
            return scaled;
        }

        public static uint Scaled(uint originalValue)
        {
            uint scaled = (uint)(originalValue * scaleFactor);
            //Keep at least 1 if the original value wasn't 0.
            if (originalValue > 0 && scaled == 0)
            {
                scaled = 1;
            }
            return scaled;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)] //When .net 4.5
        public static int Unscaled(int originalValue)
        {
            return (int)(originalValue * inverseScaleFactor);
        }

        public static float ScaleFactor
        {
            get
            {
                return scaleFactor;
            }
        }

        /// <summary>
        /// Call this to set the scale factor, note that this should only be called while the engine is being setup at the beginning and
        /// never again after that.
        /// </summary>
        /// <param name="scale"></param>
        public static void _setScaleFactor(float scale)
        {
            scaleFactor = scale;
            inverseScaleFactor = 1f / scale;
        }
    }
}
