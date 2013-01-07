using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace libRocketPlugin
{
    public class VirtualFileSystemFileInterface : ManagedFileInterface
    {
        private List<RocketFileSystemExtension> extensions = new List<RocketFileSystemExtension>();

        public override Stream Open(string path)
        {
            //Check the virtual file system
            if (VirtualFileSystem.Instance.exists(path))
            {
                return VirtualFileSystem.Instance.openStream(path, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read);
            }
            //Check the extensions
            else
            {
                foreach (RocketFileSystemExtension extension in extensions)
                {
                    if (extension.canOpenFile(path))
                    {
                        return extension.openFile(path);
                    }
                }
            }
            //If all else fails check the file system
            if (File.Exists(path))
            {
                return File.Open(path, FileMode.Open, FileAccess.Read);
            }
            return null;
        }

        public override bool Exists(string path)
        {
            if (VirtualFileSystem.Instance.exists(path))
            {
                return true;
            }
            else
            {
                foreach (RocketFileSystemExtension extension in extensions)
                {
                    if (extension.canOpenFile(path))
                    {
                        return true;
                    }
                }
            }
            //If all else fails check the file system
            return File.Exists(path);
        }

        public override bool Exists(String source, String sourcePath)
        {
            return Exists(Core.GetSystemInterface().JoinPath(sourcePath, source));
        }

        public override void addExtension(RocketFileSystemExtension extension)
        {
            extensions.Add(extension);
        }

        public override void removeExtension(RocketFileSystemExtension extension)
        {
            extensions.Remove(extension);
        }
    }
}
