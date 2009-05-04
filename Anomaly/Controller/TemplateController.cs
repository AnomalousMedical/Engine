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
using Engine.Editing;
using Engine.Saving;

namespace Anomaly
{
    class TemplateController
    {
        private TemplateGroup parentGroup;
        TemplatePanel templatePanel;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private TemplateWriter templateWriter;
        private String rootPath;
        private XmlSaver xmlSaver = new XmlSaver();
        private AnomalyController anomalyController;
        private CopySaver copySaver = new CopySaver();

        public TemplateController(String rootPath, AnomalyController anomalyController)
        {
            this.rootPath = rootPath;
            templateWriter = new TemplateWriter(rootPath);
            parentGroup = new TemplateGroup("Templates", templateWriter);
            scanForFiles(parentGroup);
            this.anomalyController = anomalyController;
        }

        public void setUI(TemplatePanel templatePanel)
        {
            this.templatePanel = templatePanel;
            templatePanel.EditInterfaceView.setEditInterface(parentGroup.getEditInterface());
            templatePanel.EditInterfaceView.OnEditInterfaceSelectionEdit += new EditInterfaceSelectionEdit(editInterfaceView_OnEditInterfaceSelectionEdit);
            templatePanel.OnCreateTemplate += new CreateTemplate(templatePanel_OnCreateTemplate);
        }

        void templatePanel_OnCreateTemplate()
        {
            InputResult result = InputBox.GetInput("Create", "Enter a name.", templatePanel.FindForm());
            while (result.ok && false)
            {
                result = InputBox.GetInput("Create", "Enter a name.", templatePanel.FindForm(), result.text);
            }
            if (result.ok)
            {
                EditInterface editInterface = templatePanel.EditInterfaceView.getSelectedEditInterface();
                if (editInterface.hasEditableProperties())
                {
                    Template template = editInterface.getEditableProperties().First() as Template;
                    if (template != null)
                    {
                        SimObjectDefinition simObjectDefinition = (SimObjectDefinition)copySaver.copyObject(template.Definition);
                        simObjectDefinition.Name = result.text;
                        anomalyController.SceneController.createSimObject(simObjectDefinition);
                    }
                }
            }
        }

        void editInterfaceView_OnEditInterfaceSelectionEdit(EditInterfaceViewEvent evt)
        {
            EditInterface editInterface = evt.EditInterface;
            if (editInterface.hasEditableProperties())
            {
                Template template = editInterface.getEditableProperties().First() as Template;
                if (template != null)
                {
                    objectEditor.EditorPanel.setEditInterface(template.Definition.getEditInterface());
                    objectEditor.ShowDialog(templatePanel.FindForm());
                    objectEditor.EditorPanel.clearEditInterface();
                    template.updated();
                }
            }
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
