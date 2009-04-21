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
        private Dictionary<String, MovableObjectDefinition> movableObjects = new Dictionary<String, MovableObjectDefinition>();
        private EditInterfaceManager<MovableObjectDefinition> movableObjectEdits;
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
                movableObjectEdits = new EditInterfaceManager<MovableObjectDefinition>(editInterface);
                editInterface.addCommand(new EditInterfaceCommand("Add Entity", addEntity));
                editInterface.addCommand(new EditInterfaceCommand("Add Light", addLight));
                editInterface.addCommand(new EditInterfaceCommand("Add Camera", addCamera));
                editInterface.addCommand(new EditInterfaceCommand("Add Manual Object", addManualObject));
                destroyEntity = new EditInterfaceCommand("Remove", removeMovableObject);
            }
            return editInterface;
        }

        /// <summary>
        /// Add a MovableObjectDefinition.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        public void addMovableObjectDefinition(MovableObjectDefinition definition)
        {
            movableObjects.Add(definition.Name, definition);
            if (editInterface != null)
            {
                addMovableObjectEdit(definition);
            }
        }

        /// <summary>
        /// Remove a MovableObjectDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeMovableObjectDefinition(MovableObjectDefinition definition)
        {
            movableObjects.Remove(definition.Name);
            if (editInterface != null)
            {
                movableObjectEdits.removeSubInterface(definition);
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
            foreach (MovableObjectDefinition movable in movableObjects.Values)
            {
                movable.createProduct(element, scene, instance);
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
        private void addMovableObjectEdit(MovableObjectDefinition definition)
        {
            EditInterface edit = definition.getEditInterface();
            edit.addCommand(destroyEntity);
            movableObjectEdits.addSubInterface(definition, edit);
        }

        private bool getName(EditUICallback callback, out String name)
        {
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.movableObjects.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            return accept;
        }

        /// <summary>
        /// Callback to add an EntityDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addEntity(EditUICallback callback, EditInterfaceCommand command)
        {         
            String name;
            if (getName(callback, out name))
            {
                addMovableObjectDefinition(new EntityDefinition(name));
            }
        }

        /// <summary>
        /// Callback to add a CameraDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addCamera(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            if (getName(callback, out name))
            {
                addMovableObjectDefinition(new CameraDefinition(name));
            }
        }

        /// <summary>
        /// Callback to add a ManualObjectDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addManualObject(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            if (getName(callback, out name))
            {
                addMovableObjectDefinition(new ManualObjectDefinition(name));
            }
        }

        /// <summary>
        /// Callback to add a LightDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addLight(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            if (getName(callback, out name))
            {
                addMovableObjectDefinition(new LightDefinition(name));
            }
        }

        /// <summary>
        /// Callback to remove an MovableObjectDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void removeMovableObject(EditUICallback callback, EditInterfaceCommand command)
        {
            removeMovableObjectDefinition(movableObjectEdits.resolveSourceObject(callback.getSelectedEditInterface()));
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
                addMovableObjectDefinition(info.GetValue<MovableObjectDefinition>(ENTITY_BASE + i));
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
            foreach (MovableObjectDefinition entity in movableObjects.Values)
            {
                info.AddValue(ENTITY_BASE + i++, entity);
            }
        }

        #endregion Saveable Members
    }
}
