pushd %~dp0
dotnet pack --include-symbols --configuration Release
popd