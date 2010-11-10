#include "StdAfx.h"
#include "OggCodec.h"
#include "Stream.h"

namespace SoundWrapper
{

OggCodec::OggCodec(Stream* stream)
:stream(stream)
{
	ov_callbacks callbacks;
	callbacks.read_func = read_cb;
    callbacks.seek_func = seek_cb;
    callbacks.tell_func = tell_cb;
    callbacks.close_func = close_cb;

	int result = 0;

	if((result = ov_open_callbacks(stream, &oggStream, NULL, 0, callbacks)) < 0)
	{
		//cout << "Ogg error " << errorString(result) << endl;
	}

	pInfo = ov_info(&oggStream, -1);
}

OggCodec::~OggCodec(void)
{
	close();
}

int OggCodec::getNumChannels()
{
	return pInfo->channels;
}

int OggCodec::getSamplingFrequency()
{
	return pInfo->rate;
}

size_t OggCodec::read(char* buffer, int length)
{
	int bitStream;
	return ov_read(&oggStream, buffer, length, ENDIAN, 2, 1, &bitStream);
}

void OggCodec::close()
{
	ov_clear(&oggStream);
}

bool OggCodec::eof()
{
	return stream->eof();
}

void OggCodec::seekToStart()
{
	ov_raw_seek(&oggStream, 0);
}

size_t OggCodec::read_cb(void *ptr, size_t size, size_t nmemb, void *datasource)
{
	return ((Stream*)datasource)->read(ptr, size, nmemb);
}

int OggCodec::seek_cb(void *datasource, ogg_int64_t offset, int whence)
{
	return ((Stream*)datasource)->seek((long)offset, whence);
}

int OggCodec::close_cb(void *datasource)
{
	Stream* stream = (Stream*)datasource;
	stream->close();
	delete stream;
	return 0; //ignored by ogg/vorbis
}

long OggCodec::tell_cb(void *datasource)
{
	return ((Stream*)datasource)->tell();
}

string OggCodec::errorString(int code)
{
    switch(code)
    {
        case OV_EREAD:
            return string("Error reading from media.");
        case OV_ENOTVORBIS:
            return string("Not Vorbis data.");
        case OV_EVERSION:
            return string("Vorbis version mismatch.");
        case OV_EBADHEADER:
            return string("Invalid Vorbis header.");
        case OV_EFAULT:
            return string("Internal logic fault (bug or heap/stack corruption.");
        default:
            return string("Unknown Ogg error.");
    }
}

}