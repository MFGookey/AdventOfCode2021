@echo off
setlocal enabledelayedexpansion
pushd %~dp0

powershell -File ./install.ps1
popd

pause & exit /b