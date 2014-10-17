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
        public delegate void SolverEvent(BEPUikSolver solver);

        private List<BEPUikBone> bones = new List<BEPUikBone>();
        private List<BEPUikControl> controls = new List<BEPUikControl>();
        private List<ExternalControl> externalControls = new List<ExternalControl>();
        private List<Control> solveControls = new List<Control>(); //Prevents garbage, this list has the same contents as controls, but holds direct references to the bepuik control class that is passed to the solver
        private List<Bone> solveBones = new List<Bone>(); //Prevents garbage, holds direct list to BEPUik bone instances for all bones loaded into this solver.
        private IKSolver ikSolver = new IKSolver();
        private List<BEPUikSolver> childSolvers = new List<BEPUikSolver>();
        private bool updatedThisTick = false;

        public event SolverEvent BeforeUpdate;

        internal BEPUikSolver(BEPUikSolverDefinition definition)
        {
            this.Name = definition.Name;
            ikSolver.ActiveSet.UseAutomass = definition.ActiveSetUseAutomass;
            ikSolver.AutoscaleControlImpulses = definition.AutoscaleControlImpulses;
            ikSolver.AutoscaleControlMaximumForce = definition.AutoscaleControlMaximumForce;
            ikSolver.TimeStepDuration = definition.TimeStepDuration;
            ikSolver.ControlIterationCount = definition.ControlIterationCount;
            ikSolver.FixerIterationCount = definition.FixerIterationCount;
            ikSolver.VelocitySubiterationCount = definition.VelocitySubiterationCount;

            foreach(var childSolverDef in definition.ChildSolvers)
            {
                childSolvers.Add(new BEPUikSolver(childSolverDef));
            }
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
                if(BeforeUpdate != null)
                {
                    BeforeUpdate.Invoke(this);
                }
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
                if (BeforeUpdate != null)
                {
                    BeforeUpdate.Invoke(this);
                }

                ikSolver.Solve(solveBones);

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

        internal void addBone(BEPUikBone bone)
        {
            bones.Add(bone);
            solveBones.Add(bone.IkBone);
        }

        internal void removeBone(BEPUikBone bone)
        {
            bones.Remove(bone);
            solveBones.Remove(bone.IkBone);
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

        internal void addExternalControl(ExternalControl control)
        {
            control.CurrentSolverName = Name;
            externalControls.Add(control);
            solveControls.Add(control.IKControl);
        }

        internal void removeExternalControl(ExternalControl control)
        {
            control.CurrentSolverName = null;
            externalControls.Remove(control);
            solveControls.Remove(control.IKControl);
        }

        internal void fillOutDefinition(BEPUikSolverDefinition definition)
        {
            definition.ActiveSetUseAutomass = ikSolver.ActiveSet.UseAutomass;
            definition.AutoscaleControlImpulses = ikSolver.AutoscaleControlImpulses;
            definition.AutoscaleControlMaximumForce = ikSolver.AutoscaleControlMaximumForce;
            definition.TimeStepDuration = ikSolver.TimeStepDuration;
            definition.ControlIterationCount = ikSolver.ControlIterationCount;
            definition.FixerIterationCount = ikSolver.FixerIterationCount;
            definition.VelocitySubiterationCount = ikSolver.VelocitySubiterationCount;
            definition.Name = Name;
            foreach(var child in childSolvers)
            {
                var childDefinition = new BEPUikSolverDefinition();
                child.fillOutDefinition(childDefinition);
                definition.addChildSolver(childDefinition);
            }
        }

        internal IKSolver IKSolver
        {
            get
            {
                return ikSolver;
            }
        }

        internal IEnumerable<BEPUikSolver> ChildSolvers
        {
            get
            {
                return childSolvers;
            }
        }

        public String Name { get; private set; }
    }
}
