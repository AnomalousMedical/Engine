#include "Stdafx.h"

#include <string>

#if APPLE_IOS
#include <unistd.h>
#endif

static int xor_value;

static zzip_ssize_t xor_read (int f, void* p, zzip_size_t l)
{
#ifdef WINDOWS
	zzip_ssize_t r = _read(f, p, l);
#endif
#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
	zzip_ssize_t r = read(f, p, l);
#endif
	zzip_ssize_t x; 
	char* q; 
	for (x=0, q=(char*)p; x < r; x++)
	{
		q[x] ^= 73;
	}
	return r;
}

static zzip_plugin_io_handlers xor_handlers;
static zzip_strings_t xor_fileext[] = { ".dat", ".DAT", ".obb", ".OBB", 0 }; 

extern "C" _AnomalousExport ZZIP_DIR* ZipFile_OpenDir(const char* cName, zzip_error_t * zzipError)
{
	std::string filename = cName;
	if(filename.find(".dat") != std::string::npos || filename.find(".DAT") != std::string::npos
		|| filename.find(".obb") != std::string::npos || filename.find(".OBB") != std::string::npos)
	{
		zzip_init_io (&xor_handlers, 0); 
		xor_handlers.fd.read = &xor_read;

		return zzip_dir_open_ext_io (filename.c_str(), zzipError, xor_fileext, &xor_handlers);
	}
	else
	{
		return zzip_dir_open(filename.c_str(), zzipError);
	}
}

extern "C" _AnomalousExport void ZipFile_CloseDir(ZZIP_DIR* zzipDir)
{
	zzip_dir_close(zzipDir);
}

extern "C" _AnomalousExport bool ZipFile_Read(ZZIP_DIR* zzipDir, ZZIP_DIRENT* zstat)
{
	return zzip_dir_read(zzipDir, zstat);
}

extern "C" _AnomalousExport int ZipFile_DirStat(ZZIP_DIR* zzipDir, const char* filename, ZZIP_STAT* zstat, int mode)
{
	return zzip_dir_stat(zzipDir, filename, zstat, mode);
}

extern "C" _AnomalousExport ZZIP_FILE* ZipFile_OpenFile(ZZIP_DIR* zzipDir, const char* filename, int mode)
{
	return zzip_file_open(zzipDir, filename, mode);
}