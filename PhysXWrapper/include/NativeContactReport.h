#pragma once

#include "NxUserContactReport.h"
#include <vcclr.h>

namespace PhysXWrapper
{

ref class PhysContactPair;
interface class PhysContactReport;

/// <summary>
/// This class interacts with the PhysX SDK and forwards messages to the wrapped
/// classes.
/// </summary>
class NativeContactReport : public NxUserContactReport
{
private:
	gcroot<PhysContactPair^> contactPair;
	gcroot<PhysContactReport^> contactReport;

public:
	NativeContactReport(void);

	~NativeContactReport();

	virtual void onContactNotify( NxContactPair& pair, NxU32 events );

	void setContactReport(PhysContactReport^ report)
	{
		contactReport = report;
	}
};

}