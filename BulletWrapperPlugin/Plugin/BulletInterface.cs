using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Resources;
using Engine.Command;

namespace BulletPlugin
{
    public class BulletInterface : PluginInterface
    {
        private static BulletInterface instance;
        UpdateTimer timer;
        BulletShapeFileManager fileManager = new BulletShapeFileManager(new BulletShapeRepository(), new BulletShapeBuilder());
        BulletDebugInterface debugInterface = null;

        public const String PluginName = "BulletPlugin";

        public static BulletInterface Instance
        {
            get
            {
                return instance;
            }
        }

        public BulletInterface()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Cannot create the BulletInterface more than once. Only call the constructor one time.");
            }
        }

        public void Dispose()
        {
            
        }

        public void initialize(PluginManager pluginManager)
        {
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Bullet Scene Definition", new CreateSimElementManager(BulletSceneDefinition.Create)));
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Transformed Bullet Scene Definition", new CreateSimElementManager(TransformedBulletSceneDefinition.CreateTransformed)));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Rigid Body", new CreateSimElement(RigidBodyDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Reshapeable Rigid Body", new CreateSimElement(ReshapeableRigidBodyDefinition.CreateReshapeable)));
            //pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Soft Body", new CreateSimElement(SoftBodyDefinition::Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Generic 6 Dof Constraint", new CreateSimElement(Generic6DofConstraintDefinition.Create)));
            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Fixed Constraint", new CreateSimElement(FixedConstraintDefinition.Create)));
            //pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Elipsoid Soft Body Provider", new CreateSimElement(EllipsoidSoftBodyProviderDefinition::Create)));
            //pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Bullet Soft Body Anchor", new CreateSimElement(SoftBodyAnchorDefinition::Create)));

	        pluginManager.addSubsystemResources("Bullet", fileManager);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.timer = mainTimer;
        }

        public string getName()
        {
            return PluginName;
        }

        public DebugInterface getDebugInterface()
        {
            if(debugInterface == null)
            {
                debugInterface = new BulletDebugInterface();
            }
            return debugInterface;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }

        public float ShapeMargin
        {
            get
            {
                return fileManager.ShapeBuilder.ShapeMargin;
            }
            set
            {
                fileManager.ShapeBuilder.ShapeMargin = value;
            }
        }

        internal UpdateTimer UpdateTimer
        {
            get
            {
                return timer;
            }
        }

        internal BulletShapeRepository ShapeRepository
        {
            get
            {
                return fileManager.ShapeRepository;
            }
        }
    }
}
