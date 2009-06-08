using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This interface allows a subclass to provide its own EditInterface when
    /// it is being scanned by the ReflectedEditInterface. If the
    /// ReflectedEditInterface comes across a class that implements this
    /// interface it will call the getEditInterface funciton instead of using
    /// reflection to grab the properties.
    /// </summary>
    public interface EditInterfaceOverride
    {
        /// <summary>
        /// This function will provide the customized EditInterface for this
        /// class when it is scanned by the ReflectedEditInterface scanner. If
        /// it is not scanned with that scanner this function is not
        /// automatically called.
        /// </summary>
        /// <param name="memberName">The name of the member that contains this object.</param>
        /// <param name="scanner">The MemberScanner used to scan the parent object.</param>
        /// <returns>A new EditInterface for this class.</returns>
        EditInterface getEditInterface(String memberName, MemberScanner scanner);
    }
}
