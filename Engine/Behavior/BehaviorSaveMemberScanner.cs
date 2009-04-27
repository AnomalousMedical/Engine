using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine.Reflection;

namespace Engine
{
    /// <summary>
    /// This class will scan a behavior and find all the members to be saved.
    /// The filter will save all public and private fields that are the basic
    /// types or that extend BehaviorObjectBase. The filter will not save fields
    /// that are marked DoNotSave or are a field for a class marked DoNotSave.
    /// This filter does not process properties.
    /// </summary>
    class BehaviorSaveMemberScanner : MemberScannerFilter
    {
        private static MemberScanner scanner;

        static BehaviorSaveMemberScanner()
        {
            scanner = new MemberScanner(new BehaviorSaveMemberScanner());
            scanner.ProcessProperties = false;
            scanner.TerminatingType = typeof(Behavior);
        }

        public static MemberScanner Scanner
        {
            get
            {
                return scanner;
            }
        }

        private BehaviorSaveMemberScanner()
        {

        }

        public bool allowMember(MemberWrapper wrapper)
        {
            return wrapper.getCustomAttributes(typeof(DoNotSaveAttribute), true).Length == 0 
                && wrapper.getWrappedType().GetCustomAttributes(typeof(DoNotSaveAttribute), true).Length == 0;
        }
    }
}
