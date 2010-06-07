#include "Stdafx.h"

extern "C" _AnomalousExport float KeyFrame_getTime(Ogre::KeyFrame* keyFrame)
{
	return keyFrame->getTime();
}