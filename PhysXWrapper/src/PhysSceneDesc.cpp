#include "StdAfx.h"
#include "..\include\PhysSceneDesc.h"

#include "NxPhysics.h"

#include "MathUtil.h"

namespace PhysXWrapper
{

using namespace System;

PhysSceneDesc::PhysSceneDesc()
:sceneDesc(new NxSceneDesc())
{
	
}

Engine::Vector3 PhysSceneDesc::Gravity::get() 
{
	return MathUtil::copyVector3(sceneDesc->gravity);
}

void PhysSceneDesc::Gravity::set(Engine::Vector3 vec) 
{ 
	MathUtil::copyVector3(vec, sceneDesc->gravity);
} 
 
float PhysSceneDesc::MaxTimestamp::get() 
{ 
	return sceneDesc->maxTimestep; 
}

void PhysSceneDesc::MaxTimestamp::set(float step) 
{ 
	sceneDesc->maxTimestep = step; 
} 

unsigned int PhysSceneDesc::MaxIter::get() 
{ 
	return sceneDesc->maxIter; 
}

void PhysSceneDesc::MaxIter::set(unsigned int iter) 
{ 
	sceneDesc->maxIter = iter; 
}

PhysTimeStepMethod PhysSceneDesc::TimeStepMethod::get() 
{ 
	return static_cast<PhysTimeStepMethod>(sceneDesc->timeStepMethod); 
}

void PhysSceneDesc::TimeStepMethod::set(PhysTimeStepMethod value) 
{ 
	sceneDesc->timeStepMethod = static_cast<NxTimeStepMethod>(value); 
}

bool PhysSceneDesc::GroundPlane::get() 
{ 
	return sceneDesc->groundPlane; 
}

void PhysSceneDesc::GroundPlane::set(bool gp) 
{ 
	sceneDesc->groundPlane = gp; 
}

bool PhysSceneDesc::BoundsPlanes::get() 
{ 
	return sceneDesc->boundsPlanes; 
}

void PhysSceneDesc::BoundsPlanes::set(bool bp) 
{ 
	sceneDesc->boundsPlanes = bp; 
}

SceneFlags PhysSceneDesc::Flags::get() 
{
	return (SceneFlags)sceneDesc->flags; 
}
void PhysSceneDesc::Flags::set(SceneFlags flags) 
{ 
	sceneDesc->flags = (NxSceneFlags)flags; 
}

unsigned int PhysSceneDesc::SimThreadStackSize::get()
{ 
	return sceneDesc->simThreadStackSize; 
}

void PhysSceneDesc::SimThreadStackSize::set(unsigned int size) 
{ 
	sceneDesc->simThreadStackSize = size; 
}

PhysThreadPriority PhysSceneDesc::SimThreadPriority::get()
{ 
	return static_cast<PhysThreadPriority>(sceneDesc->simThreadPriority); 
}

void PhysSceneDesc::SimThreadPriority::set(PhysThreadPriority value) 
{ 
	sceneDesc->simThreadPriority = static_cast<NxThreadPriority>(value);
}

unsigned int PhysSceneDesc::SimThreadMask::get() 
{ 
	return sceneDesc->simThreadMask; 
}

void PhysSceneDesc::SimThreadMask::set(unsigned int mask) 
{ 
	sceneDesc->simThreadMask = mask; 
}

unsigned int PhysSceneDesc::InternalThreadCount::get() 
{ 
	return sceneDesc->internalThreadCount; 
}

void PhysSceneDesc::InternalThreadCount::set(unsigned int count) 
{ 
	sceneDesc->internalThreadCount = count; 
}

unsigned int PhysSceneDesc::WorkerThreadStackSize::get() 
{ 
	return sceneDesc->workerThreadStackSize; 
}

void PhysSceneDesc::WorkerThreadStackSize::set(unsigned int size) 
{ 
	sceneDesc->workerThreadStackSize = size; 
}

PhysThreadPriority PhysSceneDesc::WorkerThreadPriority::get()
{ 
	return static_cast<PhysThreadPriority>(sceneDesc->workerThreadPriority); 
}

void PhysSceneDesc::WorkerThreadPriority::set(PhysThreadPriority value) 
{ 
	sceneDesc->workerThreadPriority = static_cast<NxThreadPriority>(value);
}

unsigned int PhysSceneDesc::ThreadMask::get() 
{ 
	return sceneDesc->threadMask; 
}

void PhysSceneDesc::ThreadMask::set(unsigned int mask) 
{ 
	sceneDesc->threadMask = mask; 
}

unsigned int PhysSceneDesc::BackgroundThreadCount::get() 
{ 
	return sceneDesc->backgroundThreadCount; 
}

void PhysSceneDesc::BackgroundThreadCount::set(unsigned int count) 
{ 
	sceneDesc->backgroundThreadCount = count; 
}

PhysThreadPriority PhysSceneDesc::BackgroundThreadPriority::get()
{ 
	return static_cast<PhysThreadPriority>(sceneDesc->backgroundThreadPriority); 
}

void PhysSceneDesc::BackgroundThreadPriority::set(PhysThreadPriority value) 
{ 
	sceneDesc->backgroundThreadPriority = static_cast<NxThreadPriority>(value);
}

unsigned int PhysSceneDesc::BackgroundThreadMask::get() 
{ 
	return sceneDesc->backgroundThreadMask; 
}

void PhysSceneDesc::BackgroundThreadMask::set(unsigned int mask) 
{
	sceneDesc->backgroundThreadMask = mask; 
}

unsigned int PhysSceneDesc::UpAxis::get() 
{
	return sceneDesc->upAxis; 
}

void PhysSceneDesc::UpAxis::set(unsigned int axis) 
{ 
	sceneDesc->upAxis = axis; 
}

unsigned int PhysSceneDesc::SubdivisionLevel::get() 
{ 
	return sceneDesc->subdivisionLevel; 
}

void PhysSceneDesc::SubdivisionLevel::set(unsigned int level) 
{ 
	sceneDesc->subdivisionLevel = level; 
}

PhysPruningStructure PhysSceneDesc::StaticStructure::get() 
{ 
	return static_cast<PhysPruningStructure>(sceneDesc->staticStructure); 
}

void PhysSceneDesc::StaticStructure::set(PhysPruningStructure level) 
{ 
	sceneDesc->staticStructure = static_cast<NxPruningStructure>(level); 
}

PhysPruningStructure PhysSceneDesc::DynamicStructure::get() 
{ 
	return static_cast<PhysPruningStructure>(sceneDesc->dynamicStructure); 
}

void PhysSceneDesc::DynamicStructure::set(PhysPruningStructure level) 
{ 
	sceneDesc->dynamicStructure = static_cast<NxPruningStructure>(level); 
}

unsigned int PhysSceneDesc::DynamicTreeRebuildRateHint::get()
{ 
	return sceneDesc->dynamicTreeRebuildRateHint; 
}

void PhysSceneDesc::DynamicTreeRebuildRateHint::set(unsigned int hint) 
{ 
	sceneDesc->dynamicTreeRebuildRateHint = hint; 
}

PhysBroadPhaseType PhysSceneDesc::BpType::get()
{ 
	return static_cast<PhysBroadPhaseType>(sceneDesc->bpType); 
}

void PhysSceneDesc::BpType::set(PhysBroadPhaseType value) 
{ 
	sceneDesc->bpType = static_cast<NxBroadPhaseType>(value); 
}

unsigned int PhysSceneDesc::NbGridCellsX::get() 
{ 
	return sceneDesc->nbGridCellsX; 
}

void PhysSceneDesc::NbGridCellsX::set(unsigned int cells) 
{ 
	sceneDesc->nbGridCellsX = cells; 
}

unsigned int PhysSceneDesc::NbGridCellsY::get() 
{ 
	return sceneDesc->nbGridCellsY; 
}

void PhysSceneDesc::NbGridCellsY::set(unsigned int cells) 
{ 
	sceneDesc->nbGridCellsY = cells; 
}

unsigned int PhysSceneDesc::SolverBatchSize::get() 
{ 
	return sceneDesc->solverBatchSize; 
}

void PhysSceneDesc::SolverBatchSize::set(unsigned int size) 
{ 
	sceneDesc->solverBatchSize = size; 
}


SimulationType PhysSceneDesc::SimType::get() 
{
	return (SimulationType)sceneDesc->simType;
}

void PhysSceneDesc::SimType::set(SimulationType value) 
{
	sceneDesc->simType = (NxSimulationType)value;
}

}