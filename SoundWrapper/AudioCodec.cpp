#include "StdAfx.h"
#include "AudioCodec.h"

//CWrapper
using namespace SoundWrapper;

extern "C" _AnomalousExport int AudioCodec_getNumChannels(AudioCodec* codec)
{
	return codec->getNumChannels();
}

extern "C" _AnomalousExport int AudioCodec_getSamplingFrequency(AudioCodec* codec)
{
	return codec->getSamplingFrequency();
}

extern "C" _AnomalousExport size_t AudioCodec_read(AudioCodec* codec, char* buffer, int length)
{
	return codec->read(buffer, length);
}

extern "C" _AnomalousExport void AudioCodec_close(AudioCodec* codec)
{
	codec->close();
}

extern "C" _AnomalousExport bool AudioCodec_eof(AudioCodec* codec)
{
	return codec->eof();
}

extern "C" _AnomalousExport void AudioCodec_seekToStart(AudioCodec* codec)
{
	codec->seekToStart();
}

extern "C" _AnomalousExport double AudioCodec_getDuration(AudioCodec* codec)
{
	return codec->getDuration();
}