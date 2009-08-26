#pragma once

#include "OgreDataStream.h"
#include "vcclr.h"

using namespace System::IO;

namespace OgreWrapper
{

class OgreManagedStream : public Ogre::DataStream
{
private:
	static const size_t ARRAY_SIZE = 512;

    gcroot<Stream^> stream;
	gcroot<cli::array<System::Byte, 1>^> internalBuffer;

public:
	OgreManagedStream(const Ogre::String& name, gcroot<Stream^> stream);
    
	~OgreManagedStream();
    
	size_t read(void* buf, size_t count);
    
	void skip(long count);
    
	void seek( size_t pos );
    
	size_t tell(void) const;
    
	bool eof(void) const;
    
	void close(void);
};

}