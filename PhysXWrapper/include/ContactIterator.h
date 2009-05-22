#pragma once

class NxContactStreamIterator;

namespace PhysXWrapper
{

/// <summary>
/// This class is for iterating through packed contact streams.
/// 
/// It is NOT OK to skip any of the iteration calls. For example, you may NOT put a 
/// break or a continue statement in any of the above blocks, short of completely 
/// aborting the iteration and deleting the ContactIterator object.
/// The user should not rely on the exact geometry or positioning of contact points. 
/// The SDK is free to re-organise, merge or move contact points as long as the overall 
/// physical simulation is not affected.
/// </summary>
/// <example>
/// The user code to use this class must match the following:
/// <code>
/// void MyUserContactInfo::onContactNotify(NxContactPair &amp; pair, NxU32 events)
/// {
/// NxContactStreamIterator i(pair.stream);
/// 
/// while(i.goNextPair()) // user can call getNumPairs() here 
///     {
///     while(i.goNextPatch()) // user can also call getShape() and getNumPatches() here
///         {
///         while(i.goNextPoint()) //user can also call getPatchNormal() and getNumPoints() here
///             {
///             //user can also call getPoint() and getSeparation() here
///             }
///         }
///     }
/// }
/// </code>
/// </example>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class ContactIterator
{
private:
	NxContactStreamIterator* contactStreamIterator;

public:
	ContactIterator(void);

	/// <summary>
	/// Sets the stream iterator to wrap.
	/// </summary>
	void setContactStreamIterator( NxContactStreamIterator* contactStreamIterator );

	/// <summary>
	/// Goes on to the next pair, silently skipping invalid pairs.
	/// </summary>
	bool goNextPair(); 
 
	/// <summary>
	/// Goes on to the next patch (contacts with the same normal).
	/// </summary>
	bool goNextPatch(); 
	  
	/// <summary>
	/// Goes on to the next contact point. 
	/// </summary>
	bool goNextPoint(); 
	  
	/// <summary>
	/// Returns the number of pairs in the structure. 
	/// </summary>
	unsigned int getNumPairs(); 
	
    /// <summary>
	/// Retrieves the shapes for the current pair. 
	/// </summary>
	//NOT CURRENTLY SUPPORTED NO MANAGED SHAPE INTERFACES
	//NxShape * getShape(unsigned int shapeIndex);
	  
	/// <summary>
	/// Retrieves the shape flags for the current pair. 
	/// </summary>
	unsigned short getShapeFlags(); 
	  
	/// <summary>
	/// Retrieves the number of patches for the current pair. 
	/// </summary>
	unsigned int getNumPatches(); 
	  
	/// <summary>
	/// Retrieves the number of remaining patches. 
	/// </summary>
	unsigned int getNumPatchesRemaining(); 
	  
	/// <summary>
	/// Retrieves the patch normal. 
	/// </summary>
	void getPatchNormal( Engine::Vector3% normal ); 
	  
	/// <summary>
	/// Retrieves the number of points in the current patch.
	/// </summary>
	unsigned int getNumPoints(); 
	   
	/// <summary>
	/// Retrieves the number of points remaining in the current patch. 
	/// </summary>
	unsigned int getNumPointsRemaining(); 
	  
	/// <summary>
	/// Returns the contact point position. 
	/// </summary>
	void getPoint( Engine::Vector3% point ); 
	  
	/// <summary>
	/// Return the separation for the contact point. 
	/// </summary>
	float getSeparation(); 
	  
	/// <summary>
	/// Retrieves the feature index.
	/// </summary>
	unsigned int getFeatureIndex0(); 
	   
	/// <summary>
	/// Retrieves the feature index. 
	/// </summary>
	unsigned int getFeatureIndex1(); 
	  
	/// <summary>
	/// Retrieves the point normal force.
	/// </summary>
	float getPointNormalForce(); 
};

}