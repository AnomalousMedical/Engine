using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    /// <summary>
    /// This is a base class for all event translators. It provides the native
    /// function calls to access the native classes that "derive" from this one.
    /// </summary>
    abstract class MyGUIEventTranslator : IDisposable
    {
        protected IntPtr nativeEventTranslator;

        /// <summary>
        /// Dispose the native class.
        /// </summary>
        public virtual void Dispose()
        {
            MyGUIEventTranslator_delete(nativeEventTranslator);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void MyGUIEventTranslator_delete(IntPtr nativeEventTranslator);

        [DllImport("MyGUIWrapper")]
        protected static extern void MyGUIEventTranslator_bindEvent(IntPtr nativeEventTranslator);

        [DllImport("MyGUIWrapper")]
        protected static extern void MyGUIEventTranslator_unbindEvent(IntPtr nativeEventTranslator);

        #endregion
    }
}
