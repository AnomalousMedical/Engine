using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Engine.Attributes;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This class will copy all the values from a given object into another
    /// object of the same type.
    /// </summary>
    class DeepMemberCopier : MemberCopier
    {
        /// <summary>
        /// This function will copy all the values from source into object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="info">The member wrapper that allows the values to be set.</param>
        internal override void copyValue(object source, object destination, MemberWrapper info, CopyFilter filter)
        {
            Object deepCopySource;
            Object deepCopyDestination;
            //Find the source and destination objects if no info is provided use the given objects
            if (info != null)
            {
                deepCopySource = info.getValue(source, null);
                deepCopyDestination = info.getValue(destination, null);
            }
            else
            {
                deepCopySource = source;
                deepCopyDestination = destination;
            }
            //Validate that there is a source.
            if (deepCopySource != null)
            {
                performCopy(destination, info, deepCopySource, deepCopyDestination, filter);
            }
        }

        /// <summary>
        /// Helper function, actually does the copy.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="info"></param>
        /// <param name="deepCopySource"></param>
        /// <param name="deepCopyDestination"></param>
        internal void performCopy(object destination, MemberWrapper info, Object deepCopySource, Object deepCopyDestination, CopyFilter filter)
        {
            Type type = deepCopySource.GetType();
            //Make sure the destination exists, if it does not create it    
            if (deepCopyDestination == null)
            {
                deepCopyDestination = System.Activator.CreateInstance(type);
                info.setValue(destination, deepCopyDestination, null);
            }
            //Scan and copy all properties.
            LinkedList<MemberWrapper> members = CopyMemberScanner.Scanner.getMatchingMembers(type);
            foreach (MemberWrapper member in members)
            {
                if (filter == null || filter.allowCopy(member))
                {
                    MemberCopier copier = MemberCopier.getCopyClass(member.getWrappedType());
                    copier.copyValue(deepCopySource, deepCopyDestination, member, filter);
                }
            }
        }

        /// <summary>
        /// Create a new object that is an exact copy of source.
        /// Warning: This type must have a blank constructor.  If a special constructor
        /// is needed create the object and then call copyValue.
        /// </summary>
        /// <param name="source">The object to copy.</param>
        /// <returns>A new object with the same values as source.</returns>
        internal override object createCopy(object source, CopyFilter filter)
        {
            object destination = System.Activator.CreateInstance(source.GetType());
            copyValue(source, destination, null, filter);
            return destination;
        }
    }
}
