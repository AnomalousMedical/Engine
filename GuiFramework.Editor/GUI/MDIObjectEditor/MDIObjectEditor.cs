using Engine.Editing;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    public class MDIObjectEditor : MDIDialog
    {
        private GuiFrameworkUICallback uiCallback;
        private Tree tree;
        private EditInterfaceTreeView editTreeView;

        private AddRemoveButtons addRemoveButtons;
        private ScrollView tableScroller;
        private ResizingTable table;
        private PropertiesTable propTable;

        private ObjectEditor objectEditor;

        private int gap;

        public MDIObjectEditor(String caption, String persistName)
            : base("Anomalous.GuiFramework.Editor.GUI.MDIObjectEditor.MDIObjectEditor.layout", persistName)
        {
            window.Caption = caption;

            uiCallback = new GuiFrameworkUICallback();

            tree = new Tree((ScrollView)window.findWidget("TreeScroller"));
            editTreeView = new EditInterfaceTreeView(tree, uiCallback);

            addRemoveButtons = new AddRemoveButtons((Button)window.findWidget("Add"), (Button)window.findWidget("Remove"), window.findWidget("AddRemovePanel"));
            addRemoveButtons.VisibilityChanged += addRemoveButtons_VisibilityChanged;

            tableScroller = (ScrollView)window.findWidget("TableScroller");
            table = new ResizingTable(tableScroller);
            propTable = new PropertiesTable(table, uiCallback, addRemoveButtons);

            objectEditor = new ObjectEditor(editTreeView, propTable, uiCallback);

            this.Resized += DebugVisualizer_Resized;

            gap = tableScroller.Bottom - addRemoveButtons.Top;
        }

        public override void Dispose()
        {
            objectEditor.Dispose();
            propTable.Dispose();
            editTreeView.Dispose();
            tree.Dispose();
            base.Dispose();
        }

        public EditInterface EditInterface
        {
            get
            {
                return objectEditor.EditInterface;
            }
            set
            {
                objectEditor.EditInterface = value;
            }
        }

        public String Caption
        {
            get
            {
                return window.Caption;
            }
            set
            {
                window.Caption = value;
            }
        }

        void DebugVisualizer_Resized(object sender, EventArgs e)
        {
            tree.layout();
            table.layout();
        }

        void addRemoveButtons_VisibilityChanged(AddRemoveButtons source, bool visible)
        {
            if (visible)
            {
                tableScroller.Height = window.ClientWidget.Height - (addRemoveButtons.Height - gap);
            }
            else
            {
                tableScroller.Height = window.ClientWidget.Height - tableScroller.Top;
            }
        }
    }
}
