#pragma once

using namespace System;

namespace ZipAccess
{

public ref class ZipFileInfo
{
private:
	String^ name;
	String^ directoryName;
	String^ fullName;
	size_t compressedSize;
	size_t uncompressedSize;
	bool isDirectory;

public:
	ZipFileInfo(String^ fullName, size_t compressedSize, size_t uncompressedSize);

	property String^ Name
	{
		String^ get()
		{
			return name;
		}
	}

	property String^ DirectoryName
	{
		String^ get()
		{
			return directoryName;
		}
	}

	property String^ FullName
	{
		String^ get()
		{
			return fullName;
		}
	}

	property size_t CompressedSize
	{
		size_t get()
		{
			return compressedSize;
		}
	}

	property size_t UncompressedSize
	{
		size_t get()
		{
			return uncompressedSize;
		}
	}

	property bool IsDirectory
	{
		bool get()
		{
			return isDirectory;
		}
	}
};

}