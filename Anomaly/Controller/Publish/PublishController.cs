using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using System.IO;
using Engine;
using System.Diagnostics;
using Logging;

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
                processFile(additionalResources.next(), true);
            }
            foreach (String directory in recursiveDirectories)
            {
                processRecursiveDirectory(directory);
            }
        }

        public void copyResources(String targetDirectory, String archiveName, bool compress, bool obfuscate)
        {
            throw new NotImplementedException();
            //String originalDirectory = targetDirectory;
            //if (compress)
            //{
            //    targetDirectory += "/zipTemp";
            //}
            //int resourceRootSize = 0;
            //if (Resource.ResourceRoot != null)
            //{
            //    resourceRootSize = Path.GetFullPath(Resource.ResourceRoot).Length;
            //}
            //Log.Debug("Starting file copy to {0}", targetDirectory);
            //foreach (FileInfo sourceFile in files)
            //{
            //    String trimmedSource = sourceFile.File.Substring(resourceRootSize);
            //    String destination = targetDirectory + trimmedSource;
            //    String directory = Path.GetDirectoryName(destination);
            //    if (!Directory.Exists(directory))
            //    {
            //        Directory.CreateDirectory(directory);
            //    }
            //    Log.Debug("Copying {0} to {1}.", sourceFile.File, destination);
            //    File.Copy(sourceFile.File, destination, true);
            //}
            //if (compress)
            //{
            //    String zipFileName = originalDirectory + "\\" + archiveName + ".zip";
            //    Log.Debug("Starting compression to {0}", zipFileName);
            //    if (File.Exists(zipFileName))
            //    {
            //        File.Delete(zipFileName);
            //    }
            //    Process sevenZip = new Process();
            //    sevenZip.StartInfo.UseShellExecute = false;
            //    sevenZip.StartInfo.RedirectStandardOutput = true;
            //    sevenZip.StartInfo.FileName = AnomalyConfig.Tools.SevenZipExecutable;
            //    sevenZip.StartInfo.Arguments = "a -tzip \"" + zipFileName + "\" \"" + targetDirectory + "/*\"";
            //    sevenZip.Start();
            //    using (StreamReader reader = sevenZip.StandardOutput)
            //    {
            //        while (reader.Peek() != -1)
            //        {
            //            Log.Debug(reader.ReadLine());
            //        }
            //    }
            //    sevenZip.WaitForExit();
            //    Log.Debug("Finished compression to {0}", zipFileName);
            //    Directory.Delete(targetDirectory, true);

            //    if (obfuscate)
            //    {
            //        String obfuscateFileName = originalDirectory + "\\" + archiveName + ".dat";
            //        Log.Debug("Starting obfuscation to {0}", obfuscateFileName);
            //        if (File.Exists(obfuscateFileName))
            //        {
            //            File.Delete(obfuscateFileName);
            //        }

            //        byte[] buffer = new byte[4096];
            //        using (Stream source = File.Open(zipFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //        {
            //            using (Stream dest = File.Open(obfuscateFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            //            {
            //                int numRead;
            //                while ((numRead = source.Read(buffer, 0, buffer.Length)) > 0)
            //                {
            //                    for (int i = 0; i < numRead; ++i)
            //                    {
            //                        buffer[i] ^= 73;
            //                    }
            //                    dest.Write(buffer, 0, numRead);
            //                }
            //            }
            //        }
            //        Log.Debug("Finished obfuscation to {0}", obfuscateFileName);
            //        File.Delete(zipFileName);
            //    }
            //}
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
