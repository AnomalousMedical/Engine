using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Medical;
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
        private String positionBroadcasterName;

        private PositionBroadcaster positionBroadcaster;

        public TransformedBulletScene(TransformedBulletSceneDefinition definition, UpdateTimer timer)
            :base(definition, timer)
        {
            transformSimObjectName = definition.TransformSimObjectName;
            positionBroadcasterName = definition.PositionBroadcasterName;
            factory.OnLinkPhase += factory_OnLinkPhase;
        }

        public override void Dispose()
        {
            if(positionBroadcaster != null)
            {
                positionBroadcaster.PositionChanged -= positionBroadcaster_PositionChanged;
            }
            base.Dispose();
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
                positionBroadcaster = TransformSimObject.getElement(positionBroadcasterName) as PositionBroadcaster;
                if (positionBroadcaster != null)
                {
                    positionBroadcaster.PositionChanged += positionBroadcaster_PositionChanged;
                }
                else
                {
                    SimObjectErrorManager.AddError(new SimObjectError()
                    {
                        SimObject = "TransformedBulletScene",
                        Type = this.GetType().Name,
                        Subsystem = BulletInterface.PluginName,
                        ElementName = getName(),
                        Message = String.Format("Cannot find PositionBroadcaster named '{0}' in Transform Target Sim Object '{1}'.", positionBroadcasterName, transformSimObjectName)
                    });
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
                //Note that no position broadcaster is created here, the dummy sim object will not move so we don't actually care.
                //If you do decide to care add a PositionBroadcaster to the created TransformSimObject. This is really just an error state anyway, this behavior should not be used
                //create an actual simobject setup correctly to track if you are using this scene.
            }
        }

        internal override MotionState createMotionState(RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
        {
            if (rigidBody.StayLocalTransform)
            {
                return new MotionState(rigidBody, maxContactDistance, ref initialTrans, ref initialRot);
            }
            else
            {
                return new TransformedMotionState(this, rigidBody, maxContactDistance, ref initialTrans, ref initialRot);
            }
        }

        protected override BulletSceneDefinition createDefinition(String name)
        {
            return new TransformedBulletSceneDefinition(name)
            {
                TransformSimObjectName = transformSimObjectName
            };
        }

        public SimObject TransformSimObject { get; private set; }

        void positionBroadcaster_PositionChanged(SimObject obj)
        {
            ForceNextSynchronize = true;
        }
    }
}
