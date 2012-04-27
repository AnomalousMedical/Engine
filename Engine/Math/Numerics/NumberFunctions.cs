using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public static class NumberFunctions
    {
        public static float lerp(float start, float end, float t)
        {
            return start + (end - start) * t;
        }
        
        /// <summary>
        /// Figure out the power of two number that is closest to but larger
        /// than the given number. So if the input is 1023 you will get 1024 if
        /// it is 1025 you get 2048.
        /// </summary>
        /// <param name="actualNumber">The number to compute the closest value for</param>
        /// <param name="pow2Start">A starting power of two number. Allows for less iterations if you know a close starting value. Still works even if this number is too big.</param>
        /// <returns>The closest power of two number to acutalNumber that is bigger than actualNumber</returns>
        public static int computeClosestLargerPow2(int actualNumber, int pow2Start)
        {
            int pow2Size = pow2Start;
            if (actualNumber < pow2Start)
            {
                while (pow2Size > actualNumber)
                {
                    pow2Size >>= 1;
                }
                pow2Size <<= 1;
            }
            else
            {
                while (pow2Size < actualNumber)
                {
                    pow2Size <<= 1;
                }
            }
            return pow2Size;
        }
    }
}
