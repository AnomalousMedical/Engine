using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using OgreWrapper;
using EngineMath;

namespace OgrePlugin
{
    public class SceneNodeElement : SimElement
    {
        private Identifier sceneID;
        private OgreSceneManager scene;
        private SceneNode sceneNode;
        private Dictionary<Identifier, Entity> entities = new Dictionary<Identifier, Entity>();

        public SceneNodeElement(Identifier sceneID, Subscription subscription, OgreSceneManager scene, SceneNode node)
            :base(sceneID.ElementName, subscription) 
        {
            this.sceneID = sceneID;
            this.scene = scene;
            this.sceneNode = node;
        }

        public void addEntity(Identifier identifier, Entity entity)
        {
            entities.Add(identifier, entity);
        }

        public override void Dispose()
        {
            foreach (Identifier identifier in entities.Keys)
            {
                scene.destroyEntity(identifier);
            }
            scene.destroySceneNode(sceneID);
        }

        public override void updatePosition(ref Vector3 translation, ref Quaternion rotation)
        {
            sceneNode.setPosition(translation);
            sceneNode.setOrientation(rotation);
        }

        public override void updateTranslation(ref Vector3 translation)
        {
            sceneNode.setPosition(translation);
        }

        public override void updateRotation(ref Quaternion rotation)
        {
            sceneNode.setOrientation(rotation);
        }

        public override void updateScale(ref Vector3 scale)
        {
            sceneNode.setScale(scale);
        }

        public override void setEnabled(bool enabled)
        {
            sceneNode.setVisible(enabled);
        }

        public override SimElementDefinition saveToDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
