using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Resources;
using Engine.Command;
using Autofac;
using Engine.Platform.Input;

namespace Anomalous.SidescrollerCore
{
    public class Plugin : PluginInterface
    {
        public const String PluginName = "Anomalous.TilesetPlugin";

        public Plugin(PluginManager pluginManager)
        {
            
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// This assmumes you have injected an IEventLayerKeyInjector<FireControls> and 
        /// IEventLayerKeyInjector<PlayerControls>.
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="builder"></param>
        public void initialize(PluginManager pluginManager, ContainerBuilder builder)
        {
            builder.Register(c => new EventLayerKeyInjector<FireControls>("none"))
                .SingleInstance()
                .As<IEventLayerKeyInjector<FireControls>>()
                .IfNotRegistered(typeof(IEventLayerKeyInjector<FireControls>));

            builder.Register(c => new EventLayerKeyInjector<PlayerControls>("none"))
                .SingleInstance()
                .As<IEventLayerKeyInjector<PlayerControls>>()
                .IfNotRegistered(typeof(IEventLayerKeyInjector<PlayerControls>));

            builder.RegisterType<PlayerControls>();
            builder.RegisterType<FireControls>();
        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string Name
        {
            get
            {
                return PluginName;
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }
    }
}
