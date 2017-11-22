set BASE=%~dp0
set SEARCH=%BASE%
set OUT=%BASE%nuget
rmdir %OUT%
mkdir %OUT%
pushd %SEARCH%
FOR /R %BASE% %%I in (*BuildNuget.bat) DO call %%I 
popd