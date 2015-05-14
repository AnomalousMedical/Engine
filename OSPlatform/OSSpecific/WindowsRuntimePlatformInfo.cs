using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    class WindowsRuntimePlatformInfo : RuntimePlatformInfo
    {
        protected override String LocalUserDocumentsFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        protected override String LocalDataFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

        protected override String LocalPrivateDataFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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
