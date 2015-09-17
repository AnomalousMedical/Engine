// stdafx.cpp : source file that includes just the standard includes
// Zip.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

//These just have to go somewhere, the whole zip library should probably become EngineNative or something.

extern "C" _AnomalousExport unsigned char* MemoryBlock_AllocateBuffer(int length)
{
	return new unsigned char[length];
}

extern "C" _AnomalousExport void MemoryBlock_DellocateBuffer(unsigned char* buffer)
{
	delete[] buffer;
}