using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class ElementDocument : Element
    {
        public enum FocusFlags
        {
            NONE = 0,
            FOCUS = (1 << 1),
            MODAL = (1 << 2)
        };

        public ElementDocument(IntPtr ptr)
            : base(ptr)
        {

        }

        public void Show()
        {
            ElementDocument_Show(ptr);
        }

        internal static ElementDocument WrapElementDocument(IntPtr elementDocument)
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
