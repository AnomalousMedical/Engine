using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    class iOSRuntimePlatformInfo : RuntimePlatformInfo
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
                return Path.GetFullPath(".");
            }
        }

        protected override bool ShowMoreColorsImpl
        {
            get
            {
                return false;
            }
        }
    }
}
