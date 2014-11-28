using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public delegate void MyGUIEvent(Widget source, EventArgs e);

    /// <summary>
    /// This class wraps up a single MyGUI event and allows it to be dispatched
    /// using a .net event. It does this by subscribing to the event with a
    /// native class also called WindowEventTranslator that calls back into this
    /// class, which fires the event. It will automatically bind and unbind the
    /// event on the native side as needed.
    /// </summary>
    abstract class MyGUIWidgetEventTranslator : MyGUIEventTranslator
    {
        protected Widget widget;

        private event MyGUIEvent boundEvent;

        public void initialize(Widget widget)
        {
            this.widget = widget;
            nativeEventTranslator = doInitialize(widget);
        }

        protected abstract IntPtr doInitialize(Widget widget);

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event MyGUIEvent BoundEvent
        {
            add
            {
                if (boundEvent == null)
                {
                    MyGUIEventTranslator_bindEvent(nativeEventTranslator);
                }
                boundEvent += value;
            }
            remove
            {
                boundEvent -= value;
                if (boundEvent == null)
                {
                    MyGUIEventTranslator_unbindEvent(nativeEventTranslator);
                }
            }
        }

        protected void fireEvent(EventArgs args)
        {
            if (boundEvent != null)
            {
                boundEvent.Invoke(widget, args);
            }
        }
    }
}
