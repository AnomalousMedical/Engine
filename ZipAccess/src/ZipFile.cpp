#include "StdAfx.h"
#include "..\include\ZipFile.h"
#include "ZipStream.h"
#include "ZipIOException.h"

using namespace System::IO;
using namespace System::Text::RegularExpressions;

namespace ZipAccess
{

#pragma unmanaged

static int xor_value;

static zzip_ssize_t xor_read (int f, void* p, zzip_size_t l)
{
    zzip_ssize_t r = _read(f, p, l);
    zzip_ssize_t x; 
	char* q; 
	for (x=0, q=(char*)p; x < r; x++)
	{
		q[x] ^= 73;
	}
    return r;
}

static zzip_plugin_io_handlers xor_handlers;
static zzip_strings_t xor_fileext[] = { ".dat", ".DAT", 0 }; 

ZZIP_DIR* openDir(const std::string& filename, zzip_error_t * zzipError)
{
	if(filename.find(".dat") != std::string::npos || filename.find(".DAT") != std::string::npos)
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

#pragma managed

ZipFile::ZipFile(String^ filename)
:zzipDir(0),
files(gcnew List<ZipFileInfo^>()),
directories(gcnew List<ZipFileInfo^>()),
file(filename),
fileFilter(nullptr)
{
	commonLoad();
}

ZipFile::ZipFile(String^ filename, String^ fileFilter)
:zzipDir(0),
files(gcnew List<ZipFileInfo^>()),
directories(gcnew List<ZipFileInfo^>()),
file(filename),
fileFilter(fixPathDir(fileFilter))
{
	commonLoad();
}

void ZipFile::commonLoad()
{
    zzip_error_t zzipError;
	std::string mName = convertString(file);
    //zzipDir = zzip_dir_open(mName.c_str(), &zzipError);
	zzipDir = openDir(mName, &zzipError);
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
		throw gcnew ZipIOException("Could not open zip file {0} because of {1}", file, errorMessage);
	}

    //Read the directories and files out of the zip file
    ZZIP_DIRENT zzipEntry;
    while (zzip_dir_read(zzipDir, &zzipEntry))
    {
		String^ entryName = convertString(zzipEntry.d_name);
		if(fileFilter == nullptr || entryName->StartsWith(fileFilter))
		{
			ZipFileInfo^ fileInfo = gcnew ZipFileInfo(entryName, zzipEntry.d_csize, zzipEntry.st_size);
			//Make sure we don't end with a /
			if(fileInfo->IsDirectory)
			{
				directories->Add(fileInfo);
			}
			else
			{
				files->Add(fileInfo);
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
	std::string cFile = convertString(fixPathFile(filename));
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

List<ZipFileInfo^>^ ZipFile::listFiles(String^ path, bool recursive)
{
	return findMatches(files, path, "*", recursive);
}

List<ZipFileInfo^>^ ZipFile::listFiles(String^ path, String^ searchPattern, bool recursive)
{
	return findMatches(files, path, searchPattern, recursive);
}

List<ZipFileInfo^>^ ZipFile::listDirectories(String^ path, bool recursive)
{
	return findMatches(directories, path, "*", recursive);
}

List<ZipFileInfo^>^ ZipFile::listDirectories(String^ path, String^ searchPattern, bool recursive)
{
	return findMatches(directories, path, searchPattern, recursive);
}

bool ZipFile::exists(String^ filename)
{
	std::string cFile = convertString(fixPathFile(filename));
	ZZIP_STAT zstat;
	int res = zzip_dir_stat(zzipDir, cFile.c_str(), &zstat, ZZIP_CASEINSENSITIVE);
	return (res == ZZIP_NO_ERROR);
}

ZipFileInfo^ ZipFile::getFileInfo(String^ filename)
{
	String^ fixedFileName = fixPathFile(filename);
	for each(ZipFileInfo^ file in files)
	{
		if(file->FullName == fixedFileName)
		{
			return file;
		}
	}
	fixedFileName = fixPathDir(filename);
	for each(ZipFileInfo^ file in directories)
	{
		if(file->FullName == fixedFileName)
		{
			return file;
		}
	}
	return nullptr;
}

List<ZipFileInfo^>^ ZipFile::findMatches(List<ZipFileInfo^>^ sourceList, String^ path, String^ searchPattern, bool recursive)
{
	bool matchAll = searchPattern == "*";
	searchPattern = wildcardToRegex(searchPattern);
	Regex^ r = gcnew Regex(searchPattern);
	bool matchBaseDir = (path == "" || path == "/");
	if(!matchBaseDir)
	{
		path = fixPathDir(path);
	}

	List<ZipFileInfo^>^ files = gcnew List<ZipFileInfo^>();
	//recursive
	if(recursive)
	{
		//looking in root folder
		if(matchBaseDir)
		{
			for each(ZipFileInfo^ file in sourceList)
			{
				if(matchAll || r->Match(file->FullName)->Success)
				{
					files->Add(file);
				}
			}
		}
		//looking in a specific folder
		else
		{
			for each(ZipFileInfo^ file in sourceList)
			{
				Match^ match = r->Match(file->FullName);
				if(file->FullName->Contains(path) && (matchAll || match->Success))
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
			for each(ZipFileInfo^ file in sourceList)
			{
				if(!file->FullName->Contains("/") && (matchAll || r->Match(file->FullName)->Success))
				{
					files->Add(file);
				}
			}
		}
		//looking in specific folder
		else
		{
			for each(ZipFileInfo^ file in sourceList)
			{
				if(file->FullName->Contains(path) && (matchAll || r->Match(file->FullName)->Success))
				{
					String^ cut = file->FullName->Replace(path, "");
					if(cut->EndsWith("/"))
					{
						cut = cut->Substring(0, cut->Length - 1);
					}
					if(cut != String::Empty && !cut->Contains("/"))
					{
						files->Add(file);
					}
				}
			}
		}
	}
	return files;
}

String^ ZipFile::wildcardToRegex(String^ wildcard)
{
	String^ expression = "^";
	for(int i = 0; i < wildcard->Length; i++)
	{
		switch(wildcard[i])
		{
			case '?':
				expression += '.'; // regular expression notation.
				//mIsWild = true;
				break;
			case '*':
				expression += ".*";
				//mIsWild = true;
				break;
			case '.':
				expression += "\\";
				expression += wildcard[i];
				break;
			case ';':
				expression += "|";
				//mIsWild = true;
				break;
			default:
				expression += wildcard[i];
				break;
		}
	}
	return expression;
}

String^ ZipFile::fixPathFile(String^ path)
{
	//Fix up any ../ sections to point to the upper directory.
	cli::array<System::String^>^ splitPath = path->Split(SEPS, StringSplitOptions::RemoveEmptyEntries);
	int lenMinusOne = splitPath->Length - 1;
	System::Text::StringBuilder pathString(path->Length);
	for(int i = 0; i < lenMinusOne; ++i)
	{
		if(splitPath[i+1] != "..")
		{
			pathString.Append(splitPath[i]);
			pathString.Append("/");
		}
		else
		{
			++i;
		}
	}
	if(splitPath[lenMinusOne] != "..")
	{
		pathString.Append(splitPath[lenMinusOne]);
	}
	return pathString.ToString();
}

String^ ZipFile::fixPathDir(String^ path)
{
	path = fixPathFile(path);
	path += "/";
	return path;
}


}