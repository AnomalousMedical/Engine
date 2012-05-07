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

        private static MemberScanner defaultScanner = new MemberScanner(new EditableAttributeFilter());

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
            return createEditInterface(target, scanner, name, validateCallback, DefaultReflectedEditablePropertyProvider.Instance);
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
        public static EditInterface createEditInterface(Object target, MemberScanner scanner, String name, Validate validateCallback, ReflectedEditablePropertyProvider customPropProvider)
        {
            EditInterface edit = buildInterface(name, validateCallback);
            addProperties(scanner, target, target.GetType(), edit, customPropProvider);
            return edit;
        }

        /// <summary>
        /// Create a new EditInterface and add the properties discovered by
        /// scanner. A new one is created every time this function is called.
        /// </summary>
        /// <param name="target">The target object to scan.</param>
        /// <param name="startType">The type to start the scan on. Can be used to specify something higher than the target's type up the hierarchy.</param>
        /// <param name="scanner">The scanner to use.</param>
        /// <param name="name">The name of the EditInterface.</param>
        /// <param name="validateCallback">A callback to set for validating.</param>
        /// <returns>A new EditInterface from the scanned info.</returns>
        public static EditInterface createEditInterface(Object target, Type startType, MemberScanner scanner, String name, Validate validateCallback)
        {
            return createEditInterface(target, startType, scanner, name, validateCallback, DefaultReflectedEditablePropertyProvider.Instance);
        }

        /// <summary>
        /// Create a new EditInterface and add the properties discovered by
        /// scanner. A new one is created every time this function is called.
        /// </summary>
        /// <param name="target">The target object to scan.</param>
        /// <param name="startType">The type to start the scan on. Can be used to specify something higher than the target's type up the hierarchy.</param>
        /// <param name="scanner">The scanner to use.</param>
        /// <param name="name">The name of the EditInterface.</param>
        /// <param name="validateCallback">A callback to set for validating.</param>
        /// <returns>A new EditInterface from the scanned info.</returns>
        public static EditInterface createEditInterface(Object target, Type startType, MemberScanner scanner, String name, Validate validateCallback, ReflectedEditablePropertyProvider customPropProvider)
        {
            EditInterface edit = buildInterface(name, validateCallback);
            addProperties(scanner, target, startType, edit, customPropProvider);
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
            expandEditInterface(target, scanner, edit, DefaultReflectedEditablePropertyProvider.Instance);
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
        public static void expandEditInterface(Object target, MemberScanner scanner, EditInterface edit, ReflectedEditablePropertyProvider customPropProvider)
        {
            addProperties(scanner, target, target.GetType(), edit, customPropProvider);
        }

        /// <summary>
        /// Add values to an edit interface.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="target">The target object.</param>
        /// <param name="startType">The type to start the scan on. Can be used to specify something higher than the target's type up the hierarchy.</param>
        /// <param name="scanner">The MemberScanner to use.</param>
        /// <param name="edit">The EditInterface that accepts name/value pair properties to add properties to.</param>
        public static void expandEditInterface(Object target, Type startType, MemberScanner scanner, EditInterface edit)
        {
            expandEditInterface(target, startType, scanner, edit, DefaultReflectedEditablePropertyProvider.Instance);
        }

        /// <summary>
        /// Add values to an edit interface.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="target">The target object.</param>
        /// <param name="startType">The type to start the scan on. Can be used to specify something higher than the target's type up the hierarchy.</param>
        /// <param name="scanner">The MemberScanner to use.</param>
        /// <param name="edit">The EditInterface that accepts name/value pair properties to add properties to.</param>
        public static void expandEditInterface(Object target, Type startType, MemberScanner scanner, EditInterface edit, ReflectedEditablePropertyProvider customPropProvider)
        {
            addProperties(scanner, target, startType, edit, customPropProvider);
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
        private static void addProperties(MemberScanner scanner, Object target, Type startType, EditInterface edit, ReflectedEditablePropertyProvider customPropProvider)
        {
            LinkedList<MemberWrapper> members = scanner.getMatchingMembers(startType);
            foreach (MemberWrapper memberWrapper in members)
            {
                if (!customPropProvider.addProperties(memberWrapper, target, edit))
                {
                    if (ReflectedVariable.canCreateVariable(memberWrapper.getWrappedType()))
                    {
                        Object[] editAttrs = memberWrapper.getCustomAttributes(typeof(EditableAttribute), true);
                        if (editAttrs.Length > 0)
                        {
                            EditableAttribute editable = (EditableAttribute)editAttrs[0];
                            edit.addEditableProperty(editable.createEditableProperty(memberWrapper, target));
                        }
                        else
                        {
                            edit.addEditableProperty(new ReflectedEditableProperty(memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target)));
                        }
                    }
                    else
                    {
                        Object subObject = memberWrapper.getValue(target, null);
                        if (subObject is EditInterfaceOverride)
                        {
                            EditInterfaceOverride custom = (EditInterfaceOverride)subObject;
                            edit.addSubInterface(custom.getEditInterface(memberWrapper.getWrappedName(), scanner));
                        }
                        else if (subObject != null)
                        {
                            edit.addSubInterface(createEditInterface(subObject, scanner, memberWrapper.getWrappedName() + " - " + memberWrapper.getWrappedType(), null));
                        }
                    }
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
