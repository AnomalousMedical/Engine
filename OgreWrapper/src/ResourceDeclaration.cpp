#include "StdAfx.h"
#include "..\include\ResourceDeclaration.h"

namespace Rendering
{

ResourceDeclaration::ResourceDeclaration(System::String^ resourceName, System::String^ resourceType)
:resourceName(resourceName), resourceType((ResourceTypes)System::Enum::Parse(Rendering::ResourceTypes::typeid, resourceType))
{
	
}

System::String^ ResourceDeclaration::ResourceName::get() 
{
	return resourceName;
}

ResourceTypes ResourceDeclaration::ResourceType::get() 
{
	return resourceType;
}

}