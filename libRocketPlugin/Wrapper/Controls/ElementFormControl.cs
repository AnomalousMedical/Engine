using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

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
                ElementFormControl_GetValue(ptr, stringRetriever.StringCallback);
                return stringRetriever.retrieveString();
            }
            set
            {
                ElementFormControl_SetValue(ptr, value);
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementFormControl_GetValue(IntPtr elementFormControl, StringRetriever.Callback retrieve);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementFormControl_SetValue(IntPtr elementFormControl, String value);

        #endregion
    }
}
