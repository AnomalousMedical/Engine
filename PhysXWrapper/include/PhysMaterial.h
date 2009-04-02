#pragma once
#include "Enums.h"
#include "vcclr.h"
#include "AutoPtr.h"

class NxMaterialDesc;

namespace Physics
{

ref class PhysMaterial;

typedef gcroot<PhysMaterial^> PhysMaterialGcRoot;
ref class PhysMaterialDesc;

public ref class PhysMaterial
{
private:
	System::String^ name;
	AutoPtr<PhysMaterialGcRoot> gcr;

internal:
	NxMaterial* material;

	PhysMaterial(NxMaterial* material, System::String^ name);

public:	
	virtual ~PhysMaterial(void);

	unsigned short getMaterialIndex();

	void loadFromDesc(PhysMaterialDesc^ desc);

	PhysMaterialDesc^ saveToDesc();

	void setDynamicFriction(float coef);

	float getDynamicFriction();

	void setStaticFriction(float coef);

	float getStaticFriction();

	void setRestitution(float coef);

	float getRestitution();

	void setDynamicFrictionV(float coef);

	float getDynamicFrictionV();

	void setStaticFrictionV(float coef);

	float getStaticFrictionV();

	void setDirectionOfAnisotropy(EngineMath::Vector3 dir);

	EngineMath::Vector3 getDirectionOfAnisotropy();

	void setFlags(PhysMaterialFlag flags);

	PhysMaterialFlag getFlags();

	void setFrictionCombineMode(PhysCombineMode mode);

	PhysCombineMode getFrictionCombineMode();

	void setRestitutionCombineMode(PhysCombineMode mode);

	PhysCombineMode getRestitutionCombineMode();

	System::String^ getName();
};

}