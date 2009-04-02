#include "StdAfx.h"
#include "..\include\PhysPrismaticJoint.h"
#include "NxPhysics.h"
#include "PhysPrismaticJointDesc.h"
#include "PhysActor.h"

namespace PhysXWrapper
{

PhysPrismaticJoint::PhysPrismaticJoint(NxPrismaticJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysPrismaticJoint::saveToDesc(PhysPrismaticJointDesc^ desc)
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

void PhysPrismaticJoint::loadFromDesc(PhysPrismaticJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

NxJointDesc& PhysPrismaticJoint::getDesc()
{
	NxPrismaticJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}