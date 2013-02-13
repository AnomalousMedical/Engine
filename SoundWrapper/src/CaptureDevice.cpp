#include "StdAfx.h"
#include "CaptureDevice.h"
#include "OpenALManager.h"

namespace SoundWrapper
{

CaptureDevice::CaptureDevice(OpenALManager* manager, BufferFormat format, int bufferSeconds, int rate)
	:currentCallback(NULL),
	manager(manager),
	bufferSeconds(bufferSeconds),
	rate(rate)
{
	switch(format)
	{
		case Mono8:
			this->format = AL_FORMAT_MONO8;
			sampleSize = 1;
			break;
		case Mono16:
			this->format = AL_FORMAT_MONO16;
			sampleSize = 2;
			break;
		case Stereo8:
			this->format = AL_FORMAT_STEREO8;
			sampleSize = 2;
			break;
		case Stereo16:
			this->format = AL_FORMAT_STEREO16;
			sampleSize = 4;
			break;
	}
	int size = rate * sampleSize * bufferSeconds;
	maxSampleRead = BUFFER_SIZE / sampleSize;

	device = alcCaptureOpenDevice(NULL, rate, this->format, size);
	ALenum errorCode = alGetError();
    if (errorCode != AL_NO_ERROR) {
        logger << "Error creatig OpenAL Capture Device." << error;
		device = NULL;
    }
	else
	{
		logger << "Opened OpenAL Capture Device." << error;
	}
}

CaptureDevice::~CaptureDevice(void)
{
	if(isValid())
	{
		if(active())
		{
			stop();
		}
		alcCaptureCloseDevice(device);
		logger << "Closed OpenAL Capture Device." << error;
	}
}

void CaptureDevice::start(BufferFullCallback callback)
{
	if(isValid())
	{
		if(active())
		{
			stop();
		}
		this->currentCallback = callback;
		alcCaptureStart(device);
		manager->addCaptureDeviceUpdate(this);
	}
}

void CaptureDevice::stop()
{
	if(isValid() && active())
	{
		manager->removeCaptureDeviceUpdate(this);
		readDevice();

		alcCaptureStop(device);
		this->currentCallback = NULL;
	}
}

void CaptureDevice::update()
{
	if(active())
	{
		readDevice();
	}
}

void CaptureDevice::readDevice()
{
	ALint totalSamples;
	alcGetIntegerv(device, ALC_CAPTURE_SAMPLES, (ALCsizei)sizeof(ALint), &totalSamples);

	int readSamples = maxSampleRead;
	while(totalSamples > 0)
	{
		if(totalSamples < readSamples)
		{
			readSamples = totalSamples;
		}
		alcCaptureSamples(device, (ALCvoid *)buffer, readSamples);
		currentCallback(&buffer[0], readSamples * sampleSize);
		totalSamples -= readSamples;
	}
}

}

//CWrapper

using namespace SoundWrapper;

extern "C" _AnomalousExport void CaptureDevice_Start(CaptureDevice* captureDevice, BufferFullCallback callback)
{
	captureDevice->start(callback);
}

extern "C" _AnomalousExport void CaptureDevice_Stop(CaptureDevice* captureDevice)
{
	captureDevice->stop();
}