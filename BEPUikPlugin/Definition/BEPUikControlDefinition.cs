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
    public abstract class BEPUikControlDefinition : BEPUikElementDefinition
    {
        [Editable]
        public String BoneSimObjectName { get; set; }

        [Editable]
        public String BoneName { get; set; }

        public BEPUikControlDefinition(String name)
            :base(name)
        {
            BoneSimObjectName = "this";
            BoneName = "IKBone";
        }

        protected BEPUikControlDefinition(LoadInfo info)
            :base(info)
        {
            BoneSimObjectName = info.GetString("BoneSimObjectName");
            BoneName = info.GetString("BoneName");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("BoneSimObjectName", BoneSimObjectName);
            info.AddValue("BoneName", BoneName);
        }
    }
}
