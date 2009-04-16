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

        public static void RestoreObject(Object source, LoadInfo info, MemberScanner scanner)
        {
            foreach (MemberWrapper wrapper in scanner.getMatchingMembers(source.GetType()))
            {
                if (info.hasValue(wrapper.getWrappedName()))
                {
                    wrapper.setValue(source, info.getValueObject(wrapper.getWrappedName()), null);
                }
            }
        }
    }
}
