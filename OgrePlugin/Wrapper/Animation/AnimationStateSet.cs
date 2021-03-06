﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class AnimationStateSet : IDisposable
    {
        private IntPtr animationStateSet;
        private WrapperCollection<AnimationState> states = new WrapperCollection<AnimationState>(AnimationState.createWrapper);

        public AnimationStateSet(IntPtr animationStateSet)
        {
            this.animationStateSet = animationStateSet;
        }

        public void Dispose()
        {
            states.Dispose();
            animationStateSet = IntPtr.Zero;
        }

        internal IntPtr OgreObject 
        { 
            get
            {
                return animationStateSet;
            }
        }

        /// <summary>
	    /// Create a new AnimationState instance. 
	    /// </summary>
	    /// <param name="animName">The name of the animation.</param>
	    /// <param name="timePos">Starting time position.</param>
	    /// <param name="length">Length of the animation to play.</param>
	    /// <param name="weight">Weight to apply the animation with.</param>
	    /// <param name="enabled">Whether the animation is enabled.</param>
	    /// <returns>A new AnimationState with the given parameters.</returns>
        public AnimationState createAnimationState(String animName, float timePos, float length, float weight, bool enabled)
        {
            return states.getObject(AnimationStateSet_createAnimationState(animationStateSet, animName, timePos, length, weight, enabled), this);
        }

	    /// <summary>
	    /// Get an animation state by the name of the animation. 
	    /// </summary>
	    /// <param name="name">The name of the state to search for.</param>
	    /// <returns>The animation state specified by name or null if it does not exist.</returns>
        public AnimationState getAnimationState(String name)
        {
            return states.getObject(AnimationStateSet_getAnimationState(animationStateSet, name), this);
        }

	    /// <summary>
	    /// Tests if state for the named animation is present. 
	    /// </summary>
	    /// <param name="name">The state to search for.</param>
	    /// <returns>True if the animation state is found.  False if it is not.</returns>
        public bool hasAnimationState(String name)
        {
            return AnimationStateSet_hasAnimationState(animationStateSet, name);
        }

	    /// <summary>
	    /// Remove animation state with the given name.
	    /// </summary>
	    /// <param name="name">The name of the state to remove.</param>
        public void removeAnimationState(String name)
        {
            states.destroyObject(AnimationStateSet_getAnimationState(animationStateSet, name));
            AnimationStateSet_removeAnimationState(animationStateSet, name);
        }

	    /// <summary>
	    /// Remove all animation states. 
	    /// </summary>
        public void removeAllAnimationStates()
        {
            states.clearObjects();
            AnimationStateSet_removeAllAnimationStates(animationStateSet);
        }

	    /// <summary>
	    /// Copy the state of any matching animation states from this to another. 
	    /// </summary>
	    /// <param name="target">The AnimationStateSet to copy states to.</param>
        public void copyMatchingState(AnimationStateSet target)
        {
            AnimationStateSet_copyMatchingState(animationStateSet, target.animationStateSet);
        }

	    /// <summary>
	    /// Get the latest animation state been altered frame number.
	    /// </summary>
	    /// <returns>The last frame the state was modified.</returns>
        public uint getDirtyFrameNumber()
        {
            return AnimationStateSet_getDirtyFrameNumber(animationStateSet);
        }

	    /// <summary>
	    /// Tests if exists enabled animation state in this set. (Ogre Comment).
	    /// </summary>
	    /// <returns>Uh I dunno.</returns>
        public bool hasEnabledAnimationState()
        {
            return AnimationStateSet_hasEnabledAnimationState(animationStateSet);
        }

	    /// <summary>
	    /// Notify that this animation state set is dirty.
	    /// </summary>
        public void notifyDirty()
        {
            AnimationStateSet_notifyDirty(animationStateSet);
        }

        public AnimationStateIterator AnimationStates
        {
            get
            {
                return new AnimationStateIterator(this, AnimationStateSet_getAnimationStateIterator(animationStateSet));
            }
        }

        /// <summary>
        /// Get an animation state from a given pointer. Used by iterators to get the states in this collection.
        /// </summary>
        /// <param name="animationStatePointer"></param>
        /// <returns></returns>
        internal AnimationState getStateFromPointer(IntPtr animationStatePointer)
        {
            return states.getObject(animationStatePointer, this);
        }

        internal void destroyIterator(IntPtr animationIteratorPointer)
        {
            AnimationStateSet_iteratorDelete(animationIteratorPointer);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr AnimationStateSet_createAnimationState(IntPtr animationStateSet, String animName, float timePos, float length, float weight, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr AnimationStateSet_getAnimationState(IntPtr animationStateSet, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationStateSet_hasAnimationState(IntPtr animationStateSet, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateSet_removeAnimationState(IntPtr animationStateSet, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateSet_removeAllAnimationStates(IntPtr animationStateSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateSet_copyMatchingState(IntPtr animationStateSet, IntPtr target);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint AnimationStateSet_getDirtyFrameNumber(IntPtr animationStateSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationStateSet_hasEnabledAnimationState(IntPtr animationStateSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateSet_notifyDirty(IntPtr animationStateSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr AnimationStateSet_getAnimationStateIterator(IntPtr animationStateSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateSet_iteratorDelete(IntPtr iter);

#endregion
    }
}
