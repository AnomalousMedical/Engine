using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Engine;

namespace OgreNextPlugin
{
    class OgreEngineArchive : OgreManagedArchive
    {
        private String baseName;
        private VirtualFileSystem vfs;

        public OgreEngineArchive(String name, String archType)
            :base(name, archType)
        {
            baseName = name;
        }

        protected internal override void load()
        {
            vfs = VirtualFileSystem.Instance;
        }

        protected internal override void unload()
        {
            vfs = null;
        }

        protected internal override Stream doOpen(string filename)
        {
            if (!vfs.exists(filename))
            {
                //This is technically wrong, ogre is suppost to supply filenames fully qualified, but our setup doesn't always so we have this crutch
                filename = baseName + "/" + filename;
            }
            return vfs.openStream(filename, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read);
        }

        protected internal override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            IEnumerable<String> files;
	        if(dirs)
	        {
		        files = vfs.listDirectories(baseName, recursive);
	        }
	        else
	        {
		        files = vfs.listFiles(baseName, recursive);
	        }
	        foreach(String file in files)
	        {
                OgreStringVector_push_back(ogreStringVector, file);
	        }
        }

        protected internal override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            IEnumerable<String> files;
            if(dirs)
	        {
                files = vfs.listDirectories(baseName, recursive);
		        foreach(String file in files)
		        {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(-1), new IntPtr(-1), Path.GetFileName(file), file, Path.GetDirectoryName(file));
		        }
	        }
	        else
	        {
                files = vfs.listFiles(baseName, recursive);
		        foreach(String file in files)
		        {
                    pushFixedFileInfo(file, ogreFileList, archive);
		        }
	        }
        }

        protected internal override void dofind(string pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            IEnumerable<String> files;
            if(dirs)
	        {
		        files = vfs.listDirectories(baseName, pattern, recursive);
	        }
	        else
	        {
                files = vfs.listFiles(baseName, pattern, recursive);
	        }
	        foreach(String file in files)
	        {
		        OgreStringVector_push_back(ogreStringVector, file);
	        }
        }

        protected internal override void dofindFileInfo(string pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            IEnumerable<String> files;
            if (dirs)
            {
                files = vfs.listDirectories(baseName, pattern, recursive);
                foreach (String file in files)
                {
                    OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(-1), new IntPtr(-1), Path.GetFileName(file), file, Path.GetDirectoryName(file));
                }
            }
            else
            {
                files = vfs.listFiles(baseName, pattern, recursive);
                foreach (String file in files)
                {
                    pushFixedFileInfo(file, ogreFileList, archive);
                }
            }
        }

        protected internal void pushFixedFileInfo(String file, IntPtr ogreFileList, IntPtr archive)
        {
            VirtualFileInfo archiveInfo = vfs.getFileInfo(file);
            String fixedFilename = baseName.Length > 0 ? archiveInfo.FullName.Replace(baseName, "") : archiveInfo.FullName;
            if(fixedFilename.StartsWith("/"))
            {
                fixedFilename = fixedFilename.Substring(1);
            }
            String fixedPath = baseName.Length > 0 ? archiveInfo.DirectoryName.Replace(baseName, "") : archiveInfo.DirectoryName;
            if(fixedPath.StartsWith("/"))
            {
                fixedPath = fixedPath.Substring(1);
            }
            OgreFileInfoList_push_back(ogreFileList, archive, new IntPtr(archiveInfo.CompressedSize), new IntPtr(archiveInfo.UncompressedSize), archiveInfo.Name, fixedFilename, fixedPath);
        }

        protected internal override bool exists(string filename)
        {
            //The baseName + "/" + filename check is technically invalid, however, this archive supports filenames that are not "full" so we include both checks.
            return !String.IsNullOrEmpty(filename) && (vfs.exists(filename) || vfs.exists(baseName + "/" + filename));
        }
    }
}
