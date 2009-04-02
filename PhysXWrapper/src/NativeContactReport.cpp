#include "StdAfx.h"
#include "NativeContactReport.h"
#include "PhysActor.h"
#include "NxActor.h"
#include "ContactIterator.h"

namespace Physics
{

NativeContactReport::NativeContactReport(void):
contactIterator( gcnew ContactIterator() )
{
}

NativeContactReport::~NativeContactReport()
{

}

void NativeContactReport::onContactNotify( NxContactPair& pair, NxU32 events )
{
	PhysActorGCRoot* actor0;
	PhysActorGCRoot* actor1;
	NxContactStreamIterator csi( pair.stream );
	contactIterator->setContactStreamIterator( &csi );
	if( pair.actors[0] && pair.actors[1] )
	{
		actor0 = (PhysActorGCRoot*)(pair.actors[0]->userData);
		actor1 = (PhysActorGCRoot*)(pair.actors[1]->userData);
		if( actor0 && actor1 )
		{
			(*actor0)->alertContact( (*actor1), (*actor0), contactIterator, (ContactPairFlag)events );
			(*actor1)->alertContact( (*actor0), (*actor1), contactIterator, (ContactPairFlag)events );
		}
	}
}

}