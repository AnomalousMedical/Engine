using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikScene : SimElementManager
    {
        private String name;
        private BEPUIkFactory factory;
        private List<BEPUikBone> bones = new List<BEPUikBone>();
        private List<BEPUikControl> controls = new List<BEPUikControl>();
        private List<Control> solveControls = new List<Control>(); //Prevents garbage, this list has the same contents as controls, but holds direct references to the bepuik control class that is passed to the solver
        private IKSolver ikSolver = new IKSolver();
        private UpdateTimer timer;
        private BEPUikSceneUpdater updater;

        public BEPUikScene(BEPUikSceneDefinition definition, UpdateTimer timer)
        {
            this.timer = timer;
            this.name = definition.Name;
            factory = new BEPUIkFactory(this);
            updater = new BEPUikSceneUpdater(this);
            timer.addFullSpeedUpdateListener(updater);

            ikSolver.ActiveSet.UseAutomass = definition.ActiveSetUseAutomass;
            ikSolver.AutoscaleControlImpulses = definition.AutoscaleControlImpulses;
            ikSolver.AutoscaleControlMaximumForce = definition.AutoscaleControlMaximumForce;
            ikSolver.TimeStepDuration = definition.TimeStepDuration;
            ikSolver.ControlIterationCount = definition.ControlIterationCount;
            ikSolver.FixerIterationCount = definition.FixerIterationCount;
            ikSolver.VelocitySubiterationCount = definition.VelocitySubiterationCount;
        }

        public void Dispose()
        {
            timer.removeFullSpeedUpdateListener(updater);
            ikSolver.Dispose();
        }

        public SimElementFactory getFactory()
        {
            return factory;
        }

        internal BEPUIkFactory IkFactory
        {
            get
            {
                return factory;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikScene);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            return new BEPUikSceneDefinition(name)
                {
                    ActiveSetUseAutomass = ikSolver.ActiveSet.UseAutomass,
                    AutoscaleControlImpulses = ikSolver.AutoscaleControlImpulses,
                    AutoscaleControlMaximumForce = ikSolver.AutoscaleControlMaximumForce,
                    TimeStepDuration = ikSolver.TimeStepDuration,
                    ControlIterationCount = ikSolver.ControlIterationCount,
                    FixerIterationCount = ikSolver.FixerIterationCount,
                    VelocitySubiterationCount = ikSolver.VelocitySubiterationCount
                };
        }

        internal void addBone(BEPUikBone bone)
        {
            bones.Add(bone);
        }

        internal void removeBone(BEPUikBone bone)
        {
            bones.Remove(bone);
        }

        internal void addControl(BEPUikControl control)
        {
            controls.Add(control);
            solveControls.Add(control.IKControl);
        }

        internal void removeControl(BEPUikControl control)
        {
            controls.Remove(control);
            solveControls.Remove(control.IKControl);
        }

        public void addExternalControl(Control control)
        {
            solveControls.Add(control);
        }

        public void removeExternalControl(Control control)
        {
            solveControls.Remove(control);
        }

        public void update()
        {
            PerformanceMonitor.start("BEPU IK");
            if (solveControls.Count > 0)
            {
                foreach (var control in controls)
                {
                    control.syncPosition();
                }

                ikSolver.Solve(solveControls);

                foreach (var bone in bones)
                {
                    bone.syncSimObject();
                }
            }
            PerformanceMonitor.stop("BEPU IK");
        }

        internal void drawDebug(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.begin("BepuIkScene", Engine.Renderer.DrawingType.LineList);
            foreach(var bone in bones)
            {
                draw(drawingSurface, bone);
            }
            drawingSurface.end();
        }

        private void draw(DebugDrawingSurface drawingSurface, BEPUikBone bone)
        {
            Bone ikBone = bone.IkBone;
            drawingSurface.setColor(Color.Purple);

            Quaternion orientation = ikBone.Orientation.toEngineQuat();
            Vector3 translation = ikBone.Position.toEngineVec3();
            Vector3 localUnitX = Quaternion.quatRotate(orientation, Vector3.UnitX);
            Vector3 localUnitY = Quaternion.quatRotate(orientation, Vector3.UnitY);
            Vector3 localUnitZ = Quaternion.quatRotate(orientation, Vector3.UnitZ);

            drawingSurface.drawCylinder(translation, localUnitX, localUnitY, localUnitZ, ikBone.Radius, ikBone.Height);

            float sizeLimit = ikBone.Height;
            if(ikBone.Radius < ikBone.Height)
            {
                sizeLimit = ikBone.Radius;
            }

            drawingSurface.setColor(Color.Red);
            drawingSurface.drawLine(translation, translation + localUnitX * sizeLimit);

            drawingSurface.setColor(Color.Blue);
            drawingSurface.drawLine(translation, translation + localUnitY * sizeLimit);

            drawingSurface.setColor(Color.Green);
            drawingSurface.drawLine(translation, translation + localUnitZ * sizeLimit);
        }
    }
}
