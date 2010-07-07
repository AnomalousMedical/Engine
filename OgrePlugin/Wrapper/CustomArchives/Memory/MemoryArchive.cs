using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace OgreWrapper
{
    abstract class MemoryFileInfo
    {
        public abstract long Size { get; }

        public abstract String BaseName { get; }

        public abstract String FileName { get; }

        public abstract String Path { get; }

        public abstract Stream openStream();
    }

    class MemoryStringInfo : MemoryFileInfo
    {
        private String memStr;
        private long size;
        private String baseName;
        private String fileName;
        private String path;

        public MemoryStringInfo(String resourceName, String archivePath, String memString)
        {
            this.memStr = memString;
            baseName = archivePath + resourceName;
            fileName = resourceName;
            path = archivePath;
            using (Stream stream = openStream())
            {
                size = stream.Length;
            }
        }

        public override long Size
        {
            get
            {
                return size;
            }
        }

        public override string BaseName
        {
            get
            {
                return baseName;
            }
        }

        public override string FileName
        {
            get
            {
                return fileName;
            }
        }

        public override string Path
        {
            get
            {
                return path;
            }
        }

        public override Stream openStream()
        {
            return new MemoryStream(ASCIIEncoding.Default.GetBytes(memStr));
        }
    }

    /// <summary>
    /// This class allows ogre to open stuff from memory.
    /// </summary>
    public class MemoryArchive : OgreManagedArchive
    {
        MemoryArchiveFactory factory;
        Dictionary<String, MemoryFileInfo> fileList = new Dictionary<String, MemoryFileInfo>();

        public MemoryArchive(String name, String archType, MemoryArchiveFactory factory)
            :base(name, archType)
        {
            this.factory = factory;
        }

        public void addStringResource(String resourceName, String memString)
        {
            MemoryStringInfo strInfo = new MemoryStringInfo(resourceName, name, memString);
            fileList.Add(strInfo.BaseName, strInfo);
        }

        public void removeStringResource(String resourceName)
        {
            fileList.Remove(resourceName);
        }

        protected override void load()
        {
            factory.archiveLoaded(this);
        }

        protected override void unload()
        {
            factory.archiveUnloaded(this);
        }

        protected override Stream doOpen(string filename)
        {
            return fileList[filename].openStream();
        }

        protected override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            foreach(MemoryFileInfo i in fileList.Values)
            {
                OgreStringVector_push_back(ogreStringVector, i.FileName);
            }
        }

        protected override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            foreach (MemoryFileInfo i in fileList.Values)
            {
                OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
            }
        }

        protected override void dofind(String pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains('/') || pattern.Contains('\\');
            foreach (MemoryFileInfo i in fileList.Values)
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
            foreach (MemoryFileInfo i in fileList.Values)
            {
                if (r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
                }
            }
        }

        protected override bool exists(string filename)
        {
            return fileList.ContainsKey(filename);
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
