#include "StdAfx.h"
#include "..\include\OgreManagedStream.h"
#include <stdio.h>

namespace OgreWrapper
{
OgreManagedStream::OgreManagedStream(const Ogre::String& name, gcroot<Stream^> stream)
:Ogre::DataStream(name),
stream(stream),
internalBuffer(gcnew cli::array<System::Byte, 1>(ARRAY_SIZE))
{
	mSize = stream->Length;
}
    
OgreManagedStream::~OgreManagedStream()
{
	close();
}

size_t OgreManagedStream::read(void* buf, size_t count)
{
	//Read into the managed array and copy to the unmanaged one
	int readSize = ARRAY_SIZE;
	if(count < ARRAY_SIZE)
	{
		readSize = count;
	}
	cli::array<System::Byte, 1>^ arr = internalBuffer;
	char* ogreBuf = static_cast<char*>(buf);
	size_t amountRead;
	size_t i = 0;
	for(; i < count; i += amountRead)
	{
		amountRead = stream->Read(arr, 0, readSize);
		if(amountRead != 0)
		{
			pin_ptr<unsigned char> pin = &arr[0];
			void* mBuf = pin;
			memcpy(&ogreBuf[i], mBuf, amountRead);
		}
		else
		{
			break;
		}
	}

	return i;
}

void OgreManagedStream::skip(long count)
{
	stream->Seek(count, SeekOrigin::Current);
}

void OgreManagedStream::seek( size_t pos )
{
	stream->Seek(pos, SeekOrigin::Begin);
}

size_t OgreManagedStream::tell(void) const
{
	return stream->Position;
}

bool OgreManagedStream::eof(void) const
{
	return stream->Length == stream->Position;
}

void OgreManagedStream::close(void)
{
	stream->Close();
}

}