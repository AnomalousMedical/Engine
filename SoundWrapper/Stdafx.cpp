// stdafx.cpp : source file that includes just the standard includes
// SoundWrapper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

namespace SoundWrapper
{

NativeLog logger("SoundWrapper");

void checkOpenAL(const char* hint)
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
		switch(error)
		{
			case AL_INVALID_NAME:
				logger << "OpenAL invalid name gotten. Code: " << error;
				break;
			case AL_INVALID_ENUM:
				logger << "OpenAL invalid enum gotten. Code: " << error;
				break;
			case AL_INVALID_VALUE:
				logger << "OpenAL invalid value gotten. Code: " << error;
				break;
			case AL_INVALID_OPERATION:
				logger << "OpenAL invalid operation gotten. Code: " << error;
				break;
			case AL_OUT_OF_MEMORY:
				logger << "OpenAL out of memory. Code: " << error;
				break;
			default:
				logger << "Unknown OpenAL error code " << error;
				break;
		}
		if(hint != NULL)
		{
			logger << " " << hint;
		}
		logger << SoundWrapper::error;
	}
}

}

//Log add listener function
using namespace SoundWrapper;

extern "C" _AnomalousExport void NativeLog_addLogListener(NativeLogListener* logListener)
{
	logger.addListener(logListener);
}