#pragma once

#include "Sound.h"

class AudioStream;

class MemorySound : public Sound
{
private:
    ALuint bufferID;                        //The OpenAL sound buffer ID

public:
	MemorySound(AudioStream* audioStream);

	virtual ~MemorySound(void);

	virtual void close();

	virtual bool enqueueSource(Source* source);

	ALuint getBufferID()
	{
		return bufferID;
	}
};
