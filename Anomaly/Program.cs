using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Logging;
using Engine;
using Anomaly.GUI;
using System.IO;
using Engine.Platform;

namespace Anomaly
{
    static class Program
    {
        static PublishMode publishMode;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StartupManager.SetupDllDirectories();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
                projectFileName = findProjectFile();
                startGUI = File.Exists(projectFileName);
                if (startGUI) //Add file to recent documents list.
                {
                    AnomalyConfig.RecentDocuments.addDocument(projectFileName);
                }
            }
            if (startGUI)
            {
                Program.startGUI(projectFileName);
            }
        }

        static String findProjectFile()
        {
            using (ProjectSelector openFile = new ProjectSelector())
            {
                openFile.setRecentFiles(AnomalyConfig.RecentDocuments);
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    return openFile.SelectedFile;
                }
            }
            return "";
        }

        static void startGUI(String projectFileName)
        {
            if (projectFileName != null)
            {
                using (AnomalyApp app = new AnomalyApp(projectFileName))
                {
                    app.AnomalyController.FullyLoaded += AnomalyController_FullyLoaded;
                    try
                    {
                        app.run();
                    }
                    catch (Exception e)
                    {
                        Log.Default.printException(e);
                        MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        static void AnomalyController_FullyLoaded(AnomalyController controller)
        {
            if(publishMode != null)
            {
                publishMode.publishResources(controller);
            }
        }
    }
}
