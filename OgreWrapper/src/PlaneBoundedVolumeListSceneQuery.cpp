#include "stdafx.h"
#include "PlaneBoundedVolumeListSceneQuery.h"
#include "PlaneBoundedVolume.h"
#include "SceneQueryResult.h"
#include "VoidUserDefinedObject.h"
#include "RenderEntity.h"

namespace Rendering
{

PlaneBoundedVolumeListSceneQuery::PlaneBoundedVolumeListSceneQuery(Ogre::PlaneBoundedVolumeListSceneQuery* ogreQuery)
:ogreQuery(ogreQuery), lastResults(gcnew SceneQueryResult())
{

}

Ogre::PlaneBoundedVolumeListSceneQuery* PlaneBoundedVolumeListSceneQuery::getQuery()
{
	return ogreQuery;
}

void PlaneBoundedVolumeListSceneQuery::setVolumes(PlaneBoundedVolumeList^ volumes)
{
	this->volumes = volumes;
	Ogre::PlaneBoundedVolumeList ogreList;
	for each(PlaneBoundedVolume^ vol in volumes)
	{
		ogreList.push_back(*vol->getVolume());
	}
	ogreQuery->setVolumes(ogreList);
}

PlaneBoundedVolumeList^ PlaneBoundedVolumeListSceneQuery::getVolumes()
{
	return volumes;
}

SceneQueryResult^ PlaneBoundedVolumeListSceneQuery::execute()
{
	Ogre::SceneQueryResult oResults = ogreQuery->execute();
	lastResults->clear();
	for(Ogre::SceneQueryResultMovableList::iterator iter = oResults.movables.begin(); iter != oResults.movables.end(); iter++)
	{
		VoidUserDefinedObject* userObj = (VoidUserDefinedObject*)((*iter)->getUserObject());
		if(userObj != 0)
		{
			switch(userObj->type)
			{
			case RENDER_ENTITY_GCROOT:
				RenderEntityGCRoot* root = (RenderEntityGCRoot*)userObj->object;
				lastResults->movables->AddLast(*root);
				break;
			}
		}
	}

	return lastResults;
}

SceneQueryResult^ PlaneBoundedVolumeListSceneQuery::getLastResults()
{
	return lastResults;
}

void PlaneBoundedVolumeListSceneQuery::clearResults()
{
	lastResults->clear();
	ogreQuery->clearResults();
}

void PlaneBoundedVolumeListSceneQuery::setQueryMask(unsigned int mask)
{
	ogreQuery->setQueryMask(mask);
}

unsigned int PlaneBoundedVolumeListSceneQuery::getQueryMask()
{
	return ogreQuery->getQueryMask();
}

void PlaneBoundedVolumeListSceneQuery::setQueryTypeMask(unsigned int mask)
{
	ogreQuery->setQueryTypeMask(mask);
}

unsigned int PlaneBoundedVolumeListSceneQuery::getQueryTypeMask()
{
	return ogreQuery->getQueryTypeMask();
}

}