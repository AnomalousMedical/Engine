using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.Editing;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class defines an instance of a sim object.
    /// </summary>
    public class SimObjectDefinition : Saveable
    {
        #region Fields

        private Dictionary<String, SimElementDefinition> definitions = new Dictionary<String, SimElementDefinition>();
        private String name;
        private EditInterface editInterface = null;
        private Dictionary<EditInterfaceCommand, String> createCommands = new Dictionary<EditInterfaceCommand, String>();
        private EditInterfaceCommand destroySimElement;
        private EditInterfaceManager<SimElementDefinition> elementEditInterfaces;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the definition.
        /// </summary>
        /// <param name="instanceName">The name of the definition.</param>
        public SimObjectDefinition(String name)
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
                foreach (SimElementDefinition definition in definitions.Values)
                {
                    createElementInterface(definition);
                }
                foreach (EngineCommand command in PluginManager.Instance.getCreateSimElementCommands())
                {
                    EditInterfaceCommand createSimElement = new EditInterfaceCommand(command.PrettyName, createSimElementDefinition);
                    createCommands.Add(createSimElement, command.Name);
                    editInterface.addCommand(createSimElement);
                }
                destroySimElement = new EditInterfaceCommand("Remove", removeSimElement);
            }
            return editInterface;
        }

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
                SimElementDefinition definition = (SimElementDefinition)PluginManager.Instance.getCreateSimElementCommand(createCommands[command]).execute(name);
                this.addElement(definition);
            }
        }

        private void removeSimElement(EditUICallback callback, EditInterfaceCommand command)
        {
            removeElement(elementEditInterfaces.resolveSourceObject(callback.getSelectedEditInterface()));
        }

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

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, Name);
            int i = 0;
            foreach (SimElementDefinition element in definitions.Values)
            {
                info.AddValue(ELEMENTS_BASE + i, element);
            }
        }

        #endregion
    }
}
