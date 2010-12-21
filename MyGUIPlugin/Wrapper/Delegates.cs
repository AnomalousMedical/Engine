using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    /// <summary>
    /// This delegate should be used when MyGUI is returning a temporary string.
    /// Since a temporary string will likely be destroyed crossing the P/Invoke
    /// barrier.
    /// </summary>
    /// <param name="str">The pointer to the temporary string.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void TempStringCallback(IntPtr str);
}
