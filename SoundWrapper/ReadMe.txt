--------------------------------------------------------------------------------------------------------------------
From http://forums.indiegamer.com/archive/index.php?t-1031.html

I just wanted to share my experience using OpenAL to spare fellow coders from some unwanted anguish and despair. Part of this knowledge was gained by trial and error due to a general lack of information on OpenAL.

OpenAL is quite a decent sound API with a very neat function set in the style of OpenGL. However, a lot of care must be taken to get it to work right, especially when dealing with sound lifecycle management and 3d sound functionality. I'm assuming you are at least slightly familiar with OpenAL if not proclaimed experts.

Do's:

Allocate all buffers and sources before playing any sound. Bind / unbind sources to buffers on an as-per-needed basis rather than create sources on the fly.

Be very conservative with sound sources. In the Windows implementation (based on Direct3D), the limit is usually that of your sound hardware or slightly less, that is, around 28-32 sound buffers.

Use mono samples if you want 3D sound functionality. Stereo samples are played uniformly across all your speakers.

Not sure about this.. but apparently it is better to deallocate buffers and sources in reverse order. That is if you allocate buffers 1 to 10 and sources 1 ro 5, at the end of your code deallocate sources 5 to 1 and buffers 10 to 1 in that order.

Don'ts:

Attempt to create a source after an existing source has played. The source will not be created for some inexplicable reason.

Loose track of source ID's. If your code forgets about them, OpenAL won't.. and it will come back to bite you when you uwittingly exceed the source limit.

Use stereo samples unless 3D sound is not required (such as for music)

--------------------------------------------------------------------------------------------------------------------

========================================================================
    DYNAMIC LINK LIBRARY : SoundWrapper Project Overview
========================================================================

AppWizard has created this SoundWrapper DLL for you.  

This file contains a summary of what you will find in each of the files that
make up your SoundWrapper application.

SoundWrapper.vcproj
    This is the main project file for VC++ projects generated using an Application Wizard. 
    It contains information about the version of Visual C++ that generated the file, and 
    information about the platforms, configurations, and project features selected with the
    Application Wizard.

SoundWrapper.cpp
    This is the main DLL source file.

SoundWrapper.h
    This file contains a class declaration.

AssemblyInfo.cpp
	Contains custom attributes for modifying assembly metadata.

/////////////////////////////////////////////////////////////////////////////
Other notes:

AppWizard uses "TODO:" to indicate parts of the source code you
should add to or customize.

/////////////////////////////////////////////////////////////////////////////
