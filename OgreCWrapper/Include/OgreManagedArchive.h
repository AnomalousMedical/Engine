#pragma once

typedef void (*LoadDelegate)();
typedef void (*UnloadDelegate)();
typedef Ogre::DataStream* (*OpenDelegate)(String filename, bool readOnly);
typedef Ogre::StringVector* (*ListDelegate)(bool recursive, bool dirs);
typedef Ogre::FileInfoList* (*ListFileInfoDelegate)(bool recursive, bool dirs);
typedef Ogre::StringVector* (*FindDelegate)(String pattern, bool recursive, bool dirs);
typedef Ogre::FileInfoList* (*FindFileInfoDelegate)(String pattern, bool recursive, bool dirs);
typedef bool (*ExistsDelegate)(String filename);

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

public:
	OgreManagedArchive(String name, String archType, LoadDelegate loadCallback, UnloadDelegate unloadCallback, OpenDelegate openCallback, ListDelegate listCallback, ListFileInfoDelegate listFileInfoCallback, FindDelegate findCallback, FindFileInfoDelegate findFileInfoCallback, ExistsDelegate existsCallback);

	virtual ~OgreManagedArchive(void);

	bool isCaseSensitive(void) const { return true; }

	void load();

	void unload();

	Ogre::DataStreamPtr open(const Ogre::String& filename, bool readOnly = true) const;

	Ogre::StringVectorPtr list(bool recursive = true, bool dirs = false);

	Ogre::FileInfoListPtr listFileInfo(bool recursive = true, bool dirs = false);

	Ogre::StringVectorPtr find(const Ogre::String& pattern, bool recursive = true, bool dirs = false);

	Ogre::FileInfoListPtr findFileInfo(const Ogre::String& pattern, bool recursive = true, bool dirs = false) const;

	bool exists(const Ogre::String& filename);

	time_t getModifiedTime(const Ogre::String& filename) { return 0; }
};
