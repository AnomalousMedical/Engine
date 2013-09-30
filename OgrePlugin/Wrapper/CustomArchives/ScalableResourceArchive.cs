using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OgreWrapper
{
    class ScalableResourceArchive : OgreManagedArchive
    {
        private static readonly Regex scaledFileRegex = new Regex("@\\d+%\\."); //Matches things like @200%. (including the .)

        OgreManagedArchive wrappedArchive;

        public ScalableResourceArchive(OgreManagedArchive wrappedArchive, String name, String archType)
            :base(name, archType)
        {
            this.wrappedArchive = wrappedArchive;
        }

        public override void Dispose()
        {
            wrappedArchive.Dispose();
            base.Dispose();
        }

        protected internal override void load()
        {
            wrappedArchive.load();
        }

        protected internal override void unload()
        {
            wrappedArchive.unload();
        }

        protected internal override Stream doOpen(string filename)
        {
            //Scaling is just normal return file as is
            if (ScaleHelper.ScaleFactor < 1.05f)
            {
                return wrappedArchive.doOpen(filename);
            }

            //Already has scaled string, just return as is
            if (scaledFileRegex.IsMatch(filename))
            {
                return wrappedArchive.doOpen(filename);
            }

            //Check for extension, if no extension found, return file as is
            int extIndex = filename.LastIndexOf('.');
            if (extIndex == -1)
            {
                return wrappedArchive.doOpen(filename);
            }

            //Extract file name info
            String baseName = filename.Substring(0, extIndex);
            String extension = filename.Substring(extIndex);
            String openFileName;

            //Check for 150% file
            if (ScaleHelper.ScaleFactor < 1.55f)
            {
                openFileName = String.Format("{0}@150%{1}", baseName, extension);
                if (wrappedArchive.exists(openFileName))
                {
                    return wrappedArchive.doOpen(openFileName);
                }
            }

            //150% does not exist try 200%
            openFileName = String.Format("{0}@200%{1}", baseName, extension);
            if (wrappedArchive.exists(openFileName))
            {
                return wrappedArchive.doOpen(openFileName);
            }

            //Ultimate fallback open original file
            return wrappedArchive.doOpen(filename);
        }

        protected internal override void doList(bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            wrappedArchive.doList(recursive, dirs, ogreStringVector);
        }

        protected internal override void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            wrappedArchive.doListFileInfo(recursive, dirs, ogreFileList, archive);
        }

        protected internal override void dofind(string pattern, bool recursive, bool dirs, IntPtr ogreStringVector)
        {
            wrappedArchive.dofind(pattern, recursive, dirs, ogreStringVector);
        }

        protected internal override void dofindFileInfo(string pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive)
        {
            wrappedArchive.dofindFileInfo(pattern, recursive, dirs, ogreFileList, archive);
        }

        protected internal override bool exists(string filename)
        {
            return wrappedArchive.exists(filename);
        }
    }
}
