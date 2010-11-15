#include "StdAfx.h"
#include "StreamingSound.h"
#include "AudioCodec.h"
#include "Source.h"

namespace SoundWrapper
{

StreamingSound::StreamingSound(AudioCodec* audioCodec)
:audioCodec(audioCodec),
bufferSize(48000),
numBuffers(2)
{
	configure();
}

StreamingSound::StreamingSound(AudioCodec* audioCodec, int bufferSize)
:audioCodec(audioCodec),
bufferSize(bufferSize),
numBuffers(2)
{
	configure();
}

StreamingSound::StreamingSound(AudioCodec* audioCodec, int bufferSize, int numBuffers)
:audioCodec(audioCodec),
bufferSize(bufferSize),
numBuffers(numBuffers)
{
	configure();
}

void StreamingSound::configure()
{
	bufferIDs = new ALuint[numBuffers];

	//Create buffer.
	alGenBuffers(numBuffers, bufferIDs);
    checkOpenAL();

	if (audioCodec->getNumChannels() == 1)
	{
		format = AL_FORMAT_MONO16;
	}
	else
	{
		format = AL_FORMAT_STEREO16;
	}

	freq = audioCodec->getSamplingFrequency();
}

StreamingSound::~StreamingSound(void)
{
	close();
}

void StreamingSound::close()
{
	if(audioCodec != 0)
	{
		alDeleteBuffers(numBuffers, bufferIDs);
		delete[] bufferIDs;
		checkOpenAL();
		audioCodec->close();
		delete audioCodec;
		audioCodec = 0;
	}
}

bool StreamingSound::enqueueSource(Source* source)
{
	currentSource = source;

	audioCodec->seekToStart();

	for(int i = 0; i < numBuffers; ++i)
	{
		if(!stream(bufferIDs[i]))
		{
			return false;
		}
	}
	alSourcei(source->getSourceID(), AL_LOOPING, false);
	alSourceQueueBuffers(source->getSourceID(), numBuffers, bufferIDs);
	return true;
}

bool StreamingSound::update()
{
	int source = currentSource->getSourceID();
	int processed;
    bool active = true;
 
	alGetSourcei(source, AL_BUFFERS_PROCESSED, &processed);
 
    while(processed--)
    {
        ALuint buffer;
        
        alSourceUnqueueBuffers(source, 1, &buffer);
        checkOpenAL();
 
        active = stream(buffer);
 
		if(active)
		{
			alSourceQueueBuffers(source, 1, &buffer);
			checkOpenAL();
		}
    }

    return active;
}

void StreamingSound::readBuffers(char* data, int& size)
{
	int result;
	while(size < bufferSize)
    {
		result = audioCodec->read(data + size, bufferSize - size);
    
        if(result > 0)
		{
            size += result;
		}
        else
		{
			break;
		}
    }
}

bool StreamingSound::stream(ALuint buffer)
{
	char* data = new char[bufferSize];
    int  size = 0;
 
	//Read from the stream normally.
    readBuffers(data, size);

	/// Check to see if the stream has been set to repeat and if so did we get
    /// too small of a buffer and it needs data from the start of the stream.
	if(repeat && size < bufferSize)
	{
		audioCodec->seekToStart();
		readBuffers(data, size);
	}
    
	//Make sure some data was read.
    if(size == 0)
	{
        return false;
	}
 
	//Queue the buffer
    alBufferData(buffer, format, data, size, freq);
    checkOpenAL();

	delete[] data;

    return true;
}

double StreamingSound::getDuration()
{
	return audioCodec->getDuration();
}

}