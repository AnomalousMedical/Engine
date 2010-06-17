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
            //Destroy all widgets created by this layout
            uint numWidgets = VectorWidgetPtr_getNumRootWidgets(vectorWidgetPtr).ToUInt32();
            for (uint i = 0; i < numWidgets; i++)
            {
                IntPtr rootWidget = VectorWidgetPtr_getRootWidget(vectorWidgetPtr, new UIntPtr(i));
                Widget.recursiveEraseChildren(rootWidget);
            }
            vectorWidgetPtr = IntPtr.Zero;
        }

        public IntPtr VectorWidgetPtr
        {
            get
            {
                return vectorWidgetPtr;
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr VectorWidgetPtr_getNumRootWidgets(IntPtr vectorWidgetPtr);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr VectorWidgetPtr_getRootWidget(IntPtr vectorWidgetPtr, UIntPtr index);

#endregion
    }
}
