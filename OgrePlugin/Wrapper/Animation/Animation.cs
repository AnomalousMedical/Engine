using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class Animation : IDisposable
    {

        /// <summary>
        /// The types of animation interpolation available. 
        /// </summary>
        public enum InterpolationMode : uint
        {
            /// <summary>
            /// Values are interpolated along straight lines. 
            /// </summary>
            IM_LINEAR,
            /// <summary>
            /// Values are interpolated along a spline, resulting in smoother changes in direction.
            /// </summary>
            IM_SPLINE
        };

        /// <summary>
        /// The types of rotational interpolation available. 
        /// </summary>
        public enum RotationInterpolationMode : uint
        {
            /// <summary>
            /// Values are interpolated linearly. This is faster but does not 
            /// necessarily give a completely accurate result.
            /// </summary>
            RIM_LINEAR,
            /// <summary>
            /// Values are interpolated spherically. This is more accurate but
            /// has a higher cost.
            /// </summary>
            RIM_SPHERICAL
        };

        protected IntPtr animation;
        private WrapperCollection<NodeAnimationTrack> nodeAnimations = new WrapperCollection<NodeAnimationTrack>(NodeAnimationTrack.createWrapper);
        private WrapperCollection<NumericAnimationTrack> numericAnimations = new WrapperCollection<NumericAnimationTrack>(NumericAnimationTrack.createWrapper);
        private WrapperCollection<VertexAnimationTrack> vertexAnimations = new WrapperCollection<VertexAnimationTrack>(VertexAnimationTrack.createWrapper);

        internal static Animation createWrapper(IntPtr nativePtr, object[] args)
        {
            return new Animation(nativePtr);
        }

        protected Animation(IntPtr animation)
        {
            this.animation = animation;
        }

        public void Dispose()
        {
            nodeAnimations.Dispose();
            numericAnimations.Dispose();
            vertexAnimations.Dispose();
            animation = IntPtr.Zero;
        }

        /// <summary>
	    /// Gets the name of this animation. 
	    /// </summary>
	    /// <returns>The name of the animation.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(Animation_getName(animation));
        }

	    /// <summary>
	    /// Gets the total length of the animation. 
	    /// </summary>
	    /// <returns>The length of the animation.</returns>
        public float getLength()
        {
            return Animation_getLength(animation);
        }

	    /// <summary>
	    /// Creates a NodeAnimationTrack for animating a Node. 
	    /// </summary>
	    /// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation. </param>
	    /// <returns>The newly created NodeAnimationTrack or null if an error occured such as the track already being defined.</returns>
        public NodeAnimationTrack createNodeTrack(ushort handle)
        {
            return nodeAnimations.getObject(Animation_createNodeTrack(animation, handle), this);
        }

	    /// <summary>
	    /// Creates a NumericAnimationTrack for animating any numeric value. 
	    /// </summary>
	    /// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation.</param>
	    /// <returns>The newly created NumericAnimationTrack or null if an error occured such as the track already being defined.</returns>
        public NumericAnimationTrack createNumericTrack(ushort handle)
        {
            return numericAnimations.getObject(Animation_createNumericTrack(animation, handle), this);
        }

	    /// <summary>
	    /// Creates a VertexAnimationTrack for animating vertex position data.
	    /// </summary>
	    /// <param name="handle">Handle to give the track, used for accessing the track later. Must be unique within this Animation, and is used to identify the target. For example when applied to a Mesh, the handle must reference the index of the geometry being modified; 0 for the shared geometry, and 1+ for SubMesh geometry with the same index-1.</param>
	    /// <param name="animType">Either morph or pose animation.</param>
	    /// <returns>The newly created VertexAnimationTrack or null if an error occured such as the track already being defined.</returns>
        public VertexAnimationTrack createVertexTrack(ushort handle, VertexAnimationType animType)
        {
            return vertexAnimations.getObject(Animation_createVertexTrack(animation, handle, animType), this);
        }

	    /// <summary>
	    /// Gets the number of NodeAnimationTrack objects contained in this animation.
	    /// </summary>
	    /// <returns>The number of node animations.</returns>
        public ushort getNumNodeTracks()
        {
            return Animation_getNumNodeTracks(animation);
        }

	    /// <summary>
	    /// Gets a node track by it's handle.
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>The node matching the handle or null if the node is not found.</returns>
        public NodeAnimationTrack getNodeTrack(ushort handle)
        {
            return nodeAnimations.getObject(Animation_getNodeTrack(animation, handle), this);
        }

	    /// <summary>
	    /// Check to see if a node track exists with the given handle.
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>True if a track exists with the given handle.</returns>
        public bool hasNodeTrack(ushort handle)
        {
            return Animation_hasNodeTrack(animation, handle);
        }

	    /// <summary>
	    /// Gets the number of NumericAnimationTrack objects contained in this animation.
	    /// </summary>
	    /// <returns>The number of tracks.</returns>
        public ushort getNumNumericTracks()
        {
            return Animation_getNumNumericTracks(animation);
        }

	    /// <summary>
	    /// Gets a numeric track by it's handle.
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>The node matching the handle or null if the node is not found.</returns>
        public NumericAnimationTrack getNumericTrack(ushort handle)
        {
            return numericAnimations.getObject(Animation_getNumericTrack(animation, handle), this);
        }

	    /// <summary>
	    /// Check to see if a numeric track exists with the given handle.
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>True if a track exists with the given handle.</returns>
        public bool hasNumericTrack(ushort handle)
        {
            return Animation_hasNumericTrack(animation, handle);
        }

	    /// <summary>
	    /// Gets the number of VertexAnimationTrack objects contained in this animation.
	    /// </summary>
	    /// <returns>The number of vertex tracks.</returns>
        public ushort getNumVertexTracks()
        {
            return Animation_getNumVertexTracks(animation);
        }

	    /// <summary>
	    /// Gets a Vertex track by it's handle. 
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>The node matching the handle or null if the node is not found.</returns>
        public VertexAnimationTrack getVertexTrack(ushort handle)
        {
            return vertexAnimations.getObject(Animation_getVertexTrack(animation, handle), this);
        }

	    /// <summary>
	    /// Check to see if a vertex track exists for the given handle.
	    /// </summary>
	    /// <param name="handle">The handle to search for.</param>
	    /// <returns>True if a track exists with the given handle.</returns>
        public bool hasVertexTrack(ushort handle)
        {
            return Animation_hasVertexTrack(animation, handle);
        }

	    /// <summary>
	    /// Destroys the node track with the given handle.
	    /// </summary>
	    /// <param name="handle">The node track to destroy.</param>
        public void destroyNodeTrack(ushort handle)
        {
            nodeAnimations.destroyObject(Animation_getNodeTrack(animation, handle));
            Animation_destroyNodeTrack(animation, handle);
        }

	    /// <summary>
	    /// Destroys the numeric track with the given handle. 
	    /// </summary>
	    /// <param name="handle">The numeric track to destroy.</param>
        public void destroyNumericTrack(ushort handle)
        {
            numericAnimations.destroyObject(Animation_getNumericTrack(animation, handle));
            Animation_destroyNumericTrack(animation, handle);
        }

	    /// <summary>
	    /// Destroys the Vertex track with the given handle. 
	    /// </summary>
	    /// <param name="handle">The vertex track to destroy.</param>
        public void destroyVertexTrack(ushort handle)
        {
            vertexAnimations.destroyObject(Animation_getVertexTrack(animation, handle));
            Animation_destroyVertexTrack(animation, handle);
        }

	    /// <summary>
	    /// Removes and destroys all tracks making up this animation. 
	    /// </summary>
        public void destroyAllTracks()
        {
            nodeAnimations.clearObjects();
            vertexAnimations.clearObjects();
            numericAnimations.clearObjects();
            Animation_destroyAllTracks(animation);
        }

	    /// <summary>
	    /// Removes and destroys all node tracks making up this animation. 
	    /// </summary>
        public void destroyAllNodeTracks()
        {
            nodeAnimations.clearObjects();
            Animation_destroyAllNodeTracks(animation);
        }

	    /// <summary>
	    /// Removes and destroys all numeric tracks making up this animation. 
	    /// </summary>
        public void destroyAllNumericTracks()
        {
            numericAnimations.clearObjects();
            Animation_destroyAllNumericTracks(animation);
        }

	    /// <summary>
	    /// Removes and destroys all vertex tracks making up this animation. 
	    /// </summary>
        public void destroyAllVertexTracks()
        {
            vertexAnimations.clearObjects();
            Animation_destroyAllVertexTracks(animation);
        }

	    /// <summary>
	    /// Applies an animation given a specific time point and weight. 
	    /// </summary>
	    /// <param name="timePos">The time position in the animation to apply. </param>
        public void apply(float timePos)
        {
            Animation_apply1(animation, timePos);
        }

	    /// <summary>
	    /// Applies an animation given a specific time point and weight. 
	    /// </summary>
	    /// <param name="timePos">The time position in the animation to apply. </param>
	    /// <param name="wieght">The influence to give to this track, 1.0 for full influence, less to blend with other animations. </param>
        public void apply(float timePos, float wieght)
        {
            Animation_apply2(animation, timePos, wieght);
        }

	    /// <summary>
	    /// Applies an animation given a specific time point and weight. 
	    /// </summary>
	    /// <param name="timePos">The time position in the animation to apply. </param>
	    /// <param name="wieght">The influence to give to this track, 1.0 for full influence, less to blend with other animations. </param>
	    /// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
        public void apply(float timePos, float wieght, float scale)
        {
            Animation_apply3(animation, timePos, wieght, scale);
        }

	    /// <summary>
	    /// Applies all node tracks given a specific time point and weight to a
        /// given skeleton.
	    /// <para>
	    /// Where you have associated animation tracks with Node objects, you can
        /// easily apply an animation to those nodes by calling this method. 
	    /// </para>
	    /// </summary>
	    /// <param name="skeleton">The skeleton to apply the animation to.</param>
	    /// <param name="timePos">The time position in the animation to apply. </param>
        public void apply(Skeleton skeleton, float timePos)
        {
            Animation_apply4(animation, skeleton.OgreSkeleton, timePos);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="skeleton">The skeleton to apply the animation to.</param>
	    /// <param name="timePos">The time position in the animation to apply. </param>
	    /// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
        public void apply(Skeleton skeleton, float timePos, float weight)
        {
            Animation_apply5(animation, skeleton.OgreSkeleton, timePos, weight);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="skeleton">The skeleton to apply the animation to.</param>
	    /// <param name="timePos">The time position in the animation to apply. </param>
	    /// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	    /// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
        public void apply(Skeleton skeleton, float timePos, float weight, float scale)
        {
            Animation_apply6(animation, skeleton.OgreSkeleton, timePos, weight, scale);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="skeleton">The skeleton to apply the animation to.</param>
	    /// <param name="timePos">The time position in the animation to apply.</param>
	    /// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	    /// <param name="blendMask">The influence array defining additional per bone weights. These will be modulated with the weight factor.</param>
	    /// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target. </param>
        public void apply(Skeleton skeleton, float timePos, float weight, float[] blendMask, float scale)
        {
            unsafe
            {
                fixed(float* bmPtr = &blendMask[0])
                {
                    Animation_apply7(animation, skeleton.OgreSkeleton, timePos, weight, bmPtr, blendMask.Length, scale);
                }
            }
        }

	    /// <summary>
	    /// Applies all vertex tracks given a specific time point and weight to a given entity.
	    /// </summary>
	    /// <param name="entity">The Entity to which this animation should be applied.</param>
	    /// <param name="timePos">The time position in the animation to apply.</param>
	    /// <param name="weight">The weight at which the animation should be applied (only affects pose animation).</param>
	    /// <param name="software">Whether to populate the software morph vertex data.</param>
	    /// <param name="hardware">Whether to populate the hardware morph vertex data.</param>
        public void apply(Entity entity, float timePos, float weight, bool software, bool hardware)
        {
            Animation_apply8(animation, entity.OgreObject, timePos, weight, software, hardware);
        }

	    /// <summary>
	    /// Tells the animation how to interpolate between keyframes.
	    /// <para>
        /// By default, animations normally interpolate linearly between keyframes.
        /// This is fast, but when animations include quick changes in direction it
        /// can look a little unnatural because directions change instantly at
        /// keyframes. An alternative is to tell the animation to interpolate along
        /// a spline, which is more expensive in terms of calculation time, but
        /// looks smoother because major changes in direction are distributed around
        /// the keyframes rather than just at the keyframe. 
	    /// </para>
	    /// </summary>
	    /// <param name="im">The mode to set.</param>
        public void setInterpolationMode(InterpolationMode im)
        {
            Animation_setInterpolationMode(animation, im);
        }

	    /// <summary>
	    /// Gets the current interpolation mode of this animation. 
	    /// </summary>
	    /// <returns>The current interpolation mode.</returns>
        public InterpolationMode getInterpolationMode()
        {
            return Animation_getInterpolationMode(animation);
        }

	    /// <summary>
	    /// Tells the animation how to interpolate rotations.
	    /// <para>
	    /// By default, animations interpolate linearly between rotations. This is
        /// fast but not necessarily completely accurate. If you want more accurate
        /// interpolation, use spherical interpolation, but be aware that it will
        /// incur a higher cost. 
	    /// </para>
	    /// </summary>
	    /// <param name="im">The interpolation mode to set.</param>
        public void setRotationInterpolationMode(RotationInterpolationMode im)
        {
            Animation_setRotationInterpolationMode(animation, im);
        }

	    /// <summary>
	    /// Gets the current rotation interpolation mode of this animation.
	    /// </summary>
	    /// <returns>The current rotation interpolation mode.</returns>
        public RotationInterpolationMode getRotationInterpolationMode()
        {
            return Animation_getRotationInterpolationMode(animation);
        }

	    /// <summary>
	    /// Optimise an animation by removing unnecessary tracks and keyframes.
	    /// 
        /// When you export an animation, it is possible that certain tracks have
        /// been keyframed but actually don't include anything useful - the
        /// keyframes include no transformation. These tracks can be completely
        /// eliminated from the animation and thus speed up the animation. In
        /// addition, if several keyframes in a row have the same value, then they
        /// are just adding overhead and can be removed. 
	    /// 
        /// Since track-less and identity track has difference behavior for
        /// accumulate animation blending if corresponding track presenting at other
        /// animation that is non-identity, and in normally this method didn't known
        /// about the situation of other animation, it can't deciding whether or not
        /// discards identity tracks. So there have a parameter allow you choose
        /// what you want, in case you aren't sure how to do that, you should use
        /// Skeleton::optimiseAllAnimations instead. 
	    /// </summary>
        public void optimize()
        {
            Animation_optimize1(animation);
        }

	    /// <summary>
	    /// Optimise an animation by removing unnecessary tracks and keyframes.
	    /// 
        /// When you export an animation, it is possible that certain tracks have
        /// been keyframed but actually don't include anything useful - the
        /// keyframes include no transformation. These tracks can be completely
        /// eliminated from the animation and thus speed up the animation. In
        /// addition, if several keyframes in a row have the same value, then they
        /// are just adding overhead and can be removed. 
	    /// 
        /// Since track-less and identity track has difference behavior for
        /// accumulate animation blending if corresponding track presenting at other
        /// animation that is non-identity, and in normally this method didn't known
        /// about the situation of other animation, it can't deciding whether or not
        /// discards identity tracks. So there have a parameter allow you choose
        /// what you want, in case you aren't sure how to do that, you should use
        /// Skeleton::optimiseAllAnimations instead.
	    /// </summary>
	    /// <param name="discardIdentityNodeTracks">If true, discard identity node tracks.</param>
        public void optimize(bool discardIdentityNodeTracks)
        {
            Animation_optimize2(animation, discardIdentityNodeTracks);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_getName(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern float Animation_getLength(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_createNodeTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_createNumericTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_createVertexTrack(IntPtr animation, ushort handle, VertexAnimationType animType);

        [DllImport("OgreCWrapper")]
        private static extern ushort Animation_getNumNodeTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_getNodeTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Animation_hasNodeTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern ushort Animation_getNumNumericTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_getNumericTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Animation_hasNumericTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern ushort Animation_getNumVertexTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Animation_getVertexTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Animation_hasVertexTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyNodeTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyNumericTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyVertexTrack(IntPtr animation, ushort handle);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyAllTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyAllNodeTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyAllNumericTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_destroyAllVertexTracks(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply1(IntPtr animation, float timePos);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply2(IntPtr animation, float timePos, float wieght);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply3(IntPtr animation, float timePos, float wieght, float scale);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply4(IntPtr animation, IntPtr skeleton, float timePos);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply5(IntPtr animation, IntPtr skeleton, float timePos, float weight);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply6(IntPtr animation, IntPtr skeleton, float timePos, float weight, float scale);

        [DllImport("OgreCWrapper")]
        private static extern unsafe void Animation_apply7(IntPtr animation, IntPtr skeleton, float timePos, float weight, float* blendMask, int blendMaskSize, float scale);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_apply8(IntPtr animation, IntPtr entity, float timePos, float weight, bool software, bool hardware);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_setInterpolationMode(IntPtr animation, InterpolationMode im);

        [DllImport("OgreCWrapper")]
        private static extern InterpolationMode Animation_getInterpolationMode(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_setRotationInterpolationMode(IntPtr animation, RotationInterpolationMode im);

        [DllImport("OgreCWrapper")]
        private static extern RotationInterpolationMode Animation_getRotationInterpolationMode(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_optimize1(IntPtr animation);

        [DllImport("OgreCWrapper")]
        private static extern void Animation_optimize2(IntPtr animation, bool discardIdentityNodeTracks);

#endregion
    }
}
