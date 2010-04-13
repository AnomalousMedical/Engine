using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Resources
{
    class ResourceGroupCollection
    {
        private List<ResourceGroup> resourceGroups = new List<ResourceGroup>();

        public bool ContainsKey(String key)
        {
            foreach (ResourceGroup group in resourceGroups)
            {
                if (group.Name == key)
                {
                    return true;
                }
            }
            return false;
        }

        public void Remove(String key)
        {
            ResourceGroup foundGroup = null;
            foreach (ResourceGroup group in resourceGroups)
            {
                if (group.Name == key)
                {
                    foundGroup = group;
                    break;
                }
            }
            if (foundGroup != null)
            {
                resourceGroups.Remove(foundGroup);
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Could not find key {0}.", key));
            }
        }

        public void Add(String key, ResourceGroup toAdd)
        {
            ResourceGroup foundGroup = null;
            foreach (ResourceGroup group in resourceGroups)
            {
                if (group.Name == key)
                {
                    foundGroup = group;
                    break;
                }
            }
            if (foundGroup == null)
            {
                resourceGroups.Add(toAdd);
            }
            else
            {
                throw new ArgumentException(String.Format("Key already defined {0}.", key));
            }
        }

        public ResourceGroup this[String key]
        {
            get
            {
                foreach (ResourceGroup group in resourceGroups)
                {
                    if (group.Name == key)
                    {
                        return group;
                    }
                }
                throw new KeyNotFoundException(String.Format("Could not find key {0}.", key));
            }
        }

        public IEnumerable<ResourceGroup> Values
        {
            get
            {
                return resourceGroups;
            }
        }
    }
}
