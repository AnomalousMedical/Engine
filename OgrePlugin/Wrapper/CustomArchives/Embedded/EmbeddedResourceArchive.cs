using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Engine;
using System.IO;
using System.Text.RegularExpressions;

namespace OgreWrapper
{
    class EmbeddedFileInfo
    {
        public long Size { get; set; }

        public String BaseName { get; set; }
        
        public String FileName { get; set; }
        
        public String Path { get; set; }
    }

    class EmbeddedResourceArchive : OgreManagedArchive
    {
        Assembly assembly;
        List<EmbeddedFileInfo> fileList = new List<EmbeddedFileInfo>();

        public EmbeddedResourceArchive(String name, String archType)
            :base(name, archType)
        {
            
        }

        protected internal override void load()
        {
            assembly = Type.GetType(name).Assembly();
	        String[] fileList = assembly.GetManifestResourceNames();
	        foreach(String file in fileList)
	        {
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    EmbeddedFileInfo fileInfo = new EmbeddedFileInfo();
                    fileInfo.Size = stream.Length;
                    fileInfo.BaseName = Path.GetFileName(file);
                    fileInfo.FileName = file;
                    fileInfo.Path = Path.GetDirectoryName(file);
                    this.fileList.Add(fileInfo);
                }
	        }
        }

        protected internal override void unload()
        {
            assembly = null;
            fileList.Clear();
        }

        protected internal override System.IO.Stream doOpen(string filename)
        {
            return assembly.GetManifestResourceStream(filename);
        }

        protected internal override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            foreach(EmbeddedFileInfo i in fileList)
            {
                OgreStringVector_push_back(ogreStringVector, i.FileName);
            }
        }

        protected internal override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            foreach (EmbeddedFileInfo i in fileList)
            {
                OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
            }
        }

        protected internal override void dofind(String pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains("/") || pattern.Contains("\\");
            foreach(EmbeddedFileInfo i in fileList)
            {
                if(r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreStringVector_push_back(ogreStringVector, i.FileName);
                }
            }
        }

        protected internal override void dofindFileInfo(String pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains("/") || pattern.Contains("\\");
            foreach (EmbeddedFileInfo i in fileList)
            {
                if (r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
                }
            }
        }

        protected internal override bool exists(string filename)
        {
            return !String.IsNullOrEmpty(filename) && assembly.GetManifestResourceInfo(filename) != null;
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
