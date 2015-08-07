using Engine;
using Engine.ObjectManagement;
using FreeImageAPI;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class FeedbackBuffer : IDisposable
    {
        public const String Scheme = "FeedbackBuffer";

        private FreeImageBitmap fullBitmap;
        private PixelBox fullBitmapBox;
        private TexturePtr texture;
        private RenderTexture renderTexture;
        private HardwarePixelBufferSharedPtr pixelBuffer;
        private VirtualTextureManager virtualTextureManager;
        private int id;
        private uint visibilityMask;

        //Per scene
        private Viewport vp;
        private Camera camera;
        private SceneNode node;
        private FeedbackCameraPositioner cameraPositioner;

        public FeedbackBuffer(VirtualTextureManager virtualTextureManager, IntSize2 renderSize, int id, uint visibilityMask)
        {
            this.id = id;
            this.virtualTextureManager = virtualTextureManager;
            this.visibilityMask = visibilityMask;

            texture = TextureManager.getInstance().createManual(TextureName, VirtualTextureManager.ResourceGroup, TextureType.TEX_TYPE_2D, (uint)renderSize.Width, (uint)renderSize.Height, 1, 0, OgrePlugin.PixelFormat.PF_A8R8G8B8, TextureUsage.TU_RENDERTARGET, null, false, 0);

            fullBitmap = new FreeImageBitmap((int)texture.Value.Width, (int)texture.Value.Height, FreeImageAPI.PixelFormat.Format32bppRgb);
            fullBitmapBox = fullBitmap.createPixelBox(OgrePlugin.PixelFormat.PF_A8R8G8B8);

            pixelBuffer = texture.Value.getBuffer();
            pixelBuffer.Value.OptimizeReadback = true;
            renderTexture = pixelBuffer.Value.getRenderTarget();
            renderTexture.setAutoUpdated(false);
        }

        public void Dispose()
        {
            renderTexture.Dispose();
            renderTexture = null;
            pixelBuffer.Dispose();
            TextureManager.getInstance().remove(texture);
            texture.Dispose();
            fullBitmapBox.Dispose();
            fullBitmap.Dispose();
        }

        public void update()
        {
            cameraPositioner.preRender();
            node.setPosition(cameraPositioner.Translation);
            camera.lookAt(cameraPositioner.LookAt);
            renderTexture.update();
        }

        public void blitToStaging()
        {
            pixelBuffer.Value.blitToStaging();
        }

        public void blitStagingToMemory()
        {
            pixelBuffer.Value.blitStagingToMemory(fullBitmapBox);
        }

        public unsafe void analyzeBuffer()
        {
            float u, v;
            byte m, t;
            for (int slId = 0; slId < fullBitmap.Height; ++slId)
            {
                var scanline = fullBitmap.GetScanline<RGBQUAD>(slId);
                RGBQUAD[] slData = scanline.Data;
                for (int pxId = 0; pxId < scanline.Count; ++pxId)
                {
                    var px = slData[pxId];
                    t = px.rgbReserved; //There is a change of this changing to the wrong texture id, but who knows if would happen (precision issues)

                    IndirectionTexture indirectionTexture;
                    if(t != 255 && virtualTextureManager.getIndirectionTexture(t, out indirectionTexture))
                    {
                        //Here the uvs are crushed to 8 bit, but should be ok since we are just detecting pages 
                        //with this number this allows 255 pages to be decenly processsed above that data might be lost.
                        u = px.rgbRed / 255.0f;
                        v = px.rgbGreen / 255.0f;
                        m = px.rgbBlue;
                        indirectionTexture.processPage(u, v, m);
                    }
                }
            }
        }

        public String TextureName
        {
            get
            {
                return "FeedbackBuffer" + id;
            }
        }

        public uint VisibilityMask
        {
            get
            {
                return visibilityMask;
            }
            set
            {
                visibilityMask = value;
                if (cameraPositioner != null)
                {
                    camera.setVisibilityFlags(visibilityMask);
                }
            }
        }

        internal void destroyCamera(SimScene scene)
        {
            if (cameraPositioner != null)
            {
                SimSubScene defaultScene = scene.getDefaultSubScene();
                OgreSceneManager sceneManager = defaultScene.getSimElementManager<OgreSceneManager>();

                renderTexture.destroyViewport(vp);
                vp = null;

                node.detachObject(camera);
                sceneManager.SceneManager.destroyCamera(camera);
                sceneManager.SceneManager.destroySceneNode(node);
                cameraPositioner = null;
            }
        }

        internal void createCamera(SimScene scene, FeedbackCameraPositioner cameraPositioner)
        {
            this.cameraPositioner = cameraPositioner;
            SimSubScene defaultScene = scene.getDefaultSubScene();
            OgreSceneManager sceneManager = defaultScene.getSimElementManager<OgreSceneManager>();

            camera = sceneManager.SceneManager.createCamera("VirtualTexturing.FeedbackBufferCamera" + id);
            camera.setNearClipDistance(1.0f);
            camera.setAutoAspectRatio(true);
            camera.setFOVy(new Degree(10.0f));
            node = sceneManager.SceneManager.createSceneNode("VirtualTexturing.FeedbackBufferCameraNode" + id);
            node.attachObject(camera);

            vp = renderTexture.addViewport(camera);
            vp.setMaterialScheme(Scheme);
            vp.setVisibilityMask(visibilityMask);
            vp.setBackgroundColor(new Engine.Color(0.0f, 0.0f, 0.0f, 1.0f));
            vp.clear();
        }
    }
}
