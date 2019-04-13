::Configuration Settings
set SolutionName=EngineAndroidNativeOnly.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootBuildFolder=%ThisFolder%..\
set BuildCommand="msbuild.exe" /m
set CurrentDirectory=%CD%
set OutputFolder=%ThisFolder%Release\
set SolutionPath=%ThisFolder%%SolutionName%

%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="arm" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="arm"