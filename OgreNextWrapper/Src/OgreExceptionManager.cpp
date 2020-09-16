#include "Stdafx.h"
#include "../Include/OgreExceptionManager.h"
#include "OgreException.h"
	
typedef void(*ExceptionFoundCallback)(String fullMessage);

ExceptionFoundCallback exceptionCallback;
	
extern "C" _AnomalousExport void OgreExceptionManager_setCallback(ExceptionFoundCallback exCb)
{
	exceptionCallback = exCb;
}

void sendExceptionToManagedCode(Ogre::Exception& ex)
{
	exceptionCallback(ex.getFullDescription().c_str());
}