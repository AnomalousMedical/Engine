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
        DrawingWindow frontView = new DrawingWindow();
        DrawingWindow backView = new DrawingWindow();
        DrawingWindow leftView = new DrawingWindow();
        DrawingWindow rightView = new DrawingWindow();
        private SplitViewHost splitViewHost = new SplitViewHost();
        private SplitViewHost camera1Host = new SplitViewHost();
        private SplitViewHost camera2Host = new SplitViewHost();
        private SplitViewHost camera3Host = new SplitViewHost();
        private SplitViewHost camera4Host = new SplitViewHost();
        SplitView currentView;
        DrawingWindow activeWindow = null;
        bool maximized = false;
        private AnomalyController controller;

        public SplitViewController()
        {

        }

        public void initialize(EventManager eventManager, RendererPlugin renderer, AnomalyController controller)
        {
            this.controller = controller;
            //controller.showDockContent(splitViewHost);

            CameraSection cameras = AnomalyConfig.CameraSection;
            controller.showDockContent(camera1Host);
            camera1Host.Controls.Add(frontView);
            frontView.initialize("UpperLeft", eventManager, renderer, cameras.FrontCameraPosition, cameras.FrontCameraLookAt, this);
            frontView.Dock = DockStyle.Fill;

            controller.showDockContent(camera2Host);
            camera2Host.Controls.Add(backView);
            backView.initialize("UpperRight", eventManager, renderer, cameras.BackCameraPosition, cameras.BackCameraLookAt, this);
            backView.Dock = DockStyle.Fill;

            controller.showDockContent(camera3Host);
            camera3Host.Controls.Add(leftView);
            leftView.initialize("BottomLeft", eventManager, renderer, cameras.RightCameraPosition, cameras.RightCameraLookAt, this);
            leftView.Dock = DockStyle.Fill;

            controller.showDockContent(camera4Host);
            camera4Host.Controls.Add(rightView);
            rightView.initialize("BottomRight", eventManager, renderer, cameras.LeftCameraPosition, cameras.LeftCameraLookAt, this);
            rightView.Dock = DockStyle.Fill;
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
            frontView.destroyCamera(mainTimer);
            backView.destroyCamera(mainTimer);
            leftView.destroyCamera(mainTimer);
            rightView.destroyCamera(mainTimer);
        }

        public void createCameras(UpdateTimer mainTimer, SimScene scene)
        {
            frontView.createCamera(mainTimer, scene);
            backView.createCamera(mainTimer, scene);
            leftView.createCamera(mainTimer, scene);
            rightView.createCamera(mainTimer, scene);
        }

        public void showStats(bool show)
        {
            frontView.showStats(show);
            backView.showStats(show);
            leftView.showStats(show);
            rightView.showStats(show);
        }
    }
}
