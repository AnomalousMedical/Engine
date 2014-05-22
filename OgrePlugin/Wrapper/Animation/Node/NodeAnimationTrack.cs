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
    public class NodeAnimationTrack : AnimationTrack
    {
        internal static NodeAnimationTrack createWrapper(IntPtr nativeObject, object[] args)
        {
            return new NodeAnimationTrack(nativeObject, args[0] as Animation);
        }

        WrapperCollection<TransformKeyFrame> keyFrames = new WrapperCollection<TransformKeyFrame>(TransformKeyFrame.createWrapper);

        protected NodeAnimationTrack(IntPtr nodeAnimationTrack, Animation parent)
            : base(nodeAnimationTrack, parent)
        {

        }

        public override void Dispose()
        {
            keyFrames.Dispose();
            base.Dispose();
        }

        public override KeyFrame getKeyFrame(ushort index)
        {
            return keyFrames.getObject(NodeAnimationTrack_getKeyFrame(animationTrack, index));
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2)
        {
            IntPtr kf1, kf2;
            float retVal = NodeAnimationTrack_getKeyFramesAtTime1(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), out kf1, out kf2);
            keyFrame1 = keyFrames.getObject(kf1);
            keyFrame2 = keyFrames.getObject(kf2);
            return retVal;
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2, out ushort firstKeyIndex)
        {
            IntPtr kf1, kf2;
            float retVal = NodeAnimationTrack_getKeyFramesAtTime2(animationTrack, timeIndex.getTimePos(), timeIndex.getKeyIndex(), out kf1, out kf2, out firstKeyIndex);
            keyFrame1 = keyFrames.getObject(kf1);
            keyFrame2 = keyFrames.getObject(kf2);
            return retVal;
        }

        public override KeyFrame createKeyFrame(float timePos)
        {
            return keyFrames.getObject(NodeAnimationTrack_createKeyFrame(animationTrack, timePos));
        }

        public override void removeKeyFrame(ushort index)
        {
            keyFrames.destroyObject(NodeAnimationTrack_getKeyFrame(animationTrack, index));
            NodeAnimationTrack_removeKeyFrame(animationTrack, index);
        }

        public override void removeAllKeyFrames()
        {
            keyFrames.clearObjects();
            NodeAnimationTrack_removeAllKeyFrames(animationTrack);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NodeAnimationTrack_getKeyFrame(IntPtr animTrack, ushort index);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float NodeAnimationTrack_getKeyFramesAtTime1(IntPtr animTrack, float timePos, uint keyIndex, out IntPtr keyFrame1, out IntPtr keyFrame2);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float NodeAnimationTrack_getKeyFramesAtTime2(IntPtr animTrack, float timePos, uint keyIndex, out IntPtr keyFrame1, out IntPtr keyFrame2, out ushort firstKeyIndex);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NodeAnimationTrack_createKeyFrame(IntPtr animTrack, float timePos);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NodeAnimationTrack_removeKeyFrame(IntPtr animTrack, ushort index);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NodeAnimationTrack_removeAllKeyFrames(IntPtr animTrack);

#endregion
    }
}
