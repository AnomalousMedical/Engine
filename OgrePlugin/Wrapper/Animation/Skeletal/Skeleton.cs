using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [SingleEnum]
    public enum SkeletonAnimationBlendMode : uint
    {
	    ANIMBLEND_AVERAGE,
	    ANIMBLEND_CUMULATIVE
    };

    [NativeSubsystemType]
    public class Skeleton : IDisposable
    {
        protected IntPtr skeleton;
        private WrapperCollection<Animation> animations = new WrapperCollection<Animation>(Animation.createWrapper);
        private WrapperCollection<Bone> bones = new WrapperCollection<Bone>(Bone.createWrapper);

        public Skeleton(IntPtr skeleton)
        {
            this.skeleton = skeleton;
        }

        public virtual void Dispose()
        {
            skeleton = IntPtr.Zero;
            animations.Dispose();
            bones.Dispose();
        }

        /// <summary>
	    /// Creates a brand new Bone owned by this Skeleton. 
	    /// </summary>
	    /// <returns>The new bone.</returns>
        public Bone createBone()
        {
            return bones.getObject(Skeleton_createBone(skeleton));
        }

	    /// <summary>
	    /// Creates a brand new Bone owned by this Skeleton. 
	    /// </summary>
	    /// <param name="handle">The handle to give to this new bone - must be unique within this skeleton. You should also ensure that all bone handles are eventually contiguous (this is to simplify their compilation into an indexed array of transformation matrices). For this reason it is advised that you use the simpler createBone method which automatically assigns a sequential handle starting from 0. </param>
	    /// <returns>The new bone.</returns>
        public Bone createBone(ushort handle)
        {
            return bones.getObject(Skeleton_createBoneHandle(skeleton, handle));
        }

	    /// <summary>
	    /// Creates a brand new Bone owned by this Skeleton.
	    /// This method creates an unattached new Bone for this skeleton. Unless this is to be a root 
	    /// bone (there may be more than one of these), you must attach it to another Bone in the 
	    /// skeleton using addChild for it to be any use. For this reason you will likely be better off 
	    /// creating child bones using the Bone::createChild method instead, once you have created the 
	    /// root bone. 
	    ///
        /// Note that this method automatically generates a handle for the bone, which you can retrieve 
	    /// using Bone::getHandle. If you wish the new Bone to have a specific handle, use the alternate 
	    /// form of this method which takes a handle as a parameter, although you should note the 
	    /// restrictions. 
	    /// </summary>
	    /// <param name="name">The name to give to this new bone - must be unique within this skeleton. Note that the way OGRE looks up bones is via a numeric handle, so if you name a Bone this way it will be given an automatic sequential handle. The name is just for your convenience, although it is recommended that you only use the handle to retrieve the bone in performance-critical code.</param>
	    /// <returns>The new bone.</returns>
        public Bone createBone(String name)
        {
            return bones.getObject(Skeleton_createBoneName(skeleton, name));
        }

	    /// <summary>
	    /// Creates a brand new Bone owned by this Skeleton. 
	    /// </summary>
	    /// <param name="name">The name to give to this new bone - must be unique within this skeleton. Note that the way OGRE looks up bones is via a numeric handle, so if you name a Bone this way it will be given an automatic sequential handle. The name is just for your convenience, although it is recommended that you only use the handle to retrieve the bone in performance-critical code.</param>
	    /// <param name="handle">The handle to give to this new bone - must be unique within this skeleton. You should also ensure that all bone handles are eventually contiguous (this is to simplify their compilation into an indexed array of transformation matrices). For this reason it is advised that you use the simpler createBone method which automatically assigns a sequential handle starting from 0.</param>
	    /// <returns>The new bone.</returns>
        public Bone createBone(String name, ushort handle)
        {
            return bones.getObject(Skeleton_createBoneNameHandle(skeleton, name, handle));
        }

	    /// <summary>
	    /// Returns the number of bones in this skeleton. 
	    /// </summary>
	    /// <returns>The number of bones in the skeleton.</returns>
        public ushort getNumBones()
        {
            return Skeleton_getNumBones(skeleton);
        }

	    /// <summary>
	    /// Get an iterator over the root bones in the skeleton, ie those with no parents.  Due to 
	    /// the nature of a wrapper a new List instance is created every time this function is called.
	    /// </summary>
	    /// <returns>An enumerator over the root bones.</returns>
        public IEnumerator<Bone> getRootBoneIterator()
        {
            BoneIterator boneIter = new BoneIterator(bones);
            Skeleton_getRootBoneIterator(skeleton, boneIter.boneFound);
            return boneIter.GetEnumerator();
        }

	    /// <summary>
	    /// Get an iterator over all the bones in the skeleton.  Due to the nature of a wrapper a new 
	    /// List instance is created every time this function is called.
	    /// </summary>
	    /// <returns>An enumerator over all the bones.</returns>
        public IEnumerator<Bone> getBoneIterator()
        {
            BoneIterator boneIter = new BoneIterator(bones);
            Skeleton_getBoneIterator(skeleton, boneIter.boneFound);
            return boneIter.GetEnumerator();
        }

	    /// <summary>
	    /// Gets a bone by it's handle. 
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>The bone or null if the bone is not found.</returns>
        public Bone getBone(ushort handle)
        {
            return bones.getObject(Skeleton_getBoneHandle(skeleton, handle));
        }

	    /// <summary>
	    /// Gets a bone by it's name. 
	    /// </summary>
	    /// <param name="name">The name of the bone to search for.</param>
	    /// <returns>The bone or null if it does not exist.</returns>
        public Bone getBone(String name)
        {
            return bones.getObject(Skeleton_getBoneName(skeleton, name));
        }

	    /// <summary>
	    /// Returns whether this skeleton contains the named bone.
	    /// </summary>
	    /// <param name="name">The name of the bone to search for.</param>
	    /// <returns>True if the skeleton contains the bone. False if it does not.</returns>
        public bool hasBone(String name)
        {
            return Skeleton_hasBone(skeleton, name);
        }

	    /// <summary>
	    /// Sets the current position / orientation to be the 'binding pose' i.e. the layout in which 
	    /// bones were originally bound to a mesh. 
	    /// </summary>
        public void setBindingPose()
        {
            Skeleton_setBindingPose(skeleton);
        }

	    /// <summary>
	    /// Resets the position and orientation of all bones in this skeleton to their original binding 
	    /// position. 
	    /// </summary>
        public void reset()
        {
            Skeleton_reset(skeleton);
        }

	    /// <summary>
	    /// Resets the position and orientation of all bones in this skeleton to their original binding 
	    /// position. 
	    /// </summary>
	    /// <param name="resetManualBones">If set to true, causes the state of manual bones to be reset too, which is normally not done to allow the manual state to persist even when keyframe animation is applied.</param>
        public void reset(bool resetManualBones)
        {
            Skeleton_resetResetManual(skeleton, resetManualBones);
        }

	    /// <summary>
	    /// Creates a new Animation object for animating this skeleton. 
	    /// </summary>
	    /// <param name="name">The name of this animation.</param>
	    /// <param name="length">The length of the animation in seconds.</param>
	    /// <returns></returns>
        public Animation createAnimation(String name, float length)
        {
            return animations.getObject(Skeleton_createAnimation(skeleton, name, length));
        }

	    /// <summary>
	    /// Returns the named Animation object. 
	    /// </summary>
	    /// <param name="name">The name of the animation </param>
	    /// <returns>The animation matching the name.</returns>
        public Animation getAnimation(String name)
        {
            return animations.getObject(Skeleton_getAnimation(skeleton, name));
        }

	    /// <summary>
	    /// Returns whether this skeleton contains the named animation. 
	    /// </summary>
	    /// <param name="name">The name of the animation.</param>
	    /// <returns>True if the skeleton has the animation.  False if it does not.</returns>
        public bool hasAnimation(String name)
        {
            return Skeleton_hasAnimation(skeleton, name);
        }

	    /// <summary>
	    /// Removes an Animation from this skeleton. 
	    /// </summary>
	    /// <param name="name">The name of the animation to remove.</param>
        public void removeAnimation(String name)
        {
            animations.destroyObject(Skeleton_getAnimation(skeleton, name));
            Skeleton_removeAnimation(skeleton, name);
        }

	    /// <summary>
	    /// Changes the state of the skeleton to reflect the application of the
        /// passed in collection of animations.
	    /// <para>
        /// Animating a skeleton involves both interpolating between keyframes of a
        /// specific animation, and blending between the animations themselves.
        /// Calling this method sets the state of the skeleton so that it reflects
        /// the combination of all the passed in animations, at the time index
        /// specified for each, using the weights specified. Note that the weights
        /// between animations do not have to sum to 1.0, because some animations
        /// may affect only subsets of the skeleton. If the weights exceed 1.0 for
        /// the same area of the skeleton, the movement will just be exaggerated. 
	    /// </para>
	    /// </summary>
	    /// <param name="animSet">The animation state to set.</param>
        public void setAnimationState(AnimationStateSet animSet)
        {
            Skeleton_setAnimationState(skeleton, animSet.OgreObject);
        }

	    /// <summary>
	    /// Gets the number of animations on this skeleton. 
	    /// </summary>
	    /// <returns>The number of animations.</returns>
        public ushort getNumAnimations()
        {
            return Skeleton_getNumAnimations(skeleton);
        }

	    /// <summary>
	    /// Gets the animation blending mode which this skeleton will use. 
	    /// </summary>
	    /// <returns>The blending mode.</returns>
        public SkeletonAnimationBlendMode getBlendMode()
        {
            return Skeleton_getBlendMode(skeleton);
        }

	    /// <summary>
	    /// Sets the animation blending mode this skeleton will use.
	    /// </summary>
	    /// <param name="blendMode">The blend mode to set.</param>
        public void setBlendMode(SkeletonAnimationBlendMode blendMode)
        {
            Skeleton_setBlendMode(skeleton, blendMode);
        }

	    /// <summary>
	    /// Optimise all of this skeleton's animations.
	    /// </summary>
	    /// <param name="preservingIdentityNodeTracks">If true, don't destroy identity node tracks.</param>
        public void optimizeAllAnimations(bool preservingIdentityNodeTracks)
        {
            Skeleton_optimizeAllAnimations(skeleton, preservingIdentityNodeTracks);
        }

	    /// <summary>
	    /// Have manual bones been modified since the skeleton was last updated?
	    /// </summary>
	    /// <returns>True if the bones were updated.  False if they were not.</returns>
        public bool getManualBonesDirty()
        {
            return Skeleton_getManualBonesDirty(skeleton);
        }

	    /// <summary>
	    /// Are there any manually controlled bones? 
	    /// </summary>
	    /// <returns>True if there are manually controlled bones.  False if there are not.</returns>
        public bool hasManualBones()
        {
            return Skeleton_hasManualBones(skeleton);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_createBone(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_createBoneHandle(IntPtr skeleton, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_createBoneName(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_createBoneNameHandle(IntPtr skeleton, String name, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern ushort Skeleton_getNumBones(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern IEnumerator<Bone> Skeleton_getRootBoneIterator(IntPtr skeleton, BoneIterator.BoneFoundCallback boneFound);

        [DllImport("OgreCWrapper")]
        private static extern IEnumerator<Bone> Skeleton_getBoneIterator(IntPtr skeleton, BoneIterator.BoneFoundCallback boneFound);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_getBoneHandle(IntPtr skeleton, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_getBoneName(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool Skeleton_hasBone(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_setBindingPose(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_reset(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_resetResetManual(IntPtr skeleton, bool resetManualBones);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_createAnimation(IntPtr skeleton, String name, float length);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Skeleton_getAnimation(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool Skeleton_hasAnimation(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_removeAnimation(IntPtr skeleton, String name);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_setAnimationState(IntPtr skeleton, IntPtr animSet);

        [DllImport("OgreCWrapper")]
        private static extern ushort Skeleton_getNumAnimations(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern SkeletonAnimationBlendMode Skeleton_getBlendMode(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_setBlendMode(IntPtr skeleton, SkeletonAnimationBlendMode blendMode);

        [DllImport("OgreCWrapper")]
        private static extern void Skeleton_optimizeAllAnimations(IntPtr skeleton, bool preservingIdentityNodeTracks);

        [DllImport("OgreCWrapper")]
        private static extern bool Skeleton_getManualBonesDirty(IntPtr skeleton);

        [DllImport("OgreCWrapper")]
        private static extern bool Skeleton_hasManualBones(IntPtr skeleton);

#endregion
    }
}
