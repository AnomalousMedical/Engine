using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    public class FireControls
    {
        public ButtonEvent Fire { get; private set; }

        public FireControls(object eventLayerKey)
        {
            Fire = new ButtonEvent(eventLayerKey);
            Fire.addButton(MouseButtonCode.MB_BUTTON0);

            try
            {
                DefaultEvents.registerDefaultEvent(Fire);
            }
            catch (Exception) { }
        }
    }

    public class FireAtCursor : BehaviorInterface
    {
        public FireControls Controls { get { return controls; } }

        [DoNotCopy]
        [DoNotSave]
        private FireControls controls;

        [Editable]
        public String SimObjectTemplate { get; set; }

        [DoNotCopy]
        [DoNotSave]
        private SimObjectDefinition template;

        [DoNotCopy]
        [DoNotSave]
        private Quaternion objectStartingRot;

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

            objectStartingRot = template.Rotation;

            controls = getService<FireControls>();
            controls.Fire.FirstFrameDownEvent += Fire_FirstFrameDownEvent;
        }

        protected override void destroy()
        {
            controls.Fire.FirstFrameDownEvent -= Fire_FirstFrameDownEvent;

            base.destroy();
        }

        private void Fire_FirstFrameDownEvent(EventLayer eventLayer)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                Vector3 mouseVector = eventLayer.Mouse.unproject();
                mouseVector.z = 0; //Clear wheel

                Vector3 ownerPlanarTranslation = Owner.Translation;
                ownerPlanarTranslation.z = 0;

                Vector3 fireVector = mouseVector - ownerPlanarTranslation;
                fireVector.normalize();

                template.Name = Guid.NewGuid().ToString();
                template.Translation = Owner.Translation;
                template.Rotation = Quaternion.shortestArcQuat(ref Vector3.UnitZ, ref fireVector); ;
                Owner.createOtherSimObject(template);
            }
        }
    }
}
