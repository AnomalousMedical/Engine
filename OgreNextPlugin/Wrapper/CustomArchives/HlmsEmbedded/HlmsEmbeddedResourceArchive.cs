using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Engine;
using System.IO;
using System.Text.RegularExpressions;

namespace OgreNextPlugin
{
    class HlmsEmbeddedFileInfo
    {
        public long Size { get; set; }

        public String BaseName { get; set; }

        public String FileName { get; set; }

        public String Path { get; set; }

        public String EmbeddedResourcePath { get; set; }
    }

    /// <summary>
    /// This class provides an archive for resources embedded into an assembly. You specify the name of a type in the assembly
    /// and this class will find it. You can optionally add a double pipe (||) after the assembly name with the prefix of
    /// the name of the resource to use as a filter.
    /// </summary>
    partial class HlmsEmbeddedResourceArchive : OgreManagedArchive
    {
        private static Dictionary<String, HlmsEmbeddedFileInfo> fileMap = new Dictionary<string, HlmsEmbeddedFileInfo>();

        static HlmsEmbeddedResourceArchive()
        {
            SetupFileMap();
        }

        Assembly assembly;
        IEnumerable<HlmsEmbeddedFileInfo> fileList;
        String basePath;

        public HlmsEmbeddedResourceArchive(String name, String archType)
            :base(name, archType)
        {
            
        }

        protected internal override void load()
        {
            assembly = EmbeddedResourceArchive.GetAssemblyAndArgs(name, out basePath);
            fileList = fileMap.Values;
            if(!String.IsNullOrEmpty(basePath))
            {
                basePath = basePath.Replace('\\', '/');
                if(basePath[basePath.Length -1] != '/')
                {
                    basePath += "/";
                }
                fileList = fileList.Where((f) => f.FileName.StartsWith(basePath));
            }
            else
            {
                basePath = "";
            }
        }

        protected internal override void unload()
        {
            assembly = null;
            fileList = null;
        }

        protected internal override System.IO.Stream doOpen(string filename)
        {
            //This is not yet tested, since ogre is not calling it yet.
            //Other stuff works
            filename = fixFilename(filename);
            var file = fileMap[filename];
            return assembly.GetManifestResourceStream(file.EmbeddedResourcePath);
        }

        protected internal override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            foreach(var i in fileList)
            {
                OgreStringVector_push_back(ogreStringVector, i.FileName);
            }
        }

        protected internal override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            //Not yet tested, but should be ok
            foreach (var i in fileList)
            {
                OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
            }
        }

        protected internal override void dofind(String pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            //This is probably not right, hopefully this can be dropped
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains("/") || pattern.Contains("\\");
            foreach(var i in fileList)
            {
                if(r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreStringVector_push_back(ogreStringVector, i.FileName);
                }
            }
        }

        protected internal override void dofindFileInfo(String pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            //This is probably not right, hopefully this can be dropped
            Regex r = new Regex(wildcardToRegex(pattern));
            bool fullMatch = pattern.Contains("/") || pattern.Contains("\\");
            foreach (var i in fileList)
            {
                if (r.Match(fullMatch ? i.FileName : i.BaseName).Success)
                {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(i.Size), new IntPtr(i.Size), i.BaseName, i.FileName, i.Path);
                }
            }
        }

        protected internal override bool exists(string filename)
        {
            filename = fixFilename(filename);
            return !String.IsNullOrEmpty(filename) && fileMap.ContainsKey(filename);
        }

        protected string fixFilename(String fileName)
        {
            return basePath + fileName;
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
