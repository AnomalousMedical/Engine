#pragma once

typedef size_t(*ReadDelegate)(void* buf, size_t count HANDLE_ARG);
typedef size_t(*WriteDelegate)(const void* buf, size_t count HANDLE_ARG);
typedef void(*SkipDelegate)(size_t count HANDLE_ARG);
typedef void (*SeekDelegate)(size_t pos HANDLE_ARG);
typedef size_t(*TellDelegate)(HANDLE_FIRST_ARG);
typedef bool(*EofDelegate)(HANDLE_FIRST_ARG);
typedef void(*CloseDelegate)(HANDLE_FIRST_ARG);
typedef void(*DeletedDelegate)(HANDLE_FIRST_ARG);

class OgreManagedStream : public Ogre::DataStream
{
private:
	ReadDelegate readCb;
	WriteDelegate writeCb;
	SkipDelegate skipCb;
	SeekDelegate seekCb;
	TellDelegate tellCb;
	EofDelegate eofCb;
	CloseDelegate closeCb;
	DeletedDelegate deletedCb;
	HANDLE_INSTANCE

public:
	OgreManagedStream(String name, size_t size, Ogre::DataStream::AccessMode accessMode, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted HANDLE_ARG);

	virtual ~OgreManagedStream();

	size_t read(void* buf, size_t count)
	{
		return readCb(buf, count PASS_HANDLE_ARG);
	}

	virtual size_t write(const void* buf, size_t count)
    {
		return writeCb(buf, count PASS_HANDLE_ARG);
    }

	void skip(long count)
	{
		skipCb(count PASS_HANDLE_ARG);
	}

	void seek( size_t pos )
	{
		seekCb(pos PASS_HANDLE_ARG);
	}

	size_t tell(void) const
	{
		return tellCb(PASS_HANDLE);
	}

	bool eof(void) const
	{
		return eofCb(PASS_HANDLE);
	}

	void close(void)
	{
		closeCb(PASS_HANDLE);
	}
};
