using Engine;
using Microsoft.Extensions.DependencyInjection;
using OgreNextPlugin;
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
            OgreResourceGroupManager.getInstance().addResourceLocation("", "EngineArchive", "pbstest", true);
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
            var workspaceName = "Demo Workspace";
            var backgroundColour = new Color(0.2f, 0.4f, 0.6f);
            compositorManager.createBasicWorkspaceDef(workspaceName, backgroundColour);
            compositorManager.addWorkspace(sceneManager, renderWindow.OgreRenderWindow.Texture, camera, workspaceName, true);

            var rootNode = sceneManager.getRootSceneNode();

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

            //var item = sceneManager.createItem("Sphere1000.mesh");
            var item = sceneManager.createItem("Cube_d.mesh");
            item.SetDatablock("Rocks");
            var itemNode = sceneManager.createSceneNode();
            rootNode.addChild(itemNode);
            itemNode.attachObject(item);
            itemNode.setPosition(new Vector3(0f, 0f, 0f));

            //end temp test scene
        }
    }
}
