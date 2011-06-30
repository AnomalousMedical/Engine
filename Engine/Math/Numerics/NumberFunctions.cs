using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class NumberFunctions
    {
        private NumberFunctions() { }

        public static float lerp(float start, float end, float t)
        {
            return start + (end - start) * t;
        }
    }
}
