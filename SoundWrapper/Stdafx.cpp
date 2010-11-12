// stdafx.cpp : source file that includes just the standard includes
// SoundWrapper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

namespace SoundWrapper
{

NativeLog log("SoundWrapper");

void checkOpenAL()
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
		log << "OpenAL error code gotten " << error << SoundWrapper::error;
	}
}

}

//Log add listener function
using namespace SoundWrapper;

extern "C" _AnomalousExport void NativeLog_addLogListener(NativeLogListener* logListener)
{
	log.addListener(logListener);
}