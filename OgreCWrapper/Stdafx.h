// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#ifndef STDAFX_H
#define STDAFX_H

#pragma warning(push)
#pragma warning(disable : 4635)
#include "Ogre.h"
#pragma warning(pop)

#if defined(WINDOWS) || defined(WINRT)
#define _AnomalousExport __declspec(dllexport)
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

typedef unsigned int uint;
typedef unsigned char byte;
typedef unsigned short ushort;
typedef const char* String;

#include "OgreExceptionManager.h"

class Vector3
{
public:
	float x, y, z;

	Vector3(const Ogre::Vector3& ogreVector)
		:x(ogreVector.x),
		y(ogreVector.y),
		z(ogreVector.z)
	{

	}

	Ogre::Vector3 toOgre() const
	{
		return Ogre::Vector3(x, y, z);
	}
};

class Quaternion
{
public:
	float x, y, z, w;

	Quaternion(const Ogre::Quaternion& ogreQuaternion)
		:x(ogreQuaternion.x),
		y(ogreQuaternion.y),
		z(ogreQuaternion.z),
		w(ogreQuaternion.w)
	{

	}

	Quaternion(const Ogre::Vector4& ogreVec4)
		:x(ogreVec4.x),
		y(ogreVec4.y),
		z(ogreVec4.z),
		w(ogreVec4.w)
	{

	}

	Ogre::Quaternion toOgre() const
	{
		return Ogre::Quaternion(w, x, y, z);
	}

	Ogre::Vector4 toOgreVec4() const
	{
		return Ogre::Vector4(x, y, z, w);
	}
};

class Ray3
{
public:
	Vector3 origin;
	Vector3 direction;

	Ray3(const Ogre::Ray& ray)
		:origin(ray.getOrigin()),
		direction(ray.getDirection())
	{

	}

	Ogre::Ray toOgre()
	{
		return Ogre::Ray(origin.toOgre(), direction.toOgre());
	}
};

class Matrix4x4
{
public:
	float m00;
	float m01;
	float m02;
	float m03;
	float m10;
	float m11;
	float m12;
	float m13;
	float m20;
	float m21;
	float m22;
	float m23;
	float m30;
	float m31;
	float m32;
	float m33;

	Matrix4x4(const Ogre::Matrix4& matrix)
		:m00(matrix[0][0]), m01(matrix[0][1]), m02(matrix[0][2]), m03(matrix[0][3]),
		m10(matrix[1][0]), m11(matrix[1][1]), m12(matrix[1][2]), m13(matrix[1][3]),
		m20(matrix[2][0]), m21(matrix[2][1]), m22(matrix[2][2]), m23(matrix[2][3]),
		m30(matrix[3][0]), m31(matrix[3][1]), m32(matrix[3][2]), m33(matrix[3][3])
	{

	}

	Ogre::Matrix4 toOgre()
	{
		return Ogre::Matrix4(m00, m01, m02, m03,
							 m10, m11, m12, m13,
							 m20, m21, m22, m23,
							 m30, m31, m32, m33);
	}
};

class Color
{
public:
	float r, g, b, a;

	Color(const Ogre::ColourValue& ogreColor)
		:r(ogreColor.r),
		g(ogreColor.g),
		b(ogreColor.b),
		a(ogreColor.a)
	{

	}

	Ogre::ColourValue toOgre() const
	{
		return Ogre::ColourValue(r, g, b, a);
	}
};

class AxisAlignedBox
{
public:
	Vector3 minimum;
	Vector3 maximum;

	AxisAlignedBox(const Ogre::AxisAlignedBox& ogreBox)
		:minimum(ogreBox.getMinimum()),
		maximum(ogreBox.getMaximum())
	{

	}

	Ogre::AxisAlignedBox toOgre() const
	{
		return Ogre::AxisAlignedBox(minimum.toOgre(), maximum.toOgre());
	}
};

/// <Summary>
/// This method will return a copy of the passed string in a new buffer that can
/// be freed by the clr safely. This is the default behavior if a String is
/// returned on the C# side. This only needs to be used when Ogre returns a
/// string by value or else it will go out of scope before the method ends.
/// </Summary>
inline String createClrFreeableString(const Ogre::String& ogreString)
{
	char* clrBuf = new char[ogreString.length() + 1];
	strcpy(clrBuf, ogreString.c_str());
	return clrBuf;
}

/// <Summary>
/// This function pointer will pass an Ogre::SharedPtr instance to the managed
/// side so a managed wrapper can be created if needed.
/// </Summary>
typedef void (*ProcessWrapperObjectDelegate)(const void* nativeObject, const void* stackSharedPtr);

#endif