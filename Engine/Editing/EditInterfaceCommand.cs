﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace Engine.Editing
{
    /// <summary>
    /// This is a command that can be activated on a particular EditInterface by
    /// being added to that interface. It is intended to allow the user to
    /// perform some action on a particular interface, such as adding a
    /// subinterface.
    /// </summary>
    [DoNotCopy]
    [DoNotSave]
    public class EditInterfaceCommand
    {
        private ExecuteFunc function;
        private String name;

        /// <summary>
        /// This delegate is used to call EditInterfaceCommands.
        /// </summary>
        /// <param name="callback">The UI callback.</param>
        public delegate void ExecuteFunc(EditUICallback callback);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the command. This will show up on a UI.</param>
        /// <param name="function">The funciton to execute when the command is run.</param>
        public EditInterfaceCommand(String name, ExecuteFunc function)
        {
            this.name = name;
            this.function = function;
        }

        /// <summary>
        /// Constructor. Use this verison if you don't care about having a ui callback.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="function">The funciton to exectue.</param>
        public EditInterfaceCommand(String name, Action function)
        {
            this.name = name;
            this.function = callback => function();
        }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="callback">A callback the functions can use to ask the user for additional input.</param>
        public void execute(EditUICallback callback)
        {
            function.Invoke(callback);
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
