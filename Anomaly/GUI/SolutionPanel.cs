using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Editor;

namespace Anomaly
{
    public partial class SolutionPanel : DockContent
    {
        public SolutionPanel()
        {
            InitializeComponent();
        }

        public void setSolution(Solution solution)
        {
            editInterfaceView.setEditInterface(solution.getEditInterface());
        }

        public event EditInterfaceChosen InterfaceChosen
        {
            add
            {
                editInterfaceView.OnEditInterfaceChosen += value;
            }
            remove
            {
                editInterfaceView.OnEditInterfaceChosen -= value;
            }
        }
    }
}
