using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    public class GenericSimObjectInstanceDefinition : SimObjectInstanceDefinition
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the new SimObject.</param>
        public GenericSimObjectInstanceDefinition(String name)
        {
            this.Name = name;
            Rotation = Quaternion.Identity;
            Translation = Vector3.Zero;
            Scale = Vector3.ScaleIdentity;
            DefinitionName = "";
            Enabled = false;
        }

        public SimObjectBase createSimObject()
        {
            GenericSimObject simObject = new GenericSimObject(Name);
            Vector3 vector3 = Translation;
            Quaternion quaternion = Rotation;
            simObject.updatePosition(ref vector3, ref quaternion, null);
            vector3 = Scale;
            simObject.updateScale(ref vector3, null);
            simObject.setEnabled(Enabled);
            return simObject;
        }

        /// <summary>
        /// The name of the instance of the sim object.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The initial rotation of the sim object.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// The initial translation of the sim object.
        /// </summary>
        public Vector3 Translation { get; set; }

        /// <summary>
        /// The initial scale of the object.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// The name of the defintion to load for the sim object.
        /// </summary>
        public String DefinitionName { get; set; }

        /// <summary>
        /// True if the object is enabled, false if it is disabled.
        /// </summary>
        public bool Enabled { get; set; }

        #region Saveable Members

        private const String NAME = "Name";
        private const String ROTATION = "Rotation";
        private const String TRANSLATION = "Translation";
        private const String SCALE = "Scale";
        private const String DEFINITION_NAME = "DefinitionName";
        private const String ENABLED = "Enabled";

        private GenericSimObjectInstanceDefinition(LoadInfo info)
        {
            Name = info.GetString(NAME);
            Rotation = info.GetQuaternion(ROTATION);
            Translation = info.GetVector3(TRANSLATION);
            Scale = info.GetVector3(SCALE);
            DefinitionName = info.GetString(DEFINITION_NAME);
            Enabled = info.GetBoolean(ENABLED);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, Name);
            info.AddValue(ROTATION, Rotation);
            info.AddValue(TRANSLATION, Translation);
            info.AddValue(SCALE, Scale);
            info.AddValue(DEFINITION_NAME, DefinitionName);
            info.AddValue(ENABLED, Enabled);
        }

        #endregion
    }
}
