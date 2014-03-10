@echo off

set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
set destPath=D:\Work\Git\Publish\LAS\

:: clear existing publish folder
DEL /F /Q "publish.log"
RD /S /Q "%destPath%"

call %msBuildDir%\msbuild.exe LicenseeAgent.Site\LicenseeAgent.Site.csproj "/p:Platform=AnyCPU;Configuration=Release;PublishDestination=%destPath%;AutoParameterizationWebConfigConnectionStrings=False" /t:PublishToFileSystem /l:FileLogger,Microsoft.Build.Engine;logfile=publish.log

:: call %msBuildDir%\msbuild.exe LicenseeAgent.Site\LicenseeAgent.Site.csproj /t:Package "/p:Platform=AnyCPU;Configuration=Release;PackageAsSingleFile=True;AutoParameterizationWebConfigConnectionStrings=False;PackageLocation=D:\Package.zip"

set msBuildDir=
set destPath=

start notepad publish.log