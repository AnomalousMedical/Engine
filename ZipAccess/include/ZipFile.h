#pragma once

#include "ZipFileInfo.h"

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
	String^ fileFilter;
	List<ZipFileInfo^>^ files;
	List<ZipFileInfo^>^ directories;

	List<ZipFileInfo^>^ findMatches(List<ZipFileInfo^>^ sourceList, String^ path, String^ searchPattern, bool recursive);

	String^ wildcardToRegex(String^ wildcard);
	String^ fixPathDir(String^ path);
	String^ fixPathFile(String^ path);

	void commonLoad();

public:
	ZipFile(String^ filename);

	ZipFile(String^ filename, String^ fileFilter);

	virtual ~ZipFile(void);

	ZipStream^ openFile(String^ filename);

	List<ZipFileInfo^>^ listFiles(String^ path, bool recursive);

	List<ZipFileInfo^>^ listFiles(String^ path, String^ searchPattern, bool recursive);

	List<ZipFileInfo^>^ listDirectories(String^ path, bool recursive);

	List<ZipFileInfo^>^ listDirectories(String^ path, String^ searchPattern, bool recursive);

	bool exists(String^ filename);

	ZipFileInfo^ getFileInfo(String^ filename);
};

}