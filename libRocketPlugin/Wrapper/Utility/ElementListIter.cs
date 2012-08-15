using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    class ElementListIter : RocketNativeObject, IDisposable
    {
        public ElementListIter()
            :base(ElementListIter_Create())
        {

        }

        public void Dispose()
        {
            ElementListIter_Delete(ptr);
        }

        public void startIterator()
        {
            ElementListIter_startIterator(ptr);
        }

        public Element getNextElement()
        {
            return ElementManager.getElement(ElementListIter_getNextElement(ptr));
        }

#region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementListIter_Create();

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementListIter_Delete(IntPtr elementListIter);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementListIter_startIterator(IntPtr elementListIter);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementListIter_getNextElement(IntPtr elementListIter);

#endregion
    }
}
