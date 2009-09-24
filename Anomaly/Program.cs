using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Logging;
using Engine;
using Anomaly.GUI;
using System.IO;

namespace Anomaly
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
                        PublishMode publishMode = new PublishMode();
                        publishMode.getSettingsFromCommandLine(commandArgs);
                        publishMode.publishResources();
                    }
                }
            }
            else
            {
                projectFileName = findProjectFile();
                startGUI = File.Exists(projectFileName);
            }
            if (startGUI)
            {
                Program.startGUI(projectFileName);
            }
        }

        static String findProjectFile()
        {
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.Filter = "Anomaly Projects(*.ano)|*.ano;";
                DialogResult result = openFile.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return openFile.FileName;
                }
                return null;
            }
        }

        static void startGUI(String projectFileName)
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();
            Application.DoEvents();
            if (projectFileName != null)
            {
                using (AnomalyController anomalyController = new AnomalyController())
                {
                    try
                    {
                        anomalyController.initialize(new AnomalyProject(projectFileName));
                        anomalyController.createNewScene();
                        splash.Close();
                        splash.Dispose();
                        anomalyController.start();
                    }
                    catch (Exception e)
                    {
                        Log.Default.printException(e);
                        MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
