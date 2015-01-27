using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Anomalous.Interop;

namespace libRocketPlugin
{
    public class ElementFormControl : Element
    {
        internal ElementFormControl(IntPtr ptr)
            :base(ptr)
        {

        }

        public String Value
        {
            get
            {
                ElementFormControl_GetValue(ptr, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                ElementFormControl_SetValue(ptr, value);
            }
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementFormControl_GetValue(IntPtr elementFormControl, StringRetriever.Callback retrieve, IntPtr handle);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementFormControl_SetValue(IntPtr elementFormControl, String value);

        #endregion
    }
}
