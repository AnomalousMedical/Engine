#include "StdAfx.h"
#include "NativeContactReport.h"
#include "PhysActor.h"
#include "NxActor.h"
#include "PhysContactPair.h"
#include "PhysContactReport.h"

namespace PhysXWrapper
{

NativeContactReport::NativeContactReport(void):
contactPair( gcnew PhysContactPair() )
{
}

NativeContactReport::~NativeContactReport()
{

}

void NativeContactReport::onContactNotify( NxContactPair& pair, NxU32 events )
{
	contactPair->setPair(&pair);
	contactReport->onContactNotify(contactPair, static_cast<ContactPairFlag>(events));
	contactPair->clearPair();
}

}