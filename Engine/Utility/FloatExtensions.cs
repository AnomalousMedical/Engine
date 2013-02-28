using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public static class FloatExtensions
    {
        public static bool EpsilonEquals(this float number, float test, float epsilon = float.Epsilon)
        {
            return number < test + epsilon && number > test - epsilon;
        }
    }
}
