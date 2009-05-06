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

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="simObject">The instance.</param>
        public SelectableSimObject(SimObjectDefinition definition, SimObjectBase simObject)
        {
            this.simObject = simObject;
            this.definition = definition;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Edit the translation and rotation.
        /// </summary>
        /// <param name="translation">The new translation.</param>
        /// <param name="rotation">The new rotation.</param>
        public void editPosition(ref Vector3 translation, ref Quaternion rotation)
        {
            simObject.updatePosition(ref translation, ref rotation, null);
            definition.Translation = translation;
            definition.Rotation = rotation;
        }

        /// <summary>
        /// Edit the translation.
        /// </summary>
        /// <param name="translation">The new translation.</param>
        public void editTranslation(ref Vector3 translation)
        {
            simObject.updateTranslation(ref translation, null);
            definition.Translation = translation;
        }

        /// <summary>
        /// Edit the rotation.
        /// </summary>
        /// <param name="rotation">The new rotation to set.</param>
        public void editRotation(ref Quaternion rotation)
        {
            simObject.updateRotation(ref rotation, null);
            definition.Rotation = rotation;
        }

        /// <summary>
        /// Get the rotation of the object.
        /// </summary>
        /// <returns>The rotation.</returns>
        public Quaternion getRotation()
        {
            return simObject.Rotation;
        }

        /// <summary>
        /// Get the translation of the object.
        /// </summary>
        /// <returns>The translation.</returns>
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

        #endregion Functions

        #region Properties

        /// <summary>
        /// The definition.
        /// </summary>
        public SimObjectDefinition Definition
        {
            get
            {
                return definition;
            }
        }

        /// <summary>
        /// The instance.
        /// </summary>
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

        #endregion Properties

        #region EditInterface

        private EditInterface editInterface;

        /// <summary>
        /// Get the EditInterface creating it if required.
        /// </summary>
        /// <returns>The EditInterface.</returns>
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
