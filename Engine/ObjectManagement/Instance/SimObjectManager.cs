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
        private Dictionary<String, SimObjectBase> simObjects = new Dictionary<string, SimObjectBase>();
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
            foreach (SimObjectBase simObject in simObjects.Values)
            {
                simObject.Dispose();
            }
        }

        /// <summary>
        /// Add a SimObject. It will be disposed when this manager is disposed.
        /// </summary>
        /// <param name="simObject">The SimObject to add.</param>
        public void addSimObject(SimObjectBase simObject)
        {
            simObjects.Add(simObject.Name, simObject);
        }

        /// <summary>
        /// Remove a SimObject.
        /// </summary>
        /// <param name="simObject">The SimObject to remove.</param>
        public void removeSimObject(SimObjectBase simObject)
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
            foreach (SimObjectBase simObject in simObjects.Values)
            {
                SimObjectDefinition simObjDef = simObject.saveToDefinition(simObject.Name);
                definition.addTemplate(simObjDef);
                SimObjectInstanceDefinition instance = simObject.saveInstanceDefinition();
                instance.DefinitionName = simObjDef.Name;
                definition.addInstanceDefinition(instance);
            }
            return definition;
        }
    }
}
