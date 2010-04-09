using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Saving;
using Engine.Editing;

namespace Engine.Resources
{
    /// <summary>
    /// The resource manager is a subsystem independent way of representing resources in 
    /// the engine.  It does not handle any file loading or unloading, but instead simply
    /// provides a list of resources that can be split into various groups if requested.
    /// It can also be merged with another resource manager, which will cause events to be
    /// fired as the merge occures to say if a group or specific resource has been added/removed.
    /// This way any common resources between the two ResourceManagers can stay in memory
    /// making the transition between two of them go quicker.
    /// </summary>
    public class ResourceManager : Saveable
    {
        #region Fields

        Dictionary<String, SubsystemResources> subsystemResources = new Dictionary<string, SubsystemResources>();
        private EditInterface editInterface;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        internal ResourceManager()
        {

        }

        /// <summary>
        /// Constructor.  Takes an existing resource manager and duplicates
        /// its contents into the new ResourceManager.
        /// </summary>
        /// <param name="toDuplicate">The resource manager to duplicate.</param>
        internal ResourceManager(ResourceManager toDuplicate)
        {
            foreach (SubsystemResources resource in toDuplicate.subsystemResources.Values)
            {
                this.addSubsystemResource(new SubsystemResources(resource));
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a subsystem resource to this resource manager.
        /// </summary>
        /// <param name="resources">The subsystem resources to add.</param>
        internal void addSubsystemResource(SubsystemResources resources)
        {
            if (!subsystemResources.ContainsKey(resources.Name))
            {
                subsystemResources.Add(resources.Name, resources);
            }
            else
            {
                Log.Default.sendMessage("The subsystem {0} has resources defined already.  Ignoring new resources.", LogLevel.Error, "ResourceManagement", resources.Name);
            }
        }

        /// <summary>
        /// Remove a subsystem resource from this manager.
        /// </summary>
        /// <param name="resources">The subsystem resources to remove.</param>
        internal void removeSubsystemResource(SubsystemResources resources)
        {
            if (subsystemResources.ContainsKey(resources.Name))
            {
                subsystemResources.Remove(resources.Name);
            }
            else
            {
                Log.Default.sendMessage("The subsystem {0} does not have any resources defined so it cannot be removed.  No changes made.", LogLevel.Warning, "ResourceManagement", resources.Name);
            }
        }

        /// <summary>
        /// Remove a subsystem resources based on its name.
        /// </summary>
        /// <param name="name">The name of the resources to remove.</param>
        internal void removeSubsystemResource(String name)
        {
            if (subsystemResources.ContainsKey(name))
            {
                subsystemResources.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("The subsystem {0} does not have any resources defined so it cannot be removed.  No changes made.", LogLevel.Warning, "ResourceManagement", name);
            }
        }

        /// <summary>
        /// Return the subsystem resources with the given name.
        /// </summary>
        /// <param name="name">The name of the resources to get.</param>
        /// <returns>The subsystem resources identified by name or null if this manager does not contain it.</returns>
        public SubsystemResources getSubsystemResource(String name)
        {
            if (subsystemResources.ContainsKey(name))
            {
                return subsystemResources[name];
            }
            else
            {
                Log.Default.sendMessage("The subsystem {0} does not have any resources defined.  Null returned.", LogLevel.Warning, "ResourceManagement", name);
                return null;
            }
        }

        /// <summary>
        /// Modify the contents of this resource manager to match the contents of toMatch.
        /// This will fire the add/remove events for groups and individual resources to all
        /// subscribed delegates.
        /// </summary>
        /// <param name="toMatch">The resource manager to match.</param>
        public void changeResourcesToMatch(ResourceManager toMatch)
        {
            //Unload any non matching subsystems
            LinkedList<String> unloadedResources = new LinkedList<String>();
            foreach (SubsystemResources resource in subsystemResources.Values)
            {
                if (!toMatch.subsystemResources.ContainsKey(resource.Name))
                {
                    resource.sendUnloadSignals();
                    unloadedResources.AddLast(resource.Name);
                }
            }

            //Remove unloaded resources
            foreach (String name in unloadedResources)
            {
                subsystemResources.Remove(name);
            }

            //Add any new resources
            foreach (SubsystemResources resource in toMatch.subsystemResources.Values)
            {
                if (!subsystemResources.ContainsKey(resource.Name))
                {
                    SubsystemResources sub = new SubsystemResources(resource.Name);
                    subsystemResources.Add(resource.Name, sub);
                }
                subsystemResources[resource.Name].changeResourcesToMatch(resource);
            }
        }

        /// <summary>
        /// Add resources without removing old ones. Use to combine multiple ResourceManagers together.
        /// </summary>
        /// <param name="toAdd">The ResourceManager to add resources from.</param>
        public void addResources(ResourceManager toAdd)
        {
            //Add new resources
            foreach (SubsystemResources resource in toAdd.subsystemResources.Values)
            {
                if (!subsystemResources.ContainsKey(resource.Name))
                {
                    SubsystemResources sub = new SubsystemResources(resource.Name);
                    subsystemResources.Add(resource.Name, sub);
                }
                subsystemResources[resource.Name].addResources(resource);
            }
        }

        /// <summary>
        /// This will force all subsystems to validate any resources they have not yet validated.
        /// This may not actually load the resources into memory at this time.
        /// </summary>
        public void forceResourceRefresh()
        {
            foreach (SubsystemResources resource in subsystemResources.Values)
            {
                resource.forceResourceRefresh();
            }
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface("Resource Manager");
                foreach (SubsystemResources subsystem in subsystemResources.Values)
                {
                    editInterface.addSubInterface(subsystem.getEditInterface());
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Get an Enumerator over all SubsystemResources.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SubsystemResources> getSubsystemEnumerator()
        {
            return subsystemResources.Values;
        }

        #endregion Functions

        #region Saveable Members

        private const String SUBSYSTEM_BASE = "Subsystem";

        private ResourceManager(LoadInfo info)
        {
            for (int i = 0; info.hasValue(SUBSYSTEM_BASE + i); ++i)
            {
                SubsystemResources subsystem = info.GetValue<SubsystemResources>(SUBSYSTEM_BASE + i);
                this.addSubsystemResource(subsystem);
            }
        }

        public void getInfo(SaveInfo info)
        {
            int i = 0;
            foreach (SubsystemResources subsystem in subsystemResources.Values)
            {
                info.AddValue(SUBSYSTEM_BASE + i++, subsystem);
            }
        }

        #endregion
    }
}
