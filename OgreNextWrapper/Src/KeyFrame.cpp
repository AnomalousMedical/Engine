#include "Stdafx.h"

extern "C" _AnomalousExport float KeyFrame_getTime(Ogre::v1::KeyFrame* keyFrame)
{
	return keyFrame->getTime();
}