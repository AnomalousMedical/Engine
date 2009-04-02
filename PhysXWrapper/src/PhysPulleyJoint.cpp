#include "StdAfx.h"
#include "..\include\PhysPulleyJoint.h"
#include "NxPhysics.h"
#include "PhysPulleyJointDesc.h"
#include "PhysMotorDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysPulleyJoint::PhysPulleyJoint(Engine::Identifier^ name, NxPulleyJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(name, joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysPulleyJoint::saveToDesc(PhysPulleyJointDesc^ desc)
{
	typedJoint->saveToDesc(*desc->joint.Get());
	NxActor* actor0, *actor1;
	typedJoint->getActors(&actor0, &actor1);
	if(actor0)
	{
		desc->Actor[0] = *((PhysActorGCRoot*)actor0->userData);
	}
	if(actor1)
	{
		desc->Actor[1] = *((PhysActorGCRoot*)actor1->userData);
	}
}

void PhysPulleyJoint::loadFromDesc(PhysPulleyJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

void PhysPulleyJoint::setMotor(PhysMotorDesc^ motor)
{
	typedJoint->setMotor(*motor->motorDesc);
}

void PhysPulleyJoint::getMotor(PhysMotorDesc^ motor)
{
	typedJoint->getMotor(*motor->motorDesc);
}

void PhysPulleyJoint::setFlags(PulleyJointFlag flags)
{
	typedJoint->setFlags((NxU32)flags);
}

PulleyJointFlag PhysPulleyJoint::getFlags()
{
	return (PulleyJointFlag)typedJoint->getFlags();
}

NxJointDesc& PhysPulleyJoint::getDesc()
{
	NxPulleyJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}