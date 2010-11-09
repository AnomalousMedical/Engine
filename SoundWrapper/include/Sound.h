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

	virtual void close() = 0;

	virtual bool enqueueSource(Source* source) = 0;

	void setRepeat(bool value)
	{
		repeat = value;
	}

	bool getRepeat()
	{
		return repeat;
	}
};

}