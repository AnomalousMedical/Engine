using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class ElementDocument : ReferenceCountable
    {
        public ElementDocument(IntPtr ptr)
            : base(ptr)
        {

        }

        public void show()
        {
            ElementDocument_Show(ptr);
        }

        internal static ElementDocument Wrap(IntPtr elementDocument)
        {
            if (elementDocument != IntPtr.Zero)
            {
                return new ElementDocument(elementDocument);
            }
            return null;
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_Show(IntPtr elementDocument);

        #endregion
    }
}
