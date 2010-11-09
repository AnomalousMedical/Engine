#pragma once

#include "Stream.h"
#include <fstream>



namespace SoundWrapper
{

class NativeStream : public Stream
{
private:
	FILE* f;

public:
	NativeStream(const char* file);

	virtual ~NativeStream(void);

	virtual size_t read(void* buffer, int size, int count);

	virtual int seek(long offset, SeekMode origin);

	virtual void close();

	virtual size_t tell();

	virtual bool eof();
};

}