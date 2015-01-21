#include "StdAfx.h"
#include "../Include/OgreManagedStream.h"

OgreManagedStream::OgreManagedStream(String name, size_t size, Ogre::DataStream::AccessMode accessMode, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted HANDLE_ARG)
:Ogre::DataStream(name, accessMode),
readCb(read),
writeCb(write),
skipCb(skip),
seekCb(seek),
tellCb(tell),
eofCb(eof),
closeCb(close),
deletedCb(deleted)
ASSIGN_HANDLE_INITIALIZER
{
	mSize = size;
}

OgreManagedStream::~OgreManagedStream(void)
{
	deletedCb();
}

extern "C" _AnomalousExport OgreManagedStream* OgreManagedStream_Create(String name, size_t size, Ogre::DataStream::AccessMode accessMode, ReadDelegate read, WriteDelegate write, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted HANDLE_ARG)
{
	return new OgreManagedStream(name, size, accessMode, read, write, skip, seek, tell, eof, close, deleted PASS_HANDLE_ARG);
}