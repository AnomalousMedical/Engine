using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Engine.ObjectManagement;
using Engine;
using Logging;
using Engine.Saving;

namespace Anomaly
{
    /// <summary>
    /// This class can read instance files and create the objects definined
    /// within.
    /// </summary>
    class InstanceBuilder
    {
        private const String INSTANCES_ELEMENT = "Instances";
        private const String SIMOBJECT_ELEMENT = "SimObject";
        private const String LOCATION_ELEMENT = "Location";
        private const String ROTATION_ELEMENT = "Rotation";
        private const String TEMPLATE_ATTRIBUTE = "template";

        private static readonly char[] SEPS = { ',' };
        private const float DEG_TO_RAD = 0.0174532925f;

        private TemplateController templateController;
        private CopySaver copySaver = new CopySaver();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="templateController">The TemplateController to grab templates from.</param>
        public InstanceBuilder(TemplateController templateController)
        {
            this.templateController = templateController;
        }

        /// <summary>
        /// Load the SimObject instances from the given XML stream into the
        /// given SimObjectManagerDefinition.
        /// </summary>
        /// <param name="xmlReader">The XML stream to read instances from.</param>
        /// <param name="objectManager">The SimObjectManagerDefinition to fill with the new instances.</param>
        public void loadInstances(XmlReader xmlReader, SimObjectManagerDefinition objectManager)
        {
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name.Equals(SIMOBJECT_ELEMENT))
                    {
                        String templatePath = xmlReader.GetAttribute(TEMPLATE_ATTRIBUTE);
                        SimObjectDefinition template = templateController.getTemplateFromPath(templatePath);
                        if (template != null)
                        {
                            if (!objectManager.hasSimObject(template.Name))
                            {
                                SimObjectDefinition instance = copySaver.copyObject(template) as SimObjectDefinition;
                                while (!(xmlReader.Name == SIMOBJECT_ELEMENT && xmlReader.NodeType == XmlNodeType.EndElement) && xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element)
                                    {
                                        if (xmlReader.Name == ROTATION_ELEMENT)
                                        {
                                            instance.Rotation = readRotation(xmlReader);
                                        }
                                        else if (xmlReader.Name == LOCATION_ELEMENT)
                                        {
                                            instance.Translation = readTranslation(xmlReader);
                                        }
                                    }
                                }
                                objectManager.addSimObject(instance);
                            }
                            else
                            {
                                Log.Default.sendMessage("The SimObjectManager already has an instance named {0}. Skipping this instance.", LogLevel.Warning, "Editor", template.Name);
                            }
                        }
                        else
                        {
                            Log.Default.sendMessage("Could not find template {0}. Skipping this instance.", LogLevel.Warning, "Editor", templatePath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read a rotation from the XML stream.
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        private Quaternion readRotation(XmlReader xmlReader)
        {
            Quaternion rotation = new Quaternion();
            xmlReader.Read();
            String[] rots = xmlReader.Value.Split(SEPS);
            //Euler angles
            if (rots.Length == 3)
            {
                rotation.setEuler(float.Parse(rots[0]) * DEG_TO_RAD, float.Parse(rots[1]) * DEG_TO_RAD, float.Parse(rots[2]) * DEG_TO_RAD);
            }
            else
            {
                Log.Default.sendMessage("Error loading rotation does not contain 3 numbers value: {0}.", LogLevel.Error, "ShapeLoading", xmlReader.Value);
                rotation = Quaternion.Identity;
            }
            return rotation;
        }

        /// <summary>
        /// Read a translation from the XML stream.
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
        private Vector3 readTranslation(XmlReader xmlReader)
        {
            Vector3 translation = new Vector3();
            xmlReader.Read();
            String[] locs = xmlReader.Value.Split(SEPS);
            if (locs.Length == 3)
            {
                translation.x = float.Parse(locs[0]);
                translation.y = float.Parse(locs[1]);
                translation.z = float.Parse(locs[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading translation does not contain 3 numbers value: {0}.", LogLevel.Error, "ShapeLoading", xmlReader.Value);
                translation = Vector3.Zero;
            }
            return translation;
        }
    }
}
