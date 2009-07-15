using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Logging;
using System.IO;

namespace OgreModelEditor
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

            using (OgreModelEditorController controller = new OgreModelEditorController())
            {
                try
                {
                    controller.initialize();
                    String[] commandLine = Environment.GetCommandLineArgs();
                    if (commandLine.Length > 1)
                    {
                        String file = commandLine[1];
                        if (File.Exists(file) && file.EndsWith(".mesh"))
                        {
                            controller.openModel(file);
                        }
                    }
                    controller.start();
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
