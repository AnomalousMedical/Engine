using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class manages a series of SimObjects that are part of a SubScene.
    /// </summary>
    public class SimObjectManager : IDisposable
    {
        private Dictionary<String, SimObject> simObjects = new Dictionary<string, SimObject>();
        private SimSubScene subScene;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="subScene">The SubScene that owns the objects.</param>
        public SimObjectManager(SimSubScene subScene)
        {
            this.subScene = subScene;
        }

        /// <summary>
        /// Dispose funciton. Will dispose all SimObjects.
        /// </summary>
        public void Dispose()
        {
            foreach (SimObject simObject in simObjects.Values)
            {
                simObject.Dispose();
            }
        }

        /// <summary>
        /// Add a SimObject. It will be disposed when this manager is disposed.
        /// </summary>
        /// <param name="simObject">The SimObject to add.</param>
        public void addSimObject(SimObject simObject)
        {
            simObjects.Add(simObject.Name, simObject);
        }

        /// <summary>
        /// Remove a SimObject.
        /// </summary>
        /// <param name="simObject">The SimObject to remove.</param>
        public void removeSimObject(SimObject simObject)
        {
            simObjects.Remove(simObject.Name);
        }

        /// <summary>
        /// Create a new SimObjectManagerDefinition that can recreate this
        /// SimObjectManager exactly how it is when this function is called.
        /// </summary>
        /// <returns>A new SimObjectManagerDefinition configured appropriatly.</returns>
        public SimObjectManagerDefinition saveToDefinition()
        {
            SimObjectManagerDefinition definition = new SimObjectManagerDefinition();
            foreach (SimObject simObject in simObjects.Values)
            {
                SimObjectDefinition simObjDef = simObject.saveToDefinition(simObject.Name);
                definition.addTemplate(simObjDef);
                SimObjectInstanceDefinition instance = new SimObjectInstanceDefinition(simObject.Name);
                instance.DefinitionName = simObjDef.Name;
                instance.Enabled = simObject.Enabled;
                instance.Rotation = simObject.Rotation;
                instance.Scale = simObject.Scale;
                instance.Translation = simObject.Translation;
                definition.addInstanceDefinition(instance);
            }
            return definition;
        }
    }
}
