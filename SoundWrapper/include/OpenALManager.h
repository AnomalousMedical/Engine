#pragma once

namespace SoundWrapper
{

class SourceManager;
class Sound;
class Stream;
class AudioCodec;
class Source;
class Listener;

class OpenALManager
{
private:
	ALCcontext* context;
	ALCdevice* device;
	bool ready;
	SourceManager* sourceManager;
	Listener* listener;

public:
	OpenALManager(void);

	~OpenALManager(void);

	bool isReady()
	{
		return ready;
	}

	AudioCodec* createAudioCodec(Stream* stream);

	void destroyAudioCodec(AudioCodec* codec);

	Sound* createMemorySound(Stream* stream);

	Sound* createMemorySound(AudioCodec* codec);

	Sound* createStreamingSound(Stream* stream);

	Sound* createStreamingSound(AudioCodec* codec);

	Sound* createStreamingSound(Stream* stream, int bufferSize, int numBuffers);

	Sound* createStreamingSound(AudioCodec* codec, int bufferSize, int numBuffers);

	void destroySound(Sound* sound);

	Source* getSource();

	void update();

	Listener* getListener()
	{
		return listener;
	}

private:
	AudioCodec* getCodecForStream(Stream* stream);

};

}