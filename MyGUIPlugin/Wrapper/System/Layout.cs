using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Layout : IDisposable
    {
        internal static Layout createWrapper(IntPtr vectorWidgetPtr, object[] args)
        {
            return new Layout(vectorWidgetPtr);
        }

        IntPtr vectorWidgetPtr;

        private Layout(IntPtr vectorWidgetPtr)
        {
            this.vectorWidgetPtr = vectorWidgetPtr;
        }

        public void Dispose()
        {
            vectorWidgetPtr = IntPtr.Zero;
        }

        internal IntPtr VectorWidgetPtr
        {
            get
            {
                return vectorWidgetPtr;
            }
        }

        public uint getNumWidgets()
        {
            return VectorWidgetPtr_getNumRootWidgets(vectorWidgetPtr).ToUInt32();
        }

        public Widget getWidget(uint index)
        {
            return WidgetManager.getWidget(VectorWidgetPtr_getRootWidget(vectorWidgetPtr, new UIntPtr(index)));
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VectorWidgetPtr_getNumRootWidgets(IntPtr vectorWidgetPtr);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr VectorWidgetPtr_getRootWidget(IntPtr vectorWidgetPtr, UIntPtr index);

#endregion
    }
}
