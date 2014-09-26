using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Engine.Saving;

namespace Engine
{
    class BehaviorManagerDefinition : SimElementManagerDefinition
    {
        internal static BehaviorManagerDefinition Create(String name, EditUICallback callback)
        {
            return new BehaviorManagerDefinition(name);
        }

        private EditInterface editInterface;
        private EditInterface phasesEditInterface;
        private String name;
        private List<BehaviorUpdatePhase> updatePhases = new List<BehaviorUpdatePhase>();

        public BehaviorManagerDefinition(String name)
        {
            this.name = name;
            updatePhases.Add(new BehaviorUpdatePhase("Default"));
        }

        public void Dispose()
        {

        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name + " Behavior Manager");

                phasesEditInterface = new EditInterface("Update Phases",
                    uiCallback =>
                    {
                        addUpdatePhase(new BehaviorUpdatePhase());
                    },
                    (uiCallback, property) =>
                    {
                        removeUpdatePhase((BehaviorUpdatePhase)property);
                    }, null);
                EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
                propertyInfo.addColumn(new EditablePropertyColumn("Name", false));
                phasesEditInterface.setPropertyInfo(propertyInfo);
                foreach (var phase in updatePhases)
                {
                    addPhaseProperty(phase);
                }

                editInterface.addSubInterface(phasesEditInterface);
            }
            return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            BehaviorManager behaviorManager = new BehaviorManager(name, BehaviorPluginInterface.Instance.Timer, BehaviorPluginInterface.Instance.EventManager);
            foreach(var phase in updatePhases)
            {
                behaviorManager.addUpdatePhase(phase.Name);
            }
            return behaviorManager;
        }

        public void addUpdatePhase(BehaviorUpdatePhase name)
        {
            updatePhases.Add(name);
            addPhaseProperty(name);
        }

        public void removeUpdatePhase(BehaviorUpdatePhase name)
        {
            updatePhases.Remove(name);
            removePhaseProperty(name);
        }

        public string Name
        {
            get 
            {
                return name;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BehaviorManager);
        }

        private void addPhaseProperty(BehaviorUpdatePhase phase)
        {
            if (phasesEditInterface != null)
            {
                phasesEditInterface.addEditableProperty(phase);
            }
        }

        private void removePhaseProperty(BehaviorUpdatePhase phase)
        {
            if (phasesEditInterface != null)
            {
                phasesEditInterface.removeEditableProperty(phase);
            }
        }

        #region Saveable

        private BehaviorManagerDefinition(LoadInfo info)
        {
            name = info.GetString("Name");
            info.RebuildList("UpdatePhase", updatePhases);
            if (info.Version == 0)
            {
                //Add a default phase if this version is 0.
                updatePhases.Add(new BehaviorUpdatePhase("Default"));
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.Version = 1;
            info.AddValue("Name", name);
            info.ExtractList("UpdatePhase", updatePhases);
        }

        #endregion Saveable
    }
}
