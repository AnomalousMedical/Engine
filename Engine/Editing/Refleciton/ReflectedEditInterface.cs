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
        /// Create an empty EditInterface appropriate to use with the ReflectedEditInterface.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        /// <param name="validateCallback">A validation callback.</param>
        /// <returns>A new EditInterface.</returns>
        public static EditInterface createEditInterface(String name, Validate validateCallback)
        {
            EditInterface edit = buildInterface(name, validateCallback);
            return edit;
        }

        /// <summary>
        /// Create a new EditInterface and add the properties discovered by
        /// scanner. A new one is created every time this function is called.
        /// </summary>
        /// <param name="target">The target object to scan.</param>
        /// <param name="scanner">The scanner to use.</param>
        /// <param name="name">The name of the EditInterface.</param>
        /// <param name="validateCallback">A callback to set for validating.</param>
        /// <returns>A new EditInterface from the scanned info.</returns>
        public static EditInterface createEditInterface(Object target, MemberScanner scanner, String name, Validate validateCallback)
        {
            EditInterface edit = buildInterface(name, validateCallback);
            addProperties(scanner, target, edit);
            return edit;
        }

        /// <summary>
        /// Add properties to an EditInterface using reflection to discover
        /// them. It is assumed that the PropertyInfo will be appropriate for
        /// name value pairs or else you might get errors when the editors
        /// attempt to edit the object.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="scanner">The MemberScanner to use.</param>
        /// <param name="edit">The EditInterface that accepts name/value pair properties to add properties to.</param>
        public static void expandEditInterface(Object target, MemberScanner scanner, EditInterface edit)
        {
            addProperties(scanner, target, edit);
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
        private static EditInterface buildInterface(String name, Validate validateCallback)
        {
            EditInterface createdInterface = new EditInterface(name, null, null, validateCallback);
            EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
            propertyInfo.addColumn(new EditablePropertyColumn("Name", true));
            propertyInfo.addColumn(new EditablePropertyColumn("Value", false));
            createdInterface.setPropertyInfo(propertyInfo);
            return createdInterface;
        }

        /// <summary>
        /// Add properties to an existing EditInterface.
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="target"></param>
        /// <param name="edit"></param>
        private static void addProperties(MemberScanner scanner, Object target, EditInterface edit)
        {
            LinkedList<MemberWrapper> members = scanner.getMatchingMembers(target.GetType());
            foreach (MemberWrapper memberWrapper in members)
            {
                if (ReflectedVariable.canCreateVariable(memberWrapper.getWrappedType()))
                {
                    edit.addEditableProperty(new ReflectedEditableProperty(memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target)));
                }
                else
                {
                    Object subObject = memberWrapper.getValue(target, null);
                    edit.addSubInterface(createEditInterface(subObject, scanner, memberWrapper.getWrappedName() + " - " + memberWrapper.getWrappedType(), null));
                }
            }
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
