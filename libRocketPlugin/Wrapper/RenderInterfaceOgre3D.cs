using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgrePlugin;
using Engine.Utility;
using Engine.Threads;

namespace libRocketPlugin
{
    public class RenderInterfaceOgre3D : RenderInterface
    {
        private CommonResourcesArchiveFactory commonResourcesArchiveFactory = null;
        private CallbackHandler callbackHandler;

        public RenderInterfaceOgre3D(int width, int height)
        {
            commonResourcesArchiveFactory = new CommonResourcesArchiveFactory();
            Root.getSingleton().addArchiveFactory(commonResourcesArchiveFactory);
            OgreInterface.Instance.Disposed += Ogre_Disposed;

            callbackHandler = new CallbackHandler(this);
            renderInterface = callbackHandler.create(width, height, this);
        }

        public override void Dispose()
        {
            RenderInterfaceOgre3D_Delete(renderInterface);
            callbackHandler.Dispose();
            //Note that some stuff is disposed in the Ogre_Disposed function below
        }

        void Ogre_Disposed(OgreInterface obj)
        {
            //Have to do this after ogre is disposed
            if (commonResourcesArchiveFactory != null)
            {
                commonResourcesArchiveFactory.Dispose();
                commonResourcesArchiveFactory = null;
            }
        }

        public void ConfigureRenderSystem(int windowWidth, int windowHeight, bool requiresTextureFlipping)
        {
            RenderInterfaceOgre3D_ConfigureRenderSystem(renderInterface, ref windowWidth, ref windowHeight, ref requiresTextureFlipping);
        }

        public float PixelsPerInch
        {
            get
            {
                return RenderInterfaceOgre3D_GetPixelsPerInch(renderInterface);
            }
            set
            {
                RenderInterfaceOgre3D_SetPixelsPerInch(renderInterface, value);
            }
        }

        public float PixelScale
        {
            get
            {
                return RenderInterfaceOgre3D_GetPixelScale(renderInterface);
            }
            set
            {
                RenderInterfaceOgre3D_SetPixelScale(renderInterface, value);
            }
        }

        private bool queueBackgroundImageLoad(String source, IntPtr rocketTexture, ref Vector2i size)
        {
            try
            {
                //On main thread, load image info
                var streamPtr = OgreResourceGroupManager.getInstance().openResource(source, "Rocket.Common", true);
                int width, height;
                ImageUtility.ImageFormat format = ImageUtility.GetImageInfo(streamPtr.Value, out width, out height);
                size = new Vector2i(width, height);
                ThreadManager.RunInBackground(() =>
                {
                    //Process the image itself in a background thread
                    Image image = new Image();
                    try
                    {
                        image.load(streamPtr.Value, format.ToString().ToLowerInvariant());
                        ThreadManager.invoke(() =>
                        {
                            //Commit the texture to ogre and alert libRocket we are done loading, back in the main thread
                            TexturePtr texture = null;
                            try
                            {
                                texture = TextureManager.getInstance().loadImage(source, "Rocket.Common", image);
                            }
                            catch (OgreException)
                            {

                            }
                            finally
                            {
                                RenderInterfaceOgre3D_finishTextureLoad(rocketTexture, texture != null ? texture.HeapSharedPtr : IntPtr.Zero);
                                if (image != null)
                                {
                                    image.Dispose();
                                }
                                if (texture != null)
                                {
                                    texture.Dispose();
                                }
                                RocketInterface.Instance.fireTextureLoaded();
                            }
                        });
                    }
                    finally
                    {
                        if (streamPtr != null)
                        {
                            streamPtr.Dispose();
                        }
                    }
                });

                return true;
            }
            catch (OgreException ex)
            {
                return false;
            }
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height, QueueBackgroundImageLoad queueBackgroundImageLoad
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_ConfigureRenderSystem(IntPtr renderInterface, ref int renderWidth, ref int renderHeight, ref bool requiresTextureFlipping);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelsPerInch(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelsPerInch(IntPtr renderInterface, float ppi);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelScale(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelScale(IntPtr renderInterface, float scale);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_finishTextureLoad(IntPtr rocketTexture, IntPtr texturePtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool QueueBackgroundImageLoad(String source, IntPtr rocketTexture, ref Vector2i size
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        /// <summary>
        /// This does not dispose since we have the GCHandle always for this class
        /// </summary>
        class CallbackHandler : IDisposable
        {
            private static QueueBackgroundImageLoad queueBackgroundImageLoad;
            private GCHandle gcHandle;

            static CallbackHandler()
            {
                if (RocketInterface.LoadImagesInBackground)
                {
                    queueBackgroundImageLoad = new QueueBackgroundImageLoad(queueBackgroundImageLoadImpl);
                }
            }

            public CallbackHandler(RenderInterfaceOgre3D renderInterface)
            {
                gcHandle = GCHandle.Alloc(renderInterface);
            }

            public void Dispose()
            {
                gcHandle.Free();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(QueueBackgroundImageLoad))]
            private static bool queueBackgroundImageLoadImpl(String source, IntPtr rocketTexture, ref Vector2i size, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as RenderInterfaceOgre3D).queueBackgroundImageLoad(source, rocketTexture, ref size);
            }

            public IntPtr create(int width, int height, RenderInterfaceOgre3D obj)
            {
                return RenderInterfaceOgre3D_Create(width, height, queueBackgroundImageLoad, GCHandle.ToIntPtr(gcHandle));
            }
        }
#else
        /// <summary>
        /// This does not dispose since we have the GCHandle always for this class
        /// </summary>
        class CallbackHandler : IDisposable
        {
            private QueueBackgroundImageLoad queueBackgroundImageLoad;

            public CallbackHandler(RenderInterfaceOgre3D renderInterface)
            {

            }

            public IntPtr create(int width, int height, RenderInterfaceOgre3D obj)
            {
                if (RocketInterface.LoadImagesInBackground)
                {
                    queueBackgroundImageLoad = new QueueBackgroundImageLoad(obj.queueBackgroundImageLoad);
                }

                return RenderInterfaceOgre3D_Create(width, height, queueBackgroundImageLoad);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}
