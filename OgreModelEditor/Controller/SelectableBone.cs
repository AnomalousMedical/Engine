using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.ObjectManagement;
using Anomalous.GuiFramework.Editor;
using OgrePlugin;

namespace OgreModelEditor
{
    class SelectableBone : Anomalous.GuiFramework.Editor.MovableObject
    {
        private Bone bone;

        public SelectableBone()
        {
            
        }
        
        public Bone Bone
        {
            get
            {
                return bone;
            }
            set
            {
                bone = value;
            }
        }

        public Vector3 ToolTranslation
        {
            get
            {
                return bone.getDerivedPosition();
            }
        }

        public void move(Vector3 offset)
        {
            Vector3 translation = bone.getPosition() + offset;
            bone.setPosition(translation);
            bone.setManuallyControlled(true);
            bone.needUpdate(true);
        }

        public Quaternion ToolRotation
        {
            get
            {
                return bone.getOrientation();
            }
        }

        public bool ShowTools
        {
            get
            {
                return true;
            }
        }

        public void rotate(Quaternion newRot)
        {
            bone.setOrientation(newRot);
            bone.setManuallyControlled(true);
            bone.needUpdate(true);
        }

        public void alertToolHighlightStatus(bool highlighted)
        {
            
        }
    }
}
