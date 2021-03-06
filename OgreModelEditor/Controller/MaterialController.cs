﻿using Engine;
using Engine.Editing;
using Engine.Resources;
using MyGUIPlugin;
using Newtonsoft.Json;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(MaterialController.MaterialSerializer))]
    class MaterialController
    {
        private EditInterface editInterface;
        private OgreModelEditorController controller;
        private List<MaterialDescription> currentDescriptions = new List<MaterialDescription>();
        private String currentDir;
        private String currentOutputFile;

        public MaterialController(OgreModelEditorController controller)
        {
            this.controller = controller;

            editInterface = ReflectedEditInterface.createEditInterface(this, "Materials");
            editInterface.addCommand(new EditInterfaceCommand("Add", () =>
            {
                createMaterial("NewMaterial");
            }));
            var descriptionManager = editInterface.createEditInterfaceManager<MaterialDescription>(i => i.getEditInterface());
            descriptionManager.addCommand(new EditInterfaceCommand("Remove", cb =>
            {
                var desc = descriptionManager.resolveSourceObject(cb.getSelectedEditInterface());
                removeMaterial(desc);
            }));
        }

        public void loadMaterials(String currentDir)
        {
            this.currentDir = currentDir;

            var listener = new MaterialResourceListener(this);

            var resourceManager = controller.PluginManager.createResourceManagerForListener("MaterialController", listener);
            var subsystem = resourceManager.getSubsystemResource("MaterialController");
            var group = subsystem.addResourceGroup("MaterialController");
            group.addResource("", "EngineArchive", true);

            resourceManager.initializeResources();
        }

        public void createMaterial(string entityMaterialName)
        {
            var desc = new MaterialDescription()
            {
                Name = entityMaterialName,
                Builder = "VirtualTexture"
            };
            currentDescriptions.Add(desc);
            editInterface.addSubInterface(desc);
        }

        public void removeMaterial(MaterialDescription desc)
        {
            currentDescriptions.Remove(desc);
            editInterface.removeSubInterface(desc);
        }

        public EditInterface EditInterface
        {
            get
            {
                return editInterface;
            }
        }

        private void materialFound(MaterialDescription description)
        {
            if (description.IsRoot)
            {
                currentDescriptions.Add(description);
                editInterface.addSubInterface(description, description.getEditInterface());
            }
        }

        internal void clearMaterials()
        {
            foreach (var desc in currentDescriptions)
            {
                editInterface.removeSubInterface(desc);
            }
            currentDescriptions.Clear();
        }

        internal void saveMaterials()
        {
            HashSet<String> writtenFiles = new HashSet<string>();

            bool wroteSomething = false;
            foreach (var desc in currentDescriptions)
            {
                currentOutputFile = desc.SourceFile;
                if (currentOutputFile != null && !writtenFiles.Contains(currentOutputFile))
                {
                    writtenFiles.Add(currentOutputFile);
                    String outputFile = Path.Combine(currentDir, desc.SourceFile);

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    using (StreamWriter textWriter = new StreamWriter(outputFile, false))
                    {
                        serializer.Serialize(textWriter, this);
                    }
                    wroteSomething = true;
                    Logging.Log.ImportantInfo("Saved material file {0}", outputFile);
                }
            }
            if(!wroteSomething && currentDescriptions.Count > 0)
            {
                InputBox.GetInput("Save Materials", "Enter a name for the material file.", true, nameMaterialResult);
            }
        }

        bool nameMaterialResult(String result, ref String errorPrompt)
        {
            if(!result.EndsWith(".jsonmat", StringComparison.InvariantCultureIgnoreCase))
            {
                result += ".jsonmat";
            }
            //This only happens if no descs have output files and there are actually output files if(!wroteSomething && currentDescriptions.Count > 0)
            currentDescriptions[0].SourceFile = Path.Combine(currentDir, result);
            saveMaterials();
            controller.refreshVFS();
            return true;
        }

        internal class MaterialSerializer : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(MaterialController).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return null;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                MaterialController matSerial = value as MaterialController;
                writer.WriteStartArray();

                foreach (var desc in matSerial.currentDescriptions)
                {
                    if(desc.SourceFile == null)
                    {
                        desc.SourceFile = matSerial.currentOutputFile;
                    }
                    if (desc.SourceFile == matSerial.currentOutputFile)
                    {
                        desc.rebuildVariants();
                        serializer.Serialize(writer, desc);
                    }
                }

                writer.WriteEndArray();
            }
        }

        class MaterialReader : MaterialBuilder
        {
            MaterialController controller;

            public MaterialReader(MaterialController controller)
            {
                this.controller = controller;
            }

            public override string Name
            {
                get
                {
                    return "VirtualTexture";
                }
            }

            public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
            {
                controller.materialFound(description);
            }

            public override void destroyMaterial(MaterialPtr materialPtr)
            {

            }

            public override void initializationComplete()
            {

            }
        }

        class MaterialResourceListener : ResourceListener
        {
            private MaterialReader reader;
            MaterialParserManager materialParser = new MaterialParserManager();

            public MaterialResourceListener(MaterialController controller)
            {
                reader = new MaterialReader(controller);
                materialParser.addMaterialBuilder(reader);
            }

            public void initializeResources(IEnumerable<ResourceGroup> groups)
            {
                materialParser.initializeResources(groups);
            }

            public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
            {
                materialParser.resourceAdded(group, resource);
            }

            public void resourceGroupAdded(ResourceGroup group)
            {
                materialParser.resourceGroupAdded(group);
            }

            public void resourceGroupRemoved(ResourceGroup group)
            {
                materialParser.resourceGroupRemoved(group);
            }

            public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
            {
                materialParser.resourceRemoved(group, resource);
            }
        }
    }
}
