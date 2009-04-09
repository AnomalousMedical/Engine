using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    public class SimSceneDefinition
    {
        #region Fields

        private Dictionary<String, SimElementManagerDefinition> elementManagers = new Dictionary<String,SimElementManagerDefinition>();
        private Dictionary<String, SimSubSceneDefinition> subSceneDefinitions = new Dictionary<string, SimSubSceneDefinition>();
        private EditInterface editInterface;
        
        #endregion Fields

        #region Constructors

        public SimSceneDefinition()
        {
            
        }

        #endregion Constructors

        #region Functions

        public void addSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Add(def.Name, def);
        }

        public void removeSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Remove(def.Name);
        }

        public bool hasSimElementManagerDefinition(String name)
        {
            return elementManagers.ContainsKey(name);
        }

        public void addSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Add(def.Name, def);
        }

        public void removeSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Remove(def.Name);
        }

        public bool hasSimSubSceneDefinition(String name)
        {
            return subSceneDefinitions.ContainsKey(name);
        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new SimSceneEditInterface(this);
            }
            return editInterface;
        }

        #endregion Functions
    }
}
