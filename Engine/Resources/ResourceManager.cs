using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

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
    public class ResourceManager
    {
        #region Fields

        Dictionary<String, SubsystemResources> subsystemResources = new Dictionary<string, SubsystemResources>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        ResourceManager()
        {

        }

        /// <summary>
        /// Constructor.  Takes an existing resource manager and duplicates
        /// its contents into the new ResourceManager.
        /// </summary>
        /// <param name="toDuplicate">The resource manager to duplicate.</param>
        ResourceManager(ResourceManager toDuplicate)
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
        void addSubsystemResource(SubsystemResources resources)
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
        void removeSubsystemResource(SubsystemResources resources)
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
        void removeSubsystemResource(String name)
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
        SubsystemResources getSubsystemResource(String name)
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
        void changeResourcesToMatch(ResourceManager toMatch)
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
        /// Get an enumeration over all the SubsystemResources in this resource manager.
        /// </summary>
        /// <returns>An enumeration over the resources in this manager.</returns>
        IEnumerable<SubsystemResources> getSubsystemEnum()
        {
            return subsystemResources.Values;
        }

        /// <summary>
        /// This will force all subsystems to validate any resources they have not yet validated.
        /// This may not actually load the resources into memory at this time.
        /// </summary>
        void forceResourceRefresh()
        {
            foreach (SubsystemResources resource in subsystemResources.Values)
            {
                resource.forceResourceRefresh();
            }
        }

        #endregion Functions
    }
}
