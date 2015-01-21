using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool NativeFunc_Bool(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int NativeFunc_Int(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NativeAction(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);
}
