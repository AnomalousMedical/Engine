﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;
using Engine.Attributes;

namespace Engine
{
    class CopyMemberScanner : MemberScannerFilter
    {
        private static MemberScanner scanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static CopyMemberScanner()
        {
            scanner = new FilteredMemberScanner(new CopyMemberScanner());
        }

        /// <summary>
        /// The member scanner defined by this class.
        /// </summary>
        public static MemberScanner Scanner
        {
            get
            {
                return scanner;
            }
        }

        /// <summary>
        /// Hide constructor.
        /// </summary>
        private CopyMemberScanner()
        {

        }

        /// <summary>
        /// Filter out all members that are marked with DoNotCopy or that cannot
        /// be read from and written to.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public bool allowMember(MemberWrapper wrapper)
        {
            Type wrappedType = wrapper.getWrappedType();

            bool reject = wrapper.getCustomAttributes(typeof(DoNotCopyAttribute), true).Any()
             || wrappedType.GetCustomAttributes(typeof(DoNotCopyAttribute), true).Any()
             || wrappedType.GetCustomAttributes(typeof(NativeSubsystemTypeAttribute), true).Any()
             || wrappedType.IsSubclassOf(typeof(Delegate));

            return !reject && wrapper.canWrite() && wrapper.canRead();
        }

        /// <summary>
        /// This function determines if the given type should be scanned for
        /// members. It will return true if the member should be accepted.
        /// </summary>
        /// <param name="type">The type to potentially scan for members.</param>
        /// <returns>True if the type should be scanned.</returns>
        public bool allowType(Type type)
        {
            return !type.GetCustomAttributes(typeof(DoNotCopyAttribute), false).Any();
        }
    }
}
