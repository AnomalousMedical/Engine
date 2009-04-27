using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using Engine.Command;

namespace Engine.ObjectManagement
{
    public class GenericSimObjectDefinition : SimObjectDefinition
    {
        #region Fields

        private Dictionary<String, SimElementDefinition> definitions = new Dictionary<String, SimElementDefinition>();
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
            definitions.Add(definition.Name, definition);
            if (editInterface != null)
            {
                createElementInterface(definition);
            }
        }

        /// <summary>
        /// Remove a SimElementDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeElement(SimElementDefinition definition)
        {
            definition.setSimObjectDefinition(null);
            definitions.Remove(definition.Name);
            if (editInterface != null)
            {
                elementEditInterfaces.removeSubInterface(definition);
            }
        }

        /// <summary>
        /// Register with factories to build this definition into the given SimObject.
        /// </summary>
        /// <param name="instance">The SimObject that will get the built elements.</param>
        public void register(SimSubScene subScene, SimObject instance)
        {
            foreach (SimElementDefinition definition in definitions.Values)
            {
                definition.register(subScene, instance);
            }
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                elementEditInterfaces = new EditInterfaceManager<SimElementDefinition>(editInterface);
                destroySimElement = new EditInterfaceCommand("Remove", removeSimElementDefinition);
                foreach (SimElementDefinition definition in definitions.Values)
                {
                    createElementInterface(definition);
                }
                foreach (AddSimElementCommand command in PluginManager.Instance.getCreateSimElementCommands())
                {
                    EditInterfaceCommand createSimElement = new EditInterfaceCommand(command.Name, createSimElementDefinition);
                    createCommands.Add(createSimElement, command);
                    editInterface.addCommand(createSimElement);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Helper function to create SimElementDefinitions.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
        private void createSimElementDefinition(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.definitions.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                SimElementDefinition definition = createCommands[command].execute(name, callback);
                this.addElement(definition);
            }
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

        #endregion Properties

        #region Saveable Members

        private const String NAME = "Name";
        private const String ELEMENTS_BASE = "Element";

        /// <summary>
        /// Deserialize constructor.
        /// </summary>
        /// <param name="info"></param>
        private GenericSimObjectDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
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
            int i = 0;
            foreach (SimElementDefinition element in definitions.Values)
            {
                info.AddValue(ELEMENTS_BASE + i++, element);
            }
        }

        #endregion
    }
}
