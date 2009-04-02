#include "StdAfx.h"
#include "..\include\PhysPointInPlaneJointDesc.h"
#include "NxPhysics.h"

namespace Physics
{

PhysPointInPlaneJointDesc::PhysPointInPlaneJointDesc()
:joint(new NxPointInPlaneJointDesc()), PhysJointDesc(joint.Get())
{

}

}