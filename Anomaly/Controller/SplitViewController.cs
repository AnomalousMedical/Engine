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
        Control splitControl;
        SplitView currentView;

        public SplitViewController()
        {

        }

        public void initialize(EventManager eventManager, RendererPlugin renderer, Control splitControl)
        {
            this.splitControl = splitControl;

            CameraSection cameras = AnomalyConfig.CameraSection;
            frontView.initialize("UpperLeft", eventManager, renderer, cameras.FrontCameraPosition, cameras.FrontCameraLookAt);
            frontView.Dock = DockStyle.Fill;

            backView.initialize("UpperRight", eventManager, renderer, cameras.BackCameraPosition, cameras.BackCameraLookAt);
            backView.Dock = DockStyle.Fill;

            leftView.initialize("BottomLeft", eventManager, renderer, cameras.RightCameraPosition, cameras.RightCameraLookAt);
            leftView.Dock = DockStyle.Fill;

            rightView.initialize("BottomRight", eventManager, renderer, cameras.LeftCameraPosition, cameras.LeftCameraLookAt);
            rightView.Dock = DockStyle.Fill;
        }

        public void createFourWaySplit()
        {
            FourWaySplit fourWay = new FourWaySplit();
            fourWay.Dock = DockStyle.Fill;
            currentView = fourWay;
            splitControl.Controls.Clear();
            splitControl.Controls.Add(fourWay);
            configureWindows();
        }

        public void createThreeWayUpperSplit()
        {
            ThreeWayUpperSplit threeWay = new ThreeWayUpperSplit();
            threeWay.Dock = DockStyle.Fill;
            currentView = threeWay;
            splitControl.Controls.Clear();
            splitControl.Controls.Add(threeWay);
            configureWindows();
        }

        public void createTwoWaySplit()
        {
            TwoWaySplit twoWay = new TwoWaySplit();
            twoWay.Dock = DockStyle.Fill;
            currentView = twoWay;
            splitControl.Controls.Clear();
            splitControl.Controls.Add(twoWay);
            configureWindows();
        }

        public void createOneWaySplit()
        {
            OneWaySplit oneWay = new OneWaySplit();
            oneWay.Dock = DockStyle.Fill;
            currentView = oneWay;
            splitControl.Controls.Clear();
            splitControl.Controls.Add(oneWay);
            configureWindows();
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

        /// <summary>
        /// </summary>
        private void configureWindows()
        {
            if (currentView.FrontView != null)
            {
                currentView.FrontView.Controls.Add(frontView);
                frontView.setEnabled(true);
            }
            else
            {
                frontView.setEnabled(false);
            }
            if (currentView.BackView != null)
            {
                currentView.BackView.Controls.Add(backView);
                backView.setEnabled(true);
            }
            else
            {
                backView.setEnabled(false);
            }
            if (currentView.LeftView != null)
            {
                currentView.LeftView.Controls.Add(leftView);
                leftView.setEnabled(true);
            }
            else
            {
                leftView.setEnabled(false);
            }
            if (currentView.RightView != null)
            {
                currentView.RightView.Controls.Add(rightView);
                rightView.setEnabled(true);
            }
            else
            {
                rightView.setEnabled(false);
            }
        }
    }
}
