#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxCylindricalJointDesc.h"

namespace Engine
{

namespace Physics
{

/// <summary>
/// Wrapper for NxCylindricalJointDesc.
/// </summary>
public ref class PhysCylindricalJointDesc : public PhysJointDesc
{
internal:
	AutoPtr<NxCylindricalJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="name">Debug name for the joint.</param>
	PhysCylindricalJointDesc(Engine::Identifier^ name);
};

}

}