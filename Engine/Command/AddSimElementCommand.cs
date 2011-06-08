using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;

namespace Engine.Command
{
    /// <summary>
    /// Callback to create and add a SimElement to a CompositeSimObject.
    /// </summary>
    /// <param name="name">The name of the SimElement.</param>
    /// <param name="callback">A callback for further user input.</param>
    /// <param name="simObjectDefinition">The SimObjectDefinition to add the SimElement to.</param>
    public delegate void CreateSimElement(String name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition);

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
        public void execute(String name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            function.Invoke(name, callback, simObjectDefinition);
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
