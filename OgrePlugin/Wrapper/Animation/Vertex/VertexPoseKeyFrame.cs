using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class VertexPoseKeyFrame : KeyFrame
    {
        internal static VertexPoseKeyFrame createWrapper(IntPtr nativeObject, object[] args)
        {
            return new VertexPoseKeyFrame(nativeObject);
        }

        protected VertexPoseKeyFrame(IntPtr keyFrame)
            :base(keyFrame)
        {

        }

        /// <summary>
	    /// Add a new pose reference. 
	    /// </summary>
	    /// <param name="poseIndex">The index of the pose.</param>
	    /// <param name="influence">The influence for this key frame.</param>
	    public void addPoseReference(ushort poseIndex, float influence)
        {
            VertexPoseKeyFrame_addPoseReference(keyFrame, poseIndex, influence);
        }

	    /// <summary>
	    /// Update the influence of a pose reference.
	    /// </summary>
	    /// <param name="poseIndex">The index of the pose.</param>
	    /// <param name="influence">The new influence for this key frame.</param>
        public void updatePoseReference(ushort poseIndex, float influence)
        {
            VertexPoseKeyFrame_updatePoseReference(keyFrame, poseIndex, influence);
        }

	    /// <summary>
	    /// Remove reference to a given pose. 
	    /// </summary>
	    /// <param name="poseIndex">The pose index (not the index of the reference).</param>
        public void removePoseReference(ushort poseIndex)
        {
            VertexPoseKeyFrame_removePoseReference(keyFrame, poseIndex);
        }

	    /// <summary>
	    /// Remove all pose references.
	    /// </summary>
        public void removeAllPoseReferences()
        {
            VertexPoseKeyFrame_removeAllPoseReferences(keyFrame);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexPoseKeyFrame_addPoseReference(IntPtr vpKeyFrame, ushort poseIndex, float influence);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexPoseKeyFrame_updatePoseReference(IntPtr vpKeyFrame, ushort poseIndex, float influence);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexPoseKeyFrame_removePoseReference(IntPtr vpKeyFrame, ushort poseIndex);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void VertexPoseKeyFrame_removeAllPoseReferences(IntPtr vpKeyFrame);

#endregion
    }
}
