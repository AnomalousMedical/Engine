using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Engine
{
    public class ScaleHelper : IScaleHelper
    {
        private static float scaleFactor;
        private static float inverseScaleFactor;

        public ScaleHelper(OSWindow window)
        {
            scaleFactor = window.WindowScaling;
            inverseScaleFactor = 1.0f / scaleFactor;
        }

        public int Scaled(int originalValue)
        {
            int scaled = (int)(originalValue * scaleFactor);
            //Keep at least 1 if the original value wasn't 0.
            if (originalValue > 0 && scaled == 0)
            {
                scaled = 1;
            }
            return scaled;
        }

        public uint Scaled(uint originalValue)
        {
            uint scaled = (uint)(originalValue * scaleFactor);
            //Keep at least 1 if the original value wasn't 0.
            if (originalValue > 0 && scaled == 0)
            {
                scaled = 1;
            }
            return scaled;
        }

        public IntVector2 Scaled(IntVector2 originalValue)
        {
            return new IntVector2(Scaled(originalValue.x), Scaled(originalValue.y));
        }

        public IntPad Scaled(IntPad originalValue)
        {
            return new IntPad(Scaled(originalValue.Left), Scaled(originalValue.Top), Scaled(originalValue.Right), Scaled(originalValue.Bottom));
        }

        public IntRect Scaled(IntRect originalValue)
        {
            return new IntRect(Scaled(originalValue.Left), Scaled(originalValue.Top), Scaled(originalValue.Width), Scaled(originalValue.Height));
        }

        public int Unscaled(int originalValue)
        {
            return (int)(originalValue * inverseScaleFactor);
        }

        public float ScaleFactor
        {
            get
            {
                return scaleFactor;
            }
        }
    }
}
