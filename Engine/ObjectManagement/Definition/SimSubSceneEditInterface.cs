using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    class SimSubSceneEditInterface : EditInterface
    {
        #region Fields

        private SimSubSceneDefinition definition;
        private LinkedList<SimSubSceneBinding> bindings = new LinkedList<SimSubSceneBinding>();
        private ReflectedCollectionEditInterface<SimSubSceneBinding> bindingEditor;
        private LinkedList<EditInterface> editInterfaces = new LinkedList<EditInterface>();
        private CreateEditablePropertyCommand createBindingCommand;

        #endregion Fields

        #region Constructors

        public SimSubSceneEditInterface(SimSubSceneDefinition definition)
        {
            this.definition = definition;
            bindingEditor = new ReflectedCollectionEditInterface<SimSubSceneBinding>("Bindings", bindings);
            bindingEditor.setCommandTarget(this);
            bindingEditor.setDestroyPropertyCommand(new DestroyEditablePropertyCommand("destroySimElementManagerBinding", "Destroy SimElementManager Binding", "Destroy a binding to a SimElementManager.", new DestroyEditablePropertyCommand.DestroyProperty(destroyBinding)));
            bindingEditor.setCreatePropertyCommand(new CreateEditablePropertyCommand("createSimElemenManagerBinding", "Add SimElementManager Binding", "Add a binding to a SimElementManager.", new CreateEditablePropertyCommand.CreateProperty(createBinding)));
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

        #endregion

        #endregion Functions
    }
}
