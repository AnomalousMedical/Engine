using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class TextureManager : IDisposable
    {
        static TextureManager instance = new TextureManager();

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            instance.textureCollection.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        public static TextureManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Texture> textureCollection;

        private TextureManager()
        {
            PerformanceMonitor.addValueProvider("Ogre Texture Memory Usage", () => Prettify.GetSizeReadable(MemoryUsage));
            textureCollection = new SharedPtrCollection<Texture>(Texture.createWrapper, TexturePtr_createHeapPtr, TexturePtr_Delete
            #if FULL_AOT_COMPILE
            , processWrapperObject_AOT
            #endif
            );
        }

        public void Dispose()
        {
            PerformanceMonitor.removeValueProvider("Ogre Texture Memory Usage");
            textureCollection.Dispose();
        }

        public TexturePtr createManual(String name, String group, TextureType texType, uint width, uint height, uint depth, int num_mips, PixelFormat format, TextureUsage usage, ManagedManualResourceLoader resourceLoader, bool hwGammaCorrection, uint fsaa, String fsaaHint = "")
        {
            IntPtr resourceLoaderPtr = resourceLoader != null ? resourceLoader.Ptr : IntPtr.Zero;
            TexturePtr ptr = getObject(TextureManager_createManual(name, group, texType, width, height, depth, num_mips, format, usage, resourceLoaderPtr, hwGammaCorrection, fsaa, fsaaHint, ProcessWrapperObjectCallback));
            OgreExceptionManager.fireAnyException();
            return ptr;
        }

        public TexturePtr loadImage(String name, String group, Image img, TextureType texType = TextureType.TEX_TYPE_2D, int numMipmaps = -1, float gamma = 1.0f, bool isAlpha = false, PixelFormat desiredFormat = PixelFormat.PF_UNKNOWN, bool hwGamma = false)
        {
            TexturePtr ptr = getObject(TextureManager_loadImage(name, group, img.Ptr, texType, numMipmaps, gamma, isAlpha, desiredFormat, hwGamma, ProcessWrapperObjectCallback));
            OgreExceptionManager.fireAnyException();
            return ptr;
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

        public long MemoryUsage
        {
            get
            {
                return TextureManager_getMemoryUsage().ToInt64();
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_createManual(String name, String group, TextureType texType, uint width, uint height, uint depth, int num_mips, PixelFormat format, TextureUsage usage, IntPtr loader, bool hwGammaCorrection, uint fsaa, String fsaaHint, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_loadImage(String name, String group, IntPtr img, TextureType texType, int numMipmaps, float gamma, bool isAlpha, PixelFormat desiredFormat, bool hwGamma, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_getByName1(String name, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_getByName2(String name, String group, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextureManager_removeName(String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextureManager_removeResource(IntPtr heapSharedPtr);

        //TexturePtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TexturePtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void TexturePtr_Delete(IntPtr heapSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TextureManager_getMemoryUsage();

        #endregion
    }
}
