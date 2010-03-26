using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Resources;

namespace Engine
{
    /// <summary>
    /// This is a group of shape resources.  It can process all resource files under
    /// its control which are saved as ShapeLocation instances.
    /// </summary>
    public class ShapeGroup
    {
        private Dictionary<String, ShapeLocation> shapeLocations = new Dictionary<String, ShapeLocation>();

        private bool loaded = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        public ShapeGroup(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Add a shape location to this group.
        /// </summary>
        /// <param name="location">The location to add.</param>
        /// <param name="loader">The loader to use if the shapes need to be built in the file.</param>
        /// <param name="builder">The builder to use.</param>
        public void addShapeLocation(ShapeLocation location, ShapeLoader loader, ShapeBuilder builder)
        {
            if (!shapeLocations.ContainsKey(location.LocName))
            {
                shapeLocations.Add(location.LocName, location);
                if (loaded)
                {
                    loadShape(location, loader, builder);
                }
            }
            else
            {
                Log.Default.sendMessage("Added duplicate shape resource {0}, ignored.", LogLevel.Warning, "ShapeLoading", location.LocName);
            }
        }

        /// <summary>
        /// Destroy the specified shape location.
        /// </summary>
        /// <param name="locName">The name of the location to destroy.</param>
        /// <param name="repository">The repository containing the shapes.</param>
        public void destroyShapeLocation(String locName, ShapeRepository repository)
        {
            ShapeLocation loc = shapeLocations[locName];
            if (loc.Loaded)
            {
                loc.destroyResources(repository);
            }
            shapeLocations.Remove(locName);
        }

        /// <summary>
        /// Load the shapes that have not yet been loaded.
        /// </summary>
        /// <param name="loader">The ShapeLoader to use to load the shapes.</param>
        /// <param name="builder">The builder to use to construct the shapes.</param>
        public void loadShapes(ShapeLoader loader, ShapeBuilder builder)
        {
            foreach (ShapeLocation location in shapeLocations.Values)
            {
                if (!location.Loaded)
                {
                    loadShape(location, loader, builder);
                }
            }
            loaded = true;
        }

        /// <summary>
        /// Unload all loaded shapes.
        /// </summary>
        /// <param name="repository">The repository to unload the shapes from.</param>
        public void unloadShapes(ShapeRepository repository)
        {
            foreach (ShapeLocation location in shapeLocations.Values)
            {
                location.destroyResources(repository);
                location.Loaded = false;
            }
            loaded = false;
        }

        /// <summary>
        /// The name of the shape group.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Helper function to load the shapes at the specified location.
        /// </summary>
        /// <param name="location">The location to load.</param>
        /// <param name="loader">The loader to use.</param>
        /// <param name="builder">The builder to use.</param>
        private void loadShape(ShapeLocation location, ShapeLoader loader, ShapeBuilder builder)
        {
            VirtualFileSystem vfs = VirtualFileSystem.Instance;
            builder.setCurrentShapeLocation(location);
            if (vfs.isDirectory(location.LocName))
            {
                scanDirectory(location, loader, builder, vfs);
            }
            else
            {
                if (loader.canLoadShape(location.LocName))
                {
                    loader.loadShapes(builder, location.LocName, vfs);
                }
                else
                {
                    Logging.Log.Default.sendMessage("Cannot load collision file {0}.", LogLevel.Error, "ShapeLoading", location.LocName);
                }
            }
            location.Loaded = true;
            //catch (FileNotFoundException)
            //{
            //    Logging.Log.Default.sendMessage("Cannot load collision file {0}.  Location does not exist.", LogLevel.Error, "ShapeLoading", location.LocName);
            //}
            //catch (DirectoryNotFoundException)
            //{
            //    Logging.Log.Default.sendMessage("Cannot load collision directory {0}.  Location does not exist.", LogLevel.Error, "ShapeLoading", location.LocName);
            //}
            //catch (NotSupportedException)
            //{
            //    Logging.Log.Default.sendMessage("The given path format is not supported {0}.", LogLevel.Error, "ShapeLoading", location.LocName);
            //}
            //catch (IOException e)
            //{
            //    Logging.Log.Default.sendMessage("General IO error loading collision {0}.\n{1}", LogLevel.Error, "ShapeLoading", location.LocName, e.Message);
            //}
        }

        /// <summary>
        /// Helper function to scan a directory for all shape files present.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="loader"></param>
        /// <param name="builder"></param>
        private void scanDirectory(ShapeLocation location, ShapeLoader loader, ShapeBuilder builder, VirtualFileSystem vfs)
        {
            String[] files = vfs.listFiles(location.LocName, location.Recursive);
            foreach (String path in files)
            {  
                if (loader.canLoadShape(path))
                {
                    loader.loadShapes(builder, path, vfs);
                }
            }
        }
    }
}
