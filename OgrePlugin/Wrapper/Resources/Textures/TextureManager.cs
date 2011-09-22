using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class TextureManager : IDisposable
    {
        static TextureManager instance = new TextureManager();

        public static TextureManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Texture> textureCollection = new SharedPtrCollection<Texture>(Texture.createWrapper, TexturePtr_createHeapPtr, TexturePtr_Delete);

        public void Dispose()
        {
            textureCollection.Dispose();
        }

        public TexturePtr createManual(String name, String group, TextureType texType, uint width, uint height, uint depth, int num_mips, PixelFormat format, TextureUsage usage, bool hwGammaCorrection, uint fsaa)
        {
            return getObject(TextureManager_createManual(name, group, texType, width, height, depth, num_mips, format, usage, hwGammaCorrection, fsaa, ProcessWrapperObjectCallback));
        }

        public void remove(String name)
        {
            TextureManager_removeName(name);
        }

        public void remove(TexturePtr resource)
        {
            TextureManager_removeResource(resource.HeapSharedPtr);
        }

        public TexturePtr getByName(String name)
        {
            return getObject(TextureManager_getByName1(name, ProcessWrapperObjectCallback));
        }

        public TexturePtr getByName(String name, String groupName)
        {
            return getObject(TextureManager_getByName2(name, groupName, ProcessWrapperObjectCallback));
        }

        internal TexturePtr getObject(IntPtr nativeTexture)
        {
            return new TexturePtr(textureCollection.getObject(nativeTexture));
        }

        internal ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return textureCollection.ProcessWrapperCallback;
            }
        }

        #region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_createManual(String name, String group, TextureType texType, uint width, uint height, uint depth, int num_mips, PixelFormat format, TextureUsage usage, bool hwGammaCorrection, uint fsaa, ProcessWrapperObjectDelegate processWrapper);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_getByName1(String name, ProcessWrapperObjectDelegate processWrapper);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_getByName2(String name, String group, ProcessWrapperObjectDelegate processWrapper);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextureManager_removeName(String name);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextureManager_removeResource(IntPtr heapSharedPtr);

        //TexturePtr
        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TexturePtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TexturePtr_Delete(IntPtr heapSharedPtr);

        #endregion
    }
}
