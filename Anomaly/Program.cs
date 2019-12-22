using System;
using System.Collections.Generic;
using System.Linq;
using Logging;
using Engine;
using Anomaly.GUI;
using System.IO;
using Engine.Platform;
using Anomalous.OSPlatform;
using Anomalous.OSPlatform.Win32;

namespace Anomaly
{
    public static class AnomalyProgram
    {
        static PublishMode publishMode;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Run(IAnomalyImplementation implementation)
        {
            WindowsRuntimePlatformInfo.Initialize();
            OgrePlugin.OgreInterface.CompressedTextureSupport = OgrePlugin.CompressedTextureSupport.None;

            //Parse command line
            String projectFileName = null;
            String[] commandArgs = Environment.GetCommandLineArgs();
            bool startGUI = false;
            if (commandArgs.Length > 1)
            {
                //Handle the os passing a file name
                if (commandArgs.Length == 2)
                {
                    projectFileName = commandArgs[1];
                    startGUI = File.Exists(projectFileName);
                }
                //Process a command from the command line
                else
                {
                    if (commandArgs[1].ToLower() == "publish")
                    {
                        publishMode = new PublishMode();
                        publishMode.getSettingsFromCommandLine(commandArgs);
                        projectFileName = publishMode.SolutionFile;
                        startGUI = File.Exists(projectFileName);
                    }
                }
            }
            else
            {
                //Handle no arguments
            }
            if (startGUI)
            {
                AnomalyConfig.RecentDocuments.addDocument(projectFileName);
                runGui(projectFileName, implementation);
            }
        }

        static void runGui(String projectFileName, IAnomalyImplementation implementation)
        {
            if (projectFileName != null)
            {
                using (AnomalyApp app = new AnomalyApp(projectFileName, implementation))
                {
                    app.AnomalyController.FullyLoaded += AnomalyController_FullyLoaded;
                    try
                    {
                        app.run();
                    }
                    catch (Exception e)
                    {
                        Log.Default.printException(e);
                        String errorMessage = e.Message + "\n" + e.StackTrace;
                        while (e.InnerException != null)
                        {
                            e = e.InnerException;
                            errorMessage += "\n" + e.Message + "\n" + e.StackTrace;
                        }
                        MessageDialog.showErrorDialog(errorMessage, "Exception");
                    }
                }
            }
        }

        static void AnomalyController_FullyLoaded(AnomalyController controller)
        {
            if (publishMode != null)
            {
                publishMode.publishResources(controller);
            }
        }
    }
}
