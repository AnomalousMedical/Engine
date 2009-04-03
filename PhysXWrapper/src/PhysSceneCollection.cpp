//Source
#include "stdafx.h"
#include "PhysSceneCollection.h"
#include "PhysScene.h"

namespace PhysXWrapper
{

PhysScene^ PhysSceneCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew PhysScene(static_cast<NxScene*>(nativeObject));
}

PhysScene^ PhysSceneCollection::getObject(NxScene* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void PhysSceneCollection::destroyObject(NxScene* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}