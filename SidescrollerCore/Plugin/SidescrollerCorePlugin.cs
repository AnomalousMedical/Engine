using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Resources;
using Engine.Command;
using Engine.Platform.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        /// <param name="serviceCollection"></param>
        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            RegisterControls(serviceCollection, "none");

            RegisterGamepadPlayer(serviceCollection, PlayerId.Player1, GamepadId.Pad1);
            RegisterGamepadPlayer(serviceCollection, PlayerId.Player2, GamepadId.Pad2);
            RegisterGamepadPlayer(serviceCollection, PlayerId.Player3, GamepadId.Pad3);
            RegisterGamepadPlayer(serviceCollection, PlayerId.Player4, GamepadId.Pad4);
        }

        /// <summary>
        /// Register controls using the given layer key.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="layerKey"></param>
        public static void RegisterControls(IServiceCollection serviceCollection, Object layerKey)
        {
            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<FireControls<Player1>>>(s =>
            {
                return new EventLayerKeyInjector<FireControls<Player1>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<FireControls<Player2>>>(s =>
            {
                return new EventLayerKeyInjector<FireControls<Player2>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<FireControls<Player3>>>(s =>
            {
                return new EventLayerKeyInjector<FireControls<Player3>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<FireControls<Player4>>>(s =>
            {
                return new EventLayerKeyInjector<FireControls<Player4>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<PlayerControls<Player1>>>(s =>
            {
                return new EventLayerKeyInjector<PlayerControls<Player1>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<PlayerControls<Player2>>>(s =>
            {
                return new EventLayerKeyInjector<PlayerControls<Player2>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<PlayerControls<Player3>>>(s =>
            {
                return new EventLayerKeyInjector<PlayerControls<Player3>>(layerKey);
            });

            serviceCollection.TryAddSingleton<IEventLayerKeyInjector<PlayerControls<Player4>>>(s =>
            {
                return new EventLayerKeyInjector<PlayerControls<Player4>>(layerKey);
            });
        }

        private static void RegisterGamepadPlayer(IServiceCollection services, PlayerId playerId, GamepadId padId)
        {
            //var playerType = PlayerType.GetPlayerType(playerId);
            //Type[] typeArgs = { playerType };

            //var playerControlsType = typeof(PlayerControls<>);
            //var perPlayerPlayerControls = playerControlsType.MakeGenericType(typeArgs);

            var perPlayerPlayerControls = playerId.GetPlayerKeyedType(typeof(PlayerControls<>));
            var perPlayerPlayerKeyInjector = typeof(IEventLayerKeyInjector<>).MakeGenericType(perPlayerPlayerControls);

            services.AddSingleton(perPlayerPlayerControls, s =>
            {
                var i = Activator.CreateInstance(perPlayerPlayerControls, s.GetRequiredService(perPlayerPlayerKeyInjector)) as PlayerControls;
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

                i.Build(s.GetRequiredService<EventManager>());

                return i;
            });

            var perPlayerFireControls = playerId.GetPlayerKeyedType(typeof(FireControls<>));
            var perPlayerFireKeyInjector = typeof(IEventLayerKeyInjector<>).MakeGenericType(perPlayerFireControls);

            services.AddSingleton(perPlayerFireControls, s =>
            {
                var i = Activator.CreateInstance(perPlayerFireControls, s.GetRequiredService(perPlayerFireKeyInjector)) as FireControls;

                //i.Fire.addButton(MouseButtonCode.MB_BUTTON0);

                //i.Fire.addButton(KeyboardButtonCode.KC_NUMPAD1);

                i.Fire.addButton(GamepadButtonCode.XInput_RTrigger, padId);

                i.Build(s.GetRequiredService<EventManager>());

                return i;
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
