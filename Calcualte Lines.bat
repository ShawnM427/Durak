@echo off
SET PWD="%cd%"
Powershell.exe -executionpolicy remotesigned -File  "%PWD%/line_count.ps1"