#include "StdAfx.h"
#include "..\include\PhysPointInPlaneJoint.h"
#include "NxPhysics.h"
#include "PhysPointInPlaneJointDesc.h"
#include "PhysActor.h"

namespace PhysXWrapper
{

PhysPointInPlaneJoint::PhysPointInPlaneJoint(NxPointInPlaneJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysPointInPlaneJoint::saveToDesc(PhysPointInPlaneJointDesc^ desc)
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

void PhysPointInPlaneJoint::loadFromDesc(PhysPointInPlaneJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

NxJointDesc& PhysPointInPlaneJoint::getDesc()
{
	NxPointInPlaneJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}