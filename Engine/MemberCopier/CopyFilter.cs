using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This interface allows subclasses to customize the way data is copied.  The filter
    /// will only be activated if the fields are not tagged with the DoNotCopyAttribute.
    /// That attribute still takes priority over any other custom filtering.
    /// </summary>
    public interface CopyFilter
    {
        /// <summary>
        /// This function will check to see if the given MemberInfo is valid for copying.
        /// </summary>
        /// <param name="member">The member info to be copied.</param>
        /// <returns>True to allow copy.  False to skip copying.</returns>
        bool allowCopy(MemberWrapper member);
    }
}
