using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Engine.Editing;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class SolutionWindow : MDIDialog
    {
        public event EditInterfaceViewEvent InterfaceChosen;

        private Tree tree;
        private EditInterfaceTreeView editInterfaceView;

        private EditInterface selectedEditInterface;
        private GuiFrameworkUICallback uiCallback = new GuiFrameworkUICallback();

        public SolutionWindow()
            :base("Anomaly.GUI.SolutionWindow.SolutionWindow.layout")
        {
            tree = new Tree((ScrollView)window.findWidget("TreeScroller"));
            editInterfaceView = new EditInterfaceTreeView(tree, uiCallback);

            editInterfaceView.EditInterfaceSelectionChanged += editInterfaceView_EditInterfaceSelectionChanged;

            window.WindowChangedCoord += window_WindowChangedCoord;
        }

        public override void Dispose()
        {
            editInterfaceView.Dispose();
            tree.Dispose();
            base.Dispose();
        }

        public void setSolution(Solution solution)
        {
            editInterfaceView.EditInterface = solution.getEditInterface();
        }

        public EditInterface SelectedEditInterface
        {
            get
            {
                return selectedEditInterface;
            }
        }

        void editInterfaceView_EditInterfaceSelectionChanged(EditInterfaceViewEventArgs evt)
        {
            uiCallback.SelectedEditInterface = evt.EditInterface;
            selectedEditInterface = evt.EditInterface;
            if(InterfaceChosen != null)
            {
                InterfaceChosen.Invoke(evt);
            }
        }

        void window_WindowChangedCoord(Widget source, EventArgs e)
        {
            tree.layout();
        }
    }
}
