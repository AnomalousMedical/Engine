using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikJointDefinition : BEPUikElementDefinition
    {
        public BEPUikJointDefinition(String name)
            :base(name)
        {
            ConnectionABoneName = "IKBone";
            ConnectionBBoneName = "IKBone";
        }

        internal override void createProduct(SimObjectBase instance, BEPUikScene scene)
        {
            BEPUikBone connectionA = null;
            BEPUikBone connectionB = null;

            SimObject other = instance.getOtherSimObject(ConnectionASimObjectName);
            if (other != null)
            {
                connectionA = other.getElement(ConnectionABoneName) as BEPUikBone;
            }

            other = instance.getOtherSimObject(ConnectionBSimObjectName);
            if (other != null)
            {
                connectionB = other.getElement(ConnectionBBoneName) as BEPUikBone;
            }

            if (connectionA != null && connectionB != null)
            {
                SimElement element = createConstraint(connectionA, connectionB, instance, scene);
                if (element != null)
                {
                    instance.addElement(element);
                }
            }
            else
            {
                Log.Default.sendMessage("Cannot add BEPU IK Joint {0} to SimObject {1} because connectionA {2} and connectionB {3}.", LogLevel.Warning, BEPUikInterface.PluginName, Name, instance.Name, connectionA != null ? "was found" : "was not found", connectionB != null ? "was found" : "was not found");
            }
        }

        protected abstract SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance, BEPUikScene scene);

        [Editable]
        public String ConnectionASimObjectName { get; set; }

        [Editable]
        public String ConnectionABoneName { get; set; }

        [Editable]
        public String ConnectionBSimObjectName { get; set; }

        [Editable]
        public String ConnectionBBoneName { get; set; }

        protected BEPUikJointDefinition(LoadInfo info)
            :base(info)
        {
            ConnectionASimObjectName = info.GetString("ConnectionASimObjectName");
            ConnectionABoneName = info.GetString("ConnectionABoneName");
            ConnectionBSimObjectName = info.GetString("ConnectionBSimObjectName");
            ConnectionBBoneName = info.GetString("ConnectionBBoneName");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("ConnectionASimObjectName", ConnectionASimObjectName);
            info.AddValue("ConnectionABoneName", ConnectionABoneName);
            info.AddValue("ConnectionBSimObjectName", ConnectionBSimObjectName);
            info.AddValue("ConnectionBBoneName", ConnectionBBoneName);
        }
    }
}
