using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class ClickEventTranslator : MyGUIEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget);

        private NativeEventDelegate nativeEventCallback;

        public ClickEventTranslator()
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return ClickEventTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget)
        {
            fireEvent(EventArgs.Empty);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr ClickEventTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

#endregion
    }
}
