// stdafx.cpp : source file that includes just the standard includes
// SoundWrapper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

namespace SoundWrapper
{

NativeLog log("SoundWrapper");

void checkOpenAL(const char* hint)
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
		switch(error)
		{
			case AL_INVALID_NAME:
				log << "OpenAL invalid name gotten. Code: " << error;
				break;
			case AL_INVALID_ENUM:
				log << "OpenAL invalid enum gotten. Code: " << error;
				break;
			case AL_INVALID_VALUE:
				log << "OpenAL invalid value gotten. Code: " << error;
				break;
			case AL_INVALID_OPERATION:
				log << "OpenAL invalid operation gotten. Code: " << error;
				break;
			case AL_OUT_OF_MEMORY:
				log << "OpenAL out of memory. Code: " << error;
				break;
			default:
				log << "Unknown OpenAL error code " << error;
				break;
		}
		if(hint != NULL)
		{
			log << " " << hint;
		}
		log << SoundWrapper::error;
	}
}

}

//Log add listener function
using namespace SoundWrapper;

extern "C" _AnomalousExport void NativeLog_addLogListener(NativeLogListener* logListener)
{
	log.addListener(logListener);
}