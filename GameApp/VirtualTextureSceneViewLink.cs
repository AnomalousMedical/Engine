using Anomalous.GuiFramework.Cameras;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using OgrePlugin;
using OgrePlugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GameApp
{
    public class VirtualTextureSceneViewLink : IDisposable
    {
        private VirtualTextureManager virtualTextureManager;
        private UnifiedMaterialBuilder materialBuilder;
        private CameraLink cameraLink;
        private SceneController sceneController;
        private OnUpdateListener updateListener = new OnUpdateListener();
        private UpdateTimer timer;

        public VirtualTextureSceneViewLink(SceneController sceneController, SceneViewController sceneViewController, PluginManager pluginManager, UpdateTimer timer)
        {
            this.timer = timer;
            timer.addUpdateListener(updateListener);

            this.sceneController = sceneController;

            sceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading += SceneController_OnSceneUnloading;

            cameraLink = new CameraLink(sceneViewController);

            CompressedTextureSupport textureFormat = OgreInterface.Instance.SelectedTextureFormat;

            virtualTextureManager = new VirtualTextureManager(UnifiedMaterialBuilder.GetNumCompressedTexturesNeeded(textureFormat), new IntSize2(4096, 4096), 128, 4096, textureFormat, 10, new IntSize2(256, 128), 100 * 1024 * 1024, UnifiedMaterialBuilder.AreTexturesPagedOnDisk(textureFormat));

            materialBuilder = new UnifiedMaterialBuilder(virtualTextureManager, OgreInterface.Instance.SelectedTextureFormat, pluginManager.createLiveResourceManager("UnifiedShaders"));
            OgreInterface.Instance.MaterialParser.addMaterialBuilder(materialBuilder);
        }

        public void Dispose()
        {
            this.updateListener.OnUpdate -= controller_OnUpdate;
            timer.removeUpdateListener(updateListener);
            OgreInterface.Instance.MaterialParser.removeMaterialBuilder(materialBuilder);
            sceneController.OnSceneLoaded -= SceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading -= SceneController_OnSceneUnloading;
            IDisposableUtil.DisposeIfNotNull(virtualTextureManager);
            materialBuilder.Dispose();
        }

        void controller_OnUpdate(Engine.Platform.Clock obj)
        {
            virtualTextureManager.update();
        }

        public VirtualTextureManager VirtualTextureManager
        {
            get
            {
                return virtualTextureManager;
            }
        }

        void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            this.updateListener.OnUpdate -= controller_OnUpdate;
            virtualTextureManager.destroyFeedbackBufferCamera(scene);
        }

        void SceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            virtualTextureManager.createFeedbackBufferCamera(scene, cameraLink);
            this.updateListener.OnUpdate += controller_OnUpdate;
        }

        class CameraLink : FeedbackCameraPositioner
        {
            SceneViewController sceneViewController;

            public CameraLink(SceneViewController sceneViewController)
            {
                this.sceneViewController = sceneViewController;
            }

            public Vector3 Translation
            {
                get
                {
                    return sceneViewController.ActiveWindow.Translation;
                }
            }

            public Vector3 LookAt
            {
                get
                {
                    return sceneViewController.ActiveWindow.LookAt;
                }
            }

            public void preRender()
            {
                //TransparencyController.applyTransparencyState(sceneViewController.ActiveWindow.CurrentTransparencyState);
            }
        }

        internal void clearCache()
        {
            virtualTextureManager.reset();
        }
    }
}
