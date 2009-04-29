using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using Logging;
using PhysXWrapper;

namespace PhysXPlugin
{
    /// <summary>
    /// This class responds to the ResourceManager to load and unload shapes.
    /// </summary>
    public class ShapeFileManager : IDisposable, ResourceListener
    {
        Dictionary<String, ShapeGroup> shapeGroups = new Dictionary<string, ShapeGroup>();
        PhysXShapeRepository shapeRepository;

        ShapeLoader loader = new XMLShapeLoader();
        ShapeBuilder builder;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShapeFileManager()
        {
            shapeRepository = new PhysXShapeRepository();
            builder = new PhysXShapeBuilder(shapeRepository);
        }

        /// <summary>
        /// Declare a new shape group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        public ShapeGroup declareGroup(String name)
        {
            if (!shapeGroups.ContainsKey(name))
            {
                shapeGroups.Add(name, new ShapeGroup(name));
            }
            return shapeGroups[name];
        }

        /// <summary>
        /// Remove a shape group.
        /// </summary>
        /// <param name="name">The name of the group to remove.</param>
        public void removeGroup(String name)
        {
            if (shapeGroups.ContainsKey(name))
            {
                ShapeGroup localGroup = declareGroup(name);
                localGroup.unloadShapes(shapeRepository);
                shapeGroups.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove group {0} that does not exist.  No changes made.", LogLevel.Warning, "ShapeLoading", name);
            }
        }

        /// <summary>
        /// Load all resources that are not currently loaded.
        /// </summary>
        public void loadUnloadedResources()
        {
            PhysCooking.initCooking();
            foreach (ShapeGroup group in shapeGroups.Values)
            {
                group.loadShapes(loader, builder);
            }
            PhysCooking.closeCooking();
        }

        /// <summary>
        /// Unload all resources that are loaded.
        /// </summary>
        public void unloadAllResources()
        {
            foreach (ShapeGroup group in shapeGroups.Values)
            {
                group.unloadShapes(shapeRepository);
            }
        }

        /// <summary>
        /// Call to bind this ShapeFileManager to the give SubsystemResources.  This will
        /// subscribe to the callbacks on that resources.
        /// </summary>
        /// <param name="resources"></param>
        public void setSubsystemResources(SubsystemResources resources)
        {
            resources.addResourceListener(this);
        }

        /// <summary>
        /// Destroy all resources and unload any shared data.
        /// </summary>
        public void Dispose()
        {
            unloadAllResources();
            shapeRepository.Dispose();
        }

        /// <summary>
        /// Get the shape repository being written to by this file manager.
        /// </summary>
        /// <returns>The shape repository with this file manager's shapes.</returns>
        public ShapeRepository getShapeRepository()
        {
            return shapeRepository;
        }

        /// <summary>
        /// Handler for when resources are added to the SubsystemResources in control.
        /// </summary>
        /// <param name="group">The group the resource was added to.</param>
        /// <param name="resource">The resource that was added.</param>
        public void resourceAdded(ResourceGroup group, Resource resource)
        {
            ShapeGroup localGroup = declareGroup(group.Name);
            localGroup.addShapeLocation(new ShapeLocation(resource.FullPath, resource.Type, resource.Recursive, localGroup), loader, builder);
        }

        /// <summary>
        /// Called when a resource is removed.
        /// </summary>
        /// <param name="group">The group the resource belongs to.</param>
        /// <param name="resource">The resource that was added.</param>
        public void resourceRemoved(ResourceGroup group, Resource resource)
        {
            ShapeGroup localGroup = declareGroup(group.Name);
            localGroup.destroyShapeLocation(resource.FullPath, shapeRepository);
        }

        /// <summary>
        /// Called when a resource group is added.
        /// </summary>
        /// <param name="group">The group the resource was added to.</param>
        public void resourceGroupAdded(ResourceGroup group)
        {
            declareGroup(group.Name);
        }

        /// <summary>
        /// Called whan a resource group is removed.
        /// </summary>
        /// <param name="group">The resource group that was remove.</param>
        public void resourceGroupRemoved(ResourceGroup group)
        {
            removeGroup(group.Name);
        }

        public void forceResourceRefresh()
        {
            loadUnloadedResources();
        }
    }
}
