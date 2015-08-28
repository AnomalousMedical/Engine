using Engine;
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
        private PluginManager pluginManager;
        private List<MaterialDescription> currentDescriptions = new List<MaterialDescription>();
        private String vfsRootPath;
        private String vfsPath;
        private String currentOutputFile;

        public MaterialController(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;

            editInterface = ReflectedEditInterface.createEditInterface(this, "Materials");
            editInterface.createEditInterfaceManager<MaterialDescription>();
        }

        public void loadMaterials(String vfsPath, String vfsRootPath)
        {
            this.vfsRootPath = vfsRootPath;
            this.vfsPath = vfsPath;

            var listener = new MaterialResourceListener(this);

            var resourceManager = pluginManager.createResourceManagerForListener("MaterialController", listener);
            var subsystem = resourceManager.getSubsystemResource("MaterialController");
            var group = subsystem.addResourceGroup("MaterialController");
            group.addResource(vfsPath, "EngineArchive", true);

            resourceManager.initializeResources();
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
                    String outputFile = Path.Combine(vfsRootPath, desc.SourceFile);

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    using (StreamWriter textWriter = new StreamWriter(outputFile, false))
                    {
                        serializer.Serialize(textWriter, this);
                    }
                    wroteSomething = true;
                }
            }
            if(!wroteSomething && currentDescriptions.Count > 0)
            {
                InputBox.GetInput("Save Materials", "Enter a name for the material file.", true, nameMaterialResult);
            }
        }

        bool nameMaterialResult(String result, ref String errorPrompt)
        {
            //This only happens if no descs have output files and there are actually output files if(!wroteSomething && currentDescriptions.Count > 0)
            currentDescriptions[0].SourceFile = Path.Combine(vfsPath, result);
            saveMaterials();
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
