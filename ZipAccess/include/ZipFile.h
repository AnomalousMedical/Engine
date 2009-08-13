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
	String^ file;
	List<String^>^ files;

public:
	ZipFile(String^ filename);

	virtual ~ZipFile(void);

	ZipStream^ openFile(String^ filename);

	List<String^>^ listFiles(String^ path, bool recursive);
};

}