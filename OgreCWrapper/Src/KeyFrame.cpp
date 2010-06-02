#include "Stdafx.h"

extern "C" __declspec(dllexport) float KeyFrame_getTime(Ogre::KeyFrame* keyFrame)
{
	return keyFrame->getTime();
}