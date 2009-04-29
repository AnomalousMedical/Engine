#include "StdAfx.h"
#include "..\include\PhysJointDesc.h"
#include "NxPhysics.h"
#include "PhysActor.h"

namespace PhysXWrapper
{

PhysJointDesc::PhysJointDesc(NxJointDesc* jointDesc)
:jointDesc(jointDesc),
actors(gcnew ActorArray(2))
{

}

void PhysJointDesc::setToDefault()
{
	jointDesc->setToDefault();
}

bool PhysJointDesc::isValid()
{
	return jointDesc->isValid();
}

void PhysJointDesc::setGlobalAnchor(Engine::Vector3 wsAnchor)
{
	jointDesc->setGlobalAnchor(NxVec3(wsAnchor.x, wsAnchor.y, wsAnchor.z));
}

void PhysJointDesc::setGlobalAxis(Engine::Vector3 wsAxis)
{
	jointDesc->setGlobalAxis(NxVec3(wsAxis.x, wsAxis.y, wsAxis.z));
}

PhysActor^ PhysJointDesc::Actor::get(int index) 
{
	return actors[index];
}

void PhysJointDesc::Actor::set(int index, PhysActor^ value) 
{
	if(value != nullptr)
	{
		actors[index] = value;
		jointDesc->actor[index] = value->actor;
	}
	else
	{
		actors[index] = nullptr;
		jointDesc->actor[index] = NULL;
	}
}

Engine::Vector3 PhysJointDesc::LocalNormal::get(int index) 
{
	NxVec3 v = jointDesc->localNormal[index];
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysJointDesc::LocalNormal::set(int index, Engine::Vector3 value) 
{
	jointDesc->localNormal[index] = NxVec3(value.x, value.y, value.z);
}

Engine::Vector3 PhysJointDesc::LocalAxis::get(int index) 
{
	NxVec3 v = jointDesc->localAxis[index];
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysJointDesc::LocalAxis::set(int index, Engine::Vector3 value) 
{
	jointDesc->localAxis[index] = NxVec3(value.x, value.y, value.z);
}

Engine::Vector3 PhysJointDesc::LocalAnchor::get(int index) 
{
	NxVec3 v = jointDesc->localAnchor[index];
	return Engine::Vector3(v.x, v.y, v.z);
}

void PhysJointDesc::LocalAnchor::set(int index, Engine::Vector3 value) 
{
	jointDesc->localAnchor[index] = NxVec3(value.x, value.y, value.z);
}

float PhysJointDesc::MaxForce::get() 
{
	return jointDesc->maxForce;
}

void PhysJointDesc::MaxForce::set(float value) 
{
	jointDesc->maxForce = value;
}

float PhysJointDesc::MaxTorque::get() 
{
	return jointDesc->maxTorque;
}

void PhysJointDesc::MaxTorque::set(float value) 
{
	jointDesc->maxTorque = value;
}

float PhysJointDesc::SolverExtrapolationFactor::get() 
{
	return jointDesc->solverExtrapolationFactor;
}

void PhysJointDesc::SolverExtrapolationFactor::set(float value) 
{
	jointDesc->solverExtrapolationFactor = value;
}

unsigned int PhysJointDesc::UseAccelerationSpring::get() 
{
	return jointDesc->useAccelerationSpring;
}

void PhysJointDesc::UseAccelerationSpring::set(unsigned int value) 
{
	jointDesc->useAccelerationSpring = value;
}

JointFlag PhysJointDesc::JointFlags::get() 
{
	return (JointFlag)jointDesc->jointFlags;
}

void PhysJointDesc::JointFlags::set(JointFlag value) 
{
	jointDesc->jointFlags = (NxU32)value;
}

}