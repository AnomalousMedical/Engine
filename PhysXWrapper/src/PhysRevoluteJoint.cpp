#include "StdAfx.h"
#include "..\include\PhysRevoluteJoint.h"
#include "NxPhysics.h"
#include "PhysRevoluteJointDesc.h"
#include "PhysJointLimitPairDesc.h"
#include "PhysMotorDesc.h"
#include "PhysSpringDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysRevoluteJoint::PhysRevoluteJoint(NxRevoluteJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysRevoluteJoint::saveToDesc(PhysRevoluteJointDesc^ desc)
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

void PhysRevoluteJoint::loadFromDesc(PhysRevoluteJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

void PhysRevoluteJoint::setLimits(PhysJointLimitPairDesc^ desc)
{
	typedJoint->setLimits(*desc->limitDesc);
}

bool PhysRevoluteJoint::getLimits(PhysJointLimitPairDesc^ desc)
{
	return typedJoint->getLimits(*desc->limitDesc);
}

void PhysRevoluteJoint::setMotor(PhysMotorDesc^ motor)
{
	typedJoint->setMotor(*motor->motorDesc);
}

bool PhysRevoluteJoint::getMotor(PhysMotorDesc^ motor)
{
	return typedJoint->getMotor(*motor->motorDesc);
}

void PhysRevoluteJoint::setSpring(PhysSpringDesc^ spring)
{
	typedJoint->setSpring(*spring->springDesc);
}

bool PhysRevoluteJoint::getSpring(PhysSpringDesc^ spring)
{
	return typedJoint->getSpring(*spring->springDesc);
}

float PhysRevoluteJoint::getAngle()
{
	return typedJoint->getAngle();
}

float PhysRevoluteJoint::getVelocity()
{
	return typedJoint->getVelocity();
}

void PhysRevoluteJoint::setFlags(RevoluteJointFlag flags)
{
	typedJoint->setFlags((NxU32)flags);
}

RevoluteJointFlag PhysRevoluteJoint::getFlags()
{
	return (RevoluteJointFlag)typedJoint->getFlags();
}

void PhysRevoluteJoint::setProjectionMode(JointProjectionMode projectionMode)
{
	typedJoint->setProjectionMode((NxJointProjectionMode)projectionMode);
}

JointProjectionMode PhysRevoluteJoint::getProjectionMode()
{
	return (JointProjectionMode)typedJoint->getProjectionMode();
}

NxJointDesc& PhysRevoluteJoint::getDesc()
{
	NxRevoluteJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}