#include "StdAfx.h"
#include "..\include\PhysCylindricalJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysCylindricalJointDesc::PhysCylindricalJointDesc(Engine::Identifier^ name)
:joint(new NxCylindricalJointDesc()), PhysJointDesc(joint.Get(), name)
{

}

}

}