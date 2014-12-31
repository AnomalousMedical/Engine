using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class SubMesh : IDisposable
    {
        internal static SubMesh createWrapper(IntPtr subMesh, object[] args)
        {
            return new SubMesh(subMesh);
        }

        IntPtr subMesh;
        VertexData vertex;
        IndexData index;

        private SubMesh(IntPtr subMesh)
        {
            this.subMesh = subMesh;
            IntPtr vertexDataPtr = SubMesh_VertexData(subMesh);
            if(vertexDataPtr != IntPtr.Zero)
            {
                vertex = new VertexData(vertexDataPtr);
            }
            IntPtr indexDataPtr = SubMesh_IndexData(subMesh);
            if(indexDataPtr != IntPtr.Zero)
            {
                index = new IndexData(indexDataPtr);
            }
        }

        public void Dispose()
        {
            subMesh = IntPtr.Zero;
            if(vertex != null)
            {
                vertex.Dispose();
            }
            if(index != null)
            {
                index.Dispose();
            }
        }

        public String getMaterialName()
        {
            return Marshal.PtrToStringAnsi(SubMesh_getMaterialName(subMesh));
        }

        public void setMaterialName(String name)
        {
            SubMesh_setMaterialName(subMesh, name);
        }

        public bool UseSharedVertices
        {
            get
            {
                return SubMesh_UseSharedVertices(subMesh);
            }
        }

        public VertexData vertexData
        {
            get
            {
                return vertex;
            }
        }

        public IndexData indexData
        {
            get
            {
                return index;
            }
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool SubMesh_UseSharedVertices(IntPtr subMesh);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr SubMesh_VertexData(IntPtr subMesh);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr SubMesh_IndexData(IntPtr subMesh);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SubMesh_getMaterialName(IntPtr subMesh);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SubMesh_setMaterialName(IntPtr subMesh, String name);

#endregion
    }
}
