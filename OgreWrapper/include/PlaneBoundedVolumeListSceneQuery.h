#pragma once

namespace Ogre
{
	class PlaneBoundedVolumeListSceneQuery;
}

namespace Engine
{

namespace Rendering
{

ref class PlaneBoundedVolume;
ref class SceneQueryResult;

typedef System::Collections::Generic::LinkedList<PlaneBoundedVolume^> PlaneBoundedVolumeList;

/// <summary>
/// Specialises the SceneQuery class for querying within a plane-bounded volume. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PlaneBoundedVolumeListSceneQuery
{
private:
	Ogre::PlaneBoundedVolumeListSceneQuery* ogreQuery;
	PlaneBoundedVolumeList^ volumes;
	SceneQueryResult^ lastResults;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreQuery">The ogreQuery to wrap.</param>
	PlaneBoundedVolumeListSceneQuery(Ogre::PlaneBoundedVolumeListSceneQuery* ogreQuery);

	/// <summary>
	/// Get the wrapped query.
	/// </summary>
	/// <returns>The wrapped query.</returns>
	Ogre::PlaneBoundedVolumeListSceneQuery* getQuery();

public:
	/// <summary>
	/// Sets the volume which is to be used for this query. 
	/// </summary>
	/// <param name="volumes"></param>
	void setVolumes(PlaneBoundedVolumeList^ volumes);

	/// <summary>
	/// Gets the volume which is being used for this query. 
	/// </summary>
	/// <returns></returns>
	PlaneBoundedVolumeList^ getVolumes();

	/// <summary>
	/// Executes the query, returning the results back in one list.
	/// 
    /// This method executes the scene query as configured, gathers the results
    /// into one structure and returns a reference to that structure. These
    /// results will also persist in this query object until the next query is
    /// executed, or clearResults() is called. An more lightweight version of
    /// this method that returns results through a listener is also available. 
	/// </summary>
	/// <returns></returns>
	SceneQueryResult^ execute();

	//listener execute

	/// <summary>
	/// Gets the results of the last query that was run using this object,
    /// provided the query was executed using the collection-returning version
    /// of execute.
	/// </summary>
	/// <returns>The last results of the query.</returns>
	SceneQueryResult^ getLastResults();

	/// <summary>
	/// Clears the results of the last query execution.
	/// 
    /// You only need to call this if you specifically want to free up the
    /// memory used by this object to hold the last query results. This object
    /// clears the results itself when executing and when destroying itself. 
	/// </summary>
	void clearResults();

	/// <summary>
	/// Sets the mask for results of this query.
	/// 
    /// This method allows you to set a 'mask' to limit the results of this
    /// query to certain types of result. The actual meaning of this value is up
    /// to the application; basically MovableObject instances will only be
    /// returned from this query if a bitwise AND operation between this mask
    /// value and the MovableObject::getQueryFlags value is non-zero. The
    /// application will have to decide what each of the bits means. 
	/// </summary>
	/// <param name="mask">The mask to set.</param>
	void setQueryMask(unsigned int mask);

	/// <summary>
	/// Returns the current mask for this query. 
	/// </summary>
	/// <returns>The query mask.</returns>
	unsigned int getQueryMask();

	/// <summary>
	/// Sets the type mask for results of this query.
	/// 
	/// This method allows you to set a 'type mask' to limit the results of this
    /// query to certain types of objects. Whilst setQueryMask deals with flags
    /// set per instance of object, this method deals with setting a mask on
    /// flags set per type of object. Both may exclude an object from query
    /// results. 
	/// </summary>
	/// <param name="mask">The mask to set.</param>
	void setQueryTypeMask(unsigned int mask);

	/// <summary>
	/// Returns the current mask for this query. 
	/// </summary>
	/// <returns>The query type mask.</returns>
	unsigned int getQueryTypeMask();

	//get/set world fragment type

	//get supported world framgment types
};

}

}