﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Resources
{
    public interface ResourceListener
    {
        /// <summary>
        /// Called when a resource is added.
        /// </summary>
        /// <param name="group">The group the resource is added to.</param>
        /// <param name="resource">The resource added.</param>
        void resourceAdded(ResourceGroup group, Resource resource);
	
        /// <summary>
        /// Called when a resource is removed.
        /// </summary>
        /// <param name="group">The group the resource belongs to.</param>
        /// <param name="resource">The resource removed.</param>
	    void resourceRemoved(ResourceGroup group, Resource resource);

        /// <summary>
        /// Called when a resource group is added.
        /// </summary>
        /// <param name="group">The group the resource belongs to.</param>
	    void resourceGroupAdded(ResourceGroup group);

        /// <summary>
        /// Called when a resource is removed.
        /// </summary>
        /// <param name="group">The group the resource belongs to.</param>
	    void resourceGroupRemoved(ResourceGroup group);

        /// <summary>
        /// Initialize the given resource groups.
        /// </summary>
        /// <param name="resources">An enumerator over the resource groups to initialize.</param>
	    void initializeResources(IEnumerable<ResourceGroup> groups);
    }
}
