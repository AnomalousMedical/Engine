using Engine.Platform;
using Engine.Renderer;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Cameras
{
    public class TextureSceneView : SceneViewWindow
    {
        private TexturePtr texture;
        private HardwarePixelBufferSharedPtr pixelBuffer;
        private RenderTexture renderTexture;
        private RendererWindow rendererWindow;

        private OgrePlugin.PixelFormat ogreTextureFormat = OgrePlugin.PixelFormat.PF_A8R8G8B8;

        private bool renderOneFrame = false;
        private bool alwaysRender = true;
        private bool renderingEnabled = true;

        class ManualResourceLoader : ManagedManualResourceLoader
        {
            private TextureSceneView sceneView;

            public ManualResourceLoader(TextureSceneView sceneView)
            {
                this.sceneView = sceneView;
            }

            protected override void loadResource()
            {
                sceneView.RenderOneFrame = true;
            }
        }
        private ManualResourceLoader resourceLoader;

        public TextureSceneView(SceneViewController controller, CameraMover cameraMover, String name, BackgroundScene background, int zIndexStart, int width, int height)
            :base(controller, cameraMover, name, background, zIndexStart)
        {
            resourceLoader = new ManualResourceLoader(this);
            this.TextureName = name;
            texture = TextureManager.getInstance().createManual(name, MyGUIInterface.Instance.CommonResourceGroup.FullName, TextureType.TEX_TYPE_2D, (uint)width, (uint)height, 1, 0, ogreTextureFormat, TextureUsage.TU_RENDERTARGET, resourceLoader, false, 0);

            pixelBuffer = texture.Value.getBuffer();
            renderTexture = pixelBuffer.Value.getRenderTarget();
            this.RenderingEnded += TextureSceneView_RenderingEnded;

            rendererWindow = new ManualWindow(renderTexture);
            this.RendererWindow = rendererWindow;
            this.ClearEveryFrame = true;

            this.BackColor = new Engine.Color(0, 0, 0, 0);
        }

        public override void Dispose()
        {
            base.Dispose();

            resourceLoader.Dispose();
            if (pixelBuffer != null)
            {
                pixelBuffer.Dispose();
            }
            if (texture != null)
            {
                TextureManager.getInstance().remove(texture);
                texture.Dispose();
            }
        }

        public override void close()
        {

        }

        public bool RenderingEnabled
        {
            get
            {
                return renderingEnabled;
            }
            set
            {
                if(renderingEnabled != value)
                {
                    renderingEnabled = value;
                    determineRenderingActive();
                }
            }
        }

        public bool AlwaysRender
        {
            get
            {
                return alwaysRender;
            }
            set
            {
                if(alwaysRender != value)
                {
                    alwaysRender = value;
                    determineRenderingActive();
                }
            }
        }

        public bool RenderOneFrame
        {
            get
            {
                return renderOneFrame;
            }
            set
            {
                if(renderOneFrame != value)
                {
                    renderOneFrame = value;
                    determineRenderingActive();
                }
            }
        }

        public uint Width
        {
            get
            {
                return renderTexture.getWidth();
            }
        }

        public uint Height
        {
            get
            {
                return renderTexture.getHeight();
            }
        }

        public String TextureName { get; private set; }

        void determineRenderingActive()
        {
            renderTexture.setAutoUpdated(renderingEnabled && (alwaysRender || renderOneFrame));
        }

        void TextureSceneView_RenderingEnded(SceneViewWindow window, bool currentCameraRender)
        {
            RenderOneFrame = false;
        }
    }
}
