using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;

namespace PhysXPlugin
{
    /// <summary>
    /// This class defines and builds PhysActors.
    /// </summary>
    public sealed class PhysActorDefinition : PhysComponentDefinition
    {
        #region Fields

        private PhysActorDesc actorDesc = new PhysActorDesc();
        private PhysBodyDesc bodyDesc = new PhysBodyDesc();
        private bool dynamic = false;
        private String shapeName = "";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor. Takes a name and a PhysFactory to build with.
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        /// <param name="factory">A factory to build objects with.</param>
        internal PhysActorDefinition(String name, PhysFactory factory)
            :base(name, factory)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this component with its factory so it can be built.
        /// </summary>
        /// <param name="instance">The SimObject that will get the newly created component.</param>
        public override void register(SimObject instance)
        {
            factory.addActorDefinition(instance, this);
        }

        /// <summary>
        /// Create a new component normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new component staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal override void createStaticProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        #endregion Functions

        #region Properties

        //[InstanceVariableMember]
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

        //[InstanceVariableMember(typeof(ShapeCollection))]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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

        //[InstanceVariableMember]
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
