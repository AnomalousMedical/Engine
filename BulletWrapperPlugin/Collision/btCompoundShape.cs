using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class btCompoundShape : btCollisionShape
    {
        internal btCompoundShape(IntPtr btShape)
            :base(btShape)
        {
        }

        public btCompoundShape(float collisionMargin)
            :base(CompoundShape_Create(collisionMargin))
        {

        }

        public override void Dispose()
        {
            CompoundShape_DeleteChildren(btShape);
            base.Dispose();
        }

        /// <summary>
        /// Add a child to this shape, note that this object will take ownership of the children
        /// and will delete them itself.
        /// </summary>
        public void addChildShape(btCollisionShape child, Vector3 translation, Quaternion rotation)
        {
            CompoundShape_addChildShape(btShape, child.BulletShape, ref translation, ref rotation);
        }

        /// <summary>
        /// Remove a child shape from the object. Note that removing a child will make it no longer
        /// owned by this object, which means the caller is taking responsibility for deleting the shape.
        /// </summary>
        /// <param name="child"></param>
        public void removeChildShape(btCollisionShape child)
        {
            CompoundShape_removeChildShape(btShape, child.BulletShape);
        }

        public void updateChildTransform(int childIndex, Vector3 translation, Quaternion rotation, bool shouldRecalculateLocalAabb)
        {
            CompoundShape_updateChildTransform(btShape, childIndex, ref translation, ref rotation, shouldRecalculateLocalAabb);
        }

        public int ChildCount
        {
            get
            {
                return CompoundShape_GetCount(btShape);
            }
        }

        public override btCollisionShape createClone()
        {
            return new btCompoundShape(CollisionShape_Clone(btShape));
        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CompoundShape_Create(float collisionMargin);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompoundShape_DeleteChildren(IntPtr shape);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int CompoundShape_GetCount(IntPtr shape);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompoundShape_addChildShape(IntPtr compound, IntPtr child, ref Vector3 translation, ref Quaternion rotation);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompoundShape_removeChildShape(IntPtr compound, IntPtr child);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompoundShape_updateChildTransform(IntPtr compound, int childIndex, ref Vector3 translation, ref Quaternion rotation, bool shouldRecalculateLocalAabb);
    }
}
