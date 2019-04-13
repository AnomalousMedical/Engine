::Configuration Settings
set SolutionName=EngineWindowsNativeOnly.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootBuildFolder=%ThisFolder%..\
set CurrentDirectory=%CD%
set OutputFolder=%ThisFolder%Release\
set SolutionPath=%ThisFolder%%SolutionName%

msbuild /m "%SolutionPath%" /property:Configuration=Debug;Platform="x86" /target:Clean,Build

msbuild /m "%SolutionPath%" /property:Configuration=Release;Platform="x86" /target:Clean,Build

msbuild /m "%SolutionPath%" /property:Configuration=Debug;Platform="x64" /target:Clean,Build

msbuild /m "%SolutionPath%" /property:Configuration=Release;Platform="x64" /target:Clean,Build