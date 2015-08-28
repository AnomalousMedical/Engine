using Engine;
using Engine.Editing;
using Engine.Resources;
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


        private EditInterface editInterface;
        private PluginManager pluginManager;
        private List<MaterialDescription> currentDescriptions = new List<MaterialDescription>();
        private String currentRealRootDirectory;

        public MaterialController(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;

            editInterface = ReflectedEditInterface.createEditInterface(this, "Materials");
            editInterface.createEditInterfaceManager<MaterialDescription>();
        }

        public void loadMaterials(String vfsPath, String currentRealRootDirectory)
        {
            this.currentRealRootDirectory = currentRealRootDirectory;

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
            String outputFile = Path.Combine(currentRealRootDirectory, "woot.txt");
            //String outputFile = Path.Combine(currentRealRootDirectory, desc.SourceFile);

            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            using (StreamWriter textWriter = new StreamWriter(outputFile, false))
            {
                serializer.Serialize(textWriter, this);
            }

                //HashSet<String> seenSourceFiles = new HashSet<string>();
                //foreach (var desc in currentDescriptions)
                //{
                //    String outputFile = Path.Combine(currentRealRootDirectory, "woot.txt");
                //    //String outputFile = Path.Combine(currentRealRootDirectory, desc.SourceFile);

                //    bool newFile = seenSourceFiles.Contains(outputFile);
                //    using (StreamWriter textWriter = new StreamWriter(outputFile, newFile))
                //    {
                //        if (newFile)
                //        {
                //            textWriter.Write("[\n");
                //        }
                //        serializer.Serialize(textWriter, desc);
                //        textWriter.Write(",\n");
                //        seenSourceFiles.Add(outputFile);
                //    }
                //}
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
                    serializer.Serialize(writer, desc);
                }

                writer.WriteEndArray();
            }
        }
    }
}
