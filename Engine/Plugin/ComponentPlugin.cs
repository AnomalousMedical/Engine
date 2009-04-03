using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is an interface for the main plugin class for a ComponentPlugin. A
    /// ComponentPlugin can be any subsystem that will be created as part of a
    /// SimObject such as a plugin for graphics or physics. Each plugin must
    /// define a single class that inherits from this class and implements a
    /// blank constructor. That constructor will be invoked via reflection and
    /// then the plugin will be initalized and all of the commands it can
    /// execute will be recovered.
    /// </summary>
    public abstract class ComponentPlugin : IDisposable
    {
        /// <summary>
        /// Do any initialization steps that need to be done on the plugin. Done
        /// just after the plugin is loaded and will only be called one time per
        /// load.
        /// </summary>
        public abstract void initialize();

        /// <summary>
        /// Do any shutdown steps that need to be done on the plugin. This will
        /// be done just before the plugin is unloaded and will only be done one
        /// time.
        /// </summary>
        public abstract void shutDown();

        /// <summary>
        /// Get a CommandManager for creating SimComponentManagers for this
        /// plugin.
        /// </summary>
        /// <returns>A CommandManager with all commands that create SimComponentManagers.</returns>
        public abstract CommandManager getCreateSimComponentManagerCommands();

        /// <summary>
        /// Get a CommandManager for creating SimComponentDefinitions for this
        /// plugin.
        /// </summary>
        /// <returns>A CommandManager with all commands that create SimComponentDefinitions.</returns>
        public abstract CommandManager getCreateSimComponentDefinitionCommands();

        /// <summary>
        /// Dispose function.
        /// </summary>
        public abstract void Dispose();
    }
}
