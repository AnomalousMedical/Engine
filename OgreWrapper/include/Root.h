#pragma once

#include "AutoPtr.h"
#include "RenderSceneCollection.h"
#include "RenderTargetCollection.h"
#include "RenderSystemCollection.h"

namespace Ogre
{
	class Root;
}

namespace Rendering{

[Engine::Attributes::MultiEnum]
public enum class SceneType : unsigned short
{
	ST_GENERIC = 1,
	ST_EXTERIOR_CLOSE = 2,
	ST_EXTERIOR_FAR = 4,
	ST_EXTERIOR_REAL_FAR = 8,
	ST_INTERIOR = 16
};

ref class RenderSystem;
ref class RenderWindow;
ref class SceneManager;
ref class RenderTarget;

typedef System::Collections::Generic::List<RenderSystem^> RenderSystemList;
typedef System::Collections::Generic::List<RenderScene^> SceneManagerList;
typedef System::Collections::Generic::Dictionary<System::String^, System::String^> ParamList;

/// <summary>
/// 
/// </summary>
public ref class Root
{
private:
	AutoPtr<Ogre::Root> ogreRoot;
	static Root^ instance;
	RenderSystemCollection renderSystems;
	RenderSceneCollection scenes;
	RenderTargetCollection renderTargets;

internal:
	/// <summary>
	/// Returns the native Root
	/// </summary>
	Ogre::Root* getRoot();

public:
	/// <summary>
	/// Get the singleton for this root. Note that you must call the constructor
    /// once before calling this function.
	/// </summary>
	/// <returns></returns>
	static Root^ getSingleton()
	{
		return instance;
	}

	/// <summary>
	/// Constructor. Only call one time per program execution.
	/// </summary>
	Root();

	/// <summary>
	/// Constructor. Only call one time per program execution.
	/// </summary>
	/// <param name="pluginFileName">The file that contains plugins information. Defaults to "plugins.cfg", may be left blank to ignore. </param>
	Root(System::String^ pluginFileName);

	/// <summary>
	/// Constructor. Only call one time per program execution.
	/// </summary>
	/// <param name="pluginFileName">The file that contains plugins information. Defaults to "plugins.cfg", may be left blank to ignore. </param>
	/// <param name="configFileName">The file that contains the configuration to be loaded. Defaults to "ogre.cfg", may be left blank to load nothing. </param>
	Root(System::String^ pluginFileName, System::String^ configFileName);

	/// <summary>
	/// Constructor. Only call one time per program execution.
	/// </summary>
	/// <param name="pluginFileName">The file that contains plugins information. Defaults to "plugins.cfg", may be left blank to ignore. </param>
	/// <param name="configFileName">The file that contains the configuration to be loaded. Defaults to "ogre.cfg", may be left blank to load nothing. </param>
	/// <param name="logFileName">The logfile to create, defaults to Ogre.log, may be left blank if you've already set up LogManager and Log yourself.</param>
	Root(System::String^ pluginFileName, System::String^ configFileName, System::String^ logFileName);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~Root();

	void saveConfig();

	bool restoreConfig();

	bool showConfigDialog();

	void addRenderSystem(RenderSystem^ newRend);

	RenderSystemList^ getAvailableRenderers();

	RenderSystem^ getRenderSystemByName(System::String^ name);

	void setRenderSystem(RenderSystem^ system);

	RenderSystem^ getRenderSystem();

	RenderWindow^ initialize(bool autoCreateWindow);

	RenderWindow^ initialize(bool autoCreateWindow, System::String^ windowTitle);

	RenderWindow^ initialize(bool autoCreateWindow, System::String^ windowTitle, System::String^ customCapabilitiesConfig);

	bool isInitialized();

	RenderScene^ createSceneManager(System::String^ typeName);

	RenderScene^ createSceneManager(System::String^ typeName, System::String^ instanceName);

	RenderScene^ createSceneManager(SceneType typeMask);

	RenderScene^ createSceneManager(SceneType typeMask, System::String^ instanceName);

	void destroySceneManager(RenderScene^ sceneManager);

	RenderScene^ getSceneManager(System::String^ instanceName);

	SceneManagerList^ getSceneManagerIterator();

	System::String^ getErrorDescription(long errorNumber);

	void queueEndRendering();

	void startRendering();

	bool renderOneFrame();

	void shutdown();

	RenderWindow^ getAutoCreatedWindow();

	RenderWindow^ createRenderWindow(System::String^ name, unsigned int width, unsigned int height, bool fullScreen);

	RenderWindow^ createRenderWindow(System::String^ name, unsigned int width, unsigned int height, bool fullScreen, ParamList^ paramList);

	void detachRenderTarget(RenderTarget^ pWin);

	void detachRenderTarget(System::String^ name);

	RenderTarget^ getRenderTarget(System::String^ name);

	void loadPlugin(System::String^ pluginName);

	void unloadPlugin(System::String^ pluginName);

	bool _fireFrameStarted(float timeSinceLastEvent, float timeSinceLastFrame);

	bool _fireFrameRenderingQueued(float timeSinceLastEvent, float timeSinceLastFrame);

	bool _fireFrameEnded(float timeSinceLastEvent, float timeSinceLastFrame);

	bool _fireFrameStarted();

	bool _fireFrameRenderingQueued();

	bool _fireFrameEnded();

	unsigned long getNextFrameNumber();

	void clearEventTimes();

	void setFrameSmoothingPeriod(float period);

	float getFrameSmoothingPeriod();
};

}