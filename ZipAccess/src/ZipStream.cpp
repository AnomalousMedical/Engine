#include "StdAfx.h"
#include "..\include\ZipStream.h"

namespace ZipAccess
{

ZipStream::ZipStream(ZZIP_FILE* zzipFile, size_t uncompressedSize)
:zzipFile(zzipFile),
uncompressedSize(uncompressedSize)
{

}

ZipStream::~ZipStream(void)
{
	if(zzipFile)
	{
		Close();
	}
}

void ZipStream::Close()
{
	if(zzipFile)
	{
		zzip_file_close(zzipFile);
		zzipFile = 0;
	}
}

void ZipStream::Flush()
{

} 

__int64 ZipStream::Seek(__int64 offset, SeekOrigin origin)
{
	int whence;
	switch(origin)
	{
		case SeekOrigin::Begin:
			whence = SEEK_SET;
			break;
		case SeekOrigin::Current:
			whence = SEEK_CUR;
			break;
		case SeekOrigin::End:
			whence = SEEK_END;
			break;
	}
	return static_cast<__int64>(zzip_seek(zzipFile, static_cast<zzip_off_t>(offset), whence));
} 

void ZipStream::SetLength(__int64 value)
{

} 

int ZipStream::Read(cli::array<unsigned char, 1>^ buffer, int offset, int count)
{
	pin_ptr<unsigned char> pin = &buffer[offset];
	void* buf = pin;
	return zzip_file_read(zzipFile, buf, count);
} 

void ZipStream::Write(cli::array<unsigned char, 1>^ buffer, int offset, int count)
{

} 

bool ZipStream::CanRead::get()
{
	return (zzip_tell(zzipFile) < static_cast<zzip_off_t>(uncompressedSize));
} 

bool ZipStream::CanSeek::get()
{
	return (zzip_tell(zzipFile) < static_cast<zzip_off_t>(uncompressedSize));
} 

bool ZipStream::CanWrite::get()
{
	return false;
	//return (zzip_tell(zzipFile) < static_cast<zzip_off_t>(uncompressedSize));
} 

__int64 ZipStream::Length::get()
{
	return uncompressedSize;
} 

__int64 ZipStream::Position::get()
{
	return zzip_tell(zzipFile);
} 

void ZipStream::Position::set(__int64 value)
{
	static_cast<__int64>(zzip_seek(zzipFile, static_cast<zzip_off_t>(value), SEEK_SET));
} 

}