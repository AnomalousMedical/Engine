#include "StdAfx.h"
#include "..\Include\OgreManagedStream.h"

OgreManagedStream::OgreManagedStream(String name, size_t size, ReadDelegate read, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted)
:Ogre::DataStream(name),
readCb(read),
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

extern "C" __declspec(dllexport) OgreManagedStream* OgreManagedStream_Create(String name, size_t size, ReadDelegate read, SkipDelegate skip, SeekDelegate seek, TellDelegate tell, EofDelegate eof, CloseDelegate close, DeletedDelegate deleted)
{
	return new OgreManagedStream(name, size, read, skip, seek, tell, eof, close, deleted);
}