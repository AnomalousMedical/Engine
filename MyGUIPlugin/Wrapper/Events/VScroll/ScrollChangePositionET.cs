using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public class ScrollEventArgs : EventArgs
    {
        public uint Position { get; internal set; }
    }

    class ScrollChangePositionET : MyGUIWidgetEventTranslator
    {
        static ScrollEventArgs eventArgs = new ScrollEventArgs();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, UIntPtr position);

        private NativeEventDelegate nativeEventCallback;

        public ScrollChangePositionET()
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
            return ScrollChangePositionET_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, UIntPtr position)
        {
            eventArgs.Position = position.ToUInt32();
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr ScrollChangePositionET_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}