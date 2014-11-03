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
    public class BEPUikBone : SimElement
    {
        private BEPUikScene scene;
        private Bone bone;
        private String solverName;

        public BEPUikBone(BEPUikBoneDefinition definition, SimObjectBase instance, BEPUikScene scene)
            :base(definition.Name)
        {
            this.scene = scene;
            bone = new Bone(instance.Translation.toBepuVec3(), instance.Rotation.toBepuQuat(), definition.Radius, definition.Height, definition.Mass);
            bone.Pinned = definition.Pinned;
            solverName = definition.SolverName;
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
                SolverName = solverName
            };

            customizeDefinition(definition);

            return definition;
        }

        protected virtual void customizeDefinition(BEPUikBoneDefinition definition)
        {

        }

        public bool Pinned
        {
            get
            {
                return bone.Pinned;
            }
            set
            {
                bone.Pinned = value;
            }
        }

        internal String SolverName
        {
            get
            {
                return solverName;
            }
        }

        internal virtual void syncSimObject()
        {
            Vector3 trans = bone.Position.toEngineVec3();
            Quaternion rot = bone.Orientation.toEngineQuat();

            updatePosition(ref trans, ref rot);
        }

        internal void draw(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.Color = Color.Purple;

            Quaternion orientation = bone.Orientation.toEngineQuat();
            Vector3 translation = bone.Position.toEngineVec3();
            Vector3 localUnitX = Quaternion.quatRotate(orientation, Vector3.UnitX);
            Vector3 localUnitY = Quaternion.quatRotate(orientation, Vector3.UnitY);
            Vector3 localUnitZ = Quaternion.quatRotate(orientation, Vector3.UnitZ);

            drawingSurface.drawCylinder(translation, localUnitX, localUnitY, localUnitZ, bone.Radius, bone.Height);

            float sizeLimit = bone.Height / 2;
            if (bone.Radius < sizeLimit)
            {
                sizeLimit = bone.Radius;
            }

            drawingSurface.drawAxes(translation, localUnitX, localUnitY, localUnitZ, sizeLimit);
        }

        internal Bone IkBone
        {
            get
            {
                return bone;
            }
        }

        public BEPUikScene Scene
        {
            get
            {
                return scene;
            }
        }
    }
}
