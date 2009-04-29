using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.Resources
{
    /// <summary>
    /// This is a group containing resources.  By organizing resources into groups time 
    /// can be saved between scene loads because groups can be skipped in the loading process.
    /// </summary>
    public class ResourceGroup
    {
        #region Fields

        Dictionary<String, Resource> resources = new Dictionary<string, Resource>();
        String name;
        SubsystemResources parent;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Resource group constructor.  Internal because these should only be made by
        /// SubsystemResources.
        /// </summary>
        /// <param name="name">The name of the resource group.</param>
        internal ResourceGroup(String name, SubsystemResources parent)
        {
            this.name = name;
            this.parent = parent;
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
                this.addResource(new Resource(resource.getLocName(), resource.Type, resource.Recursive));
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a resource to this resource group.  This is the ideal way to construct resources.
        /// </summary>
        /// <param name="locName">The location of the resource group.</param>
        /// <param name="type">The type of the resource group.</param>
        /// <param name="recursive">When true if the resource is a directory, subdirectories will be scanned.  False means this directory only.</param>
        public Resource addResource(String locName, ResourceType type, bool recursive)
        {
            if (!resources.ContainsKey(locName))
            {
                Resource resource = new Resource(locName, type, recursive);
                addResource(resource);
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} already contains the resource {1}.  Duplicate ignored.", LogLevel.Warning, "ResourceManagement", name, locName);
            }
            return resources[locName];
        }

        /// <summary>
        /// Add a resource directly to this group.
        /// </summary>
        /// <param name="resource">The resource to add.</param>
        public void addResource(Resource resource)
        {
            if (resource.getLocName() != null && !resources.ContainsKey(resource.getLocName()))
            {
                resources.Add(resource.getLocName(), resource);
                parent.fireResourceAdded(this, resource);
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} already contains the resource {1}.  Duplicate ignored.", LogLevel.Warning, "ResourceManagement", name, resource.getLocName());
            }
        }

        /// <summary>
        /// Determine if the resource location is in this resource group.
        /// </summary>
        /// <param name="locName">The resource location to search for.</param>
        /// <returns>True if the group contains the resource.  False if it does not.</returns>
        public bool containsResource(String locName)
        {
            return locName != null && resources.ContainsKey(locName);
        }

        /// <summary>
        /// Get the resource specified by locName.
        /// </summary>
        /// <param name="locName">The resource location to search for.</param>
        /// <returns>The resource for that location or null if it does not exist.</returns>
        public Resource getResource(String locName)
        {
            if (locName != null && resources.ContainsKey(locName))
            {
                return resources[locName];
            }
            else
            {
                Log.Default.sendMessage("ResourceGroup {0} does not contain the resource {1}.  Null returned.", LogLevel.Warning, "ResourceManagement", name, locName);
                return null;
            }
        }

        /// <summary>
        /// Remove the resource specified by its location.
        /// </summary>
        /// <param name="locName">The location of the resource to remove.</param>
        public void removeResource(String locName)
        {
            if (resources.ContainsKey(locName))
            {
                parent.fireResourceRemoved(this, resources[locName]);
                resources.Remove(locName);
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
            changeResourcesToMatch(new ResourceGroup("", null));
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
            foreach (String resource in resources.Keys)
            {
                if (!toMatch.resources.ContainsKey(resource))
                {
                    unloadedResources.AddLast(resource);
                }
                else if (!resources[resource].allPropertiesMatch(toMatch.resources[resource]))
                {
                    unloadedResources.AddLast(resource);
                }
            }

            foreach (String resource in unloadedResources)
            {
                removeResource(resource);
            }

            //Add any new resources that are not already added
            foreach (Resource resource in toMatch.resources.Values)
            {
                if (!resources.ContainsKey(resource.getLocName()))
                {
                    Resource res = new Resource(resource);
                    this.addResource(res);
                }
            }
        }

        /// <summary>
        /// Get an enumeration over all the resources in this ResourceGroup.
        /// </summary>
        /// <returns>An enumeration over all the resources.</returns>
        public IEnumerable<Resource> getResourceEnum()
        {
            return resources.Values;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the ResourceGroup.
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
