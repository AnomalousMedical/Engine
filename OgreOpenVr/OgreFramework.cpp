//|||||||||||||||||||||||||||||||||||||||||||||||
#include "stdafx.h"

#include "OgreFramework.hpp"
#include "OgreD3D11RenderSystem.h"
#include "OgreD3D11Texture.h"
#include "OgreRectangle2D.h"

//|||||||||||||||||||||||||||||||||||||||||||||||


using namespace Ogre;

//|||||||||||||||||||||||||||||||||||||||||||||||

template<> OgreFramework* Ogre::Singleton<OgreFramework>::msSingleton = 0;

//|||||||||||||||||||||||||||||||||||||||||||||||
// Test2
OgreFramework::OgreFramework()
{
	m_MoveSpeed = 0.1f;
	m_RotateSpeed = 0.3f;

	m_bShutDownOgre = false;
	m_iNumScreenShots = 0;

	m_pSceneManager = 0;
	m_pLog = 0;
	m_pRenderSystem = 0;
	m_previousError = vr::VRCompositorError_None;
}


bool OgreFramework::initOgre(Ogre::Root* root, Ogre::SceneManager* sceneManager)
{
	m_pLog = Ogre::LogManager::getSingletonPtr()->getDefaultLog();

	m_pRenderSystem = root->getRenderSystem();

	m_pSceneManager = sceneManager;
	m_pSceneManager->setAmbientLight(Ogre::ColourValue(0.7f, 0.7f, 0.7f));

	g_IPD = 0.065;
	g_poseNeutralPosition = Ogre::Vector3(0.0f, 0.0f, 0.0f);
	g_poseNeutralOrientation = Ogre::Quaternion(1.0f, 0.0f, 0.0f, 0.0f);

	float flSize = 16.0f;

	PlayerPos = Ogre::Vector3(0.0f, 0.0f, 0.0f);
	PlayerSpeed = 1;
	PlayerSpeedPress = false;

	return initOpenVR();

	return false;
}


//|||||||||||||||||||||||||||||||||||||||||||||||

OgreFramework::~OgreFramework()
{
	/*DeleteControllerModel(HAND0);
	DeleteControllerModel(HAND1);*/

	//THREAX-TODO: Handle erasing the cameras and their scene nodes here

	Ogre::TextureManager::getSingleton().remove("RttTex_VR_L");
	RTT_Texture_VR_L->~Texture();
	RTT_Mat_VR_L->removeAllTechniques();

	Ogre::TextureManager::getSingleton().remove("RttTex_VR_R");
	RTT_Texture_VR_R->~Texture();
	RTT_Mat_VR_R->removeAllTechniques();

	//THREAX-TODO: Figure out this vr shutdown thing below

	//vr::VR_Shutdown() ; // putting it here causes an debug break in OgreComPtr

	//if(m_pRoot)     delete m_pRoot;


	//vr::VR_Shutdown() ;  // putting it here causes an debug break in VR_ShutdownInternal

	// VR_Shutdown does not seem to play nice with Ogre, possibly something to do with DirectX
	// we get a break in debug on shutdown whether we have it before deleting m_pRoot or after.
	// so just skip it

	// Ended up moving shutdown into the DemoApp mainloop, I think the error is related to the main render window.
	// So in the mainloop, we catch that Ogre is going to shutdown, and then shutdown VR before Ogre shuts down the main render window.
}

//|||||||||||||||||||||||||||||||||||||||||||||||

void OgreFramework::updateOgre(double timeSinceLastFrame)
{
	m_pRenderSystem->clearFrameBuffer(FBT_COLOUR | FBT_DEPTH, Ogre::ColourValue(0.0, 0.0, 0.0, 1));

	m_nBatchCount = 0;
	m_nTriCount = 0;

	m_MoveScale = m_MoveSpeed * (float)timeSinceLastFrame;
	m_RotScale = m_RotateSpeed * (float)timeSinceLastFrame;

	m_TranslateVector = Vector3::ZERO;

	handleInput(); // vr input, includes HMD and controllers

	/////////////////////////////////////////////////
	int nHand = -1;
	if (m_nControllerTDI[HAND0] != NOCONTROLLER)
	{
		//if(m_ControllerData[HAND0].Updated)
		//{
			//sprintf(m_chMessage, "C0: A%i G%i PP %i T%i : PT %i X%.2f Y%.2f : T%i", m_ControllerData[HAND0].AppMenu, m_ControllerData[HAND0].Grip, m_ControllerData[HAND0].PadPress, m_ControllerData[HAND0].Trigger, m_ControllerData[HAND0].PadTouch, m_ControllerData[HAND0].Pad.x, m_ControllerData[HAND0].Pad.y, (int)timeSinceLastFrame) ;
			//Ogre2DGui->AddInfoText(m_chMessage, 1,1,1) ;
		//}
		nHand = HAND0;
	}
	else
		if (m_nControllerTDI[HAND1] != NOCONTROLLER)
		{
			//if(m_ControllerData[HAND1].Updated)
			//{
				//sprintf(m_chMessage, "C1: A%i G%i PP %i T%i : PT %i X%.2f Y%.2f : T%i", m_ControllerData[HAND1].AppMenu, m_ControllerData[HAND1].Grip, m_ControllerData[HAND1].PadPress, m_ControllerData[HAND1].Trigger, m_ControllerData[HAND1].PadTouch, m_ControllerData[HAND1].Pad.x, m_ControllerData[HAND1].Pad.y, (int)timeSinceLastFrame) ;
				//Ogre2DGui->AddInfoText(m_chMessage, 1,1,1) ;
			//}
			nHand = HAND1;
		}

	if (nHand > -1)
	{
		if (m_ControllerData[nHand].PadPress)
		{
			if (PlayerSpeedPress == false)
			{
				PlayerSpeed++;
				if (PlayerSpeed > 5) PlayerSpeed = 1;
			}
			PlayerSpeedPress = true;
		}
		else
			PlayerSpeedPress = false;


		float flSpeed = PlayerSpeed * 0.02f;
		float flPadLen = m_ControllerData[nHand].Pad.length();
		if ((m_ControllerData[nHand].PadTouch == 1) && (m_ControllerData[nHand].Pad.length() >= 0.334))
		{
			// multiply orienation of controller by a 3D version of the pad vector
			// suppress small deviations the player probably doesn't intend.
			float flX = m_ControllerData[nHand].Pad.x;
			if (abs(flX) < 0.2f) flX = 0.0f;
			float flZ = -m_ControllerData[nHand].Pad.y;
			if (abs(flZ) < 0.2f) flZ = 0.0f;

			Ogre::Vector3 Pad = Ogre::Vector3(flX, 0.0f, flZ);

			Pad = m_ControllerData[nHand].Orientation*Pad;
			Pad.y = 0.0f;
			Pad.normalise();

			PlayerPos.x += Pad.x*flPadLen*flSpeed;
			PlayerPos.z += Pad.z*flPadLen*flSpeed;

		}

		if (m_ControllerData[nHand].Trigger)
			PlayerPos.y += flSpeed;

		if (m_ControllerData[nHand].Grip)
			PlayerPos.y -= flSpeed;

	}

	for (int nLoop = 0; nLoop < MAXCONTROLLER; nLoop++)
		if (m_nControllerTDI[nLoop] != NOCONTROLLER)
		{
			m_ControllerSN[nLoop]->setPosition(m_ControllerData[nLoop].Position + PlayerPos);
			m_ControllerSN[nLoop]->setOrientation(m_ControllerData[nLoop].Orientation);
		}

	UpdateVR();

	g_frameIndex++;
}

//|||||||||||||||||||||||||||||||||||||||||||||||

bool OgreFramework::initOpenVR()
{
	g_frameIndex = 0;

	for (int nLoop = 0; nLoop < MAXCONTROLLER; nLoop++)
	{
		m_nControllerTDI[nLoop] = NOCONTROLLER;
		m_bControllerHasModel[nLoop] = false;
	}
	/////////////////////////////////////////////////////////////////////////
	// based on code from OpenVR hellovr_opengl sample

	// Loading the SteamVR Runtime
	vr::EVRInitError eError = vr::VRInitError_None;
	m_pHMD = vr::VR_Init(&eError, vr::VRApplication_Scene);
	if (eError != vr::VRInitError_None)
	{
		m_pHMD = NULL;
		char buf[1024];
		sprintf_s(buf, sizeof(buf), "Unable to init VR runtime: %s", vr::VR_GetVRInitErrorAsEnglishDescription(eError));
		m_pLog->logMessage(buf);
		return false;
	}



	m_strDriver = "No Driver";
	m_strDisplay = "No Display";
	m_strDriver = GetTrackedDeviceString(m_pHMD, vr::k_unTrackedDeviceIndex_Hmd, vr::Prop_TrackingSystemName_String);
	m_strDisplay = GetTrackedDeviceString(m_pHMD, vr::k_unTrackedDeviceIndex_Hmd, vr::Prop_SerialNumber_String);

	m_pHMD->GetRecommendedRenderTargetSize(&m_nRenderWidth, &m_nRenderHeight);

	//
	///////////////////////////////////////////////////////////////////////////


	// update() gets called before the first updateHMDPos() so we need to make sure this is initialized to something
	g_mat4HMDPose = Ogre::Matrix4::IDENTITY;
	for (int nLoop = 0; nLoop < MAXCONTROLLER; nLoop++)
		g_mat4ControllerPose[nLoop] = Ogre::Matrix4::IDENTITY;

	if (!vr::VRCompositor())
	{
		m_pLog->logMessage("vr::VRCompositor() failed.");
		return false;
	}

	InitOgreCameras();

	InitOgreTextures();

	SetupRenderModels();

	m_pLog->stream() << "VR Setup Complete";

	return true;
}

// taken directly from OpenVR hellovr_opengl sample
//-----------------------------------------------------------------------------
// Purpose: Helper to get a string from a tracked device property and turn it
//			into a std::string
//-----------------------------------------------------------------------------
std::string OgreFramework::GetTrackedDeviceString(vr::IVRSystem *pHmd, vr::TrackedDeviceIndex_t unDevice, vr::TrackedDeviceProperty prop, vr::TrackedPropertyError *peError)
{
	uint32_t unRequiredBufferLen = pHmd->GetStringTrackedDeviceProperty(unDevice, prop, NULL, 0, peError);
	if (unRequiredBufferLen == 0)
		return "";

	char *pchBuffer = new char[unRequiredBufferLen];
	unRequiredBufferLen = pHmd->GetStringTrackedDeviceProperty(unDevice, prop, pchBuffer, unRequiredBufferLen, peError);
	std::string sResult = pchBuffer;
	delete[] pchBuffer;
	return sResult;
}

// I'm only interested in the hand controllers, 2 at most
void OgreFramework::SetupRenderModels()
{

	if (!m_pHMD)
		return;

	for (uint32_t unTrackedDevice = vr::k_unTrackedDeviceIndex_Hmd + 1; unTrackedDevice < vr::k_unMaxTrackedDeviceCount; unTrackedDevice++)
	{
		if (!m_pHMD->IsTrackedDeviceConnected(unTrackedDevice))
			continue;

		sprintf(m_chMessage, "TrackedDeviceConnected, Index %i, Role %i, Class %i", (int)unTrackedDevice, (int)m_pHMD->GetControllerRoleForTrackedDeviceIndex(unTrackedDevice), (int)m_pHMD->GetTrackedDeviceClass(unTrackedDevice));
		m_pLog->logMessage(m_chMessage);

		if (m_pHMD->GetTrackedDeviceClass(unTrackedDevice) == vr::ETrackedDeviceClass::TrackedDeviceClass_Controller)
		{
			int nHand = -1;
			if ((m_nControllerTDI[HAND0] == NOCONTROLLER) && (m_nControllerTDI[HAND1] != (int)unTrackedDevice))
				nHand = HAND0;
			else
				if ((m_nControllerTDI[HAND1] == NOCONTROLLER) && (m_nControllerTDI[HAND0] != (int)unTrackedDevice))
					nHand = HAND1;

			if (nHand > -1)
			{
				sprintf(m_chMessage, "Adding TrackedDevice %i as Hand%i", (int)unTrackedDevice, nHand);
				m_pLog->logMessage(m_chMessage);

				m_nControllerTDI[nHand] = unTrackedDevice;
				//CreateControllerModel(nHand) ;
			}
		}
	}

}

void OgreFramework::InitOgreCameras()
{
	m_pCameraNode = m_pSceneManager->getRootSceneNode()->createChildSceneNode();
	m_pCameraL = m_pSceneManager->createCamera("EyeL");
	m_pCameraR = m_pSceneManager->createCamera("EyeR");

	Ogre::Camera* pCam;
	Ogre::Matrix4 proj;
	for (int i = 0; i < 2; ++i)
	{
		if (i == 0)
			pCam = m_pCameraL;
		else
			pCam = m_pCameraR;

		Ogre::Matrix4 camMat4 = getHMDMatrixPoseEye(static_cast<vr::Hmd_Eye>(i));

		// seems like it should work-- the eyes are swapped though?
		// depending how OpenVR does it, this might give us the exact 
		// IPD that the user has set - instead of having to set it in the app
		//Ogre::Vector3 camPos = camMat4.getTrans(); 
		const Ogre::Vector3 camPos(g_IPD * (i - 0.5f), 0, 0);
		Ogre::Quaternion camOrientation = camMat4.extractQuaternion();

		pCam->setPosition(camPos);
		pCam->setNearClipDistance(FARCLIP);
		pCam->setFarClipDistance(NEARCLIP);
		pCam->setAutoAspectRatio(true);
		pCam->detachFromParent();
		m_pCameraNode->attachObject(pCam);
		pCam->setDirection(0, 0, -1);
		m_pCameraNode->setOrientation(camOrientation);

		// get and set the projection matrix for this camera
		proj = getHMDMatrixProjectionEye(static_cast<vr::Hmd_Eye>(i));
		pCam->setCustomProjectionMatrix(true, proj);
	}

	m_pLog->stream() << "Created VR Eye Cameras";
}

// ----------------------------------------------------------------
// gets the projection matrix for the specified eye
// ----------------------------------------------------------------
Ogre::Matrix4 OgreFramework::getHMDMatrixProjectionEye(vr::Hmd_Eye nEye)
{
	if (!m_pHMD)
		return Ogre::Matrix4();

	// there may be a bug in openvr where this is returning a directx style projection matrix regardless of the api specified
	vr::HmdMatrix44_t mat = m_pHMD->GetProjectionMatrix(nEye, NEARCLIP, FARCLIP);// , vr::API_DirectX);

	// convert to Ogre::Matrix4
	return Ogre::Matrix4(
		mat.m[0][0], mat.m[0][1], mat.m[0][2], mat.m[0][3],
		mat.m[1][0], mat.m[1][1], mat.m[1][2], mat.m[1][3],
		mat.m[2][0], mat.m[2][1], mat.m[2][2], mat.m[2][3],
		mat.m[3][0], mat.m[3][1], mat.m[3][2], mat.m[3][3]
	);
}

// ----------------------------------------------------------------
// gets the location of the specified eye
// ----------------------------------------------------------------
Ogre::Matrix4 OgreFramework::getHMDMatrixPoseEye(vr::Hmd_Eye nEye)
{
	if (!m_pHMD)
		return Ogre::Matrix4();

	vr::HmdMatrix34_t matEye = m_pHMD->GetEyeToHeadTransform(nEye);
	Ogre::Matrix4 eyeTransform = convertSteamVRMatrixToOgreMatrix4(matEye);

	return eyeTransform.inverse();
}

// ----------------------------------------------------------------
// converts an OpenVR 3x4 matrix to Ogre::Matrix4
// ----------------------------------------------------------------
Ogre::Matrix4 OgreFramework::convertSteamVRMatrixToOgreMatrix4(const vr::HmdMatrix34_t &matPose)
{
	return Ogre::Matrix4(
		matPose.m[0][0], matPose.m[0][1], matPose.m[0][2], matPose.m[0][3],
		matPose.m[1][0], matPose.m[1][1], matPose.m[1][2], matPose.m[1][3],
		matPose.m[2][0], matPose.m[2][1], matPose.m[2][2], matPose.m[2][3],
		0.0f, 0.0f, 0.0f, 1.0f
	);
}


// make 2 render textures to use for left and right view.  I kinda go overboard with support stuff, like support shaders and mini-screen rectangles and nodes,
// because it's just what I normally do with manual textures.  It means it's easy to look at them and debug them, or render them onto other surfaces.
// You can probably strip some of this back if you don't care about that.
// You might also not clear them every frame, or just clear the depth, depending on what you're doing.
void OgreFramework::InitOgreTextures()
{
	RTT_Texture_VR_L = Ogre::TextureManager::getSingleton().createManual("RttTex_VR_L",
		ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME, TEX_TYPE_2D, m_nRenderWidth, m_nRenderHeight, 0, PF_R8G8B8A8, TU_RENDERTARGET);

	renderTexture_VR_L = RTT_Texture_VR_L->getBuffer()->getRenderTarget();
	renderTexture_VR_L->addViewport(m_pCameraL);
	renderTexture_VR_L->getViewport(0)->setClearEveryFrame(true, FBT_COLOUR | FBT_DEPTH);
	renderTexture_VR_L->getViewport(0)->setBackgroundColour(ColourValue::Black);
	renderTexture_VR_L->getViewport(0)->setOverlaysEnabled(false);
	renderTexture_VR_L->setAutoUpdated(false);

	RTT_Mat_VR_L = MaterialManager::getSingleton().create("RttMat_VR_L", ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME);
	RTT_Technique_VR_L = RTT_Mat_VR_L->createTechnique();
	RTT_Technique_VR_L->createPass();
	TextureUnitState* tState_VR_L = RTT_Mat_VR_L->getTechnique(0)->getPass(0)->createTextureUnitState("RttTex_VR_L");
	tState_VR_L->setTextureAddressingMode(Ogre::TextureUnitState::TAM_CLAMP);
	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setLightingEnabled(false);
	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setTextureFiltering(TFO_NONE);
	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setDepthCheckEnabled(false);
	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setDepthWriteEnabled(false);

	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setVertexProgram("TexProgDefault_vs", true);
	RTT_Mat_VR_L->getTechnique(0)->getPass(0)->setFragmentProgram("TexProgDefault_ps", true);
	RTT_Mat_VR_L->load();

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	RTT_Texture_VR_R = Ogre::TextureManager::getSingleton().createManual("RttTex_VR_R",
		ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME, TEX_TYPE_2D, m_nRenderWidth, m_nRenderHeight, 0, PF_R8G8B8A8, TU_RENDERTARGET);


	renderTexture_VR_R = RTT_Texture_VR_R->getBuffer()->getRenderTarget();
	renderTexture_VR_R->addViewport(m_pCameraR);
	renderTexture_VR_R->getViewport(0)->setClearEveryFrame(true, FBT_COLOUR | FBT_DEPTH);
	renderTexture_VR_R->getViewport(0)->setBackgroundColour(ColourValue::Black);
	renderTexture_VR_R->getViewport(0)->setOverlaysEnabled(false);
	renderTexture_VR_R->setAutoUpdated(false);

	RTT_Mat_VR_R = MaterialManager::getSingleton().create("RttMat_VR_R", ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME);
	RTT_Technique_VR_R = RTT_Mat_VR_R->createTechnique();
	RTT_Technique_VR_R->createPass();
	TextureUnitState* tState_VR_R = RTT_Mat_VR_R->getTechnique(0)->getPass(0)->createTextureUnitState("RttTex_VR_R");
	tState_VR_R->setTextureAddressingMode(Ogre::TextureUnitState::TAM_CLAMP);
	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setLightingEnabled(false);
	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setTextureFiltering(TFO_NONE);
	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setDepthCheckEnabled(false);
	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setDepthWriteEnabled(false);

	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setVertexProgram("TexProgDefault_vs", true);
	RTT_Mat_VR_R->getTechnique(0)->getPass(0)->setFragmentProgram("TexProgDefault_ps", true);
	RTT_Mat_VR_R->load();

	//char chMessage[1024];
	//sprintf(chMessage, "Created VR Textures: size %i %i", m_nRenderWidth, m_nRenderHeight);
	m_pLog->stream() << "Created VR Textures: size " << m_nRenderWidth << " " << m_nRenderHeight;

	// adjust the dimensions of the renderwindow viewport so we see a copy of the vr texture that isn't distorted.
	/*float flScaleX=(float)m_nRenderWidth/(float)m_pRenderWnd->getWidth() ;
	float flScaleY=(float)m_nRenderHeight/(float)m_pRenderWnd->getHeight() ;

	if(flScaleX>flScaleY)
	{
		float flScale=1.0f/flScaleX ;
		flScaleX*=flScale ;
		flScaleY*=flScale ;
	}
	else
	{
		float flScale=1.0f/flScaleY ;
		flScaleX*=flScale ;
		flScaleY*=flScale ;
	}

	m_pViewport->setDimensions(0.5f-flScaleX/2.0f, 0.5f-flScaleY/2.0f, flScaleX, flScaleY) ;*/
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////

// ----------------------------------------------------------------
	// gets the current position of all tracked devices
	// ----------------------------------------------------------------
void OgreFramework::updateHMDPos()
{
	if (!m_pHMD)
		return;

	vr::EVRCompositorError error = vr::VRCompositor()->WaitGetPoses(g_rTrackedDevicePose, vr::k_unMaxTrackedDeviceCount, NULL, 0);
	if (error != vr::VRCompositorError_None)
	{
		m_pLog->stream() << "WaitGetPoses error: " << CompositeErrorToString(error);
	}

	g_iValidPoseCount = 0;
	g_strPoseClasses = "";
	for (int nDevice = 0; nDevice < vr::k_unMaxTrackedDeviceCount; ++nDevice)
	{
		if (g_rTrackedDevicePose[nDevice].bPoseIsValid)
		{
			g_iValidPoseCount++;

			g_rmat4DevicePose[nDevice] = convertSteamVRMatrixToOgreMatrix4(g_rTrackedDevicePose[nDevice].mDeviceToAbsoluteTracking);

			if (g_rDevClassChar[nDevice] == 0)
			{
				switch (m_pHMD->GetTrackedDeviceClass(nDevice))
				{
				case vr::TrackedDeviceClass_Controller:        g_rDevClassChar[nDevice] = 'C'; break;
				case vr::TrackedDeviceClass_HMD:               g_rDevClassChar[nDevice] = 'H'; break;
				case vr::TrackedDeviceClass_Invalid:           g_rDevClassChar[nDevice] = 'I'; break;
				case vr::TrackedDeviceClass_GenericTracker:    g_rDevClassChar[nDevice] = 'G'; break;
				case vr::TrackedDeviceClass_TrackingReference: g_rDevClassChar[nDevice] = 'T'; break;
				default:                                       g_rDevClassChar[nDevice] = '?'; break;
				}
			}
			g_strPoseClasses += g_rDevClassChar[nDevice];
		}
	}

	if (g_rTrackedDevicePose[vr::k_unTrackedDeviceIndex_Hmd].bPoseIsValid)
	{
		g_mat4HMDPose = g_rmat4DevicePose[vr::k_unTrackedDeviceIndex_Hmd];
	}

	for (int nLoop = 0; nLoop < MAXCONTROLLER; nLoop++)
		if ((m_nControllerTDI[nLoop] != NOCONTROLLER) && (g_rTrackedDevicePose[m_nControllerTDI[nLoop]].bPoseIsValid))
			g_mat4ControllerPose[nLoop] = g_rmat4DevicePose[m_nControllerTDI[nLoop]];



}

bool OgreFramework::handleInput()
{
	bool bRet = false;

	// Process SteamVR events
	vr::VREvent_t event;
	while (m_pHMD->PollNextEvent(&event, sizeof(event)))
	{

		switch (event.eventType)
		{
		case vr::VREvent_TrackedDeviceActivated:
		{
			uint32_t unTrackedDevice = event.trackedDeviceIndex;
			// is it one of the controllers?
			if (m_pHMD->GetTrackedDeviceClass(unTrackedDevice) == vr::ETrackedDeviceClass::TrackedDeviceClass_Controller)
			{
				int nHand = -1;
				if ((m_nControllerTDI[HAND0] == NOCONTROLLER) && (m_nControllerTDI[HAND1] != (int)unTrackedDevice))
					nHand = HAND0;
				else
					if ((m_nControllerTDI[HAND1] == NOCONTROLLER) && (m_nControllerTDI[HAND0] != (int)unTrackedDevice))
						nHand = HAND1;

				if (nHand > -1)
				{
					sprintf(m_chMessage, "Connecting TrackedDevice %i as Hand%i", (int)unTrackedDevice, nHand);
					m_pLog->logMessage(m_chMessage);

					m_nControllerTDI[nHand] = unTrackedDevice;
					//CreateControllerModel(nHand) ;
				}
			}
		}
		break;

		case vr::VREvent_TrackedDeviceDeactivated:
			// is it one of the controllers?
			sprintf(m_chMessage, "Frame %i TrackedDeviceDeactivated, Index %i", g_frameIndex, (int)event.trackedDeviceIndex);
			m_pLog->logMessage(m_chMessage);
			if (event.trackedDeviceIndex == m_nControllerTDI[HAND1])
			{
				m_nControllerTDI[HAND1] = NOCONTROLLER;
				m_pLog->logMessage("Hand1 controller removed.");
			}
			else
				if (event.trackedDeviceIndex == m_nControllerTDI[HAND0])
				{
					m_nControllerTDI[HAND0] = NOCONTROLLER;
					m_pLog->logMessage("Hand0 controller removed.");
				}
			break;

		case vr::VREvent_TrackedDeviceUpdated:
			//dprintf("Device %u updated.\n", event.trackedDeviceIndex);
			sprintf(m_chMessage, "VREvent_TrackedDeviceUpdated: Device Index %i, Frame %i", (int)event.trackedDeviceIndex);
			m_pLog->logMessage(m_chMessage);
			break;

		}

	}

	// Process SteamVR controller state
	UpdateControllerData();

	return bRet;
}

void OgreFramework::UpdateControllerData()
{
	for (int nLoop = 0; nLoop < MAXCONTROLLER; nLoop++)
		if (m_nControllerTDI[nLoop] != NOCONTROLLER)
		{


			m_ControllerData[nLoop].Position = g_mat4ControllerPose[nLoop].getTrans() - g_poseNeutralPosition;
			m_ControllerData[nLoop].Orientation = g_poseNeutralOrientation.Inverse() * g_mat4ControllerPose[nLoop].extractQuaternion();

			// update controller buttons and touchpads
			vr::VRControllerState_t State;
			m_pHMD->GetControllerState(m_nControllerTDI[nLoop], &State, sizeof(State));

			if (State.unPacketNum == m_ControllerData[nLoop].unPacketNum)
				m_ControllerData[nLoop].Updated = false;
			else
				m_ControllerData[nLoop].Updated = true;
			m_ControllerData[nLoop].unPacketNum = State.unPacketNum;


			m_ControllerData[nLoop].AppMenu = (State.ulButtonPressed & vr::ButtonMaskFromId(vr::k_EButton_ApplicationMenu)) ? 1 : 0;
			m_ControllerData[nLoop].Grip = (State.ulButtonPressed & vr::ButtonMaskFromId(vr::k_EButton_Grip)) ? 1 : 0;
			m_ControllerData[nLoop].PadPress = (State.ulButtonPressed & vr::ButtonMaskFromId(vr::k_EButton_Axis0)) ? 1 : 0;
			m_ControllerData[nLoop].Trigger = (State.ulButtonPressed & vr::ButtonMaskFromId(vr::k_EButton_Axis1)) ? 1 : 0;

			m_ControllerData[nLoop].PadTouch = (State.ulButtonTouched & vr::ButtonMaskFromId(vr::k_EButton_Axis0)) ? 1 : 0;
			m_ControllerData[nLoop].Pad.x = State.rAxis[0].x;
			m_ControllerData[nLoop].Pad.y = State.rAxis[0].y;
		}
}

// ----------------------------------------------------------------
//  renders the frame
// ----------------------------------------------------------------
void OgreFramework::UpdateVR()
{
	//The example has this after submit, the docs show it before submit, both are here but only uncomment 1
	updateHMDPos();

	// update the parent camera node's orientation and position with the new tracking data
	g_orientation = g_poseNeutralOrientation.Inverse() * g_mat4HMDPose.extractQuaternion();
	m_pCameraNode->setOrientation(g_orientation);

	g_position = g_mat4HMDPose.getTrans();
	m_pCameraNode->setPosition(g_position - g_poseNeutralPosition + PlayerPos);

	renderTexture_VR_R->update(true);
	m_nBatchCount += renderTexture_VR_R->getStatistics().batchCount;
	m_nTriCount += renderTexture_VR_R->getStatistics().triangleCount;

	renderTexture_VR_L->update(true);
	m_nBatchCount += renderTexture_VR_L->getStatistics().batchCount;
	m_nTriCount += renderTexture_VR_L->getStatistics().triangleCount;

	// UV coords for each eye's texture are as expected for DirectX
	//                                       u1    v1    u2    v2
	const vr::VRTextureBounds_t boundsL = { 0.0f, 0.0f, 1.0f, 1.0f };
	const vr::VRTextureBounds_t boundsR = { 0.0f, 0.0f, 1.0f, 1.0f };

	vr::Texture_t stereoTextureL = { (void*)((Ogre::D3D11Texture*)RTT_Texture_VR_L.get())->GetTex2D() , vr::TextureType_DirectX, vr::ColorSpace_Gamma };
	vr::Texture_t stereoTextureR = { (void*)((Ogre::D3D11Texture*)RTT_Texture_VR_R.get())->GetTex2D() , vr::TextureType_DirectX, vr::ColorSpace_Gamma };

	vr::EVRCompositorError error = vr::VRCompositor()->Submit(vr::Eye_Left, &stereoTextureL, &boundsL);
	if (error != vr::VRCompositorError_None)
	{
		if (error != m_previousError) 
		{
			m_pLog->stream() << "Left Eye Submit error: " << CompositeErrorToString(error);
		}
		m_previousError = error;
	}

	error = vr::VRCompositor()->Submit(vr::Eye_Right, &stereoTextureR, &boundsR);
	if (error != vr::VRCompositorError_None)
	{
		if (error != m_previousError)
		{
			m_pLog->stream() << "Right Eye Submit error: " << CompositeErrorToString(error);
		}
		m_previousError = error;
	}

	//The example has this after submit, the docs show it before submit, both are here but only uncomment 1
	//updateHMDPos();
}

std::string OgreFramework::CompositeErrorToString(vr::EVRCompositorError error)
{
	switch (error)
	{
	case vr::VRCompositorError_None:
		return "None";
	case vr::VRCompositorError_RequestFailed:
		return "RequestFailed";
	case vr::VRCompositorError_IncompatibleVersion:
		return "IncompatibleVersion";
	case vr::VRCompositorError_DoNotHaveFocus:
		return "DoNotHaveFocus";
	case vr::VRCompositorError_InvalidTexture:
		return "InvalidTexture";
	case vr::VRCompositorError_IsNotSceneApplication:
		return "IsNotSceneApplication";
	case vr::VRCompositorError_TextureIsOnWrongDevice:
		return "TextureIsOnWrongDevice";
	case vr::VRCompositorError_TextureUsesUnsupportedFormat:
		return "TextureUsesUnsupportedFormat";
	case vr::VRCompositorError_SharedTexturesNotSupported:
		return "SharedTexturesNotSupported";
	case vr::VRCompositorError_IndexOutOfRange:
		return "IndexOutOfRange";
	case vr::VRCompositorError_AlreadySubmitted:
		return "AlreadySubmitted";
	case vr::VRCompositorError_InvalidBounds:
		return "InvalidBounds";
	default:
		std::stringstream ss;
		ss << "Unknown(" << error << ")";
		return ss.str();
	}
}