using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventComboAcceptTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, UIntPtr index);
        static ComboBoxEventArgs eventArgs = new ComboBoxEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventComboAcceptTranslator()
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
            return EventComboAcceptTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, UIntPtr index)
        {
            //Fill out the ComboBoxEventArgs
            eventArgs.Index = index.horriblyUnsafeToUInt32();
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventComboAcceptTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}