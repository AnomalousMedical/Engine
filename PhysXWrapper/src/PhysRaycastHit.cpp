#include "StdAfx.h"
#include "..\include\PhysRaycastHit.h"

namespace Physics
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