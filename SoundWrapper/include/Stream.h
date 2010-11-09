#pragma once

namespace SoundWrapper
{

enum SeekMode
{
	Set = 0,
	Current = 1,
	End = 2,
};

class Stream
{
public:
	virtual ~Stream(void)
	{
	}

	virtual size_t read(void* buffer, int size, int count) = 0;

	virtual int seek(long offset, SeekMode origin) = 0;

	virtual void close() = 0;

	virtual size_t tell() = 0;

	virtual bool eof() = 0;
};

}