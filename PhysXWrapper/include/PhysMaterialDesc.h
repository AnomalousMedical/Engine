#pragma once

class NxMaterialDesc;

#include "AutoPtr.h"
#include "Enums.h"

namespace PhysXWrapper
{

public ref class PhysMaterialDesc
{
internal:
	AutoPtr<NxMaterialDesc> desc;

public:
	PhysMaterialDesc();
	
	virtual ~PhysMaterialDesc(void);

	property float Restitution 
	{
		float get();
		void set(float value);
	}

	property float StaticFriction 
	{
		float get();
		void set(float value);
	}

	property float DynamicFriction 
	{
		float get();
		void set(float value);
	}
};

}