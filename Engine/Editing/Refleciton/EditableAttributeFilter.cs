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
        public readonly static EditableAttributeFilter Instance = new EditableAttributeFilter();

        /// <summary>
        /// Initializes a new Instance of Engine.Editing.EditableAttributeFilter
        /// </summary>
        private EditableAttributeFilter()
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
            return wrapper.getCustomAttributes(typeof(EditableAttribute), true).Length > 0;
        }
    }
}
