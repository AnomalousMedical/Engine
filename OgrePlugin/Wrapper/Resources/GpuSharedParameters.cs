using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class GpuSharedParameters : IDisposable
    {
        internal static GpuSharedParameters createWrapper(IntPtr ptr)
        {
            return new GpuSharedParameters(ptr);
        }

        private IntPtr ptr;

        internal GpuSharedParameters(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public void Dispose()
        {
            ptr = IntPtr.Zero;
        }

        public void addNamedConstant(String name, GpuConstantType constType, uint arraySize = 1)
        {
            GpuSharedParameters_addConstantDefinition(ptr, name, constType, new UIntPtr(arraySize));
        }

        public void setNamedConstant(String name, float val)
        {
            GpuSharedParameters_setNamedConstant1(ptr, name, val);
        }

        public void setNamedConstant(String name, int val)
        {
            GpuSharedParameters_setNamedConstant2(ptr, name, val);
        }

        public void setNamedConstant(String name, Quaternion vec)
        {
            GpuSharedParameters_setNamedConstant3(ptr, name, vec);
        }

        public void setNamedConstant(String name, Vector3 vec)
        {
            GpuSharedParameters_setNamedConstant4(ptr, name, vec);
        }

        public void setNamedConstant(String name, Vector2 vec)
        {
            GpuSharedParameters_setNamedConstant5(ptr, name, vec);
        }

        public void setNamedConstant(String name, Color colour)
        {
            GpuSharedParameters_setNamedConstant6(ptr, name, colour);
        }

        public unsafe void setNamedConstant(String name, int* val, UIntPtr count)
        {
            GpuSharedParameters_setNamedConstant7(ptr, name, val, count);
        }

        public unsafe void setNamedConstant(String name, float* val, UIntPtr count)
        {
            GpuSharedParameters_setNamedConstant8(ptr, name, val, count);
        }

        public unsafe void setNamedConstant(String name, double* val, UIntPtr count)
        {
            GpuSharedParameters_setNamedConstant9(ptr, name, val, count);
        }

        #region PInvoke
        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_addConstantDefinition(IntPtr param, String name, GpuConstantType constType, UIntPtr arraySize);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant1(IntPtr param, String name, float val);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant2(IntPtr param, String name, int val);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant3(IntPtr param, String name, Quaternion vec);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant4(IntPtr param, String name, Vector3 vec);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant5(IntPtr param, String name, Vector2 vec);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuSharedParameters_setNamedConstant6(IntPtr param, String name, Color colour);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void GpuSharedParameters_setNamedConstant7(IntPtr param, String name, int* val, UIntPtr count);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void GpuSharedParameters_setNamedConstant8(IntPtr param, String name, float* val, UIntPtr count);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void GpuSharedParameters_setNamedConstant9(IntPtr param, String name, double* val, UIntPtr count);

        #endregion
    }
}
