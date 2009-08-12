#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace ZipAccess
{

ref class ZipStream;

public ref class ZipFile
{
private:
	ZZIP_DIR* zzipDir;

public:
	ZipFile(void);

	virtual ~ZipFile(void);

	void open(String^ filename);

	void close();

	ZipStream^ openFile(String^ filename);
};

}