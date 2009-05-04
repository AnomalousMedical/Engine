using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Logging;
using System.Xml;
using Engine.Saving.XMLSaver;

namespace Anomaly
{
    class TemplateWriter
    {
        String rootPath;
        XmlSaver xmlSaver = new XmlSaver();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rootPath">The root path of the template groups.</param>
        public TemplateWriter(String rootPath)
        {
            this.rootPath = rootPath;
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
        }

        /// <summary>
        /// Save a template to the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void saveTemplate(TemplateGroup group, SimObjectDefinition template)
        {
            try
            {
                String path = rootPath + Path.DirectorySeparatorChar + group.FullPath + Path.DirectorySeparatorChar + template.Name + ".tpl";
                XmlTextWriter textWriter = new XmlTextWriter(path, Encoding.Unicode);
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(template, textWriter);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not save template {0} because of {1}.", LogLevel.Error, "Anomaly", template.Name, e.Message);
            }
        }

        /// <summary>
        /// Update an exising template on the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void updateTemplate(TemplateGroup group, SimObjectDefinition template)
        {
            saveTemplate(group, template);
        }

        /// <summary>
        /// Delete an existing template from the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void deleteTemplate(TemplateGroup group, SimObjectDefinition template)
        {
            try
            {
                String path = rootPath + Path.DirectorySeparatorChar + group.FullPath + Path.DirectorySeparatorChar + template.Name + ".tpl";
                File.Delete(path);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not delete template {0} because of {1}.", LogLevel.Error, "Anomaly", template.Name, e.Message);
            }
        }

        /// <summary>
        /// Add a template group to the target.
        /// </summary>
        /// <param name="group">The template group to add.</param>
        public void addTemplateGroup(TemplateGroup group)
        {
            try
            {
                String path = rootPath + Path.DirectorySeparatorChar + group.FullPath;
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not create directory for template group {0} because of {1}", LogLevel.Error, "Anomaly", group.Name, e.Message);
            }
        }

        /// <summary>
        /// Remove a template group from the target.
        /// </summary>
        /// <param name="group">The template group to remove.</param>
        public void removeTemplateGroup(TemplateGroup group)
        {
            try
            {
                String path = rootPath + Path.DirectorySeparatorChar + group.FullPath;
                Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not remove directory for template group {0} because of {1}.", LogLevel.Error, "Anomaly", group.Name, e.Message);
            }
        }
    }
}
