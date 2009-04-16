#pragma once

#include "Enums.h"

namespace Rendering
{

public value class ResourceDeclaration
{
private:
	System::String^ resourceName;
	ResourceTypes resourceType;

public:
	ResourceDeclaration(System::String^ resourceName, System::String^ resourceType);

	property System::String^ ResourceName 
	{
		System::String^ get();
	}

	property ResourceTypes ResourceType 
	{
		ResourceTypes get();
	}
};

}