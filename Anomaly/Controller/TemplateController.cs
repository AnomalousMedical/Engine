using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;

namespace Anomaly
{
    class TemplateController
    {
        private TemplateGroup parentGroup = new TemplateGroup("Templates");
        private EditInterfaceView editInterfaceView;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();

        public TemplateController()
        {

        }

        public void setEditInterfaceView(EditInterfaceView editInterfaceView)
        {
            this.editInterfaceView = editInterfaceView;
            editInterfaceView.setEditInterface(parentGroup.getEditInterface());
            editInterfaceView.OnEditInterfaceSelectionEdit += new EditInterfaceSelectionEdit(editInterfaceView_OnEditInterfaceSelectionEdit);
        }

        void editInterfaceView_OnEditInterfaceSelectionEdit(EditInterfaceViewEvent evt)
        {
            objectEditor.EditorPanel.setEditInterface(evt.EditInterface);
            objectEditor.ShowDialog(editInterfaceView.FindForm());
            objectEditor.EditorPanel.clearEditInterface();
        }
    }
}
