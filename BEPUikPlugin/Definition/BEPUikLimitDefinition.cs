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
    public abstract class BEPUikLimitDefinition : BEPUikJointDefinition
    {
        public BEPUikLimitDefinition(String name)
            :base(name)
        {
            
        }

        protected BEPUikLimitDefinition(LoadInfo info)
            :base(info)
        {
            
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}
