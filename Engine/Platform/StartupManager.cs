using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    /// <summary>
    /// This class has startup stuff for the engine.
    /// </summary>
    public class StartupManager
    {
        /// <summary>
        /// This should be called as early as possible in a client program. It will setup the x86 and x64 library folders if
        /// they exist. So if you have both modes for native libraries add them to a folder called x86 and x64 in the application
        /// directory. If these folders do not exist nothing will be changed.
        /// </summary>
        public static void SetupDllDirectories()
        {
            try
            {
                //Check bitness and determine path 
                String executionPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                String nativeLibPath;
                if (Environment.Is64BitProcess)
                {
                    nativeLibPath = Path.Combine(executionPath, "x64");
                }
                else
                {
                    nativeLibPath = Path.Combine(executionPath, "x86");
                }
                if(Directory.Exists(nativeLibPath))
                {
                    //Make sure this folder has not already been added to the path.
                    String currentPathSetting = Environment.GetEnvironmentVariable("PATH");
                    if (!currentPathSetting.Contains(nativeLibPath))
                    {
                        Environment.SetEnvironmentVariable("PATH", String.Format("{0};{1}", currentPathSetting, nativeLibPath));
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
