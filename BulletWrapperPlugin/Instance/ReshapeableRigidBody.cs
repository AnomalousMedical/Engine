using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public class ReshapeableRigidBody : RigidBody
    {
        private IntPtr nativeReshapeable;

        public ReshapeableRigidBody(ReshapeableRigidBodyDefinition description, BulletScene scene, IntPtr collisionShape, Vector3 initialTrans, Quaternion initialRot)
            :base(description, scene, collisionShape, initialTrans, initialRot)
        {
            nativeReshapeable = ReshapeableRigidBody_Create(NativeRigidBody, collisionShape);
        }

        protected override void Dispose()
        {
            if(nativeReshapeable != IntPtr.Zero)
            {
                ReshapeableRigidBody_Delete(nativeReshapeable);
            }
        }

        /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
        /// does not exist it will be created. If it does exist it will be cleared
        /// and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
        public void createHullRegion(String name, ConvexDecompositionDesc desc)
        {
            ReshapeableRigidBody_createHullRegion(nativeReshapeable, name, desc, ref Vector3.Zero, ref Quaternion.Identity);
        }

	    /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
	    ///  does not exist it will be created. If it does exist it will be cleared
	    ///  and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
	    /// <param name="origin">An origin for the hull region.</param>
	    /// <param name="orientation">An orientation for the hull region.</param>
        public void createHullRegion(String name, ConvexDecompositionDesc desc, Vector3 origin, Quaternion orientation)
        {
            ReshapeableRigidBody_createHullRegion(nativeReshapeable, name, desc, ref origin, ref orientation);
        }

	    /// <summary>
	    /// Add a Sphere to the given region. If the region does not exist it will
        /// be created.
	    /// </summary>
	    /// <param name="sectionName">The name of the section to add the sphere to.</param>
	    /// <param name="radius">The radius of the sphere.</param>
	    /// <param name="origin">The origin of the sphere.</param>
        public void addSphereShape(String regionName, float radius, Vector3 origin)
        {
            ReshapeableRigidBody_addSphereShape(nativeReshapeable, regionName, radius, ref origin);
        }

        public unsafe void addHullShape(string regionName, float[] vertices, Vector3 translation, Quaternion rotation)
        {
            fixed (float* verts = &vertices[0])
            {
                ReshapeableRigidBody_addHullShape(nativeReshapeable, regionName, verts, vertices.Length / 3, sizeof(Vector3), BulletInterface.Instance.ShapeMargin, ref translation, ref rotation);
            }
        }

        /// <summary>
        /// Add a named shape to a given region, will return true if this works correctly
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="shapeName"></param>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool addNamedShape(String regionName, String shapeName, Vector3 translation, Quaternion rotation)
        {
            BulletShapeRepository repository = BulletInterface.Instance.ShapeRepository;
            if (repository.containsValidCollection(shapeName))
            {
                IntPtr shape = repository.getCollection(shapeName).CollisionShape;
                ReshapeableRigidBody_cloneAndAddShape(nativeReshapeable, regionName, shape, ref translation, ref rotation);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Empty and destroy a region removing it from the collision shape.
        /// </summary>
        /// <param name="name">The name of the region to destroy.</param>
        public void destroyRegion(String name)
        {
            ReshapeableRigidBody_destroyRegion(nativeReshapeable, name);
        }

	    /// <summary>
	    /// This function will recompute the mass props. It should be called when
        /// the collision shape is changed.
	    /// </summary>
        public void recomputeMassProps()
        {
            ReshapeableRigidBody_recomputeMassProps(nativeReshapeable);
        }

        //Imports
        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ReshapeableRigidBody_Create(IntPtr rigidBody, IntPtr compoundShape);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_Delete(IntPtr body);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_createHullRegion(IntPtr body, String name, ConvexDecompositionDesc desc, ref Vector3 origin, ref Quaternion orientation);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_addSphereShape(IntPtr body, String regionName, float radius, ref Vector3 origin);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void ReshapeableRigidBody_addHullShape(IntPtr body, String regionName, float* vertices, int numPoints, int stride, float collisionMargin, ref Vector3 origin, ref Quaternion rotation);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_cloneAndAddShape(IntPtr body, String regionName, IntPtr toClone, ref Vector3 translation, ref Quaternion rotation);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_destroyRegion(IntPtr body, String name);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_recomputeMassProps(IntPtr body);
    }
}
