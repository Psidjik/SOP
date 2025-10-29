@echo off
set /p migration=Enter Migration Name:
if "%migration%"=="" (
	echo You didn't enter migration name!
)

cd ../Gateway

if "%migration%"=="-" (
	echo Removing last migration!
	dotnet ef migrations remove
) else (
	dotnet ef migrations add %migration%
)

if %errorlevel% == 0 (
	timeout 5
) else (
	pause
)
