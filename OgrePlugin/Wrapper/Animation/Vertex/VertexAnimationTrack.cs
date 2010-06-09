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
    public class VertexAnimationTrack : AnimationTrack
    {
        /// <summary>
        /// The target animation mode for vertex animation.
        /// </summary>
        [SingleEnum]
        public enum TargetMode : uint
        {
	        /// <summary>
	        /// Interpolate vertex positions in software.
	        /// </summary>
	        TM_SOFTWARE, 
	        /// <summary>
	        /// Bind keyframe 1 to position, and keyframe 2 to a texture coordinate
	        /// for interpolation in hardware.
	        /// </summary>
	        TM_HARDWARE
        };

        WrapperCollection<VertexMorphKeyFrame> morphKeyFrames = new WrapperCollection<VertexMorphKeyFrame>(VertexMorphKeyFrame.createWrapper);
        WrapperCollection<VertexPoseKeyFrame> poseKeyFrames = new WrapperCollection<VertexPoseKeyFrame>(VertexPoseKeyFrame.createWrapper);

        internal static VertexAnimationTrack createWrapper(IntPtr nativeObject, object[] args)
        {
            return new VertexAnimationTrack(nativeObject, args[0] as Animation);
        }

        protected VertexAnimationTrack(IntPtr animationTrack, Animation parent)
            :base(animationTrack, parent)
        {

        }

        public override void Dispose()
        {
            morphKeyFrames.Dispose();
            poseKeyFrames.Dispose();
            base.Dispose();
        }

        /// <summary>
	    /// Get the type of vertex animation we're performing. 
	    /// </summary>
	    /// <returns>The animation type enum.</returns>
        public VertexAnimationType getAnimationType()
        {
            return VertexAnimationTrack_getAnimationType(animationTrack);
        }

	    /// <summary>
	    /// Creates a new morph KeyFrame and adds it to this animation at the given
        /// time index.
	    /// <para>
        /// It is better to create KeyFrames in time order. Creating them out of
        /// order can result in expensive reordering processing. Note that a
        /// KeyFrame at time index 0.0 is always created for you, so you don't need
        /// to create this one, just access it using getKeyFrame(0); 
	    /// </para>
	    /// </summary>
	    /// <param name="timePos">The time from which this KeyFrame will apply. </param>
	    /// <returns>The new VertexMorphKeyFrame.</returns>
        public VertexMorphKeyFrame createVertexMorphKeyFrame(float timePos)
        {
            return morphKeyFrames.getObject(VertexAnimationTrack_createVertexMorphKeyFrame(animationTrack, timePos));
        }

	    /// <summary>
	    /// Creates the single pose KeyFrame and adds it to this animation.
	    /// </summary>
	    /// <param name="timePos">The time from which this KeyFrame will apply. </param>
	    /// <returns>The new VertexPoseKeyFrame.</returns>
        public VertexPoseKeyFrame createVertexPoseKeyFrame(float timePos)
        {
            return poseKeyFrames.getObject(VertexAnimationTrack_createVertexPoseKeyFrame(animationTrack, timePos));
        }

	    /// <summary>
	    /// Applies an animation track to the designated target. 
	    /// </summary>
	    /// <param name="timeIndex">The time position in the animation to apply. </param>
        public void apply(TimeIndex timeIndex)
        {
            VertexAnimationTrack_apply1(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex());
        }

	    /// <summary>
	    /// Applies an animation track to the designated target. 
	    /// </summary>
	    /// <param name="timeIndex">The time position in the animation to apply. </param>
	    /// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
        public void apply(TimeIndex timeIndex, float weight)
        {
            VertexAnimationTrack_apply2(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), weight);
        }

	    /// <summary>
	    /// Applies an animation track to the designated target. 
	    /// </summary>
	    /// <param name="timeIndex">The time position in the animation to apply. </param>
	    /// <param name="weight">The influence to give to this track, 1.0 for full influence, less to blend with other animations.</param>
	    /// <param name="scale">The scale to apply to translations and scalings, useful for adapting an animation to a different size target.</param>
        public void apply(TimeIndex timeIndex, float weight, float scale)
        {
            VertexAnimationTrack_apply3(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), weight, scale);
        }

	    /// <summary>
	    /// Returns the morph KeyFrame at the specified index. 
	    /// </summary>
	    /// <param name="index">The time index of the frame.</param>
	    /// <returns>The key frame at the given position.</returns>
        public VertexMorphKeyFrame getVertexMorphKeyFrame(ushort index)
        {
            return morphKeyFrames.getObject(VertexAnimationTrack_getVertexMorphKeyFrame(animationTrack, index));
        }

	    /// <summary>
	    /// Returns the pose KeyFrame at the specified index. 
	    /// </summary>
	    /// <param name="index">The time index of the frame.</param>
	    /// <returns></returns>
        public VertexPoseKeyFrame getVertexPoseKeyFrame(ushort index)
        {
            return poseKeyFrames.getObject(VertexAnimationTrack_getVertexPoseKeyFrame(animationTrack, index));
        }

	    /// <summary>
	    /// Set the target mode. 
	    /// </summary>
	    /// <param name="m">The mode to set.</param>
        public void setTargetMode(TargetMode m)
        {
            VertexAnimationTrack_setTargetMode(animationTrack, m);
        }

	    /// <summary>
	    /// Get the target mode. 
	    /// </summary>
	    /// <returns>The current mode.</returns>
        public TargetMode getTargetMode()
        {
            return VertexAnimationTrack_getTargetMode(animationTrack);
        }

        public override KeyFrame getKeyFrame(ushort index)
        {
            switch(VertexAnimationTrack_getAnimationType(animationTrack))
            {
                case VertexAnimationType.VAT_MORPH:
                    return morphKeyFrames.getObject(VertexAnimationTrack_getKeyFrame(animationTrack, index));
                case VertexAnimationType.VAT_POSE:
                    return poseKeyFrames.getObject(VertexAnimationTrack_getKeyFrame(animationTrack, index));
                default:
                    throw new NotImplementedException();
            }
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2)
        {
            IntPtr kf1, kf2;
            float retVal = VertexAnimationTrack_getKeyFramesAtTime1(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), out kf1, out kf2);
            switch (VertexAnimationTrack_getAnimationType(animationTrack))
            {
                case VertexAnimationType.VAT_MORPH:
                    keyFrame1 = morphKeyFrames.getObject(kf1);
                    keyFrame2 = morphKeyFrames.getObject(kf2);
                    break;
                case VertexAnimationType.VAT_POSE:
                    keyFrame1 = poseKeyFrames.getObject(kf1);
                    keyFrame2 = poseKeyFrames.getObject(kf2);
                    break;
                default:
                    keyFrame1 = null;
                    keyFrame2 = null;
                    throw new NotImplementedException();
            }
            return retVal;
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2, out ushort firstKeyIndex)
        {
            IntPtr kf1, kf2;
            float retVal = VertexAnimationTrack_getKeyFramesAtTime2(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), out kf1, out kf2, out firstKeyIndex);
            switch (VertexAnimationTrack_getAnimationType(animationTrack))
            {
                case VertexAnimationType.VAT_MORPH:
                    keyFrame1 = morphKeyFrames.getObject(kf1);
                    keyFrame2 = morphKeyFrames.getObject(kf2);
                    break;
                case VertexAnimationType.VAT_POSE:
                    keyFrame1 = poseKeyFrames.getObject(kf1);
                    keyFrame2 = poseKeyFrames.getObject(kf2);
                    break;
                default:
                    keyFrame1 = null;
                    keyFrame2 = null;
                    throw new NotImplementedException();
            }
            return retVal;
        }

        public override KeyFrame createKeyFrame(float timePos)
        {
            switch (VertexAnimationTrack_getAnimationType(animationTrack))
            {
                case VertexAnimationType.VAT_MORPH:
                    return morphKeyFrames.getObject(VertexAnimationTrack_createKeyFrame(animationTrack, timePos));
                case VertexAnimationType.VAT_POSE:
                    return poseKeyFrames.getObject(VertexAnimationTrack_createKeyFrame(animationTrack, timePos));
                default:
                    throw new NotImplementedException();
            }
        }

        public override void removeKeyFrame(ushort index)
        {
            switch (VertexAnimationTrack_getAnimationType(animationTrack))
            {
                case VertexAnimationType.VAT_MORPH:
                    morphKeyFrames.destroyObject(VertexAnimationTrack_getKeyFrame(animationTrack, index));
                    break;
                case VertexAnimationType.VAT_POSE:
                    poseKeyFrames.destroyObject(VertexAnimationTrack_getKeyFrame(animationTrack, index));
                    break;
                default:
                    throw new NotImplementedException();
            }
            VertexAnimationTrack_removeKeyFrame(animationTrack, index);
        }

        public override void removeAllKeyFrames()
        {
            morphKeyFrames.Dispose();
            poseKeyFrames.Dispose();
            VertexAnimationTrack_removeAllKeyFrames(animationTrack);
        }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern VertexAnimationType VertexAnimationTrack_getAnimationType(IntPtr animTrack);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_createVertexMorphKeyFrame(IntPtr animTrack, float timePos);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_createVertexPoseKeyFrame(IntPtr animTrack, float timePos);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_apply1(IntPtr animTrack, float timePos, uint keyIndex);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_apply2(IntPtr animTrack, float timePos, uint keyIndex, float weight);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_apply3(IntPtr animTrack, float timePos, uint keyIndex, float weight, float scale);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_getVertexMorphKeyFrame(IntPtr animTrack, ushort index);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_getVertexPoseKeyFrame(IntPtr animTrack, ushort index);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_setTargetMode(IntPtr animTrack, TargetMode m);

        [DllImport("OgreCWrapper")]
        private static extern TargetMode VertexAnimationTrack_getTargetMode(IntPtr animTrack);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_getKeyFrame(IntPtr animTrack, ushort index);

        [DllImport("OgreCWrapper")]
        private static extern float VertexAnimationTrack_getKeyFramesAtTime1(IntPtr animTrack, float timePos, uint keyIndex, out IntPtr keyFrame1, out IntPtr keyFrame2);

        [DllImport("OgreCWrapper")]
        private static extern float VertexAnimationTrack_getKeyFramesAtTime2(IntPtr animTrack, float timePos, uint keyIndex, out IntPtr keyFrame1, out IntPtr keyFrame2, out ushort firstKeyIndex);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr VertexAnimationTrack_createKeyFrame(IntPtr animTrack, float timePos);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_removeKeyFrame(IntPtr animTrack, ushort index);

        [DllImport("OgreCWrapper")]
        private static extern void VertexAnimationTrack_removeAllKeyFrames(IntPtr animTrack);

        #endregion
    }
}
