using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineMath
{
    public struct Ray3
    {
        private Vector3 origin;
        private Vector3 direction;

        /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="origin">The origin of the ray.</param>
	    /// <param name="direction">The direction the ray is facing.</param>
        public Ray3(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        /// <summary>
	    /// The origin.
	    /// </summary>
        public Vector3 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                this.origin = value;
            }
        }

        /// <summary>
	    /// The direction.
	    /// </summary>
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                this.direction = value;
            }
        }

        /// <summary>
	    /// Get the point at the specified distance along the ray.
	    /// </summary>
	    /// <param name="distance">The distance along the ray.</param>
	    /// <returns>The point at the given distance.</returns>
        public Vector3 getPoint(float distance)
        {
            return origin + direction * distance;
        }

        /// <summary>
        /// Determine if all components are numbers i.e != float::NaN.
        /// </summary>
        /// <returns>True if all components are != float::NaN</returns>
        public bool isNumber()
        {
            return origin.isNumber() && direction.isNumber();
        }

        /// <summary>
        /// ToString function.
        /// </summary>
        /// <returns>The ray as a string.</returns>
        public override String ToString()
        {
            return "{" + origin.ToString() + ", " + direction.ToString() + "}";
        }
    }
}
