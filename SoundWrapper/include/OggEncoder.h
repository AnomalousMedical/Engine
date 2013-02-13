#pragma once

namespace SoundWrapper
{

class Stream;

class OggEncoder
{
public:
	OggEncoder(void);

	virtual ~OggEncoder(void);

	bool encodeToStream(Stream* source, Stream* destination);

	long getChannels()
	{
		return channels;
	}

	void setChannels(long value)
	{
		channels = value;
	}

	long getRate()
	{
		return rate;
	}

	void setRate(long value)
	{
		rate = value;
	}

	float getBaseQuality()
	{
		return baseQuality;
	}

	void setBaseQuality(float value)
	{
		baseQuality = value;
	}

private:
	long channels;
	long rate;
	float baseQuality;
};

}