using Engine;
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
    public abstract class BEPUikJointDefinition : BEPUikConstraintDefinition
    {
        public BEPUikJointDefinition(String name)
            :base(name)
        {
            ConnectionABoneName = "IKBone";
            ConnectionBBoneName = "IKBone";
        }

        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BEPUikScene)))
            {
                BEPUikScene sceneManager = subscene.getSimElementManager<BEPUikScene>();
                sceneManager.IkFactory.addJoint(this, instance);
            }
            else
            {
                Log.Default.sendMessage("Cannot add {0} {1} to SimSubScene {2} because it does not contain a BEPUikScene.", LogLevel.Warning, this.GetType().Name, BEPUikInterface.PluginName, Name, subscene.Name);
            }
        }

        internal override void createProduct(SimObjectBase instance, BEPUikScene scene)
        {
            BEPUikBone connectionA = null;
            BEPUikBone connectionB = null;

            SimObject other = instance.getOtherSimObject(ConnectionASimObjectName);
            if(other == null)
            {
                throw new BEPUikBlacklistException("Cannot find ConnectionA SimObject named '{0}'", ConnectionASimObjectName);
            }
            
            connectionA = other.getElement(ConnectionABoneName) as BEPUikBone;
            if(connectionA == null)
            {
                throw new BEPUikBlacklistException("Cannot find ConnectionA bone named '{0}' in '{1}'", ConnectionABoneName, ConnectionASimObjectName);
            }

            other = instance.getOtherSimObject(ConnectionBSimObjectName);
            if (other == null)
            {
                throw new BEPUikBlacklistException("Cannot find ConnectionB SimObject named '{0}'", ConnectionBSimObjectName);
            }

            connectionB = other.getElement(ConnectionBBoneName) as BEPUikBone;
            if (connectionB == null)
            {
                throw new BEPUikBlacklistException("Cannot find ConnectionB bone named '{0}' in '{1}'", ConnectionBBoneName, ConnectionBSimObjectName);
            }

            SimElement element = createConstraint(connectionA, connectionB, instance);
            if (element != null)
            {
                instance.addElement(element);
            }
        }

        protected abstract SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance);

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
