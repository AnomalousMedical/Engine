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
            path = fixFileName(path);

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
            //Try to load a common resource, if this fails null will be returned.
            return CommonResources.Open(path);
        }

        public override bool Exists(string path)
        {
            path = fixFileName(path);

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
            //Try to load a common resource, if this fails false will be returned.
            return CommonResources.Exists(path);
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

        private static string fixFileName(string path)
        {
            if (path.StartsWith(RocketInterface.DefaultProtocol))
            {
                path = path.Substring(RocketInterface.DefaultProtocol.Length);
            }
            path = path.Replace('\\', '/');
            return path;
        }
    }
}
