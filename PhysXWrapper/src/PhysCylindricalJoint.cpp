#include "StdAfx.h"
#include "..\include\PhysCylindricalJoint.h"
#include "NxPhysics.h"
#include "PhysCylindricalJointDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysCylindricalJoint::PhysCylindricalJoint(NxCylindricalJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysCylindricalJoint::saveToDesc(PhysCylindricalJointDesc^ desc)
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

void PhysCylindricalJoint::loadFromDesc(PhysCylindricalJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

NxJointDesc& PhysCylindricalJoint::getDesc()
{
	NxCylindricalJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}