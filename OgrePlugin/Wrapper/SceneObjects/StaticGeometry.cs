using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgreWrapper
{
    public class StaticGeometry : IDisposable
    {
        internal static StaticGeometry createWrapper(IntPtr staticGeometry, object[] args)
        {
            return new StaticGeometry(staticGeometry);
        }

        private IntPtr staticGeometry;

        internal StaticGeometry(IntPtr staticGeometry)
        {
            this.staticGeometry = staticGeometry;
        }

        public void Dispose()
        {
            staticGeometry = IntPtr.Zero;
        }

        public void addEntity(Entity ent, Vector3 position)
        {
            StaticGeometry_addEntity(staticGeometry, ent.OgreObject, position, Quaternion.Identity, Vector3.ScaleIdentity);
        }

        public void addEntity(Entity ent, Vector3 position, Quaternion rotation)
        {
            StaticGeometry_addEntity(staticGeometry, ent.OgreObject, position, rotation, Vector3.ScaleIdentity);
        }

        public void addEntity(Entity ent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            StaticGeometry_addEntity(staticGeometry, ent.OgreObject, position, rotation, scale);
        }

        public void addSceneNode(SceneNode node)
        {
            StaticGeometry_addSceneNode(staticGeometry, node.OgreNode);
        }

        public void build()
        {
            StaticGeometry_build(staticGeometry);
        }

        public void destroy()
        {
            StaticGeometry_destroy(staticGeometry);
        }

        public void reset()
        {
            StaticGeometry_reset(staticGeometry);
        }

        public void dump(String filename)
        {
            StaticGeometry_dump(staticGeometry, filename);
        }

        public Vector3 Origin
        {
            get
            {
                return StaticGeometry_getOrigin(staticGeometry);
            }
            set
            {
                StaticGeometry_setOrigin(staticGeometry, value);
            }
        }

        public Vector3 RegionDimensions
        {
            get
            {
                return StaticGeometry_getRegionDimensions(staticGeometry);
            }
            set
            {
                StaticGeometry_setRegionDimensions(staticGeometry, value);
            }
        }

        public bool Visible
        {
            get
            {
                return StaticGeometry_isVisible(staticGeometry);
            }
            set
            {
                StaticGeometry_setVisible(staticGeometry, value);
            }
        }

        public IntPtr Ptr
        {
            get
            {
                return staticGeometry;
            }
        }

        #region NativeWrapper

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void StaticGeometry_addEntity(IntPtr staticGeometry, IntPtr ent, Vector3 position, Quaternion orientation, Vector3 scale);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_addSceneNode(IntPtr staticGeometry, IntPtr node);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_build(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_destroy(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_dump(IntPtr staticGeometry, String filename);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 StaticGeometry_getOrigin(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 StaticGeometry_getRegionDimensions(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool StaticGeometry_isVisible(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_reset(IntPtr staticGeometry);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_setOrigin(IntPtr staticGeometry, Vector3 origin);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_setRegionDimensions(IntPtr staticGeometry, Vector3 size);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticGeometry_setVisible(IntPtr staticGeometry, bool visible);

        #endregion
    }
}
