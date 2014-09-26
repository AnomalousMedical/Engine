using BEPUik;
using Engine;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    /// <summary>
    /// A wrapper for BEPUik drag controls that is not a sim element. This is here mostly so we don't have
    /// to expose the bepu ik classes for the sim elements and instead they can be used directly.
    /// </summary>
    public class ExternalDragControl : ExternalControl
    {
        private DragControl dragControl;
        private BEPUikBone targetBone;

        public ExternalDragControl()
        {
            dragControl = new DragControl();
        }

        public SingleBoneLinearMotor LinearMotor
        {
            get
            {
                return dragControl.LinearMotor;
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

        public BEPUikBone TargetBone
        {
            get
            {
                return targetBone;
            }
            set
            {
                targetBone = value;
                dragControl.TargetBone = targetBone != null ? targetBone.IkBone : null;
            }
        }

        internal override Control IKControl
        {
            get
            {
                return dragControl;
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
