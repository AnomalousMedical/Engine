using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    /// <summary>
    /// This interface allows fine grained filter control over what members are
    /// accepted by a MemberScanner.
    /// </summary>
    public interface MemberScannerFilter
    {
        /// <summary>
        /// This is the test function. It will return true if the member should
        /// be accepted.
        /// </summary>
        /// <param name="wrapper">The MemberWrapper with info about the field/property being scanned.</param>
        /// <returns>True if the member should be included in the results. False to omit it.</returns>
        bool allowMember(MemberWrapper wrapper);
    }
}
