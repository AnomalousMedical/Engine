#pragma once

namespace SoundWrapper
{

class Listener
{
public:
	Listener(void);

	~Listener(void);

	float getGain()
	{
		float value;
		alGetListenerf(AL_GAIN, &value);
		return value;
	}

	void setGain(float value)
	{
		alListenerf(AL_GAIN, value);
	}

	Vector3 getPosition()
	{
		Vector3 value;
		alGetListenerfv(AL_POSITION, (ALfloat*)&value);
		return value;
	}

	void setPosition(Vector3 value)
	{
		alListenerfv(AL_POSITION, (ALfloat*)&value);
	}

	Vector3 getVelocity()
	{
		Vector3 value;
		alGetListenerfv(AL_VELOCITY, (ALfloat*)&value);
		return value;
	}

	void setVelocity(Vector3 value)
	{
		alListenerfv(AL_VELOCITY, (ALfloat*)&value);
	}

	void getOrientation(Vector3& at, Vector3& up)
	{
		Vector3 value[2];
		alGetListenerfv(AL_ORIENTATION, (ALfloat*)&value[0]);
		at = value[0];
		up = value[1];
	}

	void setOrientation(const Vector3& at, const Vector3& up)
	{
		Vector3 value[] = {at, up};
		alListenerfv(AL_ORIENTATION, (ALfloat*)&value[0]);
	}
};

}