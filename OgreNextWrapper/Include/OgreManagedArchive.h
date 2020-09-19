#pragma once

typedef void(*LoadDelegate)(HANDLE_FIRST_ARG);
typedef void(*UnloadDelegate)(HANDLE_FIRST_ARG);
typedef Ogre::DataStream* (*OpenDelegate)(String filename, bool readOnly HANDLE_ARG);
typedef Ogre::StringVector* (*ListDelegate)(bool recursive, bool dirs HANDLE_ARG);
typedef Ogre::FileInfoList* (*ListFileInfoDelegate)(bool recursive, bool dirs HANDLE_ARG);
typedef Ogre::StringVector* (*FindDelegate)(String pattern, bool recursive, bool dirs HANDLE_ARG);
typedef Ogre::FileInfoList* (*FindFileInfoDelegate)(String pattern, bool recursive, bool dirs HANDLE_ARG);
typedef bool(*ExistsDelegate)(String filename HANDLE_ARG);

class OgreManagedArchive : public Ogre::Archive
{
private:
	LoadDelegate loadCallback;
	UnloadDelegate unloadCallback;
	OpenDelegate openCallback;
	ListDelegate listCallback;
	ListFileInfoDelegate listFileInfoCallback;
	FindDelegate findCallback;
	FindFileInfoDelegate findFileInfoCallback;
	ExistsDelegate existsCallback;
	HANDLE_INSTANCE

public:
	OgreManagedArchive(String name, String archType, LoadDelegate loadCallback, UnloadDelegate unloadCallback, OpenDelegate openCallback, ListDelegate listCallback, ListFileInfoDelegate listFileInfoCallback, FindDelegate findCallback, FindFileInfoDelegate findFileInfoCallback, ExistsDelegate existsCallback HANDLE_ARG);

	virtual ~OgreManagedArchive(void);

	bool isCaseSensitive(void) const { return true; }

	void load();

	void unload();

	Ogre::DataStreamPtr open(const Ogre::String& filename, bool readOnly = true);

	Ogre::StringVectorPtr list(bool recursive = true, bool dirs = false);

	Ogre::FileInfoListPtr listFileInfo(bool recursive = true, bool dirs = false);

	Ogre::StringVectorPtr find(const Ogre::String& pattern, bool recursive = true, bool dirs = false);

	Ogre::FileInfoListPtr findFileInfo(const Ogre::String& pattern, bool recursive = true, bool dirs = false);

	bool exists(const Ogre::String& filename);

	time_t getModifiedTime(const Ogre::String& filename) { return 0; }
};
