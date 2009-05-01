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

            ConfigSection cameras = AnomalyConfig.ConfigFile.createOrRetrieveConfigSection(AnomalyConfig.CAMERA_HEADER);
            upperLeft.initialize("UpperLeft", eventManager, renderer,
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_LOOKAT_DEFAULT));
            upperLeft.Dock = DockStyle.Fill;

            upperRight.initialize("UpperRight", eventManager, renderer,
                cameras.getValue(AnomalyConfig.BACK_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.BACK_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.BACK_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.BACK_CAMERA_LOOKAT_DEFAULT));
            upperRight.Dock = DockStyle.Fill;

            lowerLeft.initialize("BottomLeft", eventManager, renderer,
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_LOOKAT_DEFAULT));
            lowerLeft.Dock = DockStyle.Fill;

            lowerRight.initialize("BottomRight", eventManager, renderer,
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_LOOKAT_DEFAULT));
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
