using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Engine
{
    /// <summary>
    /// Segment3 math class.  A segment is similar to a ray, but the Direction vector
    /// also contains the length of the vector it is not always a unit as in the ray.
    /// </summary>
    public struct Segment3
    {
	    public Vector3 Origin;
        public Vector3 Direction;

	    /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="origin">The origin of the segment3.</param>
	    /// <param name="direction">The direction the segment3 is facing.</param>
        public Segment3(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        /// <summary>
        /// ToString().
        /// </summary>
        /// <returns></returns>
	    public override string  ToString()
        {
 	         return "{" + Origin.ToString() + ", " + Direction.ToString() + "}";
        }

	    /// <summary>
	    /// Determine if all components are numbers i.e != float::NaN.
	    /// </summary>
	    /// <returns>True if all components are != float::NaN</returns>
	    bool isNumber()
	    {
		    return Origin.isNumber() && Direction.isNumber();
	    }
    }
}
