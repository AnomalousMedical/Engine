using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Some floating point comparison functions.
    /// Based on the article http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/
    /// </summary>
    public static class FloatExtensions
    {
        public static bool EpsilonEquals(this float number, float test, float epsilon = float.Epsilon)
        {
            return number < test + epsilon && number > test - epsilon;
        }

        /// <summary>
        /// Good for comparing to 0 with appropriate value for maxDiff. Uses an absolute comparsion against maxdiff (Math.Abs(A - B) &lt;= maxDiff).
        /// </summary>
        /// <param name="A">This float</param>
        /// <param name="B">Comparison float</param>
        /// <param name="maxDiff">The maximum difference between the two numbers to count as the same.</param>
        /// <remarks>
        /// Fastest .236 seconds for 400000 floats
        /// </remarks>
        /// <returns></returns>
        public static bool AlmostEqualAbsolute(this float A, float B, float maxDiff)
        {
            float diff = Math.Abs(A - B);
            return diff <= maxDiff;
        }

        /// <summary>
        /// To compare f1 and f2 calculate diff = fabs(f1-f2). If diff is smaller than n% of max(abs(f1),abs(f2)) then f1 and f2 can be considered equal.
        /// Good for comparing to a nonzero number, but AlmostEqualUlps is a bit faster.
        /// </summary>
        /// <param name="A">This float</param>
        /// <param name="B">Comparison float</param>
        /// <param name="maxRelDiff">The percentage of the largest number to still allow.</param>
        /// <remarks>
        /// Moderate .505 seconds for 400000 floats
        /// </remarks>
        /// <returns></returns>
        public static bool AlmostEqualRelative(this float A, float B, float maxRelDiff)
        {
            // Calculate the difference.
            float diff = Math.Abs(A - B);
            A = Math.Abs(A);
            B = Math.Abs(B);
            // Find the largest
            float largest = (B > A) ? B : A;

            if (diff <= largest * maxRelDiff)
                return true;
            return false;
        }

        /// <summary>
        /// Compare equality by allowing a certain number of representable floats between the two numbers.
        /// Good for comparing to a nonzero number. As you get close to 0 the ulps increase a lot.
        /// </summary>
        /// <param name="a">This float</param>
        /// <param name="b">Comparison float</param>
        /// <param name="maxUlpsDiff">The number of representable floats to allow between the two values.</param>
        /// <remarks>
        /// Moderate .426 seconds for 400000 floats
        /// </remarks>
        /// <returns></returns>
        public static bool AlmostEqualUlps(this float a, float b, int maxUlpsDiff)
        {
            int uA = FloatToInt32Bits(a);
            int uB = FloatToInt32Bits(b);

            // Different signs means they do not match.
            if (Negative(uA) != Negative(uB))
            {
                // Check for equality to make sure +0==-0
                if (a == b)
                {
                    return true;
                }
                return false;
            }

            int ulpsDiff = Math.Abs(uA - uB);
            return ulpsDiff <= maxUlpsDiff;
        }

        /// <summary>
        /// Good for comparing two numbers that could be anything including 0 using ulps.
        /// </summary>
        /// <param name="A">This float</param>
        /// <param name="B">Comparison float</param>
        /// <param name="maxDiff">The difference to use for the absolute equal calculation.</param>
        /// <param name="maxUlpsDiff">The number of representable floats to allow between the two values.</param>
        /// <remarks>
        /// Slow, 3.350 seconds for 400000 floats
        /// </remarks>
        /// <returns></returns>
        public static bool AlmostEqualUlpsAndAbs(this float A, float B, float maxDiff, int maxUlpsDiff)
        {
            // Check if the numbers are really close -- needed
            // when comparing numbers near zero.
            float absDiff = Math.Abs(A - B);
            if (absDiff <= maxDiff)
                return true;

            int uA = FloatToInt32Bits(A);
            int uB = FloatToInt32Bits(B);

            // Different signs means they do not match.
            if (Negative(uA) != Negative(uB))
                return false;

            // Find the difference in ULPs.
            int ulpsDiff = Math.Abs(uA - uB);
            if (ulpsDiff <= maxUlpsDiff)
                return true;

            return false;
        }

        /// <summary>
        /// Good for two numbers that could be anything including 0 using a relative calculation.
        /// </summary>
        /// <param name="A">This float</param>
        /// <param name="B">Comparison float</param>
        /// <param name="maxDiff">The difference to use for the absolute equal calculation.</param>
        /// <param name="maxRelDiff">The percentage of the largest number to still allow.</param>
        /// <remarks>
        /// Slowest, 5.421 seconds for 400000 floats
        /// </remarks>
        /// <returns></returns>
        public static bool AlmostEqualRelativeAndAbs(this float A, float B, float maxDiff, float maxRelDiff)
        {
            // Check if the numbers are really close -- needed
            // when comparing numbers near zero.
            float diff = Math.Abs(A - B);
            if (diff <= maxDiff)
                return true;

            A = Math.Abs(A);
            B = Math.Abs(B);
            float largest = (B > A) ? B : A;

            if (diff <= largest * maxRelDiff)
                return true;
            return false;
        }

        public static float interpolate(this float start, float end, float t)
        {
            return start + (end - start) * t;
        }

        private static unsafe int FloatToInt32Bits(float f)
        {
            return *((int*)&f);
        }

        private static bool Negative(int iFlt)
        {
            return iFlt >> 31 != 0;
        }
    }
}
