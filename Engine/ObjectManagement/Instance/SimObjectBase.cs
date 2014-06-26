using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is the class new SimObjects should be extended from as it provides
    /// the entire interface.
    /// </summary>
    public abstract class SimObjectBase : SimObject, IDisposable
    {
        private SimObjectManager simObjectManager;

        /// <summary>
        /// Dispose function. Destroys subsystem objects, which may or may not
        /// be unmanaged. Do not call this function directly. The
        /// SimObjectManager will take care of it.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public abstract void addElement(SimElement element);

        /// <summary>
        /// Add an element that will take this sim object as an owner, but will
        /// not show up in the sim object itself. This means that the .Owner
        /// property of the element will report this sim object, but the object
        /// will not be accessible through this SimObject and will not be
        /// managed in any way by the SimObject.
        /// </summary>
        /// <param name="element">The element to add as a weak element.</param>
        public void addWeakElement(SimElement element)
        {
            element.setSimObject(this);
        }

        /// <summary>
        /// Remove a SimElement as a weak element setting its sim object back to null.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public void removeWeakElement(SimElement element)
        {
            element.setSimObject(null);
        }

        /// <summary>
        /// Update the position of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateTranslation(ref Vector3 translation, SimElement trigger);

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateRotation(ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateScale(ref Vector3 scale, SimElement trigger);

        /// <summary>
        /// Get a particular SimElement from the SimObject. This will return
        /// null if the element cannot be found. This method could potentially
        /// be fairly slow so it is best to cache the values returned from this
        /// function somehow.
        /// </summary>
        /// <param name="name">The name of the SimElement to retrieve.</param>
        /// <returns>The SimElement specified by name or null if it cannot be found.</returns>
        public abstract SimElement getElement(String name);

        /// <summary>
        /// Get an iterator over all sim elements.
        /// </summary>
        /// <returns>An IEnumerable over all sim elements.</returns>
        public abstract IEnumerable<SimElement> getElementIter();

        public SimObject getOtherSimObject(String name)
        {
            if (name == "this")
            {
                return this;
            }
            else
            {
                if (simObjectManager != null)
                {
                    SimObjectBase simObj;
                    simObjectManager.tryGetSimObject(name, out simObj);
                    return simObj;
                }
                return null;
            }
        }

        public SimObject createOtherSimObject(SimObjectDefinition definition)
        {
            if (simObjectManager != null)
            {
                return simObjectManager.createLiveSimObject(definition);
            }
            else
            {
                throw new SimObjectException(String.Format("Could not create another SimObject using {0} because it is not registered.", Name));
            }
        }

        public void destroy()
        {
            simObjectManager.destroySimObject(Name);
        }

        /// <summary>
        /// Save this SimObject to a SimObjectDefinition.
        /// </summary>
        /// <param name="definitionName">The name to give the SimObjectDefinition.</param>
        /// <returns>A new SimObjectDefinition for this SimObject.</returns>
        public abstract SimObjectDefinition saveToDefinition(String definitionName);

        /// <summary>
        /// This function will set the SimObjectManager used to lookup other SimObjects. 
        /// It should only be called by the SimObjectManager.
        /// </summary>
        /// <param name="manager">The SimObjectManager this SimObject now belongs to.</param>
        internal void _setSimObjectManager(SimObjectManager manager)
        {
            simObjectManager = manager;
        }

        /// <summary>
        /// This function will unset the SimObjectManager used to lookup other SimObjects.
        /// It should only be called by the SimObjectManager.
        /// </summary>
        internal void _unsetSimObjectManager()
        {
            simObjectManager = null;
        }

        /// <summary>
        /// Get the name of this SimObject.
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// Get the enabled status of this SimObject.
        /// </summary>
        public abstract bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Get the translation of this SimObject.
        /// </summary>
        public abstract Vector3 Translation
        {
            get;
        }

        /// <summary>
        /// Get the rotation of the SimObject.
        /// </summary>
        public abstract Quaternion Rotation
        {
            get;
        }

        /// <summary>
        /// Get the scale of the SimObject.
        /// </summary>
        public abstract Vector3 Scale
        {
            get;
        }

        public SimSubScene SubScene
        {
            get
            {
                return simObjectManager != null ? simObjectManager.SubScene : null;
            }
        }
    }
}
