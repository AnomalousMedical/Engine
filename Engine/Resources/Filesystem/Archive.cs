using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine.Resources
{ 
    abstract class Archive : IDisposable
    {
        public abstract void Dispose();

        public abstract bool containsRealAbsolutePath(String path);

        public abstract bool isArchiveFor(String path);

        public abstract IEnumerable<String> listFiles(bool recursive);

        public abstract IEnumerable<String> listFiles(String url, bool recursive);
                        
        public abstract IEnumerable<String> listFiles(String url, String searchPattern, bool recursive);
                        
        public abstract IEnumerable<String> listDirectories(bool recursive);
                        
        public abstract IEnumerable<String> listDirectories(String url, bool recursive);
                        
        public abstract IEnumerable<String> listDirectories(String url, bool recursive, bool includeHidden);
                        
        public abstract IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive);
                        
        public abstract IEnumerable<String> listDirectories(String url, String searchPattern, bool recursive, bool includeHidden);

        public abstract Stream openStream(String url, FileMode mode);

        public abstract Stream openStream(String url, FileMode mode, FileAccess access);

        public abstract Stream openStream(String url, FileMode mode, FileAccess access, FileShare share);

        public abstract bool isDirectory(String url);

        public abstract VirtualFileInfo getFileInfo(String filename);
    }
}
