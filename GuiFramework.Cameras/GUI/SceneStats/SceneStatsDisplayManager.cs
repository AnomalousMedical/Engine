using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Cameras
{
    public class SceneStatsDisplayManager
    {
        private List<SceneStatsDisplay> activeSceneStats = new List<SceneStatsDisplay>();
        private RenderTarget displayStatsTarget;
        private bool statsVisible = false;

        public SceneStatsDisplayManager(SceneViewController sceneViewController, RenderTarget displayStatsTarget)
        {
            this.displayStatsTarget = displayStatsTarget;
            sceneViewController.WindowCreated += sceneViewController_WindowCreated;
        }

        void sceneViewController_WindowCreated(SceneViewWindow window)
        {
            MDISceneViewWindow mdiWindow = window as MDISceneViewWindow;
            if (mdiWindow != null)
            {
                SceneStatsDisplay licenseDisplay = new SceneStatsDisplay(displayStatsTarget);
                licenseDisplay.Visible = statsVisible;
                activeSceneStats.Add(licenseDisplay);
                mdiWindow.addChildContainer(licenseDisplay.LayoutContainer);
                mdiWindow.Disposed += (win) =>
                {
                    activeSceneStats.Remove(licenseDisplay);
                    licenseDisplay.Dispose();
                };
            }
        }

        public bool StatsVisible
        {
            get
            {
                return statsVisible;
            }
            set
            {
                if (statsVisible != value)
                {
                    statsVisible = value;
                    foreach (var display in activeSceneStats)
                    {
                        display.Visible = statsVisible;
                    }
                }
            }
        }
    }
}
