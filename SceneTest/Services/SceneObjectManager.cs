using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class SceneObjectManager<T>
    {
        private List<SceneObject> sceneObjects = new List<SceneObject>();

        public void Add(SceneObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        public void Remove(SceneObject sceneObject)
        {
            sceneObjects.Remove(sceneObject);
        }

        public IEnumerable<SceneObject> SceneObjects
        {
            get
            {
                return sceneObjects;
            }
        }
    }
}
