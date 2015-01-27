#pragma once

namespace SoundWrapper
{
class OpenALManager;

const int BUFFER_SIZE = 22050;

typedef void(*NativeBufferFullCallback)(ALbyte* buffer, int length HANDLE_ARG);

class CaptureDevice
{
public:
	CaptureDevice(OpenALManager* manager, BufferFormat format = Stereo16, int bufferSeconds = 5, int rate = 44100);

	virtual ~CaptureDevice(void);

	bool isValid()
	{
		return device != NULL;
	}

	void start(NativeBufferFullCallback callback HANDLE_ARG);

	void stop();

	void update();

	bool active()
	{
		return currentCallback != NULL;
	}

private:
	void readDevice();

	ALCdevice *device;
	ALbyte buffer[BUFFER_SIZE];
	NativeBufferFullCallback currentCallback;
	ALCenum format;
	int rate;
	int bufferSeconds;
	int sampleSize;
	int maxSampleRead;
	OpenALManager* manager;
	HANDLE_INSTANCE
};

}
