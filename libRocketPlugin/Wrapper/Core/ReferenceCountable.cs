using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class ReferenceCountable : RocketNativeObject, IDisposable
    {
        /// <summary>
        /// Default constructor, if this is used call setPtr at some point in the base class constructor.
        /// </summary>
        public ReferenceCountable()
        {

        }

        public ReferenceCountable(IntPtr ptr)
            :base(ptr)
        {

        }

        /// <summary>
        /// This method calls remove reference. This allows these classes to be used in using statements.
        /// If this method is called there is no need to call removeReference for this instance.
        /// </summary>
        public virtual void Dispose()
        {
            removeReference();
        }

        public void addReference()
        {
            ReferenceCountable_AddReference(ptr);
        }

        public void removeReference()
        {
            ReferenceCountable_RemoveReference(ptr);
        }

        public int ReferenceCount
        {
            get
            {
                return ReferenceCountable_GetReferenceCount(ptr);
            }
        }

        public static void DumpLeakReport()
        {
            ReferenceCountable_DumpLeakReport();
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int ReferenceCountable_GetReferenceCount(IntPtr refCount);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReferenceCountable_AddReference(IntPtr refCount);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReferenceCountable_RemoveReference(IntPtr refCount);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReferenceCountable_DumpLeakReport();

        #endregion
    }
}
