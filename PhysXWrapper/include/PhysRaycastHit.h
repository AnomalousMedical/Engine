#pragma once

#include "RaycastHit.h"
#include "AutoPtr.h"

namespace Physics
{

/// <summary>
/// This class is a RaycastHit that can be constructed. It will create the
/// NxRaycastHit that is wrapped.
/// </summary>
public ref class PhysRaycastHit : public RaycastHit
{
private:
	AutoPtr<NxRaycastHit> nxRaycastHit;

internal:
	NxRaycastHit* getNxRaycastHit();

public:
	PhysRaycastHit(void);
};

}