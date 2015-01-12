::Configuration Settings
set SolutionName=Engine.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootBuildFolder=%ThisFolder%..\
set BuildCommand="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" /m
set CurrentDirectory=%CD%
set InnosetupCommand="C:\Program Files (x86)\Inno Setup 5\Compil32.exe" /cc

set SolutionPath=%ThisFolder%%SolutionName%

%BuildCommand% "%SolutionPath%" /property:Configuration=Debug;Platform="Any CPU" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Debug;Platform="Any CPU"

%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="Any CPU" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="Any CPU"

%BuildCommand% "%SolutionPath%" /property:Configuration=Debug;Platform="x64" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Debug;Platform="x64"

%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="x64" /target:Clean
%BuildCommand% "%SolutionPath%" /property:Configuration=Release;Platform="x64"

call %RootBuildFolder%DRM\CodeKey\SignEditor.bat

%InnosetupCommand% %ThisFolder%\Installer\AnomalyEditor.iss