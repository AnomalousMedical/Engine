using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.IO;
using Engine.Saving.XMLSaver;
using System.Xml;
using Engine.ObjectManagement;

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
        public bool saveInstance(InstanceGroup group, Instance instance)
        {
            try
            {
                String path = getInstanceFileName(group, instance.Name);
                XmlTextWriter textWriter = new XmlTextWriter(path, Encoding.Unicode);
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(instance, textWriter);
                textWriter.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not save instance {0} because of {1}.", LogLevel.Error, "Anomaly", instance.Name, e.Message);
                return false;
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
        public void deleteInstance(InstanceGroup group, String instanceName)
        {
            try
            {
                String path = getInstanceFileName(group, instanceName);
                File.Delete(path);
            }
            catch (Exception e)
            {
                Log.Default.sendMessage("Could not delete instance {0} because of {1}.", LogLevel.Error, "Anomaly", instanceName, e.Message);
            }
        }

        public String getInstanceFileName(InstanceGroup group, String instanceName)
        {
            return group.FullPath + Path.DirectorySeparatorChar + instanceName + ".ins";
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

        public void scanForFiles(InstanceGroup group)
        {
            String path = group.FullPath;

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
                    InstanceGroup subGroup = new InstanceGroup(dir, groupPath);
                    group.addGroup(subGroup);
                    scanForFiles(subGroup);
                }
            }
            
            String[] instances = Directory.GetFiles(path, "*.ins", SearchOption.TopDirectoryOnly);
            foreach (String instanceFile in instances)
            {
                group.addInstanceFile(Path.GetFileNameWithoutExtension(instanceFile));
            }
        }
    }
}




            ////Load legacy files
            //String[] templates = Directory.GetFiles(path, "*.tpl", SearchOption.TopDirectoryOnly);
            //foreach (String template in templates)
            //{
            //    try
            //    {
            //        XmlTextReader textReader = new XmlTextReader(Path.GetFullPath(template));
            //        SimObjectDefinition simObjectDef = (SimObjectDefinition)xmlSaver.restoreObject(textReader);
            //        textReader.Close();
            //        Instance instance = new Instance(simObjectDef.Name, simObjectDef);
            //        group.addExistingInstance(instance);
            //    }
            //    catch (Exception e)
            //    {
            //        Log.Default.sendMessage("Could not load template {0} because of {1}.", LogLevel.Error, "Editor", group.FullPath + Path.DirectorySeparatorChar + template, e.Message);
            //    }
            //}