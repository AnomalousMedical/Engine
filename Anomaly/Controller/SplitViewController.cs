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
            controller.showDockContent(splitViewHost);

            CameraSection cameras = AnomalyConfig.CameraSection;
            frontView.initialize("UpperLeft", eventManager, renderer, cameras.FrontCameraPosition, cameras.FrontCameraLookAt, this);
            frontView.Dock = DockStyle.Fill;

            backView.initialize("UpperRight", eventManager, renderer, cameras.BackCameraPosition, cameras.BackCameraLookAt, this);
            backView.Dock = DockStyle.Fill;

            leftView.initialize("BottomLeft", eventManager, renderer, cameras.RightCameraPosition, cameras.RightCameraLookAt, this);
            leftView.Dock = DockStyle.Fill;

            rightView.initialize("BottomRight", eventManager, renderer, cameras.LeftCameraPosition, cameras.LeftCameraLookAt, this);
            rightView.Dock = DockStyle.Fill;
        }

        public void createFourWaySplit()
        {
            changeSplit(new FourWaySplit());
        }

        public void createThreeWayUpperSplit()
        {
            changeSplit(new ThreeWayUpperSplit());
        }

        public void createTwoWaySplit()
        {
            changeSplit(new TwoWaySplit());
        }

        public void createOneWaySplit()
        {
            changeSplit(new OneWaySplit());
        }

        private void changeSplit(SplitView splitView)
        {
            splitView.Dock = DockStyle.Fill;
            currentView = splitView;
            splitViewHost.Controls.Clear();
            splitViewHost.Controls.Add(splitView);
            activeWindow = null;
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

        public void setActiveWindow(DrawingWindow window)
        {
            if (activeWindow != null)
            {
                activeWindow.BorderStyle = BorderStyle.None;
            }
            activeWindow = window;
            window.BorderStyle = BorderStyle.Fixed3D;
        }

        public void toggleMaximize()
        {
            if (maximized)
            {
                splitViewHost.Controls.Clear();
                splitViewHost.Controls.Add(currentView);
                configureWindows();
            }
            else
            {
                maximized = true;
                frontView.setEnabled(false);
                backView.setEnabled(false);
                leftView.setEnabled(false);
                rightView.setEnabled(false);
                activeWindow.setEnabled(true);
                splitViewHost.Controls.Clear();
                splitViewHost.Controls.Add(activeWindow);
            }
        }

        /// <summary>
        /// </summary>
        private void configureWindows()
        {
            maximized = false;
            if (currentView.FrontView != null)
            {
                currentView.FrontView.Controls.Add(frontView);
                frontView.setEnabled(true);
                if (activeWindow == null)
                {
                    setActiveWindow(frontView);
                }
            }
            else
            {
                frontView.setEnabled(false);
            }
            if (currentView.BackView != null)
            {
                currentView.BackView.Controls.Add(backView);
                backView.setEnabled(true);
                if (activeWindow == null)
                {
                    setActiveWindow(backView);
                }
            }
            else
            {
                backView.setEnabled(false);
            }
            if (currentView.LeftView != null)
            {
                currentView.LeftView.Controls.Add(leftView);
                leftView.setEnabled(true);
                if (activeWindow == null)
                {
                    setActiveWindow(leftView);
                }
            }
            else
            {
                leftView.setEnabled(false);
            }
            if (currentView.RightView != null)
            {
                currentView.RightView.Controls.Add(rightView);
                rightView.setEnabled(true);
                if (activeWindow == null)
                {
                    setActiveWindow(rightView);
                }
            }
            else
            {
                rightView.setEnabled(false);
            }
        }
    }
}
