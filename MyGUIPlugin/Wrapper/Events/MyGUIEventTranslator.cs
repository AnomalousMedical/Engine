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
    abstract class MyGUIEventTranslator : IDisposable
    {
        protected Widget widget;
        protected IntPtr nativeEventTranslator;

        private event MyGUIEvent m_BoundEvent;

        public void initialize(Widget widget)
        {
            this.widget = widget;
            nativeEventTranslator = doInitialize(widget);
        }

        protected abstract IntPtr doInitialize(Widget widget);

        /// <summary>
        /// Dispose the native class.
        /// </summary>
        public virtual void Dispose()
        {
            MyGUIEventTranslator_delete(nativeEventTranslator);
        }

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event MyGUIEvent BoundEvent
        {
            add
            {
                if (m_BoundEvent == null)
                {
                    MyGUIEventTranslator_bindEvent(nativeEventTranslator);
                }
                m_BoundEvent += value;
            }
            remove
            {
                m_BoundEvent -= value;
                if (m_BoundEvent == null)
                {
                    MyGUIEventTranslator_unbindEvent(nativeEventTranslator);
                }
            }
        }

        protected void fireEvent(EventArgs args)
        {
            if (m_BoundEvent != null)
            {
                m_BoundEvent.Invoke(widget, args);
            }
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void MyGUIEventTranslator_delete(IntPtr nativeEventTranslator);

        [DllImport("MyGUIWrapper")]
        private static extern void MyGUIEventTranslator_bindEvent(IntPtr nativeEventTranslator);

        [DllImport("MyGUIWrapper")]
        private static extern void MyGUIEventTranslator_unbindEvent(IntPtr nativeEventTranslator);

        #endregion
    }
}
