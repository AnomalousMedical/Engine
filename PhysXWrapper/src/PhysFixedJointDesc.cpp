#include "StdAfx.h"
#include "..\include\PhysFixedJointDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysFixedJointDesc::PhysFixedJointDesc()
:joint(new NxFixedJointDesc()), PhysJointDesc(joint.Get())
{

}

}