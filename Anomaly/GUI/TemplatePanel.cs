using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Editor;
using WeifenLuo.WinFormsUI.Docking;

namespace Anomaly
{
    delegate void CreateTemplate();

    partial class TemplatePanel : DockContent
    {
        public event CreateTemplate OnCreateTemplate;

        public TemplatePanel()
        {
            InitializeComponent();
        }

        public void initialize(TemplateController templateController)
        {
            templateController.setUI(this);
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (OnCreateTemplate != null)
            {
                OnCreateTemplate.Invoke();
            }
        }

        public EditInterfaceView EditInterfaceView
        {
            get
            {
                return editInterfaceView;
            }
        }
    }
}
