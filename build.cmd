@echo off
powershell -Command "if(!(Test-Path ./temp/tools/nuget/nuget.exe)) { Invoke-WebRequest https://nuget.org/nuget.exe -OutFile ./temp/tools/nuget/nuget.exe }"
"temp\tools\nuget\nuget.exe" "install" "xunit.runner.console" "-OutputDirectory" "./temp/tools" "-ExcludeVersion" "-version" "2.0.0"
"temp\tools\nuget\nuget.exe" "install" "FAKE.Core" "-OutputDirectory" "./temp/tools" "-ExcludeVersion" "-version" "4.4.2"
"temp\tools\nuget\nuget.exe" "install" "FSharp.Data" "-OutputDirectory" "./temp/tools" "-ExcludeVersion" "-version" "2.2.5"

:Build

SET TARGET="Default"

IF NOT [%1]==[] (set TARGET="%1")

SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")

"temp\tools\FAKE.Core\tools\Fake.exe" "build.fsx" "target=%TARGET%" 

rem Bail if we're running a AppVeyor build.
if /i "%BuildRunner%"=="AppVeyor" goto Quit

:Quit
exit /b %errorlevel%