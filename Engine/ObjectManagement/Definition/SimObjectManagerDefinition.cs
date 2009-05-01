using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definition for a group of SimObjects.
    /// </summary>
    public class SimObjectManagerDefinition : Saveable
    {
        private Dictionary<String, SimObjectDefinition> simObjects = new Dictionary<string, SimObjectDefinition>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimObjectManagerDefinition()
        {

        }

        /// <summary>
        /// Add a SimObjectDefinition to use as a template.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        public void addSimObject(SimObjectDefinition definition)
        {
            simObjects.Add(definition.Name, definition);
        }

        /// <summary>
        /// Remove a SimObjectDefinition to use as a template.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeSimObject(SimObjectDefinition definition)
        {
            simObjects.Remove(definition.Name);
        }

        /// <summary>
        /// Get the template specified by name.
        /// </summary>
        /// <param name="name">The name of the template.</param>
        /// <returns>The template.</returns>
        public SimObjectDefinition getSimObject(String name)
        {
            return simObjects[name];
        }

        /// <summary>
        /// Returns a new SimObjectManager that contains all the SimObjects
        /// defined in this class. The SimObjects will be empty, but they will
        /// be registered for construction.
        /// </summary>
        /// <param name="subScene">The SimSubScene that will be used to build the objects in the returned manager.</param>
        /// <returns>A new SimObjectManager that contains empty SimObjects ready to be built into the given SimSubScene.</returns>
        public SimObjectManager createSimObjectManager(SimSubScene subScene)
        {
            SimObjectManager manager = new SimObjectManager(subScene);
            foreach (SimObjectDefinition instance in simObjects.Values)
            {
                SimObjectBase simObject = instance.register(subScene);
                manager.addSimObject(simObject);
            }
            return manager;
        }

        #region Saveable Members

        private const string SIM_OBJECT_BASE = "SimObject";

        private SimObjectManagerDefinition(LoadInfo info)
        {
            for(int i = 0; info.hasValue(SIM_OBJECT_BASE + i); i++)
            {
                addSimObject(info.GetValue<SimObjectDefinition>(SIM_OBJECT_BASE + i));
            }
        }

        public void getInfo(SaveInfo info)
        {
            int i = 0;
            foreach (SimObjectDefinition simObject in simObjects.Values)
            {
                info.AddValue(SIM_OBJECT_BASE + i++, simObject);
            }
        }

        #endregion
    }
}
