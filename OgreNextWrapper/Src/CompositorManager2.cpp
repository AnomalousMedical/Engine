#include "Stdafx.h"
#include "Compositor/OgreCompositorManager2.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport void CompositorManager2_createBasicWorkspaceDef(Ogre::CompositorManager2 * compMan,
    String workspaceDefName,
    Color backgroundColour)
{
	compMan->createBasicWorkspaceDef(workspaceDefName, backgroundColour.toOgre());
}

extern "C" _AnomalousExport Ogre::CompositorWorkspace * CompositorManager2_addWorkspace(Ogre::CompositorManager2 * compMan,
    Ogre::SceneManager * sceneManager, Ogre::TextureGpu * finalRenderTarget,
    Ogre::Camera * defaultCam, String definitionName, bool bEnabled,
    int position = -1
    /*, const UavBufferPackedVec * uavBuffers = 0,
    ResourceLayoutMap * initialLayouts = 0,
    ResourceAccessMap * initialUavAccess = 0,
    Vector4 vpOffsetScale = Vector4::ZERO,
    uint8 vpModifierMask = 0x00, uint8 executionMask = 0xFF*/
)
{
    return compMan->addWorkspace(sceneManager, finalRenderTarget, defaultCam, definitionName, bEnabled, position);
}

#pragma warning(pop)