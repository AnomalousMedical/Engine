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
        internal static BehaviorManagerDefinition Create(String name)
        {
            return new BehaviorManagerDefinition(name);
        }

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
            BehaviorManager behaviorManager = new BehaviorManager(name, BehaviorInterface.Instance.Timer, BehaviorInterface.Instance.EventManager);
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

        private const string NAME = "Name";

        private BehaviorManagerDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
        }

        #endregion Saveable
    }
}
