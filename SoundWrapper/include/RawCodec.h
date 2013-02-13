#pragma once

#include "AudioCodec.h"

namespace SoundWrapper
{

class Stream;

class RawCodec : public AudioCodec
{
public:
	RawCodec(Stream* stream, int channels, int sampleFreq, int duration);

	virtual ~RawCodec(void);

	virtual int getNumChannels()
	{
		return channels;
	}

	virtual int getSamplingFrequency()
	{
		return sampleFreq;
	}

	virtual size_t read(char* buffer, int length);

	virtual void close();

	virtual bool eof();

	virtual void seekToStart();

	virtual double getDuration()
	{
		return duration;
	}

	virtual void setPlaybackPosition(float time);

private:
	Stream* stream;
	int channels;
	int sampleFreq;
	int duration;
};

}