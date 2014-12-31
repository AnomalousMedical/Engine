using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class OgreSceneViewLightManager : SceneViewLightManager
    {
        private Dictionary<SimScene, LightMover> activeSceneLights = new Dictionary<SimScene, LightMover>();

        private class LightMover : SceneListener
        {
            private Light light;

            public LightMover(Light light)
            {
                this.light = light;
            }

            public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
            {

            }

            public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
            {
                light.setPosition(viewport.getCamera().getParentSceneNode().getPosition());
            }

            public Light Light
            {
                get
                {
                    return light;
                }
            }
        }

        public void Dispose()
        {

        }

        public void sceneLoaded(SimScene scene)
        {
            if (!activeSceneLights.ContainsKey(scene))
            {
                SimSubScene subScene = scene.getDefaultSubScene();

                if (subScene.hasSimElementManagerType(typeof(OgreSceneManager)))
                {
                    OgreSceneManager sceneManager = subScene.getSimElementManager<OgreSceneManager>();
                    Light light = sceneManager.SceneManager.createLight("CameraLight");
                    LightMover lightMover = new LightMover(light);
                    sceneManager.SceneManager.addSceneListener(lightMover);
                    activeSceneLights.Add(scene, lightMover);
                }
            }
        }

        public void sceneUnloading(SimScene scene)
        {
            if (scene != null && activeSceneLights.ContainsKey(scene))
            {
                SimSubScene subScene = scene.getDefaultSubScene();

                if (subScene.hasSimElementManagerType(typeof(OgreSceneManager)))
                {
                    LightMover lightMover = activeSceneLights[scene];

                    OgreSceneManager sceneManager = subScene.getSimElementManager<OgreSceneManager>();
                    sceneManager.SceneManager.destroyLight(lightMover.Light);
                    sceneManager.SceneManager.removeSceneListener(lightMover);

                    activeSceneLights.Remove(scene);
                }
            }
        }
    }
}
