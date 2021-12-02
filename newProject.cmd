@echo off
setlocal enabledelayedexpansion
pushd %~dp0

powershell -File ./newProject.ps1 %1 %2
popd

pause & exit /b