#pragma once

namespace Rendering
{

ref class MovableObject;

typedef System::Collections::Generic::LinkedList<MovableObject^> SceneQueryResultMovableList;

/// <summary>
/// Holds the results of a scene query. 
/// </summary>
public ref class SceneQueryResult
{
public:
	SceneQueryResult();

	/// <summary>
	/// List of movable objects in the query (entities, particle systems etc). 
	/// </summary>
	SceneQueryResultMovableList^ movables;

	void clear();
};

}