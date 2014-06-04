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
                simObject._unsetSimObjectManager();
            }
            simObjects.Clear();
        }

        /// <summary>
        /// Add a SimObject. It will be disposed when this manager is disposed.
        /// </summary>
        /// <param name="simObject">The SimObject to add.</param>
        public void addSimObject(SimObjectBase simObject)
        {
            simObjects.Add(simObject.Name, simObject);
            simObject._setSimObjectManager(this);
        }

        /// <summary>
        /// Remove a SimObject. This does not destroy it and the caller takes
        /// ownership of the object for disposal.
        /// </summary>
        /// <param name="simObject">The SimObject to remove.</param>
        public void removeSimObject(SimObjectBase simObject)
        {
            simObjects.Remove(simObject.Name);
            simObject._unsetSimObjectManager();
        }

        /// <summary>
        /// Determine if a SimObject exists.
        /// </summary>
        /// <param name="name">The name of the SimObject.</param>
        /// <returns>True if the SimObject is part of this manager.</returns>
        public bool hasSimObject(String name)
        {
            return name != null && simObjects.ContainsKey(name);
        }

        /// <summary>
        /// Get the SimObject specified by name.
        /// </summary>
        /// <param name="name">The name of the SimObject.</param>
        /// <returns>The matching SimObject.</returns>
        public SimObjectBase getSimObject(String name)
        {
            return simObjects[name];
        }

        /// <summary>
        /// Get the SimObject specified by name, will return false if the sim object is not found.
        /// </summary>
        /// <param name="name">The name of the SimObject</param>
        /// <param name="simObject">The out variable to put the result into.</param>
        /// <returns>True if the object is found, false if it is not.</returns>
        public bool tryGetSimObject(String name, out SimObjectBase simObject)
        {
            return simObjects.TryGetValue(name, out simObject);
        }

        /// <summary>
        /// Create and add a SimObject to this manager. This is used to create
        /// objects after the build process has run during scene execution.
        /// </summary>
        /// <param name="definition">The definition of the object to create.</param>
        public SimObjectBase createLiveSimObject(SimObjectDefinition definition)
        {
            if (!simObjects.ContainsKey(definition.Name))
            {
                SimObjectBase simObj = definition.register(subScene);
                addSimObject(simObj);
                subScene.buildScene();
                return simObj;
            }
            else
            {
                throw new SimObjectException(String.Format("Attempted to create a SimObject {0} that already exists in sub scene {1}.", definition.Name, subScene.Name));
            }
        }

        /// <summary>
        /// Destroy the SimObject named name. This will dispose the SimObject
        /// and it will no longer be part of the scene or usable.
        /// </summary>
        /// <param name="name">The name of the SimObject to destroy.</param>
        public void destroySimObject(String name)
        {
            SimObjectBase simObject = simObjects[name];
            simObjects.Remove(name);
            simObject.Dispose();
            simObject._unsetSimObjectManager();
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
                definition.addSimObject(simObjDef);
            }
            return definition;
        }

        public IEnumerable<SimObjectBase> SimObjects
        {
            get
            {
                return simObjects.Values;
            }
        }

        public SimSubScene SubScene
        {
            get
            {
                return subScene;
            }
        }
    }
}
