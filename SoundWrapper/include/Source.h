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

	void rewind();

	bool getLooping();

//Properties

	//alSourcef
	void setPitch(float value)
	{
		alSourcef(sourceID, AL_PITCH, value);
	}

	float getPitch()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_PITCH, &returnVal);
		return returnVal;
	}

	void setGain(float value)
	{
		alSourcef(sourceID, AL_GAIN, value);
	}

	float getGain()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_GAIN, &returnVal);
		return returnVal;
	}

	void setMinGain(float value)
	{
		alSourcef(sourceID, AL_MIN_GAIN, value);
	}

	float getMinGain()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_MIN_GAIN, &returnVal);
		return returnVal;
	}

	void setMaxGain(float value)
	{
		alSourcef(sourceID, AL_MAX_GAIN, value);
	}

	float getMaxGain()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_MAX_GAIN, &returnVal);
		return returnVal;
	}

	void setMaxDistance(float value)
	{
		alSourcef(sourceID, AL_MAX_DISTANCE, value);
	}

	float getMaxDistance()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_MAX_DISTANCE, &returnVal);
		return returnVal;
	}

	void setRolloffFactor(float value)
	{
		alSourcef(sourceID, AL_ROLLOFF_FACTOR, value);
	}

	float getRolloffFactor()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_ROLLOFF_FACTOR, &returnVal);
		return returnVal;
	}

	void setConeOuterGain(float value)
	{
		alSourcef(sourceID, AL_CONE_OUTER_GAIN, value);
	}

	float getConeOuterGain()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_CONE_OUTER_GAIN, &returnVal);
		return returnVal;
	}

	void setConeInnerAngle(float value)
	{
		alSourcef(sourceID, AL_CONE_INNER_ANGLE, value);
	}

	float getConeInnerAngle()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_CONE_INNER_ANGLE, &returnVal);
		return returnVal;
	}

	void setConeOuterAngle(float value)
	{
		alSourcef(sourceID, AL_CONE_OUTER_ANGLE, value);
	}

	float getConeOuterAngle()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_CONE_OUTER_ANGLE, &returnVal);
		return returnVal;
	}

	void setReferenceDistance(float value)
	{
		alSourcef(sourceID, AL_REFERENCE_DISTANCE, value);
	}

	float getReferenceDistance()
	{
		float returnVal;
		alGetSourcef(sourceID, AL_REFERENCE_DISTANCE, &returnVal);
		return returnVal;
	}

	//alSourcefv
	void setPosition(const Vector3& value)
	{
		alSourcefv(sourceID, AL_POSITION, (ALfloat*)&value);
	}

	Vector3 getPosition()
	{
		Vector3 returnVal;
		alGetSourcefv(sourceID, AL_POSITION, (ALfloat*)&returnVal);
		return returnVal;
	}

	void setVelocity(const Vector3& value)
	{
		alSourcefv(sourceID, AL_VELOCITY, (ALfloat*)&value);
	}

	Vector3 getVelocity()
	{
		Vector3 returnVal;
		alGetSourcefv(sourceID, AL_VELOCITY, (ALfloat*)&returnVal);
		return returnVal;
	}

	void setDirection(const Vector3& value)
	{
		alSourcefv(sourceID, AL_DIRECTION, (ALfloat*)&value);
	}

	Vector3 getDirection()
	{
		Vector3 returnVal;
		alGetSourcefv(sourceID, AL_DIRECTION, (ALfloat*)&returnVal);
		return returnVal;
	}

	//alSourcei
	void setSourceRelative(bool value)
	{
		alSourcei(sourceID, AL_SOURCE_RELATIVE, value);
	}

	bool getSourceRelative()
	{
		int returnVal;
		alGetSourcei(sourceID, AL_SOURCE_RELATIVE, &returnVal);
		return returnVal;
	}

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