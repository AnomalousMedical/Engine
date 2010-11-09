// stdafx.cpp : source file that includes just the standard includes
// SoundWrapper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

void checkOpenAL()
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
        
	}
}