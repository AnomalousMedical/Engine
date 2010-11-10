#pragma once

#include "Sound.h"

namespace SoundWrapper
{

class SourceManager;
class Sound;

class Source
{
private:
	ALuint sourceID;
	bool paused;
	SourceManager* sourceManager;
	Sound* currentSound;

public:
	Source(ALuint sourceID, SourceManager* sourceManager);

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

	bool getLooping();

	//Internal do not create wrapper
	//Call only from SourceManager. Will cause underlying sound to update and checks to see if the sound is finished.
	void _update();

protected:
	void empty();
};

}