using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using MyGUIPlugin;

namespace Anomalous.GuiFramework.Editor
{
    public class ObjectEditor : IDisposable
    {
        private EditInterfaceTreeView treeView;
        private PropertyEditor propEditor;
        private EditInterface parentEditInterface;
        private EditInterface selectedEditInterface;
        private GuiFrameworkUICallback uiCallback;

        public ObjectEditor(EditInterfaceTreeView treeView, PropertyEditor propEditor, GuiFrameworkUICallback uiCallback)
        {
            this.treeView = treeView;
            treeView.EditInterfaceSelectionChanged += treeView_EditInterfaceSelectionChanged;
            this.propEditor = propEditor;
            this.uiCallback = uiCallback;
        }

        public void Dispose()
        {
            treeView.EditInterfaceSelectionChanged -= treeView_EditInterfaceSelectionChanged;
        }

        public EditInterface EditInterface
        {
            get
            {
                return parentEditInterface;
            }
            set
            {
                parentEditInterface = value;
                treeView.EditInterface = value;
                propEditor.EditInterface = value;
            }
        }

        public EditInterface SelectedEditInterface
        {
            get
            {
                return selectedEditInterface;
            }
        }

        public void clear()
        {
            EditInterface = null;
        }

        void treeView_EditInterfaceSelectionChanged(EditInterfaceViewEventArgs evt)
        {
            selectedEditInterface = evt.EditInterface;
            propEditor.EditInterface = selectedEditInterface;
            uiCallback.SelectedEditInterface = selectedEditInterface;
        }
    }
}
