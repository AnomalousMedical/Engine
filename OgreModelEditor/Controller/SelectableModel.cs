using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Engine;
using Engine.ObjectManagement;

namespace OgreModelEditor
{
    class SelectableModel : SelectableObject
    {
        private SimObjectBase modelObject;

        public SelectableModel()
        {
            
        }

        public void editPosition(ref Vector3 translation, ref Quaternion rotation)
        {
            modelObject.updatePosition(ref translation, ref rotation, null);

        }

        public void editTranslation(ref Vector3 translation)
        {
            modelObject.updateTranslation(ref translation, null);
        }

        public void editRotation(ref Quaternion rotation)
        {
            modelObject.updateRotation(ref rotation, null);
        }

        public Quaternion getRotation()
        {
            return modelObject.Rotation;
        }

        public Vector3 getTranslation()
        {
            return modelObject.Translation;
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
    }
}
