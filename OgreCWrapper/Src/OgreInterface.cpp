#include "Stdafx.h"
#include "OgrePlugin.h"
#include "OgreScriptCompiler.h"

#ifdef OGRE_STATIC_LIB
#if defined(APPLE_IOS) || defined(ANDROID)
#include "OgreGLES2Plugin.h"
#endif
#endif

//This is not actually part of ogre, but is a way for the OgreInterface class to
//get its rendersystem info

enum RenderSystemType
{
	Default = 0,
	D3D11 = 1,
	OpenGL = 2,
	OpenGLES2 = 3
};

enum CompressedTextureSupport
{
	None = 0,
	DXT = 1,
	PVRTC = 1 << 1,
	ATC = 1 << 2,
	ETC2 = 1 << 3,
	All = DXT | PVRTC | ATC | ETC2
};

#if defined(WINDOWS) || defined(WINRT)
#if _DEBUG
String Direct3D11_Library = "RenderSystem_Direct3D11_d.dll";
String OpenGL_Library = "RenderSystem_GL_d.dll";
String OpenGLES2_Library = "RenderSystem_GLES2_d.dll";
#else
String Direct3D11_Library = "RenderSystem_Direct3D11.dll";
String OpenGL_Library = "RenderSystem_GL.dll";
String OpenGLES2_Library = "RenderSystem_GLES2.dll";
#endif
#endif

extern "C" _AnomalousExport Ogre::Plugin* OgreInterface_LoadRenderSystem(RenderSystemType rendersystemType)
{
	Ogre::Plugin* plugin = NULL; //We always return a plugin even if it is null, plugins are only loaded directly when built statically

#ifdef OGRE_STATIC_LIB
#if defined(APPLE_IOS) || defined(ANDROID)
	Ogre::GLES2Plugin* gles2Plugin = new Ogre::GLES2Plugin(); //Will be delete by the managed code when this is returned.
	Ogre::Root::getSingleton().installPlugin(gles2Plugin);
	plugin = gles2Plugin;
#endif
#else

	Ogre::String name;
#ifdef MAC_OSX
	name = "/@macBundlePath/../../Frameworks/RenderSystem_GL.framework";
    Ogre::String defaultRenderSystem = name;
#endif

#ifdef ANDROID
	name = "libRenderSystem_GLES2.so.1.10.0.so";
	Ogre::String defaultRenderSystem = name;
#endif

#if defined(WINDOWS) || defined(WINRT)
	//Only windows allows switching rendering systems
	String defaultRenderSystem = Direct3D11_Library;
	switch (rendersystemType)
	{
	case Default:
		name = defaultRenderSystem;
		break;
	case D3D11:
		name = Direct3D11_Library;
		break;
	case OpenGL:
		name = OpenGL_Library;
		break;
	case OpenGLES2:
		name = OpenGLES2_Library;
		break;
	}
#endif

	try
	{
		Ogre::Root::getSingleton().loadPlugin(name);
	}
	catch (Ogre::Exception& ex)
	{
		Ogre::Root::getSingleton().loadPlugin(defaultRenderSystem);
	}

#endif

	return plugin;
}

extern "C" _AnomalousExport void OgreInterface_UnloadRenderSystem(Ogre::Plugin* renderSystemPlugin)
{
	if (renderSystemPlugin != NULL)
	{
		delete renderSystemPlugin;
	}
}

extern "C" _AnomalousExport Ogre::RenderSystem* OgreInterface_GetRenderSystem(RenderSystemType rendersystemType)
{
#if defined(WINDOWS) || defined(WINRT)
	String defaultRenderSystem = "Direct3D11 Rendering Subsystem";
	RenderSystemType defaultRendersystemType = D3D11;
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "OpenGL Rendering Subsystem";
	RenderSystemType defaultRendersystemType = OpenGL;
#endif

#if defined(APPLE_IOS) || defined(ANDROID)
	String defaultRenderSystem = "OpenGL ES 2.x Rendering Subsystem";
	RenderSystemType defaultRendersystemType = OpenGLES2;
#endif

	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::String name;
	switch (rendersystemType)
	{
	case Default:
		name = defaultRenderSystem;
		rendersystemType = defaultRendersystemType;
		break;
	case D3D11:
		name = "Direct3D11 Rendering Subsystem";
		break;
	case OpenGL:
		name = "OpenGL Rendering Subsystem";
		break;
	case OpenGLES2:
		name = "OpenGL ES 2.x Rendering Subsystem";
		break;
	}

	Ogre::RenderSystem* rs = root->getRenderSystemByName(name);
	if (rs == NULL)
	{
		Ogre::LogManager::getSingletonPtr()->getDefaultLog()->stream() << "Could not find Render System '" << name << "' using default of '" << defaultRenderSystem << "' instead.";
		rs = root->getRenderSystemByName(defaultRenderSystem);
		rendersystemType = defaultRendersystemType;
	}

	//Determine any custom settings needed all the time, rendersystemType here
	//will be an actual rendersystem and not the default as we will have determined
	//what it is by this point.
	//Disabled for now, but this is where you do it.
	switch (rendersystemType)
	{
	case D3D11:
		rs->setConfigOption("Min Requested Feature Levels", "10.0");
#ifdef _DEBUG
		rs->setConfigOption("Information Queue Exceptions Bottom Level", "Corruption");
#endif
		//rs->setConfigOption("Max Requested Feature Levels", "11.1");
		break;
	}

	return rs;
}

/*
This script compiler listener allows us to easily vary the textures used in our materials
depending on what types of compressed textures a platform supports. DDS is the assumed default
so in that case no listener is applied. As a result all materials should be setup using dds extensions.
If dds is not supported then the following two other options are available:
PVRTC - will use png for all texture units named NormalMap and pvr for all others
No compressed formats supported - will use png for everything
This is automatically setup by the OgreInterface class, so it does not need any action on the part
of the user.
*/

class AnomalousScriptCompilerListener : public Ogre::ScriptCompilerListener
{
private:
	String replacementExtension;
	size_t replacementLength;
public:
	AnomalousScriptCompilerListener(String replacementExtension)
		:replacementExtension(replacementExtension),
		replacementLength(strlen(replacementExtension))
	{

	}

	virtual ~AnomalousScriptCompilerListener()
	{

	}

	virtual bool handleEvent(Ogre::ScriptCompiler *compiler, Ogre::ScriptCompilerEvent *evt, void *retval)
	{
		if (evt->mType == Ogre::PreApplyTextureAliasesScriptCompilerEvent::eventType)
		{
			Ogre::PreApplyTextureAliasesScriptCompilerEvent* paEvt = static_cast<Ogre::PreApplyTextureAliasesScriptCompilerEvent*>(evt);
			Ogre::AliasTextureNamePairList* aliases = paEvt->mAliases;
			for (Ogre::AliasTextureNamePairList::iterator i = aliases->begin(); i != aliases->end(); i++)
			{
				size_t dotIndex = i->second.find_last_of(".") + 1;
				if (i->second.substr(dotIndex) == "dds")
				{
					i->second = i->second.replace(dotIndex, replacementLength, replacementExtension);
				}
			}

			return true;
		}
		return false;
	}
};

class AnomalousNormalMapScriptCompilerListener : public Ogre::ScriptCompilerListener
{
private:
	String replacementExtension;
	String normalMapReplacement;
public:
	AnomalousNormalMapScriptCompilerListener(String replacementExtension, String normalMapReplacement)
		:replacementExtension(replacementExtension),
		normalMapReplacement(normalMapReplacement)
	{

	}

	virtual ~AnomalousNormalMapScriptCompilerListener()
	{

	}

	virtual bool handleEvent(Ogre::ScriptCompiler *compiler, Ogre::ScriptCompilerEvent *evt, void *retval)
	{
		if (evt->mType == Ogre::PreApplyTextureAliasesScriptCompilerEvent::eventType)
		{
			Ogre::PreApplyTextureAliasesScriptCompilerEvent* paEvt = static_cast<Ogre::PreApplyTextureAliasesScriptCompilerEvent*>(evt);
			Ogre::AliasTextureNamePairList* aliases = paEvt->mAliases;
			for (Ogre::AliasTextureNamePairList::iterator i = aliases->begin(); i != aliases->end(); i++)
			{
				size_t dotIndex = i->second.find_last_of(".") + 1;
				if (i->second.substr(dotIndex) == "dds")
				{
					if (i->first == "NormalMap")
					{
						i->second = i->second.replace(dotIndex, 3, normalMapReplacement);
					}
					else
					{
						i->second = i->second.replace(dotIndex, 3, replacementExtension);
					}
				}
			}

			return true;
		}
		return false;
	}
};

extern "C" _AnomalousExport void OgreInterface_SetupVaryingCompressedTextures(CompressedTextureSupport compressedTextures)
{
	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::RenderSystem* rs = root->getRenderSystem();
	const Ogre::RenderSystemCapabilities* capabilities = rs->getCapabilities();

	Ogre::LogManager::getSingleton().stream() << "Figuring out texture compression for supported formats " << compressedTextures;

	if ((compressedTextures & DXT) == DXT && capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_DXT))
	{
		Ogre::LogManager::getSingleton().logMessage("Using DDS Texture Compression");
	}
	else if ((compressedTextures & PVRTC) == PVRTC && capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_PVRTC))
	{
		Ogre::LogManager::getSingleton().logMessage("Using PVRTC Texture Compression");
		Ogre::ScriptCompilerManager::getSingleton().setListener(new AnomalousNormalMapScriptCompilerListener("pvr", "png"));
	}
	else if ((compressedTextures & ATC) == ATC && capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_ATC))
	{
		Ogre::LogManager::getSingleton().logMessage("Using ATC Texture Compression");
		Ogre::ScriptCompilerManager::getSingleton().setListener(new AnomalousScriptCompilerListener("atc.ktx"));
	}
	else if ((compressedTextures & ETC2) == ETC2 && capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_ETC2))
	{
		Ogre::LogManager::getSingleton().logMessage("Using ETC2 Texture Compression");
		Ogre::ScriptCompilerManager::getSingleton().setListener(new AnomalousScriptCompilerListener("etc2.ktx"));
	}
	else
	{
		Ogre::LogManager::getSingleton().logMessage("Using uncompressed textures");
		Ogre::ScriptCompilerManager::getSingleton().setListener(new AnomalousScriptCompilerListener("png"));
	}
}

extern "C" _AnomalousExport void OgreInterface_DestroyVaryingCompressedTextures()
{
	Ogre::ScriptCompilerListener* scriptCompilerListener = Ogre::ScriptCompilerManager::getSingleton().getListener();
	if (scriptCompilerListener != NULL)
	{
		Ogre::ScriptCompilerManager::getSingleton().setListener(NULL);
		delete scriptCompilerListener;
	}
}