using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;

namespace Engine.Command
{
    /// <summary>
    /// This delegate will create a SimElementManagerDefinition. If for some
    /// reason there is an error or the definition was not otherwise created
    /// these functions can return null, which means no definition was created.
    /// </summary>
    /// <param name="name">The name of the SimElement.</param>
    /// <param name="callback">A callback for further user input.</param>
    /// <returns>A new SimElementManagerDefinition or null if the definition is not to be created.</returns>
    public delegate SimElementManagerDefinition CreateSimElementManager(String name, EditUICallback callback);

    public class AddSimElementManagerCommand
    {
        private CreateSimElementManager function;
        private String name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the command. This will show up on a UI.</param>
        /// <param name="function">The funciton to execute when the command is run.</param>
        public AddSimElementManagerCommand(String name, CreateSimElementManager function)
        {
            this.name = name;
            this.function = function;
        }

        /// <summary>
        /// Execute the function. This will return a new SimElementDefinition or
        /// null if none was created. This is not considered an error and UI's
        /// should just ignore that the command was executed if null is
        /// returned.
        /// </summary>
        /// <param name="callback">A callback the functions can use to ask the user for additional input.</param>
        public SimElementManagerDefinition execute(String name, EditUICallback callback)
        {
            return function.Invoke(name, callback);
        }

        /// <summary>
        /// The name of the command.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }
    }
}
