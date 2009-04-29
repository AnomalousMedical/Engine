#pragma once
#include "Enums.h"
#include "vcclr.h"
#include "AutoPtr.h"

class NxMaterialDesc;

namespace PhysXWrapper
{

ref class PhysMaterial;

typedef gcroot<PhysMaterial^> PhysMaterialGcRoot;
ref class PhysMaterialDesc;

public ref class PhysMaterial
{
private:
	AutoPtr<PhysMaterialGcRoot> gcr;

internal:
	NxMaterial* material;

	PhysMaterial(NxMaterial* material);

public:	
	virtual ~PhysMaterial(void);

	unsigned short getMaterialIndex();

	void loadFromDesc(PhysMaterialDesc^ desc);

	void saveToDesc(PhysMaterialDesc^ desc);

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

	void setDirectionOfAnisotropy(Engine::Vector3 dir);

	Engine::Vector3 getDirectionOfAnisotropy();

	void setFlags(PhysMaterialFlag flags);

	PhysMaterialFlag getFlags();

	void setFrictionCombineMode(PhysCombineMode mode);

	PhysCombineMode getFrictionCombineMode();

	void setRestitutionCombineMode(PhysCombineMode mode);

	PhysCombineMode getRestitutionCombineMode();
};

}