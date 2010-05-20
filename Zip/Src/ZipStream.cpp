#include "Stdafx.h"

extern "C" __declspec(dllexport) void ZipStream_FileClose(ZZIP_FILE* zzipFile)
{
	zzip_file_close(zzipFile);
}

extern "C" __declspec(dllexport) long ZipStream_Seek(ZZIP_FILE* zzipFile, long offset, int whence)
{
	return zzip_seek(zzipFile, static_cast<zzip_off_t>(offset), whence);
}

extern "C" __declspec(dllexport) int ZipStream_FileRead(ZZIP_FILE* zzipFile, void* buf, int count)
{
	return zzip_file_read(zzipFile, buf, static_cast<zzip_size_t>(count));
}

extern "C" __declspec(dllexport) long ZipStream_Tell(ZZIP_FILE* zzipFile)
{
	return zzip_tell(zzipFile);
}