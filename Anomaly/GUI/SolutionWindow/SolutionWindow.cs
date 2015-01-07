using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Engine.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class SolutionWindow : MDIDialog
    {
        public SolutionWindow()
            :base("Anomaly.GUI.SolutionWindow.SolutionWindow.layout")
        {
            
        }

        //public void setSolution(Solution solution)
        //{
        //    editInterfaceView.setEditInterface(solution.getEditInterface());
        //}

        //public EditInterface SelectedEditInterface
        //{
        //    get
        //    {
        //        return editInterfaceView.getSelectedEditInterface();
        //    }
        //}

        //public event EditInterfaceViewEvent InterfaceChosen
        //{
        //    add
        //    {
        //        editInterfaceView.OnEditInterfaceChosen += value;
        //    }
        //    remove
        //    {
        //        editInterfaceView.OnEditInterfaceChosen -= value;
        //    }
        //}
    }
}
