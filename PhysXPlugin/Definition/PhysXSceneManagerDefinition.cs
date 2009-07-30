using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Engine.Editing;
using Engine.Reflection;
using Engine.ObjectManagement;
using Engine.Saving;

namespace PhysXPlugin
{
    /// <summary>
    /// This is a definition class for PhysXSceneManagers.
    /// </summary>
    public class PhysXSceneManagerDefinition : SimElementManagerDefinition
    {
        #region Static

        static MemberScanner memberScanner = new MemberScanner();

        static PhysXSceneManagerDefinition()
        {
            memberScanner.ProcessFields = false;
            memberScanner.Filter = new EditableAttributeFilter();
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysXSceneManagerDefinition Create(String name, EditUICallback callback)
        {
            return new PhysXSceneManagerDefinition(name);
        }

        #endregion Static

        #region Fields

        private PhysSceneDesc sceneDesc = new PhysSceneDesc();
        private String name;
        private EditInterface editInterface = null;
        private LinkedList<PhysActorGroupPairDefinition> actorGroupPairs = new LinkedList<PhysActorGroupPairDefinition>();
        private EditInterface actorGroupPairsEdit;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        internal PhysXSceneManagerDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            sceneDesc.Dispose();
        }

        /// <summary>
        /// Create the SimElementManager this definition defines and return it.
        /// This may not be safe to call more than once per definition.
        /// </summary>
        /// <returns>The SimElementManager this definition is designed to create.</returns>
        public SimElementManager createSimElementManager()
        {
            PhysXSceneManager scene = PhysXInterface.Instance.createScene(this);
            foreach(PhysActorGroupPairDefinition pair in actorGroupPairs)
            {
                scene.PhysScene.setActorGroupPairFlags(pair.Group0, pair.Group1, pair.Flags);
            }
            return scene;
        }

        /// <summary>
        /// This will return the type the SimElementManager wishes to report
        /// itself as. Usually this will be the type of the class itself,
        /// however, it is possible to specify a superclass if desired. This
        /// will be the type reported to the SimSubScene. This should be the
        /// value returned by the SimElementManager this definition creates.
        /// </summary>
        /// <returns></returns>
        public Type getSimElementManagerType()
        {
            return typeof(PhysXSceneManager);
        }

        /// <summary>
        /// Add an ActorGroupPair to the defintion.
        /// </summary>
        /// <param name="group0"></param>
        /// <param name="group1"></param>
        /// <param name="flags"></param>
        public void addActorGroupPair(ushort group0, ushort group1, ContactPairFlag flags)
        {
            PhysActorGroupPairDefinition def = new PhysActorGroupPairDefinition(group0, group1, flags);
            actorGroupPairs.AddLast(def);
            if (actorGroupPairsEdit != null)
            {
                actorGroupPairsEdit.addEditableProperty(def);
            }
        }

        /// <summary>
        /// Remove the pair defined at index.
        /// </summary>
        /// <param name="index"></param>
        public void removeActorGroupPair(int index)
        {
            if (index < actorGroupPairs.Count)
            {
                PhysActorGroupPairDefinition def = actorGroupPairs.ElementAt(index);
                actorGroupPairs.Remove(def);
                if (actorGroupPairsEdit != null)
                {
                    actorGroupPairsEdit.removeEditableProperty(def);
                }
            }
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The PhysSceneDesc to create the scene with.
        /// </summary>
        public PhysSceneDesc SceneDesc
        {
            get
            {
                return sceneDesc;
            }
        }

        /// <summary>
	    /// Gravity vector.
	    /// </summary>
        [Editable]
	    public Vector3 Gravity 
	    { 
		    get
            {
                return sceneDesc.Gravity;
            }
		    set
            {
                sceneDesc.Gravity = value;
            }
	    }

	    /// <summary>
	    /// Maximum substep size.
	    /// </summary>
        [Editable]
        public float MaxTimestamp
	    { 
		    get
            {
                return sceneDesc.MaxTimestamp;
            }
		    set
            {
                sceneDesc.MaxTimestamp = value;
            }
	    }

	    /// <summary>
	    /// Maximum number of substeps to take.
	    /// </summary>
        [Editable]
        public uint MaxIter
	    {
		    get
            {
                return sceneDesc.MaxIter;
            }
		    set
            {
                sceneDesc.MaxIter = value;
            }
	    }

        [Editable]
        public PhysTimeStepMethod TimeStepMethod
        {
            get
            {
                return sceneDesc.TimeStepMethod;
            }
            set
            {
                sceneDesc.TimeStepMethod = value;
            }
        }

	    //public NxBounds3 maxBounds

	    //public nxscenelimts limits

        [Editable]
	    public SimulationType SimType
	    {
		    get
            {
                return sceneDesc.SimType;
            }
		    set
            {
                sceneDesc.SimType = value;
            }
	    }

	    /// <summary>
	    /// Enable/disable default ground plane.
	    /// </summary>
        [Editable]
        public bool GroundPlane
	    {
		    get
            {
                return sceneDesc.GroundPlane;
            }
		    set
            {
                sceneDesc.GroundPlane = value;
            }
	    }

	    /// <summary>
	    /// Enable/disable 6 planes around maxBounds (if available).
	    /// </summary>
        [Editable]
        public bool BoundsPlanes
	    {
		    get
            {
                return sceneDesc.BoundsPlanes;
            }
		    set
            {
                sceneDesc.BoundsPlanes = value;
            }
	    }

	    /// <summary>
	    /// Flags used to select scene options.
	    /// </summary>
        [Editable]
        public SceneFlags Flags
	    {
		    get
            {
                return sceneDesc.Flags;
            }
		    set
            {
                sceneDesc.Flags = value;
            }
	    }

	    //public NxUserScheduler customScheduler

	    /// <summary>
	    /// Allows the user to specify the stack size for the main simulation thread.
	    /// </summary>
        [Editable]
        public uint SimThreadStackSize
	    {
		    get
            {
                return sceneDesc.SimThreadStackSize;
            }
		    set
            {
                sceneDesc.SimThreadStackSize = value;
            }
	    }

        [Editable]
        public PhysThreadPriority SimThreadPriority
        {
            get
            {
                return sceneDesc.SimThreadPriority;
            }
            set
            {
                sceneDesc.SimThreadPriority = value;
            }
        }

	    /// <summary>
	    /// Allows the user to specify which (logical) 
	    /// processor to allocate the simulation thread to.
	    /// </summary>
        [Editable]
        public uint SimThreadMask
	    {
		    get
            {
                return sceneDesc.SimThreadMask;
            }
		    set
            {
                sceneDesc.SimThreadMask = value;
            }
	    }

	    /// <summary>
	    /// Sets the number of SDK managed worker threads used 
	    /// when running the simulation in parallel.
	    /// </summary>
        [Editable]
        public uint InternalThreadCount
	    {
		    get
            {
                return sceneDesc.InternalThreadCount;
            }
		    set
            {
                sceneDesc.InternalThreadCount = value;
            }
	    }

	    /// <summary>
	    /// Allows the user to specify the stack size for the worker threads created by the SDK.
	    /// </summary>
        [Editable]
        public uint WorkerThreadStackSize
	    {
		    get
            {
                return sceneDesc.WorkerThreadStackSize;
            }
		    set
            {
                sceneDesc.WorkerThreadStackSize = value;
            }
	    }

        [Editable]
        public PhysThreadPriority WorkerThreadPriority
        {
            get
            {
                return sceneDesc.WorkerThreadPriority;
            }
            set
            {
                sceneDesc.WorkerThreadPriority = value;
            }
        }

	    /// <summary>
	    /// Allows the user to specify which (logical) processor to allocate SDK 
	    /// internal worker threads to.
	    /// </summary>
        [Editable]
        public uint ThreadMask
	    {
		    get
            {
                return sceneDesc.ThreadMask;
            }
		    set
            {
                sceneDesc.ThreadMask = value;
            }
	    }

	    /// <summary>
	    /// Sets the number of SDK managed threads which will be processing background tasks.
	    /// </summary>
        [Editable]
        public uint BackgroundThreadCount
	    {
		    get
            {
                return sceneDesc.BackgroundThreadCount;
            }
		    set
            {
                sceneDesc.BackgroundThreadCount = value;
            }
	    }

        [Editable]
        public PhysThreadPriority BackgroundThreadPriority
        {
            get
            {
                return sceneDesc.BackgroundThreadPriority;
            }
            set
            {
                sceneDesc.BackgroundThreadPriority = value;
            }
        }

	    /// <summary>
	    /// Allows the user to specify which (logical) processor to allocate 
	    /// SDK background threads.
	    /// </summary>
        [Editable]
        public uint BackgroundThreadMask
	    {
		    get
            {
                return sceneDesc.BackgroundThreadMask;
            }
		    set
            {
                sceneDesc.BackgroundThreadMask = value;
            }
	    }

	    /// <summary>
	    /// Defines the up axis for your world. This is used to accelerate 
	    /// scene queries like raycasting or sweep tests. Internally, a 2D 
	    /// structure is used instead of a 3D one whenever an up axis is defined. 
	    /// This saves memory and is usually faster.
	    /// </summary>
	    /// <remarks>
	    /// Use 1 for Y = up, 2 for Z = up, or 0 to disable this feature. 
	    /// It is not possible to use X = up.  
	    /// WARNING: this is only used when maxBounds are defined. 
	    /// </remarks>
        [Editable]
        public uint UpAxis
	    {
		    get
            {
                return sceneDesc.UpAxis;
            }
		    set
            {
                sceneDesc.UpAxis = value;
            }
	    }

	    /// <summary>
	    /// Defines the subdivision level for acceleration structures used for scene queries.
	    /// </summary>
        [Editable]
        public uint SubdivisionLevel
	    {
		    get
            {
                return sceneDesc.SubdivisionLevel;
            }
		    set
            {
                sceneDesc.SubdivisionLevel = value;
            }
	    }

        [Editable]
        public PhysPruningStructure StaticStructure
        {
            get
            {
                return sceneDesc.StaticStructure;
            }
            set
            {
                sceneDesc.StaticStructure = value;
            }
        }

        [Editable]
        public PhysPruningStructure DynamicStructure
        {
            get
            {
                return sceneDesc.DynamicStructure;
            }
            set
            {
                sceneDesc.DynamicStructure = value;
            }
        }

	    /// <summary>
	    /// Hint for how much work should be done per simulation frame to 
	    /// rebuild the pruning structure.
	    /// </summary>
        [Editable]
        public uint DynamicTreeRebuildRateHint
	    {
		    get
            {
                return sceneDesc.DynamicTreeRebuildRateHint;
            }
		    set
            {
                sceneDesc.DynamicTreeRebuildRateHint = value;
            }
	    }

	    //void* userData, can implement with object/gcroot or even template

        [Editable]
        public PhysBroadPhaseType BpType
        {
            get
            {
                return sceneDesc.BpType;
            }
            set
            {
                sceneDesc.BpType = value;
            }
        }

	    /// <summary>
	    /// Defines the number of broadphase cells along the grid x-axis.
	    /// </summary>
        [Editable]
        public uint NbGridCellsX
	    {
		    get
            {
                return sceneDesc.NbGridCellsX;
            }
		    set
            {
                sceneDesc.NbGridCellsX = value;
            }
	    }

	    /// <summary>
	    /// Defines the number of broadphase cells along the grid y-axis.
	    /// </summary>
        [Editable]
        public uint NbGridCellsY
	    {
		    get
            {
                return sceneDesc.NbGridCellsY;
            }
		    set
            {
                sceneDesc.NbGridCellsY = value;
            }
	    }

	    /// <summary>
	    /// Defines the number of actors required to spawn a separate rigid body solver thread.
	    /// </summary>
        [Editable]
        public uint SolverBatchSize
	    {
            get
            {
                return sceneDesc.SolverBatchSize;
            }
            set
            {
                sceneDesc.SolverBatchSize = value;
            }
	    }

        #endregion Properties

        #region EditInterface

        /// <summary>
        /// Get an EditInterface.
        /// </summary>
        /// <returns>An EditInterface for the definition or null if there is no interface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, Name + " PhysX Scene", null);

                //Actor group pairs
                actorGroupPairsEdit = new EditInterface("Actor Group Pairs", addActorGroupPair, removeActorGroupPair);
                EditablePropertyInfo actorGroupPairsPropInfo = new EditablePropertyInfo();
                actorGroupPairsPropInfo.addColumn(new EditablePropertyColumn("Group 0", false));
                actorGroupPairsPropInfo.addColumn(new EditablePropertyColumn("Group 1", false));
                actorGroupPairsPropInfo.addColumn(new EditablePropertyColumn("Flags", false));
                actorGroupPairsEdit.setPropertyInfo(actorGroupPairsPropInfo);
                foreach (PhysActorGroupPairDefinition pair in actorGroupPairs)
                {
                    actorGroupPairsEdit.addEditableProperty(pair);
                }

                editInterface.addSubInterface(actorGroupPairsEdit);
            }
            return editInterface;
        }

        private void addActorGroupPair(EditUICallback callback)
        {
            PhysActorGroupPairDefinition binding = new PhysActorGroupPairDefinition();
            actorGroupPairs.AddLast(binding);
            if (actorGroupPairsEdit != null)
            {
                actorGroupPairsEdit.addEditableProperty(binding);
            }
        }

        private void removeActorGroupPair(EditUICallback callback, EditableProperty property)
        {
            actorGroupPairs.Remove((PhysActorGroupPairDefinition)property);
            if (actorGroupPairsEdit != null)
            {
                actorGroupPairsEdit.removeEditableProperty(property);
            }
        }

        #endregion

        #region Saveable Members

        private const String NAME = "DefinitionName";
        private const String SOLVER_BATCH_SIZE = "SolverBatchSize";
        private const String NB_GRID_CELLS_Y = "NbGridCellsY";
        private const String NB_GRID_CELLS_X = "NbGridCellsX";
        private const String DYNAMIC_TREE_REBUILD_RATE_HINT = "DynamicTreeRebuildRateHint";
        private const String SUBDIVISION_LEVEL = "SubdivisionLevel";
        private const String UP_AXIS = "UpAxis";
        private const String BACKGROUND_THREAD_MASK = "BackgroundThreadMask";
        private const String BACKGROUND_THRED_COUNT = "BackgroundThreadCount";
        private const String THREAD_MASK = "ThreadMask";
        private const String WORKER_THREAD_STACK_SIZE = "WorkerThreadStackSize";
        private const String INTERNAL_THREAD_COUNT = "InternalThreadCount";
        private const String SIM_THREAD_MASK = "SimThreadMask";
        private const String SIM_THREAD_STACK_SIZE = "SimThreadStackSize";
        private const String FLAGS = "Flags";
        private const String BOUNDS_PLANES = "BoundsPlanes";
        private const String GROUND_PLANE = "GroundPlane";
        private const String SIM_TYPE = "SimType";
        private const String MAX_ITER = "MaxIter";
        private const String MAX_TIMESTAMP = "MaxTimestamp";
        private const String GRAVITY = "Gravity";
        private const String TIME_STEP_METHOD = "TimeStepMethod";
        private const String SIM_THREAD_PRIORITY = "SimThreadPriority";
        private const String WORKER_THREAD_PRIORITY = "WorkerThreadPriority";
        private const String BACKGROUND_THREAD_PRIORITY = "BackgroundThreadPriority";
        private const String STATIC_STRUCTURE = "StaticStructure";
        private const String DYNAMIC_STRUCTURE = "DynamicStructure";
        private const String BP_TYPE = "BpType";
        private const String ACTOR_GROUP_PAIR_BASE = "ActorGroupPair";

        private PhysXSceneManagerDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
            SolverBatchSize = info.GetUInt32(SOLVER_BATCH_SIZE);
            NbGridCellsY = info.GetUInt32(NB_GRID_CELLS_Y);
            NbGridCellsX = info.GetUInt32(NB_GRID_CELLS_X);
            DynamicTreeRebuildRateHint = info.GetUInt32(DYNAMIC_TREE_REBUILD_RATE_HINT);
            SubdivisionLevel = info.GetUInt32(SUBDIVISION_LEVEL);
            UpAxis = info.GetUInt32(UP_AXIS);
            BackgroundThreadMask = info.GetUInt32(BACKGROUND_THREAD_MASK);
            BackgroundThreadCount = info.GetUInt32(BACKGROUND_THRED_COUNT);
            ThreadMask = info.GetUInt32(THREAD_MASK);
            WorkerThreadStackSize = info.GetUInt32(WORKER_THREAD_STACK_SIZE);
            InternalThreadCount = info.GetUInt32(INTERNAL_THREAD_COUNT);
            SimThreadMask = info.GetUInt32(SIM_THREAD_MASK);
            SimThreadStackSize = info.GetUInt32(SIM_THREAD_STACK_SIZE);
            Flags = info.GetValue<SceneFlags>(FLAGS);
            BoundsPlanes = info.GetBoolean(BOUNDS_PLANES);
            GroundPlane = info.GetBoolean(GROUND_PLANE);
            SimType = info.GetValue<SimulationType>(SIM_TYPE);
            MaxIter = info.GetUInt32(MAX_ITER);
            MaxTimestamp = info.GetFloat(MAX_TIMESTAMP);
            Gravity = info.GetVector3(GRAVITY);
            TimeStepMethod = info.GetValue<PhysTimeStepMethod>(TIME_STEP_METHOD);
            SimThreadPriority = info.GetValue<PhysThreadPriority>(SIM_THREAD_PRIORITY);
            WorkerThreadPriority = info.GetValue<PhysThreadPriority>(WORKER_THREAD_PRIORITY);
            BackgroundThreadPriority = info.GetValue<PhysThreadPriority>(BACKGROUND_THREAD_PRIORITY);
            StaticStructure = info.GetValue<PhysPruningStructure>(STATIC_STRUCTURE);
            DynamicStructure = info.GetValue<PhysPruningStructure>(DYNAMIC_STRUCTURE);
            BpType = info.GetValue<PhysBroadPhaseType>(BP_TYPE);
            for (int i = 0; info.hasValue(ACTOR_GROUP_PAIR_BASE + i); ++i)
            {
                actorGroupPairs.AddLast(info.GetValue<PhysActorGroupPairDefinition>(ACTOR_GROUP_PAIR_BASE + i));
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
            info.AddValue(SOLVER_BATCH_SIZE, SolverBatchSize);
            info.AddValue(NB_GRID_CELLS_Y, NbGridCellsY);
            info.AddValue(NB_GRID_CELLS_X, NbGridCellsX);
            info.AddValue(DYNAMIC_TREE_REBUILD_RATE_HINT, DynamicTreeRebuildRateHint);
            info.AddValue(SUBDIVISION_LEVEL, SubdivisionLevel);
            info.AddValue(UP_AXIS, UpAxis);
            info.AddValue(BACKGROUND_THREAD_MASK, BackgroundThreadMask);
            info.AddValue(BACKGROUND_THRED_COUNT, BackgroundThreadCount);
            info.AddValue(THREAD_MASK, ThreadMask);
            info.AddValue(WORKER_THREAD_STACK_SIZE, WorkerThreadStackSize);
            info.AddValue(INTERNAL_THREAD_COUNT, InternalThreadCount);
            info.AddValue(SIM_THREAD_MASK, SimThreadMask);
            info.AddValue(SIM_THREAD_STACK_SIZE, SimThreadStackSize);
            info.AddValue(FLAGS, Flags);
            info.AddValue(BOUNDS_PLANES, BoundsPlanes);
            info.AddValue(GROUND_PLANE, GroundPlane);
            info.AddValue(SIM_TYPE, SimType);
            info.AddValue(MAX_ITER, MaxIter);
            info.AddValue(MAX_TIMESTAMP, MaxTimestamp);
            info.AddValue(GRAVITY, Gravity);
            info.AddValue(TIME_STEP_METHOD, TimeStepMethod);
            info.AddValue(SIM_THREAD_PRIORITY, SimThreadPriority);
            info.AddValue(WORKER_THREAD_PRIORITY, WorkerThreadPriority);
            info.AddValue(BACKGROUND_THREAD_PRIORITY, BackgroundThreadPriority);
            info.AddValue(STATIC_STRUCTURE, StaticStructure);
            info.AddValue(DYNAMIC_STRUCTURE, DynamicStructure);
            info.AddValue(BP_TYPE, BpType);
            int i = 0;
            foreach (PhysActorGroupPairDefinition pair in actorGroupPairs)
            {
                info.AddValue(ACTOR_GROUP_PAIR_BASE + i++, pair);
            }
        }

        #endregion
    }
}
