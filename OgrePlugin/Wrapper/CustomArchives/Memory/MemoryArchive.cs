using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Logging;

namespace OgreWrapper
{
    class MemoryStreamInfo : IDisposable
    {
        private MemoryStream stream;
        private long size;
        private String baseName;
        private String fileName;
        private String path;

        public MemoryStreamInfo(String resourceName, String archivePath, MemoryStream stream)
        {
            this.stream = stream;
            baseName = resourceName;
            fileName = archivePath + resourceName;
            path = archivePath;
            size = stream.Length;
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public long Size
        {
            get
            {
                return size;
            }
        }

        public string BaseName
        {
            get
            {
                return baseName;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public Stream openStream()
        {
            MemoryStream newStream = new MemoryStream((int)stream.Length);
            stream.WriteTo(newStream);
            newStream.Position = 0;
            return newStream;
        }
    }

    /// <summary>
    /// This class allows ogre to open stuff from memory.
    /// </summary>
    public class MemoryArchive : OgreManagedArchive
    {
        MemoryArchiveFactory factory;
        Dictionary<String, MemoryStreamInfo> fileList = new Dictionary<String, MemoryStreamInfo>();

        public MemoryArchive(String name, String archType, MemoryArchiveFactory factory)
            :base(name, archType)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Add a stream resource to this archive. The stream passed will NOT be
        /// copied and the archive will take ownership of it for disposal. So
        /// you can add a stream and do not have to worry about disposing it.
        /// However, do not dispose it or it will break that resource in the
        /// archive as well.
        /// </summary>
        /// <param name="resourceName">The name of the stream resource.</param>
        /// <param name="stream">The MemoryStream of the resource.</param>
        public void addMemoryStreamResource(String resourceName, MemoryStream stream)
        {
            MemoryStreamInfo memStrInfo = new MemoryStreamInfo(resourceName, name, stream);
            fileList.Add(resourceName, memStrInfo);
        }

        /// <summary>
        /// Destroy a stream held by this MemoryArchive and deletes it out of
        /// the archive. Will destroy the underlying stream and it must be
        /// created and readded to be used again.
        /// </summary>
        /// <param name="resourceName">The name of the resource to destroy.</param>
        public void destroyMemoryStreamResource(String resourceName)
        {
            MemoryStreamInfo info;
            fileList.TryGetValue(resourceName, out info);
            if (info != null)
            {
                info.Dispose();
                fileList.Remove(resourceName);
            }
            else
            {
                Log.Warning("Attempted to erase resource {0} out of MemoryArchive {1} that does not exist. No changes made.", resourceName, name);
            }
        }

        protected override void load()
        {
            factory.archiveLoaded(this);
        }

        protected override void unload()
        {
            foreach (MemoryStreamInfo info in fileList.Values)
            {
                info.Dispose();
            }
            fileList.Clear();
            factory.archiveUnloaded(this);
        }

        protected override Stream doOpen(string filename)
        {
            filename = filename.Substring(name.Length);
            return fileList[filename].openStream();
        }

        protected override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            foreach (MemoryStreamInfo i in fileList.Values)
            {
                OgreStringVector_push_back(ogreStringVector, i.FileName);
            }
        }

        protected override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            foreach (MemoryStreamInfo i in fileList.Values)
            {
                OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
            }
        }

        protected override void dofind(String pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains('/') || pattern.Contains('\\');
            foreach (MemoryStreamInfo i in fileList.Values)
            {
                if(r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreStringVector_push_back(ogreStringVector, i.FileName);
                }
            }
        }

        protected override void dofindFileInfo(String pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains('/') || pattern.Contains('\\');
            foreach (MemoryStreamInfo i in fileList.Values)
            {
                if (r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName.Replace(name, ""), i.Path.Replace(name, ""));
                }
            }
        }

        protected override bool exists(string filename)
        {
            if (filename.StartsWith(name))
            {
                filename = filename.Substring(name.Length);
                return fileList.ContainsKey(filename);
            }
            else
            {
                return false;
            }
        }

        internal String ArchiveName
        {
            get
            {
                return name;
            }
        }

        private String wildcardToRegex(String wildcard)
        {
            String expression = "^";
            for (int i = 0; i < wildcard.Length; i++)
            {
                switch (wildcard[i])
                {
                    case '?':
                        expression += '.'; // regular expression notation.
                        //mIsWild = true;
                        break;
                    case '*':
                        expression += ".*";
                        //mIsWild = true;
                        break;
                    case '.':
                        expression += "\\";
                        expression += wildcard[i];
                        break;
                    case ';':
                        expression += "|";
                        //mIsWild = true;
                        break;
                    default:
                        expression += wildcard[i];
                        break;
                }
            }
            return expression;
        }
    }
}
