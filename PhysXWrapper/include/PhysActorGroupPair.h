#pragma once

#include "Nxp.h"
#include "Enums.h"

namespace PhysXWrapper
{

public value class PhysActorGroupPair
{

internal:
	PhysActorGroupPair(NxActorGroupPair* pair);

	ContactPairFlag flags;
	unsigned short group0;
	unsigned short group1;

public:
	property ContactPairFlag Flags 
	{
		ContactPairFlag get()
		{
			return flags;
		}
	}

	property unsigned short Group0 
	{
		unsigned short get()
		{
			return group0;
		}
	}

	property unsigned short Group1 
	{
		unsigned short get()
		{
			return group1;
		}
	}

};

}