using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.ObjectManagement;
using System.Windows.Forms;
using Engine.Platform;
using Engine.Renderer;

namespace Anomaly
{
    class SplitViewController
    {
        private AnomalyController controller;
        private Dictionary<String, SplitViewHost> cameras = new Dictionary<string, SplitViewHost>();

        public SplitViewController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;

            CameraSection cameraSection = AnomalyConfig.CameraSection;
            addCamera("Camera 1",cameraSection.FrontCameraPosition, cameraSection.FrontCameraLookAt);
            addCamera("Camera 2",cameraSection.BackCameraPosition, cameraSection.BackCameraLookAt);
            addCamera("Camera 3",cameraSection.RightCameraPosition, cameraSection.RightCameraLookAt);
            addCamera("Camera 4",cameraSection.LeftCameraPosition, cameraSection.LeftCameraLookAt);
        }

        private void addCamera(String name, Vector3 translation, Vector3 lookAt)
        {
            SplitViewHost cameraHost = new SplitViewHost(name);
            controller.showDockContent(cameraHost);
            cameraHost.DrawingWindow.initialize(name, controller.EventManager, controller.PluginManager.RendererPlugin, translation, lookAt, this);
            cameras.Add(cameraHost.Text, cameraHost);
        }

        public void createFourWaySplit()
        {
            //changeSplit(new FourWaySplit());
        }

        public void createThreeWayUpperSplit()
        {
            //changeSplit(new ThreeWayUpperSplit());
        }

        public void createTwoWaySplit()
        {
            //changeSplit(new TwoWaySplit());
        }

        public void createOneWaySplit()
        {
            //changeSplit(new OneWaySplit());
        }

        public void destroyCameras()
        {
            foreach (SplitViewHost host in cameras.Values)
            {
                host.DrawingWindow.destroyCamera();
            }
        }

        public void createCameras(UpdateTimer mainTimer, SimScene scene)
        {
            foreach (SplitViewHost host in cameras.Values)
            {
                host.DrawingWindow.createCamera(mainTimer, scene);
            }
        }

        public void showStats(bool show)
        {
            foreach (SplitViewHost host in cameras.Values)
            {
                host.DrawingWindow.showStats(show);
            }
        }
    }
}
