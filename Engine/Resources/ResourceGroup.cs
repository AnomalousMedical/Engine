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
    /// This is a group containing resources.  By organizing resources into groups time 
    /// can be saved between scene loads because groups can be skipped in the loading process.
    /// The resource locations defined in this group will all be unique, so if you enter a path
    /// like models\export or models/export these will map to the same physical resource and will only
    /// be added one time.
    /// </summary>
    public class ResourceGroup : Saveable
    {
        private static readonly ResourceGroup BLANK = new ResourceGroup("");

        private ResourceCollection resources = new ResourceCollection();
        private String name;
        private SubsystemResources parent;
        private EditInterface editInterface;

        /// <summary>
        /// Resource group constructor.  Internal because these should only be made by
        /// SubsystemResources.
        /// </summary>
        /// <param name="name">The name of the resource group.</param>
        internal ResourceGroup(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// This constructor will duplicate the contents of toDuplicate.
        /// </summary>
        /// <param name="toDuplicate">Duplicate the contents of this ResourceGroup.</param>
        internal ResourceGroup(ResourceGroup toDuplicate, SubsystemResources parent)
        {
            this.name = toDuplicate.name;
            this.parent = parent;
            foreach (Resource resource in toDuplicate.resources.Values)
            {
                this.addResource(new Resource(resource.LocName, resource.ArchiveType, resource.Recursive));
            }
        }

        /// <summary>
        /// Add a resource to this resource group.  This is the ideal way to construct resources.
        /// </summary>
        /// <param name="locName">The location of the resource group.</param>
        /// <param name="archiveType">The archive type name.</param>
        /// <param name="recursive">When true if the resource is a directory, subdirectories will be scanned.  False means this directory only.</param>
        public Resource addResource(String locName, String archiveType, bool recursive)
        {
            Resource resource;
            if (!resources.tryGetResource(locName, out resource))
            {
                resource = new Resource(locName, archiveType, recursive);
                addResource(resource);
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} already contains the resource {1}.  Duplicate ignored.", LogLevel.Warning, "ResourceManagement", name, locName);
            }
            return resource;
        }

        /// <summary>
        /// Add a resource directly to this group.
        /// </summary>
        /// <param name="resource">The resource to add.</param>
        public void addResource(Resource resource)
        {
            if (resource.LocName != null && !resources.containsResource(resource.LocName))
            {
                resources.addResource(resource);
                resource.setResourceGroup(this);
                onResourceAdded(resource);
                parent.fireResourceAdded(this, resource);
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} already contains the resource {1}.  Duplicate ignored.", LogLevel.Warning, "ResourceManagement", name, resource.LocName);
            }
        }

        /// <summary>
        /// Determine if the resource location is in this resource group.
        /// </summary>
        /// <param name="locName">The resource location to search for.</param>
        /// <returns>True if the group contains the resource.  False if it does not.</returns>
        public bool containsResource(String locName)
        {
            return resources.containsResource(locName);
        }

        /// <summary>
        /// Get the resource specified by locName.
        /// </summary>
        /// <param name="locName">The resource location to search for.</param>
        /// <returns>The resource for that location or null if it does not exist.</returns>
        public Resource getResource(String locName)
        {
            Resource resource;
            if (!resources.tryGetResource(locName, out resource))
            {
                Log.Default.sendMessage("ResourceGroup {0} does not contain the resource {1}.  Null returned.", LogLevel.Warning, "ResourceManagement", name, locName);
            }
            return resource;
        }

        /// <summary>
        /// Remove the resource specified by its location.
        /// </summary>
        /// <param name="locName">The location of the resource to remove.</param>
        public void removeResource(String locName)
        {
            Resource resource;
            if (resources.tryGetResource(locName, out resource))
            {
                resource.setResourceGroup(null);
                parent.fireResourceRemoved(this, resource);
                onResourceRemoved(resource);
                resources.removeResource(resource);
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} does not contain the resource {1}.  No changes made.", LogLevel.Warning, "ResourceManagement", name, locName);
            }
        }

        /// <summary>
        /// Get a count of the number of resources in this resource group,
        /// </summary>
        /// <returns>The number of resources in the group.</returns>
        public int getResourceCount()
        {
            return resources.Count;
        }

        /// <summary>
        /// Clear all resources and send unload signals.
        /// </summary>
        public void clear()
        {
            changeResourcesToMatch(BLANK);
        }

        public void initialize()
        {
            parent.fireInitializeResources(GetThisEnumerator());
        }

        private IEnumerable<ResourceGroup> GetThisEnumerator()
        {
            yield return this;
        }

        /// <summary>
        /// Change the resources in this group to match toMatch.  This will fire events
        /// as needed, see ResourceManager.
        /// </summary>
        /// <param name="toMatch">The ResourceGroup to duplicate.</param>
        public void changeResourcesToMatch(ResourceGroup toMatch)
        {
            //Unload any resources that are not shared
            LinkedList<String> unloadedResources = new LinkedList<String>();
            foreach (Resource resource in resources.Values)
            {
                if (!toMatch.resources.containsResource(resource.LocName))
                {
                    unloadedResources.AddLast(resource.LocName);
                }
                else if (!resources[resource.LocName].allPropertiesMatch(toMatch.resources[resource.LocName]))
                {
                    unloadedResources.AddLast(resource.LocName);
                }
            }

            foreach (String resource in unloadedResources)
            {
                removeResource(resource);
            }

            //Add any new resources that are not already added
            foreach (Resource resource in toMatch.resources.Values)
            {
                if (!resources.containsResource(resource.LocName))
                {
                    Resource res = new Resource(resource);
                    this.addResource(res);
                }
            }
        }

        /// <summary>
        /// Add resources without removing old ones. Use this to combine multiple groups together.
        /// </summary>
        /// <param name="toAdd">The group with resources to add.</param>
        public void addResources(ResourceGroup toAdd)
        {
            //Add any new resources that are not already added
            foreach (Resource resource in toAdd.resources.Values)
            {
                if (!resources.containsResource(resource.LocName))
                {
                    Resource res = new Resource(resource);
                    this.addResource(res);
                }
            }
        }

        /// <summary>
        /// Get an enumertor over all resources.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Resource> getResourceEnumerator()
        {
            return resources.Values;
        }

        internal void setParent(SubsystemResources parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// The name of the ResourceGroup. Don't use this to create subsystem groups, instead use the
        /// FullName to help avoid group name collisions.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The full name of the resource group. Includes the namespace of the ResourceManager.
        /// This is what should be used by the subsystem resource managers to help avoid collisions.
        /// </summary>
        public String FullName
        {
            get
            {
                return String.Format("{0}.{1}", parent.ParentResourceManager.Namespace, Name);
            }
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        internal EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name, addResource, removeResource, () =>
                {
                    foreach (Resource resource in resources.Values)
                    {
                        if (!resource.isValid())
                        {
                            String locName = resource.LocName;
                            if (locName == null || locName == String.Empty)
                            {
                                throw new ValidationException("Cannot accept empty locations. Please remove any blank entries.");
                            }
                            else
                            {
                                throw new ValidationException("Could not find the path \"{0}\". Please modify or remove that entry", resource.LocName);
                            }
                        }
                    }
                });
                editInterface.setPropertyInfo(ResourceEditableProperty.Info);
                foreach (Resource resource in resources.Values)
                {
                    onResourceAdded(resource);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Callback to add a resource.
        /// </summary>
        /// <param name="callback"></param>
        private void addResource(EditUICallback callback)
        {
            addResource(new Resource());
        }

        /// <summary>
        /// Callback to remove a resource.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="property"></param>
        private void removeResource(EditUICallback callback, EditableProperty property)
        {
            removeResource(editInterface.getKeyObjectForProperty<Resource>(property).LocName);
        }

        private void onResourceAdded(Resource resource)
        {
            if (editInterface != null)
            {
                editInterface.addEditableProperty(resource, new ResourceEditableProperty(resource));
            }
        }

        private void onResourceRemoved(Resource resource)
        {
            if (editInterface != null)
            {
                editInterface.removeEditableProperty(resource);
            }
        }

        private const String RESOURCE_BASE = "Resource";
        private const String NAME = "Name";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private ResourceGroup(LoadInfo info)
        {
            name = info.GetString(NAME);
            for (int i = 0; info.hasValue(RESOURCE_BASE + i); ++i)
            {
                Resource resource = info.GetValue<Resource>(RESOURCE_BASE + i);
                if (!resources.containsResource(resource.LocName))
                {
                    resource.setResourceGroup(this);
                    resources.addResource(resource);
                }
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
            foreach (Resource resource in resources.Values)
            {
                info.AddValue(RESOURCE_BASE + i++, resource);
            }
        }

        class ResourceCollection
        {
            Dictionary<String, Resource> resources = new Dictionary<string, Resource>();

            public bool tryGetResource(String name, out Resource resource)
            {
                resource = null;
                return name != null && resources.TryGetValue(transformName(name), out resource);
            }

            public void addResource(Resource resource)
            {
                resources.Add(transformName(resource.LocName), resource);
            }

            public void removeResource(Resource resource)
            {
                resources.Remove(transformName(resource.LocName));
            }

            public bool containsResource(String locName)
            {
                return locName != null && resources.ContainsKey(transformName(locName));
            }

            public IEnumerable<Resource> Values
            {
                get
                {
                    return resources.Values;
                }
            }

            public int Count
            {
                get
                {
                    return resources.Count;
                }
            }

            public Resource this[String locName]
            {
                get
                {
                    return resources[transformName(locName)];
                }
            }

            static String transformName(String name)
            {
                return name.Replace('/', '\\').ToLowerInvariant();
            }
        }
    }
}
