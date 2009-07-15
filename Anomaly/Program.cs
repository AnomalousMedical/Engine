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
            SplashScreen splash = new SplashScreen();
            splash.Show();
            String projectFileName = null;
            String[] commandArgs = Environment.GetCommandLineArgs();
            if (commandArgs.Length > 1)
            {
                projectFileName = commandArgs[1];
                if (!File.Exists(projectFileName))
                {
                    projectFileName = null;
                }
            }
            if (projectFileName == null)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Anomaly Projects(*.ano)|*.ano;";
                DialogResult result = openFile.ShowDialog(splash);
                if (result == DialogResult.OK)
                {
                    projectFileName = openFile.FileName;
                }
                openFile.Dispose();
            }
            Application.DoEvents();
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
