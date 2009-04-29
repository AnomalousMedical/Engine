using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Engine.Saving;
using Engine;

namespace OgrePlugin
{
    /// <summary>
    /// This class saves information about an individual bone.
    /// </summary>
    [Serializable]
    class BoneInfo : Saveable
    {
        private Quaternion orientation;
        private Vector3 translation;
        private Vector3 scale;
        private bool manualControl;
        private bool inheritOrientation;
        private bool inheritScale;

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        private BoneInfo()
        {

        }

        /// <summary>
        /// Constructor, takes the bone to save.
        /// </summary>
        /// <param name="bone">The bone to save.</param>
        public BoneInfo(Bone bone)
        {
            orientation = bone.getOrientation();
            translation = bone.getPosition();
            scale = bone.getScale();
            manualControl = bone.isManuallyControlled();
            inheritOrientation = bone.getInheritOrientation();
            inheritScale = bone.getInheritScale();
        }

        /// <summary>
        /// Restores the given bone with the saved info.
        /// </summary>
        /// <param name="bone">The bone to restore.</param>
        public void restoreBone(Bone bone)
        {
            bone.setManuallyControlled(manualControl);
            bone.setInheritOrientation(inheritOrientation);
            bone.setInheritScale(inheritScale);
            bone.setOrientation(orientation);
            bone.setPosition(translation);
            bone.setScale(scale);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            BoneInfo clone = new BoneInfo();
            clone.orientation = this.orientation;
            clone.translation = this.translation;
            clone.scale = this.scale;
            clone.manualControl = this.manualControl;
            clone.inheritOrientation = this.inheritOrientation;
            clone.inheritScale = this.inheritScale;
            return clone;
        }

        #region Saveable Members

        private const string ORIENTATION = "Orientation";
        private const string TRANSLATION = "Translation";
        private const string SCALE = "Scale";
        private const string MANUAL_CONTROL = "ManualControl";
        private const string INHERIT_ORIENTATION = "InheritOrientation";
        private const string INHERIT_SCALE = "InheritScale";

        private BoneInfo(LoadInfo info)
        {
            orientation = info.GetQuaternion(ORIENTATION);
            translation = info.GetVector3(TRANSLATION);
            scale = info.GetVector3(SCALE);
            manualControl = info.GetBoolean(MANUAL_CONTROL);
            inheritOrientation = info.GetBoolean(INHERIT_ORIENTATION);
            inheritScale = info.GetBoolean(INHERIT_SCALE);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(ORIENTATION, orientation);
            info.AddValue(TRANSLATION, translation);
            info.AddValue(SCALE, scale);
            info.AddValue(MANUAL_CONTROL, manualControl);
            info.AddValue(INHERIT_ORIENTATION, inheritOrientation);
            info.AddValue(INHERIT_SCALE, inheritScale);
        }

        #endregion
    }
}
