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
                ProjectFile = commandLine[2];
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
                if (!File.Exists(ProjectFile))
                {
                    Log.Error("Could not find project file {0}.", ProjectFile);
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
            Log.Info("Publishing resources for scene {0} from project {1} to {2}.", SceneFile, ProjectFile, Destination);
            if (Archive)
            {
                Log.Info("An archive named {0} will be created.", ArchiveName);
            }
            if (Obfuscate)
            {
                Log.Info("The archive will be obfuscated.");
            }
            
            XmlSaver xmlSaver = new XmlSaver();
            AnomalyProject project = new AnomalyProject(ProjectFile);

            Resource.ResourceRoot = project.ResourceSection.ResourceRoot;
            Log.Default.sendMessage("Resource root is \"{0}\".", LogLevel.ImportantInfo, "Editor", Resource.ResourceRoot);

            PublishController publisher = new PublishController(project);

            using (PluginManager pluginManager = new PluginManager(AnomalyConfig.ConfigFile))
            {
                DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
                ConfigIterator pluginIterator = project.PluginSection.PluginIterator;
                pluginIterator.reset();
                while (pluginIterator.hasNext())
                {
                    pluginLoader.addPath(pluginIterator.next());
                }
                pluginLoader.loadPlugins(pluginManager);

                XmlTextReader textReader = new XmlTextReader(SceneFile);
                ScenePackage scenePackage = xmlSaver.restoreObject(textReader) as ScenePackage;
                publisher.scanResources(scenePackage.ResourceManager);
                publisher.copyResources(Destination, ArchiveName, Archive, Obfuscate);
            }

            Log.Info("Finished publishing resources to {0}.", Destination);
        }

        public String ProjectFile { get; set; }

        public String SceneFile { get; set; }

        public String Destination { get; set; }

        public String ArchiveName { get; set; }

        public bool Archive { get; set; }

        public bool Obfuscate { get; set; }
    }
}
