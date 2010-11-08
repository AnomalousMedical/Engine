#include "StdAfx.h"
#include "Source.h"

Source::Source(ALuint sourceID)
:sourceID(sourceID)
{
	//Set default source info.
    alSource3f(sourceID, AL_POSITION,        0.0, 0.0, 0.0);
    alSource3f(sourceID, AL_VELOCITY,        0.0, 0.0, 0.0);
    alSource3f(sourceID, AL_DIRECTION,       0.0, 0.0, 0.0);
    alSourcef (sourceID, AL_ROLLOFF_FACTOR,  0.0          );
    alSourcei (sourceID, AL_SOURCE_RELATIVE, AL_TRUE      );
}

Source::~Source(void)
{
	alSourceStop(sourceID);
    empty();
    alDeleteSources(1, &sourceID);
    check();
}

bool Source::playSound(Sound* sound)
{
	if(playing())
	{
        return true;
	}
    
	sound->enqueueSource(this);
    alSourcePlay(sourceID);
    
    return true;
}

bool Source::playing()
{
    ALenum state;
    
    alGetSourcei(sourceID, AL_SOURCE_STATE, &state);
    
    return (state == AL_PLAYING);
}

void Source::check()
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
        
	}
}

void Source::empty()
{
    int queued;
    
    alGetSourcei(sourceID, AL_BUFFERS_QUEUED, &queued);
	if(queued < 0)
	{
		queued = 0;
	}
    
    while(queued--)
    {
        ALuint buffer;
        alSourceUnqueueBuffers(sourceID, 1, &buffer);
        check();
    }
}