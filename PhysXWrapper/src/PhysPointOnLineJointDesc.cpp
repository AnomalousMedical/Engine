#include "StdAfx.h"
#include "..\include\PhysPointOnLineJointDesc.h"
#include "NxPhysics.h"

namespace Physics
{

PhysPointOnLineJointDesc::PhysPointOnLineJointDesc()
:joint(new NxPointOnLineJointDesc()), PhysJointDesc(joint.Get())
{

}

}