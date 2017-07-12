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
    public class SidescrollerCorePlugin : PluginInterface
    {
        public const String PluginName = "Anomalous.TilesetPlugin";

        public SidescrollerCorePlugin(PluginManager pluginManager)
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

            RegisterGamepadPlayer(builder, PlayerId.Player1, GamepadId.Pad1);
            RegisterGamepadPlayer(builder, PlayerId.Player2, GamepadId.Pad2);
            RegisterGamepadPlayer(builder, PlayerId.Player3, GamepadId.Pad3);
            RegisterGamepadPlayer(builder, PlayerId.Player4, GamepadId.Pad4);
        }

        private static void RegisterGamepadPlayer(ContainerBuilder builder, PlayerId playerId, GamepadId padId)
        {

            builder.RegisterType<PlayerControls>()
                .SingleInstance()
                .Keyed<PlayerControls>(playerId)
                .OnActivated(a =>
                {
                    var i = a.Instance;
                    //i.MoveRightEvent.addButton(KeyboardButtonCode.KC_D);
                    //i.MoveLeftEvent.addButton(KeyboardButtonCode.KC_A);
                    //i.MoveUpEvent.addButton(KeyboardButtonCode.KC_W);
                    //i.MoveDownEvent.addButton(KeyboardButtonCode.KC_S);
                    //i.JumpEvent.addButton(KeyboardButtonCode.KC_SPACE);

                    //i.MoveRightEvent.addButton(KeyboardButtonCode.KC_RIGHT);
                    //i.MoveLeftEvent.addButton(KeyboardButtonCode.KC_LEFT);
                    //i.MoveUpEvent.addButton(KeyboardButtonCode.KC_UP);
                    //i.MoveDownEvent.addButton(KeyboardButtonCode.KC_DOWN);
                    //i.JumpEvent.addButton(KeyboardButtonCode.KC_NUMPAD0);

                    i.MoveRightEvent.addButton(GamepadButtonCode.XInput_DPadRight, padId);
                    i.MoveLeftEvent.addButton(GamepadButtonCode.XInput_DPadLeft, padId);
                    i.MoveUpEvent.addButton(GamepadButtonCode.XInput_DPadUp, padId);
                    i.MoveDownEvent.addButton(GamepadButtonCode.XInput_DPadDown, padId);
                    i.JumpEvent.addButton(GamepadButtonCode.XInput_RightShoulder, padId);

                    i.Build(a.Context.Resolve<EventManager>());
                });

            builder.RegisterType<FireControls>()
                            .SingleInstance()
                            .Keyed<FireControls>(playerId)
                            .OnActivated(a =>
                            {
                                var i = a.Instance;
                                //i.Fire.addButton(MouseButtonCode.MB_BUTTON0);

                                //i.Fire.addButton(KeyboardButtonCode.KC_NUMPAD1);

                                i.Fire.addButton(GamepadButtonCode.XInput_RTrigger, padId);

                                i.Build(a.Context.Resolve<EventManager>());
                            });
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
