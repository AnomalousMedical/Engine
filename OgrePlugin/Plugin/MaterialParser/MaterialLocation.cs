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
                foreach(String file in Files)
                {
                    Logging.Log.Info("Loading materials from {0}", file);
                    String currentName = null;
                    try
                    {
                        foreach (var description in getDescriptions(serializer, file))
                        {
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

        private IEnumerable<MaterialDescription> getDescriptions(JsonSerializer serializer, String file)
        {
            using (StreamReader sr = new StreamReader(VirtualFileSystem.Instance.openStream(file, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)))
            {
                char firstChar = (char)sr.Read();
                while (char.IsWhiteSpace(firstChar))
                {
                    firstChar = (char)sr.Read();
                }
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                sr.DiscardBufferedData();
                if(firstChar == '[')
                {
                    List<MaterialDescription> descriptions = serializer.Deserialize(sr, typeof(List<MaterialDescription>)) as List<MaterialDescription>;
                    foreach (var description in descriptions)
                    {
                        yield return description;
                    }
                }
                else
                {
                    MaterialDescription descripton = serializer.Deserialize(sr, typeof(MaterialDescription)) as MaterialDescription;
                    if (descripton != null)
                    {
                        yield return descripton;
                    }
                }
                
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
