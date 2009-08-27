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
	List<String^>^ directories;

	List<String^>^ findMatches(List<String^>^ sourceList, String^ path, String^ searchPattern, bool recursive);

public:
	ZipFile(String^ filename);

	virtual ~ZipFile(void);

	ZipStream^ openFile(String^ filename);

	List<String^>^ listFiles(String^ path, bool recursive);

	List<String^>^ listFiles(String^ path, String^ searchPattern, bool recursive);

	List<String^>^ listDirectories(String^ path, bool recursive);

	List<String^>^ listDirectories(String^ path, String^ searchPattern, bool recursive);

	bool exists(String^ filename);
};

}