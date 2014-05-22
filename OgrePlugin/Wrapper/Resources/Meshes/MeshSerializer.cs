using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class MeshSerializer : IDisposable
    {
        public enum Endian : uint
		{
			/// Use the platform native endian
			ENDIAN_NATIVE,
			/// Use big endian (0x1000 is serialized as 0x10 0x00)
			ENDIAN_BIG,
			/// Use little endian (0x1000 is serialized as 0x00 0x10)
			ENDIAN_LITTLE
		};

        private IntPtr meshSerializer;

        public MeshSerializer()
        {
            meshSerializer = MeshSerializer_Create();
        }

        public void Dispose()
        {
            MeshSerializer_Delete(meshSerializer);
        }

        public void exportMesh(Mesh mesh, String filename)
        {
            MeshSerializer_exportMesh(meshSerializer, mesh.OgreResource, filename);
        }

        public void exportMesh(Mesh mesh, String filename, Endian endianMode)
        {
            MeshSerializer_exportMeshEndian(meshSerializer, mesh.OgreResource, filename, endianMode);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MeshSerializer_Create();

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MeshSerializer_Delete(IntPtr meshSerializer);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MeshSerializer_exportMesh(IntPtr meshSerializer, IntPtr mesh, String filename);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MeshSerializer_exportMeshEndian(IntPtr meshSerializer, IntPtr mesh, String filename, Endian endianMode);

#endregion
    }
}
