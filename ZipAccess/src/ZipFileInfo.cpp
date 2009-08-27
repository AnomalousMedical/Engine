#include "StdAfx.h"
#include "..\include\ZipFileInfo.h"

namespace ZipAccess
{

ZipFileInfo::ZipFileInfo(String^ fullName, size_t compressedSize, size_t uncompressedSize)
:fullName(fullName),
compressedSize(compressedSize),
uncompressedSize(uncompressedSize),
isDirectory(false)
{
	String^ path = fullName;
    // Replace \ with / first
    path->Replace('\\', '/');

	//If we end with a / then this is a directory
	if(path->EndsWith("/"))
	{
		isDirectory = true;
		path = path->Substring(0, path->Length - 1);
		compressedSize = -1;
		uncompressedSize = -1;
	}

    // split based on final /
	int i = path->LastIndexOf('/');

	if (i == -1)
    {
		directoryName = String::Empty;
		name = fullName;
    }
    else
    {
		name = path->Substring(i+1, path->Length - i - 1);
        directoryName = path->Substring(0, i+1);
    }
}

}