using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class Pose : IDisposable
    {
        internal static Pose createWrapper(IntPtr pose, object[] args)
        {
            return new Pose(pose);
        }

        IntPtr pose;

        private Pose(IntPtr pose)
        {
            this.pose = pose;
        }

        public void Dispose()
        {
            pose = IntPtr.Zero;
        }

        /// <summary>
	    /// Return the name of the pose (may be blank). 
	    /// </summary>
	    /// <returns>The name of the pose.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(Pose_getName(pose));
        }

	    /// <summary>
	    /// Return the target geometry index of the pose. 
	    /// </summary>
	    /// <returns>The index of the target geometry.</returns>
        public ushort getTarget()
        {
            return Pose_getTarget(pose);
        }

	    /// <summary>
	    /// Adds an offset to a vertex for this pose.
	    /// </summary>
	    /// <param name="index">The vertex index.</param>
	    /// <param name="offset">The position offset for this pose.</param>
        public void addVertex(IntPtr index, Vector3 offset)
        {
            Pose_addVertex(pose, index, offset);
        }

	    /// <summary>
	    /// Remove a vertex offset. 
	    /// </summary>
	    /// <param name="index">The index of the offset to remove.</param>
        public void removeVertex(IntPtr index)
        {
            Pose_removeVertex(pose, index);
        }

	    /// <summary>
	    /// Clear all vertices. 
	    /// </summary>
        public void clearVertices()
        {
            Pose_clearVertices(pose);
        }

	    /// <summary>
	    /// Get the offset at the specified index.
	    /// </summary>
	    /// <param name="index">The index of the offset to get.</param>
	    /// <returns></returns>
        public Vector3 getOffset(IntPtr index)
        {
            return Pose_getOffset(pose, index);
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pose_getName(IntPtr pose);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pose_getTarget(IntPtr pose);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pose_addVertex(IntPtr pose, IntPtr index, Vector3 offset);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pose_removeVertex(IntPtr pose, IntPtr index);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pose_clearVertices(IntPtr pose);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Pose_getOffset(IntPtr pose, IntPtr index);

#endregion
    }
}
