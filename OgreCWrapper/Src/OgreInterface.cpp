#include "Stdafx.h"
#include "OgrePlugin.h"
#include "OgreScriptCompiler.h"

#ifdef APPLE_IOS
#include "OgreGLES2Plugin.h"
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

extern "C" _AnomalousExport Ogre::Plugin* OgreInterface_LoadRenderSystem(RenderSystemType rendersystemType)
{
    Ogre::Plugin* plugin = NULL;
    
#if defined(WINDOWS) || defined(WINRT)
	String defaultRenderSystem = "RenderSystem_Direct3D11";
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "/@macBundlePath/../../Frameworks/RenderSystem_GL.framework";
#endif
    
#ifdef APPLE_IOS
    String defaultRenderSystem = "RenderSystem_GLES2";
#endif

	Ogre::String name;
	switch(rendersystemType)
	{
		case Default:
			name = defaultRenderSystem;
			break;
		case D3D11:
			name = "RenderSystem_Direct3D11";
			break;
		case OpenGL:
			name = "RenderSystem_GL";
			break;
        case OpenGLES2:
            name = "RenderSystem_GLES2";
            break;
	}

#ifdef OGRE_STATIC_LIB
#ifdef APPLE_IOS
    Ogre::GLES2Plugin* gles2Plugin = new Ogre::GLES2Plugin(); //Will be delete by the managed code when this is returned.
    Ogre::Root::getSingleton().installPlugin(gles2Plugin);
    plugin = gles2Plugin;
#endif
#else
#if _DEBUG
    name = (std::string(name) + "_d").c_str();
#endif
    
	try
	{
		Ogre::Root::getSingleton().loadPlugin(name);
	}
	catch(Ogre::Exception& ex)
	{
		Ogre::Root::getSingleton().loadPlugin(defaultRenderSystem);
	}
    
#endif

	return plugin;
}

extern "C" _AnomalousExport void OgreInterface_UnloadRenderSystem(Ogre::Plugin* renderSystemPlugin)
{
	if(renderSystemPlugin != NULL)
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
    
#ifdef APPLE_IOS
    String defaultRenderSystem = "OpenGL ES 2.x Rendering Subsystem";
    RenderSystemType defaultRendersystemType = OpenGLES2; //Change this to OpenGLES later.
#endif

	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::String name;
	switch(rendersystemType)
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
	if(rs == NULL)
	{
		Ogre::LogManager::getSingletonPtr()->getDefaultLog()->stream() << "Could not find Render System '" << name << "' using default of '" << defaultRenderSystem << "' instead.";
		rs = root->getRenderSystemByName(defaultRenderSystem);
		rendersystemType = defaultRendersystemType;
	}

	//Determine any custom settings needed all the time, rendersystemType here
	//will be an actual rendersystem and not the default as we will have determined
	//what it is by this point.
	//Disabled for now, but this is where you do it.
	switch(rendersystemType)
	{
		case D3D11:
			rs->setConfigOption("Min Requested Feature Levels", "10.0");
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
public:
	AnomalousScriptCompilerListener(String replacementExtension)
		:replacementExtension(replacementExtension)
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
					i->second = i->second.replace(dotIndex, 3, replacementExtension);
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

extern "C" _AnomalousExport void OgreInterface_SetupVaryingCompressedTextures()
{
	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::RenderSystem* rs = root->getRenderSystem();
	const Ogre::RenderSystemCapabilities* capabilities = rs->getCapabilities();
	if (capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_DXT))
	{
        Ogre::LogManager::getSingleton().logMessage("Using DDS Texture Compression");
	}
	else if (capabilities->hasCapability(Ogre::RSC_TEXTURE_COMPRESSION_PVRTC))
	{
        Ogre::LogManager::getSingleton().logMessage("Using PVRTC Texture Compression");
		Ogre::ScriptCompilerManager::getSingleton().setListener(new AnomalousNormalMapScriptCompilerListener("pvr", "png"));
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