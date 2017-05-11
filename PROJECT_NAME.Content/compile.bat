@echo off
cd %~dp0\assets
..\..\Protobuild.exe --execute ProtogameAssetTool -o ..\compiled -p Windows -p MacOSX -p Linux
pause