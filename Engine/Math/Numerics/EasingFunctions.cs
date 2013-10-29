using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    [SingleEnum]
    public enum EasingFunction
    {
        None,
        EaseInQuadratic,
        EaseOutQuadratic,
        EaseInOutQuadratic,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic
    }

    public delegate float EasingFunctionDelegate(float start, float change, float time, float duration);

    /// <summary>
    /// A set of easing functions.
    /// </summary>
    /// <remarks>
    /// Based on http://www.gizma.com/easing/
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
                default:
                    throw new NotSupportedException(String.Format("Easing function {0} not supported", func));
            }
        }

        public static float None(float start, float change, float time, float duration)
        {
            return change * (time / duration) + start;
        }

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
                return change * time * time * time + start;
            }
            time -= 2;
            return change / 2 * (time * time * time + 2) + start;
        }
    }
}
