#include "stdafx.h"

#include "OgreFramework.hpp"
#include "OgreD3D11RenderSystem.h"
#include "OgreD3D11Texture.h"
#include "OgreRectangle2D.h"

using namespace Ogre;

void OgreFramework::CreateGenericCubeMesh(char chName[], char chMaterial[], Ogre::ManualObject* pMO, float flSizeX, float flSizeY, float flSizeZ, float flOffsetX, float flOffsetY, float flOffsetZ)
{

	// for calculating bounds of mesh
	float flMinX = 0.0f;
	float flMinY = 0.0f;
	float flMinZ = 0.0f;
	float flMaxX = 0.0f;
	float flMaxY = 0.0f;
	float flMaxZ = 0.0f;

	float flDisX = 0.0f;
	float flDisY = 0.0f;
	float flDisZ = 0.0f;
	float flRadius = 0.0f;
	AxisAlignedBox AABB;





	// setup the manual object


	// start defining the manualObject
	pMO->begin(chMaterial, RenderOperation::OT_TRIANGLE_LIST);


	flMinX = -flSizeX + flOffsetX;
	flMinY = -flSizeY + flOffsetY;
	flMinZ = -flSizeZ + flOffsetZ;

	flMaxX = flSizeX + flOffsetX;
	flMaxY = flSizeY + flOffsetY;
	flMaxZ = flSizeZ + flOffsetZ;


	//////////////////////////////////////////////////////
	// back face
	pMO->position(flMinX, flMaxY, flMinZ);
	pMO->normal(0.0, 0.0, -1);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMaxX, flMaxY, flMinZ);
	pMO->normal(0.0, 0.0, -1);
	pMO->textureCoord(0.0, 0.0);

	pMO->position(flMaxX, flMinY, flMinZ);
	pMO->normal(0.0, 0.0, -1);
	pMO->textureCoord(0.0, 1.0);

	pMO->position(flMinX, flMinY, flMinZ);
	pMO->normal(0.0, 0.0, -1);
	pMO->textureCoord(1.0, 1.0);

	pMO->quad(0, 1, 2, 3);
	//pMO->quad(3, 2, 1, 0) ;

	//////////////////////////////////////////////////////
	// front face
	pMO->position(flMinX, flMaxY, flMaxZ);
	pMO->normal(0.0, 0.0, 1);
	pMO->textureCoord(0.0, 0.0);

	pMO->position(flMaxX, flMaxY, flMaxZ);
	pMO->normal(0.0, 0.0, 1);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMaxX, flMinY, flMaxZ);
	pMO->normal(0.0, 0.0, 1);
	pMO->textureCoord(1.0, 1.0);

	pMO->position(flMinX, flMinY, flMaxZ);
	pMO->normal(0.0, 0.0, 1);
	pMO->textureCoord(0.0, 1.0);

	pMO->quad(7, 6, 5, 4);
	//pMO->quad(4, 5, 6, 7) ;

	//////////////////////////////////////////////////////
	// left face
	pMO->position(flMinX, flMaxY, flMaxZ);
	pMO->normal(-1.0, 0.0, 0.0);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMinX, flMaxY, flMinZ);
	pMO->normal(-1.0, 0.0, 0.0);
	pMO->textureCoord(0.0, 0.0);

	pMO->position(flMinX, flMinY, flMinZ);
	pMO->normal(-1.0, 0.0, 0.0);
	pMO->textureCoord(0.0, 1.0);

	pMO->position(flMinX, flMinY, flMaxZ);
	pMO->normal(-1.0, 0.0, 0.0);
	pMO->textureCoord(1.0, 1.0);

	pMO->quad(8, 9, 10, 11);
	//pMO->quad(11, 10, 9, 8) ;


	//////////////////////////////////////////////////////
	// right face
	pMO->position(flMaxX, flMaxY, flMaxZ);
	pMO->normal(1.0, 0.0, 0.0);
	pMO->textureCoord(0.0, 0.0);

	pMO->position(flMaxX, flMaxY, flMinZ);
	pMO->normal(1.0, 0.0, 0.0);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMaxX, flMinY, flMinZ);
	pMO->normal(1.0, 0.0, 0.0);
	pMO->textureCoord(1.0, 1.0);

	pMO->position(flMaxX, flMinY, flMaxZ);
	pMO->normal(1.0, 0.0, 0.0);
	pMO->textureCoord(0.0, 1.0);

	pMO->quad(15, 14, 13, 12);
	//pMO->quad(12, 13, 14, 15) ;

	//////////////////////////////////////////////////////
	// top face
	pMO->position(flMinX, flMaxY, flMaxZ);
	pMO->normal(0.0, 1.0, 0.0);
	pMO->textureCoord(0.0, 1.0);

	pMO->position(flMaxX, flMaxY, flMaxZ);
	pMO->normal(0.0, 1.0, 0.0);
	pMO->textureCoord(1.0, 1.0);

	pMO->position(flMaxX, flMaxY, flMinZ);
	pMO->normal(0.0, 1.0, 0.0);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMinX, flMaxY, flMinZ);
	pMO->normal(0.0, 1.0, 0.0);
	pMO->textureCoord(0.0, 0.0);

	pMO->quad(16, 17, 18, 19);
	//pMO->quad(19, 18, 17, 16) ;

	//////////////////////////////////////////////////////
	// bottom face
	pMO->position(flMinX, flMinY, flMaxZ);
	pMO->normal(0.0, -1.0, 0.0);
	pMO->textureCoord(0.0, 0.0);

	pMO->position(flMaxX, flMinY, flMaxZ);
	pMO->normal(0.0, -1.0, 0.0);
	pMO->textureCoord(1.0, 0.0);

	pMO->position(flMaxX, flMinY, flMinZ);
	pMO->normal(0.0, -1.0, 0.0);
	pMO->textureCoord(1.0, 1.0);

	pMO->position(flMinX, flMinY, flMinZ);
	pMO->normal(0.0, -1.0, 0.0);
	pMO->textureCoord(0.0, 1.0);

	pMO->quad(23, 22, 21, 20);
	//pMO->quad(20, 21, 22, 23) ;
	//////////////////////////////////////////////////////

	pMO->end();


	pMO->setCastShadows(false);
	pMO->setDynamic(false);

}