#include "StdAfx.h"
#include "..\include\PhysRaycastHit.h"

namespace PhysXWrapper
{

PhysRaycastHit::PhysRaycastHit(void)
:nxRaycastHit(new NxRaycastHit())
{
	this->setCurrentHit(nxRaycastHit.Get());
}

NxRaycastHit* PhysRaycastHit::getNxRaycastHit()
{
	return nxRaycastHit.Get();
}

}