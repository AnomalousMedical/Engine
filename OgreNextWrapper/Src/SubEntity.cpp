#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport const char* SubEntity_getMaterialName(Ogre::v1::SubEntity* subEntity)
{
	return subEntity->getMaterialName().c_str();
}

extern "C" _AnomalousExport void SubEntity_setMaterialName(Ogre::v1::SubEntity* subEntity, const char* name)
{
	subEntity->setMaterialName(name);
}

extern "C" _AnomalousExport void SubEntity_setVisible(Ogre::v1::SubEntity* subEntity, bool visible)
{
	subEntity->setVisible(visible);
}

extern "C" _AnomalousExport bool SubEntity_isVisible(Ogre::v1::SubEntity* subEntity)
{
	return subEntity->isVisible();
}

extern "C" _AnomalousExport Ogre::Material* SubEntity_getMaterial(Ogre::v1::SubEntity* subEntity, ProcessWrapperObjectDelegate processWrapperObject)
{
	const Ogre::MaterialPtr& materialPtr = subEntity->getMaterial();
	processWrapperObject(materialPtr.getPointer(), &materialPtr);
	return materialPtr.getPointer();
}

extern "C" _AnomalousExport void SubEntity_setCustomParameter(Ogre::v1::SubEntity* subEntity, size_t index, Quaternion value)
{
	subEntity->setCustomParameter(index, value.toOgreVec4());
}

extern "C" _AnomalousExport Quaternion SubEntity_getCustomParameter(Ogre::v1::SubEntity* subEntity, size_t index)
{
	try
	{
		return subEntity->getCustomParameter(index);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
    return Quaternion();
}

#pragma warning(pop)