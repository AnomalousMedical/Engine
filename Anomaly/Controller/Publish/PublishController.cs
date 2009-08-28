using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using System.IO;
using Engine;

namespace Anomaly
{
    class FileInfo
    {
        String file;
        String basePath;

        public FileInfo(String file, String basePath)
        {
            this.file = file;
            this.basePath = basePath;
        }

        public String File
        {
            get
            {
                return file;
            }
        }

        public String BasePath
        {
            get
            {
                return basePath;
            }
        }

        public override string ToString()
        {
            return file;
        }
    }

    class PublishController
    {
        //Use a dictionary to ensure that the files are unique the key is a lowercase mangled name and the value is the filename as returned.
        private HashSet<FileInfo> files = new HashSet<FileInfo>();
        HashSet<String> recursiveDirectories = new HashSet<string>();
        private AnomalyProject project;

        public PublishController(AnomalyProject project)
        {
            this.project = project;
        }

        public void scanResources(ResourceManager resourceManager)
        {
            files.Clear();
            recursiveDirectories.Clear();
            foreach (SubsystemResources subsystem in resourceManager.getSubsystemEnumerator())
            {
                foreach (ResourceGroup group in subsystem.getResourceGroupEnumerator())
                {
                    foreach (Resource resource in group.getResourceEnumerator())
                    {
                        processFile(resource.FullPath, resource.Recursive);
                    }
                }
            }
            ConfigIterator additionalResources = project.ResourceSection.AdditionalResources;
            additionalResources.reset();
            while (additionalResources.hasNext())
            {
                processFile(Resource.ResourceRoot + "/" + additionalResources.next(), true);
            }
            foreach (String directory in recursiveDirectories)
            {
                processRecursiveDirectory(directory);
            }
        }

        public void copyResources(String targetDirectory, bool flatten)
        {
            int resourceRootSize = 0;
            if (Resource.ResourceRoot != null)
            {
                resourceRootSize = Path.GetFullPath(Resource.ResourceRoot).Length;
            }
            foreach (FileInfo sourceFile in files)
            {
                String trimmedSource;
                if (flatten)
                {
                    trimmedSource = sourceFile.File.Substring(sourceFile.BasePath.Length);
                }
                else
                {
                    trimmedSource = sourceFile.File.Substring(resourceRootSize);
                }
                String destination = targetDirectory + trimmedSource;
                String directory = Path.GetDirectoryName(destination);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.Copy(sourceFile.File, destination, true);
            }
        }

        /// <summary>
        /// Get the list of files as pretty names, aka not mangled to all lower
        /// case for comparison. This is suitable for display.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileInfo> getPrettyFileList()
        {
            return files;
        }

        private void processFile(String url, bool recursive)
        {
            if (File.Exists(url))
            {
                addFile(url, Path.GetDirectoryName(url));
            }
            else if (Directory.Exists(url))
            {
                if (recursive)
                {
                    recursiveDirectories.Add(url);
                }
                else
                {
                    foreach (String file in Directory.GetFiles(url))
                    {
                        addFile(file, url);
                    }
                }
            }
        }

        private void addFile(String file, String baseDir)
        {
            if (!file.EndsWith("~") && !file.Contains(".svn"))
            {
                files.Add(new FileInfo(file, baseDir));
            }
        }

        private void processRecursiveDirectory(String directory)
        {
            foreach (String file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                addFile(file, directory);
            }
        }
    }
}
