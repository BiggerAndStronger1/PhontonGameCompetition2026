@echo off
cd /d "%~dp0"

:: Stage changes
git pull
pause
git add Assets/ Packages/ ProjectSettings/ .gitattributes "git quick command.bat" git_lfs_solution.bat ^
README.md .gitignore

echo Files added to staging.

:: Prompt for commit message
set /p "commitMsg=Enter commit message: "
git commit -m "%commitMsg%"

:: Push to the current branch
git push

pause


