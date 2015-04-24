::Configuration Settings
set SolutionName=EngineAndroid.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootBuildFolder=%ThisFolder%..\
set BuildCommand="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" /m
set CurrentDirectory=%CD%
set InnosetupCommand="C:\Program Files (x86)\Inno Setup 5\Compil32.exe" /cc
set SignCommand=call "%RootBuildFolder%DRM\CodeKey\AnomalousMedicalSign.bat"
set OutputFolder=%ThisFolder%Release\
set SolutionPath=%ThisFolder%%SolutionName%

%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="arm" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="arm"

%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="x86-Android-NVIDIA" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="x86-Android-NVIDIA"