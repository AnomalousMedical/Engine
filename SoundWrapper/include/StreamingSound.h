#pragma once
#include "Sound.h"

namespace SoundWrapper
{

class AudioCodec;

class StreamingSound : public Sound
{
private:
	ALuint* bufferIDs;
	ALenum format;
    ALsizei freq;
	AudioCodec* audioCodec;
	int bufferSize;
	int numBuffers;

	Source* currentSource;

	void configure();

public:
	StreamingSound(AudioCodec* audioCodec);

	StreamingSound(AudioCodec* audioCodec, int bufferSize);

	StreamingSound(AudioCodec* audioCodec, int bufferSize, int numBuffers);

	virtual ~StreamingSound(void);

	virtual void close();

	virtual bool enqueueSource(Source* source);

	virtual bool update();

	virtual bool stream(ALuint buffer);

	virtual double getDuration();

	virtual void setPlaybackPosition(float time);

private:
	void readBuffers(char* data, int& size);
};

}