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
        private SplitViewHost camera1Host = new SplitViewHost();
        private SplitViewHost camera2Host = new SplitViewHost();
        private SplitViewHost camera3Host = new SplitViewHost();
        private SplitViewHost camera4Host = new SplitViewHost();
        private AnomalyController controller;

        public SplitViewController()
        {

        }

        public void initialize(EventManager eventManager, RendererPlugin renderer, AnomalyController controller)
        {
            this.controller = controller;

            CameraSection cameras = AnomalyConfig.CameraSection;
            controller.showDockContent(camera1Host);
            camera1Host.DrawingWindow.initialize("UpperLeft", eventManager, renderer, cameras.FrontCameraPosition, cameras.FrontCameraLookAt, this);

            controller.showDockContent(camera2Host);
            camera2Host.DrawingWindow.initialize("UpperRight", eventManager, renderer, cameras.BackCameraPosition, cameras.BackCameraLookAt, this);

            controller.showDockContent(camera3Host);
            camera3Host.DrawingWindow.initialize("BottomLeft", eventManager, renderer, cameras.RightCameraPosition, cameras.RightCameraLookAt, this);

            controller.showDockContent(camera4Host);
            camera4Host.DrawingWindow.initialize("BottomRight", eventManager, renderer, cameras.LeftCameraPosition, cameras.LeftCameraLookAt, this);
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

        public void destroyCameras(UpdateTimer mainTimer)
        {
            camera1Host.DrawingWindow.destroyCamera(mainTimer);
            camera2Host.DrawingWindow.destroyCamera(mainTimer);
            camera3Host.DrawingWindow.destroyCamera(mainTimer);
            camera4Host.DrawingWindow.destroyCamera(mainTimer);
        }

        public void createCameras(UpdateTimer mainTimer, SimScene scene)
        {
            camera1Host.DrawingWindow.createCamera(mainTimer, scene);
            camera2Host.DrawingWindow.createCamera(mainTimer, scene);
            camera3Host.DrawingWindow.createCamera(mainTimer, scene);
            camera4Host.DrawingWindow.createCamera(mainTimer, scene);
        }

        public void showStats(bool show)
        {
            camera1Host.DrawingWindow.showStats(show);
            camera2Host.DrawingWindow.showStats(show);
            camera3Host.DrawingWindow.showStats(show);
            camera4Host.DrawingWindow.showStats(show);
        }
    }
}
