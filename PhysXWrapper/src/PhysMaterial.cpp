#include "StdAfx.h"
#include "..\include\PhysMaterial.h"
#include "MathUtil.h"
#include "PhysMaterialDesc.h"

namespace PhysXWrapper
{

PhysMaterial::PhysMaterial(NxMaterial* material, System::String^ name)
:material(material), gcr(new PhysMaterialGcRoot(this)), name(name)
{
	material->userData = gcr.Get();
}

PhysMaterial::~PhysMaterial(void)
{
	
}

unsigned short PhysMaterial::getMaterialIndex()
{
	return material->getMaterialIndex();
}

void PhysMaterial::loadFromDesc(PhysMaterialDesc^ desc)
{
	material->loadFromDesc((*desc->desc.Get()));
}

PhysMaterialDesc^ PhysMaterial::saveToDesc()
{
	PhysMaterialDesc^ desc = gcnew PhysMaterialDesc(name);
	material->saveToDesc((*desc->desc.Get()));
	return desc;
}

void PhysMaterial::setDynamicFriction(float coef)
{
	material->setDynamicFriction(coef);
}

float PhysMaterial::getDynamicFriction()
{
	return material->getDynamicFriction();
}

void PhysMaterial::setStaticFriction(float coef)
{
	material->setStaticFriction(coef);
}

float PhysMaterial::getStaticFriction()
{
	return material->getStaticFriction();
}

void PhysMaterial::setRestitution(float coef)
{
	material->setRestitution(coef);
}

float PhysMaterial::getRestitution()
{
	return material->getRestitution();
}

void PhysMaterial::setDynamicFrictionV(float coef)
{
	material->setDynamicFrictionV(coef);
}

float PhysMaterial::getDynamicFrictionV()
{
	return material->getDynamicFrictionV();
}

void PhysMaterial::setStaticFrictionV(float coef)
{
	material->setStaticFrictionV(coef);
}

float PhysMaterial::getStaticFrictionV()
{
	return material->getStaticFrictionV();
}

void PhysMaterial::setDirectionOfAnisotropy(Engine::Vector3 dir)
{
	material->setDirOfAnisotropy(MathUtil::copyVector3(dir));
}

Engine::Vector3 PhysMaterial::getDirectionOfAnisotropy()
{
	return MathUtil::copyVector3(material->getDirOfAnisotropy());
}

void PhysMaterial::setFlags(PhysMaterialFlag flags)
{
	material->setFlags((NxMaterialFlag)flags);
}

PhysMaterialFlag PhysMaterial::getFlags()
{
	return (PhysMaterialFlag)material->getFlags();
}

void PhysMaterial::setFrictionCombineMode(PhysCombineMode mode)
{
	material->setFrictionCombineMode((NxCombineMode)mode);
}

PhysCombineMode PhysMaterial::getFrictionCombineMode()
{
	return (PhysCombineMode)material->getFrictionCombineMode();
}

void PhysMaterial::setRestitutionCombineMode(PhysCombineMode mode)
{
	material->setRestitutionCombineMode((NxCombineMode)mode);
}

PhysCombineMode PhysMaterial::getRestitutionCombineMode()
{
	return (PhysCombineMode)material->getRestitutionCombineMode();
}

System::String^ PhysMaterial::getName()
{
	return name;
}

}