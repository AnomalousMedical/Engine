using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundPlugin
{
    public class OggEncoder
    {
        public static bool encodeToStream(Stream source, Stream destination)
        {
            return OggEncoder_encodeToStream(new ManagedStream(source).Pointer, new ManagedStream(destination).Pointer);
        }

        #region PInvoke

        [DllImport("SoundWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OggEncoder_encodeToStream(IntPtr source, IntPtr destination);

        #endregion
    }
}
