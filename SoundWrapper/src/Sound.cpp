#include "StdAfx.h"
#include "Sound.h"

namespace SoundWrapper
{

Sound::Sound(void)
:repeat(false)
{
}

Sound::~Sound(void)
{
}

}

//CWrapper

using namespace SoundWrapper;

extern "C" _AnomalousExport void Sound_setRepeat(Sound* sound, bool value)
{
	sound->setRepeat(value);
}

extern "C" _AnomalousExport bool Sound_getRepeat(Sound* sound)
{
	return sound->getRepeat();
}

