// Ogre 2d: a small wrapper for 2d Graphics Programming in Ogre.
/*
   Wrapper for 2d Graphics in the Ogre 3d engine.

   Coded by H. Hernán Moraldo from Moraldo Games
   www.hernan.moraldo.com.ar/pmenglish/field.php

   Thanks for the Cegui team as their rendering code in Ogre gave me
   fundamental insight on the management of hardware buffers in Ogre.

   --------------------

   Copyright (c) 2006 Horacio Hernan Moraldo

   This software is provided 'as-is', without any express or
   implied warranty. In no event will the authors be held liable
   for any damages arising from the use of this software.

   Permission is granted to anyone to use this software for any
   purpose, including commercial applications, and to alter it and
   redistribute it freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you
   must not claim that you wrote the original software. If you use
   this software in a product, an acknowledgment in the product
   documentation would be appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and
   must not be misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.

*/
/* modified by mkultra333 for the BZN engine to allow rendering to variable rendertargets and coloured sprites,
	and to add gui controls.
	The original Ogre2D code can handle multiple different textures, at the expense of additional batches.
	This version is modified to only use a single texture. 

	Update: 
	Code is pretty heavily modified at this point, bares little in common with the original.  Got rid of hardware buffers.
	Uses a manually created mesh and node instead, plus shaders and its own resource groups.
	This was due to the move to DX11 compatibility, and I couldn't find a way to generate and bind the necessary shaders
	although I know it's possible since CEGUI does it.

	 Copyright (c) 2012 Jared Prince
	 Copyright (c) 2016 Jared Prince

   This software is provided 'as-is', without any express or
   implied warranty. In no event will the authors be held liable
   for any damages arising from the use of this software.

   Permission is granted to anyone to use this software for any
   purpose, including commercial applications, and to alter it and
   redistribute it freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you
   must not claim that you wrote the original software. If you use
   this software in a product, an acknowledgment in the product
   documentation would be appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and
   must not be misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.
*/



#include "stdafx.h"

#include "ogre2d-main.h"


Ogre2dManager::Ogre2dManager()
{
}

Ogre2dManager::~Ogre2dManager()
{
}


void Ogre2dManager::init(Ogre::SceneManager* sceneManager, char* pName, char *pResourceLoc, bool bFilter)
{
   sceneMan=sceneManager ;
	SetupCharCoords() ;

	strcpy(chName, pName) ;

	
	ZeroMemory( (void*)&m_NullButton, sizeof(BZNBUTTON) ) ;

	m_flTextScaleMedX=0.7f ;
	m_flTextScaleMedY=0.7f ;

	m_flInfoTextLineHeight=15.0f ;
	m_flInfoTextFontScaleX=0.95f ;
	m_flInfoTextFontScaleY=0.95f ;
	
	m_GuiTime=0.0 ;

	m_nMenuMode=MENU_NONE ;
	m_nOldMenuMode=MENU_NONE ;
	m_nEditBoxMenu=MENU_NONE ;
	m_nEditBoxAction=EDITBOXACTION_NONE ;

	m_nLastDeactivatedEditBoxMenu=MENU_NONE ;

	m_nQueueListBoxMenu=MENU_NONE ;
	m_nListBoxMenu=MENU_NONE ;

	m_nSliderMenu=MENU_NONE ;
	m_nSliderButton=0 ;

	SetupGui() ;

	//ZeroMemory((void*)m_nKT, sizeof(m_nKT)) ; // good place to clear the keytoggle just in case
	//ZeroMemory((void*)m_dKD, sizeof(m_dKD)) ; // good place to clear the keytoggle just in case

  ZeroMemory((void*)m_chInfoText, sizeof(m_chInfoText)) ;
	m_dInfoTextTime=0.0 ;
	m_nInfoTextLine=0 ;
	m_nInfoTextLongDisplay=0 ;


	strcpy(chCameraName, "GuiCamera_") ;
	strcat(chCameraName, chName) ;
	m_pGuiCamera = sceneMan->createCamera(chCameraName);
	m_pGuiCamera->setPosition(Ogre::Vector3(0, 0, 128));
	m_pGuiCamera->lookAt(Ogre::Vector3(0, 0, 0));
	m_pGuiCamera->setNearClipDistance(8);
	m_pGuiCamera->setFarClipDistance(32768);
	m_pGuiCamera->setProjectionType(Ogre::ProjectionType::PT_ORTHOGRAPHIC);
	m_pGuiCamera->setOrthoWindow(2, 2);

	m_bGuiMeshReady=false ;
	
	m_nFrameCount=0 ;

	// create a resource group just for the gui
	strcpy(chResourceName, "GuiResource_") ;
	strcat(chResourceName, chName) ;
	strcpy(chLocationName, pResourceLoc) ;
	strcat(chLocationName, chResourceName) ;

	Ogre::ResourceGroupManager::getSingleton().createResourceGroup(chResourceName) ;
	Ogre::ResourceGroupManager::getSingleton().addResourceLocation(chLocationName, "FileSystem", chResourceName);
	Ogre::ResourceGroupManager::getSingleton().initialiseResourceGroup(chResourceName) ;



	CreateGuiMesh() ;

	if(!bFilter) Ogre::MaterialManager::getSingleton().getByName(chName)->getTechnique(0)->getPass(0)->setTextureFiltering(Ogre::TFO_NONE);

}

// get rid of the vector memory used by the menu
void Ogre2dManager::CleanupGui()
{
	for(int nMenu=0 ; nMenu<Menu.size() ; nMenu++)
	{
		// go through all the buttons and free the memory used for names.
		int nMaxButton=Menu[nMenu].Button.size() ;
		for(int nButton=0 ; nButton<nMaxButton ; nButton++)
		{
			for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
				delete [] Menu[nMenu].Button[nButton].Name[nSwitch] ;
		}

		// clear the vector used for buttons
		Menu[nMenu].Button.clear() ;
		Menu[nMenu].SpecialControl.clear() ;
	}

	Menu.clear() ;

	if(Ogre2dManager::sceneMan->hasSceneNode(chNodeName))
	{
		if(!m_GuiMeshNode->isInSceneGraph())
			Ogre2dManager::sceneMan->getRootSceneNode()->addChild(m_GuiMeshNode) ;
		Ogre2dManager::sceneMan->getRootSceneNode()->removeAndDestroyChild(chNodeName) ;
	}

	Ogre2dManager::sceneMan->destroyCamera(chCameraName) ;

	//Ogre::ResourceGroupManager::getSingleton().clearResourceGroup(chResourceName) ;

	Ogre::ResourceGroupManager::getSingleton().destroyResourceGroup(chResourceName) ;
}

bool Ogre2dManager::CreateGuiMesh()
{
	//if(!Ogre::ResourceGroupManager::getSingleton().isResourceGroupInitialised("QuickResource"))
	//	return false;

	strcpy(chMeshName, "GuiMesh_") ;
	strcat(chMeshName, chName) ;

	/// Create the mesh via the MeshManager
	m_GuiMesh = Ogre::MeshManager::getSingleton().createManual(chMeshName, chResourceName);

	/// Create one submesh
	Ogre::SubMesh* sub = m_GuiMesh->createSubMesh();


	const size_t nVertices = GUI_MAXVERT;
	const size_t vbufCount = (3 + 3 + 2)*nVertices; // position, colour, and uv.xy
	float* vertices = new float[vbufCount];
	//if (vertices == NULL) { m_pLog->logMessage("Out Of Memory: gui vertices"); return 0; }

	int nVPos = 0;
	int nLoop = 0;

	int nXPos = 0;

	float flXPosA = 0;
	float flYPosA = 0;
	float flZPosA = 0;


	// setup our initial quad data
	for (nLoop = 0; nLoop<GUI_MAXTRI/2; nLoop++)
	{

		flXPosA = nXPos;
		flYPosA = 0.0f;
		flZPosA = 0.0f;

		// vert 0
		vertices[nVPos++] = flXPosA - 8; // position
		vertices[nVPos++] = flYPosA;
		vertices[nVPos++] = flZPosA;
		vertices[nVPos++] = 1.0f; // colour
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 0.0f; // uv
		vertices[nVPos++] = 0.0f;

		// vert 1
		vertices[nVPos++] = flXPosA; // position
		vertices[nVPos++] = flYPosA - 8;
		vertices[nVPos++] = flZPosA;
		vertices[nVPos++] = 1.0f; // colour
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 0.0f; // uv
		vertices[nVPos++] = 1.0f;

		// vert 2
		vertices[nVPos++] = flXPosA + 8; // position
		vertices[nVPos++] = flYPosA;
		vertices[nVPos++] = flZPosA;
		vertices[nVPos++] = 1.0f; // colour
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f; // uv
		vertices[nVPos++] = 0.0f;

		// vert 3
		vertices[nVPos++] = flXPosA ; // position
		vertices[nVPos++] = flYPosA + 8;
		vertices[nVPos++] = flZPosA;
		vertices[nVPos++] = 1.0f; // colour
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f;
		vertices[nVPos++] = 1.0f; // uv
		vertices[nVPos++] = 1.0f;

		nXPos += 16;
	}

	/// Define the triangles
	/// The values in this table refer to vertices in the above table

	const size_t ibufCount = GUI_MAXTRI*3;
	unsigned short faces[ibufCount];
	int nVertNum=0 ;
	int nVert0=0 ;
	int nVert1=0 ;
	int nVert2=0 ;
	int nVert3=0 ;
	int nTriVert=0 ;
	for (int nLoop = 0; nLoop<GUI_MAXTRI/2; nLoop++)
	{
		nVert0=nVertNum ;
		nVert1=nVertNum+1 ;
		nVert2=nVertNum+2 ;
		nVert3=nVertNum+3 ;

		// first triangle
		faces[nTriVert++] = nVert0 ;
		faces[nTriVert++] = nVert1 ;
		faces[nTriVert++] = nVert2 ;

		// second triangle
		faces[nTriVert++] = nVert1 ;
		faces[nTriVert++] = nVert0 ;
		faces[nTriVert++] = nVert3 ;

		nVertNum+=4 ;
	}


	/// Create vertex data structure vertices
	sub->vertexData = new Ogre::VertexData(); //?? no CHECKDELETE_ARRAY or CHECKDELETE_POINTER for this... does the graphics card or ogre handle it?
	sub->vertexData->vertexCount = nVertices;


	/// Create declaration (memory format) of vertex data
	Ogre::VertexDeclaration* decl = sub->vertexData->vertexDeclaration;

	size_t offset = 0;
	// 1st buffer
	decl->addElement(0, offset, Ogre::VET_FLOAT3, Ogre::VES_POSITION);
	offset += Ogre::VertexElement::getTypeSize(Ogre::VET_FLOAT3);
	decl->addElement(0, offset, Ogre::VET_FLOAT3, Ogre::VES_DIFFUSE);
	offset += Ogre::VertexElement::getTypeSize(Ogre::VET_FLOAT3);
	decl->addElement(0, offset, Ogre::VET_FLOAT2, Ogre::VES_TEXTURE_COORDINATES);
	offset += Ogre::VertexElement::getTypeSize(Ogre::VET_FLOAT2);



	/// Allocate vertex buffer of the requested number of vertices (vertexCount) 
	/// and bytes per vertex (offset)
	Ogre::HardwareVertexBufferSharedPtr vbuf =
		Ogre::HardwareBufferManager::getSingleton().createVertexBuffer(
		offset, sub->vertexData->vertexCount, Ogre::HardwareBuffer::HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE);

	/// Upload the vertex data to the card
	vbuf->writeData(0, vbuf->getSizeInBytes(), vertices, true);

	/// Set vertex buffer binding so buffer 0 is bound to our vertex buffer
	Ogre::VertexBufferBinding* bind = sub->vertexData->vertexBufferBinding;
	bind->setBinding(0, vbuf);


	/// Allocate index buffer of the requested number of vertices (ibufCount) 
	Ogre::HardwareIndexBufferSharedPtr ibuf = Ogre::HardwareBufferManager::getSingleton().
		createIndexBuffer(
		Ogre::HardwareIndexBuffer::IT_16BIT,
		ibufCount,
		Ogre::HardwareBuffer::HBU_STATIC_WRITE_ONLY);

	/// Upload the index data to the card
	ibuf->writeData(0, ibuf->getSizeInBytes(), faces, true);

	/// Set parameters of the submesh
	sub->useSharedVertices = false;
	sub->indexData->indexBuffer = ibuf;
	sub->indexData->indexCount = ibufCount;
	sub->indexData->indexStart = 0;

	/// Set bounding information (for culling)
	m_GuiMesh->_setBounds(Ogre::AxisAlignedBox(-1000000, -1000000, -1000000, 1000000, 1000000, 1000000));
	m_GuiMesh->_setBoundingSphereRadius(1000000);

	/// Notify -Mesh object that it has been loaded
	m_GuiMesh->load();

	// create entity
	strcpy(chEntityName, "GuiEntity_") ;
	strcat(chEntityName, chName) ;
	m_GuiMeshEntity = sceneMan->createEntity(chEntityName, chMeshName);
	m_GuiMeshEntity->setCastShadows(false);

	// assign material
	m_GuiMeshEntity->setMaterialName(chName, chResourceName);

	// create scene node and attach entity
	strcpy(chNodeName, "GuiNode_") ;
	strcat(chNodeName, chName) ;
	m_GuiMeshNode = sceneMan->getRootSceneNode()->createChildSceneNode(chNodeName);
	m_GuiMeshNode->attachObject(m_GuiMeshEntity);

	m_bGuiMeshReady=true ;

	delete[] vertices;

	return true ;

}

void Ogre2dManager::UpdateMesh()
{
	//if(!m_bGuiMeshReady) 
	//{
	//	if(!CreateGuiMesh()) return ;
	//}

	Ogre::SubMesh* pSubMesh = m_GuiMesh->getSubMesh(0);

	// Vertex Position
	const Ogre::VertexElement*					VertexEle_Position =
		pSubMesh->vertexData->vertexDeclaration->findElementBySemantic(Ogre::VES_POSITION);

	// get vertex buffer info via the position element
	Ogre::HardwareVertexBufferSharedPtr VertexBuf =
		pSubMesh->vertexData->vertexBufferBinding->getBuffer(VertexEle_Position->getSource());

	unsigned char*											VertexPtr =
		static_cast<unsigned char*>(VertexBuf->lock(Ogre::HardwareBuffer::LockOptions::HBL_DISCARD));
	int VertSize = VertexBuf->getVertexSize();
	float* pElement = NULL;

	std::list<Ogre2dSprite>::iterator currSpr, endSpr;
	unsigned int nSpriteMax = sprites.size();
	endSpr = sprites.end();
	currSpr = sprites.begin();


	while (currSpr != endSpr)
	{

		VertexEle_Position->baseVertexPointerToElement(VertexPtr, &pElement);
		pElement[0] = currSpr->x1;
		pElement[1] = currSpr->y2;
		pElement[2] = -1;
		pElement[3] = currSpr->r;
		pElement[4] = currSpr->g;
		pElement[5] = currSpr->b;
		pElement[6] = currSpr->tx1;
		pElement[7] = currSpr->ty2;
		VertexPtr += VertSize; // move on to the next vertex

		VertexEle_Position->baseVertexPointerToElement(VertexPtr, &pElement);
		pElement[0] = currSpr->x2;
		pElement[1] = currSpr->y1;
		pElement[2] = -1;
		pElement[3] = currSpr->r;
		pElement[4] = currSpr->g;
		pElement[5] = currSpr->b;
		pElement[6] = currSpr->tx2;
		pElement[7] = currSpr->ty1;
		VertexPtr += VertSize; // move on to the next vertex

		VertexEle_Position->baseVertexPointerToElement(VertexPtr, &pElement);
		pElement[0] = currSpr->x1;
		pElement[1] = currSpr->y1;
		pElement[2] = -1;
		pElement[3] = currSpr->r;
		pElement[4] = currSpr->g;
		pElement[5] = currSpr->b;
		pElement[6] = currSpr->tx1;
		pElement[7] = currSpr->ty1;
		VertexPtr += VertSize; // move on to the next vertex

		VertexEle_Position->baseVertexPointerToElement(VertexPtr, &pElement);
		pElement[0] = currSpr->x2;
		pElement[1] = currSpr->y2;
		pElement[2] = -1;
		pElement[3] = currSpr->r;
		pElement[4] = currSpr->g;
		pElement[5] = currSpr->b;
		pElement[6] = currSpr->tx2;
		pElement[7] = currSpr->ty2;
		VertexPtr += VertSize; // move on to the next vertex

		currSpr++;
	}


	VertexBuf->unlock();

	// update the index, we make it as small as possible
	pSubMesh->indexData->indexCount = nSpriteMax*6 ; // 2 triangles per sprite, 3 vertex indices per triangle = 2*3 indices 

	Ogre::AxisAlignedBox AABB;
	AABB.setExtents(-1000000.0f, -1000000.0f, -1000000.0f, 1000000.0f, 1000000.0f, 1000000.0f);
	m_GuiMesh->_setBounds(AABB, false);
	m_GuiMesh->_setBoundingSphereRadius(1000000);
}

void Ogre2dManager::DrawGui(Ogre::RenderWindow* pRenderWnd, int* pBatchCount, int* pTriCount)
{
	if (sprites.empty()) return;

	UpdateMesh() ;

	Ogre::Camera* pOldCam=pRenderWnd->getViewport(0)->getCamera() ;
	pRenderWnd->getViewport(0)->setCamera(m_pGuiCamera);

	sceneMan->getRootSceneNode()->removeAllChildren();
	sceneMan->getRootSceneNode()->addChild(m_GuiMeshNode);
	pRenderWnd->update(false);
	*pBatchCount+=pRenderWnd->getStatistics().batchCount ;
	*pTriCount+=pRenderWnd->getStatistics().triangleCount ;


	pRenderWnd->getViewport(0)->setCamera(pOldCam);
}

void Ogre2dManager::DrawGui(Ogre::RenderTexture* pRenderTex, bool bSwap, int* pBatchCount, int* pTriCount)
{
	if (sprites.empty()) return;

	UpdateMesh() ;

	Ogre::Camera* pOldCam=pRenderTex->getViewport(0)->getCamera();
	pRenderTex->getViewport(0)->setCamera(m_pGuiCamera);

	sceneMan->getRootSceneNode()->removeAllChildren();
	sceneMan->getRootSceneNode()->addChild(m_GuiMeshNode);
	pRenderTex->update(bSwap) ;
	*pBatchCount+=pRenderTex->getStatistics().batchCount ;
	*pTriCount+=pRenderTex->getStatistics().triangleCount ;


	pRenderTex->getViewport(0)->setCamera(pOldCam);
}

void Ogre2dManager::SetGuiViewportDimensions(float flXSize, float flYSize)
{
	m_flViewportSizeX=flXSize ;
	m_flViewportSizeY=flYSize ;
}

void Ogre2dManager::end()
{
	CleanupGui() ;
}

void Ogre2dManager::spriteBltFull(
   double x1, double y1, double x2, double y2,
   double tx1, double ty1, double tx2, double ty2,
	 float Rd, float Gr, float Bl)
{
   Ogre2dSprite spr;

   spr.x1=x1;
   spr.y1=y1;
   spr.x2=x2;
   spr.y2=y2;

   spr.tx1=tx1;
   spr.ty1=ty1;
   spr.tx2=tx2;
   spr.ty2=ty2;

	 // bzn colour
	 spr.r=Rd ;
	 spr.g=Gr ;
	 spr.b=Bl ;

   sprites.push_back(spr);
} 

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// mkultra stuff


// should be called at the start of a new frame
void Ogre2dManager::ClearSpriteBuffer()
{
	sprites.clear();
}

// can be called any time.
void Ogre2dManager::AddSprite(float StartX, float StartY, float EndX, float EndY, float StartU, float StartV, float EndU, float EndV, float Rd, float Gr, float Bl)
{
	spriteBltFull(StartX, StartY, EndX, EndY, StartU, StartV, EndU, EndV, Rd, Gr, Bl) ;
}

// easier way to add sprites, onto the "virtual" screen.
void Ogre2dManager::AddSpriteToVirtualScreen(float PositionX, float PositionY, float Width, float Height, float StartU, float StartV, float EndU, float EndV, float Rd, float Gr, float Bl)
{

	float halfx=VSCREEN_W/2 ;
	float halfy=VSCREEN_H/2 ;

	float halfWidth=Width/2.0f ;
	float halfHeight=Height/2.0f ;

	float StartX= (PositionX-halfx-halfWidth)/halfx ;
	float StartY= -(PositionY-halfy-halfWidth)/halfy ; 
	float EndX= (PositionX-halfx+halfWidth)/halfx ; 
	float EndY= -(PositionY-halfy+halfWidth)/halfy ;

	spriteBltFull(StartX, StartY, EndX, EndY, StartU, StartV, EndU, EndV, Rd, Gr, Bl) ;

}



// bJustWidth is used to just get the width of a string, doesn't actually add it to sprites.
// useful for positioning text on the screen
int Ogre2dManager::PrintTextLRG(char text[], int nXPos, int nYPos, float flScaleX, float flScaleY, bool bJustWidth, float Rd, float Gr, float Bl)
{

		int nStartPos=nXPos ;

    int nLoop=0 ;
		int nLetter=0 ;
    int nWidth=0 ; 
		int nHeight=0 ;

		
		float halfx=VSCREEN_W/2 ;
		float halfy=VSCREEN_H/2 ;

    for (nLoop = 0; text[nLoop] != 0; nLoop++)
    {
        nLetter=text[nLoop]; 
        if(nLetter==' ') { nXPos += FONTSPACE*flScaleX ; continue; };
				if((nLetter<'!') || (nLetter>'~')) continue ;
        nLetter-= 33;

        nWidth   = (flTextCoordsLRG[nLetter][2] - flTextCoordsLRG[nLetter][0]) * flScaleX ;

				if(!bJustWidth)
				{
					nHeight  = (flTextCoordsLRG[nLetter][3] - flTextCoordsLRG[nLetter][1]) * flScaleY ;
					// convert to ogre sprite coords
					AddSprite((nXPos-halfx)/halfx, -(nYPos-halfy)/halfy, (nXPos+nWidth-halfx)/halfx, -(nYPos+nHeight-halfy)/halfy, flTextCoordsLRG[nLetter][0]/ TEXW, flTextCoordsLRG[nLetter][1]/ TEXH, flTextCoordsLRG[nLetter][2]/ TEXW, flTextCoordsLRG[nLetter][3]/ TEXH, Rd, Gr, Bl) ;
				}

				nXPos=nXPos+nWidth ;
    }

		return nXPos-nStartPos ; // width
}

void Ogre2dManager::PrintTextLRG_Centred(char text[], int nYPos, float flScaleX, float flScaleY, float Rd, float Gr, float Bl)
{
	int Width=PrintTextLRG(text, 0, nYPos, flScaleX, flScaleY, true, Rd, Gr, Bl) ;
	PrintTextLRG(text, VSCREEN_W/2-Width/2, nYPos, flScaleX, flScaleY, false, Rd, Gr, Bl) ;
}

int Ogre2dManager::PrintTextMED(char text[], int nXPos, int nYPos, float flScaleX, float flScaleY, bool bJustWidth, float Rd, float Gr, float Bl)
{

		int nStartPos=nXPos ;

    int nLoop=0 ;
		int nLetter=0 ;
    int nWidth=0 ; 
		int nHeight=0 ;

		float halfx=VSCREEN_W/2 ;
		float halfy=VSCREEN_H/2 ;

    for (nLoop = 0; text[nLoop] != 0; nLoop++)
    {
        nLetter=text[nLoop]; 
        if(nLetter==' ') { nXPos += FONTSPACE*0.33*flScaleX ; continue; };
				if((nLetter<'!') || (nLetter>'~')) continue ;
        nLetter-= 33;

        nWidth   = (flTextCoordsMED[nLetter][2] - flTextCoordsMED[nLetter][0]) * flScaleX ;

				if(!bJustWidth)
				{
					nHeight  = (flTextCoordsMED[nLetter][3] - flTextCoordsMED[nLetter][1]) * flScaleY ;
					// convert to ogre sprite coords
					AddSprite((nXPos-halfx)/halfx, -(nYPos-halfy)/halfy, (nXPos+nWidth-halfx)/halfx, -(nYPos+nHeight-halfy)/halfy, flTextCoordsMED[nLetter][0]/ TEXW, flTextCoordsMED[nLetter][1]/ TEXH, flTextCoordsMED[nLetter][2]/ TEXW, flTextCoordsMED[nLetter][3]/ TEXH, Rd, Gr, Bl) ;
				}

				nXPos=nXPos+nWidth ;
    }

		return nXPos-nStartPos ; // width
}

void Ogre2dManager::PrintTextMED_Centred(char text[], int nYPos, float flScaleX, float flScaleY, float Rd, float Gr, float Bl)
{
	int Width=PrintTextMED(text, 0, nYPos, flScaleX, flScaleY, true, Rd, Gr, Bl) ;
	PrintTextMED(text, VSCREEN_W/2-Width/2, nYPos, flScaleX, flScaleY, false, Rd, Gr, Bl) ;
}




// sets up the texture coordinates for the text texture, as well as some additional coordinates for other elements
// These are stored as actual pixel coords, and must be divided by texture x and y to get 0.0 - 1.0 ranged UV coords
void Ogre2dManager::SetupCharCoords()
{

	// coords generated by FontStudio421
	float coordsLRG[MAXTEXTCHAR][4]={
		{0 , 0.123046875 , 0.04296875 , 0.24609375 },
		{0 , 0.24609375 , 0.064453125 , 0.369140625 },
		{0 , 0.369140625 , 0.09375 , 0.4921875 },
		{0 , 0.4921875 , 0.080078125 , 0.615234375 },
		{0.80859375 , 0.861328125 , 0.943359375 , 0.984375 },
		{0.09375 , 0 , 0.19140625 , 0.123046875 },
		{0.04296875 , 0.123046875 , 0.08203125 , 0.24609375 },
		{0 , 0.73828125 , 0.05859375 , 0.861328125 },
		{0 , 0.861328125 , 0.060546875 , 0.984375 },
		{0.19140625 , 0 , 0.267578125 , 0.123046875 },
		{0.267578125 , 0 , 0.359375 , 0.123046875 },
		{0.359375 , 0 , 0.40625 , 0.123046875 },
		{0.40625 , 0 , 0.4609375 , 0.123046875 },
		{0.4609375 , 0 , 0.5 , 0.123046875 },
		{0.5 , 0 , 0.578125 , 0.123046875 },
		{0.578125 , 0 , 0.658203125 , 0.123046875 },
		{0.658203125 , 0 , 0.73046875 , 0.123046875 },
		{0.73046875 , 0 , 0.806640625 , 0.123046875 },
		{0.806640625 , 0 , 0.884765625 , 0.123046875 },
		{0.884765625 , 0 , 0.966796875 , 0.123046875 },
		{0.09375 , 0.123046875 , 0.171875 , 0.24609375 },
		{0.171875 , 0.123046875 , 0.251953125 , 0.24609375 },
		{0.09375 , 0.24609375 , 0.171875 , 0.369140625 },
		{0.251953125 , 0.123046875 , 0.33203125 , 0.24609375 },
		{0.33203125 , 0.123046875 , 0.412109375 , 0.24609375 },
		{0.09375 , 0.369140625 , 0.134765625 , 0.4921875 },
		{0.09375 , 0.4921875 , 0.140625 , 0.615234375 },
		{0.412109375 , 0.123046875 , 0.50390625 , 0.24609375 },
		{0.50390625 , 0.123046875 , 0.59375 , 0.24609375 },
		{0.59375 , 0.123046875 , 0.68359375 , 0.24609375 },
		{0.09375 , 0.615234375 , 0.162109375 , 0.73828125 },
		{0.68359375 , 0.123046875 , 0.787109375 , 0.24609375 },
		{0.787109375 , 0.123046875 , 0.876953125 , 0.24609375 },
		{0.876953125 , 0.123046875 , 0.95703125 , 0.24609375 },
		{0.171875 , 0.24609375 , 0.251953125 , 0.369140625 },
		{0.251953125 , 0.24609375 , 0.33984375 , 0.369140625 },
		{0.09375 , 0.73828125 , 0.166015625 , 0.861328125 },
		{0.09375 , 0.861328125 , 0.162109375 , 0.984375 },
		{0.33984375 , 0.24609375 , 0.4296875 , 0.369140625 },
		{0.251953125 , 0.369140625 , 0.33984375 , 0.4921875 },
		{0.171875 , 0.369140625 , 0.232421875 , 0.4921875 },
		{0.171875 , 0.4921875 , 0.23828125 , 0.615234375 },
		{0.251953125 , 0.4921875 , 0.333984375 , 0.615234375 },
		{0.171875 , 0.615234375 , 0.240234375 , 0.73828125 },
		{0.4296875 , 0.24609375 , 0.529296875 , 0.369140625 },
		{0.529296875 , 0.24609375 , 0.619140625 , 0.369140625 },
		{0.619140625 , 0.24609375 , 0.712890625 , 0.369140625 },
		{0.171875 , 0.73828125 , 0.25 , 0.861328125 },
		{0.712890625 , 0.24609375 , 0.806640625 , 0.369140625 },
		{0.251953125 , 0.615234375 , 0.3359375 , 0.73828125 },
		{0.171875 , 0.861328125 , 0.251953125 , 0.984375 },
		{0.251953125 , 0.73828125 , 0.33203125 , 0.861328125 },
		{0.251953125 , 0.861328125 , 0.33984375 , 0.984375 },
		{0.806640625 , 0.24609375 , 0.89453125 , 0.369140625 },
		{0.80859375 , 0.73828125 , 0.93359375 , 0.861328125 },
		{0.33984375 , 0.369140625 , 0.427734375 , 0.4921875 },
		{0.33984375 , 0.4921875 , 0.42578125 , 0.615234375 },
		{0.33984375 , 0.615234375 , 0.419921875 , 0.73828125 },
		{0.94140625 , 0.24609375 , 0.998046875 , 0.369140625 },
		{0.33984375 , 0.73828125 , 0.4140625 , 0.861328125 },
		{0.33984375 , 0.861328125 , 0.3984375 , 0.984375 },
		{0.427734375 , 0.369140625 , 0.51953125 , 0.4921875 },
		{0.51953125 , 0.369140625 , 0.60546875 , 0.4921875 },
		{0.60546875 , 0.369140625 , 0.671875 , 0.4921875 },
		{0.671875 , 0.369140625 , 0.748046875 , 0.4921875 },
		{0.748046875 , 0.369140625 , 0.82421875 , 0.4921875 },
		{0.82421875 , 0.369140625 , 0.892578125 , 0.4921875 },
		{0.892578125 , 0.369140625 , 0.97265625 , 0.4921875 },
		{0.427734375 , 0.4921875 , 0.50390625 , 0.615234375 },
		{0.427734375 , 0.615234375 , 0.482421875 , 0.73828125 },
		{0.50390625 , 0.4921875 , 0.583984375 , 0.615234375 },
		{0.50390625 , 0.615234375 , 0.58203125 , 0.73828125 },
		{0.95703125 , 0.123046875 , 0.99609375 , 0.24609375 },
		{0.427734375 , 0.73828125 , 0.48828125 , 0.861328125 },
		{0.427734375 , 0.861328125 , 0.501953125 , 0.984375 },
		{0.50390625 , 0.73828125 , 0.54296875 , 0.861328125 },
		{0.583984375 , 0.4921875 , 0.697265625 , 0.615234375 },
		{0.50390625 , 0.861328125 , 0.58203125 , 0.984375 },
		{0.697265625 , 0.4921875 , 0.775390625 , 0.615234375 },
		{0.775390625 , 0.4921875 , 0.8515625 , 0.615234375 },
		{0.8515625 , 0.4921875 , 0.931640625 , 0.615234375 },
		{0.931640625 , 0.4921875 , 0.986328125 , 0.615234375 },
		{0.583984375 , 0.615234375 , 0.65234375 , 0.73828125 },
		{0.583984375 , 0.73828125 , 0.640625 , 0.861328125 },
		{0.65234375 , 0.615234375 , 0.73046875 , 0.73828125 },
		{0.65234375 , 0.73828125 , 0.728515625 , 0.861328125 },
		{0.73046875 , 0.615234375 , 0.841796875 , 0.73828125 },
		{0.65234375 , 0.861328125 , 0.73046875 , 0.984375 },
		{0.841796875 , 0.615234375 , 0.91796875 , 0.73828125 },
		{0.91796875 , 0.615234375 , 0.98828125 , 0.73828125 },
		{0.73046875 , 0.73828125 , 0.80859375 , 0.861328125 },
		{0.89453125 , 0.24609375 , 0.94140625 , 0.369140625 },
		{0 , 0.615234375 , 0.076171875 , 0.73828125 },
		{0 , 0 , 0.09375 , 0.123046875 }
	} ;


	// need to tweak the coords and scale	
	for(int nLoop=0 ; nLoop<MAXTEXTCHAR ; nLoop++)
	{
		flTextCoordsLRG[nLoop][0]=(coordsLRG[nLoop][0]*TEXW+3) ;
		flTextCoordsLRG[nLoop][1]=(coordsLRG[nLoop][1]*TEXW+3) ;
		flTextCoordsLRG[nLoop][2]=(coordsLRG[nLoop][2]*TEXW+2) ;
		flTextCoordsLRG[nLoop][3]=(coordsLRG[nLoop][3]*TEXW+1) ;
	}

	// tweak a few characters because we shifted them after exporting from font studio, to make better use of space
	flTextCoordsLRG['x'-33][0]-=38.0f ;
	flTextCoordsLRG['x'-33][2]-=38.0f ;

	flTextCoordsLRG['%'-33][0]-=76.0f ;
	flTextCoordsLRG['%'-33][2]-=76.0f ;


	// coords generated by FontStudio421
	float coordsMED[MAXTEXTCHAR][4]={
		{0.00390625 , 0.00390625 , 0.013671875 , 0.0390625 },
		{0.044921875 , 0.00390625 , 0.0625 , 0.0390625 },
		{0.0859375 , 0.00390625 , 0.111328125 , 0.0390625 },
		{0.126953125 , 0.00390625 , 0.1484375 , 0.0390625 },
		{0.16796875 , 0.00390625 , 0.208984375 , 0.0390625 },
		{0.208984375 , 0.00390625 , 0.236328125 , 0.0390625 },
		{0.25 , 0.00390625 , 0.259765625 , 0.0390625 },
		{0.291015625 , 0.00390625 , 0.3046875 , 0.0390625 },
		{0.33203125 , 0.00390625 , 0.34765625 , 0.0390625 },
		{0.373046875 , 0.00390625 , 0.392578125 , 0.0390625 },
		{0.4140625 , 0.00390625 , 0.439453125 , 0.0390625 },
		{0.455078125 , 0.00390625 , 0.466796875 , 0.0390625 },
		{0.49609375 , 0.00390625 , 0.509765625 , 0.0390625 },
		{0.537109375 , 0.00390625 , 0.544921875 , 0.0390625 },
		{0.578125 , 0.00390625 , 0.599609375 , 0.0390625 },
		{0.619140625 , 0.00390625 , 0.640625 , 0.0390625 },
		{0.66015625 , 0.00390625 , 0.6796875 , 0.0390625 },
		{0.701171875 , 0.00390625 , 0.720703125 , 0.0390625 },
		{0.7421875 , 0.00390625 , 0.763671875 , 0.0390625 },
		{0.783203125 , 0.00390625 , 0.8046875 , 0.0390625 },
		{0.82421875 , 0.00390625 , 0.845703125 , 0.0390625 },
		{0.865234375 , 0.00390625 , 0.88671875 , 0.0390625 },
		{0.90625 , 0.00390625 , 0.927734375 , 0.0390625 },
		{0.947265625 , 0.00390625 , 0.96875 , 0.0390625 },
		{0.00390625 , 0.0390625 , 0.025390625 , 0.07421875 },
		{0.044921875 , 0.0390625 , 0.0546875 , 0.07421875 },
		{0.0859375 , 0.0390625 , 0.09765625 , 0.07421875 },
		{0.126953125 , 0.0390625 , 0.15234375 , 0.07421875 },
		{0.16796875 , 0.0390625 , 0.193359375 , 0.07421875 },
		{0.208984375 , 0.0390625 , 0.234375 , 0.07421875 },
		{0.25 , 0.0390625 , 0.267578125 , 0.07421875 },
		{0.291015625 , 0.0390625 , 0.3203125 , 0.07421875 },
		{0.33203125 , 0.0390625 , 0.357421875 , 0.07421875 },
		{0.373046875 , 0.0390625 , 0.39453125 , 0.07421875 },
		{0.4140625 , 0.0390625 , 0.435546875 , 0.07421875 },
		{0.455078125 , 0.0390625 , 0.478515625 , 0.07421875 },
		{0.49609375 , 0.0390625 , 0.515625 , 0.07421875 },
		{0.537109375 , 0.0390625 , 0.5546875 , 0.07421875 },
		{0.578125 , 0.0390625 , 0.603515625 , 0.07421875 },
		{0.619140625 , 0.0390625 , 0.642578125 , 0.07421875 },
		{0.66015625 , 0.0390625 , 0.67578125 , 0.07421875 },
		{0.701171875 , 0.0390625 , 0.71875 , 0.07421875 },
		{0.7421875 , 0.0390625 , 0.763671875 , 0.07421875 },
		{0.783203125 , 0.0390625 , 0.80078125 , 0.07421875 },
		{0.82421875 , 0.0390625 , 0.8515625 , 0.07421875 },
		{0.865234375 , 0.0390625 , 0.888671875 , 0.07421875 },
		{0.90625 , 0.0390625 , 0.931640625 , 0.07421875 },
		{0.947265625 , 0.0390625 , 0.966796875 , 0.07421875 },
		{0.00390625 , 0.07421875 , 0.029296875 , 0.109375 },
		{0.044921875 , 0.07421875 , 0.06640625 , 0.109375 },
		{0.0859375 , 0.07421875 , 0.107421875 , 0.109375 },
		{0.126953125 , 0.07421875 , 0.150390625 , 0.109375 },
		{0.16796875 , 0.07421875 , 0.19140625 , 0.109375 },
		{0.208984375 , 0.07421875 , 0.234375 , 0.109375 },
		{0.25 , 0.07421875 , 0.28515625 , 0.109375 },
		{0.291015625 , 0.07421875 , 0.31640625 , 0.109375 },
		{0.33203125 , 0.07421875 , 0.357421875 , 0.109375 },
		{0.373046875 , 0.07421875 , 0.39453125 , 0.109375 },
		{0.4140625 , 0.07421875 , 0.427734375 , 0.109375 },
		{0.455078125 , 0.07421875 , 0.474609375 , 0.109375 },
		{0.49609375 , 0.07421875 , 0.509765625 , 0.109375 },
		{0.537109375 , 0.07421875 , 0.5625 , 0.109375 },
		{0.578125 , 0.07421875 , 0.6015625 , 0.109375 },
		{0.619140625 , 0.07421875 , 0.63671875 , 0.109375 },
		{0.66015625 , 0.07421875 , 0.6796875 , 0.109375 },
		{0.701171875 , 0.07421875 , 0.72265625 , 0.109375 },
		{0.7421875 , 0.07421875 , 0.759765625 , 0.109375 },
		{0.783203125 , 0.07421875 , 0.8046875 , 0.109375 },
		{0.82421875 , 0.07421875 , 0.84375 , 0.109375 },
		{0.865234375 , 0.07421875 , 0.876953125 , 0.109375 },
		{0.90625 , 0.07421875 , 0.927734375 , 0.109375 },
		{0.947265625 , 0.07421875 , 0.96875 , 0.109375 },
		{0.00390625 , 0.109375 , 0.013671875 , 0.14453125 },
		{0.044921875 , 0.109375 , 0.05859375 , 0.14453125 },
		{0.0859375 , 0.109375 , 0.10546875 , 0.14453125 },
		{0.126953125 , 0.109375 , 0.13671875 , 0.14453125 },
		{0.16796875 , 0.109375 , 0.19921875 , 0.14453125 },
		{0.208984375 , 0.109375 , 0.23046875 , 0.14453125 },
		{0.25 , 0.109375 , 0.271484375 , 0.14453125 },
		{0.291015625 , 0.109375 , 0.3125 , 0.14453125 },
		{0.33203125 , 0.109375 , 0.353515625 , 0.14453125 },
		{0.373046875 , 0.109375 , 0.38671875 , 0.14453125 },
		{0.4140625 , 0.109375 , 0.431640625 , 0.14453125 },
		{0.455078125 , 0.109375 , 0.46875 , 0.14453125 },
		{0.49609375 , 0.109375 , 0.517578125 , 0.14453125 },
		{0.537109375 , 0.109375 , 0.55859375 , 0.14453125 },
		{0.578125 , 0.109375 , 0.611328125 , 0.14453125 },
		{0.619140625 , 0.109375 , 0.640625 , 0.14453125 },
		{0.66015625 , 0.109375 , 0.681640625 , 0.14453125 },
		{0.701171875 , 0.109375 , 0.71875 , 0.14453125 },
		{0.7421875 , 0.109375 , 0.76171875 , 0.14453125 },
		{0.783203125 , 0.109375 , 0.794921875 , 0.14453125 },
		{0.82421875 , 0.109375 , 0.845703125 , 0.14453125 },
		{0.865234375 , 0.109375 , 0.892578125 , 0.14453125 }
	};

	// need to tweak the coords and scale	
	int nYOffset=0;
	for(int nLoop=0 ; nLoop<MAXTEXTCHAR ; nLoop++)
	{
		nYOffset=(int)(nLoop/24)*2 ;
		flTextCoordsMED[nLoop][0]=(coordsMED[nLoop][0]*TEXW) ;
		flTextCoordsMED[nLoop][1]=(coordsMED[nLoop][1]*TEXW-2.0 +nYOffset +512) ;
		flTextCoordsMED[nLoop][2]=(coordsMED[nLoop][2]*TEXW+2.0) ;
		flTextCoordsMED[nLoop][3]=(coordsMED[nLoop][3]*TEXW-2.0 +nYOffset +512) ;
	}
	// tweak a few characters because we shifted them after exporting from font studio, to make better use of space
	flTextCoordsMED['%'-33][0]-=4.0f ;
	flTextCoordsMED['%'-33][2]-=4.0f ;



	// additional images
	UVCOORD_CURSOR.left=418.0f ;
	UVCOORD_CURSOR.right=458.0f ;
	UVCOORD_CURSOR.top=447.0f ;
	UVCOORD_CURSOR.bottom=487.0f ;

	UVCOORD_BLANK.left=507.0f ;
	UVCOORD_BLANK.right=508.0f ;
	UVCOORD_BLANK.top=450.0f ;
	UVCOORD_BLANK.bottom=451.0f ;

	UVCOORD_FRAMELRG.left=0.0f ;
	UVCOORD_FRAMELRG.right=416.0f ;
	UVCOORD_FRAMELRG.top=592.0f ;
	UVCOORD_FRAMELRG.bottom=673.0f ;

	UVCOORD_FRAMESML.left=418.0f ;
	UVCOORD_FRAMESML.right=499.0f ;
	UVCOORD_FRAMESML.top=592.0f ;
	UVCOORD_FRAMESML.bottom=673.0f ;

	UVCOORD_BACK.left=0.0f ;
	UVCOORD_BACK.right=256.0f ;
	UVCOORD_BACK.top=960.0f ;
	UVCOORD_BACK.bottom=1024.0f ;


}


// button
void Ogre2dManager::DrawButtonFancy(int nMenu, int nButton) 
{
	float flMenuCentreX=(Menu[nMenu].L+Menu[nMenu].R)/2.0f ;
	float flMenuCentreY=(Menu[nMenu].U+Menu[nMenu].D)/2.0f ;

	float flPosX=Menu[nMenu].L+Menu[nMenu].Button[nButton].PositionX ;
	float flPosY=Menu[nMenu].U+Menu[nMenu].Button[nButton].PositionY ;
	int nActive=Menu[nMenu].Button[nButton].State ;

		// is this an editbox?
	int nEditBox=0 ;
	float flFrameSizeX=Menu[nMenu].Button[nButton].FrameSizeX  ;
	float flButtonSizeX=Menu[nMenu].Button[nButton].SizeX  ;
	float flFrameSizeY=Menu[nMenu].Button[nButton].FrameSizeY  ;
	float flButtonSizeY=Menu[nMenu].Button[nButton].SizeY  ;

	switch(Menu[nMenu].Button[nButton].Action)
	{
		case BUTTONACTION_EDIT_TEXT:
			nEditBox=BUTTONACTION_EDIT_TEXT ;
			break ;

		case BUTTONACTION_EDIT_INTEGER:
			nEditBox=BUTTONACTION_EDIT_INTEGER ;
			break ;

		case BUTTONACTION_EDIT_FLOAT:
			nEditBox=BUTTONACTION_EDIT_FLOAT ;
			break ;

		default:
			nEditBox=0 ;
	}


	float flScreenX=m_flViewportSizeX ;
	float flScreenY=m_flViewportSizeY ;

	float VW=VSCREEN_W/2.0f ;
	float VH=VSCREEN_H/2.0f ;

	

	float flFrameGapX=(flFrameSizeX-flButtonSizeX)/2.0 ;
	float flFrameGapY=(flFrameSizeY-flButtonSizeY)/2.0 ;
	float flButtonEdgeL=flPosX	+ flFrameGapX ;
	
	float flButtonL=0.0f ;
	float flButtonR=0.0f ;
	float flButtonT=0.0f ;
	float flButtonB=0.0f ;

	float Rd=0.0f ;
	float Bl=0.0f ;
	float Gr=0.0f ;
	float Rd2=0.0f ;
	float Bl2=0.0f ;
	float Gr2=0.0f ;

	float offX=0.0f ;

	float flUV_L=0.0f ;
	float flUV_R=0.0f ;
	float flUV_T=0.0f ;
	float flUV_B=0.0f ;
	float flWave=0.0f ;




		// draw the frame
	flButtonL= ( flPosX	-VW)/VW ;
	flButtonR= ( flPosX + flFrameSizeX	-VW)/VW ;
	flButtonT=-( flPosY	-VH)/VH ;
	flButtonB=-( flPosY + flFrameSizeY	-VH)/VH ;

	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_CHECKBOX)
	{
		flUV_L=UVCOORD_FRAMESML.left/TEXW ;
		flUV_R=UVCOORD_FRAMESML.right/TEXW ;
		flUV_T=UVCOORD_FRAMESML.top/TEXH ;
		flUV_B=UVCOORD_FRAMESML.bottom/TEXH ;
	}
	else
	{
		flUV_L=UVCOORD_FRAMELRG.left/TEXW ;
		flUV_R=UVCOORD_FRAMELRG.right/TEXW ;
		flUV_T=UVCOORD_FRAMELRG.top/TEXH ;
		flUV_B=UVCOORD_FRAMELRG.bottom/TEXH ;
	}

	switch(nActive)
	{
		case BUTTONSTATE_INACTIVE:	Rd2=0.45f ;	Gr2=0.45f ;	Bl2=0.45f ; break ;
		case BUTTONSTATE_HOVER:			Rd2=0.75f ;	Gr2=0.75f ;	Bl2=0.75f ; break ;
		case BUTTONSTATE_PRESSED:		Rd2=0.85f ;	Gr2=0.85f ;	Bl2=0.85f ; break ;
	}

	if((Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTLJUST) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTBACK))
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;


	// button face 
	flButtonL= ( flPosX	+ flFrameGapX -VW)/VW ;
	flButtonR= ( flPosX + flFrameGapX + flButtonSizeX	-VW)/VW ;
	flButtonT=-( flPosY	+ flFrameGapY -VH)/VH ;
	flButtonB=-( flPosY + flFrameGapY +flButtonSizeY	-VH)/VH ;

	// clear the background
	flUV_L=UVCOORD_BLANK.left/TEXW ;
	flUV_R=UVCOORD_BLANK.right/TEXW ;
	flUV_T=UVCOORD_BLANK.top/TEXH ;
	flUV_B=UVCOORD_BLANK.bottom/TEXH ;

	if((Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTLJUST))
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0, 0, 0) ;


	float flSpeedMult=1.0f ;

		// if the button is the active edit button
		if((m_nEditBoxMenu==nMenu) && (m_nEditBoxButton==nButton))
		{	Rd=0.5 ; Gr=0.0 ; Bl=0.0 ; }
		else
			switch(nActive)
			{
				case BUTTONSTATE_INACTIVE:	
					Rd=0.2f ; Gr=0.2f ; Bl=0.2f ; flSpeedMult=32.0f ;
					break ;

				case BUTTONSTATE_HOVER:			
					Rd=0.4f ; Gr=0.4f ; Bl=0.45f ;	flSpeedMult=64.0f ;
					break ;

				case BUTTONSTATE_PRESSED:		
					Rd=1.0f ; Gr=1.0f ; Bl=1.0f ; flSpeedMult=128.0f ; 
					break ;
				}

		// if it's a list, make it cyan so it stands out more
		if((Menu[nMenu].Button[nButton].Action==BUTTONACTION_LIST) || (nMenu==MENU_LISTBOX))
			Rd2=0.0f ;
		else // if it's an info box, make it yellow so it stands out more
			if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTBACK)
				Bl2=0.0f ;

		int nBackWidth=(UVCOORD_BACK.right-UVCOORD_BACK.left)*0.5f ;
		offX=(int)(m_GuiTime/1000.0f*flSpeedMult)%nBackWidth ;
		flUV_L=(UVCOORD_BACK.left+offX)/TEXW ;
		flUV_R=(UVCOORD_BACK.left+nBackWidth+offX)/TEXW ;
		flUV_T=(UVCOORD_BACK.top)/TEXH ;
		flUV_B=(UVCOORD_BACK.bottom)/TEXH ;


		if((Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTLJUST))
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd, Gr, Bl) ;







	// add centered text

	float flTextScale=0 ;
	int nSwitch=0 ;
	int nTextWidth=0 ;
	float flTextHeight=0 ;
	float flGapX=0 ;
	float flGapY=0 ;

	// text colour, except for active edit boxes
	if((Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXT) || (Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTLJUST) || (Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTBACK))
	{
		Rd=Menu[nMenu].Button[nButton].R/255.0f ; 
		Gr=Menu[nMenu].Button[nButton].G/255.0f ; 
		Bl=Menu[nMenu].Button[nButton].B/255.0f ;
	}
	else
	switch(nActive)
	{
		case BUTTONSTATE_INACTIVE:	Rd=0.0f ; Gr=1.0f ; Bl=0.0f ; break ;
		case BUTTONSTATE_HOVER:			Rd=1.0f ; Gr=1.0f ; Bl=1.0f ; break ;
		case BUTTONSTATE_PRESSED:		Rd=1.0f ; Gr=1.0f ; Bl=1.0f ; break ;
	}


	if(nEditBox)
	{
		// switch 0 is the edit box name, switch 1 is the editable text
		flTextScale= BUTTONEDITTEXTSCALE ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextMED(Menu[nMenu].Button[nButton].Label, flPosX+32, flPosY+2, flTextScale, 1.0, true, 1, 1, 1) ;
			PrintTextMED(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+11, flTextScale, 1.0, false, Rd, Gr, Bl) ;
		}

		// draw the text
		if((m_nEditBoxMenu==nMenu) && (m_nEditBoxButton==nButton)) // box is being edited now
		{
			PrintTextMED(m_chEditBoxMessage, flButtonEdgeL+1, flPosY+11, flTextScale, 1.0, false, 1, 1, 1) ;
			
			// add the blinking curson
			char chTempMessage[ABSOLUTE_MAX_BUTTON_TEXT] ;
			strcpy(chTempMessage, m_chEditBoxMessage) ;
			chTempMessage[m_nEditBoxPos]='\0' ;
			int nPartSpan=PrintTextMED(chTempMessage, flButtonEdgeL+1, flPosY+11, flTextScale, 1.0, true, 1, 1, 1) ;

			// get the relative width of the character we are under
			float flCharSpan=0 ;
			if(m_chEditBoxMessage[m_nEditBoxPos]=='\0') 
				flCharSpan=1.0 ;
			else
			{
				char chCharacter[2] ;
				chCharacter[0]=m_chEditBoxMessage[m_nEditBoxPos] ;
				chCharacter[1]='\0' ;
				float flChar=PrintTextMED(chCharacter, flButtonEdgeL+1, flPosY+11, flTextScale, 1.0, true, 1, 1, 1) ;
				chCharacter[0]='_' ;
				chCharacter[1]='\0' ;
				float flUnder=PrintTextMED(chCharacter, flButtonEdgeL+1, flPosY+11, flTextScale, 1.0, true, 1, 1, 1) ;

				// we now have the width of the character we are underlining and the width of an underscore
				flCharSpan=flChar/flUnder ;
			}

			PrintTextMED("_", flButtonEdgeL+1+nPartSpan, flPosY+12, flTextScale*flCharSpan, 1.0, false, 1, 1, 1) ;

		}
		else
		{

			PrintTextMED(Menu[nMenu].Button[nButton].Name[0], flButtonEdgeL+1, flPosY+11, flTextScale, 1.0, false, Rd, Gr, Bl) ;
		}
	}
	else
	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_CHECKBOX)
	{
		// switch 0 is the edit box name
		flTextHeight=(flTextCoordsLRG['A'][3] - flTextCoordsLRG['A'][1]) * BUTTONTEXTSCALE ;
		flGapY=flFrameSizeY-flTextHeight ;
		flGapY=flGapY/2.0f+1.0f ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextLRG(Menu[nMenu].Button[nButton].Label, 0, 0, BUTTONTEXTSCALE, BUTTONTEXTSCALE, true, 1, 1, 1) ;
			PrintTextLRG(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+flGapY, BUTTONTEXTSCALE, BUTTONTEXTSCALE, false, Rd, Gr, Bl) ;
		}

		// button face 
		float flCheckSizeX=flButtonSizeX*0.667 ;
		float flCheckSizeY=flButtonSizeY*0.667 ;
		flButtonL= ( flPosX	-VW + (flFrameSizeX-flCheckSizeX)*0.5f)/VW ;
		flButtonR= ( flPosX + flFrameSizeX	-VW  - (flFrameSizeX-flCheckSizeX)*0.5f)/VW ;
		flButtonT=-( flPosY	-VH + (flFrameSizeY-flCheckSizeY)*0.5f)/VH ;
		flButtonB=-( flPosY + flFrameSizeY	-VH - (flFrameSizeY-flCheckSizeY)*0.5f)/VH ;

		flUV_L=UVCOORD_BLANK.left/TEXW ;
		flUV_R=UVCOORD_BLANK.right/TEXW ;
		flUV_T=UVCOORD_BLANK.top/TEXH ;
		flUV_B=UVCOORD_BLANK.bottom/TEXH ;

		if(Menu[nMenu].Button[nButton].Switch)
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0, 1, 0) ;
		else
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0, 0, 0) ;
	}
	else
	{
		flTextHeight=(flTextCoordsLRG['A'][3] - flTextCoordsLRG['A'][1]) * BUTTONTEXTSCALE ;
		flGapY=flFrameSizeY-flTextHeight ;
		flGapY=flGapY/2.0f+1.0f ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextLRG(Menu[nMenu].Button[nButton].Label, 0, 0, BUTTONTEXTSCALE, BUTTONTEXTSCALE, true, 1, 1, 1) ;
			PrintTextLRG(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+flGapY, BUTTONTEXTSCALE, BUTTONTEXTSCALE, false, Rd, Gr, Bl) ;
		}

		nSwitch=Menu[nMenu].Button[nButton].Switch ;
		nTextWidth=PrintTextLRG(Menu[nMenu].Button[nButton].Name[nSwitch], 0, 0, BUTTONTEXTSCALE, BUTTONTEXTSCALE, true, 1, 1, 1) ;
		flGapX=flFrameSizeX-nTextWidth ;
		PrintTextLRG(Menu[nMenu].Button[nButton].Name[nSwitch], flPosX+flGapX/2.0f, flPosY+flGapY, BUTTONTEXTSCALE, BUTTONTEXTSCALE, false, Rd, Gr, Bl) ;
	}

}



// button
void Ogre2dManager::DrawButtonSimple(int nMenu, int nButton) 
{
	int nDebug=0 ;
	if((nMenu==MENU_INFOBOX) && (nButton==MENU_INFOBOX_MESSAGE))
	{
		nDebug=1 ;
		//return ;
	}

	float flMenuCentreX=(Menu[nMenu].L+Menu[nMenu].R)/2.0f ;
	float flMenuCentreY=(Menu[nMenu].U+Menu[nMenu].D)/2.0f ;

	float flPosX=Menu[nMenu].L+Menu[nMenu].Button[nButton].PositionX ;
	float flPosY=Menu[nMenu].U+Menu[nMenu].Button[nButton].PositionY ;
	int nActive=Menu[nMenu].Button[nButton].State ;

		// is this an editbox?
	int nEditBox=0 ;
	float flFrameSizeX=Menu[nMenu].Button[nButton].FrameSizeX  ;
	float flButtonSizeX=Menu[nMenu].Button[nButton].SizeX  ;
	float flFrameSizeY=Menu[nMenu].Button[nButton].FrameSizeY  ;
	float flButtonSizeY=Menu[nMenu].Button[nButton].SizeY  ;

	float flTextHeight=1.0f ;

	switch(Menu[nMenu].Button[nButton].Action)
	{
		case BUTTONACTION_EDIT_TEXT:
			nEditBox=BUTTONACTION_EDIT_TEXT ;
			break ;

		case BUTTONACTION_EDIT_INTEGER:
			nEditBox=BUTTONACTION_EDIT_INTEGER ;
			break ;

		case BUTTONACTION_EDIT_FLOAT:
			nEditBox=BUTTONACTION_EDIT_FLOAT ;
			break ;

		default:
			nEditBox=0 ;
	}


	float flScreenX=m_flViewportSizeX ;
	float flScreenY=m_flViewportSizeY ;

	float VW=VSCREEN_W/2.0f ;
	float VH=VSCREEN_H/2.0f ;

	

	float flFrameGapX=(flFrameSizeX-flButtonSizeX)/2.0 ;
	float flFrameGapY=(flFrameSizeY-flButtonSizeY)/2.0 ;
	float flButtonEdgeL=flPosX	+ flFrameGapX ;
	
	float flButtonL=0.0f ;
	float flButtonR=0.0f ;
	float flButtonT=0.0f ;
	float flButtonB=0.0f ;

	float Rd=0.0f ;
	float Bl=0.0f ;
	float Gr=0.0f ;
	float Rd2=0.0f ;
	float Bl2=0.0f ;
	float Gr2=0.0f ;

	float offX=0.0f ;

	float flUV_L=0.0f ;
	float flUV_R=0.0f ;
	float flUV_T=0.0f ;
	float flUV_B=0.0f ;
	float flWave=0.0f ;





	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// draw button frame
	
	flButtonL= ( flPosX	-VW)/VW ;
	flButtonR= ( flPosX + flFrameSizeX	-VW)/VW ;
	flButtonT=-( flPosY	-VH)/VH ;
	flButtonB=-( flPosY + flFrameSizeY	-VH)/VH ;


	flUV_L=UVCOORD_BLANK.left/TEXW ;
	flUV_R=UVCOORD_BLANK.right/TEXW ;
	flUV_T=UVCOORD_BLANK.top/TEXH ;
	flUV_B=UVCOORD_BLANK.bottom/TEXH ;

	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN)
		switch(nActive)
		{
			case BUTTONSTATE_INACTIVE:	Rd2=0.0f ;	Gr2=0.0f ;	Bl2=0.0f ; break ;
			case BUTTONSTATE_HOVER:			Rd2=0.35f ;	Gr2=0.35f ;	Bl2=0.35f ; break ;
			case BUTTONSTATE_PRESSED:		Rd2=0.85f ;	Gr2=0.85f ;	Bl2=0.85f ; break ;
		}
	else
		switch(nActive)
		{
			case BUTTONSTATE_INACTIVE:	Rd2=0.45f ;	Gr2=0.45f ;	Bl2=0.45f ; break ;
			case BUTTONSTATE_HOVER:			Rd2=0.75f ;	Gr2=0.75f ;	Bl2=0.75f ; break ;
			case BUTTONSTATE_PRESSED:		Rd2=0.85f ;	Gr2=0.85f ;	Bl2=0.85f ; break ;
		}

	if((m_nEditBoxMenu==nMenu) && (m_nEditBoxButton==nButton)) {	Rd2=1.0 ; Gr2=1.0 ; Bl2=0.0 ; }

	// left justified text has a dark background but doesn't react to being hovered over.
	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTLJUST)
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0.0f, 0.0f, 0.0f) ;
	else
	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_NOCLICK)
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0.2f, 0.2f, 0.2f) ;
	else
	if((Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTBACK))
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;

	// end draw frame
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	





	/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// draw button face 
	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_COLUMNSLIDER)
	{
		flButtonL= ( flPosX	-VW + (flFrameSizeX-flButtonSizeX)*0.5f)/VW ;
		flButtonR= ( flPosX + flFrameSizeX	-VW  - (flFrameSizeX-flButtonSizeX)*0.5f)/VW ;
		flButtonT=-( flPosY	-VH + (flFrameSizeY-flButtonSizeY)*0.5f)/VH ;
		flButtonB=-( flPosY + flFrameSizeY	-VH - (flFrameSizeY-flButtonSizeY)*0.5f)/VH ;
		
		Rd2=0.0f ;	Gr2=0.0f ;	Bl2=0.0f ;
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;
		
		// if the button isn't pressed anymore, clear the slider info
		if(nActive==BUTTONSTATE_INACTIVE) 
		{
			Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]=0 ;
			Menu[nMenu].Button[nButton].Info[SLIDERINFO_TIME]=0 ;
		}
		
		// draw up and down buttons
		float flButtonSize=SLIDERBUTTONSIZE/VH ;
		if(Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]==SLIDERDRAWSTATE_UP)
			{Rd2=0.50f ;	Gr2=0.50f ;	Bl2=0.50f ;}
		else
			{Rd2=0.20f ;	Gr2=0.20f ;	Bl2=0.20f ;}
		AddSprite(flButtonL, flButtonT, flButtonR, flButtonT-flButtonSize, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;
		
		if(Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]==SLIDERDRAWSTATE_DOWN)
			{Rd2=0.50f ;	Gr2=0.50f ;	Bl2=0.50f ;}
		else
			{Rd2=0.20f ;	Gr2=0.20f ;	Bl2=0.20f ;}
		AddSprite(flButtonL, flButtonB+flButtonSize, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;
		
		float flTop=flButtonT-flButtonSize-flButtonSize/2.0f ;
		float flBot=flButtonB+flButtonSize+flButtonSize/2.0f ;
		float flSpan=flBot-flTop ;
		float flPos=Menu[nMenu].Button[nButton].MinVal*flSpan+flTop ;
		
		if(Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]==SLIDERDRAWSTATE_MOVE)
			{Rd2=0.50f ;	Gr2=0.50f ;	Bl2=0.50f ;}
		else
			{Rd2=0.35f ;	Gr2=0.35f ;	Bl2=0.35f ;}
		AddSprite(flButtonL, flPos-flButtonSize/2.0f, flButtonR, flPos+flButtonSize/2.0f, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;
		
		
		
		
	}
	else
	{
		flButtonL= ( flPosX	-VW + (flFrameSizeX-flButtonSizeX)*0.5f)/VW ;
		flButtonR= ( flPosX + flFrameSizeX	-VW  - (flFrameSizeX-flButtonSizeX)*0.5f)/VW ;
		flButtonT=-( flPosY	-VH + (flFrameSizeY-flButtonSizeY)*0.5f)/VH ;
		flButtonB=-( flPosY + flFrameSizeY	-VH - (flFrameSizeY-flButtonSizeY)*0.5f)/VH ;
		
		switch(nActive)
		{
			case BUTTONSTATE_INACTIVE:	Rd2=0.20f ;	Gr2=0.20f ;	Bl2=0.20f ; break ;
			case BUTTONSTATE_HOVER:			Rd2=0.35f ;	Gr2=0.35f ;	Bl2=0.35f ; break ;
			case BUTTONSTATE_PRESSED:		Rd2=0.50f ;	Gr2=0.50f ;	Bl2=0.50f ; break ;
		}

		// if it's a list, make it cyan so it stands out more
		if((Menu[nMenu].Button[nButton].Action==BUTTONACTION_LIST) || (nMenu==MENU_LISTBOX))
			Rd2=0.0f ;
		else // if it's an info box, make it yellow so it stands out more
			if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTBACK)
				Bl2=0.0f ;
			else
				if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK)
					{	Rd2=0.0 ; Gr2=0.0 ; Bl2=0.0 ; }

		
		if((m_nEditBoxMenu==nMenu) && (m_nEditBoxButton==nButton)) 
		{	Rd2=0.1 ; Gr2=0.1 ; Bl2=0.0 ; }
		else
			if(nEditBox)
				{	Rd2=0.0 ; Gr2=0.0 ; Bl2=0.0 ; }

		// clear the background
		if((Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTLJUST) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTCOLUMN) && (Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTCOLUMN_NOCLICK))
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, Rd2, Gr2, Bl2) ;
		
	} // end if not a slider
	
	// end draw face
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	
	
	
	
	
	
	// add text

	int nSwitch=0 ;
	int nTextWidth=0 ;
	float flGapX=0 ;
	float flGapY=0 ;
	
	bool bLJust=false ;

	// text colour 
	if(
				(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXT)
			||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTLJUST)
			||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTBACK)
			||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN)
			||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_NOCLICK)
			||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK)
		)
	{
		Rd=Menu[nMenu].Button[nButton].R/255.0f ; 
		Gr=Menu[nMenu].Button[nButton].G/255.0f ; 
		Bl=Menu[nMenu].Button[nButton].B/255.0f ;
	}
	else
	switch(nActive)
	{
		case BUTTONSTATE_INACTIVE:	Rd=0.90f ; Gr=0.90f ; Bl=0.90f ; break ;
		case BUTTONSTATE_HOVER:			Rd=1.0f ; Gr=1.0f ; Bl=1.0f ; break ;
		case BUTTONSTATE_PRESSED:		Rd=1.0f ; Gr=1.0f ; Bl=1.0f ; break ;
	}

	


	if(nEditBox)
	{

		// switch 0 is the edit box name, switch 1 is the editable text
		flTextHeight=(flTextCoordsMED['A'][3] - flTextCoordsMED['A'][1]) * m_flTextScaleMedY ;
		flGapY=flFrameSizeY-flTextHeight ;
		flGapY=flGapY/2.0f+1.0f ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextMED(Menu[nMenu].Button[nButton].Label, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
			PrintTextMED(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		}

		// draw the text
		if((m_nEditBoxMenu==nMenu) && (m_nEditBoxButton==nButton)) // box is being edited now
		{
			PrintTextMED(m_chEditBoxMessage, flButtonEdgeL+1, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, 1, 1, 1) ;
			
			// add the blinking curson
			char chTempMessage[ABSOLUTE_MAX_BUTTON_TEXT] ;
			strcpy(chTempMessage, m_chEditBoxMessage) ;
			chTempMessage[m_nEditBoxPos]='\0' ;
			int nPartSpan=PrintTextMED(chTempMessage, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;

			// get the relative width of the character we are under
			float flCharSpan=0 ;
			if(m_chEditBoxMessage[m_nEditBoxPos]=='\0') 
				flCharSpan=1.0 ;
			else
			{
				char chCharacter[2] ;
				chCharacter[0]=m_chEditBoxMessage[m_nEditBoxPos] ;
				chCharacter[1]='\0' ;
				float flChar=PrintTextMED(chCharacter, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
				chCharacter[0]='_' ;
				chCharacter[1]='\0' ;
				float flUnder=PrintTextMED(chCharacter, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;

				// we now have the width of the character we are underlining and the width of an underscore
				flCharSpan=flChar/flUnder ;
			}

			PrintTextMED("_", flButtonEdgeL+1+nPartSpan, flPosY+flGapY, m_flTextScaleMedX*flCharSpan, m_flTextScaleMedY, false, 1, 1, 1) ;

		}
		else
		{

			PrintTextMED(Menu[nMenu].Button[nButton].Name[0], flButtonEdgeL+1, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		}
	}
	else
	if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_CHECKBOX)
	{
		// switch 0 is the edit box name
		flTextHeight=(flTextCoordsMED['A'][3] - flTextCoordsMED['A'][1]) * m_flTextScaleMedY ;
		flGapY=flFrameSizeY-flTextHeight ;
		flGapY=flGapY/2.0f+1.0f ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextMED(Menu[nMenu].Button[nButton].Label, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
			PrintTextMED(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		}

		// button face 
		float flCheckSizeX=flButtonSizeX*0.667 ;
		float flCheckSizeY=flButtonSizeY*0.667 ;
		flButtonL= ( flPosX	-VW + (flFrameSizeX-flCheckSizeX)*0.5f)/VW ;
		flButtonR= ( flPosX + flFrameSizeX	-VW  - (flFrameSizeX-flCheckSizeX)*0.5f)/VW ;
		flButtonT=-( flPosY	-VH + (flFrameSizeY-flCheckSizeY)*0.5f)/VH ;
		flButtonB=-( flPosY + flFrameSizeY	-VH - (flFrameSizeY-flCheckSizeY)*0.5f)/VH ;

		if(Menu[nMenu].Button[nButton].Switch)
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 1, 1, 1) ;
		else
			AddSprite(flButtonL, flButtonT, flButtonR, flButtonB, flUV_L, flUV_T, flUV_R, flUV_B, 0, 0, 0) ;
	}
	else
	{
		flTextHeight=(flTextCoordsMED['A'][3] - flTextCoordsMED['A'][1]) * m_flTextScaleMedY ;
		flGapY=flFrameSizeY-flTextHeight ;
		flGapY=flGapY/2.0f+1.0f ;

		// draw the name
		if(Menu[nMenu].Button[nButton].Label[0]!='\0')
		{
			nTextWidth=PrintTextMED(Menu[nMenu].Button[nButton].Label, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
			PrintTextMED(Menu[nMenu].Button[nButton].Label, flPosX-nTextWidth-4, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		}
		
		nSwitch=Menu[nMenu].Button[nButton].Switch ;
		// left justified text is a special case
		if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTLJUST)
			PrintTextMED(Menu[nMenu].Button[nButton].Name[nSwitch], flPosX, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		else
		
		if(
					(
							(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN)
						||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_NOCLICK)
						||(Menu[nMenu].Button[nButton].Action==BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK)
					)
				&&(Menu[nMenu].Button[nButton].Name[0][0]!='\0'))
		{

			

			// column text is a lot more complex.  All the column names are stored together in the first name, separated by ~
			// each seperate name must be drawn at a different X location.
			
			// this is kinda a slow way to do things, since I work out all the sub-positions and copy strings every time the control is drawn, for every single textline in the control.
			// the alternative is to store that info somewhere, or store each name in it's own switch.  That would be much faster, but take more memory.
			// I decided since a control like this isn't drawn that often, and probably never during time-critical type stuff like gameplay rendering, it is better to save memory.
			
			int nSpecialControl=Menu[nMenu].Button[nButton].SpecialControl ;
			int nMaxName=Menu[nMenu].SpecialControl[nSpecialControl].ColumnBoxInfo.MaxColumn ;
			
			// if the specialcontrol info has just changed, momentarily display darker text
			if(m_GuiTime<Menu[nMenu].SpecialControl[nSpecialControl].ColumnBoxInfo.FillInfo.NextClickableTime)
			{
				Rd/=2.0f ;
				Gr/=2.0f ;
				Bl/=2.0f ;
			}

			int nPos=-1 ;
			char chTemp[ABSOLUTE_MAX_BUTTON_TEXT] ;
			float flOffsetX=0.0f ;
			for(int nName=0 ; nName<nMaxName ; nName++)
			{
				// copy the name to the temp
				int nSubPos=0 ;
				nPos++ ; // skip past last ~ (or move to pos 0 if first name)
				char chChar=Menu[nMenu].Button[nButton].Name[0][nPos] ;
				int nShiftText=0 ; // use the '<' character at the beginning of a column name to move the text centred left.
				do
				{
					if(chChar=='<') // shift text 1/2 over 
						nShiftText=2 ;
					else
					if(chChar=='`')	// shift text 1/3 over
						nShiftText=3 ;
					else
					{
						chTemp[nSubPos]=chChar ;
						nSubPos++ ;
					}

					// get the next char		
					nPos++ ;
					chChar=Menu[nMenu].Button[nButton].Name[0][nPos] ;
				
				}while((chChar!='~') && (chChar!='\0')) ;
				
				chTemp[nSubPos]='\0' ;
				if(nShiftText>0)
				{
					int nWidth=PrintTextMED(chTemp, flPosX+flOffsetX, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, true, Rd, Gr, Bl) ;
					nWidth/=nShiftText ;
					PrintTextMED(chTemp, flPosX+flOffsetX-nWidth, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
				}
				else
					PrintTextMED(chTemp, flPosX+flOffsetX, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
				flOffsetX+=Menu[nMenu].SpecialControl[nSpecialControl].ColumnBoxInfo.ColumnSizeX[nName] ; // shift to next position
			}// end for nName
			//}
		}
		else
		{		
			nTextWidth=PrintTextMED(Menu[nMenu].Button[nButton].Name[nSwitch], 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
			flGapX=flFrameSizeX-nTextWidth ;	
			PrintTextMED(Menu[nMenu].Button[nButton].Name[nSwitch], flPosX+flGapX/2.0f, flPosY+flGapY, m_flTextScaleMedX, m_flTextScaleMedY, false, Rd, Gr, Bl) ;
		}
	}

}



// setup menu prior to first drawing
void Ogre2dManager::InitMenu(int nMenu)
{
	if(nMenu==MENU_NONE) return ;


	int nMaxButton=Menu[nMenu].Button.size() ;
	int nButton=0 ;

	for(nButton=0 ; nButton<nMaxButton ; nButton++)
		Menu[nMenu].Button[nButton].State=BUTTONSTATE_INACTIVE ;

}


// the list box is just a normal menu of buttons, reserved as the last menu so that, if it is visible, it is always drawn on top of all others and always the one to activate if
// the mouse is over it.  When another button calls up the list box, this routine fills the list box with options taken from the calling button's switch names.  
// When a selection is made, that data is fed back to the calling button, setting its switch to the desired selection, and the listbox is made invisible again.
void Ogre2dManager::SetupListBoxMenu()
{
	
	// default all buttons to not visible
	for(int nButton=0 ; nButton<MAX_BUTTON_SWITCH ; nButton++)
		Menu[MENU_LISTBOX].Button[nButton].Visible=false ;


	// fill in the buttons
	int nMaxButton=Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].MaxSwitch ;
	int nStyle=Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].Style ;
	float flTextScale=1.0f ;
	float flTextHeight=1.0f ;

	if(nStyle==BUTTONSTYLE_SIMPLE)
	{
		flTextScale=m_flTextScaleMedY ;
		flTextHeight=(flTextCoordsMED['A'][3] - flTextCoordsMED['A'][1]) * flTextScale ;
	}
	else
	if(nStyle==BUTTONSTYLE_FANCY)
	{
		flTextScale= BUTTONTEXTSCALE ;
		flTextHeight=(flTextCoordsLRG['A'][3] - flTextCoordsLRG['A'][1]) * flTextScale ;
	}

	flTextHeight=floor(flTextHeight) ;
	float flButtonSizeY=flTextHeight+2.0f ;
	flButtonSizeY=floor(flButtonSizeY) ;
	float flMenuHeight=flButtonSizeY*nMaxButton ;

	float flFrameY=Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].FrameSizeY-Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].SizeY ;

	// setup menu
	Menu[MENU_LISTBOX].Visible=true ;
	Menu[MENU_LISTBOX].L=Menu[m_nQueueListBoxMenu].L+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].PositionX ;
	Menu[MENU_LISTBOX].R=Menu[MENU_LISTBOX].L+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].FrameSizeX ;

	if(Menu[m_nQueueListBoxMenu].U+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].PositionY<VSCREEN_H/2.0f)
	{
		Menu[MENU_LISTBOX].U=Menu[m_nQueueListBoxMenu].U+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].PositionY+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].FrameSizeY+1 ;
		Menu[MENU_LISTBOX].D=Menu[MENU_LISTBOX].U+flMenuHeight+flFrameY ;

		if(Menu[MENU_LISTBOX].D>=VSCREEN_H)
		{
			Menu[MENU_LISTBOX].D=VSCREEN_H-1 ;
			Menu[MENU_LISTBOX].U=Menu[MENU_LISTBOX].D-flMenuHeight-flFrameY ;
		}
	}
	else
	{
		Menu[MENU_LISTBOX].D=Menu[m_nQueueListBoxMenu].U+Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].PositionY ;
		Menu[MENU_LISTBOX].U=Menu[MENU_LISTBOX].D-flMenuHeight-flFrameY ;

		if(Menu[MENU_LISTBOX].U<0)
		{
			Menu[MENU_LISTBOX].U=0 ;
			Menu[MENU_LISTBOX].D=Menu[MENU_LISTBOX].U+flMenuHeight+flFrameY ;
		}
	}

	// setup the buttons
	for(int nButton=0 ; nButton<nMaxButton ; nButton++)
	{
		strcpy(Menu[MENU_LISTBOX].Button[nButton].Name[0], Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].Name[nButton]) ;
		Menu[MENU_LISTBOX].Button[nButton].Visible=true ;
		Menu[MENU_LISTBOX].Button[nButton].PositionX=flFrameY/2.0f ;
		Menu[MENU_LISTBOX].Button[nButton].PositionY=nButton*flButtonSizeY+flFrameY/2.0f ;
		Menu[MENU_LISTBOX].Button[nButton].SizeX=Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].SizeX ;
		Menu[MENU_LISTBOX].Button[nButton].SizeY=flButtonSizeY+1.0f ;
		Menu[MENU_LISTBOX].Button[nButton].FrameSizeX=Menu[m_nQueueListBoxMenu].Button[m_nQueueListBoxButton].SizeX ;
		Menu[MENU_LISTBOX].Button[nButton].FrameSizeY=flButtonSizeY+1.0f ;
		Menu[MENU_LISTBOX].Button[nButton].Style=nStyle ;
	}

	m_nListBoxMenu=m_nQueueListBoxMenu ;
	m_nListBoxButton=m_nQueueListBoxButton ;

	m_nQueueListBoxMenu=MENU_NONE ;
	InitMenu(MENU_LISTBOX) ;
}


// set folder pointer to null to disable changing folder
void Ogre2dManager::SetupSelectorMenu(char *pMenuName, char *pFolder, char *pFilename, std::vector<FILENAME> &InFileName, int nFirstFile)
{
	Menu[MENU_SELECTOR].Visible=true ;

	if(strlen(pMenuName)<Menu[MENU_SELECTOR].Button[MENU_SELECTOR_NAME].NameMem[0])
		strcpy(Menu[MENU_SELECTOR].Button[MENU_SELECTOR_NAME].Name[0], pMenuName) ;

	if(pFolder==NULL)
	{
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_FOLDER].Visible=false ;
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_PARENT].Visible=false ;
	}
	else
	{
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_FOLDER].Visible=true ;
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_PARENT].Visible=true ; 
		if(strlen(pFolder)<Menu[MENU_SELECTOR].Button[MENU_SELECTOR_FOLDER].NameMem[0])
			strcpy(Menu[MENU_SELECTOR].Button[MENU_SELECTOR_FOLDER].Name[0], pFolder) ;
	}

	if(strlen(pFilename)<Menu[MENU_SELECTOR].Button[MENU_SELECTOR_EDIT].NameMem[0])
		strcpy(Menu[MENU_SELECTOR].Button[MENU_SELECTOR_EDIT].Name[0], pFilename) ;



	// default all the file selector buttons to not visible
	for(int nButton=0 ; nButton<MAX_SELECTOR_OPTION ; nButton++)
		Menu[MENU_SELECTOR].Button[nButton+MENU_SELECTOR_FIRSTOPTION].Visible=false ;

	if(nFirstFile>0)
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_SCROLLUP].Visible=true ;
	else
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_SCROLLUP].Visible=false ;

	// fill in the button names
	int nMaxName=InFileName.size() ;
	int nMaxButton=nMaxName-nFirstFile ;
	if(nMaxButton>MAX_SELECTOR_OPTION) 
	{
		nMaxButton=MAX_SELECTOR_OPTION ;
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_SCROLLDOWN].Visible=true ;
	}
	else
		Menu[MENU_SELECTOR].Button[MENU_SELECTOR_SCROLLDOWN].Visible=false ;

	char chMessage[MISCSTRINGSIZE] ;

	for(int nButton=0 ; nButton<nMaxButton ; nButton++)
	{
		sprintf(chMessage, "%i", nButton+1+nFirstFile) ;
		strcpy(Menu[MENU_SELECTOR].Button[nButton+MENU_SELECTOR_FIRSTOPTION].Label, chMessage) ;
		if(strlen(InFileName[nFirstFile+nButton].Filename)<Menu[MENU_SELECTOR].Button[nButton+MENU_SELECTOR_FIRSTOPTION].NameMem[0])
			strcpy(Menu[MENU_SELECTOR].Button[nButton+MENU_SELECTOR_FIRSTOPTION].Name[0], InFileName[nFirstFile+nButton].Filename) ;
		Menu[MENU_SELECTOR].Button[nButton+MENU_SELECTOR_FIRSTOPTION].Visible=true ;
	}
}




void Ogre2dManager::HideInfoBoxMenu() { Menu[MENU_INFOBOX].Visible=false ; }

bool Ogre2dManager::ShowInfoBoxMenu(int nType, int nStyle, char *pMessage)
{
	// if an edit box is active, deactivate it.
	if(m_nEditBoxMenu!=MENU_NONE) 
	{
		FinishEditBox() ;
		m_nEditBoxMenu=MENU_NONE ;
	}



	// if the message box is already visible, we don't show another one and return false
	if(Menu[MENU_INFOBOX].Visible) return false ;




	if(strlen(pMessage)>ABSOLUTE_MAX_BUTTON_TEXT) // message is too long
		pMessage[MAX_BUTTON_TEXT-1]='\0' ;	

	if(pMessage[0]=='\0')
		strcpy(pMessage, "Message Menu") ; // default message if none supplied


	// default all buttons to not visible
	for(int nButton=0 ; nButton<MENU_INFOBOX_MAX ; nButton++)
	{
		Menu[MENU_INFOBOX].Button[nButton].Visible=false ;
		
		
	}
	// debug
	
	int Rcol=Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].R ;
	int Gcol=Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].G ;
	int Bcol=Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].B ;
	

	
	// width of message
	int nMessageWidth=0 ;
	int nMinMessageWidth=0 ;
	int nGap=0 ;
	int nTextHeight=0 ;
	if(nStyle==BUTTONSTYLE_FANCY)
	{
		nMessageWidth=PrintTextLRG(pMessage, 0, 0, BUTTONTEXTSCALE, BUTTONTEXTSCALE, true, 1, 1, 1) ;
		nMinMessageWidth=PrintTextLRG("Message Menu", 0, 0, BUTTONTEXTSCALE, BUTTONTEXTSCALE, true, 1, 1, 1) ;
		if(nMessageWidth<nMinMessageWidth)
			nMessageWidth=nMinMessageWidth ;
		nGap=(flTextCoordsLRG['W'][2] - flTextCoordsLRG['W'][0]) * BUTTONTEXTSCALE ;
		nTextHeight=(flTextCoordsLRG['A'][3] - flTextCoordsLRG['A'][1]) * BUTTONTEXTSCALE ;

	}
	else
	if(nStyle==BUTTONSTYLE_SIMPLE)
	{
		nMessageWidth=PrintTextMED(pMessage, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
		nMinMessageWidth=PrintTextMED("Message Menu", 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
		if(nMessageWidth<nMinMessageWidth)
			nMessageWidth=nMinMessageWidth ;
		nGap=(flTextCoordsMED['W'][2] - flTextCoordsMED['W'][0]) * m_flTextScaleMedX ;
		nTextHeight=(flTextCoordsMED['A'][3] - flTextCoordsMED['A'][1]) * m_flTextScaleMedY ;
	}

	// set the messagemenu box size
	Menu[MENU_INFOBOX].L=VSCREEN_W/2-(nMessageWidth+nGap)/2 ;
	Menu[MENU_INFOBOX].R=VSCREEN_W/2+(nMessageWidth+nGap)/2 ;
	Menu[MENU_INFOBOX].U=VSCREEN_H/2-(nGap*2) ;
	Menu[MENU_INFOBOX].D=VSCREEN_H/2+(nGap*3) ;

	Menu[MENU_INFOBOX].Visible=true ;

		// copy the text
	SetButtonString(MENU_INFOBOX, MENU_INFOBOX_MESSAGE, 0, pMessage) ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].Visible=true ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].SizeX=nMessageWidth ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].SizeY=nTextHeight ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].FrameSizeX=nMessageWidth ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].FrameSizeY=nTextHeight ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].PositionX=nGap/2 ;
	Menu[MENU_INFOBOX].Button[MENU_INFOBOX_MESSAGE].PositionY=nGap/2 ;

	if(nType==MESSAGEBOX_TYPE_OK) // ok box
	{
		SetButtonString(MENU_INFOBOX, MENU_INFOBOX_YES, 0, "OK") ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].Visible=true ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].SizeX=nGap*3 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].SizeY=nTextHeight ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].FrameSizeX=nGap*3+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].FrameSizeY=nTextHeight+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].PositionX=nMessageWidth+nGap-(nGap*3+2+2) ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].PositionY=nGap*2.5 ;
	}
	else // yes/no box
	{
		SetButtonString(MENU_INFOBOX, MENU_INFOBOX_YES, 0, "YES") ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].Visible=true ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].SizeX=nGap*3 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].SizeY=nTextHeight ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].FrameSizeX=nGap*3+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].FrameSizeY=nTextHeight+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].PositionX=nMessageWidth+nGap-(nGap*3+2+2) ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_YES].PositionY=nGap*3-2 ;

		SetButtonString(MENU_INFOBOX, MENU_INFOBOX_NO, 0, "NO") ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].Visible=true ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].SizeX=nGap*3 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].SizeY=nTextHeight ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].FrameSizeX=nGap*3+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].FrameSizeY=nTextHeight+2 ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].PositionX=(2+2) ;
		Menu[MENU_INFOBOX].Button[MENU_INFOBOX_NO].PositionY=nGap*3-2 ;
	}

	return true ;

}

// returns a menu result.
MENURESULT Ogre2dManager::UpdateMenu()
{

	MENURESULT MResult ;
	MResult.Menu=m_nMenuMode ;
	
	MResult.Button=-1 ;
	MResult.Switch=-1 ;
	MResult.SpecialControl=-1 ;
	for(int nInfo=0 ; nInfo<MENURESULTINFOMAX ; nInfo++)
		MResult.Info[nInfo]=-1 ;
	MResult.Result=MENURESULT_NOACTION ;
	MResult.Switch=0 ;

	int nLeftButton=m_nMouseLeft ;
	int nRightButton=m_nMouseRight ;

	// if a listbox is queued, create it.
	if(m_nQueueListBoxMenu!=MENU_NONE)
		SetupListBoxMenu() ;



	FindActiveMenu();
	
	// if menu's have changed, initialize it
	if(m_nMenuMode!=m_nOldMenuMode)
	{
		InitMenu(m_nOldMenuMode) ;
		InitMenu(m_nMenuMode) ;
	}
	
	m_nOldMenuMode=m_nMenuMode ;

	// get out if no menu
	if(m_nMenuMode==MENU_NONE)
	{
		// re-allow activation of last deactivated edit box (used so we can click to activate and de-activate edit boxes)
		//if(!nLeftButton) m_nLastDeactivatedEditBoxMenu=MENU_NONE ;

		// if the list box is active, and the mouse has been pressed anywhere outside the list box, turn it off.
		if((nLeftButton) || (nRightButton))
		{
			InitMenu(MENU_LISTBOX) ;
			SetMenuVisibility(MENU_LISTBOX, false) ;
		}

		return MResult ;
	}

	int nMenu=m_nMenuMode ;

	//////////////////////////////////////////////////////////////////////


	// info about cursor position
	float flScreenX=m_flViewportSizeX ;
	float flScreenY=m_flViewportSizeY ;
	float VW=VSCREEN_W/2.0f ;
	float VH=VSCREEN_H/2.0f ;
	float flIconS=24.0f ;
	float flMPosX=m_flMouseX ;
	float flMPosY=m_flMouseY ;

	float flIconL= ( flMPosX	-VW)/VW ;
	float flIconR= ( flMPosX +flIconS	-VW)/VW ;
	float flIconT=-( flMPosY	-VH)/VH ;
	float flIconB=-( flMPosY +flIconS	-VH)/VH ;


	int nMaxButton=Menu[nMenu].Button.size() ;
	int nButton=0 ;


	//////////////////////////////////////////////////////////////////////
	// sliders can be active even if not over them anymore, as long as mouse button is still down
	if(m_nSliderMenu!=MENU_NONE)
	{
		if((nLeftButton==0) || (!Menu[nMenu].Visible)) // button not pressed or menu not visible
		{
			m_nSliderMenu=MENU_NONE ;
			m_nMenuMode=MENU_NONE ;
			
		}
		else
		{
			nMenu=m_nSliderMenu ;
			nButton=m_nSliderButton ;
			
			// change the button state
			Menu[nMenu].Button[nButton].State=BUTTONSTATE_PRESSED ;
		
		
					float flPosY=Menu[nMenu].U+Menu[nMenu].Button[nButton].PositionY ;
					float flSizeY=Menu[nMenu].Button[nButton].SizeY  ;
					int nConnectionType=Menu[nMenu].Button[nButton].Info[SLIDERINFO_CONNECTIONTYPE] ;
					int nConnection=Menu[nMenu].Button[nButton].Info[SLIDERINFO_CONNECTION] ;
					
					if(
								((flMPosY<flPosY+SLIDERBUTTONSIZE) || (flMPosY>flPosY+flSizeY-SLIDERBUTTONSIZE))
							&&(Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]!=SLIDERDRAWSTATE_MOVE)
						)
					{
						int nMoveDir=-1 ;
						bool bFirstMove=false ;
						if(Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]==0)
							bFirstMove=true ;
						
						if(flMPosY<flPosY+SLIDERBUTTONSIZE)
							Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]=SLIDERDRAWSTATE_UP ;
						else
						{
							Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]=SLIDERDRAWSTATE_DOWN ;
							nMoveDir=1 ;
						}
								
						Menu[nMenu].Button[nButton].Info[SLIDERINFO_TIME]-=m_GuiTimeSinceLastFrame ;
						
						if(Menu[nMenu].Button[nButton].Info[SLIDERINFO_TIME]<=0)
						{
							if(bFirstMove)
								Menu[nMenu].Button[nButton].Info[SLIDERINFO_TIME]=500 ;
							else
								Menu[nMenu].Button[nButton].Info[SLIDERINFO_TIME]=50 ;
								
							switch(nConnectionType)
							{
								case SLIDERCONNECTIONTYPE_COLUMNBOX:
									UpdateColumnBoxStartRow(nMenu, nConnection, Menu[nMenu].SpecialControl[nConnection].ColumnBoxInfo.FillInfo.StartRow+nMoveDir) ;
									break ;
							}// end switch connectiontype
						}
						
					}
					else
					{
						float flSpan=flSizeY-SLIDERBUTTONSIZE*3 ;
						float flPos=flMPosY-flPosY-SLIDERBUTTONSIZE-SLIDERBUTTONSIZE/2 ;
						float flPercent=flPos/flSpan ;
						if(flPercent<0.0f)
							flPercent=0.0f ;
						else
							if(flPercent>1.0f)
								flPercent=1.0f ;
								
						
						
						Menu[nMenu].Button[nButton].MinVal=flPercent ;
						Menu[nMenu].Button[nButton].Info[SLIDERINFO_DRAWSTATE]=SLIDERDRAWSTATE_MOVE ;
						
						switch(nConnectionType)
						{
							case SLIDERCONNECTIONTYPE_COLUMNBOX:
								{
									int nMaxStartRow=Menu[nMenu].SpecialControl[nConnection].ColumnBoxInfo.FillInfo.MaxRow-Menu[nMenu].SpecialControl[nConnection].MaxLine ;
									// if there aren't many entries, just a few more than the box size, the slider can lose some responsiveness.
									// detect this and force either full up or full down at the halfway point, 
									// makes slider work better, otherwise it only scrolls down when slider is right at the bottom.
									int nTestResponse=nMaxStartRow*0.98f ;
									if(nTestResponse==0)
									{
										if(flPercent<0.5f)
											flPercent=0.0f ;
										else
											flPercent=1.0f ;
									}
									
									
									UpdateColumnBoxStartRow(nMenu, nConnection, nMaxStartRow*flPercent) ;
								}
								break ;
						}// end switch connectiontype
					}// end percent slider
					
		}	// end if slider still active
		
		return MResult ;
	}// end if slider
	

	// perform an edit box action if needed
	if(m_nEditBoxMenu!=MENU_NONE)
	{

		// if the mouse is pressed, finish the edit box
		if((nLeftButton) || (nRightButton)) 
		{
			// these two allow us to deactive edit boxes by clicking on them, without retriggering them when we release the mouse
			m_nLastDeactivatedEditBoxMenu=m_nEditBoxMenu ;
			m_nLastDeactivatedEditBoxButton=m_nEditBoxButton ;

			// finish the edit box
			FinishEditBox() ;
		}

		if(m_nEditBoxAction==EDITBOXACTION_FINISHEDINPUT) // finished editing an edit box
		{
			MResult.Result=MENURESULT_BUTTONACTIVATED ;//MENURESULT_EDITBOXENDED ;
			// stop editing
			m_nEditBoxMenu=MENU_NONE ;
			m_nEditBoxAction=EDITBOXACTION_NONE ;
		}
		else
			if(m_nEditBoxAction==EDITBOXACTION_SPECIALINPUT) // finished editing an edit box
			{
				if(strlen(m_chEditBoxMessage)<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].NameMem[0]) 
					strcpy(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Name[0], m_chEditBoxMessage) ;
				MResult.Result=MENURESULT_BUTTONACTIVATED ;//MENURESULT_EDITBOXALTERED ;
				// don't stop editing
			}

		
		MResult.Button=m_nEditBoxButton ;
		return MResult ;
	}
	
	
	
	





	bool bGotButton=false ; // it's possible to be over more than one button if they overlap.  Only treat one as the official active button.


	// process the buttons.  
	// Do it in reverse order, since later buttons are drawn over earlier buttons, and might be on top of each other. We want only the topmost visible button to activate.
	for(nButton=nMaxButton-1 ; nButton>=0 ; nButton--)
	{

		// is the mouse over this button?
		if(
				// check we don't already have an active button
				(bGotButton==false)
				&&
				// check button is visible
				(Menu[nMenu].Button[nButton].Visible==true)
				&&
				// static text cannot be active
				(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXT) 
				&& 
				(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTLJUST) 
				&& 
				(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTBACK)
				&&
				(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTCOLUMN_NOCLICK)
				&&
				(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK)
				&&
				// check bounding rectangle
				(flMPosX>Menu[nMenu].L+Menu[nMenu].Button[nButton].PositionX)
				&&
				(flMPosX<Menu[nMenu].L+Menu[nMenu].Button[nButton].PositionX+Menu[nMenu].Button[nButton].FrameSizeX)
				&&
				(flMPosY>Menu[nMenu].U+Menu[nMenu].Button[nButton].PositionY)
				&&
				(flMPosY<Menu[nMenu].U+Menu[nMenu].Button[nButton].PositionY+Menu[nMenu].Button[nButton].FrameSizeY)
			)
		{
			bGotButton=true ;
			MResult.Button=nButton ;
			MResult.Switch=Menu[nMenu].Button[nButton].Switch ; // might get changed if button is pressed
			MResult.SpecialControl=Menu[nMenu].Button[nButton].SpecialControl ; // any SpecialControl Info?

			if(nLeftButton) // we are on top of button, pressing mouse
			{			
				// change the button state
				Menu[nMenu].Button[nButton].State=BUTTONSTATE_PRESSED ;
				
				// if this button is a slider, set some info
				if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_COLUMNSLIDER)
				{
					m_nSliderMenu=nMenu ;
					m_nSliderButton=nButton ;
				}
				
				
			}
			else // we are on top of button, not pressing mouse
			{
				// was the button pressed before?  If so, we've got an action to do (only do action after mouse is released)
				if(Menu[nMenu].Button[nButton].State==BUTTONSTATE_PRESSED)
				{
						// set button as inactive
						Menu[nMenu].Button[nButton].State=BUTTONSTATE_INACTIVE ;


						if(nMenu==MENU_LISTBOX) // list box, update the button that created it.
						{
							Menu[m_nListBoxMenu].Button[m_nListBoxButton].Switch=nButton ;
							InitMenu(MENU_LISTBOX) ;
							SetMenuVisibility(MENU_LISTBOX, false) ;
							// send this as a MResult, get out
							MResult.Menu=m_nListBoxMenu ;
							MResult.Button=m_nListBoxButton ;
							MResult.Result=MENURESULT_BUTTONACTIVATED ;
							MResult.Switch=nButton ;
							m_nListBoxMenu=MENU_NONE ;
							return MResult ;
						}
						else 
						if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_COLUMNSLIDER) 
						{
							// columnsliders don't send any external MENURESULT_BUTTONACTIVATED message
							// since they are just for the internal control of a columnbox
							// so just return when they are unclicked.
							return MResult ;
						}
						else
						if(Menu[nMenu].Button[nButton].Action==BUTTONACTION_LIST)
						{
							m_nQueueListBoxMenu=nMenu ;
							m_nQueueListBoxButton=nButton ;
							// queuing a list box doesn't count as an action.
						}
						else
						if((Menu[nMenu].Button[nButton].Action==BUTTONACTION_EDIT_TEXT) || (Menu[nMenu].Button[nButton].Action==BUTTONACTION_EDIT_INTEGER) || (Menu[nMenu].Button[nButton].Action==BUTTONACTION_EDIT_FLOAT))// start editing an editbox
						{
							if((m_nLastDeactivatedEditBoxMenu!=nMenu) || (m_nLastDeactivatedEditBoxButton!=nButton)) // don't reactive an edit box we just deactivated
							{
								m_nEditBoxMenu=nMenu ;
								m_nEditBoxButton=nButton ;
								m_nEditBoxAction=EDITBOXACTION_NONE ;
								strcpy(m_chEditBoxMessage, Menu[nMenu].Button[nButton].Name[0]) ;
								m_nEditBoxPos=strlen(m_chEditBoxMessage) ;
							}
							// starting to edit doesn't count as an action.
						}
						else
						{
							// if the button is a switch type, do the switch
							Menu[nMenu].Button[nButton].Switch++ ;
							if(Menu[nMenu].Button[nButton].Switch>=Menu[nMenu].Button[nButton].MaxSwitch) 
								Menu[nMenu].Button[nButton].Switch=0 ;

							// update switch
							MResult.Switch=Menu[nMenu].Button[nButton].Switch ;
							MResult.Result=MENURESULT_BUTTONACTIVATED ;

							
							// handle specialcontrol extra information
							if(Menu[nMenu].Button[nButton].SpecialControl>-1)
							{
								int nSC=Menu[nMenu].Button[nButton].SpecialControl ;
								
								// columnbox
								if(Menu[nMenu].SpecialControl[nSC].Type==SPECIALCONTROL_COLUMNBOX)
								{
									// the info we send back will be the index of the item clicked.
									int nStartRow=Menu[nMenu].SpecialControl[nSC].ColumnBoxInfo.FillInfo.StartRow ;
									int nIndexOffset=nButton-Menu[nMenu].SpecialControl[nSC].ButtonStart ;
									int nIndex=nStartRow+nIndexOffset ;
									
									// if there are less entries in the columnbox, we could be returning an index that is out of bounds.
									// check for that, and if it is the case, don't return any result.
									// Also don't return any result if the columnbox has just changed.
									if(
											(nIndex>=Menu[nMenu].SpecialControl[nSC].ColumnBoxInfo.FillInfo.MaxRow)
											||
											(m_GuiTime<Menu[nMenu].SpecialControl[nSC].ColumnBoxInfo.FillInfo.NextClickableTime)
										)
										MResult.Result=MENURESULT_NOACTION ;
									else
									{
										MResult.Info[MENURESULTINFO_COLUMNBOXINDEX]=nIndex ; // return the index into the fillinfo the user has provided.
										MResult.Info[MENURESULTINFO_COLUMNBOXUPDATE]=Menu[nMenu].SpecialControl[nSC].ColumnBoxInfo.FillInfo.UpdateCount ; // remember the update, so user can check if columbox has changed since they got some data from it.
									}

									// update the feedback if it exists
									if(Menu[nMenu].SpecialControl[nSC].ColumnBoxInfo.Feedback)
									{
										int nFeedbackButton=Menu[nMenu].SpecialControl[nSC].ButtonEnd-3 ;
										strcpy_s(Menu[nMenu].Button[nFeedbackButton].Name[0], ABSOLUTE_MAX_BUTTON_TEXT, Menu[nMenu].Button[nButton].Name[0]) ;
									}

								}// end if columnbox
								

							}// end if button on specialcontrol
							
							

						}// end doing a button toggle or other action

				}// end if doing some button activity
				else
				{
					Menu[nMenu].Button[nButton].State=BUTTONSTATE_HOVER ; // indicate we are hovering over the button
				}
				

			}// end if over button, not pressed
		}
		else
			Menu[nMenu].Button[nButton].State=BUTTONSTATE_INACTIVE ; // we are not over the button (don't care if mouse is pressed or not)


	}

	// re-allow activation of last deactivated edit box (used so we can click to activate and de-activate edit boxes)
	if(!nLeftButton) m_nLastDeactivatedEditBoxMenu=MENU_NONE ;

	// if the list box is active, and the mouse has been pressed anywhere outside the list box, turn it off.
	if(  ((nLeftButton) || (nRightButton)) && (m_nMenuMode!=MENU_LISTBOX))
	{
		InitMenu(MENU_LISTBOX) ;
		SetMenuVisibility(MENU_LISTBOX, false) ;
	}

	return MResult ;

}

void Ogre2dManager::UpdateMouseInput(int nLeft, int nRight, float flPosX, float flPosY, float flWheel)
{
	m_flMouseX=flPosX/m_flViewportSizeX*VSCREEN_W ;
	m_flMouseY=flPosY/m_flViewportSizeY*VSCREEN_H ;

	m_nMouseWheelChange=0 ;
	if(flWheel<m_flMouseZ) 
		m_nMouseWheelChange=-1 ;
	else
	if(flWheel>m_flMouseZ) 
		m_nMouseWheelChange=1 ;


	m_flMouseZ=flWheel ; // mouse wheel
	m_nMouseLeft=nLeft ;
	m_nMouseRight=nRight ;
}

// altering menu visibility will fail if there is an active edit box
int Ogre2dManager::SetMenuVisibility(int nMenu, bool bVisible) 
{ 
	// if the menu in question has an active edit box, don't allow the change and fail.
	if((m_nMenuMode==nMenu) && (m_nEditBoxMenu!=MENU_NONE))
		return 0 ;

	Menu[nMenu].Visible=bVisible ;
	return 1 ;
}
bool Ogre2dManager::GetMenuVisibility(int nMenu) { return Menu[nMenu].Visible ; }

// returns true if any menu is visible, false if none are.
bool Ogre2dManager::AnyMenuIsVisible()
{
	for(int nMenu=0 ; nMenu<Menu.size() ; nMenu++)
		if(Menu[nMenu].Visible) return true ;

	return false ;
}

int Ogre2dManager::FindActiveMenu()
{
	// the active menu is the highest numbered visible one we find under the mouse,
	// unless an edit box is active, in which case it is the one with the edit box
	if((m_nMenuMode!=MENU_NONE) && ((m_nEditBoxMenu!=MENU_NONE) || (m_nSliderMenu!=MENU_NONE)) )
		return m_nMenuMode;

	// if the messagebox is visible, make that the active menu
	if(Menu[MENU_INFOBOX].Visible)
	{
		m_nMenuMode=MENU_INFOBOX ;
		return MENU_INFOBOX ;
	}

	m_nMenuMode=MENU_NONE ;


	float flScreenX=m_flViewportSizeX ;
	float flScreenY=m_flViewportSizeY ;
	float flMPosX=m_flMouseX ;
	float flMPosY=m_flMouseY ;

	for(int nMenu=0 ; nMenu<Menu.size() ; nMenu++)
		if(
				(Menu[nMenu].Visible)
				&& (flMPosX>Menu[nMenu].L) && (flMPosX<Menu[nMenu].R)
				&& (flMPosY>Menu[nMenu].U) && (flMPosY<Menu[nMenu].D)
			)
		{
			m_nMenuMode=nMenu ;
			return m_nMenuMode ;
		}

		return m_nMenuMode ;
}

void Ogre2dManager::PrepareVisibleMenusForRender()
{
	for(int nMenu=MENU_LISTBOX+1 ; nMenu<Menu.size() ; nMenu++)
		if(Menu[nMenu].Visible) DrawMenu(nMenu) ;

	// last of all draw the listbox or messagebox
	if(Menu[MENU_SELECTOR].Visible) DrawMenu(MENU_SELECTOR) ;

	if(Menu[MENU_LISTBOX].Visible) DrawMenu(MENU_LISTBOX) ;

	if(Menu[MENU_INFOBOX].Visible) DrawMenu(MENU_INFOBOX) ;
}

void Ogre2dManager::DrawMenu(int nMenu) 
{


	//////////////////////////////////////////////////////////////////////


	// info about cursor position
	float flScreenX=m_flViewportSizeX ;
	float flScreenY=m_flViewportSizeY ;
	float VW=VSCREEN_W/2.0f ;
	float VH=VSCREEN_H/2.0f ;
	float flIconS=24.0f ;
	float flMPosX=m_flMouseX ;
	float flMPosY=m_flMouseY ;

	float flIconL= ( flMPosX	-VW)/VW ;
	float flIconR= ( flMPosX +flIconS	-VW)/VW ;
	float flIconT=-( flMPosY	-VH)/VH ;
	float flIconB=-( flMPosY +flIconS	-VH)/VH ;


	int nMaxButton=Menu[nMenu].Button.size() ;
	int nButton=0 ;

	if(Menu[nMenu].Back)
	{
		float flMenuL=(Menu[nMenu].L-VW)/VW ;
		float flMenuR= (Menu[nMenu].R	-VW)/VW ;
		float flMenuT=-( Menu[nMenu].U	-VH)/VH ;
		float flMenuB=-(Menu[nMenu].D	-VH)/VH ;

		if(nMenu==MENU_LISTBOX)
			AddSprite(flMenuL, flMenuT, flMenuR, flMenuB, UVCOORD_BLANK.left/TEXW, UVCOORD_BLANK.top/TEXH, UVCOORD_BLANK.right/TEXW, UVCOORD_BLANK.bottom/TEXH, 0.85f, 0.85f, 0.85f) ;
		else
			AddSprite(flMenuL, flMenuT, flMenuR, flMenuB, UVCOORD_BLANK.left/TEXW, UVCOORD_BLANK.top/TEXH, UVCOORD_BLANK.right/TEXW, UVCOORD_BLANK.bottom/TEXH, 0.1f, 0.1f, 0.1f) ;
	}

	// draw the buttons
	for(nButton=0 ; nButton<nMaxButton ; nButton++)
		if(Menu[nMenu].Button[nButton].Visible)
			switch(Menu[nMenu].Button[nButton].Style)
			{
				case BUTTONSTYLE_FANCY:  DrawButtonFancy(nMenu, nButton) ; break ;
				case BUTTONSTYLE_SIMPLE:  DrawButtonSimple(nMenu, nButton) ; break ;
			}

	// last of all, draw the cursor
	if(m_bCursorVisible)
		AddSprite(flIconL, flIconT, flIconR, flIconB, UVCOORD_CURSOR.left/TEXW, UVCOORD_CURSOR.top/TEXH, UVCOORD_CURSOR.right/TEXW, UVCOORD_CURSOR.bottom/TEXH, 1, 1, 1) ;

}

void Ogre2dManager::FinishEditBox()
{
	// remove all trailing spaces
	int nLen=strlen(m_chEditBoxMessage) ;
	for(int nPos=nLen-1 ; nPos>=0 ; nPos--)
		if(m_chEditBoxMessage[nPos]==' ')
			m_chEditBoxMessage[nPos]='\0' ;
		else
			break ; // get out as soon as we hit some other character

	// if the edit box is an integer type, and is totally empty, set to 0, or the min value if 0 is out of range
	// if the edit box is an integer type, and is just a minus sign, set to 0 or the min value if 0 is out of range
	if(
			(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_INTEGER) 
			&& (
							(strlen(m_chEditBoxMessage)==0) // if empty
							||
							((m_chEditBoxMessage[0]=='-') && (m_chEditBoxMessage[1]=='\0')) // if just a - sign
				 )
		)
	{
		if((0>=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) && (0<=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal))
			strcpy(m_chEditBoxMessage, "0") ;
		else
			sprintf(m_chEditBoxMessage, "%i", (int)Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) ;
	}

	// if integer box and too high or low, set to max or min
	if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_INTEGER)
	{
		int nVal=atoi(m_chEditBoxMessage) ;
		if(nVal<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal)
			sprintf(m_chEditBoxMessage, "%i", (int)Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) ;
		else
			if(nVal>Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal)
				sprintf(m_chEditBoxMessage, "%i", (int)Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal) ;
	}
	

	if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_FLOAT)
	{
		// if we get an invalid input, we put in 0.0 or min value if 0.0 is out of range
		float flBadVal=0.0f ;
		if((flBadVal>=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) && (flBadVal<=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal))
			flBadVal=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal ;

				int nNPos=0 ;
				int nBadInput=0 ;
				// check first character, must be - or 0 to 9
				if( (m_chEditBoxMessage[nNPos]!='-') && ((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9')) )
					 nBadInput=1 ;

				// if first character was a '-', then second must be 0 to 9
				if( (m_chEditBoxMessage[nNPos]=='-') && ((m_chEditBoxMessage[nNPos+1]<'0') || (m_chEditBoxMessage[nNPos+1]>'9')) )
					nBadInput=1 ;

				// scan digits up to '.' looking for illegal characters
				nNPos++ ;
				while((m_chEditBoxMessage[nNPos]!='\0') && (m_chEditBoxMessage[nNPos]!='.'))
					if((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9'))
					{
						nBadInput=1 ;
						break ;
					}
					else
						nNPos++ ;

				// reached either a null or a decimal point.  If not a decimal point, revert
				if(m_chEditBoxMessage[nNPos]=='.')
				{
					// next character must be 0 to 9
					if((m_chEditBoxMessage[nNPos+1]<'0') || (m_chEditBoxMessage[nNPos+1]>'9'))
						nBadInput=1 ;


					// scan remaining for illegal characters
					nNPos++ ;
					while(m_chEditBoxMessage[nNPos]!='\0')
						if((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9'))
						{
							nBadInput=1 ;
							break ;
						}
						else
							nNPos++ ;

				}
				else
					nBadInput=1 ;

				// check for leading zeros. only allowed in 0.0 and -0.0
				if(m_chEditBoxMessage[0]=='-') 
					nNPos=1 ;
				else
					nNPos=0 ;
				if((m_chEditBoxMessage[nNPos]=='0') && (m_chEditBoxMessage[nNPos+1]!='.'))
					nBadInput=1 ;

				
				// if too big or too small, revert
				float flVal=atof(m_chEditBoxMessage) ;
				if((flVal<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) || (flVal>Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal))
					nBadInput=1 ;

				if(nBadInput)
					sprintf(m_chEditBoxMessage, "%.1f", flBadVal) ;
	}


	if(strlen(m_chEditBoxMessage)<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].NameMem[0])
		strcpy(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Name[0], m_chEditBoxMessage) ;
	m_nEditBoxAction=EDITBOXACTION_FINISHEDINPUT ;
}

void Ogre2dManager::ShowCursor() { m_bCursorVisible=true ; }
void Ogre2dManager::HideCursor()  { m_bCursorVisible=false ; }

//void Ogre2dManager::UpdateKeyboardInput(   OIS::Keyboard* m_pKB, int nShift, int nCtrl)
//{
//
//	////////////////////////////////////////////////////////////////////////////////////////////////////////
//	// if an edit box is active
//
//	if((m_nMenuMode!=MENU_NONE) && (m_nEditBoxMenu!=MENU_NONE))
//	{
//
//		m_nEditBoxAction=EDITBOXACTION_NONE ; // default is no action.
//
//		// enter, copy the temp message to the button and signal it has been updated
//		if(m_pKB->isKeyDown(OIS::KC_RETURN))
//		{
//			if(m_nKT[OIS::KC_RETURN]==0)
//			{
//				m_nKT[OIS::KC_RETURN]=1 ;
//				FinishEditBox() ;
//			}
//		}
//		else
//			m_nKT[OIS::KC_RETURN]=0 ;
//
//
//		// keep backup of old message, so that if the new message is illegal we can revert
//		char chTemp[ABSOLUTE_MAX_BUTTON_TEXT] ;
//		strcpy(chTemp, m_chEditBoxMessage) ; 
//		int nRevert=0 ;
//		int nOldEditBoxPos=m_nEditBoxPos ;
//
//		if(m_pKB->isKeyDown(OIS::KC_LEFT))	
//		{ 
//
//			if(m_dKD[OIS::KC_LEFT]<m_GuiTime)
//			{
//				if(m_nKT[OIS::KC_LEFT]==0) 
//				{ 
//					m_nKT[OIS::KC_LEFT]=1 ;
//					m_dKD[OIS::KC_LEFT]=m_GuiTime+333 ;
//				}
//				else
//					m_dKD[OIS::KC_LEFT]=m_GuiTime+25 ;
//				
//				m_nEditBoxPos-- ;
//				if(m_nEditBoxPos<0) m_nEditBoxPos=0 ; 
//			} 
//		}
//		else
//		{
//			m_nKT[OIS::KC_LEFT]=0 ;
//			m_dKD[OIS::KC_LEFT]=0 ;
//		}
//
//		if(m_pKB->isKeyDown(OIS::KC_RIGHT))	
//		{ 
//
//			if(m_dKD[OIS::KC_RIGHT]<m_GuiTime)
//			{
//				if(m_nKT[OIS::KC_RIGHT]==0) 
//				{ 
//					m_nKT[OIS::KC_RIGHT]=1 ;
//					m_dKD[OIS::KC_RIGHT]=m_GuiTime+333 ;
//				}
//				else
//					m_dKD[OIS::KC_RIGHT]=m_GuiTime+25 ;
//				
//				m_nEditBoxPos++ ;
//				if(m_nEditBoxPos>strlen(m_chEditBoxMessage)) m_nEditBoxPos=strlen(m_chEditBoxMessage) ; 
//			} 
//		}
//		else
//		{
//			m_nKT[OIS::KC_RIGHT]=0 ;
//			m_dKD[OIS::KC_RIGHT]=0 ;
//		}
//
//
//
//		if(m_pKB->isKeyDown(OIS::KC_BACK))	
//		{
//			if(m_dKD[OIS::KC_BACK]<m_GuiTime)
//			{
//
//				if(m_nKT[OIS::KC_BACK]==0)
//				{
//					m_nKT[OIS::KC_BACK]=1 ;
//					m_dKD[OIS::KC_BACK]=m_GuiTime+333 ;
//				}
//				else
//					m_dKD[OIS::KC_BACK]=m_GuiTime+25 ;
//
//				m_nEditBoxPos-- ; 
//				if(m_nEditBoxPos<0) 
//					m_nEditBoxPos=0 ;
//				else
//				{
//					// shift all other characters down 1
//					for(int nPos=m_nEditBoxPos ; nPos<ABSOLUTE_MAX_BUTTON_TEXT-1 ; nPos++)
//						m_chEditBoxMessage[nPos]=m_chEditBoxMessage[nPos+1] ;
//				}
//			}
//		}
//		else
//		{
//			m_nKT[OIS::KC_BACK]=0 ;
//			m_dKD[OIS::KC_BACK]=0 ;
//		}
//
//
//		if(m_pKB->isKeyDown(OIS::KC_DELETE))	
//		{
//			if(m_dKD[OIS::KC_DELETE]<m_GuiTime)
//			{
//				if(m_nKT[OIS::KC_DELETE]==0)
//				{
//					m_nKT[OIS::KC_DELETE]=1 ;
//					m_dKD[OIS::KC_DELETE]=m_GuiTime+333 ;
//				}
//				else
//					m_dKD[OIS::KC_DELETE]=m_GuiTime+25 ;
//
//				int nLen=strlen(m_chEditBoxMessage) ;
//					
//				if(m_nEditBoxPos<nLen)
//				{
//					// shift all other characters down 1
//					for(int nPos=m_nEditBoxPos ; nPos<ABSOLUTE_MAX_BUTTON_TEXT-1 ; nPos++)
//						m_chEditBoxMessage[nPos]=m_chEditBoxMessage[nPos+1] ;
//				}
//			}
//		}
//		else 
//		{
//			m_nKT[OIS::KC_DELETE]=0 ;
//			m_dKD[OIS::KC_DELETE]=0 ;
//		}
//
//		// if in integer mode, allow cursor up/down to increase/decrease value
//		// we also process mouse wheel input for integer edit boxes here
//		if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_INTEGER)
//		{
//			if( (m_pKB->isKeyDown(OIS::KC_UP)) || (m_nMouseWheelChange==1))	
//			{
//				if(m_dKD[OIS::KC_UP]<m_GuiTime)
//				{
//					if(m_nKT[OIS::KC_UP]==0)
//					{
//						m_nKT[OIS::KC_UP]=1 ;
//						m_dKD[OIS::KC_UP]=m_GuiTime+333 ;
//					}
//					else
//						m_dKD[OIS::KC_UP]=m_GuiTime+50 ;
//
//					// increment the integer
//					int nVal=atoi(m_chEditBoxMessage) ;
//					if(nShift)
//						nVal+=10 ;
//					else
//						if(nCtrl)
//							nVal+=32 ;
//						else
//							nVal++ ;
//
//					if(nVal>Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal) nVal=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal ;
//					sprintf(m_chEditBoxMessage, "%i", nVal) ;
//					m_nEditBoxAction=EDITBOXACTION_SPECIALINPUT ;
//
//				}
//			}
//			else
//				if( !m_pKB->isKeyDown(OIS::KC_UP))
//				{
//					m_nKT[OIS::KC_UP]=0 ;
//					m_dKD[OIS::KC_UP]=0 ;
//				}
//
//			if( (m_pKB->isKeyDown(OIS::KC_DOWN))  || (m_nMouseWheelChange==-1))		
//			{
//				if(m_dKD[OIS::KC_DOWN]<m_GuiTime)
//				{
//					if(m_nKT[OIS::KC_DOWN]==0)
//					{
//						m_nKT[OIS::KC_DOWN]=1 ;
//						m_dKD[OIS::KC_DOWN]=m_GuiTime+333 ;
//					}
//					else
//						m_dKD[OIS::KC_DOWN]=m_GuiTime+50 ;
//
//					// increment the integer
//					int nVal=atoi(m_chEditBoxMessage) ;
//					if(nShift)
//						nVal-=10 ;
//					else
//						if(nCtrl)
//							nVal-=32 ;
//						else
//							nVal-- ;
//
//					if(nVal<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) nVal=Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal ;
//					sprintf(m_chEditBoxMessage, "%i", nVal) ;
//					m_nEditBoxAction=EDITBOXACTION_SPECIALINPUT ;
//				}
//			}
//			else 
//				if( !m_pKB->isKeyDown(OIS::KC_DOWN))
//				{
//					m_nKT[OIS::KC_DOWN]=0 ;
//					m_dKD[OIS::KC_DOWN]=0 ;
//				}
//
//		}// end integer value control
//
//
//
//
//		int nAllowedKey=0 ;
//		char chL=0 ;
//		for(int nKey=OIS::KC_UNASSIGNED ; nKey<=OIS::KC_MEDIASELECT ; nKey++)
//		{
//			
//			// skip if the key isn't pressed
//			if(!m_pKB->isKeyDown((OIS::KeyCode)nKey)) 
//			{
//				m_nKT[nKey]=0 ;
//				m_dKD[nKey]=0 ;
//				continue ;
//			}
//
//			// check it is an allowed key
//			nAllowedKey=0 ;
//			if(nKey==OIS::KC_A) { nAllowedKey=1 ; if(nShift) chL='A' ; else chL='a' ; }
//			if(nKey==OIS::KC_B) { nAllowedKey=1 ; if(nShift) chL='B' ; else chL='b' ; }
//			if(nKey==OIS::KC_C) { nAllowedKey=1 ; if(nShift) chL='C' ; else chL='c' ; }
//			if(nKey==OIS::KC_D) { nAllowedKey=1 ; if(nShift) chL='D' ; else chL='d' ; }
//			if(nKey==OIS::KC_E) { nAllowedKey=1 ; if(nShift) chL='E' ; else chL='e' ; }
//			if(nKey==OIS::KC_F) { nAllowedKey=1 ; if(nShift) chL='F' ; else chL='f' ; }
//			if(nKey==OIS::KC_G) { nAllowedKey=1 ; if(nShift) chL='G' ; else chL='g' ; }
//			if(nKey==OIS::KC_H) { nAllowedKey=1 ; if(nShift) chL='H' ; else chL='h' ; }
//			if(nKey==OIS::KC_I) { nAllowedKey=1 ; if(nShift) chL='I' ; else chL='i' ; }
//			if(nKey==OIS::KC_J) { nAllowedKey=1 ; if(nShift) chL='J' ; else chL='j' ; }
//			if(nKey==OIS::KC_K) { nAllowedKey=1 ; if(nShift) chL='K' ; else chL='k' ; }
//			if(nKey==OIS::KC_L) { nAllowedKey=1 ; if(nShift) chL='L' ; else chL='l' ; }
//			if(nKey==OIS::KC_M) { nAllowedKey=1 ; if(nShift) chL='M' ; else chL='m' ; }
//			if(nKey==OIS::KC_N) { nAllowedKey=1 ; if(nShift) chL='N' ; else chL='n' ; }
//			if(nKey==OIS::KC_O) { nAllowedKey=1 ; if(nShift) chL='O' ; else chL='o' ; }
//			if(nKey==OIS::KC_P) { nAllowedKey=1 ; if(nShift) chL='P' ; else chL='p' ; }
//			if(nKey==OIS::KC_Q) { nAllowedKey=1 ; if(nShift) chL='Q' ; else chL='q' ; }
//			if(nKey==OIS::KC_R) { nAllowedKey=1 ; if(nShift) chL='R' ; else chL='r' ; }
//			if(nKey==OIS::KC_S) { nAllowedKey=1 ; if(nShift) chL='S' ; else chL='s' ; }
//			if(nKey==OIS::KC_T) { nAllowedKey=1 ; if(nShift) chL='T' ; else chL='t' ; }
//			if(nKey==OIS::KC_U) { nAllowedKey=1 ; if(nShift) chL='U' ; else chL='u' ; }
//			if(nKey==OIS::KC_V) { nAllowedKey=1 ; if(nShift) chL='V' ; else chL='v' ; }
//			if(nKey==OIS::KC_W) { nAllowedKey=1 ; if(nShift) chL='W' ; else chL='w' ; }
//			if(nKey==OIS::KC_X) { nAllowedKey=1 ; if(nShift) chL='X' ; else chL='x' ; }
//			if(nKey==OIS::KC_Y) { nAllowedKey=1 ; if(nShift) chL='Y' ; else chL='y' ; }
//			if(nKey==OIS::KC_Z) { nAllowedKey=1 ; if(nShift) chL='Z' ; else chL='z' ; }
//
//			if(nKey==OIS::KC_0) { nAllowedKey=1 ; if(nShift) chL=')' ; else chL='0' ; }
//			if(nKey==OIS::KC_1) { nAllowedKey=1 ; if(nShift) chL='!' ; else chL='1' ; }
//			if(nKey==OIS::KC_2) { nAllowedKey=1 ; if(nShift) chL='@' ; else chL='2' ; }
//			if(nKey==OIS::KC_3) { nAllowedKey=1 ; if(nShift) chL='#' ; else chL='3' ; }
//			if(nKey==OIS::KC_4) { nAllowedKey=1 ; if(nShift) chL='$' ; else chL='4' ; }
//			if(nKey==OIS::KC_5) { nAllowedKey=1 ; if(nShift) chL='%' ; else chL='5' ; }
//			if(nKey==OIS::KC_6) { nAllowedKey=1 ; if(nShift) chL='^' ; else chL='6' ; }
//			if(nKey==OIS::KC_7) { nAllowedKey=1 ; if(nShift) chL='&' ; else chL='7' ; }
//			if(nKey==OIS::KC_8) { nAllowedKey=1 ; if(nShift) chL='*' ; else chL='8' ; }
//			if(nKey==OIS::KC_9) { nAllowedKey=1 ; if(nShift) chL='(' ; else chL='9' ; }
//
//			if(nKey==OIS::KC_COMMA)				{ nAllowedKey=1 ; if(nShift) chL='<' ; else chL=',' ; }
//			if(nKey==OIS::KC_PERIOD)			{ nAllowedKey=1 ; if(nShift) chL='>' ; else chL='.' ; }
//			if(nKey==OIS::KC_SLASH)				{ nAllowedKey=1 ; if(nShift) chL='?' ; else chL='/' ; }
//
//			if(nKey==OIS::KC_SEMICOLON)		{ nAllowedKey=1 ; if(nShift) chL=':' ; else chL=';' ; }
//			if(nKey==OIS::KC_APOSTROPHE)	{ nAllowedKey=1 ; if(nShift) chL=0x22 ; else chL= 0x27; }
//
//			if(nKey==OIS::KC_LBRACKET)		{ nAllowedKey=1 ; if(nShift) chL='{' ; else chL='[' ; }
//			if(nKey==OIS::KC_RBRACKET)		{ nAllowedKey=1 ; if(nShift) chL='}' ; else chL=']' ; }
//			if(nKey==OIS::KC_BACKSLASH)		{ nAllowedKey=1 ; if(nShift) chL='|' ; else chL=0x5C ; }
//
//			if(nKey==OIS::KC_GRAVE)				{ nAllowedKey=1 ; if(nShift) chL='~' ; else chL='`' ; }
//			if(nKey==OIS::KC_MINUS)				{ nAllowedKey=1 ; if(nShift) chL='_' ; else chL='-' ; }
//			if(nKey==OIS::KC_EQUALS)			{ nAllowedKey=1 ; if(nShift) chL='+' ; else chL='=' ; }
//
//			if(nKey==OIS::KC_SPACE)				{ nAllowedKey=1 ; chL=' ' ; }
//			
//
//			// if an integer textbox, disallow non-numerical symbols
//			//if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_INTEGER)
//			//	if(!  ( ((chL>='0') && (chL<='9')) || (chL=='-') )  )
//			//		continue ;
//
//
//
//			if(!nAllowedKey) continue ;
//			if(m_dKD[nKey]>=m_GuiTime) continue ; // too soon
//
//			// set the delay and toggle
//			if(m_nKT[nKey]==0) 
//			{ 
//				m_nKT[nKey]=1 ;
//				m_dKD[nKey]=m_GuiTime+333 ;
//			}
//			else
//				m_dKD[nKey]=m_GuiTime+25 ;
//
//
//			
//
//			
//
//			// shift all other characters up 1
//			for(int nPos=ABSOLUTE_MAX_BUTTON_TEXT-1 ; nPos>m_nEditBoxPos ; nPos--)
//				m_chEditBoxMessage[nPos]=m_chEditBoxMessage[nPos-1] ;
//
//			// insert the new character
//			m_chEditBoxMessage[m_nEditBoxPos]=chL ;	
//
//			// make sure the very last character is a null
//			m_chEditBoxMessage[ABSOLUTE_MAX_BUTTON_TEXT-1]='\0' ;
//
//			// move the cursor
//			m_nEditBoxPos++ ; 
//			if(m_nEditBoxPos>strlen(m_chEditBoxMessage)) 
//				m_nEditBoxPos=strlen(m_chEditBoxMessage) ;
//
//			
//
//
//		}// end looping through characters
//
//		// check new message is legal
//			
//		///////////////////////////////////////////////////////////////////////////////////////////////////////
//		// if in integer mode, check message is a legal integer
//
//			if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_INTEGER)
//			{
//				int nVal=atoi(m_chEditBoxMessage) ;
//				char chNum[MISCSTRINGSIZE] ; 
//				sprintf(chNum, "%i", nVal) ;
//				// if the converted value doesn't match the original value, revert
//				if(strcmp(chNum, m_chEditBoxMessage)!=0)
//					nRevert=1 ;
//				
//
//				// if too big or too small, revert
//				//if((nVal<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) || (nVal>Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal))
//				//	nRevert=1 ;
//
//				// as an exception, allow totally empty edit boxes, it makes editing numbers easier.
//				if(strlen(m_chEditBoxMessage)==0) // totally empty, treat as 0
//						nRevert=0 ;
//
//				// also allow just a - sign
//				if((m_chEditBoxMessage[0]=='-') && (m_chEditBoxMessage[1]=='\0'))
//					nRevert=0 ;
//
//				// if we have a leading 0, get rid of it.
//				if((m_chEditBoxMessage[0]=='0') && (m_chEditBoxMessage[1]!='\0'))
//				{
//					int nPos=-1 ;
//					while(m_chEditBoxMessage[++nPos]!='\0')
//						m_chEditBoxMessage[nPos]=m_chEditBoxMessage[nPos+1] ;
//					nRevert=0 ;
//				}
//
//			}// end integer check
//
//		///////////////////////////////////////////////////////////////////////////////////////////////////////
//		// if in float mode, check message is a legal integer
//
//			if(Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].Action==BUTTONACTION_EDIT_FLOAT)
//			{
//				// only allow certain characters, and work out if it is a valid number only when edit box is finalized.
//				int nLen=strlen(m_chEditBoxMessage) ;
//				for(int nNPos=0 ; nNPos<nLen ; nNPos++)
//					if( (m_chEditBoxMessage[nNPos]!='.') && (m_chEditBoxMessage[nNPos]!='-') && ((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9')) )
//						nRevert=1 ;
//
//
//				/*
//				int nNPos=0 ;
//				// check first character, must be - or 0 to 9
//				if( (m_chEditBoxMessage[nNPos]!='-') && ((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9')) )
//					nRevert=1 ;
//
//				// if first character was a '-', then second must be 0 to 9
//				if( (m_chEditBoxMessage[nNPos]=='-') && ((m_chEditBoxMessage[nNPos+1]<'0') || (m_chEditBoxMessage[nNPos+1]>'9')) )
//					nRevert=1 ;
//
//				// scan digits up to '.' looking for illegal characters
//				nNPos++ ;
//				while((m_chEditBoxMessage[nNPos]!='\0') && (m_chEditBoxMessage[nNPos]!='.'))
//					if((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9'))
//					{
//						nRevert=1 ;
//						break ;
//					}
//					else
//						nNPos++ ;
//
//				// reached either a null or a decimal point.  If not a decimal point, revert
//				if(m_chEditBoxMessage[nNPos]=='.')
//				{
//					// next character must be 0 to 9
//					if((m_chEditBoxMessage[nNPos+1]<'0') || (m_chEditBoxMessage[nNPos+1]>'9'))
//						nRevert=1 ;
//
//
//					// scan remaining for illegal characters
//					nNPos++ ;
//					while(m_chEditBoxMessage[nNPos]!='\0')
//						if((m_chEditBoxMessage[nNPos]<'0') || (m_chEditBoxMessage[nNPos]>'9'))
//						{
//							nRevert=1 ;
//							break ;
//						}
//						else
//							nNPos++ ;
//
//				}
//				else
//					nRevert=1 ;
//
//				// check for leading zeros. only allowed in 0.0 and -0.0
//				if(m_chEditBoxMessage[0]=='-') 
//					nNPos=1 ;
//				else
//					nNPos=0 ;
//				if((m_chEditBoxMessage[nNPos]=='0') && (m_chEditBoxMessage[nNPos+1]!='.'))
//					nRevert=1 ;
//
//				
//				// if too big or too small, revert
//				float flVal=atof(m_chEditBoxMessage) ;
//				if((flVal<Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MinVal) || (flVal>Menu[m_nEditBoxMenu].Button[m_nEditBoxButton].MaxVal))
//					nRevert=1 ;
//				*/
//			}
//
//		///////////////////////////////////////////////////////////////////////////////////////
//		// check message will fit in edit box
//
//			float flMaxSize=0.0f ;
//			int nStyle=Menu[ m_nEditBoxMenu ].Button[ m_nEditBoxButton ].Style ;
//			int nMaxWidth=Menu[ m_nEditBoxMenu ].Button[ m_nEditBoxButton ].SizeX-2 ;
//			int nWidth=0 ;
//			if(nStyle==BUTTONSTYLE_FANCY)
//				nWidth=PrintTextMED(m_chEditBoxMessage, 0, 0, BUTTONEDITTEXTSCALE, 1.0f, true, 1, 1, 1) ;
//			else
//			if(nStyle==BUTTONSTYLE_SIMPLE)
//				nWidth=PrintTextMED(m_chEditBoxMessage, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 1, 1, 1) ;
//			if(nWidth>=nMaxWidth) nRevert=1 ; // if too big, revert message
//
//		//////////////////////////////////////////////////////////////////////////////////////
//		// if new string is illegal, revert to old string
//
//		if(nRevert)
//		{
//			strcpy(m_chEditBoxMessage, chTemp) ;
//			m_nEditBoxPos=nOldEditBoxPos ;
//		}
//
//
//	}// end if an edit box is active
//
//}

/*************************************************************************************************\

							Setup Application Specific Controls

\*************************************************************************************************/

void Ogre2dManager::CreateMenu(int nMenu, int nLeft, int nRight, int nUp, int nDown, bool bBack, bool bVisible)
{
	BZNMENU MenuTemp ;

	MenuTemp.Back=bBack ;
	MenuTemp.Visible=bVisible ;
	MenuTemp.L=nLeft ;
	MenuTemp.R=nRight ;
	MenuTemp.U=nUp ;
	MenuTemp.D=nDown ;
	MenuTemp.Type=MENUTYPE_DEFAULT ;
	
	Menu.push_back(MenuTemp) ;
}

void Ogre2dManager::CreateMenuB(int nMenu, int nLeft, int nUp, int nSizeX, int nSizeY, bool bBack, bool bVisible)
{
	BZNMENU MenuTemp ;

	MenuTemp.Back=bBack ;
	MenuTemp.Visible=bVisible ;
	MenuTemp.L=nLeft ;
	MenuTemp.R=nLeft+nSizeX ;
	MenuTemp.U=nUp ;
	MenuTemp.D=nUp+nSizeY ;
	MenuTemp.Type=MENUTYPE_DEFAULT ;

	Menu.push_back(MenuTemp) ;
}

// databox control is a special type for displayig lines of text and maybe an input edit box at the bottom.
// it automatically adds the required buttons.
int Ogre2dManager::AddSpecialControl_DataBox(int nMenu, int nPositionX, int nPositionY, int nSizeX, int nSizeY, bool bInput, bool bBack, bool bVisible)
{
	SPECIALCONTROL TempSpecialControl ;
	TempSpecialControl.Type=SPECIALCONTROL_DATABOX ;

	int nButtonSizeX=nSizeX ;
	int nButtonSizeY=12 ;
	int nButtonGapY=13 ;
	
		
	// how many lines of text?
	int nMaxLine=(nSizeY-SIMPLEBUTTONFRAMESIZE)/nButtonGapY ;

	if(bInput) nMaxLine-- ; // one less line if we need a line for input

	TempSpecialControl.MaxLine=nMaxLine ;

	// databox buttons will be the last buttons.  Need to remember the offset.
	int nButtonCount=0 ;
	int nButtonOffset=Menu[nMenu].Button.size() ;
	TempSpecialControl.ButtonStart=nButtonOffset ;
	

	// create the required buttons.

	

	
	// how wide?
	int nButtonPosY=nPositionY ;
	
	for(int nLine=0 ; nLine<nMaxLine ; nLine++)
	{
		CreateButtonSimpleLongName(nMenu, nButtonCount+nButtonOffset,			nPositionX, nButtonPosY,		nButtonSizeX, nButtonSizeY,	BUTTONACTION_STATICTEXTLJUST, "", "Test text line") ;
		Menu[nMenu].Button[nButtonCount+nButtonOffset].SpecialControl=Menu[nMenu].SpecialControl.size() ; // remember which special control this button is on. 
		nButtonCount++ ;
		nButtonPosY+=nButtonGapY ;
	}
	
	// make the input the last button.
	if(bInput)
	{
		CreateButtonSimpleLongName(nMenu, nButtonCount+nButtonOffset,			nPositionX, nButtonPosY,		nButtonSizeX, nButtonSizeY,		BUTTONACTION_EDIT_TEXT, "", "Enter Text Here") ; 
		Menu[nMenu].Button[nButtonCount+nButtonOffset].SpecialControl=Menu[nMenu].SpecialControl.size() ; // remember which special control this button is on. (Doesn't really matter here, but do it anyway)
		nButtonCount++ ;
	}

	TempSpecialControl.ButtonEnd=Menu[nMenu].Button.size() ;
	Menu[nMenu].SpecialControl.push_back(TempSpecialControl) ;

	return Menu[nMenu].SpecialControl.size()-1 ;
	

}

// returns number of characters added, text is truncated if it can't fit.  Doesn't add any if menu isn't a databox.
int Ogre2dManager::AddTextToDataBox(int nMenu, int nDataBox, char* pDataText, float flR, float flG, float flB)
{
	int nLen=strlen(pDataText) ;
	int nLastLine=Menu[nMenu].SpecialControl[nDataBox].MaxLine-1 ;
	int nButtonOffset=Menu[nMenu].SpecialControl[nDataBox].ButtonStart ;
	int nMaxSize=Menu[nMenu].Button[nButtonOffset].SizeX ;

	
	// repeated remove last character of text until the text fits on the line.
	char* pTempText=new char[nLen+1] ;
	strcpy(pTempText, pDataText) ;
	
	// make sure len is small enough to fit the max characters
	if(nLen>=ABSOLUTE_MAX_BUTTON_TEXT-1)
		nLen=ABSOLUTE_MAX_BUTTON_TEXT-1 ;
	
	while((nLen>4) && (PrintTextMED(pTempText, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 0.0f, 0.0f, 0.0f)>=nMaxSize))
	{
		pTempText[nLen-3]='.' ;
		pTempText[nLen-2]='.' ;
		pTempText[nLen-1]='.' ;
		pTempText[nLen]='\0' ;
		nLen-- ;
	}
	
	
	nLen=strlen(pTempText) ;
	
	// remember the top line memory location.
	char* pTopLine=		Menu[nMenu].Button[nButtonOffset].Name[0] ;
	char nTopLineMen=	Menu[nMenu].Button[nButtonOffset].NameMem[0] ;
	
	// move all lines up
	for(int nLine=1 ; nLine<=nLastLine ; nLine++)
	{
		int nButton=nLine+nButtonOffset ;
		Menu[nMenu].Button[nButton-1].Name[0]			=Menu[nMenu].Button[nButton].Name[0] ;
		Menu[nMenu].Button[nButton-1].NameMem[0]	=Menu[nMenu].Button[nButton].NameMem[0] ;
		Menu[nMenu].Button[nButton-1].R						=Menu[nMenu].Button[nButton].R ;
		Menu[nMenu].Button[nButton-1].G						=Menu[nMenu].Button[nButton].G ;
		Menu[nMenu].Button[nButton-1].B						=Menu[nMenu].Button[nButton].B ;	
	}
	
	// add last line
	int nButton=nLastLine+nButtonOffset ;
	Menu[nMenu].Button[nButton].Name[0]= pTopLine ;
	Menu[nMenu].Button[nButton].NameMem[0]=nTopLineMen ;
	strcpy(Menu[nMenu].Button[nButton].Name[0], pTempText) ;
	Menu[nMenu].Button[nButton].R=flR*255.0f ;
	Menu[nMenu].Button[nButton].G=flG*255.0f ;
	Menu[nMenu].Button[nButton].B=flB*255.0f ;
	
	
	delete [] pTempText ;
	
	return nLen ; // return how many characters were added.
}

// see if a databox has had input, if so return the string, the length of the string and clear the databox input.
int Ogre2dManager::CheckDataBoxInput(int nMenu, int nDataBox, char* pInput)
{		
	// if the edit box is still active, return 0
	if(m_nEditBoxMenu==nMenu)
		return 0 ;
		
	// return contents of edit box, and if it has anything in it, clear it.
	// input edit box is always the last button on a SpecialControl databox
	int nButton=Menu[nMenu].SpecialControl[nDataBox].ButtonEnd-1 ;
	strcpy(pInput, Menu[nMenu].Button[nButton].Name[0]) ;
	
	Menu[nMenu].Button[nButton].Name[0][0]='\0' ;
	
	return strlen(pInput) ;
}

// columnbox displays columns of related data, and the rows are clickable
// it automatically adds the required buttons.
int Ogre2dManager::AddSpecialControl_ColumnBox(int nMenu, int nPositionX, int nPositionY, int nSizeX, int nSizeY, COLUMNBOXINFO* pColumnBoxInfo, bool bFeedback, bool bBack, bool bVisible)
{
	SPECIALCONTROL TempSpecialControl ;
	TempSpecialControl.Type=SPECIALCONTROL_COLUMNBOX ;
	
	// copy the ColumnBoxInfo
	TempSpecialControl.ColumnBoxInfo.MaxColumn=pColumnBoxInfo->MaxColumn ;
	for(int nColumn=0 ; nColumn<COLUMNMAX ; nColumn++)
	{
		TempSpecialControl.ColumnBoxInfo.ColumnSizeX[nColumn]=pColumnBoxInfo->ColumnSizeX[nColumn] ;
		for(int nPos=0 ; nPos<COLUMNNAMESIZE ; nPos++)
			TempSpecialControl.ColumnBoxInfo.Name[nColumn*COLUMNNAMESIZE+nPos]=pColumnBoxInfo->Name[nColumn*COLUMNNAMESIZE+nPos] ;	
	}

	int nMaxDataBox=Menu[nMenu].SpecialControl.size() ;

	

	int nButtonSizeX=nSizeX ;
	int nButtonSizeY=12 ;
	int nButtonGapY=13 ;
	
	// how many lines of text?
	int nMaxLine=(nSizeY-SIMPLEBUTTONFRAMESIZE)/nButtonGapY-1 ; // -1 for the header
	if(bFeedback) nMaxLine-=1 ; // -1 for feedback bar 
	
	int nSliderSizeX=20 ;
	int nSliderSizeY=nMaxLine*nButtonGapY-SIMPLEBUTTONFRAMESIZE/2;
	int nSliderPosX=nPositionX+nSizeX-nSliderSizeX ;
	int nSliderPosY=nPositionY+nButtonGapY+SIMPLEBUTTONFRAMESIZE/2 ;

	// make slider a little shorter if the feedback bar is active
	if(bFeedback) nSliderSizeY-=2 ;
	
		
	

	TempSpecialControl.MaxLine=nMaxLine ;

	// databox buttons will be the last buttons.  Need to remember the offset.
	int nButtonCount=0 ;
	int nButtonOffset=Menu[nMenu].Button.size() ;
	TempSpecialControl.ButtonStart=nButtonOffset ;
	

	// create the required buttons.

	

	
	// how wide?
	int nButtonPosY=nPositionY+nButtonGapY ; // leave space for header
	
	for(int nLine=0 ; nLine<nMaxLine ; nLine++)
	{
		CreateButtonSimpleLongName(nMenu, nButtonCount+nButtonOffset,			nPositionX, nButtonPosY,		nButtonSizeX, nButtonSizeY,	BUTTONACTION_STATICTEXTCOLUMN, "", "") ;
		Menu[nMenu].Button[nButtonCount+nButtonOffset].SpecialControl=Menu[nMenu].SpecialControl.size() ; // remember which special control this button is on.  
		nButtonCount++ ;
		nButtonPosY+=nButtonGapY ;
	}

	// create feedback if needed. Will always be third last button
	if(bFeedback)
	{
		CreateButtonSimpleLongName(nMenu, nButtonCount+nButtonOffset,			nPositionX, nButtonPosY,		nButtonSizeX, nButtonSizeY,	BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK, "", "") ;
		Menu[nMenu].Button[nButtonCount+nButtonOffset].SpecialControl=Menu[nMenu].SpecialControl.size() ; // remember which special control this button is on. 
		SetButtonString_ColumnBox(nMenu, nButtonCount+nButtonOffset, pColumnBoxInfo->MaxColumn, COLUMNNAMESIZE, &pColumnBoxInfo->Name[0]) ;
		nButtonCount++ ;
		nButtonPosY+=nButtonGapY ;

		TempSpecialControl.ColumnBoxInfo.Feedback=true ;
	}
	else
		TempSpecialControl.ColumnBoxInfo.Feedback=false ;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// create the slider.  Slider will always be the second last button
	
	CreateButtonSimple(nMenu, nButtonCount+nButtonOffset, nSliderPosX, nSliderPosY, nSliderSizeX, nSliderSizeY, BUTTONACTION_COLUMNSLIDER, "", "") ;
	// set the slider position, stored in MinVal.
	if(pColumnBoxInfo->FillInfo.MaxRow==0)
		Menu[nMenu].Button[nButtonCount+nButtonOffset].MinVal=0.0f ;
	else
		Menu[nMenu].Button[nButtonCount+nButtonOffset].MinVal=SliderPercent(pColumnBoxInfo->FillInfo.MaxRow, nMaxLine, pColumnBoxInfo->FillInfo.StartRow) ;
	
	// remember which special control this slider is connected to.
	Menu[nMenu].Button[nButtonCount+nButtonOffset].Info[SLIDERINFO_CONNECTIONTYPE]=SLIDERCONNECTIONTYPE_COLUMNBOX ;
	Menu[nMenu].Button[nButtonCount+nButtonOffset].Info[SLIDERINFO_CONNECTION]=Menu[nMenu].SpecialControl.size() ; 
	
	nButtonCount++ ;
	
	// end create slider
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	


	// create the header.  Header will always be the last button
	CreateButtonSimpleLongName(nMenu, nButtonCount+nButtonOffset,			nPositionX, nPositionY,		nButtonSizeX, nButtonSizeY,	BUTTONACTION_STATICTEXTCOLUMN_NOCLICK, "", "") ;
	Menu[nMenu].Button[nButtonCount+nButtonOffset].SpecialControl=Menu[nMenu].SpecialControl.size() ; // remember which special control this button is on. 
	SetButtonString_ColumnBox(nMenu, nButtonCount+nButtonOffset, pColumnBoxInfo->MaxColumn, COLUMNNAMESIZE, &pColumnBoxInfo->Name[0]) ;
	nButtonCount++ ;
	
	// set the fill info
	if(pColumnBoxInfo->FillInfo.MaxRow==0) 
		TempSpecialControl.ColumnBoxInfo.FillInfo.MaxRow=0 ; // 0 means no fill info has been set
	else
	{
		TempSpecialControl.ColumnBoxInfo.FillInfo.StartRow=pColumnBoxInfo->FillInfo.StartRow ;
		TempSpecialControl.ColumnBoxInfo.FillInfo.MaxRow=pColumnBoxInfo->FillInfo.MaxRow ;
		for(int nColumn=0 ; nColumn<COLUMNMAX ; nColumn++)
		{
			TempSpecialControl.ColumnBoxInfo.FillInfo.DataTextSize[nColumn]=pColumnBoxInfo->FillInfo.DataTextSize[nColumn] ;
			TempSpecialControl.ColumnBoxInfo.FillInfo.DataText[nColumn]=pColumnBoxInfo->FillInfo.DataText[nColumn] ;
		}		
	}
	// start the update count and time from 0. 
	TempSpecialControl.ColumnBoxInfo.FillInfo.UpdateCount=0 ;
	TempSpecialControl.ColumnBoxInfo.FillInfo.NextClickableTime=0.0 ;
	
	TempSpecialControl.ButtonEnd=Menu[nMenu].Button.size() ;
	Menu[nMenu].SpecialControl.push_back(TempSpecialControl) ;
	
	return Menu[nMenu].SpecialControl.size()-1 ;
	
}

void Ogre2dManager::UpdateColumnBoxInfo(int nMenu, int nColumnBox, COLUMNBOXFILLINFO* pColumnBoxFillInfo)
{
	// remember last time columnboxinfo was changed, to stop user accidentally clicking recently changed info.
	Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.NextClickableTime=m_GuiTime+334.0 ;
	Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.UpdateCount++ ;

	// set the fill info
	if(pColumnBoxFillInfo->MaxRow==0)
	{ 
		Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow=0 ; // 0 means no fill info has been set
		
		// update the slider
		int nSliderButton=Menu[nMenu].SpecialControl[nColumnBox].ButtonEnd-2 ;
		Menu[nMenu].Button[nSliderButton].MinVal=0.0f ;
		
	}
	else
	{
		Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.StartRow=pColumnBoxFillInfo->StartRow ;
		Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow=pColumnBoxFillInfo->MaxRow ;
		for(int nColumn=0 ; nColumn<COLUMNMAX ; nColumn++)
		{
			Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.DataTextSize[nColumn]=pColumnBoxFillInfo->DataTextSize[nColumn] ;
			Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.DataText[nColumn]=pColumnBoxFillInfo->DataText[nColumn] ;
		}	
		
		// update the slider
		int nSliderButton=Menu[nMenu].SpecialControl[nColumnBox].ButtonEnd-2 ;
		Menu[nMenu].Button[nSliderButton].MinVal=SliderPercent(Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow, Menu[nMenu].SpecialControl[nColumnBox].MaxLine, Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.StartRow) ;	
	}
	
	FillColumnBox(nMenu, nColumnBox) ;
}

float Ogre2dManager::SliderPercent(int nMaxRow, int nMaxLine, int nStartRow)
{
	int nMaxStartRow=nMaxRow-nMaxLine ;
	if(nMaxStartRow<1) return 0.0f ;
	
	float flPercent=(float)nStartRow/(float)nMaxStartRow ;
	if(flPercent<0.0f)
		flPercent=0.0f ;
	else
		if(flPercent>1.0f)
			flPercent=1.0f ;
			
	return flPercent ;
}

void Ogre2dManager::UpdateColumnBoxStartRow(int nMenu, int nColumnBox, int nStartRow)
{
	// what is the max startrow that fills the control?
	int nMaxStartRow=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow-Menu[nMenu].SpecialControl[nColumnBox].MaxLine ;
	if(nStartRow>nMaxStartRow) nStartRow=nMaxStartRow ;
	if(nStartRow<0) nStartRow=0 ;
	
	Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.StartRow=nStartRow ;
	
	// update the slider
	int nSliderButton=Menu[nMenu].SpecialControl[nColumnBox].ButtonEnd-2 ;
	Menu[nMenu].Button[nSliderButton].MinVal=SliderPercent(Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow, Menu[nMenu].SpecialControl[nColumnBox].MaxLine, nStartRow) ;
	
	FillColumnBox(nMenu, nColumnBox) ;
} 

void Ogre2dManager::FillColumnBox(int nMenu, int nColumnBox)
{
	int nMaxRow=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.MaxRow ;
	int nMaxLine=Menu[nMenu].SpecialControl[nColumnBox].MaxLine ;
	int nButtonOffset=Menu[nMenu].SpecialControl[nColumnBox].ButtonStart ;
	
	if(nMaxRow==0) // means there's no info
	{
		for(int nLine=0 ; nLine<nMaxLine ; nLine++)
			Menu[nMenu].Button[nLine+nButtonOffset].Name[0][0]='\0' ;
		
		return ; // nothing else to do.
	}

	int nMaxColumn=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.MaxColumn ;
	int nStartRow=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.StartRow ;
	char chTilde[2]={'~', '\0'} ;
	
	for(int nLine=0 ; nLine<nMaxLine ; nLine++)
	{
		int nRow=nStartRow+nLine ;
		if(nRow>=nMaxRow)
			Menu[nMenu].Button[nLine+nButtonOffset].Name[0][0]='\0' ;
		else
		{
			// create the new button name by merging together the fillinfo names, seperated by ~
			char chMerge[ABSOLUTE_MAX_BUTTON_TEXT] ;
			chMerge[0]='\0' ;
			for(int nColumn=0 ; nColumn<nMaxColumn ; nColumn++)
			{
				int nDataTextSize=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.DataTextSize[nColumn] ;
				int nPos=nDataTextSize*nRow ;
				char chTemp[ABSOLUTE_MAX_BUTTON_TEXT] ;
				
				// copy the name to a temp so we can truncate it to fit the space if needed
				strcpy(chTemp, Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.FillInfo.DataText[nColumn]+nPos) ;
				
				// repeated remove last character of text until the text fits on the line.
				int nLen=strlen(chTemp) ;
				int nMaxSize=Menu[nMenu].SpecialControl[nColumnBox].ColumnBoxInfo.ColumnSizeX[nColumn] ;
				
				while((nLen>4) && (PrintTextMED(chTemp, 0, 0, m_flTextScaleMedX, m_flTextScaleMedY, true, 0.0f, 0.0f, 0.0f)>=nMaxSize))
				{
					chTemp[nLen-3]='.' ;
					chTemp[nLen-2]='.' ;
					chTemp[nLen-1]='.' ;
					chTemp[nLen]='\0' ;
					nLen-- ;
				}
				
				strcat(chMerge, chTemp) ;
				strcat(chMerge, chTilde) ;
			}
			
			// remove final tilde
			int nLen=strlen(chMerge) ;
			chMerge[nLen-1]='\0' ;
			
			strcpy(Menu[nMenu].Button[nLine+nButtonOffset].Name[0], chMerge) ;
		}
	}	
	
	
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void Ogre2dManager::CreateButton(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nFrameSizeX, int nFrameSizeY, int nAction, int nStyle, bool bVisible, float flMin, float flMax, char *pLabel, char *pName, int nNameMem)
{
	Menu[nMenu].Button.push_back(m_NullButton) ;

	
	Menu[nMenu].Button[nButton].Action=nAction ;
	
	SetButtonLabel(nMenu, nButton, pLabel) ;


	// create the memory for the button names
	// most types of buttons don't need lots of memory for the extra switches and names, so work out the exceptions
	bool bNeedsSwitchMen=false ;
	
	// scan the name, see if it needs multiple names.
	// most buttons use the ~ character to indicate a new name.
	// BUTTONACTION_STATICTEXTCOLUMN uses it to indicate the next column.
	int nLen=strlen(pName) ;
	if((nAction!=BUTTONACTION_STATICTEXTCOLUMN) && (nAction!=BUTTONACTION_STATICTEXTCOLUMN_NOCLICK))
		for(int nPos=0 ; nPos<nLen ; nPos++)
			if(pName[nPos]=='~')
			{
				bNeedsSwitchMen=true ;
				break ;
			}
	
	if(!bNeedsSwitchMen)
	{
		// only the main button name needs memory
		Menu[nMenu].Button[nButton].Name[0]=new char [nNameMem] ;
		Menu[nMenu].Button[nButton].Name[0][0]='\0' ;
		Menu[nMenu].Button[nButton].NameMem[0]=nNameMem ;

		// just 1 byte placeholders for other switches
		for(int nSwitch=1 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		{
			Menu[nMenu].Button[nButton].Name[nSwitch]=new char [1] ;
			Menu[nMenu].Button[nButton].Name[nSwitch][0]='\0' ;
			Menu[nMenu].Button[nButton].NameMem[nSwitch]=1 ;
		}
	}
	else
		// switches set aside memory for all possible switches
		for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		{
			Menu[nMenu].Button[nButton].Name[nSwitch]=new char [nNameMem] ;
			Menu[nMenu].Button[nButton].Name[nSwitch][0]='\0' ;
			Menu[nMenu].Button[nButton].NameMem[nSwitch]=nNameMem ;
		}
		
	// automatically split out the different switch names if they exist, and set the names on the buttons.
	// Don't do this for BUTTONACTION_STATICTEXTCOLUMN, that will have to be handled specially after button creation by the menu, so just leave them blank for now.
	if((nAction==BUTTONACTION_STATICTEXTCOLUMN) || (nAction==BUTTONACTION_STATICTEXTCOLUMN_NOCLICK))
		Menu[nMenu].Button[nButton].MaxSwitch=0 ; // set the max switch to 0 even if we don't do anything else
	else
		SetButtonStrings(nMenu, nButton, pName) ;
	
	Menu[nMenu].Button[nButton].Visible=bVisible ;
	Menu[nMenu].Button[nButton].Switch=0 ;
	Menu[nMenu].Button[nButton].PositionX=nPositionX ;
	Menu[nMenu].Button[nButton].PositionY=nPositionY ;
	Menu[nMenu].Button[nButton].SizeX=nSizeX ;
	Menu[nMenu].Button[nButton].SizeY=nSizeY ;
	Menu[nMenu].Button[nButton].FrameSizeX=nFrameSizeX ;
	Menu[nMenu].Button[nButton].FrameSizeY=nFrameSizeY ;
	Menu[nMenu].Button[nButton].Style=nStyle ;
	Menu[nMenu].Button[nButton].MinVal=flMin ;
	Menu[nMenu].Button[nButton].MaxVal=flMax ;
	Menu[nMenu].Button[nButton].SpecialControl=-1 ;
	for(int nInfo=0 ; nInfo<MAXBUTTONINFO ; nInfo++)
		Menu[nMenu].Button[nButton].Info[nInfo]=0 ;
	
	// buttons have default white colour in almost all cases.  If something special is required, it'll have to be manually altered after the button is created.
	Menu[nMenu].Button[nButton].R=255 ;
	Menu[nMenu].Button[nButton].G=255 ;
	Menu[nMenu].Button[nButton].B=255 ;


}

// only used for a couple of MENU_SELECTOR buttons.
void Ogre2dManager::CreateButtonSpecial(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nFrameSizeX, int nFrameSizeY, int nAction, int nStyle, bool bVisible, float flMin, float flMax, char *pLabel, char *pName, int nMemorySize)
{
	Menu[nMenu].Button.push_back(m_NullButton) ;

	
	Menu[nMenu].Button[nButton].Action=nAction ;
	
	SetButtonLabel(nMenu, nButton, pLabel) ;


	// create the memory for the button names
	for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
	{
		Menu[nMenu].Button[nButton].Name[nSwitch]=new char [nMemorySize] ;
		Menu[nMenu].Button[nButton].Name[nSwitch][0]='\0' ;
		Menu[nMenu].Button[nButton].NameMem[nSwitch]=nMemorySize ;
	}


	SetButtonStrings(nMenu, nButton, pName) ;
	
	Menu[nMenu].Button[nButton].Visible=bVisible ;
	Menu[nMenu].Button[nButton].Switch=0 ;
	Menu[nMenu].Button[nButton].PositionX=nPositionX ;
	Menu[nMenu].Button[nButton].PositionY=nPositionY ;
	Menu[nMenu].Button[nButton].SizeX=nSizeX ;
	Menu[nMenu].Button[nButton].SizeY=nSizeY ;
	Menu[nMenu].Button[nButton].FrameSizeX=nFrameSizeX ;
	Menu[nMenu].Button[nButton].FrameSizeY=nFrameSizeY ;
	Menu[nMenu].Button[nButton].Style=nStyle ;
	Menu[nMenu].Button[nButton].MinVal=flMin ;
	Menu[nMenu].Button[nButton].MaxVal=flMax ;
	Menu[nMenu].Button[nButton].SpecialControl=-1 ;
	for(int nInfo=0 ; nInfo<MAXBUTTONINFO ; nInfo++)
		Menu[nMenu].Button[nButton].Info[nInfo]=0 ;
	
	// buttons have default white colour in almost all cases.  If something special is required, it'll have to be manually altered after the button is created.
	Menu[nMenu].Button[nButton].R=255 ;
	Menu[nMenu].Button[nButton].G=255 ;
	Menu[nMenu].Button[nButton].B=255 ;


}

void Ogre2dManager::CreateButtonSimple(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nAction, char *pLabel, char *pName)
{
	int nFrameSizeX=nSizeX+SIMPLEBUTTONFRAMESIZE ;
	int nFrameSizeY=nSizeY+SIMPLEBUTTONFRAMESIZE ;

	CreateButton(nMenu, nButton, nPositionX, nPositionY, nSizeX, nSizeY, nFrameSizeX, nFrameSizeY, nAction, BUTTONSTYLE_SIMPLE, true, -32768.0f, 32767.0f, pLabel, pName, MAX_BUTTON_TEXT) ;
}

void Ogre2dManager::CreateButtonSimpleLongName(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nAction, char *pLabel, char *pName)
{
	int nFrameSizeX=nSizeX+SIMPLEBUTTONFRAMESIZE ;
	int nFrameSizeY=nSizeY+SIMPLEBUTTONFRAMESIZE ;

	CreateButton(nMenu, nButton, nPositionX, nPositionY, nSizeX, nSizeY, nFrameSizeX, nFrameSizeY, nAction, BUTTONSTYLE_SIMPLE, true, -32768.0f, 32767.0f, pLabel, pName, ABSOLUTE_MAX_BUTTON_TEXT) ;
}



// define the various gui, menus and buttons
void Ogre2dManager::SetupGui()
{
	m_bCursorVisible=true ;

	char chMessage[256] ;
	
	CreateMenuB(MENU_SELECTOR, 0, 0, VSCREEN_W, VSCREEN_H, true, false) ;
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_NAME, VSCREEN_W/2-100, 2, 200, 12, BUTTONACTION_STATICTEXT, "", "SELECTOR PANEL") ;
	
	// more memory for the edit name, to accomadate full MAX_PATH names
	CreateButtonSpecial(MENU_SELECTOR, MENU_SELECTOR_EDIT, 272, VSCREEN_H-20, VSCREEN_W-360, 12, VSCREEN_W-360+4, 12+4, BUTTONACTION_EDIT_TEXT, BUTTONSTYLE_SIMPLE, true, -32768, 32768, "Name", "File Goes here", MAX_PATH) ;
	
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_CANCEL, 8, VSCREEN_H-20, 64, 12, BUTTONACTION_NONE, "", "Cancel") ;
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_OK, VSCREEN_W-72, VSCREEN_H-20, 64, 12, BUTTONACTION_NONE, "", "Ok") ;
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_SCROLLUP, 8, VSCREEN_H-40, 48, 12, BUTTONACTION_NONE, "", "<Prev") ;
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_SCROLLDOWN, VSCREEN_W-56, VSCREEN_H-40, 48, 12, BUTTONACTION_NONE, "", "Next>") ;
	
	// more memory for the folder name, to accomadate full MAX_PATH names
	CreateButtonSpecial(MENU_SELECTOR, MENU_SELECTOR_FOLDER, 272, VSCREEN_H-40, VSCREEN_W-360, 12, VSCREEN_W-360+4, 12+4, BUTTONACTION_EDIT_TEXT, BUTTONSTYLE_SIMPLE, true, -32768, 32768, "Dir", "File Goes here", MAX_PATH) ;
	
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_PARENT, 164, VSCREEN_H-40, 56, 12, BUTTONACTION_NONE, "", "Parent") ;
	CreateButtonSimple(MENU_SELECTOR, MENU_SELECTOR_DRIVES, 96, VSCREEN_H-40, 56, 12, BUTTONACTION_NONE, "", "Drives") ;

	for(int nButton=MENU_SELECTOR_FIRSTOPTION ; nButton<MENU_SELECTOR_FIRSTOPTION+MAX_SELECTOR_OPTION ; nButton++)
	{
		Menu[MENU_SELECTOR].Button.push_back(m_NullButton) ;

		sprintf(chMessage, "%i", nButton-MENU_SELECTOR_FIRSTOPTION+1) ;
		strcpy(Menu[MENU_SELECTOR].Button[nButton].Label, chMessage) ;

		// create the memory for the button names
		// Selectors use more memory, to allow for full MAX_PATH names on the buttons.
		for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		{
			Menu[MENU_SELECTOR].Button[nButton].Name[nSwitch]=new char [MAX_PATH] ;
			Menu[MENU_SELECTOR].Button[nButton].Name[nSwitch][0]='\0' ;
			Menu[MENU_SELECTOR].Button[nButton].NameMem[nSwitch]=MAX_PATH ;
		}
		sprintf(chMessage, "Possible Selection %i", nButton-MENU_SELECTOR_FIRSTOPTION) ;
		strcpy(Menu[MENU_SELECTOR].Button[nButton].Name[0], chMessage) ;
		
		Menu[MENU_SELECTOR].Button[nButton].Visible=true ;
		Menu[MENU_SELECTOR].Button[nButton].MaxSwitch=0 ;
		Menu[MENU_SELECTOR].Button[nButton].Switch=0 ;
		
		int nBY=nButton-MENU_SELECTOR_FIRSTOPTION ;
		if(nBY<MAX_SELECTOR_OPTION/3)
		{
			Menu[MENU_SELECTOR].Button[nButton].PositionX=(VSCREEN_W*0.035) ;
			Menu[MENU_SELECTOR].Button[nButton].PositionY=20+nBY*13 ;
		}
		else
		if(nBY<MAX_SELECTOR_OPTION*2/3)
		{
			Menu[MENU_SELECTOR].Button[nButton].PositionX=VSCREEN_W*0.333 + (VSCREEN_W*0.035) ;
			Menu[MENU_SELECTOR].Button[nButton].PositionY=20+(nBY-MAX_SELECTOR_OPTION/3)*13 ;
		}
		else
		{
			Menu[MENU_SELECTOR].Button[nButton].PositionX=VSCREEN_W*0.667 + (VSCREEN_W*0.035) ;
			Menu[MENU_SELECTOR].Button[nButton].PositionY=20+(nBY-MAX_SELECTOR_OPTION*2/3)*13 ;
		}
		
		Menu[MENU_SELECTOR].Button[nButton].SizeX=VSCREEN_W*0.295 ;
		Menu[MENU_SELECTOR].Button[nButton].SizeY=13 ;
		Menu[MENU_SELECTOR].Button[nButton].FrameSizeX=VSCREEN_W*0.295 ;
		Menu[MENU_SELECTOR].Button[nButton].FrameSizeY=13 ;
		Menu[MENU_SELECTOR].Button[nButton].Action=BUTTONACTION_NONE ;
		Menu[MENU_SELECTOR].Button[nButton].Style=BUTTONSTYLE_SIMPLE ;
		
		Menu[MENU_SELECTOR].Button[nButton].R=255 ;
		Menu[MENU_SELECTOR].Button[nButton].G=255 ;
		Menu[MENU_SELECTOR].Button[nButton].B=255 ;
		
		Menu[MENU_SELECTOR].Button[nButton].SpecialControl=-1 ;
		for(int nInfo=0 ; nInfo<MAXBUTTONINFO ; nInfo++)
			Menu[MENU_SELECTOR].Button[nButton].Info[nInfo]=0 ;
		
	}


	

	int nMaxButton=MAX_BUTTON_SWITCH ;
	//////////////////////////////////////////////////////////////////////////////////////////////////
	// create a slot for the blank message box menu and buttons
	CreateMenuB(MENU_INFOBOX, 0, 0, 0, 0, true, false) ;
	for(int nButton=0 ; nButton<MENU_INFOBOX_MAX ; nButton++)
	{
		Menu[MENU_INFOBOX].Button.push_back(m_NullButton) ;
		strcpy(Menu[MENU_INFOBOX].Button[nButton].Label, "") ;
		// create the memory for the button names
		for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		{
			Menu[MENU_INFOBOX].Button[nButton].Name[nSwitch]=new char [ABSOLUTE_MAX_BUTTON_TEXT] ;
			Menu[MENU_INFOBOX].Button[nButton].Name[nSwitch][0]='\0' ;
			Menu[MENU_INFOBOX].Button[nButton].NameMem[nSwitch]=ABSOLUTE_MAX_BUTTON_TEXT ;
		}
		strcpy(Menu[MENU_INFOBOX].Button[nButton].Name[0], "") ;
		Menu[MENU_INFOBOX].Button[nButton].Visible=false ;
		Menu[MENU_INFOBOX].Button[nButton].MaxSwitch=0 ;
		Menu[MENU_INFOBOX].Button[nButton].Switch=0 ;
		Menu[MENU_INFOBOX].Button[nButton].PositionX=0 ;
		Menu[MENU_INFOBOX].Button[nButton].SizeX=0 ;
		Menu[MENU_INFOBOX].Button[nButton].SizeY=0 ;
		Menu[MENU_INFOBOX].Button[nButton].FrameSizeX=0 ;
		Menu[MENU_INFOBOX].Button[nButton].FrameSizeY=0 ;
		if(nButton<=MENU_INFOBOX_NO)
			Menu[MENU_INFOBOX].Button[nButton].Action=BUTTONACTION_NONE ;
		else
			Menu[MENU_INFOBOX].Button[nButton].Action=BUTTONACTION_STATICTEXT ;
		Menu[MENU_INFOBOX].Button[nButton].Style=BUTTONSTYLE_SIMPLE ;
		
		Menu[MENU_INFOBOX].Button[nButton].R=255 ;
		Menu[MENU_INFOBOX].Button[nButton].G=255 ;
		Menu[MENU_INFOBOX].Button[nButton].B=255 ;
		
		Menu[MENU_INFOBOX].Button[nButton].SpecialControl=-1 ;
		for(int nInfo=0 ; nInfo<MAXBUTTONINFO ; nInfo++)
			Menu[MENU_INFOBOX].Button[nButton].Info[nInfo]=0 ;
	}

		///////////////////////////////////////////////////////////////////////////////////////////////
	// create a slot for the blank listbox menu and buttons

	
	
	CreateMenuB(MENU_LISTBOX, 0, 0, 0, 0, true, false) ;
	for(int nButton=0 ; nButton<MAX_BUTTON_SWITCH ; nButton++)
	{
		Menu[MENU_LISTBOX].Button.push_back(m_NullButton) ;
		strcpy(Menu[MENU_LISTBOX].Button[nButton].Label, "") ;
		// create the memory for the button names
		for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		{
			Menu[MENU_LISTBOX].Button[nButton].Name[nSwitch]=new char [MAX_BUTTON_TEXT] ;
			Menu[MENU_LISTBOX].Button[nButton].Name[nSwitch][0]='\0' ;
			Menu[MENU_LISTBOX].Button[nButton].NameMem[nSwitch]=MAX_BUTTON_TEXT ;
		}
		strcpy(Menu[MENU_LISTBOX].Button[nButton].Name[0], "") ;
		Menu[MENU_LISTBOX].Button[nButton].Visible=false ;
		Menu[MENU_LISTBOX].Button[nButton].MaxSwitch=0 ;
		Menu[MENU_LISTBOX].Button[nButton].Switch=0 ;
		Menu[MENU_LISTBOX].Button[nButton].PositionX=0 ;
		Menu[MENU_LISTBOX].Button[nButton].SizeX=0 ;
		Menu[MENU_LISTBOX].Button[nButton].SizeY=0 ;
		Menu[MENU_LISTBOX].Button[nButton].FrameSizeX=0 ;
		Menu[MENU_LISTBOX].Button[nButton].FrameSizeY=0 ;
		Menu[MENU_LISTBOX].Button[nButton].Action=BUTTONACTION_NONE ;
		Menu[MENU_LISTBOX].Button[nButton].Style=BUTTONSTYLE_SIMPLE ;
		
		Menu[MENU_LISTBOX].Button[nButton].R=255 ;
		Menu[MENU_LISTBOX].Button[nButton].G=255 ;
		Menu[MENU_LISTBOX].Button[nButton].B=255 ;
		
		Menu[MENU_LISTBOX].Button[nButton].SpecialControl=-1 ;
		for(int nInfo=0 ; nInfo<MAXBUTTONINFO ; nInfo++)
			Menu[MENU_LISTBOX].Button[nButton].Info[nInfo]=0 ;
	}
	

}

bool Ogre2dManager::SetButtonLabel(int nMenu, int nButton, char *pLabel)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	strcpy(Menu[nMenu].Button[nButton].Label, pLabel) ;
	return true ;

}

bool Ogre2dManager::ResetButtonStrings(int nMenu, int nButton)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	for(int nSwitch=0 ; nSwitch<MAX_BUTTON_SWITCH ; nSwitch++)
		Menu[nMenu].Button[nButton].Name[nSwitch][0]='\0' ;

	Menu[nMenu].Button[nButton].MaxSwitch=0 ;
	
	return true ;
}

// add a switch name, and sets this as the last switch.  Added to the current MaxSwitch position.
// This should be called as part of a sequence of setting the switch names, from first to last,
// after ResetButtonStrings has been called.
bool Ogre2dManager::AddButtonString(int nMenu, int nButton, char *pName)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	int nSwitch=Menu[nMenu].Button[nButton].MaxSwitch ;
	if(nSwitch>=MAX_BUTTON_SWITCH)
		return false ;

	if(strlen(pName)>=Menu[nMenu].Button[nButton].NameMem[nSwitch])
		return false ;

	strcpy(Menu[nMenu].Button[nButton].Name[nSwitch], pName) ;
	Menu[nMenu].Button[nButton].MaxSwitch=nSwitch+1 ;
	return false ;

}

bool Ogre2dManager::SetButtonStrings(int nMenu, int nButton, char *pName)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	int nNamePos=0 ;
	int nLen=strlen(pName) ;
	int nNameMax=0 ;
	int nAction =Menu[nMenu].Button[nButton].Action ; 
	

	// copy in the button names.  There may be more than one, seperated by the ~ symbol
	for(int nPos=0 ; nPos<=nLen ; nPos++)
	{
		Menu[nMenu].Button[nButton].Name[nNameMax][nNamePos]=pName[nPos] ;
		if((pName[nPos]=='~') || (pName[nPos]=='\0'))
		{
			Menu[nMenu].Button[nButton].Name[nNameMax][nNamePos]='\0' ;
			nNamePos=0 ;

			nNameMax++ ;
		}
		else
		{
			nNamePos++ ;
			if(nNamePos>=Menu[nMenu].Button[nButton].NameMem[nNameMax]-1) // check if too long, leave room for null
				return false ;
		}
	}

	if((nAction==BUTTONACTION_NONE) || (nAction==BUTTONACTION_LIST))
		Menu[nMenu].Button[nButton].MaxSwitch=nNameMax ;
	else
		if(nAction==BUTTONACTION_CHECKBOX)
			Menu[nMenu].Button[nButton].MaxSwitch=2 ;
		else
			Menu[nMenu].Button[nButton].MaxSwitch=0 ;

	return true ;

}

// combine all columnbox names into a single string, and remember the start location of each string.
bool Ogre2dManager::SetButtonString_ColumnBox(int nMenu, int nButton, int nMaxName, int nMaxNameSize, char *pName)
{
	
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;
			
	// how long will the final name be?  Equivalent to length of each name plus a ~ for all but the last one.
	int nNameLen=nMaxName-1 ; // for the ~ characters after each name except the last
	for(int nName=0 ; nName<nMaxName ; nName++) 
		nNameLen+=strlen(pName+nName*nMaxNameSize) ;
		
	// if this is longer than the allowed space for the name, return.
	if(nNameLen>Menu[nMenu].Button[nButton].NameMem[0])
		return false ;
	
	
	// create the name
	int nNamePos=0 ;
	char chTilde[2]={'~', '\0'} ;
	Menu[nMenu].Button[nButton].Name[0][0]='\0' ;
	for(int nName=0 ; nName<nMaxName-1 ; nName++)
	{
		strcat(Menu[nMenu].Button[nButton].Name[0], pName+nName*nMaxNameSize) ;
		strcat(Menu[nMenu].Button[nButton].Name[0], chTilde) ;
	}
	
	// last name has no ~ on end.
	strcat(Menu[nMenu].Button[nButton].Name[0], pName+(nMaxName-1)*nMaxNameSize) ;

	
}

// set a particular switch name.  Doesn't check or update maxswitch
bool Ogre2dManager::SetButtonString(int nMenu, int nButton, int nSwitch, char *pName)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	if(strlen(pName)>=Menu[nMenu].Button[nButton].NameMem[nSwitch])
		return false ;

	strcpy(Menu[nMenu].Button[nButton].Name[nSwitch], pName) ;

	return true ;
}



// set switch by number. returns true on success  
bool Ogre2dManager::SetButtonSwitch(int nMenu, int nButton, int nSwitch)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	if((nSwitch>=0) && (nSwitch<Menu[nMenu].Button[nButton].MaxSwitch))
	{
		Menu[nMenu].Button[nButton].Switch=nSwitch ;
		return true ;
	}

	return false ;
}

// set switch by matching string.  returns true on success
bool Ogre2dManager::SetButtonSwitch(int nMenu, int nButton, char *pSwitch) 
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	int nMaxSwitch=Menu[nMenu].Button[nButton].MaxSwitch ;
	for(int nSwitch=0 ; nSwitch<nMaxSwitch ; nSwitch++)
		if(strcmp(pSwitch, Menu[nMenu].Button[nButton].Name[nSwitch])==0)
		{
			Menu[nMenu].Button[nButton].Switch=nSwitch ;
			return true ;
		}

	return false ;
}

int Ogre2dManager::GetButtonSwitch(int nMenu, int nButton)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return 0 ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return 0 ;


	return Menu[nMenu].Button[nButton].Switch ;

}


bool Ogre2dManager::SetEditBoxInteger(int nMenu, int nButton, int nNum)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	if(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_EDIT_INTEGER)
		return false ;

	if((nNum<Menu[nMenu].Button[nButton].MinVal) || (nNum>Menu[nMenu].Button[nButton].MaxVal))
		return false ;

	sprintf(Menu[nMenu].Button[nButton].Name[0], "%i", nNum) ;

	return true ;

}

int Ogre2dManager::GetEditBoxInteger(int nMenu, int nButton)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return 0 ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return 0 ;

	if(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_EDIT_INTEGER)
		return 0 ;

	return atoi(Menu[nMenu].Button[nButton].Name[0]) ;

}

bool Ogre2dManager::SetEditBoxFloat(int nMenu, int nButton, float flNum)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	if(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_EDIT_FLOAT)
		return false ;

	if((flNum<Menu[nMenu].Button[nButton].MinVal) || (flNum>Menu[nMenu].Button[nButton].MaxVal))
		return false ;

	sprintf(Menu[nMenu].Button[nButton].Name[0], "%f", flNum) ;

	return true ;

}

float Ogre2dManager::GetEditBoxFloat(int nMenu, int nButton)
{
	if((nMenu<0) || (nMenu>=Menu.size())) 
		return 0.0f ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return 0.0f ;

	if(Menu[nMenu].Button[nButton].Action!=BUTTONACTION_EDIT_FLOAT)
		return 0.0f ;

	return atof(Menu[nMenu].Button[nButton].Name[0]) ;

}

bool Ogre2dManager::SetButtonMin(int nMenu, int nButton, float flMin)
{
		if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	Menu[nMenu].Button[nButton].MinVal=flMin ;

	return true ;
}

bool Ogre2dManager::SetButtonMax(int nMenu, int nButton, float flMax)
{
		if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	Menu[nMenu].Button[nButton].MaxVal=flMax ;

	return true ;
}

bool Ogre2dManager::SetButtonMinMax(int nMenu, int nButton, float flMin, float flMax)
{
		if((nMenu<0) || (nMenu>=Menu.size())) 
		return false ;

	if((nButton<0) || (nButton>Menu[nMenu].Button.size()))
		return false ;

	Menu[nMenu].Button[nButton].MinVal=flMin ;
	Menu[nMenu].Button[nButton].MaxVal=flMax ;

	return true ;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////


void Ogre2dManager::UpdateInfoText()
{
	// if it's time to scroll, move up all the text
	if((m_nInfoTextLongDisplay==0) && (m_GuiTime>=m_dInfoTextTime))
		return ;


	// draw the text

	float flRd, flGr, flBl ;
	int nMaxLine=3 ;
	if(m_nInfoTextLongDisplay) nMaxLine=m_nInfoTextLongDisplay-1 ;//47-1 ;
	int nFirstLine=m_nInfoTextLine-(nMaxLine-1) ;
	int nArrayLine=0 ;
	int nPos=0 ;
	
	for(int nLine=m_nInfoTextLine-nMaxLine ; nLine<m_nInfoTextLine ; nLine++)
	{
		nArrayLine=nLine ;
		if(nArrayLine<0) nArrayLine+=MAX_INFOTEXT_LINES ;
		flRd=m_flInfoTextColour[nArrayLine][0] ;
		flGr=m_flInfoTextColour[nArrayLine][1] ;
		flBl=m_flInfoTextColour[nArrayLine][2] ;
		//PrintTextMED(m_chInfoText[nArrayLine], 0, nPos*10, 0.4, 0.6, false, flRd*0.75, flGr*0.75, flBl*0.75) ;
		PrintTextMED(m_chInfoText[nArrayLine], 0, nPos*m_flInfoTextLineHeight, m_flInfoTextFontScaleX, m_flInfoTextFontScaleY, false, flRd*0.90, flGr*0.90, flBl*0.90) ;
		nPos++ ;
	}

	flRd=m_flInfoTextColour[m_nInfoTextLine][0] ;
	flGr=m_flInfoTextColour[m_nInfoTextLine][1] ;
	flBl=m_flInfoTextColour[m_nInfoTextLine][2] ;
	PrintTextMED(m_chInfoText[m_nInfoTextLine], 0, nMaxLine*m_flInfoTextLineHeight, m_flInfoTextFontScaleX, m_flInfoTextFontScaleY, false, flRd, flGr, flBl) ; // last message is bigger and brighter
}


void Ogre2dManager::AddInfoText(char chMessage[], float flRd, float flGr, float flBl)
{

	// copy in new line
	m_nInfoTextLine++ ;
	if(m_nInfoTextLine>=MAX_INFOTEXT_LINES)
		m_nInfoTextLine=0 ;

	strcpy_s(m_chInfoText[m_nInfoTextLine], MAX_INFOTEXT_CHAR, chMessage) ;
	m_flInfoTextColour[m_nInfoTextLine][0]=flRd ;
	m_flInfoTextColour[m_nInfoTextLine][1]=flGr ;
	m_flInfoTextColour[m_nInfoTextLine][2]=flBl ;

	m_dInfoTextTime=m_GuiTime+INFOTEXT_CLEARTIME ;
}

void Ogre2dManager::AddInfoTextBrief(char chMessage[], float flRd, float flGr, float flBl, int nDelay)
{

	// copy in new line
	m_nInfoTextLine++ ;
	if(m_nInfoTextLine>=MAX_INFOTEXT_LINES)
		m_nInfoTextLine=0 ;

	strcpy_s(m_chInfoText[m_nInfoTextLine], MAX_INFOTEXT_CHAR, chMessage) ;
	m_flInfoTextColour[m_nInfoTextLine][0]=flRd ;
	m_flInfoTextColour[m_nInfoTextLine][1]=flGr ;
	m_flInfoTextColour[m_nInfoTextLine][2]=flBl ;

	m_dInfoTextTime=m_GuiTime+nDelay ;
}

void Ogre2dManager::AddInfoTextData(char* pMessage, int Num0, float flRd, float flGr, float flBl)
{
	char chMessage[MISCSTRINGSIZE] ;
	sprintf(chMessage, pMessage, Num0) ;
	AddInfoText(chMessage, flRd, flGr, flBl) ;

}

void Ogre2dManager::AddInfoTextData(char* pMessage, float Num0, float flRd, float flGr, float flBl)
{
	char chMessage[MISCSTRINGSIZE] ;
	sprintf(chMessage, pMessage, Num0) ;
	AddInfoText(chMessage, flRd, flGr, flBl) ;

}




























