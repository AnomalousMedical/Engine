/// <file>RaySceneQuery.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

namespace Ogre
{
	class RaySceneQuery;
}

namespace OgreWrapper
{

ref class RaySceneQueryResultEntry;
ref class MovableObject;

/// <summary>
/// This is a scene in the renderer.  You can have multiple scenes at any time.
/// This is where Cameras, RenderEntities, Lights etc are created and added to
/// a scene.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class RaySceneQuery
{
internal:
	RaySceneQuery(Ogre::RaySceneQuery* query);

	Ogre::RaySceneQuery* query;

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~RaySceneQuery();

	void setRay(Engine::Ray3 ray);

	Engine::Ray3 getRay();

	void setSortByDistance(bool sort);

	bool getSortByDistance();

	unsigned short getMaxResults();

	System::Collections::Generic::List<RaySceneQueryResultEntry^>^ execute();	
};

}