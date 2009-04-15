using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Saving
{
    public class ReflectedSaver
    {
        private ReflectedSaver()
        {

        }

        public static void SaveObject(Object source, SaveInfo info, MemberScanner scanner)
        {
            foreach (MemberWrapper wrapper in scanner.getMatchingMembers(source.GetType()))
            {
                info.AddReflectedValue(wrapper.getWrappedName(), wrapper.getValue(source, null), wrapper.getWrappedType());
            }
        }
    }
}
