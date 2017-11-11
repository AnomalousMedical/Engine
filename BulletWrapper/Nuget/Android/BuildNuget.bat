pushd %~dp0

mkdir package\contentFiles\any\any\lib\armeabi-v7a

dotnet pack --configuration Release
popd