using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    partial class TemplatePanel : UserControl
    {
        public TemplatePanel()
        {
            InitializeComponent();
        }

        public void initialize(TemplateController templateController)
        {
            templateController.setEditInterfaceView(editInterfaceView);
        }
    }
}
