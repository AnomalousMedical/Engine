/// <file>RenderTarget.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "PixelBox.h"
#include "ViewportCollection.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Ogre
{
	class RenderTarget;
}

namespace OgreWrapper
{

ref class Viewport;
ref class Camera;
ref class PixelBox;

/// <summary>
/// This class is where the renderer draws.  It can create viewports that are
/// linked to individual cameras that are then rendered onto this target.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderTarget abstract
{
public:
enum class FrameBuffer
{
	FB_FRONT,
	FB_BACK,
	FB_AUTO
};

private:
	Ogre::RenderTarget* renderTarget;
	ViewportCollection viewports;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="renderTarget">The RenderTarget to wrap.</param>
	RenderTarget(Ogre::RenderTarget* renderTarget);

	/// <summary>
	/// Return the wrapped native RenderTarget.
	/// </summary>
	/// <returns>The native render target.</returns>
	Ogre::RenderTarget* getRenderTarget();

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~RenderTarget();

	/// <summary>
	/// Returns the name of this render target.
	/// </summary>
	/// <returns>The name of the render target.</returns>
	System::String^ getName();

	/// <summary>
	/// Add a viewport to the rendering target.
	/// </summary>
	/// <remarks>
	/// A viewport is the rectangle into which rendering output is sent. This
    /// method adds a viewport to the render target, rendering from the supplied
    /// camera. The rest of the parameters are only required if you wish to add
    /// more than one viewport to a single rendering target. Note that size
    /// information passed to this method is passed as a parametric, i.e. it is
    /// relative rather than absolute. This is to allow viewports to
    /// automatically resize along with the target.
	/// </remarks>
	/// <param name="camera">The camera to use for the viewport.</param>
	/// <returns>A new viewport.</returns>
	Viewport^ addViewport(Camera^ camera);

	/// <summary>
	/// Add a viewport to the rendering target.
	/// </summary>
	/// <remarks>
	/// A viewport is the rectangle into which rendering output is sent. This
    /// method adds a viewport to the render target, rendering from the supplied
    /// camera. The rest of the parameters are only required if you wish to add
    /// more than one viewport to a single rendering target. Note that size
    /// information passed to this method is passed as a parametric, i.e. it is
    /// relative rather than absolute. This is to allow viewports to
    /// automatically resize along with the target.
	/// </remarks>
	/// <param name="camera">The camera to use for the viewport.</param>
	/// <param name="zOrder">The relative order of the viewport with others on the target (allows overlapping viewports i.e. picture-in-picture). Higher ZOrders are on top of lower ones. The actual number is irrelevant, only the relative ZOrder matters (you can leave gaps in the numbering).</param>
	/// <param name="left">The relative position of the left of the viewport on the target, as a value between 0 and 1. </param>
	/// <param name="top">The relative position of the top of the viewport on the target, as a value between 0 and 1. </param>
	/// <param name="width">The relative width of the viewport on the target, as a value between 0 and 1. </param>
	/// <param name="height">The relative height of the viewport on the target, as a value between 0 and 1.</param>
	/// <returns>A new viewport.</returns>
	Viewport^ addViewport(Camera^ camera, int zOrder, float left, float top, float width, float height);

	/// <summary>
	/// This will destroy the passed viewport.
	/// </summary>
	void destroyViewport( Viewport^ viewport );

	/// <summary>
	/// Get the width of the RenderTarget.
	/// </summary>
	/// <returns>The width.</returns>
	unsigned int getWidth();

	/// <summary>
	/// Get the height of the RenderTarget.
	/// </summary>
	/// <returns>The height.</returns>
	unsigned int getHeight();

	/// <summary>
	/// Get the color depth of the RenderTarget.
	/// </summary>
	/// <returns>The color depth.</returns>
	unsigned int getColorDepth();

	/// <summary>
	/// Tells the target to update it's contents.
	/// 
    /// If OGRE is not running in an automatic rendering loop (started using
    /// Root::startRendering), the user of the library is responsible for asking
    /// each render target to refresh. This is the method used to do this. It
    /// automatically re-renders the contents of the target using whatever
    /// cameras have been pointed at it (using Camera::setRenderTarget). 
	/// 
    /// This allows OGRE to be used in multi-windowed utilities and for contents
    /// to be refreshed only when required, rather than constantly as with the
    /// automatic rendering loop. 
	/// </summary>
	void update();

	/// <summary>
	/// Tells the target to update it's contents.
	/// 
    /// If OGRE is not running in an automatic rendering loop (started using
    /// Root::startRendering), the user of the library is responsible for asking
    /// each render target to refresh. This is the method used to do this. It
    /// automatically re-renders the contents of the target using whatever
    /// cameras have been pointed at it (using Camera::setRenderTarget). 
	/// 
    /// This allows OGRE to be used in multi-windowed utilities and for contents
    /// to be refreshed only when required, rather than constantly as with the
    /// automatic rendering loop. 
	/// </summary>
	/// <param name="swapBuffers">For targets that support double-buffering, if set to true, the target will immediately swap it's buffers after update. Otherwise, the buffers are not swapped, and you have to call swapBuffers yourself sometime later. You might want to do this on some rendersystems which pause for queued rendering commands to complete before accepting swap buffers calls - so you could do other CPU tasks whilst the queued commands complete. Or, you might do this if you want custom control over your windows, such as for externally created windows.</param>
	void update(bool swapBuffers);

	/// <summary>
	/// Swaps the frame buffers to display the next frame.
	/// 
    /// For targets that are double-buffered so that no 'in-progress' versions
    /// of the scene are displayed during rendering. Once rendering has
    /// completed (to an off-screen version of the window) the buffers are
    /// swapped to display the new frame.
	/// </summary>
	void swapBuffers();

	/// <summary>
	/// Swaps the frame buffers to display the next frame.
	/// 
    /// For targets that are double-buffered so that no 'in-progress' versions
    /// of the scene are displayed during rendering. Once rendering has
    /// completed (to an off-screen version of the window) the buffers are
    /// swapped to display the new frame.
	/// </summary>
	/// <param name="waitForVsync">If true, the system waits for the next vertical blank period (when the CRT beam turns off as it travels from bottom-right to top-left at the end of the pass) before flipping. If false, flipping occurs no matter what the beam position. Waiting for a vertical blank can be slower (and limits the framerate to the monitor refresh rate) but results in a steadier image with no 'tearing' (a flicker resulting from flipping buffers when the beam is in the progress of drawing the last frame).</param>
	void swapBuffers(bool waitForVsync);

	/// <summary>
	/// Get the number of viewports attached to this target. 
	/// </summary>
	/// <returns>The number of viewports.</returns>
	unsigned short getNumViewports();

	/// <summary>
	/// Get the viewport identified by name.
	/// </summary>
	/// <param name="index">The index of the viewport to get.</param>
	/// <returns>The viewport identified by name or null if it is not found.</returns>
	Viewport^ getViewport(unsigned short index);

	//framestats getstatistics

	/// <summary>
	/// Individual stats access - gets the number of frames per second (FPS) based on the last frame rendered. 
	/// </summary>
	/// <returns>The last fps.</returns>
	float getLastFPS();

	/// <summary>
	/// Individual stats access - gets the average frames per second (FPS) since call to Root::startRendering. 
	/// </summary>
	/// <returns>The average fps.</returns>
	float getAverageFPS();

	/// <summary>
	/// Individual stats access - gets the best frames per second (FPS) since call to Root::startRendering. 
	/// </summary>
	/// <returns>The best fps.</returns>
	float getBestFPS();

	/// <summary>
	/// Individual stats access - gets the worst frames per second (FPS) since call to Root::startRendering. 
	/// </summary>
	/// <returns>The worst fps.</returns>
	float getWorstFPS();

	/// <summary>
	/// Individual stats access - gets the best frame time. 
	/// </summary>
	/// <returns>The best frame time.</returns>
	float getBestFrameTime();

	/// <summary>
	/// Individual stats access - gets the worst frame time. 
	/// </summary>
	/// <returns>The worst frame time.</returns>
	float getWorstFrameTime();

	/// <summary>
	/// Resets saved frame-rate statistices. 
	/// </summary>
	void resetStatistics();

	/// <summary>
	/// Gets a custom (maybe platform-specific) attribute.
	/// <para>
    /// This is a nasty way of satisfying any API's need to see
    /// platform-specific details. It horrid, but D3D needs this kind of info.
    /// At least it's abstracted.
	/// </para>
	/// </summary>
	/// <param name="name">The name of the attribute.</param>
	/// <param name="pData">Pointer to memory of the right kind of structure to receive the info.</param>
	void getCustomAttribute(System::String^ name, void* pData);

	//add listener

	//remove listener

	//remove all listeners

	/// <summary>
	/// Sets the priority of this render target in relation to the others.
	/// 
	/// 
    /// This can be used in order to schedule render target updates. Lower
    /// priorities will be rendered first. Note that the priority must be set at
    /// the time the render target is attached to the render system, changes
    /// afterwards will not affect the ordering. 
	/// </summary>
	/// <param name="priority">The priority to set.</param>
	void setPriority(unsigned char priority);

	/// <summary>
	/// Gets the priority of a render target. 
	/// </summary>
	/// <returns>The priority.</returns>
	unsigned char getPriority();

	/// <summary>
	/// Used to retrieve the active state of the render target. 
	/// </summary>
	/// <returns>True if the viewport is active, false if disabled.</returns>
	bool isActive();

	/// <summary>
	/// Used to set the active state of the render target. 
	/// </summary>
	/// <param name="active">True to activate false to deactivate.</param>
	void setActive(bool active);

	/// <summary>
	/// Sets whether this target should be automatically updated if Ogre's
    /// rendering loop or Root::_updateAllRenderTargets is being used.
	/// 
    /// By default, if you use Ogre's own rendering loop (Root::startRendering)
    /// or call Root::_updateAllRenderTargets, all render targets are updated
    /// automatically. This method allows you to control that behaviour, if for
    /// example you have a render target which you only want to update
    /// periodically. 
	/// </summary>
	/// <param name="autoUpdate">If true, the render target is updated during the automatic render loop or when Root::_updateAllRenderTargets is called. If false, the target is only updated when its update() method is called explicitly. </param>
	void setAutoUpdated(bool autoUpdate);

	/// <summary>
	/// Gets whether this target is automatically updated if Ogre's rendering
    /// loop or Root::_updateAllRenderTargets is being used. 
	/// </summary>
	/// <returns>True if the target is autoupdated, false if it is not.</returns>
	bool isAutoUpdated();

	/// <summary>
	/// Copies the current contents of the render target to a pixelbox.
	/// <para>
	/// See suggestPixelFormat for a tip as to the best pixel format to extract
    /// into, although you can use whatever format you like and the results will
    /// be converted. 
	/// </para>
	/// </summary>
	/// <param name="dest">The PixelBox to write the results to.</param>
	void copyContentsToMemory(PixelBox^ dest);

	/// <summary>
	/// Copies the current contents of the render target to a pixelbox.
	/// <para>
	/// See suggestPixelFormat for a tip as to the best pixel format to extract
    /// into, although you can use whatever format you like and the results will
    /// be converted. 
	/// </para>
	/// </summary>
	/// <param name="dest">The PixelBox to write the results to.</param>
	/// <param name="buffer">The frame buffer to copy the contents from.</param>
	void copyContentsToMemory(PixelBox^ dest, FrameBuffer buffer);

	/// <summary>
	/// Suggests a pixel format to use for extracting the data in this target,
    /// when calling copyContentsToMemory.
	/// </summary>
	/// <returns>The reccomended pixel format.</returns>
	PixelFormat suggestPixelFormat();

	/// <summary>
	/// Writes the current contents of the render target to the named file. 
	/// </summary>
	/// <param name="filename">The file to write the contents to.</param>
	void writeContentsToFile(System::String^ filename);

	/// <summary>
	/// Writes the current contents of the render target to the (PREFIX)(time-stamp)(SUFFIX) file. 
	/// </summary>
	/// <param name="filenamePrefix"></param>
	/// <param name="filenameSuffix"></param>
	System::String^ writeContentsToTimestampedFile(System::String^ filenamePrefix, System::String^ filenameSuffix);

	/// <summary>
	/// Determine if this render target requires texture flipping.
	/// </summary>
	/// <returns>True if texture flipping is required, false if not.</returns>
	bool requiresTextureFlipping();

	/// <summary>
	/// Gets the number of triangles rendered in the last update() call. 
	/// </summary>
	/// <returns>The number of triangles.</returns>
	size_t getTriangleCount();

	/// <summary>
	/// Gets the number of batches rendered in the last update() call. 
	/// </summary>
	/// <returns>The number of batches.</returns>
	size_t getBatchCount();

	/// <summary>
	/// Indicates whether this target is the primary window.
	/// 
	/// The primary window is special in that it is destroyed when ogre is shut
    /// down, and cannot be destroyed directly. This is the case because it
    /// holds the context for vertex, index buffers and textures. 
	/// </summary>
	/// <returns>True if this is the primary target.</returns>
	bool isPrimary();

	/// <summary>
	/// Indicates whether on rendering, linear colour space is converted to sRGB
    /// gamma colour space.
	/// 
	/// This is the exact opposite conversion of what is indicated by
    /// Texture::isHardwareGammaEnabled, and can only be enabled on creation of
    /// the render target. For render windows, it's enabled through the 'gamma'
    /// creation misc parameter. For textures, it is enabled through the hwGamma
    /// parameter to the create call. 
	/// </summary>
	/// <returns>True if enabled.</returns>
	bool isHardwareGammaEnabled();

	/// <summary>
	/// Indicates whether multisampling is performed on rendering and at what level. 
	/// </summary>
	/// <returns>The level of FSAA.</returns>
	unsigned int getFSAA();
};

}