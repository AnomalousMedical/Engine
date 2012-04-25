#include "StdAfx.h"

class ManagedFileInterface : public Rocket::Core::FileInterface
{
public:
	typedef size_t (*OpenCb)(String path);
	typedef void (*CloseCb)(Rocket::Core::FileHandle file);
	typedef size_t (*ReadCb)(void* buffer, size_t size, Rocket::Core::FileHandle file);
	typedef bool (*SeekCb)(Rocket::Core::FileHandle file, long offset, int origin);
	typedef size_t (*TellCb)(Rocket::Core::FileHandle file);
	typedef void (*ReleaseCb)();

	ManagedFileInterface(OpenCb open, CloseCb close, ReadCb read, SeekCb seek, TellCb tell, ReleaseCb release)
		:open(open),
		close(close),
		read(read),
		seek(seek),
		tell(tell),
		release(release)
	{

	}

	virtual ~ManagedFileInterface()
	{

	}

	virtual Rocket::Core::FileHandle Open(const Rocket::Core::String& path)
	{
		return open(path.CString());
	}

	virtual void Close(Rocket::Core::FileHandle file)
	{
		close(file);
	}

	virtual size_t Read(void* buffer, size_t size, Rocket::Core::FileHandle file)
	{
		return read(buffer, size, file);
	}

	virtual bool Seek(Rocket::Core::FileHandle file, long offset, int origin)
	{
		return seek(file, offset, origin);
	}

	virtual size_t Tell(Rocket::Core::FileHandle file)
	{
		return tell(file);
	}

	virtual void Release()
	{
		release();
	}

private:
	OpenCb open;
	CloseCb close;
	ReadCb read;
	SeekCb seek;
	TellCb tell;
	ReleaseCb release;
};

extern "C" _AnomalousExport ManagedFileInterface* ManagedFileInterface_Create(ManagedFileInterface::OpenCb open, ManagedFileInterface::CloseCb close, ManagedFileInterface::ReadCb read, ManagedFileInterface::SeekCb seek, ManagedFileInterface::TellCb tell, ManagedFileInterface::ReleaseCb release)
{
	return new ManagedFileInterface(open, close, read, seek, tell, release);
}

extern "C" _AnomalousExport void ManagedFileInterface_Delete(ManagedFileInterface* fileInterface)
{
	delete fileInterface;
}