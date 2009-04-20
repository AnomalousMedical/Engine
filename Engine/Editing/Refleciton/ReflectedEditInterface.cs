using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;
using System.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This is an implementation of the EditInterface that uses reflection to
    /// automatically discover properties.
    /// </summary>
    public class ReflectedEditInterface
    {
        #region Static

        private static MemberScanner defaultScanner = new MemberScanner(EditableAttributeFilter.Instance);

        public static MemberScanner DefaultScanner
        {
            get
            {
                return defaultScanner;
            }
        }

        static ReflectedEditInterface()
        {

        }

        #endregion Static

        #region Functions

        /// <summary>
        /// Create a new EditInterface. A new one is created every time this function is called.
        /// </summary>
        /// <param name="validateCallback">A callback to set for validating.</param>
        /// <returns>A new EditInterface from the scanned info.</returns>
        public static EditInterface createEditInterface(Object target, MemberScanner scanner, String name, Validate validateCallback)
        {
            return buildInterface(name, scanner, target, validateCallback);
        }

        #endregion Functions

        #region Helper Functions

        /// <summary>
        /// Build the EditInterface for the given type.
        /// </summary>
        /// <param name="name">The name to use.</param>
        /// <param name="target">The target object.</param>
        /// <param name="validateCallback">Validate callback.</param>
        /// <returns>An EditInterface for the specific object.</returns>
        private static EditInterface buildInterface(String name, MemberScanner scanner, Object target, Validate validateCallback)
        {
            EditInterface createdInterface = new EditInterface(name, null, null, validateCallback);
            EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
            propertyInfo.addColumn(new EditablePropertyColumn("Name", true));
            propertyInfo.addColumn(new EditablePropertyColumn("Value", false));
            createdInterface.setPropertyInfo(propertyInfo);
            LinkedList<MemberWrapper> members = scanner.getMatchingMembers(target.GetType());
            foreach (MemberWrapper memberWrapper in members)
            {
                if (ReflectedVariable.canCreateVariable(memberWrapper.getWrappedType()))
                {
                    createdInterface.addEditableProperty(new ReflectedEditableProperty(memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target)));
                }
                else
                {
                    Object subObject = memberWrapper.getValue(target, null);
                    createdInterface.addSubInterface(buildInterface(memberWrapper.getWrappedName() + " - " + memberWrapper.getWrappedType(), scanner, subObject, validateCallback));
                }
            }
            return createdInterface;
        }

        #endregion Helper Functions

        #region Constructors

        /// <summary>
        /// Do nothing constructor.
        /// </summary>
        private ReflectedEditInterface()
        {

        }

        #endregion Constructors
    }
}
