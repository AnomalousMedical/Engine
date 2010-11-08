#pragma once

#include "Sound.h"

class Source
{
private:
	ALuint sourceID;                        // The OpenAL sound source

public:
	Source(ALuint sourceID);

	~Source(void);

	ALuint getSourceID()
	{
		return sourceID;
	}

	bool playSound(Sound* sound);

	bool playing();

protected:
	void check();

	void empty();
};
