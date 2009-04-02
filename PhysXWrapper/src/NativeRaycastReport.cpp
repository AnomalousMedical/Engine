#include "StdAfx.h"
#include "NativeRaycastReport.h"
#include "RaycastHit.h"
#include "RaycastReport.h"
#include "NxShape.h"

namespace Engine
{

namespace Physics
{

NativeRaycastReport::NativeRaycastReport(void)
{
	raycastHit = gcnew RaycastHit();
}

NativeRaycastReport::~NativeRaycastReport(void)
{

}

bool NativeRaycastReport::onHit( const NxRaycastHit& hit )
{
	if( hit.shape && 
		!hit.shape->getFlag( NX_TRIGGER_ON_ENTER ) &&
		!hit.shape->getFlag( NX_TRIGGER_ON_LEAVE ) &&
		!hit.shape->getFlag( NX_TRIGGER_ON_STAY ) )
	{
		raycastHit->setCurrentHit( &hit );
		return currentReport->onHit( raycastHit );
	}
	else
	{
		return true;
	}
}

void NativeRaycastReport::setCurrentReport( RaycastReport^ report )
{
	currentReport = report;
}

}

}