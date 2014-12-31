using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Logging;
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

        internal static void Create(String name, EditUICallback callback, CompositeSimObjectDefinition simObjectDef)
        {
            simObjectDef.addElement(new SceneNodeDefinition(name));
        }

        private static FilteredMemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SceneNodeDefinition()
        {
            memberScanner = new FilteredMemberScanner();
            memberScanner.ProcessFields = false;
            EditableAttributeFilter filter = new EditableAttributeFilter();
            memberScanner.Filter = filter;
        }

        #endregion Static

        private EditInterface editInterface;
        private Dictionary<String, MovableObjectDefinition> movableObjects = new Dictionary<String, MovableObjectDefinition>();
        private Dictionary<String, SceneNodeDefinition> childNodes = new Dictionary<string, SceneNodeDefinition>();
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
        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
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
        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, Name + " Scene Node", null);
                var movableObjectEdits = editInterface.createEditInterfaceManager<MovableObjectDefinition>();
                var childNodeEdits = editInterface.createEditInterfaceManager<SceneNodeDefinition>();
                editInterface.addCommand(new EditInterfaceCommand("Add Entity", addEntity));
                editInterface.addCommand(new EditInterfaceCommand("Add Light", addLight));
                editInterface.addCommand(new EditInterfaceCommand("Add Camera", addCamera));
                editInterface.addCommand(new EditInterfaceCommand("Add Manual Object", addManualObject));
                editInterface.addCommand(new EditInterfaceCommand("Add Child Node", addChildNode));
                editInterface.IconReferenceTag = EngineIcons.Node;
                movableObjectEdits.addCommand(new EditInterfaceCommand("Remove", removeMovableObject));
                childNodeEdits.addCommand(new EditInterfaceCommand("Remove", removeChildNode));
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
                editInterface.removeSubInterface(definition);
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
                editInterface.removeSubInterface(child);
            }
        }

        /// <summary>
        /// Create the product of this SceneNode.
        /// </summary>
        /// <param name="instance">The instance to get the product.</param>
        /// <param name="scene">The scene to create the product into.</param>
        internal void createProduct(SimObjectBase instance, OgreSceneManager scene)
        {
            SceneNode node = scene.SceneManager.createSceneNode(instance.Name + Name);
            node.setPosition(instance.Translation);
            node.setOrientation(instance.Rotation);
            SceneNodeElement element = new SceneNodeElement(Name, scene, node, scene.SceneManager.getRootSceneNode());
            instance.addElement(element);
            foreach (MovableObjectDefinition movable in movableObjects.Values)
            {
                element.attachObject(movable);
            }
            foreach (SceneNodeDefinition child in childNodes.Values)
            {
                child.createAsChild(instance, scene, element);
            }
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
            SceneNode node = scene.SceneManager.createSceneNode(instance.Name + Name);
            node.setPosition(LocalTranslation);
            node.setOrientation(LocalRotation);
            SceneNodeElement element = new SceneNodeElement(Name, scene, node, parentElement.SceneNode);
            instance.addWeakElement(element);
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
            editInterface.addSubInterface(definition, definition.getEditInterface());
        }

        private void addChildNodeEdit(SceneNodeDefinition child)
        {
            editInterface.addSubInterface(child, child.getEditInterface());
        }

        private bool validateMovableName(String input, ref String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (findTopLevelNode().isMovableNameTaken(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            return true;
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
        private void addEntity(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (validateMovableName(input, ref errorPrompt))
                {
                    addMovableObjectDefinition(new EntityDefinition(input));
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Callback to add a CameraDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addCamera(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (validateMovableName(input, ref errorPrompt))
                {
                    addMovableObjectDefinition(new CameraDefinition(input));
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Callback to add a ManualObjectDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addManualObject(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (validateMovableName(input, ref errorPrompt))
                {
                    addMovableObjectDefinition(new ManualObjectDefinition(input));
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Callback to add a LightDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addLight(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (validateMovableName(input, ref errorPrompt))
                {
                    addMovableObjectDefinition(new LightDefinition(input));
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Callback to remove an MovableObjectDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void removeMovableObject(EditUICallback callback)
        {
            removeMovableObjectDefinition(editInterface.resolveSourceObject<MovableObjectDefinition>(callback.getSelectedEditInterface()));
        }

        /// <summary>
        /// Callback to add a child node.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void addChildNode(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                SceneNodeDefinition topLevel = findTopLevelNode();
                if (input == topLevel.Name || topLevel.isNodeNameTaken(input))
                {
                    errorPrompt = "That name is already in use. Please provide another.";
                    return false;
                }

                addChildNode(new SceneNodeDefinition(input));
                return true;
            });
        }

        /// <summary>
        /// Callback to remove a child node.
        /// </summary>
        /// <param name="callback"></param>
        private void removeChildNode(EditUICallback callback)
        {
            removeChildNode(editInterface.resolveSourceObject<SceneNodeDefinition>(callback.getSelectedEditInterface()));
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
