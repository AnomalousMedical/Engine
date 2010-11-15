#include "StdAfx.h"
#include "MemorySound.h"
#include "AudioCodec.h"
#include "Source.h"

#include <vector>
using namespace std;

#define BUFFER_SIZE   32768     // 32 KB buffers

namespace SoundWrapper
{

MemorySound::MemorySound(AudioCodec* audioCodec)
{
	ALenum format;
    ALsizei freq;

	//Create buffer.
	alGenBuffers(1, &bufferID);
    checkOpenAL();

	//Load the actual sound data from the buffer.
	long bytes;
	char array[BUFFER_SIZE];    // Local fixed size array
	vector<char> buffer;

	if (audioCodec->getNumChannels() == 1)
	{
		format = AL_FORMAT_MONO16;
	}
	else
	{
		format = AL_FORMAT_STEREO16;
	}

	freq = audioCodec->getSamplingFrequency();
	duration = audioCodec->getDuration();

	do 
	{
		// Read up to a buffer's worth of decoded sound data
		bytes = audioCodec->read(array, BUFFER_SIZE);
		// Append to end of buffer
		buffer.insert(buffer.end(), array, array + bytes);
	} 
	while (bytes > 0);

	alBufferData(bufferID, format, &buffer[0], static_cast<ALsizei>(buffer.size()), freq);

	audioCodec->close();
}

MemorySound::~MemorySound(void)
{
	close();
}

void MemorySound::close()
{
	alDeleteBuffers(1, &bufferID);
    checkOpenAL();
}

bool MemorySound::enqueueSource(Source* source)
{
	alSourcei(source->getSourceID(), AL_LOOPING, repeat);
	alSourcei(source->getSourceID(), AL_BUFFER, bufferID);
	return true;
}

}