using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class AnimationState : IDisposable
    {
        internal static AnimationState createWrapper(IntPtr nativeObject, object[] args)
        {
            return new AnimationState(nativeObject, args[0] as AnimationStateSet);
        }

        IntPtr animationState;
        AnimationStateSet parent;

        protected AnimationState(IntPtr animationState, AnimationStateSet parent)
        {
            this.animationState = animationState;
            this.parent = parent;
        }

        public void Dispose()
        {
            animationState = IntPtr.Zero;
        }

        public IntPtr OgreAnimationState
        {
            get
            {
                return animationState;
            }
        }

        /// <summary>
	    /// Get the name of the animation.
	    /// </summary>
	    /// <returns>The name of the animation.</returns>
        public String getAnimationName()
        {
            return Marshal.PtrToStringAnsi(AnimationState_getAnimationName(animationState));
        }

	    /// <summary>
	    /// Gets the time position for this animation. 
	    /// </summary>
	    /// <returns>The time position.</returns>
        public float getTimePosition()
        {
            return AnimationState_getTimePosition(animationState);
        }

	    /// <summary>
	    /// Sets the time position for this animation. 
	    /// </summary>
	    /// <param name="timePos">The time to set.</param>
        public void setTimePosition(float timePos)
        {
            AnimationState_setTimePosition(animationState, timePos);
        }

	    /// <summary>
	    /// Gets the total length of this animation (may be shorter than whole animation). 
	    /// </summary>
	    /// <returns>The length of the animation.</returns>
        public float getLength()
        {
            return AnimationState_getLength(animationState);
        }

	    /// <summary>
	    /// Sets the total length of this animation (may be shorter than whole animation).
	    /// </summary>
	    /// <param name="length">The length to set.</param>
        public void setLength(float length)
        {
            AnimationState_setLength(animationState, length);
        }

	    /// <summary>
	    /// Gets the weight (influence) of this animation.
	    /// </summary>
	    /// <returns>The weight of the animation.</returns>
        public float getWeight()
        {
            return AnimationState_getWeight(animationState);
        }

	    /// <summary>
	    /// Sets the weight (influence) of this animation.
	    /// </summary>
	    /// <param name="weight">The weight to set.</param>
        public void setWeight(float weight)
        {
            AnimationState_setWeight(animationState, weight);
        }

	    /// <summary>
	    /// Modifies the time position, adjusting for animation length. 
	    /// </summary>
	    /// <param name="offset">The offset to add.</param>
        public void addTime(float offset)
        {
            AnimationState_addTime(animationState, offset);
        }

	    /// <summary>
	    /// Returns true if the animation has reached the end and is not looping. 
	    /// </summary>
	    /// <returns>True if the animation is over and not looping, otherwise false.</returns>
        public bool hasEnded()
        {
            return AnimationState_hasEnded(animationState);
        }

	    /// <summary>
	    /// Returns true if this animation is currently enabled. 
	    /// </summary>
	    /// <returns>True if the animation is enabled.  False if it is disabled.</returns>
        public bool getEnabled()
        {
            return AnimationState_getEnabled(animationState);
        }

	    /// <summary>
	    /// Sets whether this animation is enabled. 
	    /// </summary>
	    /// <param name="enabled">True to enable the animation.  False to disable it.</param>
        public void setEnabled(bool enabled)
        {
            AnimationState_setEnabled(animationState, enabled);
        }

	    /// <summary>
	    /// Sets whether or not an animation loops at the start and end of the animation if the time 
	    /// continues to be altered. 
	    /// </summary>
	    /// <param name="loop">True if the animation loops.  False if it does not.</param>
        public void setLoop(bool loop)
        {
            AnimationState_setLoop(animationState, loop);
        }

	    /// <summary>
	    /// Gets whether or not this animation loops. 
	    /// </summary>
	    /// <returns>True if the animation loops false if it does not.</returns>
        public bool getLoop()
        {
            return AnimationState_getLoop(animationState);
        }

	    /// <summary>
	    /// Copies the states from another animation state, preserving the animation name (unlike 
	    /// operator=) but copying everything else.
	    /// </summary>
        /// <param name="copyState">The state to copy from.</param>
        public void copyStateFrom(AnimationState copyState)
        {
            AnimationState_copyStateFrom(animationState, copyState.animationState);
        }

	    /// <summary>
	    /// Create a new blend mask with the given number of entries In addition to assigning a single 
	    /// weight value to a skeletal animation, it may be desirable to assign animation weights per 
	    /// bone using a 'blend mask'.
	    /// </summary>
	    /// <param name="blendMaskSizeHint">The number of bones of the skeleton owning this AnimationState.</param>
	    /// <param name="initialWeight">The value all the blend mask entries will be initialised with (negative to skip initialisation).</param>
        public void createBlendMask(uint blendMaskSizeHint, float initialWeight)
        {
            AnimationState_createBlendMask(animationState, blendMaskSizeHint, initialWeight);
        }

	    /// <summary>
	    /// Destroy the currently set blend mask.
	    /// </summary>
        public void destroyBlendMask()
        {
            AnimationState_destroyBlendMask(animationState);
        }

	    /// <summary>
	    /// Determine if this animation has a blend mask.
	    /// </summary>
	    /// <returns>True if the animation has a blend mask set.  False if it does not.</returns>
        public bool hasBlendMask()
        {
            return AnimationState_hasBlendMask(animationState);
        }

	    /// <summary>
	    /// Set the weight for the bone identified by the given handle.
	    /// </summary>
	    /// <param name="boneHandle">The bone handle to set a blend mask for.</param>
	    /// <param name="weight">The weight of the blend mask.</param>
        public void setBlendMaskEntry(uint boneHandle, float weight)
        {
            AnimationState_setBlendMaskEntry(animationState, boneHandle, weight);
        }

	    /// <summary>
	    /// Get the current weight for the given bone.
	    /// </summary>
	    /// <param name="boneHandle">The bone handle to get a blend mask for.</param>
	    /// <returns>The weight of the bone.</returns>
        public float getBlendMaskEntry(uint boneHandle)
        {
            return AnimationState_getBlendMaskEntry(animationState, boneHandle);
        }

	    /// <summary>
	    /// Get the AnimationStateSet this AnimationState belongs to.
	    /// </summary>
	    /// <returns>The parent AnimationStateSet.</returns>
        public AnimationStateSet getParent()
        {
            return parent;
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr AnimationState_getAnimationName(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float AnimationState_getTimePosition(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setTimePosition(IntPtr animationState, float timePos);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float AnimationState_getLength(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setLength(IntPtr animationState, float length);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float AnimationState_getWeight(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setWeight(IntPtr animationState, float weight);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_addTime(IntPtr animationState, float offset);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationState_hasEnded(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationState_getEnabled(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setEnabled(IntPtr animationState, bool enabled);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setLoop(IntPtr animationState, bool loop);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationState_getLoop(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_copyStateFrom(IntPtr animationState, IntPtr copyState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_createBlendMask(IntPtr animationState, uint blendMaskSizeHint, float initialWeight);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_destroyBlendMask(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationState_hasBlendMask(IntPtr animationState);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationState_setBlendMaskEntry(IntPtr animationState, uint boneHandle, float weight);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float AnimationState_getBlendMaskEntry(IntPtr animationState, uint boneHandle);

#endregion
    }
}
