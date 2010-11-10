#pragma once

namespace SoundWrapper
{

class Source;

class Sound
{
protected:
	bool repeat;

public:
	Sound(void);

	virtual ~Sound(void);

	void setRepeat(bool value)
	{
		repeat = value;
	}

	bool getRepeat()
	{
		return repeat;
	}

	//Internal, do not expose via wrapper
	virtual void close() = 0;

	virtual bool enqueueSource(Source* source) = 0;

	virtual bool update() = 0;
};

}