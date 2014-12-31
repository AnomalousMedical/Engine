using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class Bone : Node
    {
        private WrapperCollection<Bone> bones = new WrapperCollection<Bone>(Bone.createWrapper);

        internal static Bone createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Bone(nativeObject);
        }

        private Bone(IntPtr bone)
            :base(bone)
        {
            
        }

        public override void Dispose()
        {
            bones.Dispose();
        }

        /// <summary>
	    /// Creates a new Bone as a child of this bone.
	    /// 
        /// This method creates a new bone which will inherit the transforms of this bone, with the 
	    /// handle specified. 
	    /// </summary>
	    /// <param name="handle">The numeric handle to give the new bone; must be unique within the Skeleton.</param>
	    /// <param name="translation">Initial translation offset of child relative to parent.</param>
	    /// <param name="rotate">Initial rotation relative to parent.</param>
	    /// <returns></returns>
        public Bone createChild(ushort handle, Vector3 translation, Quaternion rotate)
        {
            return bones.getObject(Bone_createChild(ogreNode, handle, translation, rotate));
        }

	    /// <summary>
	    /// Gets the numeric handle for this bone (unique within the skeleton).
	    /// </summary>
	    /// <returns>The numeric handle for the bone.</returns>
        public ushort getHandle()
        {
            return Bone_getHandle(ogreNode);
        }

	    /// <summary>
	    /// Sets the current position / orientation to be the 'binding pose' ie the layout in which 
	    /// bones were originally bound to a mesh.
	    /// </summary>
        public void setBindingPose()
        {
            Bone_setBindingPose(ogreNode);
        }

	    /// <summary>
	    /// Resets the position and orientation of this Bone to the original binding position.
	    /// 
        /// Bones are bound to the mesh in a binding pose. They are then modified from this position 
	    /// during animation. This method returns the bone to it's original position and orientation. 
	    /// </summary>
        public void reset()
        {
            Bone_reset(ogreNode);
        }

	    /// <summary>
	    /// Sets whether or not this bone is manually controlled.
	    /// 
        /// Manually controlled bones can be altered by the application at runtime, and their positions 
	    /// will not be reset by the animation routines. Note that you should also make sure that there 
	    /// are no AnimationTrack objects referencing this bone, or if there are, you should disable 
	    /// them using pAnimation->destroyTrack(pBone->getHandle()); 
	    /// </summary>
	    /// <param name="manuallyControlled">True to enable manual control.  False to disable it.</param>
        public void setManuallyControlled(bool manuallyControlled)
        {
            Bone_setManuallyControlled(ogreNode, manuallyControlled);
        }

	    /// <summary>
	    /// Determine if this bone is manually controlled.
	    /// </summary>
	    /// <returns>True if the bone is manually controlled.</returns>
        public bool isManuallyControlled()
        {
            return Bone_isManuallyControlled(ogreNode);
        }

	    /// <summary>
	    /// To be called in the event of transform changes to this node that require it's recalculation. 
	    /// </summary>
	    /// <param name="forceParentUpdate">Even if the node thinks it has already told it's parent, tell it anyway.</param>
        public void needUpdate(bool forceParentUpdate)
        {
            Bone_needUpdate(ogreNode, forceParentUpdate);
        }

	    /// <summary>
	    /// Returns the name of the bone. 
	    /// </summary>
	    /// <returns>The name of the bone.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(Bone_getName(ogreNode));
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Bone_createChild(IntPtr bone, ushort handle, Vector3 translation, Quaternion rotate);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Bone_getHandle(IntPtr bone);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Bone_setBindingPose(IntPtr bone);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Bone_reset(IntPtr bone);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Bone_setManuallyControlled(IntPtr bone, bool manuallyControlled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Bone_isManuallyControlled(IntPtr bone);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Bone_needUpdate(IntPtr bone, bool forceParentUpdate);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Bone_getName(IntPtr bone);
#endregion
    }
}
