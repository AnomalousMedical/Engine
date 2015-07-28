using Anomalous.GuiFramework.Cameras;
using Engine;
using Engine.ObjectManagement;
using OgrePlugin;
using OgrePlugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly
{
    class VirtualTextureSceneViewLink : IDisposable
    {
        private VirtualTextureManager virtualTextureManager;
        private AnomalyController controller;
        private UnifiedMaterialBuilder materialBuilder;
        private CameraLink cameraLink;

        public VirtualTextureSceneViewLink(AnomalyController controller)
        {
            this.controller = controller;
            controller.SceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
            controller.SceneController.OnSceneUnloading += SceneController_OnSceneUnloading;

            cameraLink = new CameraLink(controller.SceneViewController);

            CompressedTextureSupport textureFormat = OgreInterface.Instance.SelectedTextureFormat;
            int padding;
            switch (textureFormat)
            {
                case CompressedTextureSupport.DXT:
                    padding = 4;
                    break;
                default:
                    padding = 1;
                    break;
            }

            virtualTextureManager = new VirtualTextureManager(4, new IntSize2(4096, 4096), 128, textureFormat, padding, 10, new IntSize2(256, 128));

            materialBuilder = new UnifiedMaterialBuilder(virtualTextureManager, OgreInterface.Instance.SelectedTextureFormat, controller.PluginManager.createLiveResourceManager("UnifiedShaders"));
            OgreInterface.Instance.MaterialParser.addMaterialBuilder(materialBuilder);
            materialBuilder.InitializationComplete += materialBuilder_InitializationComplete;
        }

        public void Dispose()
        {
            OgreInterface.Instance.MaterialParser.removeMaterialBuilder(materialBuilder);
            controller.SceneController.OnSceneLoaded -= SceneController_OnSceneLoaded;
            controller.SceneController.OnSceneUnloading -= SceneController_OnSceneUnloading;
            controller.OnUpdate -= controller_OnUpdate;
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
            this.controller.OnUpdate -= controller_OnUpdate;
            virtualTextureManager.destroyFeedbackBufferCamera(scene);
        }

        void SceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            virtualTextureManager.createFeedbackBufferCamera(scene, cameraLink);
            this.controller.OnUpdate += controller_OnUpdate;
        }

        void materialBuilder_InitializationComplete(UnifiedMaterialBuilder obj)
        {
            if (obj.MaterialCount > 0)
            {
                materialBuilder.InitializationComplete -= materialBuilder_InitializationComplete;
                virtualTextureManager.update();
            }
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
