using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This is a filter that finds only fields marked with the
    /// EditableAttribute attribute.
    /// </summary>
    public class EditableAttributeFilter : MemberScannerFilter
    {
        /// <summary>
        /// Initializes a new Instance of Engine.Editing.EditableAttributeFilter
        /// </summary>
        public EditableAttributeFilter()
        {

        }

        /// <summary>
        /// This is the test function. It will return true if the member should
        /// be accepted.
        /// </summary>
        /// <param name="wrapper">The MemberWrapper with info about the field/property being scanned.</param>
        /// <returns>True if the member should be included in the results. False to omit it.</returns>
        public bool allowMember(MemberWrapper wrapper)
        {
            return wrapper.getCustomAttributes(typeof(EditableAttribute), true).Any();
        }

        /// <summary>
        /// This function determines if the given type should be scanned for
        /// members. It will return true if the member should be accepted.
        /// </summary>
        /// <param name="type">The type to potentially scan for members.</param>
        /// <returns>True if the type should be scanned.</returns>
        public bool allowType(Type type)
        {
            return type != TerminatingType;
        }

        /// <summary>
        /// This is the type the filter will stop allowing types for.
        /// </summary>
        public Type TerminatingType { get; set; }
    }
}
