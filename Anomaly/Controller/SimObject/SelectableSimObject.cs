using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Editor;
using Engine;
using Engine.Editing;

namespace Anomaly
{
    /// <summary>
    /// This class is a container that holds a SimObject and its
    /// SimObjectDefinition for the SimObjectController.
    /// </summary>
    class SelectableSimObject : SelectableObject
    {
        #region Fields

        private SimObjectBase simObject;
        SimObjectDefinition definition;

        #endregion Fields

        public SelectableSimObject(SimObjectDefinition definition, SimObjectBase simObject)
        {
            this.simObject = simObject;
            this.definition = definition;
        }

        public void editTranslation(ref Vector3 localTrans)
        {
            simObject.updateTranslation(ref localTrans, null);
            definition.Translation = localTrans;
        }

        public void editPosition(ref Vector3 localTrans, ref Quaternion localRot)
        {
            simObject.updatePosition(ref localTrans, ref localRot, null);
            definition.Translation = localTrans;
            definition.Rotation = localRot;
        }

        public void editRotation(ref Quaternion newRot)
        {
            simObject.updateRotation(ref newRot, null);
            definition.Rotation = newRot;
        }

        public Quaternion getRotation()
        {
            return simObject.Rotation;
        }

        public Vector3 getTranslation()
        {
            return simObject.Translation;
        }

        /// <summary>
        /// Make the properties of the definition match the instance.
        /// </summary>
        public void captureInstanceProperties()
        {
            this.definition = Instance.saveToDefinition(Instance.Name);
        }

        public SimObjectDefinition Definition
        {
            get
            {
                return definition;
            }
        }

        public SimObjectBase Instance
        {
            get
            {
                return simObject;
            }
            set
            {
                simObject = value;
            }
        }

        #region EditInterface

        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(definition.Name);
            }
            return editInterface;
        }

        #endregion EditInterface
    }
}
