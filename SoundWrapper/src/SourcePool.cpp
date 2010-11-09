#include "StdAfx.h"
#include "SourcePool.h"
#include "Source.h"

namespace SoundWrapper
{

SourcePool::SourcePool(void)
{
	int error = AL_NO_ERROR;
	ALuint sourceID;
	for(int i = 0; i < 32; ++i)
	{
		alGenSources(1, &sourceID);
		error = alGetError();
		if(error == AL_NO_ERROR)
		{
			sources.push_back(new Source(sourceID));
		}
		else
		{
			break;
		}
	}
}

SourcePool::~SourcePool(void)
{
	for(vector<Source*>::iterator iter = sources.begin(); iter != sources.end(); ++iter)
	{
		delete *iter;
	}
	sources.clear();
}

Source* SourcePool::getPooledSource()
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

}