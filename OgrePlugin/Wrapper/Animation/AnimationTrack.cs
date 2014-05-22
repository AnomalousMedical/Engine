using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public enum VertexAnimationType : uint
    {
	    /// <summary>
	    /// No animation
	    /// </summary>
	    VAT_NONE,
	    /// <summary>
	    /// Morph animation is made up of many interpolated snapshot keyframes
	    /// </summary>
	    VAT_MORPH = 1,
	    /// <summary>
	    /// Pose animation is made up of a single delta pose keyframe
	    /// </summary>
	    VAT_POSE = 2
    };

    [NativeSubsystemType]
    public abstract class AnimationTrack : IDisposable
    {
        protected IntPtr animationTrack;
        private Animation parent;

        protected AnimationTrack(IntPtr animationTrack, Animation parent)
        {
            this.animationTrack = animationTrack;
            this.parent = parent;
        }

        public virtual void Dispose()
        {
            animationTrack = IntPtr.Zero;
        }

        /// <summary>
	    /// Method to determine if this track has any KeyFrames which are doing
        /// anything useful - can be used to determine if this track can be
        /// optimised out.
	    /// </summary>
	    /// <returns>True if non zero key frames exist.</returns>
        public bool hasNonZeroKeyFrames()
        {
            return AnimationTrack_hasNonZeroKeyFrames(animationTrack);
        }

	    /// <summary>
	    /// Optimise the current track by removing any duplicate keyframes. 
	    /// </summary>
        public void optimize()
        {
            AnimationTrack_optimize(animationTrack);
        }

	    /// <summary>
	    /// Get the handle associated with this track. 
	    /// </summary>
	    /// <returns>This track's handle.</returns>
        public ushort getHandle()
        {
            return AnimationTrack_getHandle(animationTrack);
        }

	    /// <summary>
	    /// Returns the number of keyframes in this animation. 
	    /// </summary>
	    /// <returns>The number of key frames.</returns>
        public ushort getNumKeyFrames()
        {
            return AnimationTrack_getNumKeyFrames(animationTrack);
        }

	    /// <summary>
	    /// Returns the KeyFrame at the specified index. 
	    /// </summary>
	    /// <param name="index">The index of the key frame.</param>
	    /// <returns>The key frame at the given index, or null if the index does not exist.</returns>
        public abstract KeyFrame getKeyFrame(ushort index);

	    /// <summary>
	    /// <para>
	    /// Gets the 2 KeyFrame objects which are active at the time given, and the
        /// blend value between them.
	    /// </para>
	    /// <para>
        /// At any point in time in an animation, there are either 1 or 2 keyframes
        /// which are 'active', 1 if the time index is exactly on a keyframe, 2 at
        /// all other times i.e. the keyframe before and the keyframe after. 
	    /// </para>
	    /// <para>
        /// This method returns those keyframes given a time index, and also returns
        /// a parametric value indicating the value of 't' representing where the
        /// time index falls between them. E.g. if it returns 0, the time index is
        /// exactly on keyFrame1, if it returns 0.5 it is half way between keyFrame1
        /// and keyFrame2 etc. 
	    /// </para>
	    /// </summary>
	    /// <param name="timeIndex">The time index.</param>
	    /// <param name="keyFrame1">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just before or at this time index.</param>
	    /// <param name="keyFrame2">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just after this time index.</param>
	    /// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0.</returns>
        public abstract float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2);

	    /// <summary>
	    /// Gets the 2 KeyFrame objects which are active at the time given, and the
        /// blend value between them.
	    /// <para>
        /// At any point in time in an animation, there are either 1 or 2 keyframes
        /// which are 'active', 1 if the time index is exactly on a keyframe, 2 at
        /// all other times i.e. the keyframe before and the keyframe after. 
	    /// </para>
	    /// <para>
        /// This method returns those keyframes given a time index, and also returns
        /// a parametric value indicating the value of 't' representing where the
        /// time index falls between them. E.g. if it returns 0, the time index is
        /// exactly on keyFrame1, if it returns 0.5 it is half way between keyFrame1
        /// and keyFrame2 etc. 
	    /// </para>
	    /// </summary>
	    /// <param name="timeIndex">The time index.</param>
	    /// <param name="keyFrame1">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just before or at this time index.</param>
	    /// <param name="keyFrame2">Pointer to a KeyFrame pointer which will receive the pointer to the keyframe just after this time index.</param>
	    /// <returns>Parametric value indicating how far along the gap between the 2 keyframes the timeIndex value is, e.g. 0.0 for exactly at 1, 0.25 for a quarter etc. By definition the range of this value is: 0.0 &lt;= returnValue &lt; 1.0.</returns>
        public abstract float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2, out ushort firstKeyIndex);

	    /// <summary>
	    /// Creates a new KeyFrame and adds it to this animation at the given time
        /// index.
	    /// <para>
        /// It is better to create KeyFrames in time order. Creating them out of
        /// order can result in expensive reordering processing. Note that a
        /// KeyFrame at time index 0.0 is always created for you, so you don't need
        /// to create this one, just access it using getKeyFrame(0); 
	    /// </para>
	    /// </summary>
	    /// <param name="timePos">The time from which this KeyFrame will apply.</param>
	    /// <returns>The new KeyFrame.</returns>
        public abstract KeyFrame createKeyFrame(float timePos);

	    /// <summary>
	    /// Removes a KeyFrame by it's index. 
	    /// </summary>
	    /// <param name="index">The index of the key frame to remove.</param>
        public abstract void removeKeyFrame(ushort index);

	    /// <summary>
	    /// Removes all the KeyFrames from this track. 
	    /// </summary>
        public abstract void removeAllKeyFrames();

	    /// <summary>
	    /// Returns the parent Animation object for this track. 
	    /// </summary>
	    /// <returns>The parent animation.</returns>
        public Animation getParent()
        {
            return parent;
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationTrack_hasNonZeroKeyFrames(IntPtr animationTrack);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationTrack_optimize(IntPtr animationTrack);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort AnimationTrack_getHandle(IntPtr animationTrack);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort AnimationTrack_getNumKeyFrames(IntPtr animationTrack);

#endregion
    }
}
