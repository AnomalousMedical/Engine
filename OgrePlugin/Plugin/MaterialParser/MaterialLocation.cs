using Engine;
using Engine.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class MaterialLocation : MaterialRepository
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
        private MaterialParserManager parent;
        List<MaterialEntry> loadedMaterials = new List<MaterialEntry>();
        HashSet<String> groups = new HashSet<string>();
        private String transformedName;

        static String transformName(String name)
        {
            return name.Replace('/', '\\').ToLowerInvariant();
        }

        public MaterialLocation(Engine.Resources.Resource resource, MaterialParserManager parent)
        {
            this.parent = parent;
            LocName = resource.LocName;
            Recursive = resource.Recursive;
            ArchiveType = resource.ArchiveType;
            loaded = false;
            transformedName = transformName(resource.LocName);
        }

        public void addMaterial(MaterialPtr material, MaterialDescription description)
        {
            loadedMaterials.Add(new MaterialEntry(material, description.Builder));
        }

        public String LocName { get; private set; }

        public bool Recursive { get; private set; }

        public String ArchiveType { get; private set; }

        internal void initializeResources(JsonSerializer serializer)
        {
            if(loaded && groups.Count == 0)
            {
                foreach (var mat in loadedMaterials)
                {
                    parent.destroyMaterial(mat.ptr, mat.builder);
                }
                loaded = false;
                loadedMaterials.Clear();
            }
            else if(!loaded && groups.Count > 0)
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
                            description.Group = groups.First();
                            currentName = description.Name;
                            parent.buildMaterial(description, this);
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

        internal void addGroup(ResourceGroup group)
        {
            groups.Add(group.FullName);
        }

        internal void removeGroup(ResourceGroup group)
        {
            groups.Remove(group.FullName);
        }

        internal bool represents(Engine.Resources.Resource resource)
        {
            return this.transformedName == transformName(resource.LocName);
        }

        public bool InGroup
        {
            get
            {
                return groups.Count > 0;
            }
        }

        private IEnumerable<MaterialDescription> getDescriptions(JsonSerializer serializer, String file)
        {
            List<MaterialDescription> descriptions = null;
            MaterialDescription mainDescription = null;
            //Try to read the stream in one shot
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
                    descriptions = serializer.Deserialize(sr, typeof(List<MaterialDescription>)) as List<MaterialDescription>;
                }
                else
                {
                    mainDescription = serializer.Deserialize(sr, typeof(MaterialDescription)) as MaterialDescription;
                }
            }

            //Enumerate results
            if(descriptions != null)
            {
                foreach (var description in descriptions)
                {
                    yield return description;
                    foreach(var variant in description.AllVariants)
                    {
                        yield return variant;
                    }
                }
            }
            else if (mainDescription != null)
            {
                yield return mainDescription;
                foreach (var variant in mainDescription.AllVariants)
                {
                    yield return variant;
                }
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
