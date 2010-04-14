using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.IO;
using System.Xml;
using Engine.Saving.XMLSaver;
using Engine.ObjectManagement;
using Engine;
using Engine.Resources;

namespace Anomaly
{
    class PublishMode
    {
        public PublishMode()
        {
            Archive = false;
            Obfuscate = false;
        }

        public void getSettingsFromCommandLine(String[] commandLine)
        {
            if (commandLine.Length >= 5)
            {
                SolutionFile = commandLine[2];
                SceneFile = commandLine[3];
                Destination = commandLine[4];
                for (int i = 4; i < commandLine.Length; ++i)
                {
                    if (commandLine[i].ToLower() == "-a")
                    {
                        Archive = true;
                        ++i;
                        if(i < commandLine.Length)
                        {
                            ArchiveName = commandLine[i];
                        }
                    }
                    else if (commandLine[i].ToLower() == "-o")
                    {
                        Obfuscate = true;
                    }
                }
            }
        }

        public void publishResources()
        {
            //Create the log.
            using (LogFileListener logListener = new LogFileListener())
            {
                logListener.openLogFile(AnomalyConfig.DocRoot + "/log.log");
                Log.Default.addLogListener(logListener);
                if (!File.Exists(SolutionFile))
                {
                    Log.Error("Could not find solution file {0}.", SolutionFile);
                    return;
                }
                if (!File.Exists(SceneFile))
                {
                    Log.Error("Could not find scene file {0}.", SceneFile);
                    return;
                }
                doPublish();
            }
        }

        private void doPublish()
        {
            Log.Info("Publishing resources for scene {0} from solution {1} to {2}.", SceneFile, SolutionFile, Destination);
            if (Archive)
            {
                Log.Info("An archive named {0} will be created.", ArchiveName);
            }
            if (Obfuscate)
            {
                Log.Info("The archive will be obfuscated.");
            }
            
            XmlSaver xmlSaver = new XmlSaver();
            Solution solution = new Solution(SolutionFile);

            using (PluginManager pluginManager = new PluginManager(AnomalyConfig.ConfigFile))
            {
                PublishController publisher = new PublishController(solution);

                VirtualFileSystem.Instance.addArchive(solution.ResourceRoot);

                DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
                ConfigIterator pluginIterator = solution.PluginSection.PluginIterator;
                pluginIterator.reset();
                while (pluginIterator.hasNext())
                {
                    pluginLoader.addPath(pluginIterator.next());
                }
                pluginLoader.loadPlugins(pluginManager);

                XmlTextReader textReader = new XmlTextReader(SceneFile);
                ScenePackage scenePackage = xmlSaver.restoreObject(textReader) as ScenePackage;
                publisher.scanResources();
                publisher.copyResources(Destination, ArchiveName, Archive, Obfuscate);
            }

            Log.Info("Finished publishing resources to {0}.", Destination);
        }

        public String SolutionFile { get; set; }

        public String SceneFile { get; set; }

        public String Destination { get; set; }

        public String ArchiveName { get; set; }

        public bool Archive { get; set; }

        public bool Obfuscate { get; set; }
    }
}
