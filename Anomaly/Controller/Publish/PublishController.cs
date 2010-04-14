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
    class PublishController
    {
        //Use a dictionary to ensure that the files are unique the key is a lowercase mangled name and the value is the filename as returned.
        private List<VirtualFileInfo> files = new List<VirtualFileInfo>();
        HashSet<String> recursiveDirectories = new HashSet<string>();
        private Solution solution;

        public PublishController(Solution solution)
        {
            this.solution = solution;
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
            foreach (ExternalResource additionalResource in solution.AdditionalResources)
            {
                processFile(additionalResource.Path, true);
            }
            foreach (String directory in recursiveDirectories)
            {
                processRecursiveDirectory(directory);
            }
        }

        public void copyResources(String targetDirectory, String archiveName, bool compress, bool obfuscate)
        {
            String originalDirectory = targetDirectory;
            if (compress)
            {
                targetDirectory += "/zipTemp";
            }
            Log.Debug("Starting file copy to {0}", targetDirectory);
            foreach (VirtualFileInfo sourceFile in files)
            {
                String destination = targetDirectory + Path.DirectorySeparatorChar + sourceFile.FullName;
                String directory = Path.GetDirectoryName(destination);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                Log.Debug("Copying {0} to {1}.", sourceFile.RealLocation, destination);
                File.Copy(sourceFile.RealLocation, destination, true);
            }
            if (compress)
            {
                String zipFileName = originalDirectory + "\\" + archiveName + ".zip";
                Log.Debug("Starting compression to {0}", zipFileName);
                if (File.Exists(zipFileName))
                {
                    File.Delete(zipFileName);
                }
                Process sevenZip = new Process();
                sevenZip.StartInfo.UseShellExecute = false;
                sevenZip.StartInfo.RedirectStandardOutput = true;
                sevenZip.StartInfo.FileName = AnomalyConfig.Tools.SevenZipExecutable;
                sevenZip.StartInfo.Arguments = "a -tzip \"" + zipFileName + "\" \"" + targetDirectory + "/*\"";
                sevenZip.Start();
                using (StreamReader reader = sevenZip.StandardOutput)
                {
                    while (reader.Peek() != -1)
                    {
                        Log.Debug(reader.ReadLine());
                    }
                }
                sevenZip.WaitForExit();
                Log.Debug("Finished compression to {0}", zipFileName);
                Directory.Delete(targetDirectory, true);

                if (obfuscate)
                {
                    String obfuscateFileName = originalDirectory + "\\" + archiveName + ".dat";
                    obfuscateZipFile(zipFileName, obfuscateFileName);
                    File.Delete(zipFileName);
                }
            }
        }

        public static void obfuscateZipFile(String zipFileName, String obfuscateFileName)
        {
            Log.Debug("Starting obfuscation to {0}", obfuscateFileName);
            if (File.Exists(obfuscateFileName))
            {
                File.Delete(obfuscateFileName);
            }

            byte[] buffer = new byte[4096];
            using (Stream source = File.Open(zipFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (Stream dest = File.Open(obfuscateFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    int numRead;
                    while ((numRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < numRead; ++i)
                        {
                            buffer[i] ^= 73;
                        }
                        dest.Write(buffer, 0, numRead);
                    }
                }
            }
            Log.Debug("Finished obfuscation to {0}", obfuscateFileName);
        }

        /// <summary>
        /// Get the list of files as pretty names, aka not mangled to all lower
        /// case for comparison. This is suitable for display.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VirtualFileInfo> getPrettyFileList()
        {
            return files;
        }

        private void processFile(String url, bool recursive)
        {
            VirtualFileSystem vfs = VirtualFileSystem.Instance;
            if (vfs.isDirectory(url))
            {
                if (recursive)
                {
                    recursiveDirectories.Add(url);
                }
                else
                {
                    foreach (String file in vfs.listFiles(url, false))
                    {
                        addFile(vfs.getFileInfo(file));
                    }
                }
            }
            else
            {
                addFile(vfs.getFileInfo(url));                
            }
        }

        private void addFile(VirtualFileInfo fileInfo)
        {
            if (!fileInfo.Name.EndsWith("~") && !fileInfo.DirectoryName.Contains(".svn"))
            {
                files.Add(fileInfo);
            }
        }

        private void processRecursiveDirectory(String directory)
        {
            foreach (String file in VirtualFileSystem.Instance.listFiles(directory, true))
            {
                addFile(VirtualFileSystem.Instance.getFileInfo(file));
            }
        }
    }
}
