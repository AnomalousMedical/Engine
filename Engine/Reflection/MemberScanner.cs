using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    /// <summary>
    /// This interface will scan a given type and return the list of fields and
    /// properties as configured.
    /// </summary>
    public interface MemberScanner
    {
        IEnumerable<MemberWrapper> getMatchingMembers(Type type);
    }
}
