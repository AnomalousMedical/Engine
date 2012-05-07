using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This attribute should be used on any field or property that should be
    /// picked up and used by the ReflectedEditInterface when it is creating the
    /// automatic interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EditableAttribute : Attribute
    {
        private String info;

        public EditableAttribute()
            :this("")
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of field this variable wraps if it represents something else. Can be used to show more advanced editors.</param>
        public EditableAttribute(String info)
        {
            this.info = info;
        }

        public virtual EditableProperty createEditableProperty(MemberWrapper memberWrapper, Object target)
        {
            return new ReflectedEditableProperty(memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target));
        }
    }
}
