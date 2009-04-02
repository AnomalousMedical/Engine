#include "StdAfx.h"
#include "..\include\PhysSphericalJoint.h"
#include "NxPhysics.h"
#include "PhysSphericalJointDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysSphericalJoint::PhysSphericalJoint(NxSphericalJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysSphericalJoint::saveToDesc(PhysSphericalJointDesc^ desc)
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

void PhysSphericalJoint::loadFromDesc(PhysSphericalJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

void PhysSphericalJoint::setFlags(SphericalJointFlag flags)
{
	typedJoint->setFlags((NxU32)flags);
}

SphericalJointFlag PhysSphericalJoint::getFlags()
{
	return (SphericalJointFlag)typedJoint->getFlags();
}

void PhysSphericalJoint::setProjectionMode(JointProjectionMode projectionMode)
{
	typedJoint->setProjectionMode((NxJointProjectionMode)projectionMode);
}

JointProjectionMode PhysSphericalJoint::getProjectionMode()
{
	return (JointProjectionMode)typedJoint->getProjectionMode();
}

NxJointDesc& PhysSphericalJoint::getDesc()
{
	NxSphericalJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}