using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventComboChangePositionTranslator : MyGUIWidgetEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, UIntPtr index);
        static ComboBoxEventArgs eventArgs = new ComboBoxEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventComboChangePositionTranslator()
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
            return EventComboChangePositionTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, UIntPtr index)
        {
            //Fill out the ComboBoxEventArgs
            eventArgs.Index = index.ToUInt32();
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventComboChangePositionTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}