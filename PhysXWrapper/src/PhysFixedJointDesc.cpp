#include "StdAfx.h"
#include "..\include\PhysFixedJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysFixedJointDesc::PhysFixedJointDesc()
:joint(new NxFixedJointDesc()), PhysJointDesc(joint.Get())
{

}

}

}