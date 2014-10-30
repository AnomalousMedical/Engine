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
    public class BEPUikDragControlDefinition : BEPUikControlDefinition
    {
        [DoNotSave]
        private EditInterface editInterface;

        public BEPUikDragControlDefinition(String name)
            :base(name)
        {
            MaximumForce = float.MaxValue;
        }

        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BEPUikScene)))
            {
                BEPUikScene sceneManager = subscene.getSimElementManager<BEPUikScene>();
                sceneManager.IkFactory.addControl(this, instance);
            }
            else
            {
                Log.Default.sendMessage("Cannot add BEPUikDragControl {0} to SimSubScene {1} because it does not contain a BEPUikScene.", LogLevel.Warning, BEPUikInterface.PluginName, Name, subscene.Name);
            }
        }

        [Editable]
        public float MaximumForce { get; set; }

        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, String.Format("{0} - IK Drag Control", Name));
            }
            return editInterface;
        }

        internal override void createProduct(SimObjectBase instance, BEPUikScene scene)
        {
            var otherObject = instance.getOtherSimObject(this.BoneSimObjectName);
            if(otherObject != null)
            {
                var bone = otherObject.getElement(this.BoneName) as BEPUikBone;
                if (bone != null)
                {
                    instance.addElement(new BEPUikDragControl(bone, scene, this, instance, Subscription));
                }
                else
                {
                    Log.Default.sendMessage("Cannot add BEPUikDragControl {0} to SimObject {1} because the Sim Object does not contain a bone named {2}.", LogLevel.Warning, BEPUikInterface.PluginName, Name, BoneSimObjectName, BoneName);
                }
            }
            else
            {
                Log.Default.sendMessage("Cannot add BEPUikDragControl {0} to SimObject {1} because the Sim Object does not exist.", LogLevel.Warning, BEPUikInterface.PluginName, Name, BoneSimObjectName);
            }
        }

        protected BEPUikDragControlDefinition(LoadInfo info)
            :base(info)
        {
            MaximumForce = info.GetValue("MaximumForce", float.MaxValue);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MaximumForce", MaximumForce);
        }

        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikDragControlDefinition(name));
        }
    }
}
