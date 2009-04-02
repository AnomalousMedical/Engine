#pragma once

#include "NxUserContactReport.h"
#include <vcclr.h>

namespace PhysXWrapper
{

ref class ContactIterator;

/// <summary>
/// This class interacts with the PhysX SDK and forwards messages to the wrapped
/// classes.
/// </summary>
class NativeContactReport : public NxUserContactReport
{
private:
	gcroot<ContactIterator^> contactIterator;

public:
	NativeContactReport(void);

	~NativeContactReport();

	virtual void onContactNotify( NxContactPair& pair, NxU32 events );
};

}