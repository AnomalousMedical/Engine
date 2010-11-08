#pragma once

#include "AudioStream.h"
#include <fstream>
#include <vorbis/vorbisfile.h>
#include <string>

using namespace std;

class OggStream : public AudioStream
{
private:
	OggVorbis_File oggStream;
	vorbis_info *pInfo;
	FILE* f;

public:
	OggStream(const char* file);

	virtual ~OggStream(void);

	virtual int getNumChannels();

	virtual int getSamplingFrequency();

	virtual size_t read(char* buffer, int length);

	virtual void close();

	virtual bool eof();

private:
	static size_t read_cb(void *ptr, size_t size, size_t nmemb, void *datasource);

	static int seek_cb(void *datasource, ogg_int64_t offset, int whence);

	static int close_cb(void *datasource);

	static long tell_cb(void *datasource);

	static string errorString(int code);
};
