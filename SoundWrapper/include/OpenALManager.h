#pragma once

namespace SoundWrapper
{

class SourcePool;
class Sound;
class Stream;
class AudioCodec;

class OpenALManager
{
private:
	ALCcontext* context;
	ALCdevice* device;
	bool ready;
	SourcePool* sourcePool;

public:
	OpenALManager(void);

	~OpenALManager(void);

	bool isReady()
	{
		return ready;
	}

	Sound* createMemorySound(Stream* stream);

	Sound* createStreamingSound(Stream* stream);

	Sound* createStreamingSound(Stream* stream, int bufferSize, int numBuffers);

	void destroySound(Sound* sound);

private:
	AudioCodec* getCodecForStream(Stream* stream);

};

}