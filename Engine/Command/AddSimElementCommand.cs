using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;

namespace Engine.Command
{
    /// <summary>
    /// This delegate will create a SimElementDefinition. If for some reason
    /// there is an error or the definition was not otherwise created these
    /// functions can return null, which means no definition was created.
    /// </summary>
    /// <param name="name">The name of the SimElement.</param>
    /// <param name="callback">A callback for further user input.</param>
    /// <returns>A new SimElementDefinition or null if the definition is not to be created.</returns>
    public delegate SimElementDefinition CreateSimElement(String name, EditUICallback callback);

    public class AddSimElementCommand
    {
        private CreateSimElement function;
        private String name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the command. This will show up on a UI.</param>
        /// <param name="function">The funciton to execute when the command is run.</param>
        public AddSimElementCommand(String name, CreateSimElement function)
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
        public SimElementDefinition execute(String name, EditUICallback callback)
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
