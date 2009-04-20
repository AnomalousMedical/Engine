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
    /// <summary>
    /// The SimElement for a SceneNode.
    /// </summary>
    public class SceneNodeElement : SimElement
    {
        private Identifier sceneID;
        private OgreSceneManager scene;
        private SceneNode sceneNode;
        private Dictionary<Identifier, Entity> entities = new Dictionary<Identifier, Entity>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sceneID">The identifier of the scene node.</param>
        /// <param name="subscription">The subscription of the scene node.</param>
        /// <param name="scene">The scene this node belongs to.</param>
        /// <param name="node">The node to manage.</param>
        public SceneNodeElement(Identifier sceneID, Subscription subscription, OgreSceneManager scene, SceneNode node)
            :base(sceneID.ElementName, subscription) 
        {
            this.sceneID = sceneID;
            this.scene = scene;
            this.sceneNode = node;
        }

        /// <summary>
        /// Add an Entity to the node. This will attach it to the SceneNode.
        /// </summary>
        /// <param name="identifier">The identifier of the Entity.</param>
        /// <param name="entity">The Entity to attach.</param>
        public void addEntity(Identifier identifier, Entity entity)
        {
            sceneNode.attachObject(entity);
            entities.Add(identifier, entity);
        }

        /// <summary>
        /// Dispose this SceneNode and all attached objects.
        /// </summary>
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
