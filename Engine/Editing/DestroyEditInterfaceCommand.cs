using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class DestroyEditInterfaceCommand : EngineCommand
    {
        /// <summary>
        /// A delegate for destroying SubEditInterfaces.
        /// </summary>
        /// <param name="targetObject">The object this command will execute on. Allows sharing of command instances.</param>
        /// <param name="callback">The EditUICallback for additional user input.</param>
        /// <param name="subCommand">A SubCommand to run if required. This may be null if no SubCommand is required.</param>
        /// <returns>A new EditInterface to the object that was just created or null if it does not have an EditInterface.</returns>
        public delegate void DestroySubObject(Object targetObject, EditUICallback callback, String subCommand);

        private DestroySubObject command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">See EngineCommand.</param>
        /// <param name="prettyName">See EngineCommand.</param>
        /// <param name="helpText">See EngineCommand.</param>
        /// <param name="createCommand">A CreateSubObject delegate instance that will be called by the UI.</param>
        public DestroyEditInterfaceCommand(String name, String prettyName, String helpText, DestroySubObject createCommand)
            : base(name, prettyName, helpText, createCommand)
        {
            SubCommand = null;
            this.command = createCommand;
        }

        /// <summary>
        /// A SubCommand name to run when this command is called back. It will
        /// be passed to the delegate for further processing.
        /// </summary>
        public String SubCommand { get; set; }

        /// <summary>
        /// Execute the delegate directly. Will skip the reflection based invoke and should be faster.
        /// </summary>
        /// <param name="targetObject">The object this command will execute on. Allows sharing of command instances.</param>
        /// <param name="callback">The EditUICallback for additional user input.</param>
        public void execute(Object target, EditUICallback callback)
        {
            command.Invoke(target, callback, SubCommand);
        }
    }
}
