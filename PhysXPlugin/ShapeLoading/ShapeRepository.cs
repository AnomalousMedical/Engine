using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Logging;

namespace PhysXPlugin
{
    public delegate void ShapeLoaded(ShapeCollection shape);
    public delegate void ShapeRemoved(ShapeCollection shape);
    public delegate void SoftBodyMeshLoaded(PhysSoftBodyMesh softBodyMesh);
    public delegate void SoftBodyMeshRemoved(PhysSoftBodyMesh softBodyMesh);

    /// <summary>
    /// This class holds shape collections for easy reuse.
    /// </summary>
    public abstract class ShapeRepository : IDisposable
    {
        public event ShapeLoaded OnShapeLoaded;
        public event ShapeRemoved OnShapeRemoved;
        public event SoftBodyMeshLoaded OnSoftBodyMeshLoaded;
        public event SoftBodyMeshRemoved OnSoftBodyMeshRemoved;

        protected Dictionary<String, ShapeCollection> shapeCollections = new Dictionary<String, ShapeCollection>();

        public ShapeLocation CurrentLoadingLocation { get; set; }

        /// <summary>
        /// Add a shape collection to the repository.  The name of the shape is deteremined
        /// by the name set on it, which must be unique.
        /// </summary>
        /// <param name="collection">The shape to add.</param>
        /// <returns>True if the shape was added sucessfully.  False if there was a problem.  The shape should be considered invalid if false is returned and can be cleaned up.</returns>
        public bool addCollection(ShapeCollection collection)
        {
            if( shapeCollections.ContainsKey(collection.Name))
            {
                Log.Default.sendMessage("Attempted to add a shape with a duplicate name " + collection.Name + " ignoring the new shape.", LogLevel.Error, "Physics");
                return false;
            }
            else
            {
                shapeCollections.Add(collection.Name, collection);
                collection.SourceLocation = CurrentLoadingLocation;
                CurrentLoadingLocation.addShape(collection.Name);

                if (OnShapeLoaded != null)
                {
                    OnShapeLoaded.Invoke(collection);
                }

                return true;
            }
        }

        /// <summary>
        /// Removes a shape from the repository.
        /// </summary>
        /// <param name="name">The shape to remove.</param>
        public void removeCollection(String name)
        {
            if (shapeCollections.ContainsKey(name))
            {
                if (OnShapeRemoved != null)
                {
                    OnShapeRemoved.Invoke(shapeCollections[name]);
                }

                shapeCollections.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove a shape " + name + " that does not exist.  No changes made.", LogLevel.Error, "Physics");
            }
        }

        /// <summary>
        /// Get a specific shape collection from the repository.
        /// </summary>
        /// <param name="name">The name of the shape to get.</param>
        /// <returns>The shape specified by name, or null if no such shape exists.</returns>
        public ShapeCollection getCollection(String name)
        {
            if (name != null && shapeCollections.ContainsKey(name))
            {
                return shapeCollections[name];
            }
            else
            {
                Log.Default.sendMessage("Could not find a shape named " + name + ".", LogLevel.Error, "Physics");
            }
            return null;
        }

        /// <summary>
        /// Returns true if the shape collection specified by name exists and has shapes in it.
        /// </summary>
        /// <param name="name">The name of the collection to test.</param>
        /// <returns>True if the collection exists and contains shapes.  False if it is invalid.</returns>
        public bool containsValidCollection(String name)
        {
            return name != null && shapeCollections.ContainsKey(name) && shapeCollections[name].Count > 0;
        }

        /// <summary>
        /// Add a convex mesh that will be managed by this reposotory.  This will
        /// be released when this object is disposed.
        /// </summary>
        /// <param name="mesh">The mesh to add.</param>
        public abstract void addConvexMesh(String name, PhysConvexMesh mesh);

        /// <summary>
        /// Remove a convex mesh from the repository and destroy it.  The mesh is no longer
        /// usable after this method is called.
        /// </summary>
        /// <param name="name">The mesh to remove.</param>
        public abstract void destroyConvexMesh(String name);

        /// <summary>
        /// Add control over a triangle mesh to this repository.  When the repository
        /// is disposed this mesh will be released and will no longer be avaliable.
        /// </summary>
        /// <param name="mesh">The triangle mesh to add.</param>
        public abstract void addTriangleMesh(String name, PhysTriangleMesh mesh);

        /// <summary>
        /// Remove a triangle mesh from this repository and destroy it.  The mesh is no longer
        /// usable after this method is called.
        /// </summary>
        /// <param name="name">The mesh to remove.</param>
        public abstract void destroyTriangleMesh(String name);

        /// <summary>
        /// Add a material to the repository.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        /// <param name="material">The material itself.</param>
        public abstract void addMaterial(PhysMaterialDesc materialDesc);

        /// <summary>
        /// Get the material named name.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        public abstract PhysMaterialDesc getMaterial(String name);

        /// <summary>
        /// Destroy a material and remove it from this repositiory.
        /// </summary>
        /// <param name="name">The name of the material to destroy.</param>
        public abstract void destroyMaterial(String name);

        /// <summary>
        /// Add a soft body mesh to the repository.
        /// </summary>
        /// <param name="softBodyMesh">The soft body mesh to add.</param>
        public abstract void addSoftBodyMesh(PhysSoftBodyMesh softBodyMesh);

        /// <summary>
        /// Get a soft body mesh from the repository.
        /// </summary>
        /// <param name="name">The name of the soft body mesh to get.</param>
        /// <returns>The soft body mesh requested, or null if it does not exist.</returns>
        public abstract PhysSoftBodyMesh getSoftBodyMesh(String name);

        /// <summary>
        /// Destroy the soft body mesh specified by name.
        /// </summary>
        /// <param name="name">The name of the soft body mesh to destroy.</param>
        public abstract void destroySoftBodyMesh(String name);

        /// <summary>
        /// Dispose.  Should release any native resources.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Helper function to fire the OnSoftBodyMeshLoaded event.
        /// </summary>
        /// <param name="softBodyMesh">The soft body.</param>
        protected void fireSoftBodyLoaded(PhysSoftBodyMesh softBodyMesh)
        {
            if (OnSoftBodyMeshLoaded != null)
            {
                OnSoftBodyMeshLoaded.Invoke(softBodyMesh);
            }
        }

        /// <summary>
        /// Helper function to fire the OnSoftBodyMeshRemoved event.
        /// </summary>
        /// <param name="softBodyMesh">The soft body.</param>
        protected void fireSoftBodyRemoved(PhysSoftBodyMesh softBodyMesh)
        {
            if (OnSoftBodyMeshRemoved != null)
            {
                OnSoftBodyMeshRemoved.Invoke(softBodyMesh);
            }
        }
    }
}
