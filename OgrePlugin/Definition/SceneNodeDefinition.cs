using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Logging;
using OgreWrapper;
using Engine.Saving;

namespace OgrePlugin
{
    /// <summary>
    /// The definition class for a SceneNode.
    /// </summary>
    public class SceneNodeDefinition : SimElementDefinition
    {
        private EditInterface editInterface;
        private Dictionary<String, EntityDefinition> entities = new Dictionary<String, EntityDefinition>();
        private EditInterfaceManager<EntityDefinition> entityEditInterfaces;
        private EditInterfaceCommand destroyEntity;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        public SceneNodeDefinition(String name)
            :base(name)
        {

        }

        /// <summary>
        /// Register this class with the factory to be built.
        /// </summary>
        /// <param name="subscene">The subscene to add this definition to.</param>
        /// <param name="instance">The SimObject that will get the product.</param>
        public override void register(SimSubScene subscene, SimObject instance)
        {
            if (subscene.hasSimElementManagerType(typeof(OgreSceneManager)))
            {
                OgreSceneManager sceneManager = subscene.getSimElementManager<OgreSceneManager>();
                sceneManager.getOgreFactory().addSceneNodeDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add SceneNodeDefinition {0} to SimSubScene {1} because it does not contain an OgreSceneManager.", LogLevel.Warning, OgreInterface.PluginName);
            }
        }

        /// <summary>
        /// Get the EditInterface for this SceneNode.
        /// </summary>
        /// <returns>The node's EditInterface.</returns>
        public override EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(Name + " Scene Node");
                entityEditInterfaces = new EditInterfaceManager<EntityDefinition>(editInterface);
                editInterface.addCommand(new EditInterfaceCommand("Add Entity", addEntity));
                destroyEntity = new EditInterfaceCommand("Remove", removeEntity);
            }
            return editInterface;
        }

        /// <summary>
        /// Add an EntityDefinition.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        public void addEntityDefinition(EntityDefinition definition)
        {
            entities.Add(definition.Name, definition);
            if (editInterface != null)
            {
                addEntityEditInterface(definition);
            }
        }

        /// <summary>
        /// Remove an EntityDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeEntityDefinition(EntityDefinition definition)
        {
            entities.Remove(definition.Name);
            if (editInterface != null)
            {
                entityEditInterfaces.removeSubInterface(definition);
            }
        }

        /// <summary>
        /// Create the product of this SceneNode.
        /// </summary>
        /// <param name="instance">The instance to get the product.</param>
        /// <param name="scene">The scene to create the product into.</param>
        internal void createProduct(SimObject instance, OgreSceneManager scene)
        {
            Identifier identifier = new Identifier(instance.Name, Name);
            SceneNode node = scene.createSceneNode(identifier);
            node.setPosition(instance.Translation);
            node.setOrientation(instance.Rotation);
            SceneNodeElement element = new SceneNodeElement(identifier, this.subscription, scene, node);
            foreach (EntityDefinition entity in entities.Values)
            {
                entity.createProduct(element, scene, instance);
            }
            instance.addElement(element);
            scene.SceneManager.getRootSceneNode().addChild(node);
        }

        /// <summary>
        /// Create a static product.
        /// </summary>
        /// <param name="instance">The instance to get the product.</param>
        /// <param name="scene">The scene to create the product into.</param>
        internal void createStaticProduct(SimObject instance, OgreSceneManager scene)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper function to add an EditInterface for an EntityDefinition.
        /// </summary>
        /// <param name="definition"></param>
        private void addEntityEditInterface(EntityDefinition definition)
        {
            EditInterface edit = definition.getEditInterface();
            edit.addCommand(destroyEntity);
            entityEditInterfaces.addSubInterface(definition, edit);
        }
        
        /// <summary>
        /// Callback to add an EntityDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addEntity(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.entities.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                addEntityDefinition(new EntityDefinition(name));
            }
        }

        /// <summary>
        /// Callback to remove an EntityDefiniton.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void removeEntity(EditUICallback callback, EditInterfaceCommand command)
        {
            removeEntityDefinition(entityEditInterfaces.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        #region Saveable Members

        private const String ENTITY_BASE = "Entity";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info"></param>
        private SceneNodeDefinition(LoadInfo info)
            :base(info)
        {
            for (int i = 0; info.hasValue(ENTITY_BASE + i); i++)
            {
                addEntityDefinition(info.GetValue<EntityDefinition>(ENTITY_BASE + i));
            }
        }

        /// <summary>
        /// GetInfo function.
        /// </summary>
        /// <param name="info"></param>
        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            int i = 0;
            foreach (EntityDefinition entity in entities.Values)
            {
                info.AddValue(ENTITY_BASE + i++, entity);
            }
        }

        #endregion Saveable Members
    }
}
