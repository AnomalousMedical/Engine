#include "StdAfx.h"
#include "Source.h"

Source::Source(ALuint sourceID)
:sourceID(sourceID),
paused(false)
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
    checkOpenAL();
}

bool Source::playSound(Sound* sound)
{
	if(playing())
	{
        return true;
	}
    
	if(sound->enqueueSource(this))
	{
		paused = false;
		alSourcePlay(sourceID);
		return true;
	}

	return false;
}

bool Source::playing()
{
    ALenum state;
    alGetSourcei(sourceID, AL_SOURCE_STATE, &state);
    return (state == AL_PLAYING);
}

void Source::stop()
{
	paused = false;
	alSourceStop(sourceID);
	empty();
}

void Source::pause()
{
	if(!paused && playing())
	{
		paused = true;
		alSourcePause(sourceID);
	}
}

bool Source::resume()
{
	if(paused)
	{
		paused = false;
		alSourcePlay(sourceID);
		return true;
	}
	else
	{
		return false;
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
        checkOpenAL();
    }
}