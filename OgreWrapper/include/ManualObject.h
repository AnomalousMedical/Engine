/// <file>ManualObject.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "MovableObject.h"
#include "Enums.h"
#include "gcroot.h"
#include "VoidUserDefinedObject.h"
#include "AutoPtr.h"
#include "ManualObjectSectionCollection.h"

namespace Ogre
{
	class Entity;
}

namespace Engine{

namespace Rendering{

ref class ManualObjectSection;
ref class ManualObject;

typedef gcroot<ManualObject^> ManualObjectRoot;

/// <summary>
/// This class wraps a native manual object.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class ManualObject : MovableObject
{
private:
	Ogre::ManualObject* obj;
	System::String^ name;
	ManualObjectSectionCollection sections;
	AutoPtr<ManualObjectRoot> root;
	AutoPtr<VoidUserDefinedObject> userDefinedObj;

internal:
	/// <summary>
	/// Gets the underlying native entity.
	/// </summary>
	/// <returns>The underlying native entity.</returns>
	Ogre::ManualObject* getManualObject();

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="obj">The manual object to wrap.</param>
	/// <param name="name">The name of the ManualObject.</param>
	ManualObject(Ogre::ManualObject* obj, System::String^ name);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	~ManualObject();

	/// <summary>
	/// Gets the name of the entity.
	/// </summary>
	/// <returns>The name of the entity.</returns>
	virtual System::String^ getName() override;

	/// <summary>
	/// Completely clear the contents of the object. 
	/// </summary>
	void clear();

	/// <summary>
	/// Estimate the number of vertices ahead of time.
	/// </summary>
	/// <param name="count">The number of vertices to predict.</param>
	void estimateVertexCount(unsigned int count);

	/// <summary>
	/// Estimate the number of indices ahead of time. 
	/// </summary>
	/// <param name="count">The number of indicies to predict.</param>
	void estimateIndexCount(unsigned int count);

	/// <summary>
	/// Start defining a part of the object. 
	/// </summary>
	/// <param name="materialName">The name of the material to use.</param>
	/// <param name="opType">The type of object to define.</param>
	void begin(System::String^ materialName, OperationType opType);

	/// <summary>
	/// Use before defining geometry to indicate that you intend to update the geometry 
	/// regularly and want the internal structure to reflect that.
	/// </summary>
	/// <param name="dyn">True to indicate dynamic geometry.</param>
	void setDynamic(bool dyn);

	/// <summary>
	/// Gets whether this object is marked as dynamic. 
	/// </summary>
	/// <returns>True if the object is dynamic.  False if it is not.</returns>
	bool getDynamic();

	/// <summary>
	/// Start the definition of an update to a part of the object. 
	/// </summary>
	/// <param name="sectionIndex">The section to update.</param>
	void beginUpdate(unsigned int sectionIndex);

	/// <summary>
	/// Add a vertex position, starting a new vertex at the same time. 
	/// </summary>
	/// <param name="pos">A vector3 with the position of the vertex.</param>
	void position(EngineMath::Vector3% pos);

	/// <summary>
	/// Add a vertex position, starting a new vertex at the same time.
	/// A vertex position is slightly special among the other vertex data methods like normal() 
	/// and textureCoord(), since calling it indicates the start of a new vertex. All other 
	/// vertex data methods you call after this are assumed to be adding more information 
	/// (like normals or texture coordinates) to the last vertex started with position(). 
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	void position(float x, float y, float z);

	/// <summary>
	/// Add a vertex normal to the current vertex. 
	/// </summary>
	/// <param name="normal">A vector3 with the normal.</param>
	void normal(EngineMath::Vector3% normal);

	/// <summary>
	/// Add a vertex normal to the current vertex. 
	/// Vertex normals are most often used for dynamic lighting, and their
	/// components should be normalised. 
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	void normal(float x, float y, float z);

	/// <summary>
	/// Add a texture coordinate to the current vertex. 
	/// </summary>
	/// <param name="u"></param>
	void textureCoord(float u);

	/// <summary>
	/// Add a texture coordinate to the current vertex.
	/// You can call this method multiple times between position() calls to add multiple texture 
	/// coordinates to a vertex. Each one can have between 1 and 3 dimensions, depending on your 
	// needs, although 2 is most common. There are several versions of this method for the 
	/// variations in number of dimensions.
	/// </summary>
	/// <param name="u"></param>
	/// <param name="v"></param>
	void textureCoord(float u, float v);

	/// <summary>
	/// Add a texture coordinate to the current vertex.
	/// </summary>
	/// <param name="u">u</param>
	/// <param name="v">v</param>
	/// <param name="w">w</param>
	void textureCoord(float u, float v, float w);

	/// <summary>
	/// Add a texture coordinate to the current vertex.
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	/// <param name="w">w</param>
	void textureCoord(float x, float y, float z, float w);

	/// <summary>
	/// Add a texture coordinate to the current vertex.
	/// </summary>
	/// <param name="uvw">A Vector3 with the coord.</param>
	void textureCoord(EngineMath::Vector3% uvw);

	/// <summary>
	/// Add a vertex colour to a vertex. 
	/// </summary>
	/// <param name="r">r</param>
	/// <param name="g">g</param>
	/// <param name="b">b</param>
	/// <param name="a">a</param>
	void color(float r, float g, float b, float a);

	/// <summary>
	/// Add a vertex index to construct faces / lines / points via indexing rather than just by 
	/// a simple list of vertices.  You will have to call this 3 times for each face for a triangle 
	/// list, or use the alternative 3-parameter version. Other operation types require different 
	/// numbers of indexes.
	/// </summary>
	/// <param name="idx">A vertex index from 0 to 4294967295.</param>
	void index(unsigned int idx);

	/// <summary>
	/// Add a set of 3 vertex indices to construct a triangle; this is a shortcut to calling index() 
	/// 3 times. 
	/// </summary>
	/// <param name="i1">First index.</param>
	/// <param name="i2">Second index.</param>
	/// <param name="i3">Third index.</param>
	void triangle(unsigned int i1, unsigned int i2, unsigned int i3);

	/// <summary>
	/// Add a set of 4 vertex indices to construct a quad (out of 2 triangles); this is a shortcut 
	/// to calling index() 6 times, or triangle() twice. 
	/// </summary>
	/// <param name="i1">First index.</param>
	/// <param name="i2">Second index.</param>
	/// <param name="i3">Third index.</param>
	/// <param name="i4">Fourth index.</param>
	void quad(unsigned int i1, unsigned int i2, unsigned int i3, unsigned int i4);

	/// <summary>
	/// Finish defining the object and compile the final renderable version.
	/// </summary>
	ManualObjectSection^ end();

	/// <summary>
	/// Alter the material for a subsection of this object after it has been specified. 
	/// </summary>
	/// <param name="subindex">The index of the subsection to alter.</param>
	/// <param name="name">	The name of the new material to use.</param>
	void setMaterialName(unsigned int subindex, System::String^ name);

	/// <summary>
	/// Sets whether or not to use an 'identity' projection.  Usually ManualObjects will use a 
	/// projection matrix as determined by the active camera. However, if they want they can cancel 
	/// this out and use an identity projection, which effectively projects in 2D using a {-1, 1} 
	/// view space. Useful for overlay rendering. Normally you don't need to change this. The 
	/// default is false.
	/// </summary>
	/// <param name="useIdentityProjection">True to use the identity projection.</param>
	void setUseIdentityProjection(bool useIdentityProjection);

	/// <summary>
	/// Returns whether or not to use an 'identity' projection. 
	/// </summary>
	/// <returns>True if the identity projection is being used.  False if it is not.</returns>
	bool getUseIdentityProjection();

	/// <summary>
	/// Sets whether or not to use an 'identity' view.  Usually ManualObjects will use a view 
	/// matrix as determined by the active camera. However, if they want they can cancel this out 
	/// and use an identity matrix, which means all geometry is assumed to be relative to camera 
	/// space already. Useful for overlay rendering. Normally you don't need to change this. The 
	/// default is false.
	/// </summary>
	/// <param name="useIdentityView">True to use the identity view matrix.</param>
	void setUseIdentityView(bool useIdentityView);

	/// <summary>
	/// Returns whether or not to use an 'identity' view. 
	/// </summary>
	/// <returns>True if the identity view is being used.</returns>
	bool getUseIdentityView();

	/// <summary>
	/// Gets a reference to a ManualObjectSection.
	/// </summary>
	/// <param name="index">The index of the section to get.</param>
	/// <returns>The section at index or null if no section is at that index.</returns>
	ManualObjectSection^ getSection(unsigned int index);

	/// <summary>
	/// Retrieves the number of ManualObjectSection objects making up this ManualObject. 
	/// </summary>
	/// <returns>The number of sections.</returns>
	unsigned int getNumSections();

	/// <summary>
	/// Sets whether or not to keep the original declaration order when queuing the renderables. 
	/// </summary>
	/// <param name="keepOrder">Whether to keep the declaration order or not.  This overrides the 
	/// default behavior of the rendering queue, specifically stating the desired order of 
	/// rendering. Might result in a performance loss, but lets the user to have more direct 
	/// control when creating geometry through this class.</param>
	void setKeepDeclarationOrder(bool keepOrder);

	/// <summary>
	/// Gets whether or not the declaration order is to be kept or not. 
	/// </summary>
	/// <returns>True if the declaration order is being maintained.</returns>
	bool getKeepDeclarationOrder();

	/// <summary>
	/// Retrieves the radius of the origin-centered bounding sphere for this object.
	/// </summary>
	/// <returns>The bounding radius.</returns>
	float getBoundingRadius();
};

}

}
