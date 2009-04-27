using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// Simply copy the value of source into destination.
    /// </summary>
    class SimpleMemberCopier : MemberCopier
    {
        /// <summary>
        /// Copy source to destination.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="info">The info to use to set the value.</param>
        internal override void copyValue(object source, object destination, MemberWrapper info, CopyFilter filter)
        {
            info.setValue(destination, info.getValue(source, null), null);
        }

        /// <summary>
        /// Create a new object that is an exact copy of source.
        /// </summary>
        /// <param name="source">The object to copy.</param>
        /// <returns>A new object that is a copy of source.</returns>
        internal override object createCopy(object source, CopyFilter filter)
        {
            return source;
        }
    }
}
