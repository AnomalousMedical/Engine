using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Anomalous.Minimus
{
    /// <summary>
    /// Provides a common shareable way of finding folders.
    /// </summary>
    public static class FolderFinder
    {
        private static String userRoot;
        private static String localDataFolder;
        private static String localPrivateDataFolder;
        private static String programFolder = null;

        public static void setProgramName(String programName)
        {
            userRoot = Path.Combine(PlatformConfig.LocalUserDocumentsFolder, programName);
            localDataFolder = Path.Combine(PlatformConfig.LocalDataFolder, programName);
            localPrivateDataFolder = Path.Combine(PlatformConfig.LocalPrivateDataFolder, programName);
        }

        /// <summary>
        /// A folder that user documents and settings can be put into.
        /// </summary>
        public static String LocalUserDocumentsFolder
        {
            get
            {
                return userRoot;
            }
        }

        /// <summary>
        /// A non roaming folder that larger data files (like plugins and program downloads) can be put into.
        /// </summary>
        public static String LocalDataFolder
        {
            get
            {
                return localDataFolder;
            }
        }

        /// <summary>
        /// A non roaming folder that data that should be kept private (like license files) can be put into.
        /// </summary>
        public static String LocalPrivateDataFolder
        {
            get
            {
                return localPrivateDataFolder;
            }
        }

        /// <summary>
        /// The folder the current executable is running under.
        /// </summary>
        public static String ExecutableFolder
        {
            get
            {
                if (programFolder == null)
                {
                    String[] args = Environment.GetCommandLineArgs();
                    if (args.Length > 0)
                    {
                        programFolder = Path.GetDirectoryName(args[0]);
                    }
                    else
                    {
                        programFolder = Path.GetFullPath(".");
                    }
                }
                return programFolder;
            }
        }
    }
}
