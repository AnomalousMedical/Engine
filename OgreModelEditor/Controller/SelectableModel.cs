using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.ObjectManagement;
using Anomalous.GuiFramework.Editor;

namespace OgreModelEditor
{
    class SelectableModel : MovableObject
    {
        private SimObjectBase modelObject;

        public SelectableModel()
        {
            
        }
        
        public SimObjectBase ModelObject
        {
            get
            {
                return modelObject;
            }
            set
            {
                modelObject = value;
            }
        }

        public Vector3 ToolTranslation
        {
            get
            {
                return modelObject.Translation;
            }
        }

        public void move(Vector3 offset)
        {
            Vector3 translation = modelObject.Translation + offset;
            modelObject.updateTranslation(ref translation, null);
        }

        public Quaternion ToolRotation
        {
            get
            {
                return modelObject.Rotation;
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
            modelObject.updateRotation(ref newRot, null);
        }

        public void alertToolHighlightStatus(bool highlighted)
        {
            
        }
    }
}
