using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikSceneDefinition : SimElementManagerDefinition
    {
        internal static SimElementManagerDefinition Create(string name, EditUICallback callback)
        {
            return new BEPUikSceneDefinition(name);
        }

        private String name;

        [DoNotSave]
        private EditInterface editInterface;

        private BEPUikSolverDefinition rootSolver;

        public BEPUikSceneDefinition(String name)
        {
            this.name = name;
            rootSolver = new BEPUikSolverDefinition();
            rootSolver.Name = "Root";
        }

        public void Dispose()
        {

        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, name + " - BEPUIk Scene", null);
                editInterface.addSubInterface(rootSolver.getEditInterface());
            }
            return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            return BEPUikInterface.Instance.createScene(this);
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public BEPUikSolverDefinition RootSolverDefinition
        {
            get
            {
                return rootSolver;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikSceneDefinition);
        }

        protected BEPUikSceneDefinition(LoadInfo info)
        {
            if(info.Version == 0)
            {
                rootSolver = new BEPUikSolverDefinition();
                rootSolver.Name = "Root";
                rootSolver.ActiveSetUseAutomass = info.GetBoolean("activeSetUseAutomass");
                rootSolver.AutoscaleControlImpulses = info.GetBoolean("autoscaleControlImpulses");
                rootSolver.AutoscaleControlMaximumForce = info.GetFloat("autoscaleControlMaximumForce");
                rootSolver.ControlIterationCount = info.GetInt32("controlIterationCount");
                rootSolver.FixerIterationCount = info.GetInt32("fixerIterationCount");
                rootSolver.TimeStepDuration = info.GetFloat("timeStepDuration");
                rootSolver.VelocitySubiterationCount = info.GetInt32("velocitySubiterationCount");
            }
            else
            {
                rootSolver = info.GetValue<BEPUikSolverDefinition>("RootSolver");
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.Version = 1;
            info.AddValue("RootSolver", rootSolver);
        }
    }
}
