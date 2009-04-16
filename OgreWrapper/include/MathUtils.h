#pragma once

namespace Ogre
{
	class Vector3;
	class Quaternion;
}

namespace Rendering
{

value class Color;

ref class MathUtils
{
public:
	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const Ogre::Vector3& source, EngineMath::Vector3% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const EngineMath::Vector3% source, Ogre::Vector3& dest );

	/// <summary>
	/// Copies source into a new vector.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Vector3 copyVector3(const EngineMath::Vector3% source);

	/// <summary>
	/// Copies source into a new vector.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Vector3 copyVector3(const Ogre::Vector3& source); 

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(const Ogre::Quaternion& source, EngineMath::Quaternion% dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(const EngineMath::Quaternion% source, Ogre::Quaternion& dest);

	/// <summary>
	/// Copies source into a new quaternion.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Quaternion copyQuaternion(const EngineMath::Quaternion% source);

	/// <summary>
	/// Copies source into a new quaternion.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Quaternion copyQuaternion(const Ogre::Quaternion& source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(EngineMath::Ray3% source, Ogre::Ray& dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(Ogre::Ray& source, EngineMath::Ray3% dest);

	/// <summary>
	/// Copies source into a new ray.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::Ray copyRay(EngineMath::Ray3% source);

	/// <summary>
	/// Copies source into a new ray.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Ray3 copyRay(Ogre::Ray& source);

	/// <summary>
	/// Copies source into a new color.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Rendering::Color copyColor(const Ogre::ColourValue& source);

	/// <summary>
	/// Copies source into a new color.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Ogre::ColourValue copyColor(Rendering::Color% source);
};

}