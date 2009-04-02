#include "StdAfx.h"
#include "..\include\PhysPointInPlaneJointDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysPointInPlaneJointDesc::PhysPointInPlaneJointDesc()
:joint(new NxPointInPlaneJointDesc()), PhysJointDesc(joint.Get())
{

}

}