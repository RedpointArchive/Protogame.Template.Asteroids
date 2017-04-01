@echo off
cd %~dp0
..\Protobuild.exe --execute ProtogameAssetTool -o compiled -p Windows -p MacOSX -p Linux
pause