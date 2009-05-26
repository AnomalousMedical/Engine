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
        private static char[] SEPS = { Path.DirectorySeparatorChar };

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
            while (result.ok && anomalyController.SimObjectController.hasSimObject(result.text))
            {
                result = InputBox.GetInput("Create", "That name is already in use. Please enter another.", templatePanel.FindForm(), result.text);
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
                        simObjectDefinition.Enabled = true;
                        simObjectDefinition.Translation = anomalyController.MoveController.Translation;
                        anomalyController.SimObjectController.createSimObject(simObjectDefinition);
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
                if ((File.GetAttributes(path + Path.DirectorySeparatorChar + dir) & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    TemplateGroup subGroup = new TemplateGroup(dir, templateWriter);
                    group.addGroup(subGroup);
                    scanForFiles(subGroup);
                }
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

        /// <summary>
        /// Scan the templates and find the definition specified by fullpath. If
        /// the definition cannot be found null will be returned.
        /// </summary>
        /// <param name="fullpath">The path to the defintion to retrieve.</param>
        /// <returns>The SimObjectDefintion specified by fullpath or null if it cannot be found.</returns>
        public SimObjectDefinition getTemplateFromPath(String fullpath)
        {
            String[] elements = fullpath.Split(SEPS);
            return parentGroup.getTemplateFromPath(elements, 1);
        }
    }
}
