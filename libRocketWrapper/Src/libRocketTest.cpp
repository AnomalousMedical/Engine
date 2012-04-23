#include "StdAfx.h"
#include <Rocket/Core/String.h>
#include <Rocket/Core/SystemInterface.h>
#include <Ogre.h>

namespace Rocket {
namespace Core {

class Context;

}
}

class AnomalousSystemInterface;
class RenderInterfaceOgre3D;

/**
	@author Peter Curry
 */

class libRocketTest
{
	public:
		libRocketTest();
		virtual ~libRocketTest();

	public:
		void createScene(RenderInterfaceOgre3D* renderInterface, Rocket::Core::SystemInterface* systemInterface, Rocket::Core::Context* con);
		void destroyScene();

		void createFrameListener();

		/// Called from Ogre before a queue group is rendered.
		virtual void renderQueueStarted(Ogre::uint8 queueGroupId);

	private:
		// Configures Ogre's rendering system for rendering Rocket.
		void ConfigureRenderSystem();
		// Builds an OpenGL-style orthographic projection matrix.
		void BuildProjectionMatrix(Ogre::Matrix4& matrix);

		// Absolute path to the libRocket directory.
		Rocket::Core::String rocket_path;
		// Absolute path to the Ogre3d sample directory;
		Rocket::Core::String sample_path;

		Rocket::Core::Context* context;
};


//cpp
#include <Rocket/Core.h>
#include <Rocket/Controls.h>
#include <Rocket/Debugger.h>
#include "RenderInterfaceOgre3D.h"
#include <direct.h>



libRocketTest::libRocketTest()
{
	context = NULL;

	// Switch the working directory to Ogre's bin directory.
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
	rocket_path = "S:/Junk/librocket/";
	sample_path = "S:/Junk/librocket/libRocket/Samples/";
#if OGRE_DEBUG_MODE
	chdir((Ogre::String(getenv("OGRE_HOME")) + "\\bin\\debug\\").c_str());
#else
	//chdir((Ogre::String(getenv("OGRE_HOME")) + "\\bin\\release\\").c_str());
#endif
#endif
}

libRocketTest::~libRocketTest()
{
	destroyScene();
}

void libRocketTest::createScene(RenderInterfaceOgre3D* renderInterface, Rocket::Core::SystemInterface* systemInterface, Rocket::Core::Context* con)
{
	//Ogre::ResourceGroupManager::getSingleton().createResourceGroup("Rocket");
	//Ogre::ResourceGroupManager::getSingleton().addResourceLocation(rocket_path.Replace("\\", "/").CString(), "FileSystem", "Rocket");

	// Rocket initialisation.
	//Rocket::Core::SetRenderInterface(renderInterface);

	//Rocket::Core::SetSystemInterface(systemInterface);

	//Rocket::Core::Initialise();
	//Rocket::Controls::Initialise();

	// Load the fonts from the path to the sample directory.
	/*Rocket::Core::FontDatabase::LoadFontFace(sample_path + "assets/Delicious-Roman.otf");
	Rocket::Core::FontDatabase::LoadFontFace(sample_path + "assets/Delicious-Bold.otf");
	Rocket::Core::FontDatabase::LoadFontFace(sample_path + "assets/Delicious-Italic.otf");
	Rocket::Core::FontDatabase::LoadFontFace(sample_path + "assets/Delicious-BoldItalic.otf");*/

	/*context = Rocket::Core::CreateContext("main", Rocket::Core::Vector2i(renderInterface->getScissorRight(), renderInterface->getScissorBottom()));*/
	context = con;
	//Rocket::Debugger::Initialise(context);

	// Load the mouse cursor and release the caller's reference.
	Rocket::Core::ElementDocument* cursor = context->LoadMouseCursor(sample_path + "assets/cursor.rml");
	if (cursor)
		cursor->RemoveReference();

	Rocket::Core::ElementDocument* document = context->LoadDocument(sample_path + "assets/demo.rml");
	if (document)
	{
		document->Show();
		document->RemoveReference();
	}

	// Add the application as a listener to Ogre's render queue so we can render during the overlay.
	//Ogre::Root::getSingletonPtr()->getSceneManagerIterator().begin()->second->addRenderQueueListener(this);
	//mSceneMgr->addRenderQueueListener(this);
}

void libRocketTest::destroyScene()
{
	// Shutdown Rocket.
	//context->RemoveReference();
	//Rocket::Core::Shutdown();
}

void libRocketTest::createFrameListener()
{
	
}

// Called from Ogre before a queue group is rendered.
void libRocketTest::renderQueueStarted(Ogre::uint8 queueGroupId)
{
	if (queueGroupId == Ogre::RENDER_QUEUE_OVERLAY)// && Ogre::Root::getSingleton().getRenderSystem()->_getViewport()->getOverlaysEnabled())
	{
		context->Update();

		ConfigureRenderSystem();
		context->Render();
	}
}

// Configures Ogre's rendering system for rendering Rocket.
void libRocketTest::ConfigureRenderSystem()
{
	Ogre::RenderSystem* render_system = Ogre::Root::getSingleton().getRenderSystem();

	// Set up the projection and view matrices.
	Ogre::Matrix4 projection_matrix;
	BuildProjectionMatrix(projection_matrix);
	render_system->_setProjectionMatrix(projection_matrix);
	render_system->_setViewMatrix(Ogre::Matrix4::IDENTITY);

	// Disable lighting, as all of Rocket's geometry is unlit.
	render_system->setLightingEnabled(false);
	// Disable depth-buffering; all of the geometry is already depth-sorted.
	render_system->_setDepthBufferParams(false, false);
	// Rocket generates anti-clockwise geometry, so enable clockwise-culling.
	render_system->_setCullingMode(Ogre::CULL_CLOCKWISE);
	// Disable fogging.
	render_system->_setFog(Ogre::FOG_NONE);
	// Enable writing to all four channels.
	render_system->_setColourBufferWriteEnabled(true, true, true, true);
	// Unbind any vertex or fragment programs bound previously by the application.
	render_system->unbindGpuProgram(Ogre::GPT_FRAGMENT_PROGRAM);
	render_system->unbindGpuProgram(Ogre::GPT_VERTEX_PROGRAM);

	// Set texture settings to clamp along both axes.
	Ogre::TextureUnitState::UVWAddressingMode addressing_mode;
	addressing_mode.u = Ogre::TextureUnitState::TAM_CLAMP;
	addressing_mode.v = Ogre::TextureUnitState::TAM_CLAMP;
	addressing_mode.w = Ogre::TextureUnitState::TAM_CLAMP;
	render_system->_setTextureAddressingMode(0, addressing_mode);

	// Set the texture coordinates for unit 0 to be read from unit 0.
	render_system->_setTextureCoordSet(0, 0);
	// Disable texture coordinate calculation.
	render_system->_setTextureCoordCalculation(0, Ogre::TEXCALC_NONE);
	// Enable linear filtering; images should be rendering 1 texel == 1 pixel, so point filtering could be used
	// except in the case of scaling tiled decorators.
	render_system->_setTextureUnitFiltering(0, Ogre::FO_LINEAR, Ogre::FO_LINEAR, Ogre::FO_POINT);
	// Disable texture coordinate transforms.
	render_system->_setTextureMatrix(0, Ogre::Matrix4::IDENTITY);
	// Reject pixels with an alpha of 0.
	render_system->_setAlphaRejectSettings(Ogre::CMPF_GREATER, 0, false);
	// Disable all texture units but the first.
	render_system->_disableTextureUnitsFrom(1);

	// Enable simple alpha blending.
	render_system->_setSceneBlending(Ogre::SBF_SOURCE_ALPHA, Ogre::SBF_ONE_MINUS_SOURCE_ALPHA);

	// Disable depth bias.
	render_system->_setDepthBias(0, 0);
}

// Builds an OpenGL-style orthographic projection matrix.
void libRocketTest::BuildProjectionMatrix(Ogre::Matrix4& projection_matrix)
{
	float z_near = -1;
	float z_far = 1;

	projection_matrix = Ogre::Matrix4::ZERO;

	// Set up matrices.
	projection_matrix[0][0] = 2.0f / 1920;//width; (the window width)
	projection_matrix[0][3]= -1.0000000f;
	projection_matrix[1][1]= -2.0f / 1080;// height;(the window height)
	projection_matrix[1][3]= 1.0000000f;
	projection_matrix[2][2]= -2.0f / (z_far - z_near);
	projection_matrix[3][3]= 1.0000000f;
}

extern "C" _AnomalousExport libRocketTest* libRocketTest_Create(RenderInterfaceOgre3D* renderInterface, Rocket::Core::SystemInterface* systemInterface, Rocket::Core::Context* con)
{
	libRocketTest* rocketTest = new libRocketTest();
	rocketTest->createScene(renderInterface, systemInterface, con);
	return rocketTest;
}

extern "C" _AnomalousExport void libRocketTest_Delete(libRocketTest* test)
{
	delete test;
}

extern "C" _AnomalousExport void libRocketTest_Render(libRocketTest* test, Ogre::uint8 queue)
{
	test->renderQueueStarted(queue);
}