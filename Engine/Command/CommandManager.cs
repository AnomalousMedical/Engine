using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This class manages a set of commands.
    /// </summary>
    public class CommandManager
    {
        #region Fields

        Dictionary<String, EngineCommand> commands = new Dictionary<string, EngineCommand>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandManager()
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get a specific command from this manager.
        /// </summary>
        /// <param name="name">The name of the command to retrieve.</param>
        /// <returns>The command specified by name or null if the command is not found.</returns>
        public EngineCommand getCommand(String name)
        {
            EngineCommand command;
            commands.TryGetValue(name, out command);
            return command;
        }

        /// <summary>
        /// Get a list of all commands in this command manager.
        /// </summary>
        /// <returns>A list of all commands in this command manager.</returns>
        public IEnumerable<EngineCommand> getCommandList()
        {
            return commands.Values;
        }

        /// <summary>
        /// Add a command.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void addCommand(EngineCommand command)
        {
            commands.Add(command.Name, command);
        }

        /// <summary>
        /// Remove a command.
        /// </summary>
        /// <param name="command">The command to remove.</param>
        public void removeCommand(EngineCommand command)
        {
            commands.Remove(command.Name);
        }

        #endregion Functions
    }
}
