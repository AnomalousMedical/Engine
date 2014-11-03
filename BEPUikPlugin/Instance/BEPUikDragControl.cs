using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikDragControl : BEPUikControl
    {
        private DragControl dragControl;
        private BEPUikBone bone;

        public BEPUikDragControl(BEPUikBone bone, BEPUikScene scene, BEPUikDragControlDefinition definition, SimObjectBase instance)
            :base(scene, definition.Name)
        {
            this.bone = bone;
            dragControl = new DragControl();
            dragControl.TargetBone = bone.IkBone;
            dragControl.LinearMotor.Offset = (instance.Translation - bone.Owner.Translation).toBepuVec3();
            dragControl.MaximumForce = definition.MaximumForce;
        }

        public override SimElementDefinition saveToDefinition()
        {
            return new BEPUikDragControlDefinition(Name)
            {
                BoneSimObjectName = Owner == bone.Owner ? "this" : Owner.Name,
                BoneName = bone.Name,
                MaximumForce = dragControl.MaximumForce
            };
        }

        internal override void syncPosition()
        {
            dragControl.LinearMotor.TargetPosition = Owner.Translation.toBepuVec3();
        }

        internal override Control IKControl
        {
            get
            {
                return dragControl;
            }
        }

        public Vector3 TargetPosition
        {
            get
            {
                return Owner.Translation;
            }
            set
            {
                Quaternion rot = Owner.Rotation;
                updatePosition(ref value, ref rot);
                MovedThisTick = true;
            }
        }

        public float MaximumForce
        {
            get
            {
                return dragControl.MaximumForce;
            }
            set
            {
                dragControl.MaximumForce = value;
            }
        }

        /// <summary>
        /// Don't allow this bone to be changed, we rely on it staying the same in the scene to find
        /// which solver to add/remove this bone from.
        /// </summary>
        public override BEPUikBone Bone
        {
            get
            {
                return bone;
            }
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            Vector3 offset = dragControl.LinearMotor.TargetBone.Position.toEngineVec3() + dragControl.LinearMotor.Offset.toEngineVec3();
            Vector3 target = dragControl.LinearMotor.TargetPosition.toEngineVec3();
            drawingSurface.drawAxes(offset, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 10, Color.Orange, Color.Orange, Color.Orange);
            drawingSurface.drawAxes(target, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 10, Color.Yellow, Color.Yellow, Color.Yellow);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(offset, target);
        }
    }
}
