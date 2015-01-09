using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Engine;
using Engine.Editing;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    public delegate void PropertiesEditorEvent(EditInterface editInterface, object editingObject);

    public class PropertiesEditor : MDIDialog
    {
        /// <summary>
        /// Called when the main object in this ui has changed.
        /// </summary>
        public event PropertiesEditorEvent MainInterfaceChanged;

        /// <summary>
        /// Called when the active interface has changed.
        /// </summary>
        public event PropertiesEditorEvent ActiveInterfaceChanged;

        /// <summary>
        /// Called when a field has been changed.
        /// </summary>
        public event PropertiesEditorEvent FieldChanged;

        private GuiFrameworkUICallback uiCallback;
        private Tree tree;
        private EditInterfaceTreeView editTreeView;

        private AddRemoveButtons addRemoveButtons;
        private ScrollView tableScroller;
        private ResizingTable table;
        private PropertiesTable propTable;

        private ObjectEditor objectEditor;

        private Object currentEditingObject;
        private PropertiesEditorEvent currentFieldChangedCallback;

        private int gap;

        private Splitter splitter;

        public PropertiesEditor(String caption, String persistName, bool horizontal)
            : base(horizontal ? MDIObjectEditor.HorizontalLayoutName : MDIObjectEditor.VerticalLayoutName, persistName)
        {
            window.Caption = caption;

            uiCallback = new GuiFrameworkUICallback();

            tree = new Tree((ScrollView)window.findWidget("TreeScroller"));
            editTreeView = new EditInterfaceTreeView(tree, uiCallback);
            editTreeView.EditInterfaceSelectionChanged += editTreeView_EditInterfaceSelectionChanged;
            editTreeView.EditInterfaceAdded += editTreeView_EditInterfaceAdded;
            editTreeView.EditInterfaceRemoved += editTreeView_EditInterfaceRemoved;

            addRemoveButtons = new AddRemoveButtons((Button)window.findWidget("Add"), (Button)window.findWidget("Remove"), window.findWidget("AddRemovePanel"));
            addRemoveButtons.VisibilityChanged += addRemoveButtons_VisibilityChanged;

            tableScroller = (ScrollView)window.findWidget("TableScroller");
            table = new ResizingTable(tableScroller);
            propTable = new PropertiesTable(table, uiCallback, addRemoveButtons);
            propTable.EditablePropertyValueChanged += propTable_EditablePropertyValueChanged;
            addRemoveButtons.RemoveButtonClicked += addRemoveButtons_RemoveButtonClicked;
            addRemoveButtons.AddButtonClicked += addRemoveButtons_AddButtonClicked;

            objectEditor = new ObjectEditor(editTreeView, propTable, uiCallback);

            this.Resized += DebugVisualizer_Resized;

            gap = tableScroller.Bottom - addRemoveButtons.Top;

            splitter = new Splitter(window.findWidget("Splitter"));
            splitter.Widget1Resized += split => tree.layout();
            splitter.Widget2Resized += split => table.layout();
        }

        public override void Dispose()
        {
            objectEditor.Dispose();
            propTable.Dispose();
            editTreeView.Dispose();
            tree.Dispose();
            base.Dispose();
        }

        public void setEditInterface(EditInterface editInterface, object editingObject, PropertiesEditorEvent FieldChangedCallback)
        {
            objectEditor.EditInterface = editInterface;
            Caption = editInterface.getName();

            currentEditingObject = editingObject;
            currentFieldChangedCallback = FieldChangedCallback;

            if(AutoExpand)
            {
                editTreeView.expandAll();
            }

            if (MainInterfaceChanged != null)
            {
                MainInterfaceChanged.Invoke(editInterface, editingObject);
            }

            if (ActiveInterfaceChanged != null)
            {
                ActiveInterfaceChanged.Invoke(editInterface, editingObject);
            }
        }

        public void clearEditInterface()
        {
            objectEditor.EditInterface = null;

            currentEditingObject = null;
            currentFieldChangedCallback = null;

            this.Caption = "Properties";

            if (MainInterfaceChanged != null)
            {
                MainInterfaceChanged.Invoke(null, null);
            }
        }

        protected override void customDeserialize(ConfigSection section, ConfigFile file)
        {
            base.customDeserialize(section, file);
            splitter.SplitterPosition = section.getValue("SplitterPosition", splitter.SplitterPosition);
        }

        protected override void customSerialize(ConfigSection section, ConfigFile file)
        {
            base.customSerialize(section, file);
            section.setValue("SplitterPosition", splitter.SplitterPosition);
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

        public bool AutoExpand { get; set; }

        public EditInterface SelectedEditInterface
        {
            get
            {
                return objectEditor.SelectedEditInterface;
            }
        }

        void DebugVisualizer_Resized(object sender, EventArgs e)
        {
            splitter.layout();
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

        void editTreeView_EditInterfaceSelectionChanged(EditInterfaceViewEventArgs evt)
        {
            if (ActiveInterfaceChanged != null)
            {
                ActiveInterfaceChanged.Invoke(evt.EditInterface, null);
            }
        }

        void propTable_EditablePropertyValueChanged(EditableProperty var)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(objectEditor.EditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(objectEditor.EditInterface, null);
            }
        }

        void addRemoveButtons_RemoveButtonClicked(Widget source, EventArgs e)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(objectEditor.EditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(objectEditor.EditInterface, null);
            }
        }

        void addRemoveButtons_AddButtonClicked(Widget source, EventArgs e)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(objectEditor.EditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(objectEditor.EditInterface, null);
            }
        }

        void editTreeView_EditInterfaceAdded(EditInterfaceViewEventArgs evt)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(objectEditor.EditInterface, currentEditingObject);
            }
        }

        void editTreeView_EditInterfaceRemoved(EditInterfaceViewEventArgs evt)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(objectEditor.EditInterface, currentEditingObject);
            }
        }
    }
}
