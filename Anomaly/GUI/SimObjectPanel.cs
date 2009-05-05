using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Editor;

namespace Anomaly
{
    partial class SimObjectPanel : UserControl
    {
        private AnomalyController controller;

        public SimObjectPanel()
        {
            InitializeComponent();
        }

        public void intialize(AnomalyController controller)
        {
            this.controller = controller;
            controller.SimObjectController.setUI(this);
        }

        public EditInterfaceView EditInterface
        {
            get
            {
                return editInterfaceView;
            }
        }
    }
}
