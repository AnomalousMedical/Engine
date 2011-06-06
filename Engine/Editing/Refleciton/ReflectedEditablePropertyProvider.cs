using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Reflection;

namespace Engine
{
    public interface ReflectedEditablePropertyProvider
    {
        /// <summary>
        /// Create the property for the MemberWrapper provided and add it to the
        /// provided EditInterface. Return true to skip using default member
        /// scanners for the given MemberWrapper. This is likely what you will
        /// want to do if you add anything to the EditInterface. If you make no
        /// changes and want the scanner to add its own EditableProperties
        /// return false.
        /// </summary>
        /// <param name="memberWrapper">The member to wrap.</param>
        /// <returns>True to skip the rest of the scan process for this MemberWrapper.</returns>
        bool addProperties(MemberWrapper memberWrapper, Object instance, EditInterface editInterface);
    }

    class DefaultReflectedEditablePropertyProvider : ReflectedEditablePropertyProvider
    {
        private static DefaultReflectedEditablePropertyProvider instance;

        public static ReflectedEditablePropertyProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultReflectedEditablePropertyProvider();
                }
                return instance;
            }
        }

        public bool addProperties(MemberWrapper memberWrapper, Object instance, EditInterface editInterface)
        {
            return false;
        }
    }
}
