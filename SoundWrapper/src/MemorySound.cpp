#include "StdAfx.h"
#include "MemorySound.h"
#include "AudioStream.h"
#include "Source.h"


#include <vector>
using namespace std;

MemorySound::MemorySound(AudioStream* audioStream)
{
	ALenum format;
    ALsizei freq;

	//Create buffer.
	alGenBuffers(1, &bufferID);
    check();

	//Load the actual sound data from the buffer.
	long bytes;
	char array[BUFFER_SIZE];    // Local fixed size array
	vector<char> buffer;

	if (audioStream->getNumChannels() == 1)
	{
		format = AL_FORMAT_MONO16;
	}
	else
	{
		format = AL_FORMAT_STEREO16;
	}

	freq = audioStream->getSamplingFrequency();

	do 
	{
		// Read up to a buffer's worth of decoded sound data
		bytes = audioStream->read(array, BUFFER_SIZE);
		// Append to end of buffer
		buffer.insert(buffer.end(), array, array + bytes);
	} 
	while (bytes > 0);

	alBufferData(bufferID, format, &buffer[0], static_cast<ALsizei>(buffer.size()), freq);

	audioStream->close();
}

MemorySound::~MemorySound(void)
{
	close();
}

void MemorySound::close()
{
	alDeleteBuffers(1, &bufferID);
    check();
}

void MemorySound::enqueueSource(Source* source)
{
	alSourcei(source->getSourceID(), AL_BUFFER, bufferID);
}

void MemorySound::check()
{
    int error = alGetError();
 
    if(error != AL_NO_ERROR)
	{
        
	}
}

