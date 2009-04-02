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
	int group0;
	int group1;

public:
	property ContactPairFlag Flags 
	{
		ContactPairFlag get()
		{
			return flags;
		}
	}

	property int Group0 
	{
		int get()
		{
			return group0;
		}
	}

	property int Group1 
	{
		int get()
		{
			return group1;
		}
	}

};

}