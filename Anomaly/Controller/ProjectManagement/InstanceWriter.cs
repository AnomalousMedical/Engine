using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.IO;
using Engine.Saving.XMLSaver;
using System.Xml;

namespace Anomaly
{
    class InstanceWriter
    {
        public static InstanceWriter Instance { get; private set; }

        static InstanceWriter()
        {
            Instance = new InstanceWriter();
        }

        XmlSaver xmlSaver = new XmlSaver();

        /// <summary>
        /// Save a template to the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void saveInstance(InstanceGroup group, Instance instance)
        {
            try
            {
                String path = group.FullPath + Path.DirectorySeparatorChar + instance.Name + ".ins";
                XmlTextWriter textWriter = new XmlTextWriter(path, Encoding.Unicode);
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(instance, textWriter);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not save instance {0} because of {1}.", LogLevel.Error, "Anomaly", instance.Name, e.Message);
            }
        }

        /// <summary>
        /// Update an exising template on the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void updateInstance(InstanceGroup group, Instance instance)
        {
            saveInstance(group, instance);
        }

        /// <summary>
        /// Delete an existing template from the target.
        /// </summary>
        /// <param name="group">The group the template belongs to.</param>
        /// <param name="template">The template to save.</param>
        public void deleteInstance(InstanceGroup group, Instance instance)
        {
            try
            {
                String path = group.FullPath + Path.DirectorySeparatorChar + instance.Name + ".ins";
                File.Delete(path);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not delete instance {0} because of {1}.", LogLevel.Error, "Anomaly", instance.Name, e.Message);
            }
        }

        /// <summary>
        /// Add a template group to the target.
        /// </summary>
        /// <param name="group">The template group to add.</param>
        public void addInstanceGroup(InstanceGroup group)
        {
            try
            {
                String path = group.FullPath;
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
        public void removeInstanceGroup(InstanceGroup group)
        {
            try
            {
                String path = group.FullPath;
                Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not remove directory for template group {0} because of {1}.", LogLevel.Error, "Anomaly", group.Name, e.Message);
            }
        }
    }
}
