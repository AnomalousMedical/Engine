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
    public class BEPUikJointDefinition : SimElementDefinition
    {
        [DoNotSave]
        private EditInterface editInterface;

        public BEPUikJointDefinition(String name)
            :base(name)
        {
            
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
                Log.Default.sendMessage("Cannot add BEPUikJoint {0} to SimSubScene {1} because it does not contain a BEPUikScene.", LogLevel.Warning, BEPUikInterface.PluginName, Name, subscene.Name);
            }
        }

        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, String.Format("{0} - BEPU IK Joint", Name));
            }
            return editInterface;
        }

        public BEPUikJointDefinition(LoadInfo info)
            :base(info)
        {
            
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}
