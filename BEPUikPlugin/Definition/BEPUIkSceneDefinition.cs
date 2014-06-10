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

        //Note that these defaults are taken from the InverseKinematicsTestDemo not the actual defaults for IKSolver.
        //These settings seem to be more stable in general.
        private bool activeSetUseAutomass = true;
        private bool autoscaleControlImpulses = true;
        private float autoscaleControlMaximumForce = float.MaxValue;
        private float timeStepDuration = .1f;
        private int controlIterationCount = 100;
        private int fixerIterationCount = 10;
        private int velocitySubiterationCount = 3;

        public BEPUikSceneDefinition(String name)
        {
            this.name = name;
        }

        public void Dispose()
        {

        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, name + " - BEPUIk Scene", null);
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

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikSceneDefinition);
        }

        protected BEPUikSceneDefinition(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info); 
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
        }
    }
}
