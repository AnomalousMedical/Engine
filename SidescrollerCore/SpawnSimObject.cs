using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    class SpawnSimObject : BehaviorInterface
    {
        [Editable]
        public String SimObjectTemplate { get; set; }

        [Editable]
        public String TriggerName { get; set; }

        [DoNotCopy]
        [DoNotSave]
        private SimObjectDefinition template;

        [DoNotCopy]
        [DoNotSave]
        private Triggerable trigger;

        protected override void link()
        {
            base.link();

            //We need a real template that does not involve sucking an object out of the scene, but whatever for now
            var templateObject = Owner.getOtherSimObject(SimObjectTemplate);
            if (templateObject == null)
            {
                blacklist($"Cannot find template object {SimObjectTemplate}");
            }

            template = templateObject.saveToDefinition("");
            ThreadManager.invoke(templateObject.destroy);

            trigger = Owner.getElement(TriggerName) as Triggerable;
            if(trigger == null)
            {
                blacklist($"Cannot find trigger {TriggerName}");
            }

            trigger.Triggered += Trigger_Triggered;
        }

        private void Trigger_Triggered(Triggerable obj)
        {
            template.Name = Guid.NewGuid().ToString();
            Owner.createOtherSimObject(template);
        }
    }
}
