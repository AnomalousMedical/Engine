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

        public void Remove(ResourceGroup group)
        {
            resourceGroups.Remove(group);
        }

        public void Add(ResourceGroup toAdd)
        {
            if (!ContainsKey(toAdd.Name))
            {
                resourceGroups.Add(toAdd);
            }
            else
            {
                throw new ArgumentException(String.Format("Group already defined {0}.", toAdd.Name));
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
