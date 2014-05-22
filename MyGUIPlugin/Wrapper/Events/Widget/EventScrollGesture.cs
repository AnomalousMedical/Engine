using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine;

namespace MyGUIPlugin
{
    class EventScrollGesture : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, int absx, int absy, int deltax, int deltay);
        static ScrollGestureEventArgs eventArgs = new ScrollGestureEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventScrollGesture()
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
            return EventScrollGesture_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, int absx, int absy, int deltax, int deltay)
        {
            //Fill out the ScrollGestureEventArgs
            eventArgs.AbsX = absx;
            eventArgs.AbsY = absy;
            eventArgs.DeltaX = deltax;
            eventArgs.DeltaY = deltay;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventScrollGesture_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}