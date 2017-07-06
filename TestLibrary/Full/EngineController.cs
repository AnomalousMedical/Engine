using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;
using OgrePlugin;
using Engine.Platform;
using Engine.Renderer;
using System.Threading;
using System.Xml;
using Engine.ObjectManagement;
using Engine.Saving.XMLSaver;
using Engine.Resources;
using System.IO;
using BulletPlugin;
using MyGUIPlugin;
using SoundPlugin;
using libRocketPlugin;
using BEPUikPlugin;
using Anomalous.OSPlatform;
using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.libRocketWidget;

namespace Anomalous.Minimus.Full
{
    public sealed class EngineController
    {
        //Controller
        private SceneController sceneController;

        //Serialization
        private XmlSaver xmlSaver = new XmlSaver();

        //Scene
        private String currentSceneFile;
        private String currentSceneDirectory;

        public EngineController(SceneController sceneController)
        {
            this.sceneController = sceneController;
        }

        /// <summary>
        /// Attempt to open the given scene file. Will return true if the scene was loaded correctly.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>True if the scene was loaded, false on an error.</returns>
        public IEnumerable<SceneBuildStatus> openScene(String filename)
        {
            sceneController.destroyScene();
            VirtualFileSystem sceneArchive = VirtualFileSystem.Instance;
            if (sceneArchive.exists(filename))
            {
                currentSceneFile = VirtualFileSystem.GetFileName(filename);
                currentSceneDirectory = VirtualFileSystem.GetDirectoryName(filename);
                using (Stream file = sceneArchive.openStream(filename, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read))
                {
                    XmlTextReader textReader = null;
                    ScenePackage scenePackage = null;
                    try
                    {
                        yield return new SceneBuildStatus()
                        {
                            Message = "Loading Scene File"
                        };
                        textReader = new XmlTextReader(file);
                        scenePackage = xmlSaver.restoreObject(textReader) as ScenePackage;
                    }
                    finally
                    {
                        if (textReader != null)
                        {
                            textReader.Close();
                        }
                    }
                    if (scenePackage != null)
                    {
                        foreach (var status in sceneController.loadScene(scenePackage, SceneBuildOptions.SingleUseDefinitions))
                        {
                            yield return status;
                        }
                    }
                }
            }
            else
            {
                Log.Error("Could not load scene {0}.", filename);
            }
        }

        public void addSimObject(SimObjectBase simObject)
        {
            sceneController.addSimObject(simObject);
        }

        public SimScene CurrentScene
        {
            get
            {
                return sceneController.CurrentScene;
            }
        }

        public IEnumerable<SimObjectBase> SimObjects
        {
            get
            {
                return sceneController.SimObjects;
            }
        }

        public SimObject getSimObject(String name)
        {
            return sceneController.getSimObject(name);
        }

        public String CurrentSceneFile
        {
            get
            {
                return currentSceneFile;
            }
        }

        public String CurrentSceneDirectory
        {
            get
            {
                return currentSceneDirectory;
            }
        }
    }
}
