#pragma once

namespace SoundWrapper
{

class AudioCodec
{
public:
	virtual ~AudioCodec() {};

	virtual int getNumChannels() = 0;

	virtual int getSamplingFrequency() = 0;

	virtual size_t read(char* buffer, int length) = 0;

	virtual void close() = 0;

	virtual bool eof() = 0;

	virtual void seekToStart() = 0;

	virtual double getDuration() = 0;
};

}