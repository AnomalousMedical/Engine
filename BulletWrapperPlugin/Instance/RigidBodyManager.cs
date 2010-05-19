using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    class RigidBodyManager
    {
        static Dictionary<IntPtr, RigidBody> constraintMap = new Dictionary<IntPtr, RigidBody>();

        private RigidBodyManager()
        {

        }

        public static void add(IntPtr ptr, RigidBody element)
        {
            constraintMap.Add(ptr, element);
        }

        public static void remove(IntPtr ptr)
        {
            constraintMap.Remove(ptr);
        }

        public static RigidBody get(IntPtr ptr)
        {
            RigidBody element;
            constraintMap.TryGetValue(ptr, out element);
            return element;
        }
    }
}
