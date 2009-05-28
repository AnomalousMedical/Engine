#include "StdAfx.h"
#include "..\include\PhysFixedJoint.h"
#include "NxPhysics.h"
#include "PhysFixedJointDesc.h"
#include "PhysActor.h"

namespace PhysXWrapper
{

PhysFixedJoint::PhysFixedJoint(NxFixedJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysFixedJoint::saveToDesc(PhysFixedJointDesc^ desc)
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

void PhysFixedJoint::loadFromDesc(PhysFixedJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

}