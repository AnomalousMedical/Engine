#pragma once

#include "AudioCodec.h"
#include <vorbis/vorbisfile.h>
#include <string>

using namespace std;

namespace SoundWrapper
{

class Stream;

class OggCodec : public AudioCodec
{
private:
	OggVorbis_File oggStream;
	vorbis_info *pInfo;
	Stream* stream; //Stream is deleted in close_cb

public:
	OggCodec(Stream* stream);

	virtual ~OggCodec(void);

	virtual int getNumChannels();

	virtual int getSamplingFrequency();

	virtual size_t read(char* buffer, int length);

	virtual void close();

	virtual bool eof();

	virtual void seekToStart();

private:
	static size_t read_cb(void *ptr, size_t size, size_t nmemb, void *datasource);

	static int seek_cb(void *datasource, ogg_int64_t offset, int whence);

	static int close_cb(void *datasource);

	static long tell_cb(void *datasource);

	static string errorString(int code);
};

}