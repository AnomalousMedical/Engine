using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace SoundPlugin
{
    public class Listener : SoundPluginObject
    {
        internal Listener(IntPtr listener)
            :base(listener)
        {

        }

        public void setOrientation(Vector3 at, Vector3 up)
        {
            Listener_setOrientation(Pointer, at, up);
        }

        public float Gain
        {
            get
            {
                return Listener_getGain(Pointer);
            }
            set
            {
                Listener_setGain(Pointer, value);
            }
        }

        public Vector3 Position
        {
            get
            {
                return Listener_getPosition(Pointer);
            }
            set
            {
                Listener_setPosition(Pointer, value);
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return Listener_getVelocity(Pointer);
            }
            set
            {
                Listener_setVelocity(Pointer, value);
            }
        }

        public Vector3 OrientationAt
        {
            get
            {
                Vector3 at;
                Vector3 up;
                Listener_getOrientation(Pointer, out at, out up);
                return at;
            }
            set
            {
                Listener_setOrientation(Pointer, value, OrientationUp);
            }
        }

        public Vector3 OrientationUp
        {
            get
            {
                Vector3 at;
                Vector3 up;
                Listener_getOrientation(Pointer, out at, out up);
                return up;
            }
            set
            {
                Listener_setOrientation(Pointer, OrientationAt, value);
            }
        }

        #region PInvoke

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Listener_getGain(IntPtr listener);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Listener_setGain(IntPtr listener, float value);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Listener_getPosition(IntPtr listener);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Listener_setPosition(IntPtr listener, Vector3 value);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Listener_getVelocity(IntPtr listener);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Listener_setVelocity(IntPtr listener, Vector3 value);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Listener_getOrientation(IntPtr listener, out Vector3 at, out Vector3 up);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Listener_setOrientation(IntPtr listener, Vector3 at, Vector3 up);

        #endregion
    }
}
