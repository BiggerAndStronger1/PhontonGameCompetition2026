@echo off
setlocal enabledelayedexpansion

REM Get repo root (assumes script is run from repo root)
set "REPO_ROOT=%cd%"

REM Prompt for file or folder
set /p TARGET=Enter the file or folder name/path to track with Git LFS: 

REM Remove surrounding quotes if present
set "TARGET=%TARGET:"=%"

REM Ask if it's a folder
set /p ISFOLDER=Is "%TARGET%" a folder? (y/n): 

REM Validate path exists
if not exist "%TARGET%" (
    echo ERROR: The specified path does not exist.
    pause
    exit /b
)

if /I "%ISFOLDER%"=="y" (
    REM Loop through all files under the folder
    for /r "%TARGET%" %%F in (*) do (
        REM Get full path and convert to relative
        set "FULL=%%~fF"
        call set "REL=%%FULL:%REPO_ROOT%\=%%"
        git lfs track "!REL!"
    )
    git add .gitattributes
    git add "%TARGET%"
    git commit -m "Track all files under %TARGET% with Git LFS"
) else (
    REM Get full path and convert to relative
    for %%I in ("%TARGET%") do (
        set "FULL=%%~fI"
        call set "REL=%%FULL:%REPO_ROOT%\=%%"
    )
    git lfs track "!REL!"
    git add .gitattributes
    git add "!REL!"
    git commit -m "Track !REL! with Git LFS"
)

git push
pause
