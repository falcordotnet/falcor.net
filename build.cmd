@echo off
powershell -Command "if(!(Test-Path ./build/tools/nuget/nuget.exe)) { Invoke-WebRequest https://nuget.org/nuget.exe -OutFile ./build/tools/nuget/nuget.exe }"
"build\tools\nuget\nuget.exe" "install" "xunit.runner.console" "-OutputDirectory" "./build/packages" "-ExcludeVersion" "-version" "2.0.0"
"build\tools\nuget\nuget.exe" "install" "FAKE.Core" "-OutputDirectory" "./build/packages" "-ExcludeVersion" "-version" "4.4.2"
"build\tools\nuget\nuget.exe" "install" "FSharp.Data" "-OutputDirectory" "./build/packages" "-ExcludeVersion" "-version" "2.2.5"

:Build

SET TARGET="Default"

IF NOT [%1]==[] (set TARGET="%1")

SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")

"build\packages\FAKE.Core\tools\Fake.exe" "build\build.fsx" "target=%TARGET%" 

rem Bail if we're running a AppVeyor build.
if /i "%BuildRunner%"=="AppVeyor" goto Quit

:Quit
exit /b %errorlevel%