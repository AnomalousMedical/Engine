#pragma once

class NxMaterialDesc;

#include "AutoPtr.h"
#include "Enums.h"

namespace PhysXWrapper
{

public ref class PhysMaterialDesc
{
private:
	System::String^ name;

internal:
	AutoPtr<NxMaterialDesc> desc;

public:
	PhysMaterialDesc(System::String^ name);
	
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

	property System::String^ Name
	{
		System::String^ get();
	}
};

}