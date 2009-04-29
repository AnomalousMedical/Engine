using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Logging;
using Engine;

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

            LogFileListener logListener = new LogFileListener();
            logListener.openLogFile(AnomalyConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);
            AnomalyConfig.ConfigFile.loadConfigFile();

            using (PluginManager pluginManager = new PluginManager())
            {
                try
                {
                    //pluginManager.OnConfigureDefaultWindow = createWindow;
                    DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
                    ConfigSection plugins = AnomalyConfig.ConfigFile.createOrRetrieveConfigSection("Plugins");
                    for (int i = 0; plugins.hasValue("Plugin" + i); ++i)
                    {
                        pluginLoader.addPath(plugins.getValue("Plugin" + i, ""));
                    }
                    pluginLoader.loadPlugins(pluginManager);
                    pluginManager.initializePlugins();
                }
                catch (Exception e)
                {
                    Log.Default.sendMessage("Exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Anomaly", e.GetType().Name, e.Message, e.StackTrace);
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        Log.Default.sendMessage("--Inner exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Anomaly", e.GetType().Name, e.Message, e.StackTrace);
                    }
                    MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            AnomalyConfig.ConfigFile.writeConfigFile();
            logListener.closeLogFile();
        }
    }
}
