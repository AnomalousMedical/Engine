using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using OgreWrapper;
using Engine;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// The SimElement for a SceneNode.
    /// </summary>
    public class SceneNodeElement : SimElement
    {
        private OgreSceneManager scene;
        private SceneNode sceneNode;
        private List<SceneNodeElement> children = new List<SceneNodeElement>();
        private Dictionary<String, MovableObjectContainer> nodeObjects = new Dictionary<String, MovableObjectContainer>();
        private SceneNode parentNode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sceneID">The identifier of the scene node.</param>
        /// <param name="subscription">The subscription of the scene node.</param>
        /// <param name="scene">The scene this node belongs to.</param>
        /// <param name="node">The node to manage.</param>
        public SceneNodeElement(String elementName, Subscription subscription, OgreSceneManager scene, SceneNode node, SceneNode parentNode)
            :base(elementName, subscription) 
        {
            this.scene = scene;
            this.sceneNode = node;
            node.UserObject = this;
            this.parentNode = parentNode;
        }

        /// <summary>
        /// Add an Entity to the node. This will attach it to the SceneNode.
        /// </summary>
        /// <param name="identifier">The identifier of the Entity.</param>
        /// <param name="entity">The Entity to attach.</param>
        public void attachObject(MovableObjectDefinition definition)
        {
            if (!nodeObjects.ContainsKey(definition.Name))
            {
                MovableObjectContainer moveObj = definition.createProduct(scene, Owner.Name + Name);
                if (moveObj != null)
                {
                    sceneNode.attachObject(moveObj.MovableObject);
                    nodeObjects.Add(moveObj.DefinitionName, moveObj);
                }
            }
            else
            {
                Log.Default.sendMessage("Attempted to add another MovableObject to the node {0} named {1} that already exists. The second entry has been ignored.", LogLevel.Warning, "OgrePlugin", Owner.Name + Name, definition.Name);
            }

        }

        /// <summary>
        /// Check to see if the node has an object named name.
        /// </summary>
        /// <param name="name">The name to test for.</param>
        /// <returns>True if name exists.</returns>
        public bool hasNodeObject(String name)
        {
            return nodeObjects.ContainsKey(name);
        }

        /// <summary>
        /// Get the object specified by name. Test for its existance first using
        /// hasNodeObject.
        /// </summary>
        /// <param name="name">The name to retrieve.</param>
        /// <returns>The MovableObject specified by name.</returns>
        public MovableObject getNodeObject(String name)
        {
            MovableObjectContainer container;
            nodeObjects.TryGetValue(name, out container);
            return container != null ? container.MovableObject : null;
        }

        /// <summary>
        /// Add a child SceneNodeElement.
        /// </summary>
        /// <param name="element">The SceneNodeElement to add as a child.</param>
        public void addChild(SceneNodeElement element)
        {
            children.Add(element);
        }

        /// <summary>
        /// Remove a child SceneNodeElement.
        /// </summary>
        /// <param name="element">The SceneNodeElement to remove.</param>
        public void removeChild(SceneNodeElement element)
        {
            children.Remove(element);
        }

        /// <summary>
        /// Get the derived position.
        /// </summary>
        /// <returns>The derived position.</returns>
        public Vector3 getDerivedPosition()
        {
            return sceneNode.getDerivedPosition();
        }

        /// <summary>
        /// Get the derived orientation.
        /// </summary>
        /// <returns>The derived orientation.</returns>
        public Quaternion getDerivedOrientation()
        {
            return sceneNode.getDerivedOrientation();
        }

        /// <summary>
        /// Get the derived scale.
        /// </summary>
        /// <returns>The derived scale.</returns>
        public Vector3 getDerivedScale()
        {
            return sceneNode.getDerivedScale();
        }

        /// <summary>
        /// Dispose this SceneNode and all attached objects.
        /// </summary>
        protected override void Dispose()
        {
            foreach (MovableObjectContainer movable in nodeObjects.Values)
            {
                movable.destroy(scene);
            }
            foreach (SceneNodeElement child in children)
            {
                child.Dispose();
            }
            scene.SceneManager.destroySceneNode(sceneNode);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            sceneNode.setPosition(translation);
            sceneNode.setOrientation(rotation);
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            sceneNode.setPosition(translation);
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            sceneNode.setOrientation(rotation);
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            sceneNode.setScale(scale);
        }

        protected override void setEnabled(bool enabled)
        {
            if (enabled)
            {
                parentNode.addChild(sceneNode);
            }
            else
            {
                parentNode.removeChild(sceneNode);
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            return saveToSceneNodeDefinition();
        }

        internal SceneNode SceneNode
        {
            get
            {
                return sceneNode;
            }
        }

        /// <summary>
        /// Helper function to recursivly save scene node definitions for all
        /// children.
        /// </summary>
        /// <returns>A new SceneNodeDefinition for this node.</returns>
        private SceneNodeDefinition saveToSceneNodeDefinition()
        {
            SceneNodeDefinition definition = new SceneNodeDefinition(Name);
            definition.LocalTranslation = sceneNode.getPosition();
            definition.LocalRotation = sceneNode.getOrientation();
            foreach (MovableObjectContainer movable in nodeObjects.Values)
            {
                definition.addMovableObjectDefinition(movable.createDefinition());
            }
            foreach (SceneNodeElement child in children)
            {
                definition.addChildNode(child.saveToSceneNodeDefinition());
            }
            return definition;
        }
    }
}
