using BEPUik;
using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikBone : SimElement
    {
        private BEPUikScene scene;
        private Bone bone;

        public BEPUikBone(BEPUikBoneDefinition definition, SimObjectBase instance, BEPUikScene scene)
            :base(definition.Name, definition.Subscription)
        {
            this.scene = scene;
            bone = new Bone(instance.Translation.toBepuVec3(), instance.Rotation.toBepuQuat(), definition.Radius, definition.Height, definition.Mass);
            bone.Pinned = definition.Pinned;
        }

        protected override void Dispose()
        {
            scene.removeBone(this);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            bone.Position = translation.toBepuVec3();
            bone.Orientation = rotation.toBepuQuat();
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            bone.Position = translation.toBepuVec3();
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            bone.Orientation = rotation.toBepuQuat();
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            
        }

        protected override void setEnabled(bool enabled)
        {
            if(enabled)
            {
                scene.addBone(this);
            }
            else
            {
                scene.removeBone(this);
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikBoneDefinition(Name)
            {
                Pinned = bone.Pinned,
                Radius = bone.Radius,
                Height = bone.Height,
                Subscription = this.Subscription
            };

            customizeDefinition(definition);

            return definition;
        }

        protected virtual void customizeDefinition(BEPUikBoneDefinition definition)
        {

        }

        public virtual void syncSimObject()
        {
            Vector3 trans = bone.Position.toEngineVec3();
            Quaternion rot = bone.Orientation.toEngineQuat();

            updatePosition(ref trans, ref rot);
        }

        public Bone IkBone
        {
            get
            {
                return bone;
            }
        }
    }
}
