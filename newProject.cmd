@echo off
setlocal enabledelayedexpansion
pushd %~dp0

powershell -NoLogo -NoProfile -File ./newProject.ps1 %1 %2
popd

pause & exit /b