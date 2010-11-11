#include "StdAfx.h"
#include "SourceManager.h"
#include "Source.h"

namespace SoundWrapper
{

SourceManager::SourceManager(void)
:inUpdateIterLoop(false)
{
	int error = AL_NO_ERROR;
	ALuint sourceID;
	for(int i = 0; i < 32; ++i)
	{
		alGenSources(1, &sourceID);
		error = alGetError();
		if(error == AL_NO_ERROR)
		{
			sources.push_back(new Source(sourceID, this));
		}
		else
		{
			break;
		}
	}
	log << "Created " << sources.size() << " sources." << info;
}

SourceManager::~SourceManager(void)
{
	for(vector<Source*>::iterator iter = sources.begin(); iter != sources.end(); ++iter)
	{
		delete *iter;
	}
	sources.clear();
}

Source* SourceManager::getPooledSource()
{
	if(sources.size() > 0)
	{
		Source* returnVal = sources.back();
		sources.pop_back();
		log << "Source checked out " << returnVal << debug;
		return returnVal;
	}
	else
	{
		return NULL;
	}
}

void SourceManager::_addPlayingSource(Source* source)
{
	log << "Playing source added " << source << debug;
	if(inUpdateIterLoop)
	{
		addedSources.push_back(source);
	}
	else
	{
		playingSources.push_back(source);
	}
}

void SourceManager::_removePlayingSource(Source* source)
{
	log << "Playing source removed " << source << debug;
	if(inUpdateIterLoop)
	{
		removedSources.push_back(source);
	}
	else
	{
		playingSources.remove(source);
	}
}

void SourceManager::_addSourceToPool(Source* source)
{
	sources.push_back(source);
	log << "Source returned " << source << debug;
}

void SourceManager::_update()
{
	inUpdateIterLoop = true;
	list<Source*>::iterator sourceEnd = playingSources.end();
	for(list<Source*>::iterator iter = playingSources.begin(); iter != sourceEnd; ++iter)
	{
		(*iter)->_update();
	}
	inUpdateIterLoop = false;
	
	//Remove any sources that need to be removed.
	vector<Source*>::iterator removedEnd = removedSources.end();
	for(vector<Source*>::iterator iter = removedSources.begin(); iter != removedEnd; ++iter)
	{
		playingSources.remove(*iter);
	}

	//Add any sources that need to be added.
	vector<Source*>::iterator addedEnd = addedSources.end();
	for(vector<Source*>::iterator iter = addedSources.begin(); iter != addedEnd; ++iter)
	{
		playingSources.push_back(*iter);
	}
}

}