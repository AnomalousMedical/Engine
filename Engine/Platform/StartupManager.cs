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
        /// This should be called as early as possible in a client program. It will setup the x86 and x64 library folders.
        /// Even though it uses Windows P/Invoke libraries it should work ok on any os since all exceptions are caught.
        /// </summary>
        public static void SetupDllDirectories()
        {
            try
            {
                String executionPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (Environment.Is64BitProcess)
                {
                    SetDllDirectory(Path.Combine(executionPath, "x64"));
                }
                else
                {
                    SetDllDirectory(Path.Combine(executionPath, "x86"));
                }
            }
            catch (Exception) { }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);
    }
}
