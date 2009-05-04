using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using System.IO;
using Logging;
using Engine.ObjectManagement;
using System.Xml;
using Engine.Saving.XMLSaver;

namespace Anomaly
{
    class TemplateController
    {
        private TemplateGroup parentGroup;
        private EditInterfaceView editInterfaceView;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private TemplateWriter templateWriter;
        private String rootPath;
        private XmlSaver xmlSaver = new XmlSaver();

        public TemplateController(String rootPath)
        {
            this.rootPath = rootPath;
            templateWriter = new TemplateWriter(rootPath);
            parentGroup = new TemplateGroup("Templates", templateWriter);
            scanForFiles(parentGroup);
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
            evt.EditInterface.fireInterfaceChanged();
            objectEditor.EditorPanel.clearEditInterface();
        }

        /// <summary>
        /// This function will search for any existing files under a group node.
        /// </summary>
        public void scanForFiles(TemplateGroup group)
        {
            String path = rootPath + Path.DirectorySeparatorChar + group.FullPath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            String[] groups = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (String groupPath in groups)
            {
                String dir = Path.GetFileName(groupPath);
                TemplateGroup subGroup = new TemplateGroup(dir, templateWriter);
                group.addGroup(subGroup);
                scanForFiles(subGroup);
            }
            String[] templates = Directory.GetFiles(path, "*.tpl", SearchOption.TopDirectoryOnly);
            foreach (String template in templates)
            {
                try
                {
                    XmlTextReader textReader = new XmlTextReader(Path.GetFullPath(template));
                    SimObjectDefinition simObjectDef = (SimObjectDefinition)xmlSaver.restoreObject(textReader);
                    textReader.Close();
                    group.addSimObject(simObjectDef);
                }
                catch (Exception e)
                {
                    Log.Default.sendMessage("Could not load template {0} because of {1}.", LogLevel.Error, "Editor", group.FullPath + Path.DirectorySeparatorChar + template, e.Message);
                }
            }
        }
    }
}
