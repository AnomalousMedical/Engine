using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    public class EditableAttributeFilter : MemberScannerFilter
    {
        public readonly static EditableAttributeFilter Instance = new EditableAttributeFilter();

        private EditableAttributeFilter()
        {

        }

        public bool allowMember(MemberWrapper wrapper)
        {
            return wrapper.getCustomAttributes(typeof(EditableAttribute), true).Length > 0;
        }
    }
}
