using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    class EventMouseMoveTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, int left, int top);
        static MouseEventArgs eventArgs = new MouseEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventMouseMoveTranslator()
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
            eventArgs.Button = MouseButtonCode.NUM_BUTTONS;
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return EventMouseMoveTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, int left, int top)
        {
            //Fill out the MouseEventArgs
            eventArgs.Position = new IntVector2(left, top);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr EventMouseMoveTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
