using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// The MemberCopier is designed to produce a complete copy of any object using reflection.
    /// It is a generic way to do a deep copy of any object.
    /// </summary>
    abstract class MemberCopier
    {
        private static Dictionary<Type, MemberCopier> memberCopiers = new Dictionary<Type,MemberCopier>();
        private static DeepMemberCopier deepCopier = new DeepMemberCopier();
        private static SimpleMemberCopier simpleCopier = new SimpleMemberCopier();
        private static Type IDictionaryType = typeof(IDictionary<Object, Object>).GetGenericTypeDefinition();
        private static Type ICollectionType= typeof(ICollection<Object>).GetGenericTypeDefinition();
        private static Type LinkedListType = typeof(LinkedList<Object>).GetGenericTypeDefinition();
        private static Type DictionaryType = typeof(Dictionary<Object, Object>).GetGenericTypeDefinition();
        private static Type HashSetType = typeof(HashSet<Object>).GetGenericTypeDefinition();
        private static Type ListType = typeof(List<Object>).GetGenericTypeDefinition();
        private static Type QueueType = typeof(Queue<Object>).GetGenericTypeDefinition();
        private static Type SortedDictionaryType = typeof(SortedDictionary<Object, Object>).GetGenericTypeDefinition();
        private static Type SortedListType = typeof(SortedList<Object, Object>).GetGenericTypeDefinition();
        private static Type StackType = typeof(SortedList<Object, Object>).GetGenericTypeDefinition();
        private static Type IDictionaryMemberCopierType = typeof(IDictionaryMemberCopier<Object, Object>).GetGenericTypeDefinition();
        private static Type ICollectionMemberCopierType = typeof(ICollectionCopier<Object>).GetGenericTypeDefinition();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MemberCopier()
        {
            memberCopiers.Add(typeof(int), simpleCopier);
            memberCopiers.Add(typeof(long), simpleCopier);
            memberCopiers.Add(typeof(short), simpleCopier);
            memberCopiers.Add(typeof(uint), simpleCopier);
            memberCopiers.Add(typeof(ulong), simpleCopier);
            memberCopiers.Add(typeof(ushort), simpleCopier);
            memberCopiers.Add(typeof(float), simpleCopier);
            memberCopiers.Add(typeof(decimal), simpleCopier);
            memberCopiers.Add(typeof(double), simpleCopier);
            memberCopiers.Add(typeof(String), simpleCopier);
            memberCopiers.Add(typeof(char), simpleCopier);
            memberCopiers.Add(typeof(byte), simpleCopier);
            memberCopiers.Add(typeof(bool), simpleCopier);
            memberCopiers.Add(typeof(Vector3), simpleCopier);
            memberCopiers.Add(typeof(Quaternion), simpleCopier);
            memberCopiers.Add(typeof(Ray3), simpleCopier);
            memberCopiers.Add(typeof(Color), simpleCopier);
        }

        public static T CreateCopy<T>(T source)
        {
            return (T)getCopyClass(source.GetType()).createCopy(source);
        }

        /// <summary>
        /// Static function to perform a lookup of what type of class to copy with.
        /// </summary>
        /// <param name="t">The type that will be copied.</param>
        /// <returns>The MemberCopier that can copy t.</returns>
        internal static MemberCopier getCopyClass(Type t)
        {
            if (memberCopiers.ContainsKey(t))
            {
                return memberCopiers[t];
            }
            else if (t.GetCustomAttributes(typeof(NativeSubsystemTypeAttribute), true).Length > 0)
            {
                return simpleCopier;
            }
            else if (t.IsGenericType)
            {
                Type generic = t.GetGenericTypeDefinition();
                Type[] types = t.GetGenericArguments();
                Type specificType = null;
                if (generic == DictionaryType ||
                    generic == SortedDictionaryType ||
                    generic == SortedListType)
                {
                    specificType = IDictionaryMemberCopierType.MakeGenericType(types);
                }
                else if (generic == LinkedListType ||
                         generic == HashSetType || 
                         generic == ListType ||
                         generic == StackType)
                {
                    specificType = ICollectionMemberCopierType.MakeGenericType(types);
                }
                else
                {
                    //Return the deep copier if no custom generic copier is found
                    return deepCopier;
                }
                return (MemberCopier)System.Activator.CreateInstance(specificType);
            }
            else if (t.IsEnum)
            {
                return simpleCopier;
            }
            else
            {
                return deepCopier;
            }
        }

        /// <summary>
        /// This will copy source into destination.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="info">The info to use to set the value.</param>
        internal void copyValue(Object source, Object destination, MemberWrapper info)
        {
            copyValue(source, destination, info, null);
        }

        /// <summary>
        /// This will copy source into destination.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="info">The info to use to set the value.</param>
        /// <param name="filter">The filter to use when copying.  Can be null.</param>
        internal abstract void copyValue(Object source, Object destination, MemberWrapper info, CopyFilter filter);

        /// <summary>
        /// Create a new instance of the copy type that is a clone of source.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>An exact copy of source.</returns>
        internal Object createCopy(Object source)
        {
            return createCopy(source, null);
        }

        /// <summary>
        /// Create a new instance of the copy type that is a clone of source.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="filter">The filter to use when copying.  Can be null.</param>
        /// <returns>An exact copy of source.</returns>
        internal abstract Object createCopy(Object source, CopyFilter filter);
    }
}
