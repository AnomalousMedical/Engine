#include "StdAfx.h"
#include "..\include\BulletSceneDefinition.h"
#include "BulletInterface.h"
#include "BulletScene.h"

namespace BulletPlugin
{

BulletSceneDefinition::BulletSceneDefinition(String^ name)
:name(name),
editInterface(nullptr),
worldAabbMin(Vector3(-10000, -10000, -10000)),
worldAabbMax(Vector3(10000, 10000, 10000)),
maxProxies(1024),
gravity(Vector3(0, -9.8f, 0))
{

}

BulletSceneDefinition::~BulletSceneDefinition()
{

}

EditInterface^ BulletSceneDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, name + " - Bullet Scene", nullptr);
	}
	return editInterface;
}

SimElementManager^ BulletSceneDefinition::createSimElementManager()
{
	return BulletInterface::Instance->createScene(this);
}

Type^ BulletSceneDefinition::getSimElementManagerType()
{
	return BulletSceneDefinition::typeid;
}

//Saving
BulletSceneDefinition::BulletSceneDefinition(LoadInfo^ info)
{
	name = info->GetString("Name");
	worldAabbMin = info->GetVector3("WorldAABBMin");
	worldAabbMax = info->GetVector3("WorldAABBMax");
	maxProxies = info->GetInt32("MaxProxies");
	gravity = info->GetVector3("Gravity");
}

void BulletSceneDefinition::getInfo(SaveInfo^ info)
{
	info->AddValue("Name", name);
	info->AddValue("WorldAABBMin", worldAabbMin);
	info->AddValue("WorldAABBMax", worldAabbMax);
	info->AddValue("MaxProxies", maxProxies);
	info->AddValue("Gravity", gravity);
}
//EndSaving
}