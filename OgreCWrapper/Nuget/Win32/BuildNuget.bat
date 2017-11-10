pushd %~dp0

mkdir contentFiles\any\any\x86
mkdir contentFiles\any\any\x64

copy ..\..\..\..\Dependencies\Ogre\WindowsBuild\bin\release\OgreMain.dll                contentFiles\any\any\x86\OgreMain.dll
copy ..\..\..\..\Dependencies\Ogre\WindowsBuild\bin\release\RenderSystem_Direct3D11.dll contentFiles\any\any\x86\RenderSystem_Direct3D11.dll
copy ..\..\..\..\Dependencies\Ogre\WindowsBuild\bin\release\RenderSystem_GL.dll         contentFiles\any\any\x86\RenderSystem_GL.dll
copy ..\..\..\..\Dependencies\Ogre\WindowsBuild\bin\release\RenderSystem_GLES2.dll      contentFiles\any\any\x86\RenderSystem_GLES2.dll
copy ..\..\..\Release\x86\OgreCWrapper.dll                                              contentFiles\any\any\x86\OgreCWrapper.dll

copy ..\..\..\..\Dependencies\Ogre\Win64Build\bin\release\OgreMain.dll                  contentFiles\any\any\x64\OgreMain.dll
copy ..\..\..\..\Dependencies\Ogre\Win64Build\bin\release\RenderSystem_Direct3D11.dll   contentFiles\any\any\x64\RenderSystem_Direct3D11.dll
copy ..\..\..\..\Dependencies\Ogre\Win64Build\bin\release\RenderSystem_GL.dll           contentFiles\any\any\x64\RenderSystem_GL.dll
copy ..\..\..\Release\x64\OgreCWrapper.dll                                              contentFiles\any\any\x64\OgreCWrapper.dll

nuget pack
popd