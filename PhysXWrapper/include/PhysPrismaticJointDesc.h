#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxPrismaticJointDesc.h"

namespace PhysXWrapper
{

/// <summary>
/// Wrapper for NxPrismaticJointDesc.
/// Describes PrismaticJoint.
/// </summary>
public ref class PhysPrismaticJointDesc : public PhysJointDesc
{
internal:
	AutoPtr<NxPrismaticJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysPrismaticJointDesc();
};

}