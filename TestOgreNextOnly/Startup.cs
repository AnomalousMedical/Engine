using Engine;
using Microsoft.Extensions.DependencyInjection;
using OgreNextPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Anomalous.Minimus.Full
{
    public class Startup : IStartup
    {
        public string Title => "Anomalous Minimus with Ogre Next";

        public string Name => "OgreNextTest";

        public IEnumerable<Assembly> AdditionalPluginAssemblies => new Assembly[0];

        public Startup()
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Initialized(CoreApp pharosApp, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            var root = scope.ServiceProvider.GetService<Root>();
            var ogreInterface = scope.ServiceProvider.GetService<OgreInterface>();
            var renderWindow = ogreInterface.PrimaryOgreWindow;

            var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            VirtualFileSystem.Instance.addArchive(Path.Combine(assemblyPath, "Media"));
            OgreResourceGroupManager.getInstance().addResourceLocation("", "EngineArchive", "pbstest", false);

            OgreResourceGroupManager.getInstance().addResourceLocation("Common", "EngineArchive", "pbstest", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("Common/Any", "EngineArchive", "pbstest", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("Common/GLSL", "EngineArchive", "pbstest", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("Common/GLSLES", "EngineArchive", "pbstest", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("Common/HLSL", "EngineArchive", "pbstest", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("Common/Metal", "EngineArchive", "pbstest", false);

            OgreResourceGroupManager.getInstance().initializeAllResourceGroups(false);

            //temp test scene
            var sceneManager = root.createSceneManager(SceneType.ST_GENERIC, 1);
            var camera = sceneManager.createCamera("Main Camera");

            // Position it at 500 in Z direction
            camera.setPosition(new Vector3(0, 5, 15));
            // Look back along -Z
            camera.lookAt(new Vector3(0, 0, 0));
            camera.setNearClipDistance(0.2f);
            camera.setFarClipDistance(1000.0f);
            camera.setAutoAspectRatio(true);

            // Setup a basic compositor with a blue clear colour
            CompositorManager2 compositorManager = root.CompositorManager2;
            var workspaceName = "PbsMaterialsWorkspace"; //This comes from the compositor file
            //Can create manually too
            //var backgroundColour = new Color(0.2f, 0.4f, 0.6f);
            //compositorManager.createBasicWorkspaceDef(workspaceName, backgroundColour);
            compositorManager.addWorkspace(sceneManager, renderWindow.OgreRenderWindow.Texture, camera, workspaceName, true);

            var rootNode = sceneManager.getRootSceneNode();

            //Add item
            var item = sceneManager.createItem("Sphere1000.mesh");
            //var item = sceneManager.createItem("Cube_d.mesh");
            item.SetDatablock("Rocks");
            //item.SetDatablock("Marble");
            item.setVisibilityFlags(0x000000001);
            var itemNode = sceneManager.createSceneNode();
            rootNode.addChild(itemNode);
            itemNode.attachObject(item);
            itemNode.setPosition(new Vector3(0f, 0f, 0f));

            //Add lights
            var light = sceneManager.createLight();
            var lightNode = sceneManager.createSceneNode();
            rootNode.addChild(lightNode);
            lightNode.attachObject(light);
            light.setPowerScale(1.0f);
            light.setType(Light.LightTypes.LT_DIRECTIONAL);
            light.setDirection(new Vector3(-1, -1, -1).normalized());

            sceneManager.setAmbientLight(new Color(0.3f * 0.1f * 0.75f, 0.5f * 0.1f * 0.75f, 0.7f * 0.1f * 0.75f),
                                           new Color(0.6f * 0.065f * 0.75f, 0.45f * 0.065f * 0.75f, 0.3f * 0.065f * 0.75f),
                                           -light.getDirection() + Vector3.UnitY * 0.2f);

            light = sceneManager.createLight();
            lightNode = sceneManager.createSceneNode();
            rootNode.addChild(lightNode);
            lightNode.attachObject(light);
            light.setDiffuseColor(0.8f, 0.4f, 0.2f); //Warm
            light.setSpecularColor(0.8f, 0.4f, 0.2f);
            light.setPowerScale((float)Math.PI);
            light.setType(Light.LightTypes.LT_SPOTLIGHT);
            lightNode.setPosition(new Vector3(0.0f, 10.0f, 0.0f));
            light.setDirection(new Vector3(0, -1, 0).normalized());
            light.setAttenuationBasedOnRadius(10.0f, 0.01f);

            //mLightNodes[1] = lightNode;

            light = sceneManager.createLight();
            lightNode = sceneManager.createSceneNode();
            rootNode.addChild(lightNode);
            lightNode.attachObject(light);
            light.setDiffuseColor(0.2f, 0.4f, 0.8f); //Cold
            light.setSpecularColor(0.2f, 0.4f, 0.8f);
            light.setPowerScale((float)Math.PI);
            light.setType(Light.LightTypes.LT_SPOTLIGHT);
            lightNode.setPosition(new Vector3(0.0f, -10.0f, 0.0f));
            light.setDirection(new Vector3(0, 1, 0).normalized());
            light.setAttenuationBasedOnRadius(10.0f, 0.01f);

            //end temp test scene
        }
    }
}
