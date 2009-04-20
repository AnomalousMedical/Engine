using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public delegate void EditInterfaceFunction(EditUICallback callback, EditInterfaceCommand caller);

    /// <summary>
    /// This is a command that can be activated on a particular EditInterface by
    /// being added to that interface. It is intended to allow the user to
    /// perform some action on a particular interface, such as adding a
    /// subinterface.
    /// </summary>
    public class EditInterfaceCommand
    {
        private EditInterfaceFunction function;
        private String name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the command. This will show up on a UI.</param>
        /// <param name="function">The funciton to execute when the command is run.</param>
        public EditInterfaceCommand(String name, EditInterfaceFunction function)
        {
            this.name = name;
            this.function = function;
        }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="callback">A callback the functions can use to ask the user for additional input.</param>
        public void execute(EditUICallback callback)
        {
            function.Invoke(callback, this);
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
