//Source
#include "stdafx.h"
#include "PhysSoftBodyCollection.h"
#include "PhysSoftBody.h"

namespace Engine{

namespace Physics{

PhysSoftBody^ PhysSoftBodyCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew PhysSoftBody(static_cast<NxSoftBody*>(nativeObject));
}

PhysSoftBody^ PhysSoftBodyCollection::getObject(NxSoftBody* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void PhysSoftBodyCollection::destroyObject(NxSoftBody* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}