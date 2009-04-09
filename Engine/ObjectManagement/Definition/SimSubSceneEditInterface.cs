using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    public class SimSubSceneEditInterface : EditInterface
    {
        #region Fields

        private SimSubSceneDefinition definition;
        private LinkedList<SimSubSceneBinding> bindings = new LinkedList<SimSubSceneBinding>();
        private ReflectedCollectionEditInterface<SimSubSceneBinding> bindingEditor;
        private DestroyEditInterfaceCommand destroyCommand;

        #endregion Fields

        #region Constructors

        public SimSubSceneEditInterface(SimSubSceneDefinition definition)
        {
            this.definition = definition;
            bindingEditor = new ReflectedCollectionEditInterface<SimSubSceneBinding>("Bindings", bindings);
            bindingEditor.setCommandTarget(this);
            bindingEditor.setDestroyPropertyCommand(new DestroyEditablePropertyCommand("destroySimElementManagerBinding", "Destroy SimElementManager Binding", "Destroy a binding to a SimElementManager.", new DestroyEditablePropertyCommand.DestroyProperty(destroyBinding)));
            bindingEditor.setCreatePropertyCommand(new CreateEditablePropertyCommand("createSimElemenManagerBinding", "Add SimElementManager Binding", "Add a binding to a SimElementManager.", new CreateEditablePropertyCommand.CreateProperty(createBinding)));
        }

        #endregion Constructors

        #region Functions

        public EditableProperty addBinding()
        {
            SimSubSceneBinding binding = new SimSubSceneBinding();
            bindings.AddLast(binding);
            return bindingEditor.addItem(binding);
        }

        public void removeBinding(SimSubSceneBinding binding)
        {
            bindings.Remove(binding);
            bindingEditor.removeItem(binding);
        }

        private EditableProperty createBinding(EditUICallback callback, String subCommand)
        {
            return addBinding();
        }

        private void destroyBinding(EditableProperty property, EditUICallback callback, String subCommand)
        {
            ReflectedObjectEditableProperty reflectedProp = (ReflectedObjectEditableProperty)property;
            removeBinding((SimSubSceneBinding)reflectedProp.getTargetObject());
        }

        #region EditInterface Members

        public string getName()
        {
            return definition.Name + " - Subscene";
        }

        public bool hasEditableProperties()
        {
            return true;
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return bindingEditor.getEditableProperties();
        }

        public EditablePropertyInfo getPropertyInfo()
        {
            return bindingEditor.getPropertyInfo();
        }

        public bool hasSubEditInterfaces()
        {
            return false;
        }

        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return null;
        }

        public bool hasCreateSubObjectCommands()
        {
            return false;
        }

        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            return null;
        }

        public bool hasDestroyObjectCommand()
        {
            return destroyCommand != null;
        }

        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            return destroyCommand;
        }

        public void setDestroyObjectCommand(DestroyEditInterfaceCommand destroyCommand)
        {
            this.destroyCommand = destroyCommand;
        }

        public bool canAddRemoveProperties()
        {
            return bindingEditor.canAddRemoveProperties();
        }

        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return bindingEditor.getCreatePropertyCommand();
        }

        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return bindingEditor.getDestroyPropertyCommand();
        }

        #endregion

        #endregion Functions
    }
}
