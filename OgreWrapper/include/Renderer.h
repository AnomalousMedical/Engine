///// <file>Renderer.h</file>
///// <author>Andrew Piper</author>
///// <company>Joint Based Engineering</company>
///// <copyright>
///// Copyright (c) Joint Based Engineering 2008, All rights reserved
///// </copyright>
//
//#pragma once
//
//#include "RenderSceneCollection.h"
//#include "RenderTargetCollection.h"
//
//namespace Ogre
//{
//	class Root;
//}
//
//namespace Engine
//{
//
//namespace Rendering
//{
//
//typedef System::Collections::Generic::Dictionary<System::String^, System::String^> ParamList;
//
//ref class RenderScene;
//ref class RenderTarget;
//ref class RenderWindow;
//
//class OgreLogListener;
//class NRendererUpdate;
//
///// <summary>
///// The main class for the Rendering subsystem.  This provides a way to create
///// and destroy the various things that can be put into the renderer.  By centralizing
///// into this class memory management of the native types is much easier.
///// </summary>
//[Engine::Attributes::DoNotSaveAttribute]
//public ref class Renderer
//{
//private:
//	Ogre::Root* root;
//	RenderSceneCollection scenes;
//	RenderTargetCollection renderTargets;
//	NRendererUpdate* rendererUpdate;
//	OgreLogListener* ogreLogListener;
//	
//	/// <summary>
//	/// Copy the contents of the dictionary param to the pair list.
//	/// </summary>
//	void buildParamList(ParamList^ params, Ogre::NameValuePairList& pairList);
//
//public:
//	/// <summary>
//	/// Constructor
//	/// </summary>
//	Renderer(void);
//
//	/// <summary>
//	/// Destructor
//	/// </summary>
//	~Renderer();
//
//	/// <summary>
//	/// Sets up the properties of the renderer and loads any other libraries it is dependent
//	/// on.  This should be called before any other functions.
//	/// </summary>
//	/// <returns>True if everything initialized ok, false if there was an error </returns>
//	bool initialize();
//
//	/// <summary>
//	/// Creates a window for the renderer.  At least one of these should be created
//	/// after initialize and before any other functions are called.  It is possible to 
//	/// create more than one render window for multiple views.
//	/// </summary>
//	/// <remarks>
//	/// See embed window for information on the misc parameters.
//	/// </remarks>
//	/// <param name="width">The width of the window.</param>
//	/// <param name="height">The height of the window.</param>
//	/// <param name="fullscreen">True if the window should be fullscreen.  False for windowed mode.</param>
//	/// <param name="windowName">The name of the window.</param>
//	/// <param name="miscParams">A set of other parameters in a dictionary.  This can be null.</param>
//	/// <returns>A new RenderWindow with the specified properties.</returns>
//	RenderWindow^ createWindow( int width, int height, bool fullscreen, System::String^ windowName, ParamList^ miscParams );
//
//	/// <summary>
//	/// <para>Embeds the renderer in another window.  At least one of these should be created
//	/// after initialize and before any other functions are called.  It is possible to 
//	/// create more than one render window for multiple views.  Embedded windows cannot be
//	/// fullscreen.</para>
//	/// <para>Misc Param values:</para>
//	/// <list type="table">
//	/// <listheader>
//	///     <term>Key</term>
//	///     <description>Value</description>
//	/// </listheader>
//    /// <item>
//    ///    <term>title</term>
//    ///    <description>The title of the window that will appear in the title bar Values: 
//	///                 string Default: RenderTarget name</description>
//	/// </item>
//    /// <item>
//    ///    <term>colourDepth</term>
//    ///    <description>Colour depth of the resulting rendering window; only applies if 
//	///                 fullScreen is set. Values: 16 or 32 Default: desktop depth Notes: 
//	///                 [W32 specific]</description>
//	/// </item>
//    /// <item>
//    ///    <term>left</term>
//    ///    <description>screen x coordinate from left Values: positive integers Default: 
//	///                 'center window on screen' Notes: Ignored in case of full screen</description>
//	/// </item>
//    /// <item>
//    ///    <term>top</term>
//    ///    <description>screen y coordinate from top Values: positive integers Default: 
//	///                 'center window on screen' Notes: Ignored in case of full screen</description>
//	/// </item>
//    /// <item>
//    ///    <term>depthBuffer" [DX9 specific]</term>
//    ///    <description>Use depth buffer Values: false or true Default: true</description>
//	/// </item>
//    /// <item>
//    ///    <term>externalGLControl" [Win32 OpenGL specific]</term>
//    ///    <description>Let the external window control OpenGL i.e. don't select a pixel 
//	///                 format for the window, do not change v-sync and do not swap buffer. 
//	///                 When set to true, the calling application is responsible of OpenGL 
//	///                 initialization and buffer swapping. It should also create an OpenGL
//	///                 context for its own rendering, Ogre will create one for its use. 
//	///                 Then the calling application must also enable Ogre OpenGL context 
//	///                 before calling any Ogre function and restore its OpenGL context 
//	///                 after these calls. The Ogre OpenGL context can be retrieved after 
//	///                 Ogre initialisation by calling wglGetCurrentDC() and 
//	///                 wglGetCurrentContext(). It is only used when the externalWindowHandle
//	///                 parameter is used. Values: true, false Default: false</description>
//	/// </item>
//    /// <item>
//    ///    <term>externalGLContext" [Win32 OpenGL specific]</term>
//	///    <description>Use an externally created GL context Values: 
//	///                 {context as="" unsigned="" long=""} Default: 0 (create own context)</description>
//	/// </item>
//    /// <item>
//    ///    <term>parentWindowHandle" [API specific]</term>
//    ///    <description>Parent window handle, for embedding the OGRE context 
//	///                 Values: positive integer for W32 (HWND handle) poslong:posint:poslong 
//	///                 for GLX (display*:screen:windowHandle) Default: 0 (None)</description>
//	/// </item>
//    /// <item>
//    ///    <term>FSAA</term>
//    ///    <description>Full screen antialiasing factor Values: 0,2,4,6,... Default: 0</description>
//	/// </item>
//    /// <item>
//    ///    <term>displayFrequency</term>
//    ///    <description>Display frequency rate, for fullscreen mode Values: 60...? 
//	///                 Default: Desktop vsync rate</description>
//	/// </item>
//    /// <item>
//    ///    <term>vsync</term>
//    ///    <description>Synchronize buffer swaps to vsync Values: true, false Default: 0</description>
//	/// </item>
//    /// <item>
//    ///    <term>border</term>
//    ///    <description>The type of window border (in windowed mode) Values: none, 
//	///                 fixed, resize Default: resize</description>
//	/// </item>
//    /// <item>
//    ///    <term>outerDimensions</term>
//    ///    <description>Whether the width/height is expressed as the size of the outer 
//	///                 window, rather than the content area Values: true, false 
//	///                 Default: false</description>
//	/// </item>
//    /// <item>
//    ///    <term>useNVPerfHUD" [DX9 specific] </term>
//    ///    <description>Enable the use of nVidia NVPerfHUD Values: true, false Default: false</description>
//	/// </item>
//	/// </list>
//	/// </summary>
//	/// <param name="width">The width of the window.</param>
//	/// <param name="height">The height of the window.</param>
//	/// <param name="windowName">The name of the window.  This will not change the title text of the parent window.</param>
//	/// <param name="windowHandle">A pointer to the parent window.</param>
//	/// <param name="miscParams">A set of other parameters in a dictionary.  This can be null.</param>
//	/// <returns>A new RenderWindow with the specified properties.</returns>
//	RenderWindow^ embedWindow( int width, int height, System::String^ windowName, Windowing::OSWindow^ windowHandle, ParamList^ miscParams );
//
//	/// <summary>
//	/// Get the render target specified by name.
//	/// </summary>
//	/// <param name="name">The name of the render target.</param>
//	/// <returns>The render target specified by name or null if it does not exist.</returns>
//	RenderTarget^ getRenderTarget(System::String^ name);
//
//	/// <summary>
//	/// Destroy the given render target releasing its underlying native resources and
//	/// disposing the managed object.
//	/// </summary>
//	/// <param name="target">The render target to destroy.</param>
//	void destroyRenderTarget( RenderTarget^ target );
//
//	/// <summary>
//	/// Creates a new RenderScene in which renderer objects can be placed and viewed.
//	///  This will also set the scene as the currently active scene.
//	/// </summary>
//	/// <param name="name">The name of the scene.</param>
//	/// <returns>A new RenderScene named name.</returns>
//	RenderScene^ createScene( System::String^ name );
//
//	/// <summary>
//	/// Destroys the given RenderScene releasing its underlying native resources and
//	/// disposing the managed object.
//	/// </summary>
//	/// <param name="scene">The RenderScene to destroy.</param>
//	void destroyScene( RenderScene^ scene );
//
//	/// <summary>
//	/// Attach this renderer to the given timer to receive updates.
//	/// </summary>
//	/// <param name="timer">The timer to attach to.</param>
//	void attachToTimer(Timing::Timer^ timer);
//};
//
//}
//
//}