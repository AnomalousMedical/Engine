#pragma once

#include "Sound.h"

class Source
{
private:
	ALuint sourceID;
	bool paused;

public:
	Source(ALuint sourceID);

	~Source(void);

	ALuint getSourceID()
	{
		return sourceID;
	}

	bool playSound(Sound* sound);

	bool playing();

	void stop();

	void pause();

	bool resume();

protected:
	void empty();
};
