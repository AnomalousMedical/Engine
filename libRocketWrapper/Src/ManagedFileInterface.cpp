#include "StdAfx.h"

class ManagedFileInterface : public Rocket::Core::FileInterface
{
public:
	typedef size_t (*OpenCb)(String path HANDLE_ARG);
	typedef void(*CloseCb)(Rocket::Core::FileHandle file HANDLE_ARG);
	typedef size_t(*ReadCb)(void* buffer, size_t size, Rocket::Core::FileHandle file HANDLE_ARG);
	typedef bool(*SeekCb)(Rocket::Core::FileHandle file, long offset, int origin HANDLE_ARG);
	typedef size_t(*TellCb)(Rocket::Core::FileHandle file HANDLE_ARG);
	typedef void (*ReleaseCb)(HANDLE_FIRST_ARG);

	ManagedFileInterface(OpenCb open, CloseCb close, ReadCb read, SeekCb seek, TellCb tell, ReleaseCb release HANDLE_ARG)
		:open(open),
		close(close),
		read(read),
		seek(seek),
		tell(tell),
		release(release)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedFileInterface()
	{

	}

	virtual Rocket::Core::FileHandle Open(const Rocket::Core::String& path)
	{
		return open(path.CString() PASS_HANDLE_ARG);
	}

	virtual void Close(Rocket::Core::FileHandle file)
	{
		close(file PASS_HANDLE_ARG);
	}

	virtual size_t Read(void* buffer, size_t size, Rocket::Core::FileHandle file)
	{
		return read(buffer, size, file PASS_HANDLE_ARG);
	}

	virtual bool Seek(Rocket::Core::FileHandle file, long offset, int origin)
	{
		return seek(file, offset, origin PASS_HANDLE_ARG);
	}

	virtual size_t Tell(Rocket::Core::FileHandle file)
	{
		return tell(file PASS_HANDLE_ARG);
	}

	virtual void Release()
	{
		release(PASS_HANDLE);
	}

private:
	OpenCb open;
	CloseCb close;
	ReadCb read;
	SeekCb seek;
	TellCb tell;
	ReleaseCb release;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedFileInterface* ManagedFileInterface_Create(ManagedFileInterface::OpenCb open, ManagedFileInterface::CloseCb close, ManagedFileInterface::ReadCb read, ManagedFileInterface::SeekCb seek, ManagedFileInterface::TellCb tell, ManagedFileInterface::ReleaseCb release HANDLE_ARG)
{
	return new ManagedFileInterface(open, close, read, seek, tell, release PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedFileInterface_Delete(ManagedFileInterface* fileInterface)
{
	delete fileInterface;
}