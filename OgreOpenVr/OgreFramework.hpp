// This is just a basic framework to show OpenVR working in Ogre 1.10, DX11.
// Written by mkultra333, based on work by Kojack, cherrychalk and OpenVR sample code.
// See license.txt for license info, basically it's MIT/BSD.

//|||||||||||||||||||||||||||||||||||||||||||||||
#include "stdafx.h"

#ifndef OGRE_FRAMEWORK_HPP
#define OGRE_FRAMEWORK_HPP

//|||||||||||||||||||||||||||||||||||||||||||||||


// based on code from OpenVR hellovr_opengl sample
struct FramebufferDesc
{
	unsigned int m_nDepthBufferId;
	unsigned int m_nRenderTextureId;
	unsigned int m_nRenderFramebufferId;
	unsigned int m_nResolveTextureId;
	unsigned int m_nResolveFramebufferId;
};
	
#define FARCLIP		1000.0
#define NEARCLIP	0.01

#define MAXCONTROLLER	2 // allow for up to two VR controllers
#define NOCONTROLLER	-1 
#define HAND0			0
#define HAND1			1

#define CONTROLLERTEXTURESIZE	512

struct CONTROLLERDATA
{
	Ogre::Vector3 Position ;
	Ogre::Quaternion Orientation ;

	int AppMenu ;
	int Grip ;
	int PadPress ;
	int PadTouch ;
	int Trigger ;

	Ogre::Vector2 Pad ;

	bool Updated ;
	uint32_t unPacketNum;
};

#define WORKSIZEX	1280
#define WORKSIZEY	720


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

class OgreFramework : public Ogre::Singleton<OgreFramework>//, OIS::KeyListener, OIS::MouseListener//, OgreBites::SdkTrayListener
{
public:
        OgreFramework();
        ~OgreFramework();

		void Safe_ZeroMemory(void* pMemStart, int nSize) ;



		char m_chBug[10240] ;

        bool initOgre(Ogre::Root* root);
        void updateOgre(double timeSinceLastFrame);

        bool isOgreToBeShutDown()const{return m_bShutDownOgre;}  

        Ogre::SceneManager*                     m_pSceneManager;
        Ogre::Log*                                      m_pLog;
		Ogre::RenderSystem*						m_pRenderSystem;

		Ogre::Vector3	PlayerPos ;
		float			PlayerSpeed ;
		bool			PlayerSpeedPress ;



		void CreateGenericCubeMesh(char chName[], char chMaterial[], Ogre::ManualObject* pMO, float flSizeX, float flSizeY, float flSizeZ, float flOffsetX, float flOffsetY, float flOffsetZ) ;
		Ogre::ManualObject* m_pWorldMO ;
		Ogre::SceneNode*	m_pWorldSN ;

	Ogre::TexturePtr			RTT_Texture_VR_L ;
	Ogre::RenderTexture*		renderTexture_VR_L ;
	Ogre::MaterialPtr			RTT_Mat_VR_L ;
	Ogre::Technique*			RTT_Technique_VR_L ;

	Ogre::TexturePtr			RTT_Texture_VR_R ;
	Ogre::RenderTexture*		renderTexture_VR_R ;
	Ogre::MaterialPtr			RTT_Mat_VR_R ;
	Ogre::Technique*			RTT_Technique_VR_R ;

	int m_nBatchCount ;
	int m_nTriCount ;

	bool initOpenVR();
	std::string GetTrackedDeviceString( vr::IVRSystem *pHmd, vr::TrackedDeviceIndex_t unDevice, vr::TrackedDeviceProperty prop, vr::TrackedPropertyError *peError = NULL ) ;
	void InitOgreCameras() ;
	Ogre::Matrix4 getHMDMatrixProjectionEye(vr::Hmd_Eye nEye) ;
	Ogre::Matrix4 getHMDMatrixPoseEye(vr::Hmd_Eye nEye) ;
	Ogre::Matrix4 convertSteamVRMatrixToOgreMatrix4(const vr::HmdMatrix34_t &matPose) ;
	void InitOgreTextures() ;

	void SetupRenderModelForTrackedDevice( vr::TrackedDeviceIndex_t unTrackedDeviceIndex ) ;
	void SetupRenderModels() ;
	void CreateControllerModel(int nController) ;
	void DeleteControllerModel(int nController) ;
	void DefaultControllerModel(int nController) ;

	int m_nControllerTDI[MAXCONTROLLER] ; // Tracked Device Index of the two hand controllers
	bool m_bControllerHasModel[MAXCONTROLLER] ; // whether we've created assets for this controller.
	Ogre::ManualObject* m_ControllerMO[MAXCONTROLLER] ;
	Ogre::SceneNode* m_ControllerSN[MAXCONTROLLER] ;
	CONTROLLERDATA m_ControllerData[MAXCONTROLLER] ;

	vr::TrackedDevicePose_t m_rTrackedDevicePose[ vr::k_unMaxTrackedDeviceCount ];

	void updateHMDPos() ;
	bool handleInput() ;
	void UpdateControllerData() ;
	void UpdateVR() ;

	uint32_t					m_nRenderWidth;
	uint32_t					m_nRenderHeight;
	vr::IVRSystem*				m_pHMD;
	std::string					m_strDriver;
	std::string					m_strDisplay;

	Ogre::Camera*				m_pCameraL ;
	Ogre::Camera*				m_pCameraR ;
	Ogre::SceneNode*			m_pCameraNode ;

	Ogre::Quaternion			g_orientation;
	Ogre::Vector3				g_position;
	Ogre::Vector3				g_poseNeutralPosition ;
	Ogre::Quaternion			g_poseNeutralOrientation ;

	float						g_IPD ;


	// openvr
	
	vr::TrackedDevicePose_t		g_rTrackedDevicePose[vr::k_unMaxTrackedDeviceCount];
	Ogre::Matrix4				g_rmat4DevicePose[vr::k_unMaxTrackedDeviceCount];
	Ogre::Matrix4				g_mat4HMDPose;
	Ogre::Matrix4				g_mat4ControllerPose[MAXCONTROLLER];


	bool						g_rbShowTrackedDevice[vr::k_unMaxTrackedDeviceCount];	
	int							g_iValidPoseCount;
	int							g_iValidPoseCount_Last;
	std::string					g_strPoseClasses;			// what classes we saw poses for this frame
	char						g_rDevClassChar[vr::k_unMaxTrackedDeviceCount];	// for each device, a character representing its class

	FramebufferDesc leftEyeDesc;
	FramebufferDesc rightEyeDesc;

	int					g_frameIndex ;

	char m_chMessage[1024] ;


private:
        OgreFramework(const OgreFramework&);
        OgreFramework& operator= (const OgreFramework&);

        int                                                     m_iNumScreenShots;

        bool                                            m_bShutDownOgre;
        
        Ogre::Vector3                           m_TranslateVector;
        Ogre::Real                                      m_MoveSpeed; 
        Ogre::Degree                            m_RotateSpeed; 
        float                                           m_MoveScale; 
        Ogre::Degree                            m_RotScale;	

};

//|||||||||||||||||||||||||||||||||||||||||||||||

#endif 

//|||||||||||||||||||||||||||||||||||||||||||||||