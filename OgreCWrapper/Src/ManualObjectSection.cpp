#include "Stdafx.h"

extern "C" __declspec(dllexport) const char* ManualObjectSection_getMaterialName(Ogre::ManualObject::ManualObjectSection* ogreSection)
{
	return ogreSection->getMaterialName().c_str();
}

extern "C" __declspec(dllexport) void ManualObjectSection_setMaterialName(Ogre::ManualObject::ManualObjectSection* ogreSection, const char* name)
{
	ogreSection->setMaterialName(name);
}

extern "C" __declspec(dllexport) void ManualObjectSection_set32BitIndices(Ogre::ManualObject::ManualObjectSection* ogreSection, bool n32)
{
	ogreSection->set32BitIndices(n32);
}

extern "C" __declspec(dllexport) bool ManualObjectSection_get32BitIndices(Ogre::ManualObject::ManualObjectSection* ogreSection)
{
	return ogreSection->get32BitIndices();
}