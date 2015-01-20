using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundPlugin
{
    public class OggEncoder : IDisposable
    {
        private IntPtr ptr;

        public OggEncoder()
        {
            ptr = OggEncoder_create();
        }

        public void Dispose()
        {
            OggEncoder_delete(ptr);
            ptr = IntPtr.Zero;
        }

        public bool encodeToStream(Stream source, Stream destination)
        {
            return OggEncoder_encodeToStream(ptr, new ManagedStream(source).Pointer, new ManagedStream(destination).Pointer);
        }

        public long Channels
        {
            get
            {
                return OggEncoder_getChannels(ptr);
            }
            set
            {
                OggEncoder_setChannels(ptr, value);
            }
        }

        public long Rate
        {
            get
            {
                return OggEncoder_getRate(ptr);
            }
            set
            {
                OggEncoder_setRate(ptr, value);
            }
        }

        public float BaseQuality
        {
            get
            {
                return OggEncoder_getBaseQuality(ptr);
            }
            set
            {
                OggEncoder_setBaseQuality(ptr, value);
            }
        }

        #region PInvoke

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OggEncoder_create();

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OggEncoder_delete(IntPtr encoder);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OggEncoder_encodeToStream(IntPtr encoder, IntPtr source, IntPtr destination);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern long OggEncoder_getChannels(IntPtr encoder);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OggEncoder_setChannels(IntPtr encoder, long value);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern long OggEncoder_getRate(IntPtr encoder);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OggEncoder_setRate(IntPtr encoder, long value);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float OggEncoder_getBaseQuality(IntPtr encoder);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OggEncoder_setBaseQuality(IntPtr encoder, float value);

        #endregion
    }
}
