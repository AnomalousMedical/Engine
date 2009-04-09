using System;
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
        #region Fields

        private List<ReflectedVariable> variableList;
        private Object target;
        private MemberScanner memberScanner;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">The target object.</param>
        public ReflectedObjectEditableProperty(Object target)
            : this(target, new MemberScanner(EditableAttributeFilter.Instance))
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
            LinkedList<MemberWrapper> matches = memberScanner.getMatchingMembers(target.GetType());
            variableList = new List<ReflectedVariable>(matches.Count);
            foreach (MemberWrapper wrapper in matches)
            {
                if (ReflectedVariable.canCreateVariable(wrapper.getWrappedType()))
                {
                    variableList.Add(ReflectedVariable.createVariable(wrapper, target));
                }
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get the object that this property is providing an interface for.
        /// </summary>
        /// <returns>The object used to create this interface.</returns>
        public Object getTargetObject()
        {
            return target;
        }

        #region EditableProperty Members

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public object getValue(int column)
        {
            return variableList[column].getValue();
        }

        /// <summary>
        /// Set the value of this property.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to set. Must be the correct type.</param>
        public void setValue(int column, object value)
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

        #endregion

        #endregion Functions
    }
}
