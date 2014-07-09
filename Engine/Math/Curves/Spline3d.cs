using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// Interface for 3d vector spline classes, allows them to be more interchangeable.
    /// </summary>
    public interface Spline3d
    {
        /// <summary>
        /// Add a control point to the set. You must call computeSplines to
        /// update the changes.
        /// </summary>
        /// <param name="controlPoint">The control point to add.</param>
        void addControlPoint(Vector3 controlPoint);

        /// <summary>
        /// Remove a control point from the set. You must call computeSplines to
        /// update the changes.
        /// </summary>
        /// <param name="index">The index of the control point to remove.</param>
        void removeControlPoint(int index);

        /// <summary>
        /// Update the control point at a given index. You must call
        /// computeSplines to update the changes.
        /// </summary>
        /// <param name="index">The index of the control point to update.</param>
        /// <param name="value">The value to set at index.</param>
        void updateControlPoint(int index, Vector3 value);

        /// <summary>
        /// Gets the number of control points in the spline.
        /// </summary>
        int NumControlPoints { get; }

        /// <summary>
        /// Get a specific control point in the spline set.
        /// </summary>
        /// <param name="index">The index of the control point to retrieve.</param>
        /// <returns>The index of the control point.</returns>
        Vector3 getControlPoint(int index);

        /// <summary>
        /// An enumerator over all the control points.
        /// </summary>
        IEnumerable<Vector3> ControlPoints { get; }

        /// <summary>
        /// Get the value of the spline at a certain percentage.
        /// </summary>
        /// <param name="percent">A value between 0 and 1 for the position on the spline.</param>
        /// <returns>A Vector3 with the location of the spline.</returns>
        Vector3 interpolate(float percent);

        /// <summary>
        /// Get the rotation along the spline at a certain percentage. This will be the rotation of the vector
        /// between the interpolated values from percent to offset against the starting direction.
        /// </summary>
        /// <param name="percent">A value between 0 and 1 for the position on the spline.</param>
        /// <param name="offset">An offset from percent (will be added) for the direction of the vector.</param>
        /// <param name="startingDirection">The vector to compare the calculated vector against.</param>
        /// <returns>The rotation as a quaternion.</returns>
        Quaternion interpolateRotation(float percent, float offset, Vector3 startingDirection);

        /// <summary>
        /// Recompute the splines in the set. This must be called every time the
        /// control points are changed.
        /// </summary>
        void recompute();
    }
}
