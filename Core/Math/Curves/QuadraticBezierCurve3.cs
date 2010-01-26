using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A QuadraticBezierCurve calculator class. This will compute one quadratic
    /// bezier curve.
    /// </summary>
    public class QuadraticBezierCurve3
    {
        Vector3 start;
        Vector3 middle;
        Vector3 end;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuadraticBezierCurve3()
        {
            start = Vector3.Zero;
            middle = Vector3.Zero;
            end = Vector3.Zero;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="middle">The middle point.</param>
        /// <param name="end">The end point.</param>
        public QuadraticBezierCurve3(Vector3 start, Vector3 middle, Vector3 end)
        {
            this.start = start;
            this.middle = middle;
            this.end = end;
        }

        /// <summary>
        /// Compute a curve based on a percent. Percent must be between 0 and 1.
        /// </summary>
        /// <param name="time">A value between 0 and 1 for the point to compute on the curve.</param>
        /// <returns>The Vector3 with the position of the point on the line at percent.</returns>
        public Vector3 computePoint(float percent)
        {
            float minusPercent = 1.0f - percent;
            return new Vector3(start.x * minusPercent * minusPercent + middle.x * 2 * percent * minusPercent + end.x * percent * percent,
                               start.y * minusPercent * minusPercent + middle.y * 2 * percent * minusPercent + end.y * percent * percent,
                               start.z * minusPercent * minusPercent + middle.z * 2 * percent * minusPercent + end.z * percent * percent);
        }

        /// <summary>
        /// The starting point of the curve.
        /// </summary>
        public Vector3 Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        /// <summary>
        /// The middle point of the curve.
        /// </summary>
        public Vector3 Middle
        {
            get
            {
                return middle;
            }
            set
            {
                middle = value;
            }
        }

        /// <summary>
        /// The end point of the curve.
        /// </summary>
        public Vector3 End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }
    }
}
