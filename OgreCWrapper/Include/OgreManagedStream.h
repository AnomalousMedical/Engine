#pragma once

typedef size_t (*ReadDelegate)(void* buf, size_t count);
typedef void (*SkipDelegate)(size_t count);
typedef void (*SeekDelegate)(size_t pos);
typedef size_t (*TellDelegate)();
typedef bool (*EofDelegate)();
typedef void (*CloseDelegate)();
typedef void (*DeletedDelegate)();

class OgreManagedStream : public Ogre::DataStream
{
private:
	ReadDelegate readCb;
	SkipDelegate skipCb;
	SeekDelegate seekCb;
	TellDelegate tellCb;
	EofDelegate eofCb;
	CloseDelegate closeCb;
	DeletedDelegate deletedCb;

public:
	OgreManagedStream(String name, ReadDelegate read, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted);

	virtual ~OgreManagedStream();

	size_t read(void* buf, size_t count)
	{
		return readCb(buf, count);
	}

	void skip(long count)
	{
		skipCb(count);
	}

	void seek( size_t pos )
	{
		seekCb(pos);
	}

	size_t tell(void) const
	{
		return tellCb();
	}

	bool eof(void) const
	{
		return eofCb();
	}

	void close(void)
	{
		closeCb();
	}
};
