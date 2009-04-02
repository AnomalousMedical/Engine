#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxFixedJointDesc.h"

namespace PhysXWrapper
{

/// <summary>
/// Wrapper for NxFixedJointDesc.
/// </summary>
public ref class PhysFixedJointDesc : public PhysJointDesc
{
internal:
	AutoPtr<NxFixedJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysFixedJointDesc();
};

}