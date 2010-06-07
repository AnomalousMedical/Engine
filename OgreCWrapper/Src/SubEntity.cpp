#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport const char* SubEntity_getMaterialName(Ogre::SubEntity* subEntity)
{
	return subEntity->getMaterialName().c_str();
}

extern "C" _AnomalousExport void SubEntity_setMaterialName(Ogre::SubEntity* subEntity, const char* name)
{
	subEntity->setMaterialName(name);
}

extern "C" _AnomalousExport void SubEntity_setVisible(Ogre::SubEntity* subEntity, bool visible)
{
	subEntity->setVisible(visible);
}

extern "C" _AnomalousExport bool SubEntity_isVisible(Ogre::SubEntity* subEntity)
{
	return subEntity->isVisible();
}

extern "C" _AnomalousExport Ogre::Material* SubEntity_getMaterial(Ogre::SubEntity* subEntity, ProcessWrapperObjectDelegate processWrapperObject)
{
	const Ogre::MaterialPtr& materialPtr = subEntity->getMaterial();
	processWrapperObject(materialPtr.getPointer(), &materialPtr);
	return materialPtr.getPointer();
}

extern "C" _AnomalousExport void SubEntity_setCustomParameter(Ogre::SubEntity* subEntity, size_t index, Quaternion value)
{
	subEntity->setCustomParameter(index, value.toOgreVec4());
}

extern "C" _AnomalousExport Quaternion SubEntity_getCustomParameter(Ogre::SubEntity* subEntity, size_t index)
{
	return subEntity->getCustomParameter(index);
}

#pragma warning(pop)