﻿using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using Logging;
using System.Windows.Forms;
using Editor;
using Engine.ObjectManagement;
using PhysXWrapper;
using System.Xml;
using Engine.Saving.XMLSaver;
using System.IO;
using Engine.Renderer;
using Engine.Platform;
using Engine.Resources;

namespace Test
{
    class Program
    {
        static TestForm testForm;
        static WindowsFormsWindow testOSWindow;
        static Engine.Platform.UpdateTimer mainTimer;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            testForm = new TestForm();
            testOSWindow = new WindowsFormsWindow(testForm.PaintPanel);
            testForm.FormClosed += new FormClosedEventHandler(testForm_FormClosed);
            OSWindow outerWindow = new WindowsFormsWindow(testForm);

            LogFileListener logListener = new LogFileListener();
            logListener.openLogFile("Log.log");
            Log.Default.addLogListener(logListener);
            using (PluginManager pluginManager = new PluginManager(new ConfigFile("foo.ini")) )
            {
                
                pluginManager.OnConfigureDefaultWindow = createWindow;
                DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
                pluginLoader.addPath("PhysXPlugin.dll");
                pluginLoader.addPath("Win32Platform.dll");
                pluginLoader.addPath("OgrePlugin.dll");
                pluginLoader.loadPlugins(pluginManager);
                pluginManager.initializePlugins();

                //pluginManager.getOtherCommand("addResourceLocation").execute("S:/export/Shaders/Articulometrics", "FileSystem", "Test", true);
                //pluginManager.getOtherCommand("addResourceLocation").execute("S:/export/Media/Perfect/Skull", "FileSystem", "Test", true);
                //pluginManager.getOtherCommand("initializeResourceGroups").execute();
                Resource.ResourceRoot = null;
                ResourceManager secondaryResources;
                if (!File.Exists("resources.xml"))
                {
                    secondaryResources = pluginManager.createSecondaryResourceManager();
                    SubsystemResources ogreResources = secondaryResources.getSubsystemResource("Ogre");
                    ResourceGroup ogreTestGroup = ogreResources.addResourceGroup("Test");
                    ogreTestGroup.addResource("S:/export/Shaders/Articulometrics", ResourceType.FileSystem, true);
                    ogreTestGroup.addResource("S:/export/Media/Perfect/Skull", ResourceType.FileSystem, true);

                    ObjectEditorForm resourceForm = new ObjectEditorForm();
                    resourceForm.EditorPanel.setEditInterface(secondaryResources.getEditInterface());
                    resourceForm.ShowDialog();

                    XmlTextWriter resourceWriter = new XmlTextWriter("resources.xml", Encoding.Unicode);
                    resourceWriter.Formatting = Formatting.Indented;
                    XmlSaver resourceSaver = new XmlSaver();
                    resourceSaver.saveObject(secondaryResources, resourceWriter);
                    resourceWriter.Close();
                }
                else
                {
                    XmlTextReader resourceReader = new XmlTextReader("resources.xml");
                    XmlSaver xmlSaver = new XmlSaver();
                    secondaryResources = xmlSaver.restoreObject(resourceReader) as ResourceManager;
                    resourceReader.Close();
                }

                pluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
                pluginManager.PrimaryResourceManager.forceResourceRefresh();

                //Timer
                mainTimer = pluginManager.PlatformPlugin.createTimer();
                mainTimer.processMessageLoop(true);
                Coroutine.SetTimerFixed(mainTimer);

                InputHandler inputHandler = pluginManager.PlatformPlugin.createInputHandler(outerWindow, false, false, false);
                EventManager eventManager = new EventManager(inputHandler);
                EventUpdateListener eventUpdates = new EventUpdateListener(eventManager);
                mainTimer.addFixedUpdateListener(eventUpdates);

                pluginManager.setPlatformInfo(mainTimer, eventManager);

                ObjectEditorForm form;
                GenericSimObjectDefinition objectDef;

                //Create a scene definition
                SimSceneDefinition sceneDef;
                if (!File.Exists("scene.xml"))
                {
                    sceneDef = new SimSceneDefinition();
                    form = new ObjectEditorForm();
                    form.EditorPanel.setEditInterface(sceneDef.getEditInterface());
                    form.ShowDialog();

                    XmlTextWriter textWriter = new XmlTextWriter("scene.xml", Encoding.Unicode);
                    textWriter.Formatting = Formatting.Indented;
                    XmlSaver xmlSaver = new XmlSaver();
                    xmlSaver.saveObject(sceneDef, textWriter);
                    textWriter.Close();
                }
                else
                {
                    XmlTextReader textReader = new XmlTextReader("scene.xml");
                    XmlSaver xmlSaver = new XmlSaver();
                    sceneDef = xmlSaver.restoreObject(textReader) as SimSceneDefinition;
                    textReader.Close();
                }

                using (SimScene scene = sceneDef.createScene())
                {
                    SimSubScene subScene = scene.getDefaultSubScene();
                    if (subScene != null)
                    {
                        SimObjectManagerDefinition simObjectManagerDef = null;

                        if (!File.Exists("simObjects.xml"))
                        {
                            objectDef = new GenericSimObjectDefinition("Test");
                            TestBehavior testBehavior = new TestBehavior();
                            objectDef.addElement(new BehaviorDefinition("Im a test", testBehavior));
                            objectDef.Translation = new Vector3(0.0f, 5.0f, 0.0f);
                            objectDef.Enabled = true;
                            form = new ObjectEditorForm();
                            form.EditorPanel.setEditInterface(objectDef.getEditInterface());
                            form.ShowDialog();

                            //Test construction
                            simObjectManagerDef = new SimObjectManagerDefinition();
                            simObjectManagerDef.addSimObject(objectDef);

                            XmlTextWriter textWriter = new XmlTextWriter("simObjects.xml", Encoding.Unicode);
                            textWriter.Formatting = Formatting.Indented;
                            XmlSaver xmlSaver = new XmlSaver();
                            xmlSaver.saveObject(simObjectManagerDef, textWriter);
                            textWriter.Close();
                        }
                        else
                        {
                            XmlTextReader textReader = new XmlTextReader("simObjects.xml");
                            XmlSaver xmlSaver = new XmlSaver();
                            simObjectManagerDef = xmlSaver.restoreObject(textReader) as SimObjectManagerDefinition;
                            textReader.Close();
                        }

                        SimObjectManagerDefinition managerDef;
                        using (SimObjectManager manager = simObjectManagerDef.createSimObjectManager(subScene))
                        {
                            scene.buildScene();

                            CameraControl cameraControl = pluginManager.RendererPlugin.PrimaryWindow.createCamera(subScene, "TestCamera", new Vector3(0.0f, 0f, -75f), Vector3.Zero);
                            cameraControl.BackgroundColor = new Color(0.0f, 0.0f, 1.0f);
                            cameraControl.addLight();
                            OrbitCameraController orbitCamera = new OrbitCameraController(cameraControl, eventManager);
                            mainTimer.addFixedUpdateListener(orbitCamera);
                            orbitCamera.setNewPosition(new Vector3(0f, 0f, 150f), Vector3.Zero);

                            PhysSDK.Instance.connectRemoteDebugger("127.0.0.1");
                            testForm.Show();
                            Coroutine.Start(coroutineTest(mainTimer));
                            mainTimer.startLoop();
                            pluginManager.RendererPlugin.PrimaryWindow.destroyCamera(cameraControl);

                            managerDef = manager.saveToDefinition();
                        }

                        form = new ObjectEditorForm();
                        form.EditorPanel.setEditInterface(managerDef.getSimObject("Test").getEditInterface());
                        form.ShowDialog();
                    }
                    eventManager.Dispose();
                    pluginManager.PlatformPlugin.destroyInputHandler(inputHandler);
                }

                pluginManager.PlatformPlugin.destroyTimer(mainTimer);
            }
            
            logListener.closeLogFile();
        }

        static void testForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainTimer.stopLoop();
        }

        static void createWindow(out DefaultWindowInfo defaultWindow)
        {
            defaultWindow = new DefaultWindowInfo(testOSWindow);
            //defaultWindow = new DefaultWindowInfo("Test", 1024, 768);
            //defaultWindow.Fullscreen = true;
        }

        static IEnumerator<YieldAction> coroutineTest(UpdateTimer timer)
        {
            yield return Coroutine.Wait(5.0);
            System.Console.WriteLine("Coroutine works ");
        }
    }
}
