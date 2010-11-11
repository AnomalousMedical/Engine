#pragma once

#include "Sound.h"

namespace SoundWrapper
{

typedef void (*SourceFinishedCallback)(Source* source);

class SourceManager;
class Sound;

class Source
{
private:
	ALuint sourceID;
	bool paused;
	SourceManager* sourceManager;
	Sound* currentSound;
	SourceFinishedCallback finishedCallback;

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

	void setFinishedCallback(SourceFinishedCallback callback)
	{
		finishedCallback = callback;
	}

	//Internal do not create wrapper
	//Call only from SourceManager. Will cause underlying sound to update and checks to see if the sound is finished.
	void _update();

protected:
	void empty();

	void finished();
};

}