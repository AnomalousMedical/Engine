using Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class MaterialLocation
    {
        class MaterialEntry
        {
            public MaterialEntry(MaterialPtr ptr, String builder)
            {
                this.ptr = ptr;
                this.builder = builder;
            }

            public MaterialPtr ptr;
            public String builder;
        }

        private bool loaded;
        private MaterialParserGroup parent;
        List<MaterialEntry> loadedMaterials = new List<MaterialEntry>();

        public MaterialLocation(Engine.Resources.Resource resource, MaterialParserGroup parent)
        {
            this.parent = parent;
            LocName = resource.LocName;
            Recursive = resource.Recursive;
            ArchiveType = resource.ArchiveType;
            loaded = false;
        }

        public String LocName { get; private set; }

        public bool Recursive { get; private set; }

        public String ArchiveType { get; private set; }

        internal void initializeResources(JsonSerializer serializer)
        {
            if(!loaded)
            {
                VirtualFileSystem vfs = VirtualFileSystem.Instance;
                foreach(var file in Files)
                {
                    String currentName = null;
                    try
                    {
                        using (StreamReader sr = new StreamReader(vfs.openStream(file, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)))
                        {
                            MaterialDescription description = serializer.Deserialize(sr, typeof(MaterialDescription)) as MaterialDescription;
                            description.SourceFile = file;
                            description.Group = parent.Name;
                            currentName = description.Name;
                            loadedMaterials.Add(new MaterialEntry(parent.buildMaterial(description), description.Builder));
                        }
                    }
                    catch(Exception ex)
                    {
                        Logging.Log.Error("Material '{0}' in file '{1}' could not be loaded.\n{2}: {3}", currentName, file, ex.GetType(), ex.Message);
                    }
                }
                loaded = true;
            }
        }

        internal void unloadResources()
        {
            if(loaded)
            {
                foreach(var mat in loadedMaterials)
                {
                    parent.destroyMaterial(mat.ptr, mat.builder);
                }
                loaded = false;
            }
        }

        private IEnumerable<String> Files
        {
            get
            {
                VirtualFileSystem vfs = VirtualFileSystem.Instance;
                if (vfs.isDirectory(LocName))
                {
                    var files = vfs.listFiles(LocName, "*.jsonmat", Recursive);
                    foreach (String path in files)
                    {
                        yield return path;
                    }
                }
                else
                {
                    if (Path.GetExtension(LocName).ToLowerInvariant() == ".jsonmat")
                    {
                        yield return LocName;
                    }
                }
            }
        }
    }
}
