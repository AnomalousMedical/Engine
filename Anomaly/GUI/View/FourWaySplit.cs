using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine;
using Engine.ObjectManagement;

namespace Anomaly.GUI.View
{
    partial class FourWaySplit : UserControl
    {
        public FourWaySplit()
        {
            InitializeComponent();
        }

        public void initialize(AnomalyController controller)
        {
            ConfigSection cameras = AnomalyConfig.ConfigFile.createOrRetrieveConfigSection(AnomalyConfig.CAMERA_HEADER);
            upperLeft.initialize("UpperLeft", controller.EventManager, controller.PluginManager.RendererPlugin,
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_LOOKAT_DEFAULT));

            upperRight.initialize("UpperRight", controller.EventManager, controller.PluginManager.RendererPlugin,
                cameras.getValue(AnomalyConfig.BACK_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.BACK_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.BACK_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.BACK_CAMERA_LOOKAT_DEFAULT));

            lowerLeft.initialize("BottomLeft", controller.EventManager, controller.PluginManager.RendererPlugin,
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_LOOKAT_DEFAULT));

            lowerRight.initialize("BottomRight", controller.EventManager, controller.PluginManager.RendererPlugin,
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_LOOKAT_DEFAULT));

            controller.OnSceneLoaded += new SceneLoaded(controller_OnSceneLoaded);
            controller.OnSceneUnloading += new SceneUnloading(controller_OnSceneUnloading);
        }

        void controller_OnSceneUnloading(AnomalyController controller, SimScene scene)
        {
            upperLeft.destroyCamera(controller.MainTimer);
            upperRight.destroyCamera(controller.MainTimer);
            lowerLeft.destroyCamera(controller.MainTimer);
            lowerRight.destroyCamera(controller.MainTimer);
        }

        void controller_OnSceneLoaded(AnomalyController controller, SimScene scene)
        {
            upperLeft.createCamera(controller.MainTimer, scene);
            upperRight.createCamera(controller.MainTimer, scene);
            lowerLeft.createCamera(controller.MainTimer, scene);
            lowerRight.createCamera(controller.MainTimer, scene);
        }
    }
}
