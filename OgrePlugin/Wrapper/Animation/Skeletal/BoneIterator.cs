using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    /// <summary>
    /// This class will be filled out by the getRootBoneIterator and
    /// getBoneIterator functions in Skeleton.
    /// </summary>
    internal class BoneIterator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void BoneFoundCallback(IntPtr bone);
        internal BoneFoundCallback boneFound;

        private List<Bone> bones = new List<Bone>();
        private WrapperCollection<Bone> boneCollection;

        public BoneIterator(WrapperCollection<Bone> boneCollection)
        {
            this.boneCollection = boneCollection;
            boneFound = new BoneFoundCallback(boneFoundCallback);
        }

        public IEnumerator<Bone> GetEnumerator()
        {
            return bones.GetEnumerator();
        }

        private void boneFoundCallback(IntPtr bone)
        {
            bones.Add(boneCollection.getObject(bone));
        }
    }
}
