using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class DestroyEditablePropertyCommand : EngineCommand
    {
        /// <summary>
        /// A delegate for destroying EditableProperties.
        /// </summary>
        /// <param name="targetObject">The object this command will execute on. Allows sharing of command instances.</param>
        /// <param name="callback">The EditUICallback for additional user input.</param>
        /// <param name="subCommand">A SubCommand to run if required. This may be null if no SubCommand is required.</param>
        public delegate void DestroyProperty(Object targetObject, EditUICallback callback, String subCommand);

        private DestroyProperty command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">See EngineCommand.</param>
        /// <param name="prettyName">See EngineCommand.</param>
        /// <param name="helpText">See EngineCommand.</param>
        /// <param name="createCommand">A CreateSubObject delegate instance that will be called by the UI.</param>
        public DestroyEditablePropertyCommand(String name, String prettyName, String helpText, DestroyProperty createCommand)
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
        /// <param name="subCommand">A SubCommand to run if required. This may be null if no SubCommand is required.</param>
        public void execute(Object target, EditUICallback callback, String subCommand)
        {
            command.Invoke(target, callback, subCommand);
        }
    }
}
