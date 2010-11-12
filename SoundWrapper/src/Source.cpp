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

extern "C" _AnomalousExport void Source_setPitch(Source* source, float value)
{
	source->setPitch(value);
}

extern "C" _AnomalousExport float Source_getPitch(Source* source)
{
	return source->getPitch();
}

extern "C" _AnomalousExport void Source_setGain(Source* source, float value)
{
	source->setGain(value);
}

extern "C" _AnomalousExport float Source_getGain(Source* source)
{
	return source->getGain();
}

extern "C" _AnomalousExport void Source_setMinGain(Source* source, float value)
{
	source->setMinGain(value);
}

extern "C" _AnomalousExport float Source_getMinGain(Source* source)
{
	return source->getMinGain();
}

extern "C" _AnomalousExport void Source_setMaxGain(Source* source, float value)
{
	source->setMaxGain(value);
}

extern "C" _AnomalousExport float Source_getMaxGain(Source* source)
{
	return source->getMaxGain();
}

extern "C" _AnomalousExport void Source_setMaxDistance(Source* source, float value)
{
	source->setMaxDistance(value);
}

extern "C" _AnomalousExport float Source_getMaxDistance(Source* source)
{
	return source->getMaxDistance();
}

extern "C" _AnomalousExport void Source_setRolloffFactor(Source* source, float value)
{
	source->setRolloffFactor(value);
}

extern "C" _AnomalousExport float Source_getRolloffFactor(Source* source)
{
	return source->getRolloffFactor();
}

extern "C" _AnomalousExport void Source_setConeOuterGain(Source* source, float value)
{
	source->setConeOuterGain(value);
}

extern "C" _AnomalousExport float Source_getConeOuterGain(Source* source)
{
	return source->getConeOuterGain();
}

extern "C" _AnomalousExport void Source_setConeInnerAngle(Source* source, float value)
{
	source->setConeInnerAngle(value);
}

extern "C" _AnomalousExport float Source_getConeInnerAngle(Source* source)
{
	return source->getConeInnerAngle();
}

extern "C" _AnomalousExport void Source_setConeOuterAngle(Source* source, float value)
{
	source->setConeOuterAngle(value);
}

extern "C" _AnomalousExport float Source_getConeOuterAngle(Source* source)
{
	return source->getConeOuterAngle();
}

extern "C" _AnomalousExport void Source_setReferenceDistance(Source* source, float value)
{
	source->setReferenceDistance(value);
}

extern "C" _AnomalousExport float Source_getReferenceDistance(Source* source)
{
	return source->getReferenceDistance();
}

extern "C" _AnomalousExport void Source_setPosition(Source* source, Vector3 value)
{
	source->setPosition(value);
}

extern "C" _AnomalousExport Vector3 Source_getPosition(Source* source)
{
	return source->getPosition();
}

extern "C" _AnomalousExport void Source_setVelocity(Source* source, Vector3 value)
{
	source->setVelocity(value);
}

extern "C" _AnomalousExport Vector3 Source_getVelocity(Source* source)
{
	return source->getVelocity();
}

extern "C" _AnomalousExport void Source_setDirection(Source* source, Vector3 value)
{
	source->setDirection(value);
}

extern "C" _AnomalousExport Vector3 Source_getDirection(Source* source)
{
	return source->getDirection();
}

extern "C" _AnomalousExport void Source_setSourceRelative(Source* source, bool value)
{
	source->setSourceRelative(value);
}

extern "C" _AnomalousExport bool Source_getSourceRelative(Source* source)
{
	return source->getSourceRelative();
}

extern "C" _AnomalousExport void Source_setFinishedCallback(Source* source, SourceFinishedCallback callback)
{
	source->setFinishedCallback(callback);
}