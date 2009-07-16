using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using System.IO;

namespace Anomaly
{
    class PublishController
    {
        //Use a dictionary to ensure that the files are unique the key is a lowercase mangled name and the value is the filename as returned.
        private HashSet<String> files = new HashSet<String>();

        public PublishController()
        {

        }

        public void scanResources(ResourceManager resourceManager)
        {
            files.Clear();
            HashSet<String> recursiveDirectories = new HashSet<string>();
            foreach (SubsystemResources subsystem in resourceManager.getSubsystemEnumerator())
            {
                foreach (ResourceGroup group in subsystem.getResourceGroupEnumerator())
                {
                    foreach (Resource resource in group.getResourceEnumerator())
                    {
                        if (File.Exists(resource.FullPath))
                        {
                            processFile(resource.FullPath);
                        }
                        else if (Directory.Exists(resource.FullPath))
                        {
                            if (resource.Recursive)
                            {
                                recursiveDirectories.Add(resource.FullPath);
                            }
                            else
                            {
                                foreach (String file in Directory.GetFiles(resource.FullPath))
                                {
                                    processFile(file);
                                }
                            }
                        }
                    }
                }
            }
            foreach (String directory in recursiveDirectories)
            {
                processRecursiveDirectory(directory);
            }
        }

        public void copyResources(String targetDirectory)
        {
            int resourceRootSize = 0;
            if (Resource.ResourceRoot != null)
            {
                resourceRootSize = Path.GetFullPath(Resource.ResourceRoot).Length;
            }
            foreach (String sourceFile in files)
            {
                String trimmedSource = sourceFile.Substring(resourceRootSize);
                String destination = targetDirectory + trimmedSource;
                String directory = Path.GetDirectoryName(destination);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.Copy(sourceFile, destination, true);
            }
        }

        /// <summary>
        /// Get the list of files as pretty names, aka not mangled to all lower
        /// case for comparison. This is suitable for display.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> getPrettyFileList()
        {
            return files;
        }

        private void processFile(String file)
        {
            if (!file.EndsWith("~"))
            {
                files.Add(file);
            }
        }

        private void processRecursiveDirectory(String directory)
        {
            foreach (String file in Directory.GetFiles(directory))
            {
                processFile(file);
            }
            foreach (String subDir in Directory.GetDirectories(directory))
            {
                if ((File.GetAttributes(subDir) & FileAttributes.Hidden) == 0)
                {
                    processRecursiveDirectory(subDir);
                }
            }
        }
    }
}
