pushd %~dp0

mkdir contentFiles\any\any\x86
mkdir contentFiles\any\any\x64

copy ..\..\..\Release\x86\BulletWrapper.dll contentFiles\any\any\x86\BulletWrapper.dll
copy ..\..\..\Release\x64\BulletWrapper.dll contentFiles\any\any\x64\BulletWrapper.dll

dotnet pack --configuration Release
REM move *.nupkg  ..\..\..\Release

popd