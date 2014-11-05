using Engine.Attributes;
using Engine.Editing;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikSolverDefinition : Saveable
    {
        //Note that these defaults are taken from the InverseKinematicsTestDemo not the actual defaults for IKSolver.
        //These settings seem to be more stable in general.
        private bool activeSetUseAutomass = true;
        private bool autoscaleControlImpulses = true;
        private float autoscaleControlMaximumForce = float.MaxValue;
        private float timeStepDuration = .1f;
        private int controlIterationCount = 100;
        private int fixerIterationCount = 10;
        private int velocitySubiterationCount = 3;
        private bool autosolveOnParentUpdate = true;

        [DoNotSave]
        private List<BEPUikSolverDefinition> childSolvers = new List<BEPUikSolverDefinition>();

        [DoNotSave]
        private EditInterface editInterface;

        [DoNotSave]
        private EditInterfaceManager<BEPUikSolverDefinition> childSolversManager;

        public BEPUikSolverDefinition()
        {

        }

        public void addChildSolver(BEPUikSolverDefinition child)
        {
            childSolvers.Add(child);
            if(childSolversManager != null)
            {
                childSolversManager.addSubInterface(child, child.getEditInterface());
            }
        }

        public void removeChildSolver(BEPUikSolverDefinition child)
        {
            childSolvers.Remove(child);
            if (childSolversManager != null)
            {
                childSolversManager.removeSubInterface(child);
            }
        }

        [Editable]
        public string Name { get; set; }

        [Editable]
        public bool ActiveSetUseAutomass
        {
            get
            {
                return activeSetUseAutomass;
            }
            set
            {
                activeSetUseAutomass = value;
            }
        }

        [Editable]
        public bool AutoscaleControlImpulses
        {
            get
            {
                return autoscaleControlImpulses;
            }
            set
            {
                autoscaleControlImpulses = value;
            }
        }

        [Editable]
        public float AutoscaleControlMaximumForce
        {
            get
            {
                return autoscaleControlMaximumForce;
            }
            set
            {
                autoscaleControlMaximumForce = value;
            }
        }

        [EditableMinMax(0.0000000001, float.MaxValue, 0.1)]
        public float TimeStepDuration
        {
            get
            {
                return timeStepDuration;
            }
            set
            {
                timeStepDuration = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of solver iterations to perform in an attempt to reach specified goals.
        /// Increases the speed at which the goal is reached.
        /// </summary>
        [Editable]
        public int ControlIterationCount
        {
            get
            {
                return controlIterationCount;
            }
            set
            {
                controlIterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of solter iterations to perform after the control iterations in an attempt to minimize
        /// errors introduced by unreachable goals.
        /// Reduces jitter when goal can't be reached.
        /// </summary>
        [Editable]
        public int FixerIterationCount
        {
            get
            {
                return fixerIterationCount;
            }
            set
            {
                fixerIterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of velocity iterations to perform per control or fixer iteration.
        /// Just a multiplier?
        /// </summary>
        [Editable]
        public int VelocitySubiterationCount
        {
            get
            {
                return velocitySubiterationCount;
            }
            set
            {
                velocitySubiterationCount = value;
            }
        }

        [Editable]
        public bool AutosolveOnParentUpdate
        {
            get
            {
                return autosolveOnParentUpdate;
            }
            set
            {
                autosolveOnParentUpdate = value;
            }
        }

        /// <summary>
        /// Get all child solvers from this node all the way down the solver tree.
        /// </summary>
        internal IEnumerable<BEPUikSolverDefinition> ChildSolvers
        {
            get
            {
                foreach(var child in childSolvers)
                {
                    yield return child;
                    foreach(var innerChild in child.ChildSolvers)
                    {
                        yield return innerChild;
                    }
                }
            }
        }

        internal EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, Name + " - IK Solver", null);
                editInterface.addCommand(new EditInterfaceCommand("Add Child Solver", (callback, command) =>
                    {
                        addChildSolver(new BEPUikSolverDefinition()
                        {
                            Name = "Child"
                        });
                    }));
                childSolversManager = editInterface.createEditInterfaceManager<BEPUikSolverDefinition>();
                childSolversManager.addCommand(new EditInterfaceCommand("Remove", (callback, command) =>
                    {
                        removeChildSolver(childSolversManager.resolveSourceObject(callback.getSelectedEditInterface()));
                    }));
                foreach(var child in childSolvers)
                {
                    childSolversManager.addSubInterface(child, child.getEditInterface());
                }
            }
            return editInterface;
        }

        protected BEPUikSolverDefinition(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info);
            info.RebuildList("ChildSolver", childSolvers);
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
            info.ExtractList("ChildSolver", childSolvers);
        }
    }
}
