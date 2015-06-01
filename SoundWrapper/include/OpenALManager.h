#pragma once

#include <list>

namespace SoundWrapper
{

class CaptureDevice;
class SourceManager;
class Sound;
class Stream;
class AudioCodec;
class Source;
class Listener;

class OpenALManager
{
public:
	OpenALManager(void);

	~OpenALManager(void);

	bool isReady()
	{
		return ready;
	}

	CaptureDevice* createCaptureDevice(BufferFormat format = Stereo16, int bufferSeconds = 5, int rate = 44100);

	void destroyCaptureDevice(CaptureDevice* captureDevice);

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

	void addCaptureDeviceUpdate(CaptureDevice* captureDevice)
	{
		activeDevices.push_back(captureDevice);
	}

	void removeCaptureDeviceUpdate(CaptureDevice* captureDevice)
	{
		activeDevices.remove(captureDevice);
	}

	void createDevice();

	void destroyDevice();

private:
	AudioCodec* getCodecForStream(Stream* stream);

	ALCcontext* context;
	ALCdevice* device;
	bool ready;
	SourceManager* sourceManager;
	Listener* listener;
	std::list<CaptureDevice*> activeDevices;
};

}