using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    /// <summary>
    /// Provides platform specific runtime info.
    /// </summary>
    public abstract class RuntimePlatformInfo
    {
        /// <summary>
        /// A folder that user documents and settings can be put into.
        /// </summary>
        public static String LocalUserDocumentsFolder
        {
            get
            {
                return Instance.LocalUserDocumentsFolderImpl;
            }
        }

        /// <summary>
        /// A non roaming folder that larger data files (like plugins and program downloads) can be put into.
        /// </summary>
        public static String LocalDataFolder
        {
            get
            {
                return Instance.LocalDataFolderImpl;
            }
        }

        /// <summary>
        /// A non roaming folder that data that should be kept private (like license files) can be put into.
        /// </summary>
        public static String LocalPrivateDataFolder
        {
            get
            {
                return Instance.LocalPrivateDataFolderImpl;
            }
        }

        /// <summary>
        /// The path to the main executable.
        /// </summary>
        public static String ExecutablePath
        {
            get
            {
                return Instance.ExecutablePathImpl;
            }
        }

        /// <summary>
        /// This will be true if the platform info has been properly setup.
        /// </summary>
        internal static bool IsValid
        {
            get
            {
                return Instance != null;
            }
        }

        /// <summary>
        /// The instance of this platform info. Will be created when the subclass is instantiated.
        /// </summary>
        protected static RuntimePlatformInfo Instance { get; set; }

        protected RuntimePlatformInfo()
        {
            if(Instance != null)
            {
                throw new Exception("Can only create one instance of the RuntimePlatformInfo class.");
            }
            Instance = this;
        }

        /// <summary>
        /// A folder that user documents and settings can be put into.
        /// </summary>
        protected abstract String LocalUserDocumentsFolderImpl { get; }

        /// <summary>
        /// A non roaming folder that larger data files (like plugins and program downloads) can be put into.
        /// </summary>
        protected abstract String LocalDataFolderImpl { get; }

        /// <summary>
        /// A non roaming folder that data that should be kept private (like license files) can be put into.
        /// </summary>
        protected abstract String LocalPrivateDataFolderImpl { get; }

        /// <summary>
        /// The path to the main executable.
        /// </summary>
        protected abstract string ExecutablePathImpl { get; }
    }
}
