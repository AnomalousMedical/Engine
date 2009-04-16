//Source
#include "stdafx.h"
#include "SubMeshCollection.h"
#include "SubMesh.h"

namespace OgreWrapper{

SubMesh^ SubMeshCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew SubMesh(static_cast<Ogre::SubMesh*>(nativeObject));
}

SubMesh^ SubMeshCollection::getObject(Ogre::SubMesh* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void SubMeshCollection::destroyObject(Ogre::SubMesh* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}