#include "StdAfx.h"
#include "Stream.h"

namespace SoundWrapper
{

typedef size_t (*ReadDelegate)(void* buffer, int size, int count HANDLE_ARG);
typedef int(*SeekDelegate)(size_t offset, size_t origin HANDLE_ARG);
typedef void (*CloseDelegate)(HANDLE_FIRST_ARG);
typedef size_t(*TellDelegate)(HANDLE_FIRST_ARG);
typedef bool(*EofDelegate)(HANDLE_FIRST_ARG);
typedef void(*DeleteDelegate)(HANDLE_FIRST_ARG);

class ManagedStream : public Stream
{
private:
	ReadDelegate readCB;
	ReadDelegate writeCB;
	SeekDelegate seekCB;
	CloseDelegate closeCB;
	TellDelegate tellCB;
	EofDelegate eofCB;
	DeleteDelegate deleteCB;
	HANDLE_INSTANCE

public:
	ManagedStream(ReadDelegate readCB, ReadDelegate writeCB, SeekDelegate seekCB, CloseDelegate closeCB, TellDelegate tellCB, EofDelegate eofCB, DeleteDelegate deleteCB HANDLE_ARG)
		:readCB(readCB), writeCB(writeCB), seekCB(seekCB), closeCB(closeCB), tellCB(tellCB), eofCB(eofCB), deleteCB(deleteCB) ASSIGN_HANDLE_INITIALIZER
	{
	}


	virtual ~ManagedStream(void)
	{
		deleteCB(PASS_HANDLE);
	}

	virtual size_t read(void* buffer, int size, int count)
	{
		return readCB(buffer, size, count PASS_HANDLE_ARG);
	}

	virtual size_t write(void* buffer, int size, int count)
	{
		return writeCB(buffer, size, count PASS_HANDLE_ARG);
	}

	virtual int seek(long offset, int origin)
	{
		return seekCB(offset, origin PASS_HANDLE_ARG);
	}

	virtual void close()
	{
		closeCB(PASS_HANDLE);
	}

	virtual size_t tell()
	{
		return tellCB(PASS_HANDLE);
	}

	virtual bool eof()
	{
		return eofCB(PASS_HANDLE);
	}
};

}

//CWrapper

using namespace SoundWrapper;

extern "C" _AnomalousExport ManagedStream* ManagedStream_create(ReadDelegate readCB, ReadDelegate writeCB, SeekDelegate seekCB, CloseDelegate closeCB, TellDelegate tellCB, EofDelegate eofCB, DeleteDelegate deleteCB HANDLE_ARG)
{
	return new ManagedStream(readCB, writeCB, seekCB, closeCB, tellCB, eofCB, deleteCB PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedStream_destroy(ManagedStream* managedStream)
{
	delete managedStream;
}