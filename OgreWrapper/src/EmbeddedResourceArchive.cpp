#include "StdAfx.h"
#include "..\include\EmbeddedResourceArchive.h"
#include "MarshalUtils.h"
#include "OgreManagedStream.h"

namespace OgreWrapper
{

EmbeddedResourceArchive::EmbeddedResourceArchive(const Ogre::String& name, const Ogre::String& archType )
:Ogre::Archive(name, archType)
{

}

EmbeddedResourceArchive::~EmbeddedResourceArchive()
{

}

void EmbeddedResourceArchive::load()
{
	assembly = Assembly::GetAssembly(Type::GetType(MarshalUtils::convertString(mName)));
	cli::array<System::String^>^ fileList = assembly->GetManifestResourceNames();
	for each(System::String^ file in fileList)
	{
		Ogre::FileInfo ogreInfo;
		ogreInfo.archive = this;
		Stream^ stream = assembly->GetManifestResourceStream(file);
		ogreInfo.compressedSize = stream->Length;
		ogreInfo.uncompressedSize = stream->Length;
		stream->Close();
		ogreInfo.basename = MarshalUtils::convertString(System::IO::Path::GetFileName(file));
		ogreInfo.filename = MarshalUtils::convertString(file);
		ogreInfo.path = MarshalUtils::convertString(System::IO::Path::GetDirectoryName(file));
		mFileList.push_back(ogreInfo);
	}
}

void EmbeddedResourceArchive::unload()
{
	assembly = nullptr;
	mFileList.clear();
}

Ogre::DataStreamPtr EmbeddedResourceArchive::open(const Ogre::String& filename) const
{
	return Ogre::DataStreamPtr(OGRE_NEW OgreManagedStream(filename, assembly->GetManifestResourceStream(MarshalUtils::convertString(filename))));
}

Ogre::StringVectorPtr EmbeddedResourceArchive::list(bool recursive, bool dirs)
{
	Ogre::StringVectorPtr ret = Ogre::StringVectorPtr(OGRE_NEW_T(Ogre::StringVector, Ogre::MEMCATEGORY_GENERAL)(), Ogre::SPFM_DELETE_T);

    Ogre::FileInfoList::iterator i, iend;
    iend = mFileList.end();
    for (i = mFileList.begin(); i != iend; ++i)
	{
        if ((dirs == (i->compressedSize == size_t (-1))) && (recursive || i->path.empty()))
		{
            ret->push_back(i->filename);
		}
	}

    return ret;
}

Ogre::FileInfoListPtr EmbeddedResourceArchive::listFileInfo(bool recursive, bool dirs)
{
	Ogre::FileInfoList* fil = OGRE_NEW_T(Ogre::FileInfoList, Ogre::MEMCATEGORY_GENERAL)();

    Ogre::FileInfoList::const_iterator i, iend;
    iend = mFileList.end();
    for (i = mFileList.begin(); i != iend; ++i)
	{
        if ((dirs == (i->compressedSize == size_t (-1))) && (recursive || i->path.empty()))
		{
            fil->push_back(*i);
		}
	}
    return Ogre::FileInfoListPtr(fil, Ogre::SPFM_DELETE_T);
}

Ogre::StringVectorPtr EmbeddedResourceArchive::find(const Ogre::String& pattern, bool recursive, bool dirs)
{
	Ogre::StringVectorPtr ret = Ogre::StringVectorPtr(OGRE_NEW_T(Ogre::StringVector, Ogre::MEMCATEGORY_GENERAL)(), Ogre::SPFM_DELETE_T);
    // If pattern contains a directory name, do a full match
	bool full_match = (pattern.find ('/') != Ogre::String::npos) || (pattern.find ('\\') != Ogre::String::npos);

    Ogre::FileInfoList::iterator i, iend;
    iend = mFileList.end();
    for (i = mFileList.begin(); i != iend; ++i)
	{
        if ((dirs == (i->compressedSize == size_t (-1))) && (recursive || full_match || i->path.empty()))
		{
            // Check basename matches pattern (zip is case insensitive)
			if (Ogre::StringUtil::match(full_match ? i->filename : i->basename, pattern, false))
			{
                ret->push_back(i->filename);
			}
		}
	}

    return ret;
}

Ogre::FileInfoListPtr EmbeddedResourceArchive::findFileInfo(const Ogre::String& pattern, bool recursive, bool dirs)
{
	Ogre::FileInfoListPtr ret = Ogre::FileInfoListPtr(OGRE_NEW_T(Ogre::FileInfoList, Ogre::MEMCATEGORY_GENERAL)(), Ogre::SPFM_DELETE_T);
    // If pattern contains a directory name, do a full match
    bool full_match = (pattern.find ('/') != Ogre::String::npos) || (pattern.find ('\\') != Ogre::String::npos);

    Ogre::FileInfoList::iterator i, iend;
    iend = mFileList.end();
    for (i = mFileList.begin(); i != iend; ++i)
	{
        if ((dirs == (i->compressedSize == size_t (-1))) && (recursive || full_match || i->path.empty()))
		{
            // Check name matches pattern (zip is case insensitive)
            if (Ogre::StringUtil::match(full_match ? i->filename : i->basename, pattern, false))
			{
                ret->push_back(*i);
			}
		}
	}

    return ret;
}

bool EmbeddedResourceArchive::exists(const Ogre::String& filename)
{
	return assembly->GetManifestResourceInfo(MarshalUtils::convertString(filename)) != nullptr;
}

time_t EmbeddedResourceArchive::getModifiedTime(const Ogre::String& filename)
{
	return 0;
}

}