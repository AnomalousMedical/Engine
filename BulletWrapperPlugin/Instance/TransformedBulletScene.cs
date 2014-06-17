using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class TransformedBulletScene : BulletScene
    {
        private String transformSimObjectName;

        public unsafe TransformedBulletScene(TransformedBulletSceneDefinition definition, UpdateTimer timer)
            :base(definition, timer)
        {
            transformSimObjectName = definition.TransformSimObjectName;
            factory.OnLinkPhase += factory_OnLinkPhase;
        }

        void factory_OnLinkPhase(BulletFactory obj)
        {
            if(rigidBodies.Count > 0)
            {
                TransformSimObject = rigidBodies[0].Owner.getOtherSimObject(transformSimObjectName);
                if(TransformSimObject == null)
                {
                    SimObjectErrorManager.AddError(new SimObjectError()
                    {
                        SimObject = "TransformedBulletScene",
                        Type = this.GetType().Name,
                        Subsystem = BulletInterface.PluginName,
                        ElementName = getName(),
                        Message = String.Format("Cannot find Transform Target Sim Object named '{0}'.", transformSimObjectName)
                    });
                    TransformSimObject = new GenericSimObject(getName() + "TransformedBulletScene_AutoCreatedSimObject", Vector3.Zero, Quaternion.Identity, Vector3.ScaleIdentity, true);
                }
            }
            else
            {
                SimObjectErrorManager.AddError(new SimObjectError()
                {
                    SimObject = "TransformedBulletScene",
                    Type = this.GetType().Name,
                    Subsystem = BulletInterface.PluginName,
                    ElementName = getName(),
                    Message = "No Actors in Bullet Scene"
                });
                TransformSimObject = new GenericSimObject(getName() + "TransformedBulletScene_AutoCreatedSimObject", Vector3.Zero, Quaternion.Identity, Vector3.ScaleIdentity, true);
            }
        }

        internal override MotionState createMotionState(RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
        {
            return new TransformedMotionState(this, rigidBody, maxContactDistance, ref initialTrans, ref initialRot);
        }

        public SimObject TransformSimObject { get; private set; }
    }
}
