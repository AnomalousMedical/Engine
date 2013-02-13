#include "stdafx.h"
#include "..\include\RawCodec.h"
#include "Stream.h"

namespace SoundWrapper
{

RawCodec::RawCodec(Stream* stream, int channels, int sampleFreq, int duration)
	:stream(stream),
	channels(channels),
	sampleFreq(sampleFreq),
	duration(duration)
{
}


RawCodec::~RawCodec(void)
{
	close();
}

size_t RawCodec::read(char* buffer, int length)
{
	return stream->read(buffer, 1, length);
}

void RawCodec::close()
{
	if(stream != NULL)
	{
		stream->close();
		delete stream;
		stream = NULL;
	}
}

bool RawCodec::eof()
{
	return stream->eof();
}

void RawCodec::seekToStart()
{
	stream->seek(0, 0);
}

void RawCodec::setPlaybackPosition(float time)
{

}

}