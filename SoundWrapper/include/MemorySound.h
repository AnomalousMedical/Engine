#pragma once

#include "Sound.h"

namespace SoundWrapper
{

class AudioCodec;

class MemorySound : public Sound
{
private:
    ALuint bufferID;                        //The OpenAL sound buffer ID
	double duration;
	Source* currentSource;

public:
	MemorySound(AudioCodec* audioCodec);

	virtual ~MemorySound(void);

	virtual void close();

	virtual bool enqueueSource(Source* source);

	ALuint getBufferID()
	{
		return bufferID;
	}

	virtual bool update()
	{
		return true;
	}

	virtual double getDuration()
	{
		return duration;
	}

	virtual void setPlaybackPosition(float time);
};

}