#include "StdAfx.h"
#include "NativeStream.h"

namespace SoundWrapper
{

NativeStream::NativeStream(const char* file)
{
	f = fopen(file, "rb");
}

NativeStream::~NativeStream(void)
{
	if(f != NULL)
	{
		close();
	}
}

size_t NativeStream::read(void* buffer, int size, int count)
{
	return fread(buffer, size, count, f);
}

int NativeStream::seek(long offset, SeekMode origin)
{
	return fseek(f, (long)offset, (int)origin);
}

void NativeStream::close()
{
	fclose(f);
	f = NULL;
}

size_t NativeStream::tell()
{
	return ftell(f);
}

bool NativeStream::eof()
{
	return feof(f);
}

}