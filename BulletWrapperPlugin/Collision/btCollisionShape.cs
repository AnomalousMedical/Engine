﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class btCollisionShape : IDisposable
    {
        protected IntPtr btShape;

        public IntPtr BulletShape
        {
            get
            {
                return btShape;
            }
        }

        protected btCollisionShape(IntPtr btShape)
        {
            this.btShape = btShape;
        }

        public virtual void Dispose()
        {
            CollisionShape_Delete(btShape);
        }

        public void calculateLocalInertia(float mass, ref Vector3 localInertia)
        {
            CollisionShape_CalculateLocalInertia(btShape, mass, ref localInertia);
        }

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CollisionShape_Delete(IntPtr shape);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CollisionShape_CalculateLocalInertia(IntPtr shape, float mass, ref Vector3 localInertia);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CollisionShape_Clone(IntPtr source);
    }
}