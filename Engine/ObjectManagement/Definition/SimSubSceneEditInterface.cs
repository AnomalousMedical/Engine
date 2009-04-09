using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    class SimSubSceneEditInterface : EditInterface
    {
        #region Static

        private static CreateEditablePropertyCommand createBindingCommand = new CreateEditablePropertyCommand("createSimElemenManagerBinding", "Add SimElementManager Binding", "Add a binding to a SimElementManager.", new CreateEditablePropertyCommand.CreateProperty(createBinding));
        private static DestroyEditablePropertyCommand destroyBindingCommand = new DestroyEditablePropertyCommand("destroySimElementManagerBinding", "Destroy SimElementManager Binding", "Destroy a binding to a SimElementManager.", new DestroyEditablePropertyCommand.DestroyProperty(destroyBinding));

        private static EditableProperty createBinding(Object target, EditUICallback callback, String subCommand)
        {
            SimSubSceneEditInterface editInterface = (SimSubSceneEditInterface)target;
            return editInterface.addBinding();
        }

        private static void destroyBinding(Object target, EditableProperty property, EditUICallback callback, String subCommand)
        {
            ReflectedObjectEditableProperty reflectedProp = (ReflectedObjectEditableProperty)property;
            SimSubSceneEditInterface editInterface = (SimSubSceneEditInterface)target;
            editInterface.removeBinding((SimSubSceneBinding)reflectedProp.getTargetObject());
        }

        #endregion Static

        #region Fields

        private SimSubSceneDefinition definition;
        private LinkedList<SimSubSceneBinding> bindings = new LinkedList<SimSubSceneBinding>();
        private ReflectedCollectionEditInterface<SimSubSceneBinding> bindingEditor;
        private LinkedList<EditInterface> editInterfaces = new LinkedList<EditInterface>();

        #endregion Fields

        #region Constructors

        public SimSubSceneEditInterface(SimSubSceneDefinition definition)
        {
            this.definition = definition;
            bindingEditor = new ReflectedCollectionEditInterface<SimSubSceneBinding>("Bindings", bindings);
            bindingEditor.setCommandTarget(this);
            bindingEditor.setDestroyPropertyCommand(destroyBindingCommand);
            bindingEditor.setCreatePropertyCommand(createBindingCommand);
            editInterfaces.AddLast(bindingEditor);
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

        #region EditInterface Members

        public string getName()
        {
            return definition.Name + " - Subscene";
        }

        public bool hasEditableProperties()
        {
            return false;
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return null;
        }

        public EditablePropertyInfo getPropertyInfo()
        {
            return null;
        }

        public bool hasSubEditInterfaces()
        {
            return true;
        }

        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            return editInterfaces;
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
            return true;
        }

        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            throw new NotImplementedException();
        }

        public bool canAddRemoveProperties()
        {
            return true;
        }

        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            throw new NotImplementedException();
        }

        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            throw new NotImplementedException();
        }

        public object getCommandTargetObject()
        {
            return this;
        }

        #endregion

        #endregion Functions
    }
}
