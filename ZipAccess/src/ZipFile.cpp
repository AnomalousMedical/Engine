#include "StdAfx.h"
#include "..\include\ZipFile.h"
#include "ZipStream.h"
#include "ZipIOException.h"

using namespace System::IO;
using namespace System::Text::RegularExpressions;

namespace ZipAccess
{

ZipFile::ZipFile(String^ filename)
:zzipDir(0),
files(gcnew List<String^>())
{
	file = filename;
	if (!zzipDir)
    {
        zzip_error_t zzipError;
		std::string mName = convertString(filename);
        zzipDir = zzip_dir_open(mName.c_str(), &zzipError);
		if(zzipError != ZZIP_NO_ERROR)
		{
			System::String^ errorMessage;
			switch (zzipError)
			{
				case ZZIP_OUTOFMEM:
					errorMessage = "Out of memory";
					break;            
				case ZZIP_DIR_OPEN:
				case ZZIP_DIR_STAT: 
				case ZZIP_DIR_SEEK:
				case ZZIP_DIR_READ:
					errorMessage = "Unable to read zip file";
					break;           
				case ZZIP_UNSUPP_COMPR:
					errorMessage = "Unsupported compression format";
					break;            
				case ZZIP_CORRUPTED:
					errorMessage = "Archive corrupted";
					break;            
				default:
					errorMessage = "Unknown ZZIP error number";
					break;            
			};
			throw gcnew ZipIOException("Could not open zip file {0} because of {1}", filename, errorMessage);
		}

        //Read the directories and files out of the zip file
        ZZIP_DIRENT zzipEntry;
        while (zzip_dir_read(zzipDir, &zzipEntry))
        {
			String^ entryName = convertString(zzipEntry.d_name);
			//Make sure we don't end with a /
			if(!entryName->EndsWith("/"))
			{
				files->Add(entryName);
			}
			else
			{
				directories->Add(entryName);
			}
		}
    }
}

ZipFile::~ZipFile(void)
{
	if(zzipDir)
	{
		zzip_dir_close(zzipDir);
        zzipDir = 0;
	}
}

ZipStream^ ZipFile::openFile(String^ filename)
{
	std::string cFile = convertString(filename);
	//Get uncompressed size
	ZZIP_STAT zstat;
	zzip_dir_stat(zzipDir, cFile.c_str(), &zstat, ZZIP_CASEINSENSITIVE);

	//Open file
	ZZIP_FILE* zzipFile = zzip_file_open(zzipDir, cFile.c_str(), ZZIP_ONLYZIP | ZZIP_CASELESS);
	if(zzipFile != 0)
	{
		return gcnew ZipStream(zzipFile, static_cast<size_t>(zstat.st_size));
	}
	else
	{
		return nullptr;
	}
}

List<String^>^ ZipFile::listFiles(String^ path, bool recursive)
{
	return findMatches(files, path, "*", recursive);
}

List<String^>^ ZipFile::listFiles(String^ path, String^ searchPattern, bool recursive)
{
	return findMatches(files, path, searchPattern, recursive);
}

List<String^>^ ZipFile::listDirectories(String^ path, bool recursive)
{
	return findMatches(directories, path, "*", recursive);
}

List<String^>^ ZipFile::listDirectories(String^ path, String^ searchPattern, bool recursive)
{
	return findMatches(directories, path, searchPattern, recursive);
}

bool ZipFile::exists(String^ filename)
{
	std::string cFile = convertString(filename);
	ZZIP_STAT zstat;
	int res = zzip_dir_stat(zzipDir, cFile.c_str(), &zstat, ZZIP_CASEINSENSITIVE);
	return (res == ZZIP_NO_ERROR);
}

List<String^>^ ZipFile::findMatches(List<String^>^ sourceList, String^ path, String^ searchPattern, bool recursive)
{
	Regex^ r = gcnew Regex(searchPattern);
	path->Replace('\\', '/');
	bool matchBaseDir = (path == "" || path == "/");
	if(!matchBaseDir && !path->EndsWith("/"))
	{
		path += "/";
	}

	List<String^>^ files = gcnew List<String^>();
	//recursive
	if(recursive)
	{
		//looking in root folder
		if(matchBaseDir)
		{
			for each(String^ file in sourceList)
			{
				if(r->Match(file)->Success)
				{
					files->Add(file);
				}
			}
		}
		//looking in a specific folder
		else
		{
			for each(String^ file in sourceList)
			{
				if(file->Contains(path) && r->Match(file)->Success)
				{
					files->Add(file);
				}
			}
		}
	}
	//Non recursive
	else
	{
		//looking in root folder
		if(matchBaseDir)
		{
			for each(String^ file in sourceList)
			{
				if(!file->Contains("/") && r->Match(file)->Success)
				{
					files->Add(file);
				}
			}
		}
		//looking in specific folder
		else
		{
			for each(String^ file in sourceList)
			{
				if(file->Contains(path) && r->Match(file)->Success)
				{
					String^ cut = file->Replace(path, "");
					if(!cut->Contains("/"))
					{
						files->Add(file);
					}
				}
			}
		}
	}
	return files;
}

}