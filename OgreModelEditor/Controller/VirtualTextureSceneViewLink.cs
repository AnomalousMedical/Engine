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

namespace OgreModelEditor
{
    class VirtualTextureSceneViewLink : IDisposable
    {
        private VirtualTextureManager virtualTextureManager;
        private OgreModelEditorController controller;
        private UnifiedMaterialBuilder materialBuilder;
        private CameraLink cameraLink;

        public VirtualTextureSceneViewLink(OgreModelEditorController controller)
        {
            this.controller = controller;
            //standaloneController.SceneLoaded += standaloneController_SceneLoaded;
            //standaloneController.SceneUnloading += standaloneController_SceneUnloading;

            cameraLink = new CameraLink(controller.SceneViewController);

            CompressedTextureSupport textureFormat = OgreInterface.Instance.SelectedTextureFormat;
            int padding = VirtualTextureManager.SuggestPadding(textureFormat);

            virtualTextureManager = new VirtualTextureManager(4, new IntSize2(4096, 4096), 128, textureFormat, padding, 10, new IntSize2(256, 128));

            materialBuilder = new UnifiedMaterialBuilder(virtualTextureManager, OgreInterface.Instance.SelectedTextureFormat, controller.PluginManager.createLiveResourceManager("UnifiedShaders"));
            OgreInterface.Instance.MaterialParser.addMaterialBuilder(materialBuilder);
            materialBuilder.InitializationComplete += materialBuilder_InitializationComplete;
        }

        void materialBuilder_InitializationComplete(UnifiedMaterialBuilder obj)
        {
            if (obj.MaterialCount > 0)
            {
                materialBuilder.InitializationComplete -= materialBuilder_InitializationComplete;
                virtualTextureManager.update();
            }
        }

        public void Dispose()
        {
            OgreInterface.Instance.MaterialParser.removeMaterialBuilder(materialBuilder);
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

        public void sceneUnloading(SimScene scene)
        {
            controller.OnUpdate -= controller_OnUpdate;
            virtualTextureManager.destroyFeedbackBufferCamera(scene);
        }

        public void sceneLoaded(SimScene scene)
        {
            virtualTextureManager.createFeedbackBufferCamera(scene, cameraLink);
            controller.OnUpdate += controller_OnUpdate;
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
