using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Logging;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definition for a group of SimObjects.
    /// </summary>
    public class SimObjectManagerDefinition : Saveable
    {
        private Dictionary<String, SimObjectInstanceDefinition> instances = new Dictionary<string, SimObjectInstanceDefinition>();
        private Dictionary<String, SimObjectDefinition> templates = new Dictionary<string, SimObjectDefinition>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimObjectManagerDefinition()
        {

        }

        /// <summary>
        /// Add a definition for a single instance of a SimObject.
        /// </summary>
        /// <param name="instance">The definition to add.</param>
        public void addInstanceDefinition(SimObjectInstanceDefinition instance)
        {
            instances.Add(instance.Name, instance);
        }

        /// <summary>
        /// Remove a definition for a single instance of a SimObject.
        /// </summary>
        /// <param name="instance">The instance to remove.</param>
        public void removeInstanceDefinition(SimObjectInstanceDefinition instance)
        {
            instances.Remove(instance.Name);
        }

        /// <summary>
        /// Add a SimObjectDefinition to use as a template.
        /// </summary>
        /// <param name="template">The template to add.</param>
        public void addTemplate(SimObjectDefinition template)
        {
            templates.Add(template.Name, template);
        }

        /// <summary>
        /// Remove a SimObjectDefinition to use as a template.
        /// </summary>
        /// <param name="template">The template to remove.</param>
        public void removeTemplate(SimObjectDefinition template)
        {
            templates.Remove(template.Name);
        }

        /// <summary>
        /// Returns a new SimObjectManager that contains all the SimObjects
        /// defined in this class. The SimObjects will be empty, but they will
        /// be registered for construction.
        /// </summary>
        /// <param name="subScene">The SimSubScene that will be used to build the objects in the returned manager.</param>
        /// <returns>A new SimObjectManager that contains empty SimObjects ready to be built into the given SimSubScene.</returns>
        public SimObjectManager createSimObjectManager(SimSubScene subScene)
        {
            SimObjectManager manager = new SimObjectManager(subScene);
            foreach (SimObjectInstanceDefinition instance in instances.Values)
            {
                if (templates.ContainsKey(instance.DefinitionName))
                {
                    SimObject simObject = new SimObject(instance.Name);
                    Vector3 vector3 = instance.Translation;
                    Quaternion quaternion = instance.Rotation;
                    simObject.updatePosition(ref vector3, ref quaternion, null);
                    vector3 = instance.Scale;
                    simObject.updateScale(ref vector3, null);
                    simObject.setEnabled(instance.Enabled);
                    templates[instance.DefinitionName].register(subScene, simObject);
                    manager.addSimObject(simObject);
                }
                else
                {
                    Log.Default.sendMessage("Had a definition for a sim object {0} that had an undefined template {1}. Skipped.", LogLevel.Warning, "Engine", instance.Name, instance.DefinitionName);
                }
            }
            return manager;
        }

        #region Saveable Members

        private const string INSTANCE_BASE = "Instance";
        private const string TEMPLATE_BASE = "Template";

        public void getInfo(SaveInfo info)
        {
            int i = 0;
            foreach (SimObjectInstanceDefinition instance in instances.Values)
            {
                info.AddValue(INSTANCE_BASE + i, instance);
            }
            i = 0;
            foreach (SimObjectDefinition template in templates.Values)
            {
                info.AddValue(TEMPLATE_BASE + i, template);
            }
        }

        #endregion
    }
}
