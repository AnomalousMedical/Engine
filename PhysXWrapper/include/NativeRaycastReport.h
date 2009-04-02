#pragma once

#include "NxUserRaycastReport.h"
#include <vcclr.h>

namespace PhysXWrapper
{

interface class RaycastReport;
ref class RaycastHit;

/// <summary>
/// This class is the native side of the raycast report.  It is used globally to 
/// generate all reports from the sdk
/// </summary>
class NativeRaycastReport : public NxUserRaycastReport
{
private:
	gcroot<RaycastHit^> raycastHit;
	gcroot<RaycastReport^> currentReport;
public:

	/// <summary>
	/// Constructor
	/// </summary>
	NativeRaycastReport(void);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~NativeRaycastReport(void);

	/// <summary>
	/// Sets the raycast report we want to report back to
	/// </summary>
	void setCurrentReport( RaycastReport^ report );

	/// <summary>
	/// Called by the api when something is hit.
	/// </summary>
	virtual bool onHit( const NxRaycastHit& hit );
};

}