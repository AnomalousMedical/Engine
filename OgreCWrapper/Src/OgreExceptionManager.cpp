#include "Stdafx.h"
#include "../Include/OgreExceptionManager.h"
#include "OgreException.h"
	
NativeAction_String_NoHandle exceptionCallback;
	
extern "C" _AnomalousExport void OgreExceptionManager_setCallback(NativeAction_String_NoHandle exCb)
{
	exceptionCallback = exCb;
}

void sendExceptionToManagedCode(Ogre::Exception& ex)
{
	exceptionCallback(ex.getFullDescription().c_str());
}