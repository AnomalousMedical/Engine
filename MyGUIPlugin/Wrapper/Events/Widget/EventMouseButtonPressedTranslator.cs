﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    class EventMouseButtonPressedTranslator : MyGUIWidgetEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, int left, int top, MouseButtonCode id);
        static MouseEventArgs eventArgs = new MouseEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventMouseButtonPressedTranslator()
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
            return EventMouseButtonPressedTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, int left, int top, MouseButtonCode id)
        {
            //Fill out the MouseEventArgs
            eventArgs.Button = id;
            eventArgs.Position = new Vector2(left, top);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventMouseButtonPressedTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}