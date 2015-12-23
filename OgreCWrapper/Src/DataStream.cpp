#include "Stdafx.h"

extern "C" _AnomalousExport bool DataStream_isReadable(Ogre::DataStream* dataStream)
{
	return dataStream->isReadable();
}

extern "C" _AnomalousExport bool DataStream_isWriteable(Ogre::DataStream* dataStream)
{
	return dataStream->isWriteable();
}

extern "C" _AnomalousExport size_t DataStream_tell(Ogre::DataStream* dataStream)
{
	return dataStream->tell();
}

extern "C" _AnomalousExport size_t DataStream_size(Ogre::DataStream* dataStream)
{
	return dataStream->size();
}

extern "C" _AnomalousExport size_t DataStream_read(Ogre::DataStream* dataStream, void* buf, size_t count)
{
	return dataStream->read(buf, count);
}

extern "C" _AnomalousExport size_t DataStream_write(Ogre::DataStream* dataStream, const void* buf, size_t count)
{
	return dataStream->write(buf, count);
}

extern "C" _AnomalousExport void DataStream_seek(Ogre::DataStream* dataStream, size_t pos)
{
	dataStream->seek(pos);
}

//DataStreamPtr
extern "C" _AnomalousExport Ogre::DataStreamPtr* DataStreamPtr_createHeapPtr(Ogre::DataStreamPtr* stackSharedPtr)
{
	return new Ogre::DataStreamPtr(*stackSharedPtr);
}

extern "C" _AnomalousExport void DataStreamPtr_Delete(Ogre::DataStreamPtr* heapSharedPtr)
{
	delete heapSharedPtr;
}