#include "StdAfx.h"
#include "OpenALManager.h"
#include "SourceManager.h"
#include "MemorySound.h"
#include "StreamingSound.h"
#include "Listener.h"

//Codecs
#include "OggCodec.h"

namespace SoundWrapper
{

OpenALManager::OpenALManager(void)
:ready(true),
listener(new Listener())
{
	logger << "Starting OpenAL" << info;
	
	device = alcOpenDevice(NULL);
	if (device == NULL)
	{
		logger << "Error creatig OpenAL Device." << error;
		ready = false;
		return;
	}

	const ALchar* vendor = alGetString(AL_VENDOR);
	if(vendor != NULL)
	{
		logger << " Vendor: " << vendor << info;
	}

	const ALchar* version = alGetString(AL_VERSION);
	if(version != NULL)
	{
		logger << " Version: " << version << info;
	}

	const ALchar* renderer = alGetString(AL_RENDERER);
	if(renderer != NULL)
	{
		logger << " Renderer: " << renderer << info;
	}

	//Create context(s)
	context = alcCreateContext(device,NULL);

	//Set active context
	alcMakeContextCurrent(context);

	// Clear Error Code
	alGetError();

	sourceManager = new SourceManager();
}

OpenALManager::~OpenALManager(void)
{
	logger << "Shutting down OpenAL" << info;

	delete sourceManager;

	//Disable context
	alcMakeContextCurrent(NULL);
	//Release context(s)
	alcDestroyContext(context);
	//Close device
	alcCloseDevice(device);

	delete listener;
}

AudioCodec* OpenALManager::createAudioCodec(Stream* stream)
{
	return getCodecForStream(stream);
}

void OpenALManager::destroyAudioCodec(AudioCodec* codec)
{
	delete codec;
}

Sound* OpenALManager::createMemorySound(Stream* stream)
{
	return new MemorySound(getCodecForStream(stream));
}

Sound* OpenALManager::createMemorySound(AudioCodec* codec)
{
	return new MemorySound(codec);
}

Sound* OpenALManager::createStreamingSound(Stream* stream)
{
	return new StreamingSound(getCodecForStream(stream));
}

Sound* OpenALManager::createStreamingSound(AudioCodec* codec)
{
	return new StreamingSound(codec);
}

Sound* OpenALManager::createStreamingSound(Stream* stream, int bufferSize, int numBuffers)
{
	return new StreamingSound(getCodecForStream(stream), bufferSize, numBuffers);
}

Sound* OpenALManager::createStreamingSound(AudioCodec* codec, int bufferSize, int numBuffers)
{
	return new StreamingSound(codec, bufferSize, numBuffers);
}

void OpenALManager::destroySound(Sound* sound)
{
	delete sound;
}

AudioCodec* OpenALManager::getCodecForStream(Stream* stream)
{
	return new OggCodec(stream);
}

Source* OpenALManager::getSource()
{
	return sourceManager->getPooledSource();
}

void OpenALManager::update()
{
	sourceManager->_update();
}

}


//CWrapper

using namespace SoundWrapper;

extern "C" _AnomalousExport OpenALManager* OpenALManager_create()
{
	return new OpenALManager();
}

extern "C" _AnomalousExport void OpenALManager_destroy(OpenALManager* openALManager)
{
	delete openALManager;
}

extern "C" _AnomalousExport AudioCodec* OpenALManager_createAudioCodec(OpenALManager* openALManager, Stream* stream)
{
	return openALManager->createAudioCodec(stream);
}

extern "C" _AnomalousExport void OpenALManager_destroyAudioCodec(OpenALManager* openALManager, AudioCodec* codec)
{
	openALManager->destroyAudioCodec(codec);
}

extern "C" _AnomalousExport Sound* OpenALManager_createMemorySound(OpenALManager* openALManager, Stream* stream)
{
	return openALManager->createMemorySound(stream);
}

extern "C" _AnomalousExport Sound* OpenALManager_createMemorySoundCodec(OpenALManager* openALManager, AudioCodec* codec)
{
	return openALManager->createMemorySound(codec);
}

extern "C" _AnomalousExport Sound* OpenALManager_createStreamingSound(OpenALManager* openALManager, Stream* stream)
{
	return openALManager->createStreamingSound(stream);
}

extern "C" _AnomalousExport Sound* OpenALManager_createStreamingSoundCodec(OpenALManager* openALManager, AudioCodec* codec)
{
	return openALManager->createStreamingSound(codec);
}

extern "C" _AnomalousExport Sound* OpenALManager_createStreamingSound2(OpenALManager* openALManager, Stream* stream, int bufferSize, int numBuffers)
{
	return openALManager->createStreamingSound(stream, bufferSize, numBuffers);
}

extern "C" _AnomalousExport Sound* OpenALManager_createStreamingSound2Codec(OpenALManager* openALManager, AudioCodec* codec, int bufferSize, int numBuffers)
{
	return openALManager->createStreamingSound(codec, bufferSize, numBuffers);
}

extern "C" _AnomalousExport void OpenALManager_destroySound(OpenALManager* openALManager, Sound* sound)
{
	openALManager->destroySound(sound);
}

extern "C" _AnomalousExport Source* OpenALManager_getSource(OpenALManager* openALManager)
{
	return openALManager->getSource();	
}

extern "C" _AnomalousExport void OpenALManager_update(OpenALManager* openALManager)
{
	openALManager->update();	
}

extern "C" _AnomalousExport Listener* OpenALManager_getListener(OpenALManager* openALManager)
{
	return openALManager->getListener();
}