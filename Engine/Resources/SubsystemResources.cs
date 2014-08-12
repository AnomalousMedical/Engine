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
    /// This is a collection of resources for a particular subsystem.
    /// </summary>
    public class SubsystemResources : Saveable
    {
        private ResourceGroupCollection resourceGroups = new ResourceGroupCollection();
        private String name;
        private ResourceListener listener;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the subsystem this provides resources for.</param>
        internal SubsystemResources(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the subsystem this provides resources for.</param>
        /// <param name="listener">The ResourceListener attached to this subsystem. This can be null and will never be copied if the copy constructor is used.</param>
        internal SubsystemResources(String name, ResourceListener listener)
        {
            this.name = name;
            this.listener = listener;
        }

        /// <summary>
        /// Constructor. Takes the SubsystemResource to duplicate. This does not copy the listener.
        /// </summary>
        /// <param name="toDuplicate">Enter the contents of this SubystemResource into the new one.</param>
        /// <param name="live">True to also hook up the source subsystem resources listener. If you are setting this to true
        /// it is reccomended that there not actually be any resources in the ResourceManager.</param>
        internal SubsystemResources(SubsystemResources toDuplicate, bool live)
        {
            this.name = toDuplicate.name;
            foreach (ResourceGroup group in toDuplicate.resourceGroups.Values)
            {
                this.addResourceGroup(new ResourceGroup(group, this));
            }
            //We do the live hookup last since this is a clone and if there are resources, the subsystems already know about them.
            if(live)
            {
                this.listener = toDuplicate.listener;
            }
        }

        /// <summary>
        /// Create and add a new ResourceGroup named name.
        /// </summary>
        /// <param name="name">The name of the ResourceGroup to add</param>
        /// <returns>The newly created resource group.</returns>
        public ResourceGroup addResourceGroup(String name)
        {
            if (!resourceGroups.ContainsKey(name))
            {
                ResourceGroup group = new ResourceGroup(name);
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
                ResourceGroup group = resourceGroups[name];
                fireResourceGroupRemoved(group);
                if (editInterface != null)
                {
                    editInterface.removeSubInterface(group);
                }
                resourceGroups.Remove(group);
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
            foreach (ResourceGroup group in resourceGroups.Values)
            {
                if (!toMatch.resourceGroups.ContainsKey(group.Name))
                {
                    unloadedResources.AddLast(group.Name);
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
                    ResourceGroup resourceGroup = new ResourceGroup(group.Name);
                    this.addResourceGroup(resourceGroup);
                }
                resourceGroups[group.Name].changeResourcesToMatch(group);
            }
        }

        /// <summary>
        /// Add resources without removing old ones.
        /// </summary>
        /// <param name="toAdd">The resources to add.</param>
        public void addResources(SubsystemResources toAdd)
        {
            //Add any new groups
            foreach (ResourceGroup group in toAdd.resourceGroups.Values)
            {
                if (!resourceGroups.ContainsKey(group.Name))
                {
                    ResourceGroup resourceGroup = new ResourceGroup(group.Name);
                    this.addResourceGroup(resourceGroup);
                }
                resourceGroups[group.Name].addResources(group);
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
        /// Fire a InitializeResources event to all listeners.  See ResourceManager.
        /// </summary>
        public void initializeResources()
        {
            fireInitializeResources(Groups);
        }

        /// <summary>
        /// Get an enumerator over all resource groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ResourceGroup> getResourceGroupEnumerator()
        {
            return resourceGroups.Values;
        }

        /// <summary>
        /// Add a resource group directly.
        /// </summary>
        /// <param name="group">The group to add.</param>
        internal void addResourceGroup(ResourceGroup group)
        {
            if (!resourceGroups.ContainsKey(group.Name))
            {
                group.setParent(this);
                resourceGroups.Add(group);
                if (editInterface != null)
                {
                    addResourceGroupEditInterface(group);
                }
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
            if(listener != null)
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
            if (listener != null)
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
            if (listener != null)
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
            if (listener != null)
            {
                listener.resourceGroupRemoved(group);
            }
        }

        /// <summary>
        /// Call to fire a resource refresh event.
        /// </summary>
        internal void fireInitializeResources(IEnumerable<ResourceGroup> groups)
        {
            if (listener != null)
            {
                listener.initializeResources(groups);
            }
        }

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

        public IEnumerable<ResourceGroup> Groups
        {
            get
            {
                return resourceGroups.Values;
            }
        }

        internal ResourceManager ParentResourceManager { get; set; }

        #region EditInterface

        private EditInterface editInterface;

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface</returns>
        internal EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.addCommand(new EditInterfaceCommand("Add Group", addResourceGroup));
                var groupEdits = editInterface.createEditInterfaceManager<ResourceGroup>();
                groupEdits.addCommand(new EditInterfaceCommand("Remove", destroyResourceGroup));
                foreach (ResourceGroup group in resourceGroups.Values)
                {
                    addResourceGroupEditInterface(group);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Helper function for adding a resource group EditInterface.
        /// </summary>
        /// <param name="group">The ResourceGroup to add the interface for.</param>
        private void addResourceGroupEditInterface(ResourceGroup group)
        {
            editInterface.addSubInterface(group, group.getEditInterface());
        }

        /// <summary>
        /// Callback to destroy a resource group.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="caller"></param>
        private void destroyResourceGroup(EditUICallback callback, EditInterfaceCommand caller)
        {
            removeResourceGroup(editInterface.resolveSourceObject<ResourceGroup>(callback.getSelectedEditInterface()).Name);
        }

        /// <summary>
        /// Callback to create a resource group.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="caller"></param>
        private void addResourceGroup(EditUICallback callback, EditInterfaceCommand caller)
        {
            callback.getInputString("Enter a name for the group.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                if (resourceGroups.ContainsKey(input))
                {
                    errorPrompt = "That name is already in use. Please provide another.";
                    return false;
                }

                this.addResourceGroup(input);
                return true;
            });
        }

        #endregion EditInterface

        #region Saveable Members

        private const String RESOURCE_GROUP_BASE = "ResourceGroup";
        private const String NAME = "Name";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private SubsystemResources(LoadInfo info)
        {
            name = info.GetString(NAME);
            for (int i = 0; info.hasValue(RESOURCE_GROUP_BASE + i); ++i)
            {
                ResourceGroup group = info.GetValue<ResourceGroup>(RESOURCE_GROUP_BASE + i);
                this.addResourceGroup(group);
            }
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info">Save info.</param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
            int i = 0;
            foreach (ResourceGroup group in resourceGroups.Values)
            {
                info.AddValue(RESOURCE_GROUP_BASE + i++, group);
            }
        }

        #endregion
    }
}
