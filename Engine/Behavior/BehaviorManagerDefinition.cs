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
        private EditInterface editInterface;
        private String name;

        public BehaviorManagerDefinition(String name)
        {
            this.name = name;
        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name + " Behavior Manager");
            }
            return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            BehaviorManager behaviorManager = new BehaviorManager(name);
            return behaviorManager;
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

        public void Dispose()
        {

        }

        #region Saveable

        private BehaviorManagerDefinition(LoadInfo info)
        {

        }

        public void getInfo(SaveInfo info)
        {
            
        }

        #endregion Saveable
    }
}
