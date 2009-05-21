using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Logging;
using OgreWrapper;
using Engine.Saving;
using Engine.Reflection;
using Engine;

namespace OgrePlugin
{
    /// <summary>
    /// The definition class for a SceneNode.
    /// </summary>
    public class SceneNodeDefinition : SimElementDefinition
    {
        #region Static

        internal static SimElementDefinition Create(String name, EditUICallback callback)
        {
            return new SceneNodeDefinition(name);
        }

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SceneNodeDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            EditableAttributeFilter filter = new EditableAttributeFilter();
            memberScanner.Filter = filter;
        }

        #endregion Static

        private EditInterface editInterface;
        private Dictionary<String, MovableObjectDefinition> movableObjects = new Dictionary<String, MovableObjectDefinition>();
        private Dictionary<String, SceneNodeDefinition> childNodes = new Dictionary<string, SceneNodeDefinition>();
        private EditInterfaceManager<MovableObjectDefinition> movableObjectEdits;
        private EditInterfaceManager<SceneNodeDefinition> childNodeEdits;
        private EditInterfaceCommand destroyMovableObject;
        private EditInterfaceCommand destroyChildNode;
        private SceneNodeDefinition parentNode = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        public SceneNodeDefinition(String name)
            :base(name)
        {
            LocalRotation = Quaternion.Identity;
        }

        /// <summary>
        /// Register this class with the factory to be built.
        /// </summary>
        /// <param name="subscene">The subscene to add this definition to.</param>
        /// <param name="instance">The SimObject that will get the product.</param>
        public override void register(SimSubScene subscene, SimObjectBase instance)
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
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, Name + " Scene Node", null);
                movableObjectEdits = new EditInterfaceManager<MovableObjectDefinition>(editInterface);
                childNodeEdits = new EditInterfaceManager<SceneNodeDefinition>(editInterface);
                editInterface.addCommand(new EditInterfaceCommand("Add Entity", addEntity));
                editInterface.addCommand(new EditInterfaceCommand("Add Light", addLight));
                editInterface.addCommand(new EditInterfaceCommand("Add Camera", addCamera));
                editInterface.addCommand(new EditInterfaceCommand("Add Manual Object", addManualObject));
                editInterface.addCommand(new EditInterfaceCommand("Add Child Node", addChildNode));
                destroyMovableObject = new EditInterfaceCommand("Remove", removeMovableObject);
                destroyChildNode = new EditInterfaceCommand("Remove", removeChildNode);
                foreach (MovableObjectDefinition movable in movableObjects.Values)
                {
                    addMovableObjectEdit(movable);
                }
                foreach (SceneNodeDefinition child in childNodes.Values)
                {
                    addChildNodeEdit(child);
                }
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
        /// Add a SceneNodeDefinition as a child node.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public void addChildNode(SceneNodeDefinition child)
        {
            childNodes.Add(child.Name, child);
            child.setParentNode(this);
            if (editInterface != null)
            {
                addChildNodeEdit(child);
            }
        }

        /// <summary>
        /// Remove a SceneNodeDefinition from the children.
        /// </summary>
        /// <param name="child">The child to remove.</param>
        public void removeChildNode(SceneNodeDefinition child)
        {
            childNodes.Remove(child.Name);
            child.setParentNode(null);
            if (editInterface != null)
            {
                childNodeEdits.removeSubInterface(child);
            }
        }

        /// <summary>
        /// Create the product of this SceneNode.
        /// </summary>
        /// <param name="instance">The instance to get the product.</param>
        /// <param name="scene">The scene to create the product into.</param>
        internal void createProduct(SimObjectBase instance, OgreSceneManager scene)
        {
            Identifier identifier = new Identifier(instance.Name, Name);
            SceneNode node = scene.SceneManager.createSceneNode(identifier.FullName);
            node.setPosition(instance.Translation);
            node.setOrientation(instance.Rotation);
            SceneNodeElement element = new SceneNodeElement(identifier, this.subscription, scene, node);
            foreach (MovableObjectDefinition movable in movableObjects.Values)
            {
                element.attachObject(movable);
            }
            foreach (SceneNodeDefinition child in childNodes.Values)
            {
                child.createAsChild(instance, scene, element);
            }
            instance.addElement(element);
            scene.SceneManager.getRootSceneNode().addChild(node);
        }
        
        /// <summary>
        /// The translation of this node relative to its parent. Does nothing
        /// for root definitions.
        /// </summary>
        [Editable]
        public Vector3 LocalTranslation { get; set; }

        /// <summary>
        /// The Rotation of this node relative to its parent. Does nothing
        /// for root definitions.
        /// </summary>
        [Editable]
        public Quaternion LocalRotation { get; set; }

        /// <summary>
        /// Function used to create all child scene nodes. This will construct
        /// the SceneNode a bit differently and add it as a child of element
        /// instead of the root node.
        /// </summary>
        /// <param name="instance">The instance to get the product.</param>
        /// <param name="scene">The scene to create the product into.</param>
        /// <param name="parentElement">The element of the parent node.</param>
        private void createAsChild(SimObjectBase instance, OgreSceneManager scene, SceneNodeElement parentElement)
        {
            Identifier identifier = new Identifier(instance.Name, Name);
            SceneNode node = scene.SceneManager.createSceneNode(identifier.FullName);
            node.setPosition(LocalTranslation);
            node.setOrientation(LocalRotation);
            SceneNodeElement element = new SceneNodeElement(identifier, this.subscription, scene, node);
            foreach (MovableObjectDefinition movable in movableObjects.Values)
            {
                element.attachObject(movable);
            }
            foreach (SceneNodeDefinition child in childNodes.Values)
            {
                child.createAsChild(instance, scene, element);
            }
            parentElement.addChild(element);
        }

        /// <summary>
        /// Helper function to add an EditInterface for an EntityDefinition.
        /// </summary>
        /// <param name="definition"></param>
        private void addMovableObjectEdit(MovableObjectDefinition definition)
        {
            EditInterface edit = definition.getEditInterface();
            edit.addCommand(destroyMovableObject);
            movableObjectEdits.addSubInterface(definition, edit);
        }

        private void addChildNodeEdit(SceneNodeDefinition child)
        {
            EditInterface edit = child.getEditInterface();
            edit.addCommand(destroyChildNode);
            childNodeEdits.addSubInterface(child, edit);
        }

        /// <summary>
        /// Helper function to get a name for a Moveable object being added to a
        /// scene node. This will return true if the user entered a valid name.
        /// </summary>
        /// <param name="callback">The callback to use.</param>
        /// <param name="name">This will contain the name the user entered last.</param>
        /// <returns>True if the user entered a valid name.</returns>
        private bool getMovableName(EditUICallback callback, out String name)
        {
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && findTopLevelNode().isMovableNameTaken(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            return accept;
        }

        /// <summary>
        /// Helper function to get a name for a SceneNode child being added to a
        /// scene node. This will return true if the user entered a valid name.
        /// </summary>
        /// <param name="callback">The callback to use.</param>
        /// <param name="name">This will contain the name the user entered last.</param>
        /// <returns>True if the user entered a valid name.</returns>
        private bool getChildNodeName(EditUICallback callback, out String name)
        {
            bool accept = callback.getInputString("Enter a name.", out name);
            SceneNodeDefinition topLevel = findTopLevelNode();
            while (accept && (name == topLevel.Name || topLevel.isNodeNameTaken(name)))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            return accept;
        }

        /// <summary>
        /// Helper function to set the parent node of this node.
        /// </summary>
        /// <param name="parent">The parent SceneNodeDefinition to set.</param>
        private void setParentNode(SceneNodeDefinition parent)
        {
            this.parentNode = parent;
        }

        /// <summary>
        /// Find the top level node of this SceneNodeDefinition graph.
        /// </summary>
        /// <returns></returns>
        private SceneNodeDefinition findTopLevelNode()
        {
            if (parentNode != null)
            {
                return parentNode.findTopLevelNode();
            }
            return this;
        }

        /// <summary>
        /// Check to see if a MovableObject exists that uses the specified name
        /// in any of the child nodes or this node.
        /// </summary>
        /// <param name="name">The name to check for.</param>
        /// <returns>True if the name is taken.</returns>
        private bool isMovableNameTaken(String name)
        {
            if (movableObjects.ContainsKey(name))
            {
                return true;
            }
            else
            {
                foreach (SceneNodeDefinition child in childNodes.Values)
                {
                    if (child.isMovableNameTaken(name))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check to see if a SceneNode exists that uses a specified name in any
        /// child nodes or this node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isNodeNameTaken(String name)
        {
            if (childNodes.ContainsKey(name))
            {
                return true;
            }
            else
            {
                foreach (SceneNodeDefinition child in childNodes.Values)
                {
                    if (child.isNodeNameTaken(name))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Callback to add an EntityDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addEntity(EditUICallback callback, EditInterfaceCommand command)
        {         
            String name;
            if (getMovableName(callback, out name))
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
            if (getMovableName(callback, out name))
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
            if (getMovableName(callback, out name))
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
            if (getMovableName(callback, out name))
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

        /// <summary>
        /// Callback to add a child node.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addChildNode(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            if (getChildNodeName(callback, out name))
            {
                addChildNode(new SceneNodeDefinition(name));
            }
        }

        /// <summary>
        /// Callback to remove a child node.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void removeChildNode(EditUICallback callback, EditInterfaceCommand command)
        {
            removeChildNode(childNodeEdits.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        #region Saveable Members

        private const String ENTITY_BASE = "Entity";
        private const String LOCAL_TRANS = "LocalTranslation";
        private const String LOCAL_ROT = "LocalRotation";
        private const string CHILD_BASE = "Child";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info"></param>
        private SceneNodeDefinition(LoadInfo info)
            :base(info)
        {
            LocalTranslation = info.GetVector3(LOCAL_TRANS);
            LocalRotation = info.GetQuaternion(LOCAL_ROT);
            for (int i = 0; info.hasValue(ENTITY_BASE + i); i++)
            {
                addMovableObjectDefinition(info.GetValue<MovableObjectDefinition>(ENTITY_BASE + i));
            }
            for (int i = 0; info.hasValue(CHILD_BASE + i); i++)
            {
                addChildNode(info.GetValue<SceneNodeDefinition>(CHILD_BASE + i));
            }
        }

        /// <summary>
        /// GetInfo function.
        /// </summary>
        /// <param name="info"></param>
        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(LOCAL_TRANS, LocalTranslation);
            info.AddValue(LOCAL_ROT, LocalRotation);
            int i = 0;
            foreach (MovableObjectDefinition entity in movableObjects.Values)
            {
                info.AddValue(ENTITY_BASE + i++, entity);
            }
            i = 0;
            foreach (SceneNodeDefinition child in childNodes.Values)
            {
                info.AddValue(CHILD_BASE + i++, child);
            }
        }

        #endregion Saveable Members
    }
}
