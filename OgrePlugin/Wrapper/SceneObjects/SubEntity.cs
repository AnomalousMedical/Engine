using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class SubEntity : IDisposable
    {
        private IntPtr subEntity;

        internal static SubEntity createWrapper(IntPtr nativeObject, object[] args)
        {
            return new SubEntity(nativeObject);
        }

        private SubEntity(IntPtr subEntity)
        {
            this.subEntity = subEntity;
        }

        public void Dispose()
        {
            subEntity = IntPtr.Zero;
        }

        public String getMaterialName()
        {
            return Marshal.PtrToStringAnsi(SubEntity_getMaterialName(subEntity));
        }

        public void setMaterialName(String name)
        {
            SubEntity_setMaterialName(subEntity, name);
        }

        public void setVisible(bool visible)
        {
            SubEntity_setVisible(subEntity, visible);
        }

        public bool isVisible()
        {
            return SubEntity_isVisible(subEntity);
        }

        public MaterialPtr getMaterial()
        {
            //SubEntity_(subEntity);
            throw new NotImplementedException();
        }

        public void setCustomParameter(int index, Quaternion value)
        {
            SubEntity_setCustomParameter(subEntity, new IntPtr(index), value);
        }

        public Quaternion getCustomParameter(int index)
        {
            return SubEntity_getCustomParameter(subEntity, new IntPtr(index));
        }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SubEntity_getMaterialName(IntPtr subEntity);

        [DllImport("OgreCWrapper")]
        private static extern void SubEntity_setMaterialName(IntPtr subEntity, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SubEntity_setVisible(IntPtr subEntity, bool visible);

        [DllImport("OgreCWrapper")]
        private static extern bool SubEntity_isVisible(IntPtr subEntity);

        [DllImport("OgreCWrapper")]
        //Warning this will return a pointer to a newly allocated heap Ogre::MaterialPtr this must be deleted or it will leak.
        private static extern IntPtr SubEntity_getMaterial(IntPtr subEntity);

        [DllImport("OgreCWrapper")]
        private static extern void SubEntity_setCustomParameter(IntPtr subEntity, IntPtr index, Quaternion value);

        [DllImport("OgreCWrapper")]
        private static extern Quaternion SubEntity_getCustomParameter(IntPtr subEntity, IntPtr index);

        #endregion
    }
}
