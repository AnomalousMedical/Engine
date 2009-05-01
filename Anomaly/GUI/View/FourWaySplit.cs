using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine;

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
            upperLeft.initialize("UpperLeft", controller,
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.FRONT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.FRONT_CAMERA_LOOKAT_DEFAULT));

            upperRight.initialize("UpperRight", controller,
                cameras.getValue(AnomalyConfig.BACK_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.BACK_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.BACK_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.BACK_CAMERA_LOOKAT_DEFAULT));

            lowerLeft.initialize("BottomLeft", controller,
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.RIGHT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.RIGHT_CAMERA_LOOKAT_DEFAULT));

            lowerRight.initialize("BottomRight", controller,
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_POSITION_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_POSITION_DEFAULT),
                cameras.getValue(AnomalyConfig.LEFT_CAMERA_LOOKAT_ENTRY,
                    AnomalyConfig.LEFT_CAMERA_LOOKAT_DEFAULT));
        }
    }
}
