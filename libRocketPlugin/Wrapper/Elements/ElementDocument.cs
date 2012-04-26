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

        internal ElementDocument(IntPtr ptr)
            : base(ptr)
        {

        }

        public void Show()
        {
            ElementDocument_Show(ptr);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_Show(IntPtr elementDocument);

        #endregion
    }
}
