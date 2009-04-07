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

namespace PhysXPlugin
{
    /// <summary>
    /// This class defines and builds PhysActors.
    /// </summary>
    public sealed class PhysActorDefinition : PhysElementDefinition
    {
        #region Static

        private static MemberScanner memberScanner;

        static PhysActorDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        #endregion Static

        #region Fields

        private PhysActorDesc actorDesc = new PhysActorDesc();
        private PhysBodyDesc bodyDesc = new PhysBodyDesc();
        private bool dynamic = false;
        private String shapeName = null;
        private ReflectedEditInterface editInterface = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor. Takes a name and a PhysFactory to build with.
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        internal PhysActorDefinition(String name)
            :base(name)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this element with its factory so it can be built.
        /// </summary>
        /// <param name="subscene">The SimSubScene that will get the built product.</param>
        /// <param name="instance">The SimObject that will get the newly created element.</param>
        public override void register(SimSubScene subscene, SimObject instance)
        {
            if (subscene.hasSimElementManagerType(typeof(PhysXSceneManager)))
            {
                PhysXSceneManager sceneManager = subscene.getSimElementManager<PhysXSceneManager>();
                sceneManager.getPhysFactory().addActorDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add PhysActorDefinition {0} to SimSubScene {1} because it does not contain a PhysXSceneManager.", LogLevel.Warning, PhysXInterface.PluginName);
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
                editInterface = new ReflectedEditInterface(this, memberScanner);
            }
            return editInterface;
        }

        /// <summary>
        /// Create a new Element normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createProduct(SimObject instance, PhysXSceneManager scene)
        {
            if (dynamic)
            {
                actorDesc.Body = bodyDesc;
            }
            else
            {
                actorDesc.Body = null;
            }
            if (actorDesc.isValid())
            {
                Identifier actorId = new Identifier(instance.Name, this.Name);
                PhysActor actor = scene.createPhysActor(actorId, actorDesc);
                instance.addElement(new PhysActorElement(actor, scene, actorId, subscription));
            }
        }

        /// <summary>
        /// Create a new element staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createStaticProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        #endregion Functions

        #region Properties

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
    }
}
