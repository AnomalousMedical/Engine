using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Interop
{
//NativeAction - functions that take values and return void

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NativeAction(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);
}