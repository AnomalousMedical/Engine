﻿using Engine;
using Engine.Command;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikInterface : PluginInterface
    {
        public const String PluginName = "BEPUikPlugin";

        public static BEPUikInterface Instance { get; private set; }

        private UpdateTimer timer;
        private BEPUikDebugInterface debugInterface;

        public BEPUikInterface()
        {
            Instance = this;
        }

        public void Dispose()
        {

        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create BEPU Ik Scene Definition", new CreateSimElementManager(BEPUikSceneDefinition.Create)));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Bone", new CreateSimElement(BEPUikBoneDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Drag Controller", new CreateSimElement(BEPUikDragControlDefinition.Create)));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Angular Joint", new CreateSimElement(BEPUikAngularJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Ball Socket Joint", new CreateSimElement(BEPUikBallSocketJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Distance Joint", new CreateSimElement(BEPUikDistanceJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Revolute Joint", new CreateSimElement(BEPUikRevoluteJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Swivel Hinge Joint", new CreateSimElement(BEPUikSwivelHingeJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Twist Joint", new CreateSimElement(BEPUikTwistJointDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Point on Line Joint", new CreateSimElement(BEPUikPointOnLineJointDefinition.Create)));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Distance Limit", new CreateSimElement(BEPUikDistanceLimitDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Swing Limit", new CreateSimElement(BEPUikSwingLimitDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create BEPU Ik Twist Limit", new CreateSimElement(BEPUikTwistLimitDefinition.Create)));
        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            timer = mainTimer;
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
            if (debugInterface == null)
            {
                debugInterface = new BEPUikDebugInterface();
            }
            return debugInterface;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }

        internal BEPUikScene createScene(BEPUikSceneDefinition definition)
        {
            return new BEPUikScene(definition, timer);
        }
    }
}
