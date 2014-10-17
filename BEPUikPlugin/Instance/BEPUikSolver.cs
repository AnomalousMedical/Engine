using BEPUik;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikSolver : IDisposable
    {
        private List<BEPUikBone> bones = new List<BEPUikBone>();
        private List<BEPUikControl> controls = new List<BEPUikControl>();
        private List<ExternalControl> externalControls = new List<ExternalControl>();
        private List<Control> solveControls = new List<Control>(); //Prevents garbage, this list has the same contents as controls, but holds direct references to the bepuik control class that is passed to the solver
        private IKSolver ikSolver = new IKSolver();
        private List<BEPUikSolver> childSolvers = new List<BEPUikSolver>();
        private bool updatedThisTick = false;

        internal BEPUikSolver(BEPUikSceneDefinition definition, String name)
        {
            this.Name = name;
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
            ikSolver.Dispose();
        }

        internal void drawDebug(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            foreach (var bone in bones)
            {
                if ((drawMode & DebugDrawMode.Bones) != 0)
                {
                    bone.draw(drawingSurface);
                }
                foreach (var joint in bone.IkBone.Joints)
                {
                    ((BEPUikConstraint)joint.UserObject).draw(drawingSurface, drawMode);
                }
            }
            if ((drawMode & DebugDrawMode.Controls) != 0)
            {
                foreach (var control in externalControls)
                {
                    control.draw(drawingSurface, drawMode);
                }
                foreach (var control in controls)
                {
                    control.draw(drawingSurface, drawMode);
                }
            }
            foreach(var child in childSolvers)
            {
                child.drawDebug(drawingSurface, drawMode);
            }
        }

        internal void update(bool parentUpdated)
        {
            if (solveControls.Count > 0)
            {
                foreach (var control in controls)
                {
                    control.syncPosition();
                }

                ikSolver.Solve(solveControls);
                parentUpdated = true;
                updatedThisTick = true;
            }
            else if (parentUpdated)
            {
                //Crappy way to force joint update, come up with something better
                List<IKJoint> joints = new List<IKJoint>();
                foreach(var bone in bones)
                {
                    foreach(var joint in bone.IkBone.Joints)
                    {
                        if(!joints.Contains(joint))
                        {
                            joints.Add(joint);
                        }
                    }
                }
                ikSolver.Solve(joints);

                updatedThisTick = true;
            }

            foreach (var child in childSolvers)
            {
                child.update(parentUpdated);
            }
        }

        internal void sync()
        {
            if (updatedThisTick)
            {
                foreach (var bone in bones)
                {
                    if (bone.IkBone.IsActive)
                    {
                        bone.syncSimObject();
                    }
                }
                updatedThisTick = false;
            }
            foreach (var child in childSolvers)
            {
                child.sync();
            }
        }

        internal void addChildSolver(BEPUikSolver childSolver)
        {
            childSolvers.Add(childSolver);
        }

        internal void removeChildSolver(BEPUikSolver childSolver)
        {
            childSolvers.Remove(childSolver);
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

        public void addExternalControl(ExternalControl control)
        {
            control.CurrentSolverName = Name;
            externalControls.Add(control);
            solveControls.Add(control.IKControl);
        }

        public void removeExternalControl(ExternalControl control)
        {
            control.CurrentSolverName = null;
            externalControls.Remove(control);
            solveControls.Remove(control.IKControl);
        }

        internal IKSolver IKSolver
        {
            get
            {
                return ikSolver;
            }
        }

        public String Name { get; private set; }
    }
}
