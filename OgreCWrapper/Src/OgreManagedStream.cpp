#include "StdAfx.h"
#include "../Include/OgreManagedStream.h"

OgreManagedStream::OgreManagedStream(String name, size_t size, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted)
:Ogre::DataStream(name, 3), //temp always reporting read, write
readCb(read),
writeCb(write),
skipCb(skip),
seekCb(seek),
tellCb(tell),
eofCb(eof),
closeCb(close),
deletedCb(deleted)
{
	mSize = size;
}

OgreManagedStream::~OgreManagedStream(void)
{
	deletedCb();
}

extern "C" _AnomalousExport OgreManagedStream* OgreManagedStream_Create(String name, size_t size, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted)
{
	return new OgreManagedStream(name, size, read, write, skip, seek, tell, eof, close, deleted);
}