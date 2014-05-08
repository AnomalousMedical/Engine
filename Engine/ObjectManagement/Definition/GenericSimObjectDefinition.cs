using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using Engine.Command;
using Engine.Reflection;

namespace Engine.ObjectManagement
{
    public class GenericSimObjectDefinition : SimObjectDefinition, CompositeSimObjectDefinition
    {
        #region Static

        private static FilteredMemberScanner genericSimObjectScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static GenericSimObjectDefinition()
        {
            genericSimObjectScanner = new FilteredMemberScanner();
            genericSimObjectScanner.ProcessFields = false;
            genericSimObjectScanner.Filter = new EditableAttributeFilter();
        }

        #endregion Static

        #region Fields

        private LinkedList<SimElementDefinition> definitions = new LinkedList<SimElementDefinition>();
        private String name;
        private EditInterface editInterface = null;
        private Dictionary<EditInterfaceCommand, AddSimElementCommand> createCommands = new Dictionary<EditInterfaceCommand, AddSimElementCommand>();
        private EditInterfaceCommand destroySimElement;
        private EditInterfaceManager<SimElementDefinition> elementEditInterfaces;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the definition.
        /// </summary>
        /// <param name="instanceName">The name of the definition.</param>
        public GenericSimObjectDefinition(String name)
        {
            this.name = name;
            this.Translation = Vector3.Zero;
            this.Rotation = Quaternion.Identity;
            this.Enabled = true;
            this.Scale = Vector3.ScaleIdentity;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a SimElementDefinition. A element should only be added to
        /// one defintion.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        public void addElement(SimElementDefinition definition)
        {
            definition.setSimObjectDefinition(this);
            definitions.AddLast(definition);
            if (editInterface != null)
            {
                createElementInterface(definition);
            }
        }

        public void pasteElement(SimElementDefinition definition)
        {
            addElement(definition);
        }

        /// <summary>
        /// Remove a SimElementDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeElement(SimElementDefinition definition)
        {
            definition.setSimObjectDefinition(null);
            definitions.Remove(definition);
            if (editInterface != null)
            {
                elementEditInterfaces.removeSubInterface(definition);
            }
        }

        /// <summary>
        /// Register with factories to build this definition into the given SimObject.
        /// </summary>
        /// <param name="instance">The SimObject that will get the built elements.</param>
        public SimObjectBase register(SimSubScene subScene)
        {
            SimObjectBase instance = new GenericSimObject(name, Translation, Rotation, Scale, Enabled);
            foreach (SimElementDefinition definition in definitions)
            {
                definition.registerScene(subScene, instance);
            }
            return instance;
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, genericSimObjectScanner, name, null);//new EditInterface(name);
                editInterface.IconReferenceTag = EngineIcons.SimObject;
                elementEditInterfaces = new EditInterfaceManager<SimElementDefinition>(editInterface);
                destroySimElement = new EditInterfaceCommand("Remove", removeSimElementDefinition);
                foreach (SimElementDefinition definition in definitions)
                {
                    createElementInterface(definition);
                }
                foreach (AddSimElementCommand command in PluginManager.Instance.getCreateSimElementCommands())
                {
                    EditInterfaceCommand createSimElement = new EditInterfaceCommand(command.Name, createSimElementDefinition);
                    createCommands.Add(createSimElement, command);
                    editInterface.addCommand(createSimElement);
                }

                GenericClipboardEntry clipboardEntry = new GenericClipboardEntry(typeof(SimElementDefinition));
                clipboardEntry.PasteFunction = pasteObject;
                editInterface.ClipboardEntry = clipboardEntry;
            }
            return editInterface;
        }

        private void pasteObject(Object pasted)
        {
            pasteElement((SimElementDefinition)pasted);
        }

        /// <summary>
        /// Helper function to create SimElementDefinitions.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void createSimElementDefinition(EditUICallback callback, EditInterfaceCommand command)
        {
            callback.getInputString("Enter a name.", delegate(String result, ref String errorPrompt)
            {
                if (result == null || result == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                foreach (SimElementDefinition definition in definitions)
                {
                    if (definition.Name == result)
                    {
                        errorPrompt = "That name is already in use. Please provide another.";
                        return false;
                    }
                }

                createCommands[command].execute(result, callback, this);

                return true;
            });
        }

        /// <summary>
        /// Remove a SimElementDefinition.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void removeSimElementDefinition(EditUICallback callback, EditInterfaceCommand command)
        {
            removeElement(elementEditInterfaces.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        /// <summary>
        /// Helper function to create the EditInterface for a given SimElementDefinition.
        /// </summary>
        /// <param name="definition"></param>
        private void createElementInterface(SimElementDefinition definition)
        {
            EditInterface edit = definition.getEditInterface();
            edit.addCommand(destroySimElement);
            elementEditInterfaces.addSubInterface(definition, edit);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The instance name of this SimObject.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// The initial rotation of the sim object.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// The initial translation of the sim object.
        /// </summary>
        public Vector3 Translation { get; set; }

        /// <summary>
        /// The initial scale of the object.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// True if the object is enabled, false if it is disabled.
        /// </summary>
        [Editable]
        public bool Enabled { get; set; }

        #endregion Properties

        #region Saveable Members

        private const String NAME = "Name";
        private const String ELEMENTS_BASE = "Element";
        private const String TRANSLATION = "Translation";
        private const String ROTATION = "Rotation";
        private const String SCALE = "Scale";
        private const String ENABLED = "Enabled";

        /// <summary>
        /// Deserialize constructor.
        /// </summary>
        /// <param name="info"></param>
        private GenericSimObjectDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
            Translation = info.GetVector3(TRANSLATION);
            Rotation = info.GetQuaternion(ROTATION);
            Scale = info.GetVector3(SCALE);
            Enabled = info.GetBoolean(ENABLED);
            for (int i = 0; info.hasValue(ELEMENTS_BASE + i); i++)
            {
                addElement(info.GetValue<SimElementDefinition>(ELEMENTS_BASE + i));
            }
        }

        /// <summary>
        /// Serialize function.
        /// </summary>
        /// <param name="info"></param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, Name);
            info.AddValue(TRANSLATION, Translation);
            info.AddValue(ROTATION, Rotation);
            info.AddValue(SCALE, Scale);
            info.AddValue(ENABLED, Enabled);
            int i = 0;
            foreach (SimElementDefinition element in definitions)
            {
                info.AddValue(ELEMENTS_BASE + i++, element);
            }
        }

        #endregion
    }
}
