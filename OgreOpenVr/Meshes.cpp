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

void OgreFramework::CreateWorldGuiMesh(float flSizeX, float flSizeY, float flSizeZ, float flOffsetX, float flOffsetY, float flOffsetZ, float flGuiSize)
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



	float flAspect = (float)VSCREEN_W / (float)VSCREEN_H;

	// setup the manual object


	// start defining the manualObject
	m_pWorldGuiMO->begin("WorldGui", RenderOperation::OT_TRIANGLE_LIST);
	//m_pWorldGuiMO->begin("CubeTex", RenderOperation::OT_TRIANGLE_LIST) ;


	flMinX = -flSizeX + flOffsetX + 4.0f;//flGuiSize/2.0f ;
	flMinY = -flSizeY + flOffsetY + 4.0f;//flGuiSize/2.0f ;
	flMinZ = -flSizeZ + flOffsetZ + 2.0f;//flGuiSize/2.0f ;

	flMaxX = flSizeX + flOffsetX - 4.0f;//flGuiSize/2.0f ;
	flMaxY = flSizeY + flOffsetY - 4.0f;//flGuiSize/2.0f ;
	flMaxZ = flSizeZ + flOffsetZ - 4.0f;//flGuiSize/2.0f ;

	float flMinXb = flOffsetX - flGuiSize * flAspect;
	float flMaxXb = flOffsetX + flGuiSize * flAspect;
	float flMinZb = flOffsetZ - flGuiSize * flAspect;
	float flMaxZb = flOffsetZ + flGuiSize * flAspect;

	float flMinYb = flMinY;
	float flMaxYb = flMinYb + flGuiSize * 2.0f;


	//////////////////////////////////////////////////////
	// back face
	m_pWorldGuiMO->position(flMinXb, flMaxYb, flMinZ);
	m_pWorldGuiMO->normal(0.0, 0.0, -1);
	m_pWorldGuiMO->textureCoord(0.0, 0.0);

	m_pWorldGuiMO->position(flMaxXb, flMaxYb, flMinZ);
	m_pWorldGuiMO->normal(0.0, 0.0, -1);
	m_pWorldGuiMO->textureCoord(1.0, 0.0);

	m_pWorldGuiMO->position(flMaxXb, flMinYb, flMinZ);
	m_pWorldGuiMO->normal(0.0, 0.0, -1);
	m_pWorldGuiMO->textureCoord(1.0, 1.0);

	m_pWorldGuiMO->position(flMinXb, flMinYb, flMinZ);
	m_pWorldGuiMO->normal(0.0, 0.0, -1);
	m_pWorldGuiMO->textureCoord(0.0, 1.0);

	m_pWorldGuiMO->quad(3, 2, 1, 0);
	//m_pWorldGuiMO->quad(3, 2, 1, 0) ;

	//////////////////////////////////////////////////////
	// front face
	m_pWorldGuiMO->position(flMinXb, flMaxYb, flMaxZ);
	m_pWorldGuiMO->normal(0.0, 0.0, 1);
	m_pWorldGuiMO->textureCoord(1.0, 0.0);

	m_pWorldGuiMO->position(flMaxXb, flMaxYb, flMaxZ);
	m_pWorldGuiMO->normal(0.0, 0.0, 1);
	m_pWorldGuiMO->textureCoord(0.0, 0.0);

	m_pWorldGuiMO->position(flMaxXb, flMinYb, flMaxZ);
	m_pWorldGuiMO->normal(0.0, 0.0, 1);
	m_pWorldGuiMO->textureCoord(0.0, 1.0);

	m_pWorldGuiMO->position(flMinXb, flMinYb, flMaxZ);
	m_pWorldGuiMO->normal(0.0, 0.0, 1);
	m_pWorldGuiMO->textureCoord(1.0, 1.0);

	m_pWorldGuiMO->quad(4, 5, 6, 7);
	//m_pWorldGuiMO->quad(4, 5, 6, 7) ;

	//////////////////////////////////////////////////////
	// left face
	m_pWorldGuiMO->position(flMinX, flMaxYb, flMaxZb);
	m_pWorldGuiMO->normal(-1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(0.0, 0.0);

	m_pWorldGuiMO->position(flMinX, flMaxYb, flMinZb);
	m_pWorldGuiMO->normal(-1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(1.0, 0.0);

	m_pWorldGuiMO->position(flMinX, flMinYb, flMinZb);
	m_pWorldGuiMO->normal(-1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(1.0, 1.0);

	m_pWorldGuiMO->position(flMinX, flMinYb, flMaxZb);
	m_pWorldGuiMO->normal(-1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(0.0, 1.0);

	m_pWorldGuiMO->quad(11, 10, 9, 8);
	//m_pWorldGuiMO->quad(11, 10, 9, 8) ;
	//m_pWorldGuiMO->quad(0, 1, 2, 3) ;

	//////////////////////////////////////////////////////
	// right face
	m_pWorldGuiMO->position(flMaxX, flMaxYb, flMaxZb);
	m_pWorldGuiMO->normal(1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(1.0, 0.0);

	m_pWorldGuiMO->position(flMaxX, flMaxYb, flMinZb);
	m_pWorldGuiMO->normal(1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(0.0, 0.0);

	m_pWorldGuiMO->position(flMaxX, flMinYb, flMinZb);
	m_pWorldGuiMO->normal(1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(0.0, 1.0);

	m_pWorldGuiMO->position(flMaxX, flMinYb, flMaxZb);
	m_pWorldGuiMO->normal(1.0, 0.0, 0.0);
	m_pWorldGuiMO->textureCoord(1.0, 1.0);

	m_pWorldGuiMO->quad(12, 13, 14, 15);
	//m_pWorldGuiMO->quad(12, 13, 14, 15) ;
	//m_pWorldGuiMO->quad(4, 5, 6, 7) ;
	//////////////////////////////////////////////////////

	m_pWorldGuiMO->end();


	m_pWorldGuiMO->setCastShadows(false);
	m_pWorldGuiMO->setDynamic(false);

}

void OgreFramework::CreateControllerModel(int nController)
{
	// creating the manual object and scene node will only be done once regardless of how many times the device is connected or reconnected.
	// (Unless the models/nodes are deliberately destroyed on a scene load or something, in which case m_bControllerHasModel must be set to false.)
	if (m_bControllerHasModel[nController]) return;

	sprintf(m_chMessage, "Creating controller model for controller %i, device index %i", nController, m_nControllerTDI[nController]);
	m_pLog->logMessage(m_chMessage);


	////////////////////////////////////////////////////////////////////////////////
	// try to load the model

	vr::TrackedDeviceIndex_t unTrackedDeviceIndex = m_nControllerTDI[nController];
	std::string sRenderModelName = GetTrackedDeviceString(m_pHMD, unTrackedDeviceIndex, vr::Prop_RenderModelName_String);


	vr::RenderModel_t *pModel;
	vr::EVRRenderModelError error;
	while (1)
	{
		error = vr::VRRenderModels()->LoadRenderModel_Async(sRenderModelName.c_str(), &pModel);
		if (error != vr::VRRenderModelError_Loading)
			break;

		Sleep(1);
	}

	if (error != vr::VRRenderModelError_None)
	{
		sprintf(m_chMessage, "Unable to load render model %s - %s", sRenderModelName.c_str(), vr::VRRenderModels()->GetRenderModelErrorNameFromEnum(error));
		m_pLog->logMessage(m_chMessage);
		DefaultControllerModel(nController);
		return;
	}

	vr::RenderModel_TextureMap_t *pTexture;
	while (1)
	{
		error = vr::VRRenderModels()->LoadTexture_Async(pModel->diffuseTextureId, &pTexture);
		if (error != vr::VRRenderModelError_Loading)
			break;

		Sleep(1);
	}

	if (error != vr::VRRenderModelError_None)
	{
		sprintf(m_chMessage, "Unable to load render texture id:%d for render model %s", pModel->diffuseTextureId, sRenderModelName.c_str());
		m_pLog->logMessage(m_chMessage);
		vr::VRRenderModels()->FreeRenderModel(pModel);
		DefaultControllerModel(nController);
		return;
	}


	/////////////////////////////////////////////////////////////////////////////////////////////
	// create the texture

	float flScaleX = 1.0;
	float flScaleY = 1.0;
	Ogre::HardwarePixelBufferSharedPtr pixelBuffer;
	if (nController == 0)
	{
		pixelBuffer = Ogre::TextureManager::getSingleton().getByName("Controller0.tga")->getBuffer();
		flScaleX = (float)pTexture->unWidth / (float)Ogre::TextureManager::getSingleton().getByName("Controller0.tga")->getWidth();
		flScaleY = (float)pTexture->unHeight / (float)Ogre::TextureManager::getSingleton().getByName("Controller0.tga")->getHeight();
	}
	else
	{
		pixelBuffer = Ogre::TextureManager::getSingleton().getByName("Controller1.tga")->getBuffer();
		flScaleX = (float)pTexture->unWidth / (float)Ogre::TextureManager::getSingleton().getByName("Controller0.tga")->getWidth();
		flScaleY = (float)pTexture->unHeight / (float)Ogre::TextureManager::getSingleton().getByName("Controller0.tga")->getHeight();
	}

	pixelBuffer->lock(Ogre::HardwareBuffer::HBL_WRITE_ONLY);
	const Ogre::PixelBox& pixelBox = pixelBuffer->getCurrentLock();
	Ogre::uint8* pDest = static_cast<Ogre::uint8*>(pixelBox.data);





	for (size_t i = 0; i < CONTROLLERTEXTURESIZE; i++)
	{
		int nPosY = i * flScaleY;
		const unsigned char *pIndexY = pTexture->rubTextureMapData + nPosY * pTexture->unWidth * 4;


		for (size_t j = 0; j < CONTROLLERTEXTURESIZE; j++)
		{
			int nPosX = j * flScaleX;


			const unsigned char *pIndex = pIndexY + nPosX * 4;



			*pDest++ = *pIndex++;
			*pDest++ = *pIndex++;
			*pDest++ = *pIndex++;
			*pDest++ = *pIndex++;

		}
	}

	pixelBuffer->unlock();




	/////////////////////////////////////////////////////////////////////////////////////////////
	// create the manual object

	char chName[256];
	sprintf(chName, "Controller%i", nController);

	m_ControllerSN[nController] = m_pSceneManager->getRootSceneNode()->createChildSceneNode(chName);
	m_ControllerMO[nController] = m_pSceneManager->createManualObject(chName);

	m_ControllerMO[nController]->begin(chName, RenderOperation::OT_TRIANGLE_LIST);

	for (int nVert = 0; nVert<pModel->unVertexCount; nVert++)
	{
		m_ControllerMO[nController]->position(pModel->rVertexData[nVert].vPosition.v[0], pModel->rVertexData[nVert].vPosition.v[1], pModel->rVertexData[nVert].vPosition.v[2]);
		m_ControllerMO[nController]->normal(pModel->rVertexData[nVert].vNormal.v[0], pModel->rVertexData[nVert].vNormal.v[1], pModel->rVertexData[nVert].vNormal.v[2]);
		m_ControllerMO[nController]->textureCoord(pModel->rVertexData[nVert].rfTextureCoord[0], pModel->rVertexData[nVert].rfTextureCoord[1]);
	}

	int nIndex = 0;
	for (int nTri = 0; nTri<pModel->unTriangleCount; nTri++)
	{
		m_ControllerMO[nController]->triangle(pModel->rIndexData[nIndex], pModel->rIndexData[nIndex + 1], pModel->rIndexData[nIndex + 2]);
		nIndex += 3;
	}

	m_ControllerMO[nController]->end();

	// add the line 
	m_ControllerMO[nController]->begin("Line", RenderOperation::OT_LINE_LIST);
	m_ControllerMO[nController]->position(0, 0, 0);
	m_ControllerMO[nController]->colour(1, 1, 1);
	m_ControllerMO[nController]->position(0, 0, -128);
	m_ControllerMO[nController]->colour(1, 1, 1);
	m_ControllerMO[nController]->end();



	m_ControllerSN[nController]->attachObject(m_ControllerMO[nController]);
	m_pSceneManager->getRootSceneNode()->removeChild(m_ControllerSN[nController]);
	m_bControllerHasModel[nController] = true;



	/////////////////////////////////////////////////////////////////////////////////////////////


	vr::VRRenderModels()->FreeRenderModel(pModel);
	vr::VRRenderModels()->FreeTexture(pTexture);
}

void OgreFramework::DeleteControllerModel(int nController)
{
	if (!m_bControllerHasModel[nController]) return;

	m_ControllerSN[nController]->detachObject(m_ControllerMO[nController]);

	Ogre::SceneNode* pParent = (Ogre::SceneNode*)m_ControllerSN[nController]->getParent();
	if (pParent == NULL)
	{
		m_pSceneManager->getRootSceneNode()->addChild(m_ControllerSN[nController]);
		m_pSceneManager->getRootSceneNode()->removeAndDestroyChild(m_ControllerSN[nController]->getName());
	}
	else
		pParent->removeAndDestroyChild(m_ControllerSN[nController]->getName());

	m_pSceneManager->destroyManualObject(m_ControllerMO[nController]);
	m_bControllerHasModel[nController] = false;
}

void OgreFramework::DefaultControllerModel(int nController)
{
	float flControlSizeX = 0.025f;
	float flControlSizeY = 0.025f;
	float flControlSizeZ = 0.15f;

	char chName[256];
	sprintf(chName, "Controller%i", nController);

	m_ControllerSN[nController] = m_pSceneManager->getRootSceneNode()->createChildSceneNode(chName);
	m_ControllerMO[nController] = m_pSceneManager->createManualObject(chName);
	CreateGenericCubeMesh(chName, "CubeTex", m_ControllerMO[nController], flControlSizeX, flControlSizeY, flControlSizeZ, 0.0f, 0.0f, flControlSizeZ / 2.0f);
	m_ControllerSN[nController]->attachObject(m_ControllerMO[nController]);
	m_pSceneManager->getRootSceneNode()->removeChild(m_ControllerSN[nController]);
	m_bControllerHasModel[nController] = true;
}