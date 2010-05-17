using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletPlugin
{
    /// <summary>
    /// This class maps constraint pointers back to the actual constraints.
    /// </summary>
    class TypedConstraintManager
    {
        static Dictionary<IntPtr, TypedConstraintElement> constraintMap = new Dictionary<IntPtr, TypedConstraintElement>();

        private TypedConstraintManager()
        {

        }

        public static void addConstraint(IntPtr ptr, TypedConstraintElement element)
        {
            constraintMap.Add(ptr, element);
        }

        public static void removeConstraint(IntPtr ptr)
        {
            constraintMap.Remove(ptr);
        }

        public static TypedConstraintElement getElement(IntPtr ptr)
        {
            TypedConstraintElement element;
            constraintMap.TryGetValue(ptr, out element);
            return element;
        }
    }
}
