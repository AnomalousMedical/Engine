using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.Resources
{
    /// <summary>
    /// This is a collection of resources for a particular subsystem.
    /// </summary>
    public class SubsystemResources
    {
        #region Fields

        private Dictionary<String, ResourceGroup> resourceGroups = new Dictionary<string, ResourceGroup>();
        private String name;
        private List<ResourceListener> listeners = new List<ResourceListener>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the subsystem this provides resources for.</param>
        public SubsystemResources(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Constructor.  Takes the SubsystemResource to duplicate.
        /// </summary>
        /// <param name="toDuplicate">Enter the contents of this SubystemResource into the new one.</param>
        internal SubsystemResources(SubsystemResources toDuplicate)
        {
            this.name = toDuplicate.name;
            foreach (ResourceGroup group in toDuplicate.resourceGroups.Values)
            {
                this.addResourceGroup(new ResourceGroup(group, this));
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Create and add a new ResourceGroup named name.
        /// </summary>
        /// <param name="name">The name of the ResourceGroup to add</param>
        /// <returns>The newly created resource group.</returns>
        public ResourceGroup addResourceGroup(String name)
        {
            if (!resourceGroups.ContainsKey(name))
            {
                ResourceGroup group = new ResourceGroup(name, this);
                addResourceGroup(group);
            }
            else
            {
                Log.Default.sendMessage("Subsystem {0} already contains resources for group {1}.  Simply returned existing group.", LogLevel.Warning, "ResourceManagement", Name, name);
            }
            return resourceGroups[name];
        }

        /// <summary>
        /// Get the resource group specified by name. Will return null if it is not found.
        /// </summary>
        /// <param name="name">The resourceGroup to get.</param>
        /// <returns>The resource group specified by name or null if it does not exist.</returns>
        public ResourceGroup getResourceGroup(String name)
        {
            if (resourceGroups.ContainsKey(name))
            {
                return resourceGroups[name];
            }
            else
            {
                Log.Default.sendMessage("Subsystem {0} does not contain resource group {1}.  Null returned.", LogLevel.Warning, "ResourceManagement", Name, name);
                return null;
            }
        }

        /// <summary>
        /// Remove a resource group with the specified name.
        /// </summary>
        /// <param name="name">The name of the resource group to remove.</param>
        public void removeResourceGroup(String name)
        {
            if (resourceGroups.ContainsKey(name))
            {
                fireResourceGroupRemoved(resourceGroups[name]);
                resourceGroups.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("Subsystem {0} does not contain resource group {1}.  No changes made.", LogLevel.Warning, "ResourceManagement", Name, name);
            }
        }

        /// <summary>
        /// Change the resources of this SubsystemResources to match the ones in toMatch.
        /// This will fire the events as appropriate.  See ResourceManager for more details.
        /// </summary>
        /// <param name="toMatch">The SubsystemResources to match.</param>
        public void changeResourcesToMatch(SubsystemResources toMatch)
        {
            //Unload non matching groups
            LinkedList<String> unloadedResources = new LinkedList<String>();
            foreach (String group in resourceGroups.Keys)
            {
                if (!toMatch.resourceGroups.ContainsKey(group))
                {
                    unloadedResources.AddLast(group);
                }
            }

            //Remove unloaded groups
            foreach (String name in unloadedResources)
            {
                removeResourceGroup(name);
            }

            //Add any new groups
            foreach (ResourceGroup group in toMatch.resourceGroups.Values)
            {
                if (!resourceGroups.ContainsKey(group.Name))
                {
                    ResourceGroup resourceGroup = new ResourceGroup(group.Name, this);
                    this.addResourceGroup(resourceGroup);
                }
                resourceGroups[group.Name].changeResourcesToMatch(group);
            }
        }

        /// <summary>
        /// Send unload signals to all resource groups.
        /// </summary>
        public void sendUnloadSignals()
        {
            foreach (ResourceGroup group in resourceGroups.Values)
            {
                fireResourceGroupRemoved(group);
            }
        }

        /// <summary>
        /// Fire a ForceResourceRefresh event to all listeners.  See ResourceManager.
        /// </summary>
        public void forceResourceRefresh()
        {
            fireForceRefresh();
        }

        /// <summary>
        /// Get an enumeration over all resource groups.
        /// </summary>
        /// <returns>An enumeration over the resource groups.</returns>
        public IEnumerable<ResourceGroup> getResourceGroupEnum()
        {
            return resourceGroups.Values;
        }

        /// <summary>
        /// Add a resource listener.
        /// </summary>
        /// <param name="listener">The resource listener to add.</param>
        public void addResourceListener(ResourceListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Remove a resource listener.
        /// </summary>
        /// <param name="listener">The resource listener to remove.</param>
        public void removeResourceListener(ResourceListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Add a resource group directly.
        /// </summary>
        /// <param name="group">The group to add.</param>
        internal void addResourceGroup(ResourceGroup group)
        {
            if (!resourceGroups.ContainsKey(group.Name))
            {
                resourceGroups.Add(group.Name, group);
                fireResourceGroupAdded(group);
            }
            else
            {
                Log.Default.sendMessage("Subsystem {0} already contains resources for group {1}.  All values in duplicate entry are ignored.", LogLevel.Error, "ResourceManagement", Name, group.Name);
            }
        }

        /// <summary>
        /// Call to fire a ResourceAdded event.
        /// </summary>
        /// <param name="group">The resource group the resource was added to.</param>
        /// <param name="resource">The resource that was added.</param>
        internal void fireResourceAdded(ResourceGroup group, Resource resource)
        {
            foreach (ResourceListener listener in listeners)
            {
                listener.resourceAdded(group, resource);
            }
        }

        /// <summary>
        /// Call to fire a ResourceRemoved event.
        /// </summary>
        /// <param name="group">The resource group the resource was removed from.</param>
        /// <param name="resource">The resource that was removed.</param>
        internal void fireResourceRemoved(ResourceGroup group, Resource resource)
        {
            foreach (ResourceListener listener in listeners)
            {
                listener.resourceRemoved(group, resource);
            }
        }

        /// <summary>
        /// Call to fire a ResourceGroupAdded event.
        /// </summary>
        /// <param name="group">The group added.</param>
        private void fireResourceGroupAdded(ResourceGroup group)
        {
            foreach (ResourceListener listener in listeners)
            {
                listener.resourceGroupAdded(group);
            }
        }

        /// <summary>
        /// Call to fire a ResourceGroupRemoved event.
        /// </summary>
        /// <param name="group">The group removed.</param>
        private void fireResourceGroupRemoved(ResourceGroup group)
        {
            foreach (ResourceListener listener in listeners)
            {
                listener.resourceGroupRemoved(group);
            }
        }

        /// <summary>
        /// Call to fire a resource refresh event.
        /// </summary>
        private void fireForceRefresh()
        {
            foreach (ResourceListener listener in listeners)
            {
                listener.forceResourceRefresh();
            }
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the subsystem.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        #endregion Properties
    }
}
