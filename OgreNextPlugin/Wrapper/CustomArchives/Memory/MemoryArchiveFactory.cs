using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreNextPlugin
{
    public class MemoryArchiveFactory : OgreManagedArchiveFactory
    {
        private static MemoryArchiveFactory instance = null;

        public static MemoryArchiveFactory Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<String, MemoryArchive> archives = new Dictionary<string, MemoryArchive>();

        internal MemoryArchiveFactory()
            : base("Memory")
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Do not create more than one MemoryArchiveFactory.");
            }
        }

        public MemoryArchive getArchive(String name)
        {
            MemoryArchive archive;
            archives.TryGetValue(name, out archive);
            return archive;
        }

        protected override OgreManagedArchive doCreateInstance(string name)
        {
            return new MemoryArchive(name, "Memory", this);
        }

        internal void archiveLoaded(MemoryArchive archive)
        {
            archives.Add(archive.ArchiveName, archive);
        }

        internal void archiveUnloaded(MemoryArchive archive)
        {
            archives.Remove(archive.ArchiveName);
        }
    }
}
