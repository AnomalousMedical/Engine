using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;

namespace PhysXPlugin
{
    using ConvexMeshList = Dictionary<String, PhysConvexMesh>;
    using TriangleMeshList = Dictionary<String, PhysTriangleMesh>;
    using MaterialList = Dictionary<String, PhysMaterialDesc>;
    using SoftBodyShapeList = Dictionary<String, PhysSoftBodyMesh>;
    using Logging;

    /// <summary>
    /// This class is a specialized repository for physx shapes.  It will
    /// manage the memory for convex meshes and other specialized shapes
    /// within the SDK.
    /// </summary>
    public class PhysXShapeRepository : ShapeRepository, IDisposable
    {
        private ConvexMeshList convexMeshes = new ConvexMeshList();
        private TriangleMeshList triangleMeshes = new TriangleMeshList();
        private MaterialList materials = new MaterialList();
        private SoftBodyShapeList softBodies = new SoftBodyShapeList();

        public PhysXShapeRepository()
        {
            
        }

        /// <summary>
        /// Add a convex mesh that will be managed by this reposotory.  This will
        /// be released when this object is disposed.
        /// </summary>
        /// <param name="mesh">The mesh to add.</param>
        public override void addConvexMesh(String name, PhysConvexMesh mesh)
        {
            convexMeshes.Add(name, mesh);
            CurrentLoadingLocation.addHull(name);
        }

        /// <summary>
        /// Remove a convex mesh from the repository and destroy it.  The mesh is no longer
        /// usable after this method is called.
        /// </summary>
        /// <param name="name">The mesh to remove.</param>
        public override void destroyConvexMesh(String name)
        {
            if (convexMeshes.ContainsKey(name))
            {
                PhysSDK.Instance.releaseConvexMesh(convexMeshes[name]);
                convexMeshes.Remove(name);
            }
            else
            {
                Logging.Log.Default.sendMessage("Attempted to destroy convex mesh named {0} that is not in this shape repository.", LogLevel.Warning, "ShapeLoading", name);
            }
        }

        /// <summary>
        /// Add control over a triangle mesh to this repository.  When the repository
        /// is disposed this mesh will be released and will no longer be avaliable.
        /// </summary>
        /// <param name="mesh">The triangle mesh to add.</param>
        public override void addTriangleMesh(String name, PhysTriangleMesh mesh)
        {
            triangleMeshes.Add(name, mesh);
            CurrentLoadingLocation.addMesh(name);
        }

        /// <summary>
        /// Remove a triangle mesh from this repository and destroy it.  The mesh is no longer
        /// usable after this method is called.
        /// </summary>
        /// <param name="name">The mesh to remove.</param>
        public override void destroyTriangleMesh(String name)
        {
            if (triangleMeshes.ContainsKey(name))
            {
                PhysSDK.Instance.releaseTriangleMesh(triangleMeshes[name]);
                triangleMeshes.Remove(name);
            }
            else
            {
                Logging.Log.Default.sendMessage("Attempted to destroy triangle mesh named {0} that is not in this shape repository.", LogLevel.Warning, "ShapeLoading", name);
            }
        }

        /// <summary>
        /// Add a material to the repository.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        /// <param name="material">The material itself.</param>
        public override void addMaterial(PhysMaterialDesc materialDesc)
        {
            if (!materials.ContainsKey(materialDesc.Name))
            {
                materials.Add(materialDesc.Name, materialDesc);
                CurrentLoadingLocation.addMaterial(materialDesc.Name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to add a duplicate material {0}.  Duplicate will be ignrored.", LogLevel.Warning, "ShapeLoading", materialDesc.Name);
            }
        }

        /// <summary>
        /// Get the material named name.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        public override PhysMaterialDesc getMaterial(String name)
        {
            if (name != null && materials.ContainsKey(name))
            {
                return materials[name];
            }
            return null;
        }

        /// <summary>
        /// Destroy a material and remove it from this repositiory.
        /// </summary>
        /// <param name="name">The name of the material to destroy.</param>
        public override void destroyMaterial(String name)
        {
            if (materials.ContainsKey(name))
            {
                PhysMaterialDesc material = materials[name];
                materials.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to destroy a non existant material {0}", LogLevel.Warning, "ShapeLoading", name);
            }
        }

        /// <summary>
        /// Add a soft body mesh to the repository.
        /// </summary>
        /// <param name="softBodyMesh">The soft body mesh to add.</param>
        public override void addSoftBodyMesh(PhysSoftBodyMesh softBodyMesh)
        {
            CurrentLoadingLocation.addSoftBody(softBodyMesh.Name);
            softBodies.Add(softBodyMesh.Name, softBodyMesh);
            fireSoftBodyLoaded(softBodyMesh);
        }

        /// <summary>
        /// Get a soft body mesh from the repository.
        /// </summary>
        /// <param name="name">The name of the soft body mesh to get.</param>
        /// <returns>The soft body mesh requested, or null if it does not exist.</returns>
        public override PhysSoftBodyMesh getSoftBodyMesh(String name)
        {
            if (softBodies.ContainsKey(name))
            {
                return softBodies[name];
            }
            Log.Default.sendMessage("Could not find a soft body named {0}.", LogLevel.Warning, "Physics", name);
            return null;
        }

        /// <summary>
        /// Destroy the soft body mesh specified by name.
        /// </summary>
        /// <param name="name">The name of the soft body mesh to destroy.</param>
        public override void destroySoftBodyMesh(String name)
        {
            if (softBodies.ContainsKey(name))
            {
                PhysSoftBodyMesh softBodyMesh = softBodies[name];
                fireSoftBodyRemoved(softBodyMesh);
                PhysSDK.Instance.releaseSoftBodyMesh(softBodyMesh);
                softBodies.Remove(name);
                softBodyMesh.Dispose();
            }
            else
            {
                Log.Default.sendMessage("Tried to erase a soft body named {0} that does not exist.", LogLevel.Warning, "Physics", name);
            }
        }

        /// <summary>
        /// Release all meshes under this repository's control.  This will
        /// invalidate them and they must be reloaded to continue using them.
        /// </summary>
        public override void Dispose()
        {
            foreach (PhysConvexMesh mesh in convexMeshes.Values)
            {
                PhysSDK.Instance.releaseConvexMesh(mesh);
            }
            foreach (PhysTriangleMesh mesh in triangleMeshes.Values)
            {
                PhysSDK.Instance.releaseTriangleMesh(mesh);
            }
            foreach (PhysSoftBodyMesh mesh in softBodies.Values)
            {
                PhysSDK.Instance.releaseSoftBodyMesh(mesh);
            }
        }
    }
}
