#pragma once
#include "Sound.h"

class AudioStream;

class StreamingSound : public Sound
{
private:
	ALuint* bufferIDs;
	ALenum format;
    ALsizei freq;
	AudioStream* audioStream;
	int bufferSize;
	int numBuffers;

	Source* currentSource;

	void configure();

public:
	StreamingSound(AudioStream* audioStream);

	StreamingSound(AudioStream* audioStream, int bufferSize);

	StreamingSound(AudioStream* audioStream, int bufferSize, int numBuffers);

	virtual ~StreamingSound(void);

	virtual void close();

	virtual bool enqueueSource(Source* source);

	virtual bool update();

	virtual bool stream(ALuint buffer);
};
