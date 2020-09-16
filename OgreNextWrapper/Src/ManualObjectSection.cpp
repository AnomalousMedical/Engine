#include "Stdafx.h"

extern "C" _AnomalousExport const char* ManualObjectSection_getMaterialName(Ogre::v1::ManualObject::ManualObjectSection* ogreSection)
{
	return ogreSection->getMaterialName().c_str();
}

extern "C" _AnomalousExport void ManualObjectSection_setMaterialName(Ogre::v1::ManualObject::ManualObjectSection* ogreSection, const char* name)
{
	ogreSection->setMaterialName(name);
}

extern "C" _AnomalousExport void ManualObjectSection_set32BitIndices(Ogre::v1::ManualObject::ManualObjectSection* ogreSection, bool n32)
{
	ogreSection->set32BitIndices(n32);
}

extern "C" _AnomalousExport bool ManualObjectSection_get32BitIndices(Ogre::v1::ManualObject::ManualObjectSection* ogreSection)
{
	return ogreSection->get32BitIndices();
}

extern "C" _AnomalousExport void ManualObject_setCustomParameter(Ogre::v1::ManualObject::ManualObjectSection* ogreSection, size_t index, Quaternion value)
{
	ogreSection->setCustomParameter(index, value.toOgreVec4());
}

extern "C" _AnomalousExport Quaternion ManualObject_getCustomParameter(Ogre::v1::ManualObject::ManualObjectSection* ogreSection, size_t index)
{
	return ogreSection->getCustomParameter(index);
}