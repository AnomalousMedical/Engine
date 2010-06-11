﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace CEGUIPlugin
{
    /// <summary>
    /// This class wraps up a single CEGUI event and allows it to be dispatched
    /// using a .net event. It does this by subscribing to the event with a
    /// native class also called WindowEventTranslator that calls back into this
    /// class, which fires the event. It will automatically bind and unbind the
    /// event on the native side as needed. To use the class create an instance of it.
    /// <code>
    /// clickedTranslator = new WindowEventTranslator("Clicked", this);
    /// </code>
    /// Then create a public delegate and overwrite add and remove to forward to this class.
    /// <code>
    /// public event CEGUIEvent Clicked
    /// {
    ///     add
    ///     {
    ///         clickedTranslator.BoundEvent += value;
    ///     }
    ///     remove
    ///     {
    ///         clickedTranslator.BoundEvent -= value;
    ///     }
    /// }
    /// </code>
    /// Don't forget to dispose it in your dispose method.
    /// </summary>
    abstract class CEGUIEventTranslator : IDisposable
    {
        Window ceguiWindow;
        IntPtr nativeEventTranslator;

        private event CEGUIEvent m_BoundEvent;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventName">The name of the CEGUI event to listen to.</param>
        /// <param name="ceguiWindow">The window to subscribe to.</param>
        protected CEGUIEventTranslator(Window ceguiWindow)
        {
            this.ceguiWindow = ceguiWindow;
        }

        /// <summary>
        /// This function must be called in subclass constructors. It must be
        /// like this since the event must be made in the constructor.
        /// </summary>
        /// <param name="nativeEventTranslator"></param>
        protected void setNativeEventTranslator(IntPtr nativeEventTranslator)
        {
            this.nativeEventTranslator = nativeEventTranslator;
        }

        /// <summary>
        /// Dispose the native class.
        /// </summary>
        public void Dispose()
        {
            CEGUIEventTranslator_delete(nativeEventTranslator);
            nativeEventTranslator = IntPtr.Zero;
        }

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event CEGUIEvent BoundEvent
        {
            add
            {
                if (m_BoundEvent == null)
                {
                    CEGUIEventTranslator_bindEvent(nativeEventTranslator, ceguiWindow.CEGUIWindow);
                }
                m_BoundEvent += value;
            }
            remove
            {
                m_BoundEvent -= value;
                if (m_BoundEvent == null)
                {
                    CEGUIEventTranslator_unbindEvent(nativeEventTranslator);
                }
            }
        }

        /// <summary>
        /// Callback from native code.
        /// </summary>
        /// <returns>True, to say the event was handled.</returns>
        protected bool fireEvent(EventArgs args)
        {
            if (m_BoundEvent != null)
            {
                m_BoundEvent.Invoke(args);
            }
            return true;
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIEventTranslator_delete(IntPtr nativeEventTranslator);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIEventTranslator_bindEvent(IntPtr nativeEventTranslator, IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIEventTranslator_unbindEvent(IntPtr nativeEventTranslator);

#endregion
    }
}
