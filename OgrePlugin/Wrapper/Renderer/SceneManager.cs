using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class SceneManager : IDisposable
    {
        internal static SceneManager createWrapper(IntPtr nativePointer, object[] args)
        {
            return new SceneManager(nativePointer);
        }

        IntPtr ogreSceneManager;
        SceneNode rootNode;
        WrapperCollection<Camera> cameras = new WrapperCollection<Camera>(Camera.createWrapper);
        WrapperCollection<Light> lights = new WrapperCollection<Light>(Light.createWrapper);
        WrapperCollection<SceneNode> sceneNodes = new WrapperCollection<SceneNode>(SceneNode.createWrapper);
        WrapperCollection<Entity> entities = new WrapperCollection<Entity>(Entity.createWrapper);
        WrapperCollection<ManualObject> manualObjects = new WrapperCollection<ManualObject>(ManualObject.createWrapper);

        internal IntPtr OgreSceneManager
        {
            get
            {
                return ogreSceneManager;
            }
        }

        public SceneManager(IntPtr ogreSceneManager)
        {
            this.ogreSceneManager = ogreSceneManager;
        }

        public void Dispose()
        {
            ogreSceneManager = IntPtr.Zero;
        }
    }
}
