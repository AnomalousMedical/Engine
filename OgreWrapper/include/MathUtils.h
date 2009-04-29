#pragma once

namespace Ogre
{
	class Vector3;
	class Quaternion;
}

namespace OgreWrapper
{

ref class MathUtils
{
public:
	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const Ogre::Vector3& source, Engine::Vector3% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const Engine::Vector3% source, Ogre::Vector3& dest );

	/// <summary>
	/// Copies source into a new vector.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Vector3 copyVector3(const Engine::Vector3% source);

	/// <summary>
	/// Copies source into a new vector.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Vector3 copyVector3(const Ogre::Vector3& source); 

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(const Ogre::Quaternion& source, Engine::Quaternion% dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(const Engine::Quaternion% source, Ogre::Quaternion& dest);

	/// <summary>
	/// Copies source into a new quaternion.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Quaternion copyQuaternion(const Engine::Quaternion% source);

	/// <summary>
	/// Copies source into a new quaternion.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Quaternion copyQuaternion(const Ogre::Quaternion& source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(Engine::Ray3% source, Ogre::Ray& dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(Ogre::Ray& source, Engine::Ray3% dest);

	/// <summary>
	/// Copies source into a new ray.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Ray copyRay(Engine::Ray3% source);

	/// <summary>
	/// Copies source into a new ray.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Ray3 copyRay(Ogre::Ray& source);

	/// <summary>
	/// Copies source into a new color.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Color copyColor(const Ogre::ColourValue& source);

	/// <summary>
	/// Copies source into a new color.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::ColourValue copyColor(Engine::Color% source);
};

}