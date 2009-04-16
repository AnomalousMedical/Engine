/// <file>RaySceneQuery.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\RaySceneQuery.h"
#include "OgreSceneQuery.h"
#include "MovableObject.h"
#include "MathUtils.h"
#include "VoidUserDefinedObject.h"
#include "RaySceneQueryResultEntry.h"
#include "gcroot.h"
#include "Entity.h"

namespace OgreWrapper{

using namespace System;

RaySceneQuery::RaySceneQuery(Ogre::RaySceneQuery* query)
:query(query)
{
	
}

RaySceneQuery::~RaySceneQuery()
{
	
}

void RaySceneQuery::setRay(EngineMath::Ray3 ray)
{
	Ogre::Ray oRay;
	MathUtils::copyRay(ray, oRay);
	query->setRay(oRay);
}

EngineMath::Ray3 RaySceneQuery::getRay()
{
	EngineMath::Ray3 ray;
	Ogre::Ray oRay = query->getRay();
	MathUtils::copyRay(oRay, ray);
	return ray;
}

void RaySceneQuery::setSortByDistance(bool sort)
{
	query->setSortByDistance(sort);
}

bool RaySceneQuery::getSortByDistance()
{
	return query->getSortByDistance();
}

unsigned short RaySceneQuery::getMaxResults()
{
	return query->getMaxResults();
}

System::Collections::Generic::List<RaySceneQueryResultEntry^>^ RaySceneQuery::execute()
{
	System::Collections::Generic::List<RaySceneQueryResultEntry^>^ results = gcnew System::Collections::Generic::List<RaySceneQueryResultEntry^>();
	Ogre::RaySceneQueryResult oResults = query->execute();

	for(Ogre::RaySceneQueryResult::iterator iter = oResults.begin(); iter != oResults.end(); iter++)
	{
		VoidUserDefinedObject* userObj = (VoidUserDefinedObject*)((*iter).movable->getUserObject());
		if(userObj != 0)
		{
			switch(userObj->type)
			{
			case RENDER_ENTITY_GCROOT:
				RenderEntityGCRoot* root = (RenderEntityGCRoot*)userObj->object;
				results->Add(gcnew RaySceneQueryResultEntry((*iter).distance, *root));
				break;
			}
		}
	}

	return results;
}

}