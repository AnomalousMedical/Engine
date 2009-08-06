using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine
{
    public interface SoftBodyMesh { }
    public interface TriangleMesh { }
    public interface ConvexMesh { }

    /// <summary>
    /// This class holds shape collections for easy reuse.
    /// </summary>
    public abstract class ShapeRepository : IDisposable
    {
        public delegate void SoftBodyMeshLoaded(SoftBodyMesh softBodyMesh);
        public delegate void SoftBodyMeshRemoved(SoftBodyMesh softBodyMesh);

        public event SoftBodyMeshLoaded OnSoftBodyMeshLoaded;
        public event SoftBodyMeshRemoved OnSoftBodyMeshRemoved;

        public ShapeLocation CurrentLoadingLocation { get; set; }

        /// <summary>
        /// Removes a shape from the repository.
        /// </summary>
        /// <param name="name">The shape to remove.</param>
        public abstract void removeCollection(String name);
        
        /// <summary>
        /// Add a convex mesh that will be managed by this reposotory.  This will
        /// be released when this object is disposed.
        /// </summary>
        /// <param name="mesh">The mesh to add.</param>
        public abstract void addConvexMesh(String name, ConvexMesh mesh);

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
        public abstract void addTriangleMesh(String name, TriangleMesh mesh);

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
        public abstract void addMaterial(String name, ShapeMaterial materialDesc);

        /// <summary>
        /// Determine if the repository has a given material.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        /// <returns>True if the repository has the given material.</returns>
        public abstract bool hasMaterial(String name);

        /// <summary>
        /// Get the material named name.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        public abstract ShapeMaterial getMaterial(String name);

        /// <summary>
        /// Destroy a material and remove it from this repositiory.
        /// </summary>
        /// <param name="name">The name of the material to destroy.</param>
        public abstract void destroyMaterial(String name);

        /// <summary>
        /// Add a soft body mesh to the repository.
        /// </summary>
        /// <param name="softBodyMesh">The soft body mesh to add.</param>
        public abstract void addSoftBodyMesh(SoftBodyMesh softBodyMesh);

        /// <summary>
        /// Get a soft body mesh from the repository.
        /// </summary>
        /// <param name="name">The name of the soft body mesh to get.</param>
        /// <returns>The soft body mesh requested, or null if it does not exist.</returns>
        public abstract SoftBodyMesh getSoftBodyMesh(String name);

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
        protected void fireSoftBodyLoaded(SoftBodyMesh softBodyMesh)
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
        protected void fireSoftBodyRemoved(SoftBodyMesh softBodyMesh)
        {
            if (OnSoftBodyMeshRemoved != null)
            {
                OnSoftBodyMeshRemoved.Invoke(softBodyMesh);
            }
        }
    }
}
