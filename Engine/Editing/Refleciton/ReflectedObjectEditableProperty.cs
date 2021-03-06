﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This class will read an object and build a EditableProperty out of it
    /// with multiple columns. Useful for editing data in a collection of some
    /// kind. Note that this can only process types that have a
    /// ReflectedVariable instance.
    /// </summary>
    public class ReflectedObjectEditableProperty : EditableProperty
    {
        private static MemberScanner sharedScanner;

        static ReflectedObjectEditableProperty()
        {
            sharedScanner = new FilteredMemberScanner(new EditableAttributeFilter());
        }

        private List<ReflectedVariable> variableList;
        private Object target;
        private MemberScanner memberScanner;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">The target object.</param>
        public ReflectedObjectEditableProperty(Object target)
            : this(target, sharedScanner)
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="scanner">The MemberScanner to use to learn about target.</param>
        public ReflectedObjectEditableProperty(Object target, MemberScanner scanner)
        {
            this.target = target;
            this.memberScanner = scanner;
            IEnumerable<MemberWrapper> matches = memberScanner.getMatchingMembers(target.GetType());
            variableList = new List<ReflectedVariable>();
            foreach (MemberWrapper wrapper in matches)
            {
                if (ReflectedVariable.canCreateVariable(wrapper.getWrappedType()))
                {
                    variableList.Add(ReflectedVariable.createVariable(wrapper, target));
                }
            }
        }

        /// <summary>
        /// Get the object that this property is providing an interface for.
        /// </summary>
        /// <returns>The object used to create this interface.</returns>
        public Object getTargetObject()
        {
            return target;
        }

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public String getValue(int column)
        {
            return variableList[column].getValue().ToString();
        }

        public Object getRealValue(int column)
        {
            return variableList[column].getValue();
        }

        public void setValue(int column, Object value)
        {
            variableList[column].setValue(value);
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void setValueStr(int column, string value)
        {
            variableList[column].setValueString(value);
        }

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(int column, string value, out string errorMessage)
        {
            if (variableList[column].canParseString(value))
            {
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = String.Format("Cannot parse value in column {0}", column);
                return false;
            }
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <param name="column"></param>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType(int column)
        {
            return variableList[column].getVariableType();
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public bool readOnly(int column)
        {
            return false;
        }

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        public bool Advanced { get; set; }
    }
}
