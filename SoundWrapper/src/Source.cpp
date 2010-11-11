#include "StdAfx.h"
#include "Source.h"
#include "SourceManager.h"

namespace SoundWrapper
{

Source::Source(ALuint sourceID, SourceManager* sourceManager)
:sourceID(sourceID),
paused(false),
sourceManager(sourceManager),
finishedCallback(NULL)
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
		sourceManager->_addPlayingSource(this);
		currentSound = sound;
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
	finished();
}

void Source::pause()
{
	if(!paused && playing())
	{
		paused = true;
		alSourcePause(sourceID);
		sourceManager->_removePlayingSource(this);
	}
}

bool Source::resume()
{
	if(paused)
	{
		paused = false;
		alSourcePlay(sourceID);
		sourceManager->_addPlayingSource(this);
		return true;
	}
	else
	{
		return false;
	}
}

void Source::rewind()
{
	alSourceRewind(sourceID);
}

bool Source::getLooping()
{
	ALint ret;
	alGetSourcei(sourceID, AL_LOOPING, &ret);
	return ret == AL_TRUE;
}

void Source::_update()
{
	currentSound->update();
	if(!playing())
	{
		finished();
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

void Source::finished()
{
	if(finishedCallback != NULL)
	{
		finishedCallback(this);
	}
	currentSound = NULL;
	sourceManager->_removePlayingSource(this);
	sourceManager->_addSourceToPool(this);
}

}

//CWrapper

using namespace SoundWrapper;

extern "C" _AnomalousExport bool Source_playSound(Source* source, Sound* sound)
{
	return source->playSound(sound);
}

extern "C" _AnomalousExport bool Source_playing(Source* source)
{
	return source->playing();
}

extern "C" _AnomalousExport void Source_stop(Source* source)
{
	source->stop();
}

extern "C" _AnomalousExport void Source_pause(Source* source)
{
	source->pause();
}

extern "C" _AnomalousExport bool Source_resume(Source* source)
{
	return source->resume();
}

extern "C" _AnomalousExport void Source_rewind(Source* source)
{
	source->rewind();
}

extern "C" _AnomalousExport bool Source_getLooping(Source* source)
{
	return source->getLooping();
}

extern "C" _AnomalousExport void Source_setFinishedCallback(Source* source, SourceFinishedCallback callback)
{
	source->setFinishedCallback(callback);
}