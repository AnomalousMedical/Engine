#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxSceneDesc;

namespace PhysXWrapper
{

/// <summary>
/// Wrapper for NxSceneDesc.  Describes a scene.
/// </summary>
public ref class PhysSceneDesc
{
internal:
	//Pointer is avaliable internally for easy access.
	AutoPtr<NxSceneDesc> sceneDesc;

public:
	/// <summary>
	/// constructor sets to default (no gravity, no ground plane, collision detection on).
	/// </summary>
	PhysSceneDesc();

	/// <summary>
	/// Gravity vector.
	/// </summary>
	property Engine::Vector3 Gravity 
	{ 
		Engine::Vector3 get();
		void set(Engine::Vector3 vec); 
	}

	/// <summary>
	/// Maximum substep size.
	/// </summary>
	property float MaxTimestamp
	{ 
		float get();
		void set(float step);
	}

	/// <summary>
	/// Maximum number of substeps to take.
	/// </summary>
	property unsigned int MaxIter
	{
		unsigned int get();
		void set(unsigned int iter);
	}


	/// <summary>
	/// Used to specify the timestepping behavior. 
	/// </summary>
	property PhysTimeStepMethod TimeStepMethod
	{
		PhysTimeStepMethod get();
		void set(PhysTimeStepMethod value);
	}

	//property NxBounds3 maxBounds

	//property nxscenelimts limits

	property SimulationType SimType
	{
		SimulationType get();
		void set(SimulationType value);
	}

	/// <summary>
	/// Enable/disable default ground plane.
	/// </summary>
	property bool GroundPlane
	{
		bool get();
		void set(bool gp);
	}

	/// <summary>
	/// Enable/disable 6 planes around maxBounds (if available).
	/// </summary>
	property bool BoundsPlanes
	{
		bool get();
		void set(bool bp);
	}

	/// <summary>
	/// Flags used to select scene options.
	/// </summary>
	property SceneFlags Flags
	{
		SceneFlags get();
		void set(SceneFlags flags);
	}

	//property NxUserScheduler customScheduler

	/// <summary>
	/// Allows the user to specify the stack size for the main simulation thread.
	/// </summary>
	property unsigned int SimThreadStackSize
	{
		unsigned int get();
		void set(unsigned int size);
	}

	property PhysThreadPriority SimThreadPriority
	{
		PhysThreadPriority get();
		void set(PhysThreadPriority value);
	}

	/// <summary>
	/// Allows the user to specify which (logical) 
	/// processor to allocate the simulation thread to.
	/// </summary>
	property unsigned int SimThreadMask
	{
		unsigned int get();
		void set(unsigned int mask);
	}

	/// <summary>
	/// Sets the number of SDK managed worker threads used 
	/// when running the simulation in parallel.
	/// </summary>
	property unsigned int InternalThreadCount
	{
		unsigned int get();
		void set(unsigned int count);
	}

	/// <summary>
	/// Allows the user to specify the stack size for the worker threads created by the SDK.
	/// </summary>
	property unsigned int WorkerThreadStackSize
	{
		unsigned int get();
		void set(unsigned int size);
	}

	property PhysThreadPriority WorkerThreadPriority
	{
		PhysThreadPriority get();
		void set(PhysThreadPriority value);
	}

	/// <summary>
	/// Allows the user to specify which (logical) processor to allocate SDK 
	/// internal worker threads to.
	/// </summary>
	property unsigned int ThreadMask
	{
		unsigned int get();
		void set(unsigned int mask);
	}

	/// <summary>
	/// Sets the number of SDK managed threads which will be processing background tasks.
	/// </summary>
	property unsigned int BackgroundThreadCount
	{
		unsigned int get();
		void set(unsigned int count);
	}

	property PhysThreadPriority BackgroundThreadPriority
	{
		PhysThreadPriority get();
		void set(PhysThreadPriority value);
	}

	/// <summary>
	/// Allows the user to specify which (logical) processor to allocate 
	/// SDK background threads.
	/// </summary>
	property unsigned int BackgroundThreadMask
	{
		unsigned int get();
		void set(unsigned int mask);
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
	property unsigned int UpAxis
	{
		unsigned int get();
		void set(unsigned int axis);
	}

	/// <summary>
	/// Defines the subdivision level for acceleration structures used for scene queries.
	/// </summary>
	property unsigned int SubdivisionLevel
	{
		unsigned int get();
		void set(unsigned int level);
	}

	property PhysPruningStructure StaticStructure
	{
		PhysPruningStructure get();
		void set(PhysPruningStructure value);
	}

	property PhysPruningStructure DynamicStructure
	{
		PhysPruningStructure get();
		void set(PhysPruningStructure value);
	}

	/// <summary>
	/// Hint for how much work should be done per simulation frame to 
	/// rebuild the pruning structure.
	/// </summary>
	property unsigned int DynamicTreeRebuildRateHint
	{
		unsigned int get();
		void set(unsigned int hint);
	}

	//void* userData, can implement with object/gcroot or even template

	property PhysBroadPhaseType BpType
	{
		PhysBroadPhaseType get();
		void set(PhysBroadPhaseType value);
	}

	/// <summary>
	/// Defines the number of broadphase cells along the grid x-axis.
	/// </summary>
	property unsigned int NbGridCellsX
	{
		unsigned int get();
		void set(unsigned int cells);
	}

	/// <summary>
	/// Defines the number of broadphase cells along the grid y-axis.
	/// </summary>
	property unsigned int NbGridCellsY
	{
		unsigned int get();
		void set(unsigned int cells);
	}

	/// <summary>
	/// Defines the number of actors required to spawn a separate rigid body solver thread.
	/// </summary>
	property unsigned int SolverBatchSize
	{
		unsigned int get();
		void set(unsigned int size);
	}
};

}