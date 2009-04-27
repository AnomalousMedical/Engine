using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;
using Engine.Editing;
using Engine.Reflection;
using Logging;
using Engine.ObjectManagement;
using Engine.Saving;

namespace PhysXPlugin
{
    /// <summary>
    /// This class defines and builds PhysActors.
    /// </summary>
    public sealed class PhysActorDefinition : PhysElementDefinition
    {
        #region Static

        private static MemberScanner actorMemberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PhysActorDefinition()
        {
            actorMemberScanner = new MemberScanner();
            actorMemberScanner.ProcessFields = false;
            actorMemberScanner.Filter = new EditableAttributeFilter();
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysActorDefinition Create(String name, EditUICallback callback)
        {
            return new PhysActorDefinition(name);
        }

        #endregion Static

        #region Fields

        private PhysActorDesc actorDesc = new PhysActorDesc();
        private PhysBodyDesc bodyDesc = new PhysBodyDesc();
        private bool dynamic = false;
        private String shapeName = null;
        private EditInterface editInterface = null;
        private EditInterfaceCommand destroyShape;
        private EditInterfaceManager<ShapeDefinition> shapeEdits;
        private LinkedList<ShapeDefinition> shapeDefinitions = new LinkedList<ShapeDefinition>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor. Takes a name and a PhysFactory to build with.
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        public PhysActorDefinition(String name)
            :base(name)
        {

        }

        /// <summary>
        /// Constructor. Bases the definition off of an exisiting actor.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        /// <param name="shapeName"></param>
        /// <param name="actor"></param>
        internal PhysActorDefinition(String name, String shapeName, PhysActor actor)
            :base(name)
        {
            this.shapeName = shapeName;
            actor.saveToDesc(actorDesc);
            Dynamic = actor.saveBodyToDesc(bodyDesc);
            List<PhysShape> shapes = actor.getShapes();
            foreach (PhysShape shape in shapes)
            {
                switch (shape.getShapeType())
                {
                    case PhysShapeType.NX_SHAPE_BOX:
                        BoxShapeDefinition boxShape = new BoxShapeDefinition();
                        ((PhysBoxShape)shape).saveToDesc((PhysBoxShapeDesc)boxShape.PhysShapeDesc);
                        addShape(boxShape);
                        break;
                }
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this element with its factory so it can be built.
        /// </summary>
        /// <param name="subscene">The SimSubScene that will get the built product.</param>
        /// <param name="instance">The SimObject that will get the newly created element.</param>
        public override void register(SimSubScene subscene, SimObjectBase instance)
        {
            if (subscene.hasSimElementManagerType(typeof(PhysXSceneManager)))
            {
                PhysXSceneManager sceneManager = subscene.getSimElementManager<PhysXSceneManager>();
                sceneManager.getPhysFactory().addActorDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add PhysActorDefinition {0} to SimSubScene {1} because it does not contain a PhysXSceneManager.", LogLevel.Warning, PhysXInterface.PluginName, Name, subscene.Name);
            }
        }

        /// <summary>
        /// Get an EditInterface for the SimElementDefinition so it can be
        /// modified.
        /// </summary>
        /// <returns>The EditInterface for this SimElementDefinition.</returns>
        public override EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, actorMemberScanner, Name + " - PhysActor", null);
                shapeEdits = new EditInterfaceManager<ShapeDefinition>(editInterface);
                editInterface.addCommand(new EditInterfaceCommand("Add Box Shape", addBoxShape));
                destroyShape = new EditInterfaceCommand("Remove", removeShape);
                foreach (ShapeDefinition shape in shapeDefinitions)
                {
                    addShapeEditInterface(shape);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Create a new Element normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createProduct(SimObjectBase instance, PhysXSceneManager scene)
        {
            if (dynamic)
            {
                actorDesc.Body = bodyDesc;
            }
            else
            {
                actorDesc.Body = null;
            }
            actorDesc.clearShapes();
            if (shapeName == null || shapeName == String.Empty)
            {
                foreach (ShapeDefinition shape in shapeDefinitions)
                {
                    actorDesc.addShape(shape.PhysShapeDesc);
                }
            }
            else
            {
                //assign shapes from shapecollection

            }
            if (actorDesc.isValid())
            {
                actorDesc.setGlobalPose(instance.Translation, instance.Rotation);
                Identifier actorId = new Identifier(instance.Name, this.Name);
                PhysActorElement actor = scene.createPhysActor(actorId, this);
                actor.ShapeName = shapeName;
                instance.addElement(actor);
            }
            else
            {
                Log.Default.sendMessage("Invalid PhysActorDesc in SimObject {0} ActorDesc {1}.", LogLevel.Warning, PhysXInterface.PluginName, instance.Name, this.Name);
            }
        }

        /// <summary>
        /// Create a new element staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createStaticProduct(SimObjectBase instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        public void addShape(ShapeDefinition physShape)
        {
            //actorDesc.addShape(physShape.PhysShapeDesc);
            shapeDefinitions.AddLast(physShape);
            if (editInterface != null)
            {
                addShapeEditInterface(physShape);
            }
        }

        public void removeShape(ShapeDefinition physShape)
        {
            //actorDesc.removeShape(physShape.PhysShapeDesc);
            shapeDefinitions.Remove(physShape);
            if (editInterface != null)
            {
                shapeEdits.removeSubInterface(physShape);
            }
        }

        private void addShapeEditInterface(ShapeDefinition physShape)
        {
            EditInterface shapeEdit = physShape.getEditInterface();
            shapeEdit.addCommand(destroyShape);
            shapeEdits.addSubInterface(physShape, shapeEdit);
        }

        private void removeShape(EditUICallback callback, EditInterfaceCommand command)
        {
            removeShape(shapeEdits.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void addBoxShape(EditUICallback callback, EditInterfaceCommand command)
        {
            addShape(new BoxShapeDefinition());
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The ActorDesc for this actor.
        /// </summary>
        internal PhysActorDesc ActorDesc
        {
            get
            {
                return actorDesc;
            }
        }

        [Editable("Density used during mass/inertia computation.")]
        public float Density
        {
            get
            {
                return actorDesc.Density;
            }
            set
            {
                actorDesc.Density = value;
            }
        }

        [Editable("The name of a collision shape to use. If this is specified any shapes added manually will be ignored.")/*(typeof(ShapeCollection))*/]
        public String ShapeName
        {
            get
            {
                return shapeName;
            }
            set
            {
                shapeName = value;
            }
        }

        [Editable("Flags that control the actor.")]
        public ActorFlag Flags
        {
            get
            {
                return (ActorFlag)actorDesc.Flags;
            }
            set
            {
                actorDesc.Flags = (uint)value;
            }
        }

        [Editable("The actor's group")]
        public ushort ActorGroup
        {
            get
            {
                return actorDesc.Group;
            }
            set
            {
                actorDesc.Group = value;
            }
        }

        [Editable("Flags that determine how contact with other actors is reported.")]
        public uint ContactReportFlags
        {
            get
            {
                return (uint)actorDesc.ContactReportFlags;
            }
            set
            {
                actorDesc.ContactReportFlags = (ContactPairFlag)value;
            }
        }

        [Editable("Enable to make the actor dynamic.")]
        public bool Dynamic
        {
            get
            {
                return dynamic;
            }
            set
            {
                dynamic = value;
            }
        }

        [Editable("Diagonal mass space inertia tensor in bodies mass frame. Set to all zeros to let the SDK compute it.")]
        public Vector3 MassSpaceIntertia
        {
            get
            {
                return bodyDesc.MassSpaceInertia;
            }
            set
            {
                bodyDesc.MassSpaceInertia = value;
            }
        }

        [Editable("Mass of body. Should not be zero. To make this actor static set dynamic to false.")]
        public float Mass
        {
            get
            {
                return bodyDesc.Mass;
            }
            set
            {
                bodyDesc.Mass = value;
            }
        }

        [Editable("Linear Velocity of the body.")]
        public Vector3 LinearVelocity
        {
            get
            {
                return bodyDesc.LinearVelocity;
            }
            set
            {
                bodyDesc.LinearVelocity = value;
            }
        }

        [Editable("Angular Velocity of the body.")]
        public Vector3 AngularVelocity
        {
            get
            {
                return bodyDesc.AngularVelocity;
            }
            set
            {
                bodyDesc.AngularVelocity = value;
            }
        }

        [Editable("The body's initial wake up counter.")]
        public float WakeUpCounter
        {
            get
            {
                return bodyDesc.WakeUpCounter;
            }
            set
            {
                bodyDesc.WakeUpCounter = value;
            }
        }

        [Editable("Linear damping applied to the body.")]
        public float LinearDamping
        {
            get
            {
                return bodyDesc.LinearDamping;
            }
            set
            {
                bodyDesc.LinearDamping = value;
            }
        }

        [Editable("Angular damping applied to the body.")]
        public float AngularDamping
        {
            get
            {
                return bodyDesc.AngularDamping;
            }
            set
            {
                bodyDesc.AngularDamping = value;
            }
        }

        [Editable("Maximum allowed angular velocity. Use a negative value to use the default.")]
        public float MaxAngularVelocity
        {
            get
            {
                return bodyDesc.MaxAngularVelocity;
            }
            set
            {
                bodyDesc.MaxAngularVelocity = value;
            }
        }

        [Editable("When CCD is globally enabled, it is still not performed if the motion distance of all points on the body is below this threshold.")]
        public float CCDMotionThreshold
        {
            get
            {
                return bodyDesc.CCDMotionThreshold;
            }
            set
            {
                bodyDesc.CCDMotionThreshold = value;
            }
        }

        [Editable("Flags that determine the properties of the rigid body.")]
        public BodyFlag BodyFlags
        {
            get
            {
                return (BodyFlag)bodyDesc.Flags;
            }
            set
            {
                bodyDesc.Flags = value;
            }
        }

        [Editable("Maximum linear velocity at which body can go to sleep. If negative, the global default will be used.")]
        public float SleepLinearVelocity
        {
            get
            {
                return bodyDesc.SleepLinearVelocity;
            }
            set
            {
                bodyDesc.SleepLinearVelocity = value;
            }
        }

        [Editable("Number of solver iterations performed when processing joint/contacts connected to this body.")]
        public uint SolverIterationCount
        {
            get
            {
                return bodyDesc.SolverIterationCount;
            }
            set
            {
                bodyDesc.SolverIterationCount = value;
            }
        }

        [Editable("Threshold for the energy-based sleeping algorithm. Only used when the NX_BF_ENERGY_SLEEP_TEST flag is set.")]
        public float SleepEnergyThreshold
        {
            get
            {
                return bodyDesc.SleepEnergyThreshold;
            }
            set
            {
                bodyDesc.SleepEnergyThreshold = value;
            }
        }

        [Editable("Damping factor for bodies that are about to sleep.")]
        public float SleepDamping
        {
            get
            {
                return bodyDesc.SleepDamping;
            }
            set
            {
                bodyDesc.SleepDamping = value;
            }
        }

        [Editable("The force threshold for contact reports.")]
        public float ContactReportThreshold
        {
            get
            {
                return bodyDesc.ContactReportThreshold;
            }
            set
            {
                bodyDesc.ContactReportThreshold = value;
            }
        }

        #endregion Properties

        #region Saveable Members

        private const String DENSITY = "Density";
        private const String SHAPE_NAME = "ShapeName";
        private const String FLAGS = "Flags";
        private const String ACTOR_GROUP = "ActorGroup";
        private const String CONTACT_REPORT_FLAGS = "ContactReportFlags";
        private const String DYNAMIC = "Dynamic";
        private const String MASS_SPACE_INERTIA = "MassSpaceIntertia";
        private const String MASS = "Mass";
        private const String LINEAR_VELOCITY = "LinearVelocity";
        private const String ANGULAR_VELOCITY = "AngularVelocity";
        private const String WAKE_UP_COUNTER = "WakeUpCounter";
        private const String LINEAR_DAMPING = "LinearDamping";
        private const String ANGULAR_DAMPING = "AngularDamping";
        private const String MAX_ANGULAR_VELOCITY = "MaxAngularVelocity";
        private const String CCD_MOTION_THREASHOLD = "CCDMotionThreshold";
        private const String BODY_FLAGS = "BodyFlags";
        private const String SLEEP_LINEAR_VELOCITY = "SleepLinearVelocity";
        private const String SOLVER_ITERATION_COUNT = "SolverIterationCount";
        private const String SLEEP_ENERGY_THRESHOLD = "SleepEnergyThreshold";
        private const String SLEEP_DAMPING = "SleepDamping";
        private const String CONTACT_REPORT_THRESHOLD = "ContactReportThreshold";
        private const String SHAPE_DEF_BASE = "ShapeDefinition";

        private PhysActorDefinition(LoadInfo info)
            :base(info)
        {
            Density = info.GetFloat(DENSITY);
            ShapeName = info.GetString(SHAPE_NAME);
            Flags = info.GetValue<ActorFlag>(FLAGS);
            ActorGroup = info.GetUInt16(ACTOR_GROUP);
            ContactReportFlags = info.GetUInt32(CONTACT_REPORT_FLAGS);
            Dynamic = info.GetBoolean(DYNAMIC);
            MassSpaceIntertia = info.GetVector3(MASS_SPACE_INERTIA);
            Mass = info.GetFloat(MASS);
            LinearVelocity = info.GetVector3(LINEAR_VELOCITY);
            AngularVelocity = info.GetVector3(ANGULAR_VELOCITY);
            WakeUpCounter = info.GetFloat(WAKE_UP_COUNTER);
            LinearDamping = info.GetFloat(LINEAR_DAMPING);
            AngularDamping = info.GetFloat(ANGULAR_DAMPING);
            MaxAngularVelocity = info.GetFloat(MAX_ANGULAR_VELOCITY);
            CCDMotionThreshold = info.GetFloat(CCD_MOTION_THREASHOLD);
            BodyFlags = info.GetValue<BodyFlag>(BODY_FLAGS);
            SleepLinearVelocity = info.GetFloat(SLEEP_LINEAR_VELOCITY);
            SolverIterationCount = info.GetUInt32(SOLVER_ITERATION_COUNT);
            SleepEnergyThreshold = info.GetFloat(SLEEP_ENERGY_THRESHOLD);
            SleepDamping = info.GetFloat(SLEEP_DAMPING);
            ContactReportThreshold = info.GetFloat(CONTACT_REPORT_THRESHOLD);
            for (int i = 0; info.hasValue(SHAPE_DEF_BASE + i); i++)
            {
                addShape(info.GetValue<ShapeDefinition>(SHAPE_DEF_BASE + i));
            }
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(DENSITY, Density);
            info.AddValue(SHAPE_NAME, ShapeName);
            info.AddValue(FLAGS, Flags);
            info.AddValue(ACTOR_GROUP, ActorGroup);
            info.AddValue(CONTACT_REPORT_FLAGS, ContactReportFlags);
            info.AddValue(DYNAMIC, Dynamic);
            info.AddValue(MASS_SPACE_INERTIA, MassSpaceIntertia);
            info.AddValue(MASS, Mass);
            info.AddValue(LINEAR_VELOCITY, LinearVelocity);
            info.AddValue(ANGULAR_VELOCITY, AngularVelocity);
            info.AddValue(WAKE_UP_COUNTER, WakeUpCounter);
            info.AddValue(LINEAR_DAMPING, LinearDamping);
            info.AddValue(ANGULAR_DAMPING, AngularDamping);
            info.AddValue(MAX_ANGULAR_VELOCITY, MaxAngularVelocity);
            info.AddValue(CCD_MOTION_THREASHOLD, CCDMotionThreshold);
            info.AddValue(BODY_FLAGS, BodyFlags);
            info.AddValue(SLEEP_LINEAR_VELOCITY, SleepLinearVelocity);
            info.AddValue(SOLVER_ITERATION_COUNT, SolverIterationCount);
            info.AddValue(SLEEP_ENERGY_THRESHOLD, SleepEnergyThreshold);
            info.AddValue(SLEEP_DAMPING, SleepDamping);
            info.AddValue(CONTACT_REPORT_THRESHOLD, ContactReportThreshold);
            int i = 0;
            foreach (ShapeDefinition shape in shapeDefinitions)
            {
                info.AddValue(SHAPE_DEF_BASE + i++, shape);
            }
        }

        #endregion
    }
}
