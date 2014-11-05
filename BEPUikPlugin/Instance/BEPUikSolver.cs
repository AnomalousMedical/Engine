using BEPUik;
using BEPUutilities;
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

        /// <summary>
        /// This event is fired before the solver updates on the thread that the solver updates on.
        /// </summary>
        public event SolverEvent BeforeUpdate;

        /// <summary>
        /// This event is fired after the solver syncs its positions on the thread that sync occurs on. This will only
        /// fire if this solver acutally updated this frame (updatedThisTick == true).
        /// </summary>
        public event SolverEvent AfterSync;

        internal BEPUikSolver(BEPUikSolverDefinition definition)
        {
            this.Name = definition.Name;
            this.AutosolveOnParentUpdate = definition.AutosolveOnParentUpdate;
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
            if (HasControlsToSolve) //Control solve
            {
                if (BeforeUpdate != null)
                {
                    BeforeUpdate.Invoke(this);
                }

                //Seed needupdate, will be true if there are external controls or the parent was updated
                bool needUpdate = externalControls.Count > 0 || parentUpdated;

                foreach (var control in controls)
                {
                    //See if any controls moved
                    needUpdate |= control.MovedThisTick;
                    control.MovedThisTick = false;
                    control.syncPosition();
                }

                //Something requires us to update
                if (needUpdate)
                {
                    ikSolver.Solve(solveControls);
                    parentUpdated = true;
                    updatedThisTick = true;
                }
            }
            else if (AutosolveOnParentUpdate && parentUpdated) //Solve bones if parent updated and no controls
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

                if(AfterSync != null)
                {
                    AfterSync.Invoke(this);
                }
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
            control.MovedThisTick = false;
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
            definition.AutosolveOnParentUpdate = AutosolveOnParentUpdate;
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

        public bool HasControlsToSolve
        {
            get
            {
                return solveControls.Count > 0;
            }
        }

        public bool AutosolveOnParentUpdate { get; set; }
    }
}
