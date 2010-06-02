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

        [DllImport("OgreCWrapper")]
        private static extern IntPtr TextureManager_createManual(String name, String group, TextureType texType, uint width, uint height, uint depth, int num_mips, PixelFormat format, TextureUsage usage, bool hwGammaCorrection, uint fsaa, ProcessWrapperObjectDelegate processWrapper);

        //TexturePtr
        [DllImport("OgreCWrapper")]
        private static extern IntPtr TexturePtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport("OgreCWrapper")]
        private static extern void TexturePtr_Delete(IntPtr heapSharedPtr);

        #endregion
    }
}
