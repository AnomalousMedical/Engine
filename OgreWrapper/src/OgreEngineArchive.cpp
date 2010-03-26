#include "StdAfx.h"
#include "..\include\OgreEngineArchive.h"
#include "MarshalUtils.h"
#include "OgreManagedStream.h"

namespace OgreWrapper
{

OgreEngineArchive::OgreEngineArchive(const Ogre::String& name, const Ogre::String& archType)
:Ogre::Archive(name, archType),
archive(nullptr),
baseName(MarshalUtils::convertString(name))
{

}

OgreEngineArchive::~OgreEngineArchive()
{
	unload();
}

void OgreEngineArchive::load()
{
	archive = Engine::VirtualFileSystem::Instance;
}

void OgreEngineArchive::unload()
{
	archive = nullptr;
}

Ogre::DataStreamPtr OgreEngineArchive::open(const Ogre::String& filename, bool readOnly) const
{
	System::String^ file = MarshalUtils::convertString(filename);
	if(!archive->exists(file))
	{
		file = baseName + "/" + file;
	}
	return Ogre::DataStreamPtr(OGRE_NEW OgreManagedStream(filename, archive->openStream(file, Engine::Resources::FileMode::Open, Engine::Resources::FileAccess::Read)));
}

Ogre::StringVectorPtr OgreEngineArchive::list(bool recursive, bool dirs)
{
	Ogre::StringVectorPtr ret = Ogre::StringVectorPtr(OGRE_NEW_T(Ogre::StringVector, Ogre::MEMCATEGORY_GENERAL)(), Ogre::SPFM_DELETE_T);
	cli::array<String^>^ files;
	if(dirs)
	{
		files = archive->listDirectories(baseName, recursive);
	}
	else
	{
		files = archive->listFiles(baseName, recursive);
	}
	for each(String^ file in files)
	{
		ret->push_back(MarshalUtils::convertString(file));
	}
	return ret;
}

Ogre::FileInfoListPtr OgreEngineArchive::listFileInfo(bool recursive, bool dirs)
{
	Ogre::FileInfoList* fil = OGRE_NEW_T(Ogre::FileInfoList, Ogre::MEMCATEGORY_GENERAL)();
	cli::array<String^>^ files;
	if(dirs)
	{
		files = archive->listDirectories(baseName, recursive);
		for each(String^ file in files)
		{
			Ogre::FileInfo ogreInfo;
			ogreInfo.archive = this;
			ogreInfo.compressedSize = -1;
			ogreInfo.uncompressedSize = -1;
			ogreInfo.basename = MarshalUtils::convertString(System::IO::Path::GetFileName(file));
			ogreInfo.filename = MarshalUtils::convertString(file);
			ogreInfo.path = MarshalUtils::convertString(System::IO::Path::GetDirectoryName(file));
			fil->push_back(ogreInfo);
		}
	}
	else
	{
		files = archive->listFiles(baseName, recursive);
		for each(String^ file in files)
		{
			Engine::VirtualFileInfo^ archiveInfo = archive->getFileInfo(file);
			Ogre::FileInfo ogreInfo;
			ogreInfo.archive = this;
			ogreInfo.compressedSize = archiveInfo->CompressedSize;
			ogreInfo.uncompressedSize = archiveInfo->UncompressedSize;
			ogreInfo.basename = MarshalUtils::convertString(archiveInfo->Name);
			ogreInfo.filename = MarshalUtils::convertString(archiveInfo->FullName);
			ogreInfo.path = MarshalUtils::convertString(archiveInfo->DirectoryName);
			fil->push_back(ogreInfo);
		}
	}
	return Ogre::FileInfoListPtr(fil, Ogre::SPFM_DELETE_T);
}

Ogre::StringVectorPtr OgreEngineArchive::find(const Ogre::String& pattern, bool recursive, bool dirs)
{
	Ogre::StringVectorPtr ret = Ogre::StringVectorPtr(OGRE_NEW_T(Ogre::StringVector, Ogre::MEMCATEGORY_GENERAL)(), Ogre::SPFM_DELETE_T);
	cli::array<String^>^ files;
	if(dirs)
	{
		files = archive->listDirectories(baseName, MarshalUtils::convertString(pattern), recursive);
	}
	else
	{
		files = archive->listFiles(baseName, MarshalUtils::convertString(pattern), recursive);
	}
	for each(String^ file in files)
	{
		ret->push_back(MarshalUtils::convertString(file));
	}
	return ret;
}

Ogre::FileInfoListPtr OgreEngineArchive::findFileInfo(const Ogre::String& pattern, bool recursive, bool dirs)
{
	Ogre::FileInfoList* fil = OGRE_NEW_T(Ogre::FileInfoList, Ogre::MEMCATEGORY_GENERAL)();
	cli::array<String^>^ files;
	if(dirs)
	{
		files = archive->listDirectories(baseName, MarshalUtils::convertString(pattern), recursive);
		for each(String^ file in files)
		{
			Ogre::FileInfo ogreInfo;
			ogreInfo.archive = this;
			ogreInfo.compressedSize = -1;
			ogreInfo.uncompressedSize = -1;
			ogreInfo.basename = MarshalUtils::convertString(System::IO::Path::GetFileName(file));
			ogreInfo.filename = MarshalUtils::convertString(file);
			ogreInfo.path = MarshalUtils::convertString(System::IO::Path::GetDirectoryName(file));
			fil->push_back(ogreInfo);
		}
	}
	else
	{
		files = archive->listFiles(baseName, MarshalUtils::convertString(pattern), recursive);
		for each(String^ file in files)
		{
			Engine::VirtualFileInfo^ archiveInfo = archive->getFileInfo(file);
			Ogre::FileInfo ogreInfo;
			ogreInfo.archive = this;
			ogreInfo.compressedSize = archiveInfo->CompressedSize;
			ogreInfo.uncompressedSize = archiveInfo->UncompressedSize;
			ogreInfo.basename = MarshalUtils::convertString(archiveInfo->Name);
			ogreInfo.filename = MarshalUtils::convertString(archiveInfo->FullName);
			ogreInfo.path = MarshalUtils::convertString(archiveInfo->DirectoryName);
			fil->push_back(ogreInfo);
		}
	}
	return Ogre::FileInfoListPtr(fil, Ogre::SPFM_DELETE_T);
}

bool OgreEngineArchive::exists(const Ogre::String& filename)
{
	return archive->exists(baseName + "/" + MarshalUtils::convertString(filename));
}

time_t OgreEngineArchive::getModifiedTime(const Ogre::String& filename)
{
	return 0;
}

}