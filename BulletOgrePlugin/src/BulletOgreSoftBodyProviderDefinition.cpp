#include "StdAfx.h"
#include "BulletOgreSoftBodyProviderDefinition.h"
#include "BulletOgreSoftBodyProvider.h"

namespace BulletOgrePlugin
{

BulletOgreSoftBodyProviderDefinition::BulletOgreSoftBodyProviderDefinition(String^ name)
:SoftBodyProviderDefinition(name),
renderQueue(50)
{

}

BulletOgreSoftBodyProviderDefinition::~BulletOgreSoftBodyProviderDefinition(void)
{

}

SoftBodyProvider^ BulletOgreSoftBodyProviderDefinition::createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene)
{
	OgrePlugin::OgreSceneManager^ ogreScene = subScene->getSimElementManager<OgrePlugin::OgreSceneManager^>();
	return gcnew BulletOgreSoftBodyProvider(this, ogreScene);
}

BulletOgreSoftBodyProviderDefinition::BulletOgreSoftBodyProviderDefinition(LoadInfo^ info)
:SoftBodyProviderDefinition(info)
{
	meshName = info->GetString("MeshName");
	renderQueue = info->GetByte("RenderQueue");
}

void BulletOgreSoftBodyProviderDefinition::getInfo(SaveInfo^ info)
{
	SoftBodyProviderDefinition::getInfo(info);
	info->AddValue("MeshName", meshName);
	info->AddValue("RenderQueue", renderQueue);
}

}