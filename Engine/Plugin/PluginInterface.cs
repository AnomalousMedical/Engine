using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;

namespace Engine
{
    public delegate SimElementDefinition CreateElementDefinition(String name);
    public delegate SimElementManagerDefinition CreateElementManagerDefinition(String name);

    /// <summary>
    /// This is an interface for the main plugin class. Each plugin must define
    /// a single class that inherits from this class and implements a blank
    /// constructor. That constructor will be invoked via reflection and then
    /// the plugin will be initalized.
    /// </summary>
    public interface PluginInterface : IDisposable
    {
        /// <summary>
        /// Do any initialization steps that need to be done on the plugin. Done
        /// just after the plugin is loaded and will only be called one time.
        /// </summary>
        void initialize(PluginManager pluginManager);

        /// <summary>
        /// Set the classes from the platform that a plugin may be interested
        /// in. The timer can be subscribed to for updates and the EventManager
        /// will be updated with events every frame.
        /// </summary>
        /// <param name="mainTimer">The main update timer.</param>
        /// <param name="eventManager">The main event manager.</param>
        void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager);

        /// <summary>
        /// Get a name for this plugin. Care should be taken that this return
        /// value is unique. The best way would be to name it after the plugin
        /// dll.
        /// </summary>
        /// <returns>The name of the plugin.</returns>
        String getName();

        /// <summary>
        /// Get the DebugInterface for this plugin. This function might return
        /// null, which means that the plugin has no debug interface. Ideally
        /// plugins will not build their DebugInterface unless this funciton is
        /// called to speed up loading.
        /// </summary>
        /// <returns>The DebugInterface for the plugin or null if it does not have one.</returns>
        DebugInterface getDebugInterface();

        /// <summary>
        /// This function will create any debug commands for the plugin and add them to the commands list.
        /// </summary>
        /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
        void createDebugCommands(List<CommandManager> commands);

        /// <summary>
        /// This funciton will be called if a type name is missed on loading if the findType function is used on the PluginManager. 
        /// Plugins can use it to add any types that have been renamed to the RenamedTypeMap.
        /// </summary>
        /// <param name="renamedTypeMap">The map to add renamed types to.</param>
        void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap);
    }
}
