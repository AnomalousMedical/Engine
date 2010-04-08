using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine;
using Engine.Saving;

namespace Anomaly
{
    public partial class Instance : Saveable
    {
        /// <summary>
        /// The key from a positions file that determines where this instance goes.
        /// </summary>
        [Editable]
        private String positionKey;

        /// <summary>
        /// The SimObject definition for this Instance.
        /// </summary>
        private SimObjectDefinition simObject;

        private String name;

        public Instance(String name, SimObjectDefinition definition)
        {
            this.name = name;
            simObject = definition;
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public SimObjectDefinition Definition
        {
            get
            {
                return simObject;
            }
        }

        [Editable]
        public Vector3 Translation
        {
            get
            {
                return simObject.Translation;
            }
            set
            {
                simObject.Translation = value;
            }
        }

        [Editable]
        public Vector3 Rotation
        {
            get
            {
                return simObject.Rotation.getEuler();
            }
            set
            {
                simObject.Rotation = new Quaternion(value.x, value.y, value.z);
            }
        }

        [Editable]
        public Vector3 Scale
        {
            get
            {
                return simObject.Scale;
            }
            set
            {
                simObject.Scale = value;
            }
        }

        #region Saveable Members

        protected Instance(LoadInfo info)
        {
            positionKey = info.GetString("PositionKey");
            name = info.GetString("Name");
            simObject = info.GetValue<SimObjectDefinition>("SimObject");
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("PositionKey", positionKey);
            info.AddValue("Name", name);
            info.AddValue("SimObject", simObject);
        }

        #endregion
    }

    public partial class Instance
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, BehaviorEditMemberScanner.Scanner, name, null);
                editInterface.IconReferenceTag = AnomalyIcons.Instance;
                editInterface.addSubInterface(simObject.getEditInterface());
            }
            return editInterface;
        }
    }
}
