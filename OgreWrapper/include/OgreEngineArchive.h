#pragma once

#include "OgreArchive.h"
#include <vcclr.h>

using namespace System;
using namespace Engine::Resources;

namespace OgreWrapper
{

class OgreEngineArchive : public Ogre::Archive 
{
private:
	gcroot<Engine::VirtualFileSystem^> archive;
	gcroot<String^> baseName;

	Ogre::FileInfoList mFileList;

public:
    OgreEngineArchive(const Ogre::String& name, const Ogre::String& archType);

    ~OgreEngineArchive();

    bool isCaseSensitive(void) const { return true; }

    void load();

    void unload();

    Ogre::DataStreamPtr open(const Ogre::String& filename, bool readOnly) const;

    Ogre::StringVectorPtr list(bool recursive = true, bool dirs = false);

    Ogre::FileInfoListPtr listFileInfo(bool recursive = true, bool dirs = false);

    Ogre::StringVectorPtr find(const Ogre::String& pattern, bool recursive = true, bool dirs = false);

    Ogre::FileInfoListPtr findFileInfo(const Ogre::String& pattern, bool recursive = true, bool dirs = false);

    bool exists(const Ogre::String& filename);

	time_t getModifiedTime(const Ogre::String& filename);
};

}