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
        private List<Identifier> entities = new List<Identifier>();
        private List<Identifier> cameras = new List<Identifier>();
        private List<Identifier> lights = new List<Identifier>();
        private List<Identifier> manualObjects = new List<Identifier>();
        private List<SceneNodeElement> children = new List<SceneNodeElement>();

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
        public void attachObject(Identifier identifier, Entity entity)
        {
            sceneNode.attachObject(entity);
            entities.Add(identifier);
        }

        public void attachObject(Identifier identifier, Camera camera)
        {
            sceneNode.attachObject(camera);
            cameras.Add(identifier);
        }

        public void attachObject(Identifier identifier, Light light)
        {
            sceneNode.attachObject(light);
            lights.Add(identifier);
        }

        public void attachObject(Identifier identifier, ManualObject manualObject)
        {
            sceneNode.attachObject(manualObject);
            manualObjects.Add(identifier);
        }

        public void addChild(SceneNodeElement element)
        {
            children.Add(element);
            sceneNode.addChild(element.sceneNode);
        }

        public void removeChild(SceneNodeElement element)
        {
            children.Remove(element);
            sceneNode.removeChild(element.sceneNode);
        }

        /// <summary>
        /// Dispose this SceneNode and all attached objects.
        /// </summary>
        protected override void Dispose()
        {
            foreach (Identifier identifier in entities)
            {
                scene.destroyEntity(identifier);
            }
            foreach (Identifier identifier in cameras)
            {
                scene.destroyCamera(identifier);
            }
            foreach (Identifier identifier in lights)
            {
                scene.destroyLight(identifier);
            }
            foreach (Identifier identifier in manualObjects)
            {
                scene.destroyManualObject(identifier);
            }
            foreach (SceneNodeElement child in children)
            {
                child.Dispose();
            }
            scene.destroySceneNode(sceneID);
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
            sceneNode.setVisible(enabled);
        }

        public override SimElementDefinition saveToDefinition()
        {
            return saveToSceneNodeDefinition();
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
            foreach (Identifier identifier in entities)
            {
                definition.addMovableObjectDefinition(new EntityDefinition(identifier.ElementName, scene.getEntity(identifier)));
            }
            foreach (Identifier identifier in cameras)
            {
                definition.addMovableObjectDefinition(new CameraDefinition(identifier.ElementName, scene.getCamera(identifier)));
            }
            foreach (Identifier identifier in lights)
            {
                definition.addMovableObjectDefinition(new LightDefinition(identifier.ElementName, scene.getLight(identifier)));
            }
            foreach (Identifier identifier in manualObjects)
            {
                definition.addMovableObjectDefinition(new ManualObjectDefinition(identifier.ElementName, scene.getManualObject(identifier)));
            }
            foreach (SceneNodeElement child in children)
            {
                definition.addChildNode(child.saveToSceneNodeDefinition());
            }
            return definition;
        }
    }
}
