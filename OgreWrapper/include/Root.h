#pragma once

#include "AutoPtr.h"
#include "SceneManagerCollection.h"
#include "RenderTargetCollection.h"
#include "RenderSystemCollection.h"
#include "EmbeddedResourceArchiveFactory.h"
#include "OgreEngineArchiveFactory.h"
#include "vcclr.h"

namespace Ogre
{
	class Root;
}

namespace OgreWrapper{

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
typedef System::Collections::Generic::List<SceneManager^> SceneManagerList;
typedef System::Collections::Generic::Dictionary<System::String^, System::String^> ParamList;

public ref class FrameEvent
{
public:
	/** Elapsed time in seconds since the last event.
        This gives you time between frame start & frame end,
        and between frame end and next frame start.
        @remarks
            This may not be the elapsed time but the average
            elapsed time between recently fired events.
    */
    float timeSinceLastEvent;
    /** Elapsed time in seconds since the last event of the same type,
        i.e. time for a complete frame.
        @remarks
            This may not be the elapsed time but the average
            elapsed time between recently fired events of the same type.
    */
    float timeSinceLastFrame;
};

public delegate void FrameEventHandler(FrameEvent^ frameEvent);

class ManagedFrameListener;

/// <summary>
/// 
/// </summary>
public ref class Root
{
private:
	Ogre::Root* ogreRoot;
	EmbeddedResourceArchiveFactory* embeddedResourceArchiveFactory;
	OgreEngineArchiveFactory* engineArchive;
	static Root^ instance;
	RenderSystemCollection renderSystems;
	RenderSceneCollection scenes;
	RenderTargetCollection renderTargets;
	ManagedFrameListener* frameListener;

internal:
	/// <summary>
	/// Returns the native Root
	/// </summary>
	Ogre::Root* getRoot();

public:
	event FrameEventHandler^ FrameStarted;
	event FrameEventHandler^ FrameRenderingQueued;
	event FrameEventHandler^ FrameEnded;

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

	SceneManager^ createSceneManager(System::String^ typeName);

	SceneManager^ createSceneManager(System::String^ typeName, System::String^ instanceName);

	SceneManager^ createSceneManager(SceneType typeMask);

	SceneManager^ createSceneManager(SceneType typeMask, System::String^ instanceName);

	void destroySceneManager(SceneManager^ sceneManager);

	SceneManager^ getSceneManager(System::String^ instanceName);

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

	bool _updateAllRenderTargets();

internal:
	void fireFrameStartedEvent(FrameEvent^ frameEvent)
	{
		FrameStarted(frameEvent);
	}

	void fireFrameRenderingQueuedEvent(FrameEvent^ frameEvent)
	{
		FrameRenderingQueued(frameEvent);
	}

	void fireFrameEndedEvent(FrameEvent^ frameEvent)
	{
		FrameEnded(frameEvent);
	}
};

class ManagedFrameListener : public Ogre::FrameListener
{
private:
	gcroot<Root^> root;
	gcroot<FrameEvent^> frameEvent;

public:
	ManagedFrameListener(gcroot<Root^> root)
		:root(root),
		frameEvent(gcnew FrameEvent())
	{

	}

	virtual ~ManagedFrameListener() 
	{
		root = nullptr;
	}

	virtual bool frameStarted(const Ogre::FrameEvent& evt)
    {
		frameEvent->timeSinceLastEvent = evt.timeSinceLastEvent;
		frameEvent->timeSinceLastFrame = evt.timeSinceLastFrame;
		root->fireFrameStartedEvent(frameEvent);
		return true; 
	}
	
	virtual bool frameRenderingQueued(const Ogre::FrameEvent& evt)
    { 
		frameEvent->timeSinceLastEvent = evt.timeSinceLastEvent;
		frameEvent->timeSinceLastFrame = evt.timeSinceLastFrame;
		root->fireFrameRenderingQueuedEvent(frameEvent);
		return true; 
	}

    virtual bool frameEnded(const Ogre::FrameEvent& evt)
    { 
		frameEvent->timeSinceLastEvent = evt.timeSinceLastEvent;
		frameEvent->timeSinceLastFrame = evt.timeSinceLastFrame;
		root->fireFrameEndedEvent(frameEvent);
		return true; 
	}
};

}