#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" __declspec(dllexport) const char* SubEntity_getMaterialName(Ogre::SubEntity* subEntity)
{
	return subEntity->getMaterialName().c_str();
}

extern "C" __declspec(dllexport) void SubEntity_setMaterialName(Ogre::SubEntity* subEntity, const char* name)
{
	subEntity->setMaterialName(name);
}

extern "C" __declspec(dllexport) void SubEntity_setVisible(Ogre::SubEntity* subEntity, bool visible)
{
	subEntity->setVisible(visible);
}

extern "C" __declspec(dllexport) bool SubEntity_isVisible(Ogre::SubEntity* subEntity)
{
	return subEntity->isVisible();
}

extern "C" __declspec(dllexport) Ogre::MaterialPtr* SubEntity_getMaterial(Ogre::SubEntity* subEntity)
{
	return new Ogre::MaterialPtr(subEntity->getMaterial());
}

extern "C" __declspec(dllexport) void SubEntity_setCustomParameter(Ogre::SubEntity* subEntity, size_t index, Quaternion value)
{
	subEntity->setCustomParameter(index, value.toOgreVec4());
}

extern "C" __declspec(dllexport) Quaternion SubEntity_getCustomParameter(Ogre::SubEntity* subEntity, size_t index)
{
	return subEntity->getCustomParameter(index);
}

#pragma warning(pop)