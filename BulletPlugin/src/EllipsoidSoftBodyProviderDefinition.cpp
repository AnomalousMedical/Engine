#include "StdAfx.h"
#include "..\include\EllipsoidSoftBodyProviderDefinition.h"
#include "EllipsoidSoftBodyProvider.h"

namespace BulletPlugin
{

EllipsoidSoftBodyProviderDefinition::EllipsoidSoftBodyProviderDefinition(String^ name)
:SoftBodyProviderDefinition(name)
{

}

EllipsoidSoftBodyProviderDefinition::~EllipsoidSoftBodyProviderDefinition(void)
{

}

SoftBodyProvider^ EllipsoidSoftBodyProviderDefinition::createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene)
{
	return gcnew EllipsoidSoftBodyProvider(this);
}

EllipsoidSoftBodyProviderDefinition::EllipsoidSoftBodyProviderDefinition(LoadInfo^ info)
:SoftBodyProviderDefinition(info)
{

}

void EllipsoidSoftBodyProviderDefinition::getInfo(SaveInfo^ info)
{
	SoftBodyProviderDefinition::getInfo(info);
}

}