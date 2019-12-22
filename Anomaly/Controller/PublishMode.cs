using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.IO;
using System.Xml;
using Engine.ObjectManagement;
using Engine;
using Engine.Resources;
using Engine.Threads;
using MyGUIPlugin;
using System.Threading.Tasks;

namespace Anomaly
{
    //Controller for publish mode, supports the following command line structure
    //publish PROJECT_NAME DESTINATION_FOLDER [-a ARCHIVE_NAME] [-p PROFILE_NAME]
    //Note that publishing does NOT build scenes.
    public class PublishMode
    {
        public PublishMode()
        {
            Archive = false;
            Obfuscate = false;
        }

        public void getSettingsFromCommandLine(String[] commandLine)
        {
            if (commandLine.Length >= 4)
            {
                SolutionFile = commandLine[2];
                Destination = commandLine[3];
                for (int i = 3; i < commandLine.Length; ++i)
                {
                    switch(commandLine[i].ToLowerInvariant())
                    {
                        case "-a":
                            Archive = true;
                            ++i;
                            if(i < commandLine.Length)
                            {
                                ArchiveName = commandLine[i];
                            }
                            break;
                        case "-o":
                            Obfuscate = true;
                            break;
                        case "-p":
                            ++i;
                            if (i < commandLine.Length)
                            {
                                Profile = commandLine[i];
                            }
                            break;
                    }
                }
            }
        }

        public void publishResources(AnomalyController controller)
        {
            MessageBox.show("Publishing, please wait.", "Publishing", MessageBoxStyle.IconInfo);
            Task task = new Task(() =>
                {
                    Log.Info("Publishing resources for solution {0} to {1}.", SolutionFile, Destination);
                    if (Archive)
                    {
                        Log.Info("An archive named {0} will be created.", ArchiveName);
                    }
                    if (Obfuscate)
                    {
                        Log.Info("The archive will be obfuscated.");
                    }

                    PublishController publisher = new PublishController(controller.Solution);

                    if (!String.IsNullOrEmpty(Profile))
                    {
                        Log.Info("Using profile {0}", Profile);
                    }

                    publisher.scanResources(Profile);

                    publisher.copyResources(Destination, ArchiveName, Archive, Obfuscate);

                    Log.Info("Finished publishing resources to {0}.", Destination);

                    ThreadManager.invoke(() =>
                    {
                        controller.shutdown();
                    });
                });
            task.Start();
        }

        public String Profile { get; set; }

        public String SolutionFile { get; set; }

        public String Destination { get; set; }

        public String ArchiveName { get; set; }

        public bool Archive { get; set; }

        public bool Obfuscate { get; set; }
    }
}
