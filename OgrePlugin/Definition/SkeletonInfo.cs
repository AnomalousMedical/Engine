using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Logging;
using Engine.Saving;

namespace OgrePlugin
{
    /// <summary>
    /// This class saves the state of a SkeletonInstance for saving to a definition.
    /// </summary>
    [Serializable]
    class SkeletonInfo : Saveable
    {
        private List<BoneInfo> bones = new List<BoneInfo>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SkeletonInfo()
        {

        }

        /// <summary>
        /// Save the skeleton to this description.
        /// </summary>
        /// <param name="skeleton">The skeleton to save.</param>
        public void saveSkeleton(SkeletonInstance skeleton)
        {
            if (skeleton != null)
            {
                for (ushort i = 0; i < skeleton.getNumBones(); i++)
                {
                    bones.Add(new BoneInfo(skeleton.getBone(i)));
                }
            }
        }

        /// <summary>
        /// Load a skeleton back from this description.
        /// </summary>
        /// <param name="skeleton"></param>
        public void initialzeSkeleton(SkeletonInstance skeleton)
        {
            if (skeleton != null)
            {
                if (skeleton.getNumBones() == bones.Count)
                {
                    for (ushort i = 0; i < skeleton.getNumBones(); i++)
                    {
                        bones[i].restoreBone(skeleton.getBone(i));
                    }
                }
                else
                {
                    Log.Default.sendMessage("Mismatched number of bones in loaded skeleton, state not restored.", LogLevel.Error, "ObjectManagement");
                }
            }
        }

        /// <summary>
        /// Clear this skeleton definition.
        /// </summary>
        public void clear()
        {
            bones.Clear();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            SkeletonInfo clone = new SkeletonInfo();
            foreach (BoneInfo bone in this.bones)
            {
                clone.bones.Add((BoneInfo)bone.Clone());
            }
            return clone;
        }

        #region Saveable Members

        private const string BONE_BASE = "Bone";

        private SkeletonInfo(LoadInfo info)
        {
            for (int i = 0; info.hasValue(BONE_BASE + i); i++)
            {
                bones.Add(info.GetValue<BoneInfo>(BONE_BASE + i));
            }
        }

        public void getInfo(SaveInfo info)
        {
            int i = 0;
            foreach (BoneInfo bone in bones)
            {
                info.AddValue(BONE_BASE + i++, bone);
            }
        }

        #endregion
    }
}
