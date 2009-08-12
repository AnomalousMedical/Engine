#pragma once

using namespace System::IO;

namespace ZipAccess
{

public ref class ZipStream : Stream
{
private:
	ZZIP_FILE* zzipFile;
	size_t uncompressedSize;

public:
	ZipStream(ZZIP_FILE* zzipFile, size_t uncompressedSize);

	~ZipStream(void);

	virtual void Close() override;

	virtual void Flush() override;

	virtual __int64 Seek(__int64 offset, SeekOrigin origin) override;

	virtual void SetLength(__int64 value) override;

	virtual int Read(cli::array<unsigned char, 1>^ buffer, int offset, int count) override;

	virtual void Write(cli::array<unsigned char, 1>^ buffer, int offset, int count) override;

	virtual property bool CanRead
	{
		bool get() override;
	} 

	virtual property bool CanSeek
	{
		bool get() override;
	} 

	virtual property bool CanWrite
	{
		bool get() override;
	} 

	virtual property __int64 Length
	{
		__int64 get() override;
	} 

	virtual property __int64 Position
	{
		__int64 get() override;
		void set(__int64 value) override;
	}
};

}