#include "StdAfx.h"
#include "SourceManager.h"
#include "Source.h"

namespace SoundWrapper
{

SourceManager::SourceManager(void)
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
		return returnVal;
	}
	else
	{
		return NULL;
	}
}

void SourceManager::_addPlayingSource(Source* source)
{
	playingSources.push_back(source);
}

void SourceManager::_removePlayingSource(Source* source)
{
	playingSources.remove(source);
}

void SourceManager::_update()
{
	for(list<Source*>::iterator iter = playingSources.begin(); iter != playingSources.end(); ++iter)
	{
		(*iter)->_update();
	}
}

}