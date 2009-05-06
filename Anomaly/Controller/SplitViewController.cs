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
        DrawingWindow upperLeft = new DrawingWindow();
        DrawingWindow upperRight = new DrawingWindow();
        DrawingWindow lowerLeft = new DrawingWindow();
        DrawingWindow lowerRight = new DrawingWindow();
        Control splitControl;
        SplitView currentView;

        public SplitViewController()
        {

        }

        public void initialize(EventManager eventManager, RendererPlugin renderer, Control splitControl)
        {
            this.splitControl = splitControl;

            CameraSection cameras = AnomalyConfig.CameraSection;
            upperLeft.initialize("UpperLeft", eventManager, renderer, cameras.FrontCameraPosition, cameras.FrontCameraLookAt);
            upperLeft.Dock = DockStyle.Fill;

            upperRight.initialize("UpperRight", eventManager, renderer, cameras.BackCameraPosition, cameras.BackCameraLookAt);
            upperRight.Dock = DockStyle.Fill;

            lowerLeft.initialize("BottomLeft", eventManager, renderer, cameras.RightCameraPosition, cameras.RightCameraLookAt);
            lowerLeft.Dock = DockStyle.Fill;

            lowerRight.initialize("BottomRight", eventManager, renderer, cameras.LeftCameraPosition, cameras.LeftCameraLookAt);
            lowerRight.Dock = DockStyle.Fill;
        }

        public void createFourWaySplit()
        {
            FourWaySplit fourWay = new FourWaySplit();
            fourWay.Dock = DockStyle.Fill;
            currentView = fourWay;
            splitControl.Controls.Clear();
            splitControl.Controls.Add(fourWay);
            currentView.UpperLeft.Controls.Add(upperLeft);
            upperLeft.setEnabled(true);
            currentView.UpperRight.Controls.Add(upperRight);
            upperRight.setEnabled(true);
            currentView.LowerLeft.Controls.Add(lowerLeft);
            lowerLeft.setEnabled(true);
            currentView.LowerRight.Controls.Add(lowerRight);
            lowerRight.setEnabled(true);
        }

        public void destroyCameras(UpdateTimer mainTimer)
        {
            upperLeft.destroyCamera(mainTimer);
            upperRight.destroyCamera(mainTimer);
            lowerLeft.destroyCamera(mainTimer);
            lowerRight.destroyCamera(mainTimer);
        }

        public void createCameras(UpdateTimer mainTimer, SimScene scene)
        {
            upperLeft.createCamera(mainTimer, scene);
            upperRight.createCamera(mainTimer, scene);
            lowerLeft.createCamera(mainTimer, scene);
            lowerRight.createCamera(mainTimer, scene);
        }
    }
}
