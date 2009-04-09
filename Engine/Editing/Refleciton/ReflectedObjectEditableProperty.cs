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
        private List<ReflectedVariable> variableList;
        private Object target;
        private MemberScanner memberScanner;

        public ReflectedObjectEditableProperty(Object target)
            : this(target, new MemberScanner(EditableAttributeFilter.Instance))
        {
                        
        }

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

        #region EditableProperty Members

        public object getValue(int column)
        {
            return variableList[column].getValue();
        }

        public void setValue(int column, object value)
        {
            variableList[column].setValue(value);
        }

        public void setValueStr(int column, string value)
        {
            variableList[column].setValueString(value);
        }

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

        public Type getPropertyType(int column)
        {
            return variableList[column].getVariableType();
        }

        #endregion
    }
}
