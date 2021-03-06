::Configuration Settings
set SolutionName=Engine.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootBuildFolder=%ThisFolder%..\
set CurrentDirectory=%CD%
set InnosetupCommand="C:\Program Files (x86)\Inno Setup 5\Compil32.exe" /cc
set SignCommand=call "%RootBuildFolder%DRM\CodeKey\AnomalousMedicalSign.bat"
set OutputFolder=%ThisFolder%Release\
set SolutionPath=%ThisFolder%%SolutionName%

msbuild.exe /m "%SolutionPath%" /property:Configuration=Debug;Platform="x86" /target:Clean,Build

msbuild.exe /m "%SolutionPath%" /property:Configuration=Release;Platform="x86" /target:Clean,Build

msbuild.exe /m "%SolutionPath%" /property:Configuration=Debug;Platform="x64" /target:Clean,Build

msbuild.exe /m "%SolutionPath%" /property:Configuration=Release;Platform="x64" /target:Clean,Build

%SignCommand% "Anomaly" "%OutputFolder%\Anomaly.exe"
%SignCommand% "Image Atlas Packer" "%OutputFolder%\ImageAtlasPacker.exe"
%SignCommand% "Ogre Model Editor" "%OutputFolder%\OgreModelEditor.exe"

%InnosetupCommand% %ThisFolder%\Installer\AnomalyEditor.iss