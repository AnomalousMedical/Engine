using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Interop
{
//NativeFunc - functions that return values, the last value in the list is what it returns.

//NativeAction - functions that take values and return void

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NativeAction(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NativeAction_Bool(bool arg0
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

//NativeAction no handles - these do not have instanceHandles passed no matter what mode.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NativeAction_String_NoHandle(IntPtr str0);
}