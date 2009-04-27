using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This copier can copy collections from one to another.
    /// </summary>
    /// <typeparam name="T">The type of the collection's contents.</typeparam>
    class ICollectionCopier<T> : MemberCopier
    {
        /// <summary>
        /// Copy the values from source into destination.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="destination">The destination collection.</param>
        /// <param name="info">The member wrapper to set the info.</param>
        internal override void copyValue(object source, object destination, MemberWrapper info, CopyFilter filter)
        {
            ICollection<T> sourceCollection = (ICollection<T>)info.getValue(source, null);
            ICollection<T> destinationCollection = (ICollection<T>)System.Activator.CreateInstance(sourceCollection.GetType());
            MemberCopier copier = MemberCopier.getCopyClass(sourceCollection.GetType().GetGenericArguments()[0]);
            foreach (T item in sourceCollection)
            {
                Object newObj = copier.createCopy(item, filter);
                destinationCollection.Add((T)newObj);
            }
            info.setValue(destination, destinationCollection, null);
        }

        /// <summary>
        /// Create a new collection that is an exact copy of source.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>A new collection that is an exact copy of source.</returns>
        internal override object createCopy(object source, CopyFilter filter)
        {
            ICollection<T> sourceCollection = (ICollection<T>)source;
            ICollection<T> destinationCollection = (ICollection<T>)System.Activator.CreateInstance(sourceCollection.GetType());
            MemberCopier copier = MemberCopier.getCopyClass(sourceCollection.GetType().GetGenericArguments()[0]);
            foreach (T item in sourceCollection)
            {
                Object newObj = copier.createCopy(item, filter);
                destinationCollection.Add((T)newObj);
            }
            return destinationCollection;
        }
    }
}
