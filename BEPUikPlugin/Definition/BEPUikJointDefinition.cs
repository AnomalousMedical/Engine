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
    public abstract class BEPUikJointDefinition : SimElementDefinition
    {
        public BEPUikJointDefinition(String name)
            :base(name)
        {
            ConnectionABoneName = "IKBone";
            ConnectionBBoneName = "IKBone";
        }

        [Editable]
        public String ConnectionASimObjectName { get; set; }

        [Editable]
        public String ConnectionABoneName { get; set; }

        [Editable]
        public String ConnectionBSimObjectName { get; set; }

        [Editable]
        public String ConnectionBBoneName { get; set; }

        public BEPUikJointDefinition(LoadInfo info)
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
