#include "StdAfx.h"
#include "StreamingSound.h"
#include "AudioStream.h"
#include "Source.h"

#include <vector>
using namespace std;

StreamingSound::StreamingSound(AudioStream* audioStream)
:audioStream(audioStream),
bufferSize(48000),
numBuffers(2)
{
	configure();
}

StreamingSound::StreamingSound(AudioStream* audioStream, int bufferSize)
:audioStream(audioStream),
bufferSize(bufferSize),
numBuffers(2)
{
	configure();
}

StreamingSound::StreamingSound(AudioStream* audioStream, int bufferSize, int numBuffers)
:audioStream(audioStream),
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

	if (audioStream->getNumChannels() == 1)
	{
		format = AL_FORMAT_MONO16;
	}
	else
	{
		format = AL_FORMAT_STEREO16;
	}

	freq = audioStream->getSamplingFrequency();
}

StreamingSound::~StreamingSound(void)
{
	close();
}

void StreamingSound::close()
{
	alDeleteBuffers(numBuffers, bufferIDs);
	delete[] bufferIDs;
    checkOpenAL();
	audioStream->close();
}

bool StreamingSound::enqueueSource(Source* source)
{
	currentSource = source;

	for(int i = 0; i < numBuffers; ++i)
	{
		if(!stream(bufferIDs[i]))
		{
			return false;
		}
	}

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
 
        alSourceQueueBuffers(source, 1, &buffer);
        checkOpenAL();
    }

    return active;
}

bool StreamingSound::stream(ALuint buffer)
{
	char* data = new char[bufferSize];
    int  size = 0;
    int  section;
    int  result;
 
    while(size < bufferSize)
    {
		result = audioStream->read(data + size, bufferSize - size);
    
        if(result > 0)
		{
            size += result;
		}
        else
		{
			break;
		}
    }
    
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