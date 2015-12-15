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
        /// Add a named shape to a given region, will return true if this works correctly
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="shapeName"></param>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool addNamedShape(String regionName, String shapeName, Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            BulletShapeRepository repository = BulletInterface.Instance.ShapeRepository;
            if (repository.containsValidCollection(shapeName))
            {
                IntPtr shape = repository.getCollection(shapeName).CollisionShape;
                ReshapeableRigidBody_cloneAndAddShape(nativeReshapeable, regionName, shape, ref translation, ref rotation, ref scale);

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

        public void moveOrigin(String regionName, Vector3 translation, Quaternion rotation)
        {
            ReshapeableRigidBody_moveOrigin(nativeReshapeable, regionName, ref translation, ref rotation);
        }

        public void setLocalScaling(String regionName, Vector3 scaling)
        {
            ReshapeableRigidBody_setLocalScaling(nativeReshapeable, regionName, ref scaling);
        }

        //Imports
        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ReshapeableRigidBody_Create(IntPtr rigidBody, IntPtr compoundShape);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_Delete(IntPtr body);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_cloneAndAddShape(IntPtr body, String regionName, IntPtr toClone, ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_destroyRegion(IntPtr body, String name);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_recomputeMassProps(IntPtr body);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_moveOrigin(IntPtr body, String regionName, ref Vector3 translation, ref Quaternion orientation);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReshapeableRigidBody_setLocalScaling(IntPtr body, String regionName, ref Vector3 scale);
    }
}
