using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Saving
{
    /// <summary>
    /// This class will scan an object and save or restore it.
    /// </summary>
    public class ReflectedSaver
    {
        /// <summary>
        /// Hide constructor.
        /// </summary>
        private ReflectedSaver()
        {

        }

        /// <summary>
        /// Save an object to a SaveInfo using the default scanner.
        /// </summary>
        /// <param name="source">The source object to save.</param>
        /// <param name="info">The info to save the object to.</param>
        public static void SaveObject(Object source, SaveInfo info)
        {
            SaveObject(source, info, DefaultScanner);
        }

        /// <summary>
        /// Save an object to a SaveInfo.
        /// </summary>
        /// <param name="source">The source object to save.</param>
        /// <param name="info">The info to save the object to.</param>
        /// <param name="scanner">The MemberScanner to use to save the object.</param>
        public static void SaveObject(Object source, SaveInfo info, MemberScanner scanner)
        {
            foreach (MemberWrapper wrapper in scanner.getMatchingMembers(source.GetType()))
            {
                info.AddReflectedValue(wrapper.getWrappedName(), wrapper.getValue(source, null), wrapper.getWrappedType());
            }
        }

        /// <summary>
        /// Restore an object from a LoadInfo using the default scanner.
        /// </summary>
        /// <param name="source">The object to restore values to.</param>
        /// <param name="info">The LoadInfo with the values to restore.</param>
        public static void RestoreObject(Object source, LoadInfo info)
        {
            RestoreObject(source, info, DefaultScanner);
        }

        /// <summary>
        /// Restore an object from a LoadInfo.
        /// </summary>
        /// <param name="source">The object to restore values to.</param>
        /// <param name="info">The LoadInfo with the values to restore.</param>
        /// <param name="scanner">A MemberScanner to use to restore the object.</param>
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

        public static MemberScanner DefaultScanner
        {
            get
            {
                return BehaviorSaveMemberScanner.Scanner;
            }
        }
    }
}