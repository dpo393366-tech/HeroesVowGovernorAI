@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Setup.ps1" -Action remove
if errorlevel 1 echo Operation failed. Read the message above.
pause
