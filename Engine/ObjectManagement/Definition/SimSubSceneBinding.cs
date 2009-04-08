using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This is a binding between a SimSubSceneDefinition and a
    /// SimElementManagerDefiniton in a SimSceneDefinition.
    /// </summary>
    class SimSubSceneBinding : EditInterface
    {
        #region Static

        private static DestroyEditInterfaceCommand destroyCommand = new DestroyEditInterfaceCommand("destroySimSceneBinding", "Destroy", "Remove this SimObjectManagerDefinition", new DestroyEditInterfaceCommand.DestroySubObject(Destroy));

        private static void Destroy(Object target, EditUICallback callback, String subCommand)
        {

        }

        #endregion Static

        private List<EditableProperty> properties = new List<EditableProperty>();
        private SimSubSceneDefinition subSceneDef;

        public SimSubSceneBinding(SimSubSceneDefinition subSceneDef)
        {
            this.subSceneDef = subSceneDef;
        }
        
        public string getName()
        {
            return "SubSceneBinding";
        }

        public bool hasEditableProperties()
        {
            return true;
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            return properties;
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
            return true;
        }

        public EngineCommand getDestroyObjectCommand()
        {
            throw new NotImplementedException();
        }

        public object getCommandTargetObject()
        {
            return this;
        }
    }
}
