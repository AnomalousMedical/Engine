#pragma once

namespace Ogre
{
	class Renderable;
}

namespace OgreWrapper
{

/// <summary>
/// 
/// </summary>
public ref class Renderable abstract
{
private:
	Ogre::Renderable* renderable;

internal:
	/// <summary>
	/// Returns the native Renderable
	/// </summary>
	Ogre::Renderable* getRenderable();

	/// <summary>
	/// Constructor
	/// </summary>
	Renderable(Ogre::Renderable* renderable);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~Renderable();
};

}
