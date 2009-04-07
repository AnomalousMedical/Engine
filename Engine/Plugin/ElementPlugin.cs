using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is an interface for the main plugin class for an ElementPlugin. An
    /// ElementPlugin can be any subsystem that will be created as part of a
    /// SimObject such as a plugin for graphics or physics. Each plugin must
    /// define a single class that inherits from this class and implements a
    /// blank constructor. That constructor will be invoked via reflection and
    /// then the plugin will be initalized and all of the commands it can
    /// execute will be recovered.
    /// </summary>
    public abstract class ElementPlugin : IDisposable
    {
        /// <summary>
        /// Do any initialization steps that need to be done on the plugin. Done
        /// just after the plugin is loaded and will only be called one time.
        /// </summary>
        public abstract void initialize();

        /// <summary>
        /// Get a CommandManager for creating SimElementManagers for this
        /// plugin.
        /// </summary>
        /// <returns>A CommandManager with all commands that create SimElementManagers.</returns>
        public abstract CommandManager getCreateSimElementManagerCommands();

        /// <summary>
        /// Get a CommandManager for creating SimElementDefinitions for this
        /// plugin. The commands must accept a single string that is the name of
        /// the element.
        /// </summary>
        /// <returns>A CommandManager with all commands that create SimElementDefinitions.</returns>
        public abstract CommandManager getCreateSimElementDefinitionCommands();

        /// <summary>
        /// Get the SimElementFactory that creates objects for this plugin.
        /// </summary>
        /// <returns>The SimElementFactory that creates objects for this plugin.</returns>
        public abstract SimElementFactory getElementFactory();

        /// <summary>
        /// Do any shutdown steps that need to be done on the plugin. This will
        /// be done just before the plugin is unloaded and will only be done one
        /// time.
        /// </summary>
        public abstract void Dispose();
    }
}
