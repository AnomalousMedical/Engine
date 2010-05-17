using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace BulletPlugin
{
    class CollisionShapeInterface
    {
        [DllImport("BulletWrapper")]
        public static extern void CollisionShape_Delete(IntPtr shape);

        [DllImport("BulletWrapper")]
        public static extern IntPtr CompoundShape_Create(float collisionMargin);

        [DllImport("BulletWrapper")]
        public static extern void CompoundShape_DeleteChildren(IntPtr shape);

        [DllImport("BulletWrapper")]
        public static extern int CompoundShape_GetCount(IntPtr shape);

        [DllImport("BulletWrapper")]
        public static extern void CompoundShape_addChildShape(IntPtr compound, IntPtr child, ref Vector3 translation, ref Quaternion rotation);

        [DllImport("BulletWrapper")]
        public static extern IntPtr SphereShape_Create(float radius, float collisionMargin);

        [DllImport("BulletWrapper")]
        public static extern IntPtr BoxShape_Create(ref Vector3 extents, float collisionMargin);

        [DllImport("BulletWrapper")]
        public static extern IntPtr CapsuleShape_Create(float radius, float height, float collisionMargin);

        [DllImport("BulletWrapper")]
        public static extern unsafe IntPtr ConvexHullShape_Create(float* vertices, int numPoints, int stride, float collisionMargin);

        [DllImport("BulletWrapper")]
        public static extern void CollisionShape_CalculateLocalInertia(IntPtr shape, float mass, ref Vector3 localInertia);
    }
}
