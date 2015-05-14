using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    class MacRuntimePlatformInfo : RuntimePlatformInfo
    {
        protected override String LocalUserDocumentsFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalUserDocumentsFolder;
            }
        }

        protected override String LocalDataFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalDataFolder;
            }
        }

        protected override String LocalPrivateDataFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalPrivateDataFolder;
            }
        }

        protected override string ExecutablePathImpl
        {
            get
            {
                String[] args = Environment.GetCommandLineArgs();
                if (args.Length > 0)
                {
                    return Path.GetDirectoryName(args[0]);
                }
                else
                {
                    return Path.GetFullPath(".");
                }
            }
        }

        protected override bool ShowMoreColorsImpl
        {
            get
            {
                return true;
            }
        }
    }
}
