#include "StdAfx.h"
#include "..\include\PhysPointOnLineJointDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysPointOnLineJointDesc::PhysPointOnLineJointDesc()
:joint(new NxPointOnLineJointDesc()), PhysJointDesc(joint.Get())
{

}

}