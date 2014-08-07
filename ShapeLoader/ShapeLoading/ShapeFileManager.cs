using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using Logging;

namespace Engine
{
    /// <summary>
    /// This class responds to the ResourceManager to load and unload shapes.
    /// </summary>
    public abstract class ShapeFileManager : IDisposable, ResourceListener
    {
        Dictionary<String, ShapeGroup> shapeGroups = new Dictionary<string, ShapeGroup>();
        ShapeRepository shapeRepository;

        ShapeLoader loader = new XMLShapeLoader();
        ShapeBuilder builder;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShapeFileManager(ShapeRepository repository, ShapeBuilder builder)
        {
            this.shapeRepository = repository;
            this.builder = builder;
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
                Log.Info("Created shape resource group {0}.", name);
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
                Log.Info("Destroyed shape resource group {0}.", name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove group {0} that does not exist.  No changes made.", LogLevel.Warning, "ShapeLoading", name);
            }
        }

        /// <summary>
        /// Load all resources that are not currently loaded.
        /// </summary>
        public void loadUnloadedResources(IEnumerable<String> groups)
        {
            Log.Info("Started loading shape resources.");
            loadStarted();
            foreach(String group in groups)
            {
                shapeGroups[group].loadShapes(loader, builder);
            }
            loadEnded();
            Log.Info("Finished loading shape resources.");
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
        /// Destroy all resources and unload any shared data.
        /// </summary>
        public void Dispose()
        {
            unloadAllResources();
            shapeRepository.Dispose();
        }

        /// <summary>
        /// Handler for when resources are added to the SubsystemResources in control.
        /// </summary>
        /// <param name="group">The group the resource was added to.</param>
        /// <param name="resource">The resource that was added.</param>
        public void resourceAdded(ResourceGroup group, Resource resource)
        {
            ShapeGroup localGroup = declareGroup(group.FullName);
            localGroup.addShapeLocation(new ShapeLocation(resource.LocName, resource.Recursive, localGroup), loader, builder);
        }

        /// <summary>
        /// Called when a resource is removed.
        /// </summary>
        /// <param name="group">The group the resource belongs to.</param>
        /// <param name="resource">The resource that was added.</param>
        public void resourceRemoved(ResourceGroup group, Resource resource)
        {
            ShapeGroup localGroup = declareGroup(group.FullName);
            localGroup.destroyShapeLocation(resource.LocName, shapeRepository);
        }

        /// <summary>
        /// Called when a resource group is added.
        /// </summary>
        /// <param name="group">The group the resource was added to.</param>
        public void resourceGroupAdded(ResourceGroup group)
        {
            declareGroup(group.FullName);
        }

        /// <summary>
        /// Called whan a resource group is removed.
        /// </summary>
        /// <param name="group">The resource group that was remove.</param>
        public void resourceGroupRemoved(ResourceGroup group)
        {
            removeGroup(group.FullName);
        }

        public void initializeResources(SubsystemResources resources)
        {
            loadUnloadedResources(resources.Groups.Select(g => g.FullName));
        }

        /// <summary>
        /// Called before shapes are loaded. If a cooker or anything else needs
        /// to be initialized do it here.
        /// </summary>
        protected abstract void loadStarted();

        /// <summary>
        /// Called when shapes are finished being loaded. If any cleanup needs
        /// to be done do it here.
        /// </summary>
        protected abstract void loadEnded();
    }
}
