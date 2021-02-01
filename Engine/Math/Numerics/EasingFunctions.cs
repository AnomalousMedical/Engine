using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public enum EasingFunction
    {
        None,
        EaseInQuadratic,
        EaseOutQuadratic,
        EaseInOutQuadratic,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo
    }

    public delegate float EasingFunctionDelegate(float start, float change, float time, float duration);

    /// <summary>
    /// A set of easing functions.
    /// </summary>
    /// <remarks>
    /// Based on http://www.gizma.com/easing/
    /// Nice cheat sheet on some of the functions at http://easings.net/
    /// </remarks>
    public static class EasingFunctions
    {
        public static float Ease(EasingFunction func, float start, float change, float time, float duration)
        {
            switch (func)
            {
                case EasingFunction.None:
                    return None(start, change, time, duration);
                
                case EasingFunction.EaseInQuadratic:
                    return EaseInQuadratic(start, change, time, duration);
                case EasingFunction.EaseOutQuadratic:
                    return EaseOutQuadratic(start, change, time, duration);
                case EasingFunction.EaseInOutQuadratic:
                    return EaseInOutQuadratic(start, change, time, duration);

                case EasingFunction.EaseInCubic:
                    return EaseInCubic(start, change, time, duration);
                case EasingFunction.EaseOutCubic:
                    return EaseOutCubic(start, change, time, duration);
                case EasingFunction.EaseInOutCubic:
                    return EaseInOutCubic(start, change, time, duration);

                case EasingFunction.EaseInQuart:
                    return EaseInQuart(start, change, time, duration);
                case EasingFunction.EaseOutQuart:
                    return EaseOutQuart(start, change, time, duration);
                case EasingFunction.EaseInOutQuart:
                    return EaseInOutQuart(start, change, time, duration);

                case EasingFunction.EaseInQuint:
                    return EaseInQuint(start, change, time, duration);
                case EasingFunction.EaseOutQuint:
                    return EaseOutQuint(start, change, time, duration);
                case EasingFunction.EaseInOutQuint:
                    return EaseInOutQuint(start, change, time, duration);

                case EasingFunction.EaseInExpo:
                    return EaseInExpo(start, change, time, duration);
                case EasingFunction.EaseOutExpo:
                    return EaseOutExpo(start, change, time, duration);
                case EasingFunction.EaseInOutExpo:
                    return EaseInOutExpo(start, change, time, duration);
                default:
                    throw new NotSupportedException(String.Format("Easing function {0} not supported", func));
            }
        }

        public static EasingFunctionDelegate GetEasingFunction(EasingFunction func)
        {
            switch (func)
            {
                case EasingFunction.None:
                    return None;

                case EasingFunction.EaseInQuadratic:
                    return EaseInQuadratic;;
                case EasingFunction.EaseOutQuadratic:
                    return EaseOutQuadratic;
                case EasingFunction.EaseInOutQuadratic:
                    return EaseInOutQuadratic;

                case EasingFunction.EaseInCubic:
                    return EaseInCubic;
                case EasingFunction.EaseOutCubic:
                    return EaseOutCubic;
                case EasingFunction.EaseInOutCubic:
                    return EaseInOutCubic;

                case EasingFunction.EaseInQuart:
                    return EaseInQuart;
                case EasingFunction.EaseOutQuart:
                    return EaseOutQuart;
                case EasingFunction.EaseInOutQuart:
                    return EaseInOutQuart;

                case EasingFunction.EaseInQuint:
                    return EaseInQuint;
                case EasingFunction.EaseOutQuint:
                    return EaseOutQuint;
                case EasingFunction.EaseInOutQuint:
                    return EaseInOutQuint;

                case EasingFunction.EaseInExpo:
                    return EaseInExpo;
                case EasingFunction.EaseOutExpo:
                    return EaseOutExpo;
                case EasingFunction.EaseInOutExpo:
                    return EaseInOutExpo;
                default:
                    throw new NotSupportedException(String.Format("Easing function {0} not supported", func));
            }
        }

        // Linear easing.
        public static float None(float start, float change, float time, float duration)
        {
            return change * (time / duration) + start;
        }

        // Quadratic easing
        public static float EaseInQuadratic(float start, float change, float time, float duration)
        {
            time /= duration;
            return change * time * time + start;
        }

        public static float EaseOutQuadratic(float start, float change, float time, float duration)
        {
            time /= duration;
            return -change * time * (time - 2) + start;
        }

        public static float EaseInOutQuadratic(float start, float change, float time, float duration)
        {
            time /= duration / 2;
            if (time < 1)
            {
                return change / 2 * time * time + start;
            }
            time--;
            return -change / 2 * (time*(time - 2) - 1) + start;
        }

        // Cubic easing        
        public static float EaseInCubic(float start, float change, float time, float duration)
        {
            time /= duration;
            return change * time * time * time + start;
        }

        public static float EaseOutCubic(float start, float change, float time, float duration)
        {
            time /= duration;
            time--;
            return change * (time * time * time + 1) + start;
        }

        public static float EaseInOutCubic(float start, float change, float time, float duration)
        {
            time /= duration / 2;
            if (time < 1)
            {
                return change / 2 * time * time * time + start;
            }
            time -= 2;
            return change / 2 * (time * time * time + 2) + start;
        }

        // Quartic easing
        public static float EaseInQuart(float start, float change, float time, float duration)
        {
	        time /= duration;
	        return change * time * time * time * time + start;
        }

        public static float EaseOutQuart(float start, float change, float time, float duration) 
        {
	        time /= duration;
	        time--;
	        return -change * (time * time * time * time - 1) + start;
        }

        public static float EaseInOutQuart(float start, float change, float time, float duration)
        {
	        time /= duration / 2;
            if (time < 1)
            {
                return change / 2 * time * time * time * time + start;
            }
	        time -= 2;
	        return -change / 2 * (time * time * time * time - 2) + start;
        }

        // Quintic easing
        public static float EaseInQuint(float start, float change, float time, float duration)
        {
            time /= duration;
            return change * time * time * time * time * time + start;
        }

        public static float EaseOutQuint(float start, float change, float time, float duration) 
        {
	        time /= duration;
	        time--;
	        return change*(time * time * time * time * time + 1) + start;
        }

        public static float EaseInOutQuint(float start, float change, float time, float duration) 
        {
	        time /= duration/2;
	        if (time < 1) return change/2*time*time*time*time*time + start;
	        time -= 2;
	        return change/2*(time*time*time*time*time + 2) + start;
        }

        // Exponential easing        
        public static float EaseInExpo(float start, float change, float time, float duration) 
        {
	        return change * (float)Math.Pow(2, 10 * ( time / duration - 1)) + start;
        }

        public static float EaseOutExpo(float start, float change, float time, float duration)
        {
            return change * (-(float)Math.Pow(2, -10 * time / duration) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float change, float time, float duration) 
        {
	        time /= duration / 2;
            if (time < 1)
            {
                return change / 2 * (float)Math.Pow(2, 10 * (time - 1)) + start;
            }
	        time--;
            return change / 2 * (-(float)Math.Pow(2, -10 * time) + 2) + start;
        }

    }
}
